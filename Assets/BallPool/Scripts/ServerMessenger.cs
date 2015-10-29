using UnityEngine;
using System.Collections;

public class ServerMessenger : MonoBehaviour 
{
	public CueController cueController;
	void Awake ()
	{
		Application.runInBackground = true;
	}

	void OnEnable () 
	{
		MenuControllerGenerator.controller.OnLoadLevel += FindCueController;
	}

	void FindCueController (MenuController menuController)
	{
		StopCoroutine("WaitAndFindCueController");
		StopCoroutine("WaitAndEnabledCueController");

		StartCoroutine("WaitAndFindCueController", menuController.preloader);
	}
	void OnDisable ()
	{
		MenuControllerGenerator.controller.OnLoadLevel -= FindCueController;
	}
	IEnumerator WaitAndFindCueController (Preloader preloader)
	{
		cueController = null;
		while(!preloader.isDone)
		{
			yield return null;
		}
		while(!cueController)
		{
			cueController = CueController.FindObjectOfType<CueController>();
			yield return null;
		}
		Debuger.DebugOnScreen("CueController " + (cueController != null).ToString());
		OnFindCueController();
	}
	public void ShotWithAI ()
	{
		ServerController.logicAI.ShotCue(cueController);
	}

	void OnFindCueController()
	{
		if(MenuControllerGenerator.controller.playWithAI)
		{
			OnReadyToPlay ();
			if(!ServerController.serverController.isMyQueue)
			{
				ShotWithAI();
			}
		}
		else
		{
			Debuger.DebugOnScreen("SendRPCToServer ReadyToPlay");
			ServerController.serverController.SendRPCToServer("ReadyToPlay", ServerController.serverController.otherNetworkPlayer);
		}
	}
	public void ShowOtherMessage (string message)
	{
		ProfileMessenger.FindObjectOfType<ProfileMessenger>().ShowOtherMessage(message);
	}
	public void SetMoveInTable ()
	{
		if(!cueController)
			return;
		cueController.cueFSMController.setMoveInTable();
	}
	public void OnChangeQueue (bool myTurn)
	{

	}
	public void OnChanghAllIsSleeping ()
	{
		if(!cueController)
			return;
		cueController.networkAllIsSleeping = true;
	}
	public void OnWantToPlayAgain ()
	{
		StartCoroutine(WaitForOtherWantToPlayAgain());
	}
	IEnumerator WaitForOtherWantToPlayAgain ()
	{
		while(!cueController)
		{
			yield return null;
		}
		yield return new WaitForEndOfFrame(); 
		cueController.otherWantToPlayAgain = true;
		//cueController.gameManager.otherProfile.WantToPlayAgain.gameObject.SetActive(true);
	}
	public void SetPrizeToOther (int otherPrize)
	{
		StartCoroutine(WaitAndSetPrizeToOther (otherPrize));
	}
	IEnumerator WaitAndSetPrizeToOther (int  otherPrize)
	{
		while(!cueController)
		{
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		GameManager gameManager = GameManager.FindObjectOfType<GameManager>();
		gameManager.SetPrizeToOther(otherPrize);
	}
	public void SetHighScoreToOther (int otherHighScore)
	{
		StartCoroutine(WaitAndSetHighScoreToOther (otherHighScore));
	}
	IEnumerator WaitAndSetHighScoreToOther (int otherHighScore)
	{
		while(!cueController)
		{
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		GameManager gameManager = GameManager.FindObjectOfType<GameManager>();
		gameManager.SetHighScoreToOther(otherHighScore);
	}
	public void SetCoinsToOther(int otherCoins)
	{
		StartCoroutine(WaitAndSetCoinsToOther (otherCoins));
	}
	IEnumerator WaitAndSetCoinsToOther (int otherCoins)
	{
		while(!cueController)
		{
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		GameManager gameManager = GameManager.FindObjectOfType<GameManager>();
		gameManager.SetCoinsToOther(otherCoins);
	}
	public void SetErrorText (string errorText)
	{
		if(!cueController)
			return;
		cueController.gameManager.ShowGameInfoError(errorText);
	}
	public void OnReadyToPlay ()
	{
		Debuger.DebugOnScreen("OnReadyToPlay");
		StartCoroutine("WaitAndEnabledCueController");
	}
	IEnumerator WaitAndEnabledCueController ()
	{
		Debuger.DebugOnScreen("StartEnabledCueController");
		Debuger.DebugOnScreen((cueController != null).ToString());
		Debuger.DebugOnScreen((ServerController.serverController.menuButtonsIsActive).ToString());

		Debuger.DebugOnScreen((ServerController.serverController.otherNetworkPlayer < 1).ToString());
		Debuger.DebugOnScreen((!MenuControllerGenerator.controller.playWithAI).ToString());

		while(!cueController || ServerController.serverController.menuButtonsIsActive || 
		      (ServerController.serverController.otherNetworkPlayer < 1 && !MenuControllerGenerator.controller.playWithAI))
		{
			cueController = CueController.FindObjectOfType<CueController>();
			yield return null;
		}
		Debuger.DebugOnScreen("WaitForEndOfFrame");
        yield return new WaitForEndOfFrame();
        cueController.enabled = true;
		cueController.cueFSMController.enabled = true;
		string info = ServerController.serverController.isMyQueue? "Bạn đánh!":"Đối thủ đánh!";
		//cueController.gameManager.ShowGameInfo(info, 2.5f);
        cueController.gameManager.ShowGameInfoError (info);
		Debuger.DebugOnScreen("EndEnabledCueController");
	}
	public void OnSelectBall (Vector3 position)
	{
		if(!cueController)
			return;
		cueController.OnSelectBall(position);
	}
	public void OnUnselectBall ()
	{
		if(!cueController)
			return;
		cueController.OnUnselectBall();
	}
	public void SetOnMoveBall(Vector3 positin )
	{
		if(!cueController)
			return;
		cueController.ballMovePosition = positin;
	}
	public void SetBallMoveRequest (int id)
	{
		if(!cueController)
			return;
		BallController ballController = cueController.startBallControllers[id];
		if(!ballController.inForceMove)
		{
			ServerController.serverController.SendRPCToServer("ForceSetBallMove", ServerController.serverController.otherNetworkPlayer, ballController.id, ballController.GetComponent<Rigidbody>().position, ballController.GetComponent<Rigidbody>().velocity, ballController.GetComponent<Rigidbody>().angularVelocity);
		}
	}
	public void ForceSetBallMove (int id, Vector3 positin, Vector3 velocity, Vector3 angularVelocity )
	{
		if(!cueController)
			return;
		BallController ballController = cueController.startBallControllers[id];
		if(ServerController.serverController.isMyQueue || ballController.ballIsOut)
			return;
		ballController.ForceSetMove(positin, velocity, angularVelocity);
	}
	public void OnPlayBallAudio (int id, float audioVolume)
	{
		if(!cueController)
			return;
		BallController ballController = cueController.startBallControllers[id];
		if(ServerController.serverController.isMyQueue || ballController.ballIsOut)
			return;
		ballController.OnPlayBallAudio(audioVolume);
	}
	public void SetBallSleeping(int id, Vector3 positin)
	{
		if(!cueController)
			return;
		BallController ballController = cueController.startBallControllers[id];
		if(ServerController.serverController.isMyQueue || ballController.ballIsOut)
			return;

		ballController.GetComponent<Rigidbody>().position = positin;
		if(!ballController.GetComponent<Rigidbody>().isKinematic)
		{
			ballController.GetComponent<Rigidbody>().velocity = Vector3.zero;
			ballController.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			StartCoroutine(WaitForFixedUpdateBall (ballController));
		}
	}
	IEnumerator WaitForFixedUpdateBall (BallController ballController)
	{
		yield return new WaitForFixedUpdate();
		if(ballController)
		{
			ballController.GetComponent<Rigidbody>().Sleep();
		}
	}
	private BallController FindBallById(int id)
	{
		return null;
	}
	public void SendCueControl( Quaternion localRotation, Vector3 localPosition, Vector3 rotationDisplacement)
	{
		if(!cueController)
			return;
		cueController.SetCueControlFromNetwork(localRotation, localPosition, new Vector2(rotationDisplacement.x, rotationDisplacement.y));
	}
	public void OnShotCue()
	{
		if(!cueController)
			return;
		cueController.OnShotCue();
	}
	//Player is shot (for ball)
	public void ShotBall(Vector3 ballShotVelocity, Vector3 hitBallVelocity, Vector3 secondVelocity, Vector3 ballShotAngularVelocity)
	{
		if(!cueController)
			return;
		cueController.ballShotVelocity = ballShotVelocity;
		cueController.hitBallVelocity = hitBallVelocity;
		cueController.secondVelocity = secondVelocity;
		cueController.ballShotAngularVelocity = ballShotAngularVelocity;
		cueController.ballController.ShotBall();
	}
	public void SendOnTriggerEnter (int ballId, float audioVolume, float currentLungth, int holleId)
	{
		HolleController holleController = HolleController.FindeHoleById(holleId);
		holleController.SendOnTriggerEnter(ballId, audioVolume, currentLungth, holleId);
	}
}
