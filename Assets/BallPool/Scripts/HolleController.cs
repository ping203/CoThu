using UnityEngine;
using System.Collections;

public class HolleController : MonoBehaviour
{
	public int id = 1;
	private CueController cueController;
	[SerializeField]
	private Transform[] nodes;
	public AnimationSpline ballSpline;

	[System.NonSerialized]
	public float splineLungth = 0.0f;
	[System.NonSerialized]
	public float splineCurrentLungth = 0.0f;
	private Vector3 ballVeolociy;
	[SerializeField]
	private HolleController[] neighbors;


	public static HolleController FindeHoleById (int holeId)
	{
		foreach (HolleController item in FindObjectsOfType(typeof(HolleController)) as HolleController[] )
		{
			if(item.id == holeId)
				return item;
		}
		return null;
	}
	public bool haveNeighbors(HolleController holleController)
	{
		if(!holleController)
			return false;

		foreach (HolleController item in neighbors) 
		{
			if(holleController == item)
				return true;
		}
		return false;
	}

	void Awake ()
	{
		//collider.enabled = false;
		for (int i = 0; i < nodes.Length; i++) 
		{
			nodes[i].GetComponent<Renderer>().enabled = false;
		}
		cueController = CueController.FindObjectOfType(typeof(CueController)) as CueController;
		GetComponent<Renderer>().enabled = false;
		ballSpline = new AnimationSpline(WrapMode.Clamp);
	}
	void Start ()
	{
		if(nodes.Length > 0)
		{
			ballSpline.CreateSpline(nodes, true, false, false, 2.0f*cueController.ballRadius);
			splineLungth = ballSpline.splineLength;
		}
	}
	public void DecreaseSplineLength ()
	{
		splineCurrentLungth = splineLungth;
		splineLungth -= 2.0f*cueController.ballRadius;
		for (int i = 0; i < neighbors.Length; i++) 
		{
			neighbors[i].splineCurrentLungth = splineLungth;
			neighbors[i].splineLungth -= 2.0f*cueController.ballRadius;
		}
	}
	public void IncreaseSplineLength ()
	{
		splineLungth += 2.0f*cueController.ballRadius;
		for (int i = 0; i < neighbors.Length; i++) 
		{
			neighbors[i].splineLungth += 2.0f*cueController.ballRadius;
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		BallController ballController = other.GetComponent<BallController>();
		if(ballController)
		{
			if(ballController.ballIsOut)
				return;
			if(!ServerController.serverController || ServerController.serverController.isMyQueue || MenuControllerGenerator.controller.playWithAI)
			{
				float audioVolume = Mathf.Clamp01( ballController.GetComponent<Rigidbody>().velocity.magnitude/cueController.ballMaxVelocity );
				GetComponent<AudioSource>().volume = audioVolume;
				GetComponent<AudioSource>().Play();
				ballController.step = 0.0f;
				DecreaseSplineLength ();

				ballController.ballIsOut = true;
				ballController.OnSetHoleSpline(splineCurrentLungth, id);

				StartCoroutine("RessetBall", ballController);

				if(ServerController.serverController && ServerController.serverController.isMyQueue && !MenuControllerGenerator.controller.playWithAI)
				{
					ServerController.serverController.SendRPCToServer("ForceSetBallMove", ServerController.serverController.otherNetworkPlayer, ballController.id, ballController.GetComponent<Rigidbody>().position, 
					                                                       ballController.GetComponent<Rigidbody>().velocity, ballController.GetComponent<Rigidbody>().angularVelocity);
					ServerController.serverController.SendRPCToServer("SendOnTriggerEnter", ServerController.serverController.otherNetworkPlayer, ballController.id, audioVolume, splineCurrentLungth, id);
					ballController.CancelInvokeSetBallCollisionData ();
				}
			}
		}

	}
	public void SendOnTriggerEnter (int ballId, float audioVolume, float currentLungth, int holleId)
	{
		BallController ballController = cueController.startBallControllers[ballId];
		if(ballController.ballIsOut)
			return;
		GetComponent<AudioSource>().volume = audioVolume;
		GetComponent<AudioSource>().Play();
		ballController.step = 0.0f;
		DecreaseSplineLength ();
		
		ballController.ballIsOut = true;
		ballController.OnSetHoleSpline(currentLungth, holleId);
		StartCoroutine("RessetBall", ballController);
	}

	IEnumerator RessetBall (BallController ballController) 
	{
		if(ballController.isMain)
		{
			yield return new WaitForFixedUpdate();
			if(ServerController.serverController)
			{
				if(ServerController.serverController.isMyQueue)
				{
					if(!(cueController.gameManager.blackBallInHolle || cueController.gameManager.otherBlackBallInHolle))
					{
						cueController.gameManager.needToChangeQueue = true;
						cueController.gameManager.needToForceChangeQueue = true;
						cueController.gameManager.setMoveInTable = true;
						cueController.gameManager.gameInfoErrorText = "đánh bóng lỗi";
					}

					cueController.gameManager.mainBallIsOut = true;
				}
				else
				{
					if(MenuControllerGenerator.controller.playWithAI)
					{
						if(!(cueController.gameManager.blackBallInHolle || cueController.gameManager.otherBlackBallInHolle))
						{
							cueController.gameManager.needToChangeQueue = true;
							cueController.gameManager.needToForceChangeQueue = true;
							cueController.gameManager.setMoveInTable = true;
							cueController.gameManager.gameInfoErrorText = "đánh bóng lỗi";
						}
					}
					cueController.gameManager.otherMainBallIsOut = true;
				}
			}
			ballController.ballReflaction.GetComponent<Renderer>().enabled = false;
			IncreaseSplineLength ();
			cueController.OnBallIsOut(true);
			yield return new WaitForSeconds(2.0f);

			while(!cueController.othersSleeping)
			{
				yield return null;
			}

			ballController.RessetPosition(cueController.centerPoint.position, false);
			ballController.ballReflaction.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			yield return new WaitForFixedUpdate();
			ballController.OnCheckHolle();
			if(ServerController.serverController)
			{
				if(ballController.isBlack)
				{
					cueController.gameManager.needToChangeQueue = false;
					cueController.gameManager.needToForceChangeQueue = false;

					if(cueController.gameManager.remainedBlackBall)
					{
                        //cueController.gameManager.myProfile.RemoveGuiBall(ballController.id);
                        cueController.gameManager.myProfileNew.RemoveGuiBall(ballController.id);
					}
					if(cueController.gameManager.otherRemainedBlackBall)
					{
                        //cueController.gameManager.otherProfile.RemoveGuiBall(ballController.id);
                        cueController.gameManager.otherProfileNew.RemoveGuiBall(ballController.id);
					}

					if(ServerController.serverController.isMyQueue)
					{
						cueController.gameManager.blackBallInHolle = true;
					}
					else
					{
						cueController.gameManager.otherBlackBallInHolle = true;
					}

				}
				else 
				{
					if(cueController.gameManager.isFirstShot)
					{
						if(!cueController.gameManager.needToForceChangeQueue)
						{
							cueController.gameManager.needToChangeQueue = false;
						}
						cueController.gameManager.firsBalls.Add(ballController);
					} else
					if(cueController.gameManager.tableIsOpened)
					{
						cueController.gameManager.tableIsOpened = false;

						if(ServerController.serverController.isMyQueue)
						{
							if(!cueController.gameManager.needToForceChangeQueue)
							{
								cueController.gameManager.needToChangeQueue = false;
							}
							cueController.gameManager.SetBallType(ballController.ballType);

                            //cueController.gameManager.myProfile.RemoveGuiBall(ballController.id);
                            cueController.gameManager.myProfileNew.RemoveGuiBall(ballController.id);

							//string info = ballController.ballType == 1? "Bạn đánh bi":
								//                                  "You are STRIPES\nPot all the striped balls to win";
                            if(ballController.ballType == 1) {
                                cueController.gameManager.ShowGameInfo ("Bạn đánh bi", 1);
                            } else {
                                cueController.gameManager.ShowGameInfo ("Bạn đánh bi", 9);
                            }
							
						}
						else
						{
							if(MenuControllerGenerator.controller.playWithAI)
							{
								if(!cueController.gameManager.needToForceChangeQueue)
								{
									cueController.gameManager.needToChangeQueue = false;
								}
							}
							cueController.gameManager.SetBallType(-ballController.ballType);

                            //cueController.gameManager.otherProfile.RemoveGuiBall(ballController.id);
                            cueController.gameManager.otherProfileNew.RemoveGuiBall(ballController.id);

							//string info = ballController.ballType == -1? "You are SOLIDS\nPot all the solid balls to win":
							//	                                   "You are STRIPES\nPot all the striped balls to win";
							//cueController.gameManager.ShowGameInfo(info, 3.5f);

                            if(ballController.ballType == -1) {
                                cueController.gameManager.ShowGameInfo ("Bạn đánh bi", 1);
                            } else {
                                cueController.gameManager.ShowGameInfo ("Bạn đánh bi", 9);
                            }
						}

						foreach (BallController item in cueController.gameManager.firsBalls) 
						{
							if(item.ballType == cueController.gameManager.ballType)
							{
                                //cueController.gameManager.myProfile.RemoveGuiBall(item.id);
                                cueController.gameManager.myProfileNew.RemoveGuiBall(item.id);
							}
							else
							{
                                //cueController.gameManager.otherProfile.RemoveGuiBall(item.id);
                                cueController.gameManager.otherProfileNew.RemoveGuiBall(item.id);
							}
						}
						cueController.gameManager.firsBalls = null;
					}
					else 
					{
						if( cueController.gameManager.ballType == ballController.ballType)
						{
							if(!MenuControllerGenerator.controller.playWithAI || ServerController.serverController.isMyQueue)
							{
								if(!cueController.gameManager.firstHitBall || cueController.gameManager.firstHitBall.ballType == cueController.gameManager.ballType)
								{
									if(!cueController.gameManager.needToForceChangeQueue)
									{
										cueController.gameManager.needToChangeQueue = false;
									}
								}
							}
                            //cueController.gameManager.myProfile.RemoveGuiBall(ballController.id);
                            cueController.gameManager.myProfileNew.RemoveGuiBall(ballController.id);
						}
						else
						{
							if(MenuControllerGenerator.controller.playWithAI && !ServerController.serverController.isMyQueue)
							{
								if(!cueController.gameManager.firstHitBall || cueController.gameManager.firstHitBall.ballType == -cueController.gameManager.ballType)
								{
									if(!cueController.gameManager.needToForceChangeQueue)
									{
										cueController.gameManager.needToChangeQueue = false;
									}
								}
							}
                            //cueController.gameManager.otherProfile.RemoveGuiBall(ballController.id);
                            cueController.gameManager.otherProfileNew.RemoveGuiBall(ballController.id);
						}
					}
				}
			}
		}
	}
}
