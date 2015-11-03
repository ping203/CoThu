using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public CueController cueController;
    [SerializeField]
    private Toast gameInfoToast;
    [SerializeField]
    private Toast gameInfoErrorToast;
    [System.NonSerialized]
    public string gameInfoErrorText = "";
    [System.NonSerialized]
    public bool isFirstShot = true;
    [System.NonSerialized]
    public int firstShotHitCount = 0;

    public ProfilePlayer myProfileNew;
    public ProfilePlayer otherProfileNew;

    [System.NonSerialized]
    public bool needToChangeQueue = true;
    [System.NonSerialized]
    public bool needToForceChangeQueue = false;
    [System.NonSerialized]
    public bool setMoveInTable = true;
    [System.NonSerialized]
    public bool tableIsOpened = true;
    [System.NonSerialized]
    public int ballType = 0;
    [System.NonSerialized]
    public List<BallController> firsBalls;
    [System.NonSerialized]
    public bool remainedBlackBall = false;
    [System.NonSerialized]
    public bool otherRemainedBlackBall = false;

    [System.NonSerialized]
    public bool afterRemainedBlackBall = false;
    [System.NonSerialized]
    public bool afterOtherRemainedBlackBall = false;

    [System.NonSerialized]
    public bool mainBallIsOut = false;
    [System.NonSerialized]
    public bool otherMainBallIsOut = false;

    [System.NonSerialized]
    public bool blackBallInHolle = false;
    [System.NonSerialized]
    public bool otherBlackBallInHolle = false;
    [System.NonSerialized]
    public BallController firstHitBall = null;
    [System.NonSerialized]
    public bool isWinner = false;
    [System.NonSerialized]
    public bool outOfTime = false;
    [SerializeField]
    private UILabel prize;
    [SerializeField]
    private float shotTime = 30.0f;
    private float shotCurrentTime = 0.0f;
    private bool calculateShotTime = false;
    [System.NonSerialized]
    public bool ballsInMove = false;

    public GameObject menu;

    public List<int> cardsID = new List<int> ();

    void Awake () {
        if(!MenuControllerGenerator.controller)
            return;
        if(!ServerController.serverController && !MenuControllerGenerator.controller.playWithAI) {
            Destroy (gameObject);
            return;
        }
        firsBalls = new List<BallController> (0);
        ActivateMenuButtons (false);

        myProfileNew.isMain = true;
        otherProfileNew.isMain = false;
        myProfileNew.gameManager = this;
        otherProfileNew.gameManager = this;
        myProfileNew.OnAwakeGameManager ();
        otherProfileNew.OnAwakeGameManager ();

        myProfileNew.playerName.text = ServerController.serverController.myName;
        myProfileNew.playerNameAvata.text = ServerController.serverController.myName[0] + "";

        otherProfileNew.playerName.text = ServerController.serverController.otherName;
        otherProfileNew.playerNameAvata.text = ServerController.serverController.otherName[0] + "";

        if(ServerController.serverController.isFirstPlayer || MenuControllerGenerator.controller.playWithAI) {
            prize.text = ServerController.serverController.prize.ToString ();
            if(!MenuControllerGenerator.controller.playWithAI) {
                ServerController.serverController.SendRPCToServer ("SetPrizeToOther", ServerController.serverController.otherNetworkPlayer, ServerController.serverController.prize);
            }
        }

        myProfileNew.coins.text = ServerController.serverController.coins.ToString ();

        if(!MenuControllerGenerator.controller.playWithAI) {
            ServerController.serverController.SendRPCToServer ("SetCoinsToOther", ServerController.serverController.otherNetworkPlayer, ServerController.serverController.coins);
        } else {
            SetCoinsToOther (ServerController.serverController.otherCoins);
        }

        int highScore = Profile.GetUserDate (ServerController.serverController.myName + "_High_Score");
        ServerController.serverController.highScore = highScore;
        //myProfile.highScore.text = ServerController.serverController.highScore.ToString();

        if(!MenuControllerGenerator.controller.playWithAI) {
            ServerController.serverController.SendRPCToServer ("SetHighScoreToOther", ServerController.serverController.otherNetworkPlayer, ServerController.serverController.highScore);
        }
        HideGameInfoError ();

        StartCalculateShotTime ();

        myProfileNew.timeSlider.SetValue (0.0f);
        otherProfileNew.timeSlider.SetValue (0.0f);

        ServerController.serverController.OnChangeQueueEvent += OnChangeCalculateShotTime;

        if(ServerController.serverController.isFirstPlayer) {
            //StartCoroutine(myProfile.SetAvatar(firstAvatar));
            //StartCoroutine(otherProfile.SetAvatar(secondAvatar));
        } else if(ServerController.serverController.isSecondPlayer) {
            //StartCoroutine(otherProfile.SetAvatar(firstAvatar));
            //StartCoroutine(myProfile.SetAvatar(secondAvatar));
        }

        genCard ();

        for(int i = 0; i < cardsID.Count; i++) {
            if(i < 9) {
                myProfileNew.AddGuiCard (cardsID[i], i);
            } else {
                otherProfileNew.AddGuiCard (cardsID[i], i - 9);
            }
        }
    }

    /*void setPlayer(ProfilePlayer pl, string name, string coints){
        pl.playerName.text = name;
        pl.playerNameAvata.text = name [0];
        pl.coins.text = coints;
    }
*/

    public void genCard () {
        int i2 = 0;
        while(i2 < 18) {
            int a = Random.Range (0, 52);

            bool isExist = false;
            for(int j=0; j < cardsID.Count; j++) {
                if(a == cardsID[j]) {
                    isExist = true;
                    break;
                }
            }
            if(!isExist) {
                i2++;
                cardsID.Add (a);
            }
        }
    }

    void Update () {
        if(calculateShotTime) {
            if(cueController.enabled) {
                shotCurrentTime -= Time.deltaTime;
            }

            if(shotCurrentTime <= 0.0f) {
                StopCalculateShotTime ();
                gameInfoErrorText = "hết thời gian!";


                if(ServerController.serverController.isMyQueue) {
                    string myGameInfoErrorText = "Bạn " + gameInfoErrorText + "\n Tới lượt " + ServerController.serverController.otherName;
                    string otherGameInfoErrorText = ServerController.serverController.myName + " " + gameInfoErrorText + "\nTới lượt bạn!";

                    ShowGameInfoError (myGameInfoErrorText);
                    ServerController.serverController.SendRPCToServer ("SetErrorText", ServerController.serverController.otherNetworkPlayer, otherGameInfoErrorText);
                    ServerController.serverController.SendRPCToServer ("SetMoveInTable", ServerController.serverController.otherNetworkPlayer);

                    ServerController.serverController.ChangeQueue (false);
                } else if(MenuControllerGenerator.controller.playWithAI) {
                    string otherGameInfoErrorText = ServerController.serverController.otherName + " " + gameInfoErrorText + "\nTới lượt bạn!";
                    ShowGameInfoError (otherGameInfoErrorText);
                    cueController.cueFSMController.setMoveInTable ();
                    ServerController.serverController.ChangeQueue (true);

                }
            }
            if(ServerController.serverController.isMyQueue) {
                myProfileNew.timeSlider.SetValue (shotCurrentTime / shotTime);
                otherProfileNew.timeSlider.SetValue (0.0f);

                //myProfileNew.setPercentage(shotCurrentTime / shotTime);
                //otherProfileNew.setPercentage(100);
            } else {
                myProfileNew.timeSlider.SetValue (0.0f);
                otherProfileNew.timeSlider.SetValue (shotCurrentTime / shotTime);

                //myProfileNew.setPercentage(100);
                //otherProfileNew.setPercentage(shotCurrentTime / shotTime);
            }
        }
    }
    void OnDestroy () {
        if(ServerController.serverController) {
            ServerController.serverController.OnChangeQueueEvent -= OnChangeCalculateShotTime;
        }
    }
    void OnChangeCalculateShotTime (bool myTurn) {
        cueController.RessetShotOptions ();
        StartCalculateShotTime ();
    }
    public void StartCalculateShotTime () {
        shotCurrentTime = shotTime;
        calculateShotTime = true;
    }
    public void StopCalculateShotTime () {
        calculateShotTime = false;
        shotCurrentTime = 0.0f;
    }
    public void SetPrizeToOther (int otherPrize) {
        ServerController.serverController.prize = otherPrize;
        prize.text = otherPrize.ToString ();
    }
    public void SetHighScoreToOther (int otherHighScore) {
        ServerController.serverController.otherHighScore = otherHighScore;
        //otherProfile.highScore.text = otherHighScore.ToString();
    }
    public void SetCoinsToOther (int otherCoins) {
        ServerController.serverController.otherCoins = otherCoins;
        //otherProfile.coins.text = otherCoins.ToString();
        otherProfileNew.coins.text = otherCoins.ToString ();
    }
    public void ActivateMenuButtons (bool value) {
        if(!value) {
            menu.SetActive (value);
        }
        ServerController.serverController.menuButtonsIsActive = value;
        if(value) {
            cueController.enabled = false;
            cueController.cueFSMController.enabled = false;

            if(!afterRemainedBlackBall && !afterOtherRemainedBlackBall) {
                isWinner = !ServerController.serverController.isMyQueue;
            } else {
                if(ServerController.serverController.isMyQueue) {
                    isWinner = (afterRemainedBlackBall && !mainBallIsOut);

                    if(MenuControllerGenerator.controller.playWithAI) {
                        if(!mainBallIsOut && firstHitBall && !firstHitBall.isBlack && afterRemainedBlackBall && ballType != firstHitBall.ballType) {
                            isWinner = false;
                        }
                    }
                } else {
                    isWinner = !(afterOtherRemainedBlackBall && !otherMainBallIsOut);

                    if(MenuControllerGenerator.controller.playWithAI) {
                        if(!otherMainBallIsOut && firstHitBall && !firstHitBall.isBlack && afterOtherRemainedBlackBall && ballType == firstHitBall.ballType) {
                            isWinner = true;
                        }
                    }
                }
            }
            ServerController.serverController.isMyQueue = isWinner;

            ServerController.serverController.coins += isWinner ? ServerController.serverController.prize : -ServerController.serverController.prize;
            ServerController.serverController.coins = Mathf.Clamp (ServerController.serverController.coins, ServerController.serverController.minCoins, ServerController.serverController.maxCoins);
            Profile.SetUserDate (ServerController.serverController.myName + "_Coins", ServerController.serverController.coins);

            myProfileNew.coins.text = ServerController.serverController.coins.ToString ();

            ServerController.serverController.SendRPCToServer ("SetCoinsToPlayerClient", ServerController.serverController.myNetworkPlayer, ServerController.serverController.coins);

            ServerController.serverController.otherCoins += isWinner ? -ServerController.serverController.prize : ServerController.serverController.prize;
            ServerController.serverController.otherCoins = Mathf.Clamp (ServerController.serverController.otherCoins, ServerController.serverController.minCoins, ServerController.serverController.maxCoins);
            if(MenuControllerGenerator.controller.playWithAI) {
                Profile.SetUserDate (ServerController.serverController.otherName + "_Coins", ServerController.serverController.otherCoins);
            }
            otherProfileNew.coins.text = ServerController.serverController.otherCoins.ToString ();

            if(isWinner) {
                myProfileNew.setWinner ();
            } else {
                otherProfileNew.setWinner ();
            }
            if(ServerController.serverController.coins < ServerController.serverController.prize) {
                if(!MenuControllerGenerator.controller.playWithAI) {
                    StartCoroutine (WaitAndDisconnect ());
                } else {
                    MenuControllerGenerator.controller.playWithAI = false;
                    MenuControllerGenerator.controller.OnGoBack ();
                }
            } else {
                menu.SetActive (value);
            }
            if(MenuControllerGenerator.controller.playWithAI) {
                cueController.otherWantToPlayAgain = true;
                //otherProfile.WantToPlayAgain.gameObject.SetActive(true);
            }
        }
    }
    IEnumerator WaitAndDisconnect () {
        yield return new WaitForSeconds (1.5f);
        MasterServerGUI.Disconnect ();
    }
    //public void ShowGameInfo(string info)
    //{
    //    gameInfo.gameObject.SetActive(true);
    //    gameInfo.text = info;
    //}
    public void ShowGameInfo (string info, int startIndex) {
        gameInfoToast.setText (info, startIndex);
    }

    public void ShowGameInfoError (string info) {
        if(MenuControllerGenerator.controller.playWithAI) {
            cueController.cueFSMController.setMoveInTable ();
        }
        gameInfoErrorToast.setText (info);
    }
    public void HideGameInfoError () {
        gameInfoErrorToast.gameObject.SetActive (false);
    }
    public void HideGameInfo () {
        gameInfoToast.gameObject.SetActive (false);
    }
    public void SetBallType (int ballType) {
        this.ballType = ballType;
        if(ballType == 1) {
            for(int i = 0; i < 7; i++) {
                myProfileNew.AddGuiBall (i + 1, i);
                otherProfileNew.AddGuiBall (i + 9, i);
            }
        } else if(ballType == -1) {
            for(int i = 0; i < 7; i++) {
                otherProfileNew.AddGuiBall (i + 1, i);
                myProfileNew.AddGuiBall (i + 9, i);
            }
        }
    }
    void GetMenu () {
        if(MenuControllerGenerator.controller.playWithAI) {
            MenuControllerGenerator.controller.playWithAI = false;
            MenuControllerGenerator.controller.OnGoBack ();
        } else {
            MasterServerGUI.Disconnect ();
        }

    }
    public void PlayAgain () {
        //myProfile.WantToPlayAgain.gameObject.SetActive(true);
        ServerController.serverController.SendRPCToServer ("WantToPlayAgain", ServerController.serverController.otherNetworkPlayer);
        ActivateMenuButtons (false);
        //ShowGameInfo("Waiting for your opponent");
        StartCoroutine (WaitWhenOtherWantToPlayAgain ());
    }

    public void back () {
        MenuControllerGenerator.controller.playWithAI = false;
        MenuControllerGenerator.controller.OnGoBack ();
    }

    IEnumerator WaitWhenOtherWantToPlayAgain () {
        while(!cueController.otherWantToPlayAgain) {
            yield return null;
        }
        yield return new WaitForSeconds (1.5f);
        MenuControllerGenerator.controller.LoadLevel (Application.loadedLevel);
    }
}
