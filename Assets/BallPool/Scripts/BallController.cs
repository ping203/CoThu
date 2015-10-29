using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallController : MonoBehaviour 
{
	public bool isMain = false;
	public bool isBlack = false;
	public int ballType = 1;
	public int id = 0;
	public Transform ballReflaction;
	[System.NonSerialized]
	public CueController cueController;
	[System.NonSerialized]
	public bool ballIsSelected = false;
	[System.NonSerialized]
	public bool ballIsOut = false;
	[System.NonSerialized]
	public bool inMove = false;
	
	[System.NonSerialized]
	public float step = 0.0f;
	[System.NonSerialized]
	public float speed = 0.0f;

	private float velocityNormalized = 0.0f;

	[System.NonSerialized]
	public AnimationSpline holeSpline;
	[System.NonSerialized]
	public HolleController holleController;
	[System.NonSerialized]
	public float holeSplineLungth = 0.0f;
	private Vector3 ballVeolociyInHole;
	private Vector3 checkVelocity = Vector3.zero;
	private Vector3 checkAngularVelocity = Vector3.zero;
	
	private BallController inBall = null;
	
	private bool firstHitIsChecked = false;
	
	private bool hasFirstHit = false;
	private bool haveCollision = false;
	[System.NonSerialized]
	public bool inForceMove = false;
	private Rigidbody body;
	
	void Awake ()
	{
		body = GetComponent<Rigidbody> ();

		body.collisionDetectionMode = CollisionDetectionMode.Discrete;
		body.interpolation = RigidbodyInterpolation.Interpolate;
	}
	public void ForceSetMove (Vector3 position, Vector3 velocity, Vector3 angularVelocity)
	{
		body.position = position;
		if(!body.isKinematic)
		{
			body.velocity = velocity;
			body.angularVelocity = angularVelocity;
		}
	}
	public void OnStart ()
	{
		if(MenuControllerGenerator.controller)
		{
			if(isMain)
			{
				InvokeRepeating("CheckInBall", 0.0f, 1.0f);
			}
		}

		body.maxDepenetrationVelocity = cueController.ballMaxVelocity;
		body.maxAngularVelocity = 250.0f;
		body.GetComponent<SphereCollider>().contactOffset = 0.01f;
	}
	void OnDestroy ()
	{
		CancelInvoke( "CheckInBall");
	}
	void CheckInBall ()
	{

		if(body && !cueController.allIsSleeping)
		{
			inBall = null;
			foreach (BallController item in cueController.ballControllers)
			{
				if(item != this)
				{
					float inBallDistance = Vector3.Distance(transform.position, item.transform.position);
					if(inBallDistance < 1.99f*cueController.ballRadius)
					{
						inBall = item;
						if(inBall)
						{
							Vector3 normal = (transform.position - inBall.transform.position).normalized;
							float dist = 2.0f*cueController.ballRadius - inBallDistance;

							body.position += 0.5f*dist*normal;
							inBall.body.position += -0.5f*dist*normal;
						}
						break;
					}
				}
			}

		}
	}
	public bool IsSleeping ()
	{
		//return rigidbody.IsSleeping();
		return GetComponent<Rigidbody> ().velocity.magnitude < Physics.sleepThreshold && GetComponent<Rigidbody> ().angularVelocity.magnitude * cueController.ballRadius < Physics.sleepThreshold;
	}
	public void OnSetHoleSpline (float lenght, int holleId)
	{
		HolleController holleController = HolleController.FindeHoleById(holleId);
		holeSpline = holleController.ballSpline;
		holeSplineLungth = lenght;
		this.holleController = holleController;
		if(!isMain && cueController.ballController.ballIsOut && (this.holleController == cueController.ballController.holleController || this.holleController.haveNeighbors(cueController.ballController.holleController)))
		{
			cueController.ballController.holeSplineLungth = holeSplineLungth - 2.0f*cueController.ballRadius;
			if(cueController.ballController.step >= cueController.ballController.holeSplineLungth)
			{
				cueController.ballController.body.position = cueController.ballController.holeSpline.Evaluate(holeSplineLungth);
			}
		}
	}

	public void RessetPosition (Vector3 position, bool forceResset)
	{
		Vector3 newStrPosition = position;

		if(!forceResset)
		{
			ballIsOut = false;
			cueController.ballIsOut = false;

			body.useGravity = false;
			body.isKinematic = true;
			body.GetComponent<Collider>().enabled = false;
	

			Ray ray = new Ray(position + (5.0f*cueController.ballRadius)*Vector3.up, -Vector3.up);
			RaycastHit hit;
			int tryCunt = 0;
			while(tryCunt < 7 && Physics.SphereCast(ray, (1.05f*cueController.ballRadius), out hit, 10.0f*cueController.ballRadius, cueController.ballMask) )
			{
				tryCunt ++;
				ray = new Ray(newStrPosition + (2.5f*cueController.ballRadius)*Vector3.up, -Vector3.up);
				newStrPosition += (3.0f*cueController.ballRadius)*Vector3.right;
			}
			cueController.cueFSMController.setMoveInTable();
		}

		body.position = newStrPosition;

		body.isKinematic = false;
		body.GetComponent<Collider>().enabled = true;
		body.useGravity = true;

		body.velocity = Vector3.zero;
		body.angularVelocity = Vector3.zero;
	}

	public void ShotBall ()
	{
		haveCollision = false;
		if(ServerController.serverController)
		{
		    cueController.gameManager.StopCalculateShotTime();
		}
		cueController.OnPlayCueSound(cueController.ballShotVelocity.magnitude/cueController.ballMaxVelocity);
		if(!body.isKinematic)
		{
			body.velocity = Vector3.ClampMagnitude( cueController.ballShotVelocity, cueController.ballMaxVelocity);
			body.angularVelocity = cueController.ballShotAngularVelocity;
		}
		checkVelocity = body.velocity;
		checkAngularVelocity = cueController.ballShotAngularVelocity;
		body.AddTorque( body.mass*cueController.ballShotAngularVelocity, ForceMode.Impulse);
		firstHitIsChecked = false;


		if(ServerController.serverController)
		{
			if(ServerController.serverController.isMyQueue && !MenuControllerGenerator.controller.playWithAI)
			{
				SetBallCollisionData ();
			}
		}
		cueController.cueBallPivotLocalPosition = Vector3.zero;
		cueController.cueRotationLocalPosition = Vector3.zero;

		if(!ServerController.serverController || ServerController.serverController.isMyQueue || MenuControllerGenerator.controller.playWithAI)
		{
			StartCoroutine(StartMove ());
		}

	}

	IEnumerator StartMove ()
	{
		yield return new WaitForFixedUpdate();
		while(!IsSleeping() && !haveCollision && inMove && !body.isKinematic)
		{
			float decreaseSpeed = body.velocity.magnitude/cueController.ballShotVelocity.magnitude;
			body.velocity = decreaseSpeed*cueController.ballShotVelocity;
			yield return new WaitForFixedUpdate();
		}
	}

	public void UpdateReflaction ()
	{
		if(!ballReflaction)
			return;
		ballReflaction.localPosition = transform.localPosition;
		ballReflaction.localRotation = transform.localRotation;
	}
	public void OnBallIsOut (float _holeSplineLungth)
	{
		body.useGravity = false;
		body.isKinematic = true;
		GetComponent<Collider>().enabled = false;

		body.position = holeSpline.Evaluate(_holeSplineLungth);

		if(!isMain)
		{
			enabled = false;
		}
	}

	void FixedUpdate ()
	{
		if(!cueController || !cueController.ballsIsCreated)
			return;

		if(ballIsOut)
		{
			if(!body.isKinematic && step < holeSplineLungth)
			{
				holeSpline.AnimationSlider(transform, Mathf.Clamp( 0.2f*cueController.ballMaxVelocity, 0.01f, 20.0f ), ref step, out ballVeolociyInHole, 1, false);

				body.velocity = ballVeolociyInHole;
			}
			else
			{
				if(GetComponent<Collider>().enabled)
				OnBallIsOut (holeSplineLungth);
			}
		}
		else
		{
			if(!ballIsSelected && inMove && !body.isKinematic)
			{
				//if(!ServerController.serverController || ServerController.serverController.isMyQueue)
				//
			        velocityNormalized = body.velocity.magnitude/cueController.ballMaxVelocity;


					if(velocityNormalized < 0.01f)
					{
						body.velocity = Vector3.Lerp(body.velocity, Vector3.zero, 5.0f*Time.fixedDeltaTime);
						body.angularVelocity = Vector3.Lerp(body.angularVelocity, Vector3.zero, 1.5f*Time.fixedDeltaTime);
					}
				//}

			}


			UpdateReflaction ();
		}

		checkVelocity = Vector3.Lerp(checkVelocity, body.velocity, 10.0f*Time.fixedDeltaTime);
		checkAngularVelocity = Vector3.Lerp(checkAngularVelocity, body.angularVelocity, 10.0f*Time.fixedDeltaTime);


	}
	public void OnCheckHolle ()
	{
		Destroy(ballReflaction.gameObject);
		cueController.ballControllers.Remove(this);
		cueController.ballControllers.TrimExcess();
	}
	public void OnSetBallReflaction (bool show)
	{
		ballReflaction.GetComponent<Renderer>().enabled = show;
	}

	void OnCollisionExit(Collision collision)
	{
		if(ServerController.serverController && ServerController.serverController.isMyQueue && !MenuControllerGenerator.controller.playWithAI)
		{
			SetBallCollisionData ();
		}
	}
	public void CancelInvokeSetBallCollisionData ()
	{
		StopCoroutine("SetBallCollisionDataRepeating");
	}
	public void SetBallCollisionData ()
	{
		StopCoroutine("SetBallCollisionDataRepeating");
		if(inMove && !ballIsOut)
		StartCoroutine("SetBallCollisionDataRepeating");
	}
	IEnumerator SetBallCollisionDataRepeating ()
	{
		inForceMove = true;
		if(!ballIsOut)
		{
			ServerController.serverController.SendRPCToServer("ForceSetBallMove", ServerController.serverController.otherNetworkPlayer, id, body.position, body.velocity, body.angularVelocity);
		}
		yield return new WaitForSeconds(0.1f);
		while(!IsSleeping() && !ballIsOut)
		{
			if(inMove && !ballIsOut)
			{
				ServerController.serverController.SendRPCToServer("ForceSetBallMove", ServerController.serverController.otherNetworkPlayer, id, body.position, body.velocity, body.angularVelocity);
			}
			yield return new WaitForSeconds(0.1f);
		}
		if(!ballIsOut)
		{
			ServerController.serverController.SendRPCToServer("SetBallSleeping", ServerController.serverController.otherNetworkPlayer, id, body.position);
		}
		inForceMove = false;
	}
	public void OnPlayBallAudio (float audioVolume)
	{
		GetComponent<AudioSource>().volume = audioVolume;
		GetComponent<AudioSource>().Play();
	}
	void OnCollisionEnter(Collision collision) 
	{
		string layerName = LayerMask.LayerToName(collision.collider.gameObject.layer);

		if(layerName != "Wall" && layerName != "Ball")
			return;

		haveCollision = true;
		if(ServerController.serverController && !ServerController.serverController.isMyQueue && !MenuControllerGenerator.controller.playWithAI)
		{
			ServerController.serverController.SendRPCToServer("SetBallMoveRequest", ServerController.serverController.otherNetworkPlayer, id);
		}
		else
		if(ServerController.serverController && (ServerController.serverController.isMyQueue || MenuControllerGenerator.controller.playWithAI))
		{
			if(layerName == "Wall")
			{
				if(cueController.gameManager.isFirstShot && !hasFirstHit && !isMain && !isBlack)
				{
					hasFirstHit = true;
					cueController.gameManager.firstShotHitCount ++;
				}
			}
			else if(isMain && layerName == "Ball")
			{
				BallController firstHitBall = collision.collider.GetComponent<BallController>();
				if(!cueController.gameManager.firstHitBall && firstHitBall)
				{
					cueController.gameManager.firstHitBall = firstHitBall;

					if(ServerController.serverController.isMyQueue)
					{
						if((!cueController.gameManager.tableIsOpened && 
						    ( cueController.gameManager.firstHitBall.ballType == cueController.gameManager.ballType || 
						      (cueController.gameManager.firstHitBall.isBlack && cueController.gameManager.afterRemainedBlackBall)
						    )
						   ) ||
						    (!cueController.gameManager.isFirstShot && cueController.gameManager.tableIsOpened && !cueController.gameManager.firstHitBall.isBlack)
						   )
						{
							cueController.gameManager.setMoveInTable = false;
						}
						else if(cueController.gameManager.gameInfoErrorText == "")
						{
							if(!cueController.gameManager.tableIsOpened && cueController.gameManager.firstHitBall.ballType != cueController.gameManager.ballType)
							{
								cueController.gameManager.gameInfoErrorText = "need to hit a " + (cueController.gameManager.ballType == 1? "solid":"striped") +  " ball";
							}
						}
					} else
					{
						if((!cueController.gameManager.tableIsOpened && 
						    (  cueController.gameManager.firstHitBall.ballType == -cueController.gameManager.ballType || 
						 (cueController.gameManager.firstHitBall.isBlack && cueController.gameManager.afterOtherRemainedBlackBall)
						 )) ||
						   (!cueController.gameManager.isFirstShot && cueController.gameManager.tableIsOpened && !cueController.gameManager.firstHitBall.isBlack)
						   )
						{
							cueController.gameManager.setMoveInTable = false;
						}
						else if(cueController.gameManager.gameInfoErrorText == "")
						{
							if(!cueController.gameManager.tableIsOpened && cueController.gameManager.firstHitBall.ballType == cueController.gameManager.ballType)
							{
								cueController.gameManager.gameInfoErrorText = "need to hit a " + (cueController.gameManager.ballType == -1? "solid":"striped") +  " ball";
							}
						}
					}
				}

			}
		}
		if(cueController.ballsAudioPlayingCount < 3)
		{
			float audioVolume = Mathf.Clamp01( collision.relativeVelocity.magnitude/cueController.ballMaxVelocity );
			if(!ServerController.serverController || ServerController.serverController.isMyQueue || MenuControllerGenerator.controller.playWithAI)
			{
				OnPlayBallAudio(audioVolume);
				if(ServerController.serverController && !MenuControllerGenerator.controller.playWithAI)
					ServerController.serverController.SendRPCToServer("OnPlayBallAudio", ServerController.serverController.otherNetworkPlayer, id, audioVolume);
			}
		}
		if(isMain)
		{
			if(inMove && cueController &&  collision.collider.GetComponent<Rigidbody>())
			{
				firstHitIsChecked = true;
				if(cueController.currentHitBallController && cueController.currentHitBallController.body == collision.collider.GetComponent<Rigidbody>())
					CheckCurrentHitBall (collision.relativeVelocity.magnitude, cueController.ballShotVelocity.magnitude);
				else
					cueController.currentHitBallController = null;
			}
			else
			if(layerName == "Wall" && !body.isKinematic && !ballIsOut)
			{
				if(!firstHitIsChecked)
				{
					firstHitIsChecked = true;
					float decreaseSpeed = checkVelocity.magnitude/cueController.ballShotVelocity.magnitude;
					body.velocity = 0.9f*decreaseSpeed*cueController.secondVelocity;
				}
				else
				SetBallVelocity (checkVelocity);
			}
		}

	}
	void SetBallVelocity (Vector3 velocity)
	{
		if(!ServerController.serverController || ServerController.serverController.isMyQueue || MenuControllerGenerator.controller.playWithAI)
		{
			body.velocity = Vector3.ClampMagnitude( 0.9f*VectorOperator.getBallWallVelocity(0.99f*cueController.ballRadius, velocity, body.position, 
			                                                                                      cueController.wallMask, 20.0f*cueController.ballRadius, Vector3.Project( checkAngularVelocity, Vector3.up)), 1000.0f);
		}
			checkVelocity = body.velocity;
			cueController.currentHitBallController = null;

			if(ServerController.serverController && ServerController.serverController.isMyQueue && !MenuControllerGenerator.controller.playWithAI)
			{
				SetBallCollisionData ();
			}
	}
	public void CheckCurrentHitBall (float velocityMagnitude, float ballShotSpeed)
	{
		if(!ServerController.serverController || ServerController.serverController.isMyQueue || MenuControllerGenerator.controller.playWithAI)
		{
			float decreaseSpeed = velocityMagnitude/ballShotSpeed;
			cueController.currentHitBallController.body.velocity = decreaseSpeed*cueController.hitBallVelocity;
			Vector3 addVelocity = cueController.ballRadius*VectorOperator.getPerpendicularXZ( VectorOperator.getProjectXZ( checkAngularVelocity, false ));
			body.velocity = Vector3.ClampMagnitude( decreaseSpeed*(cueController.secondVelocity + cueController.ballVelocityCurve.Evaluate(cueController.cueForceValue)*addVelocity), body.velocity.magnitude);
		}
			StartCoroutine( WaitAndUncheck ());
		
		if(ServerController.serverController && ServerController.serverController.isMyQueue && !MenuControllerGenerator.controller.playWithAI)
		{
			SetBallCollisionData ();
			cueController.currentHitBallController.SetBallCollisionData ();
		}
	}
	IEnumerator WaitAndUncheck ()
	{
		yield return new WaitForFixedUpdate ();
		yield return new WaitForFixedUpdate ();
		cueController.currentHitBallController = null;
	}
}
