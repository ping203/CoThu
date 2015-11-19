using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class CueController : MonoBehaviour {
    #region Parametrs
    //Distance of balls in the start, (correctly when it is a zero),
    [SerializeField]
    private float ballsDistance = 0.0f;
    //Maximum displacement of the cue during shot,
    public float cueMaxDisplacement = 9.0f;
    //Maximum velocity of the ball during shot, with maximum displacement of the cue,
    public float ballMaxVelocity = 85.0f;
    //Maximum length of the line, which shows the direction of the velocity after hitting balls,
    [SerializeField]
    private float ballLineLength = 7.0f;

    //Defines the angular velocity of the ball at impact, depending on the impact force,
    public AnimationCurve ballAngularVelocityCurve;
    //When the ball collides with another ball, he loses the linear velocity,but, because it also has a angular velocity, he can ride forward after collision
    //depending on the increased the force shot , the  ball gets a less angular velocity because no time to roll on the ground,
    public AnimationCurve ballVelocityCurve;
    //Control  sensitivity of cue for the mobile devices.
    public float touchSensitivity = 0.075f;
    #endregion
    [SerializeField]
    private Menu menu;
    [SerializeField]
    private Camera guiCamera;
    [SerializeField]
    private Camera camera2D;
    [SerializeField]
    private Camera camera3D;
    [SerializeField]
    private Texture2D collisionBall;
    [SerializeField]
    private Texture2D collisionBallRed;
    private bool checkMyBall = false;
    private bool oldCheckMyBall = false;

    public CueFSMController cueFSMController;

    [SerializeField]
    private Transform mainBallPoint;
    [SerializeField]
    private Transform firstBallPoint;
    public Transform centerPoint;
    [System.NonSerialized]
    public List<BallController> ballControllers;
    [System.NonSerialized]
    public BallController[] startBallControllers;

    public Texture2D[] ballTextures;
    [SerializeField]
    private Vector2[] deltaPositions;
    [System.NonSerialized]
    public int ballsCount = 16;

    [SerializeField]
    private Transform ballsParent;
    [SerializeField]
    private Transform ballsReflactionParent;
    [SerializeField]
    private Renderer lights;


    [SerializeField]
    private BallController ballControllerPrefab;
    [System.NonSerialized]
    public BallController ballController;

    [System.NonSerialized]
    public BallController currentSelectedBallController;

    private Camera currentCamera;
    public Transform cuePivot;

    public Transform cueRotation;
    private Vector3 checkCuePosition = Vector3.zero;

    public BallPivotController cueBallPivot;
    [System.NonSerialized]
    public float ballRadius = 0.35f;
    [System.NonSerialized]
    public float cueDisplacement = 0.0f;


    [System.NonSerialized]
    public bool shotingInProgress = false;
    [System.NonSerialized]
    public bool thenInshoting = true;

    [System.NonSerialized]
    public bool allIsSleeping = true;
    [System.NonSerialized]
    public bool othersSleeping = true;
    private bool checkAllIsSleeping = true;
    private bool checkOthersIsSleeping = true;
    private bool checkInProgress = false;
    [System.NonSerialized]
    public bool inMove = false;
    [System.NonSerialized]
    public bool canMoveBall = true;
    [System.NonSerialized]
    public bool isFirsTime = true;
    [System.NonSerialized]
    public bool ballIsOut = false;

    private bool oldAllIsSleeping = true;

    [SerializeField]
    private GameObject cameraCircularSlider;
    public Transform StartCube;
    public Transform MoveCube;
    public Transform collisionSphere;
    [SerializeField]
    private LineRenderer firstCollisionLine;
    [SerializeField]
    private LineRenderer secondCollisionLine;
    [SerializeField]
    private LineRenderer ballCollisionLine;

    private Vector3 ballSelectPosition = Vector3.zero;

    private Vector3 ballCurrentPosition = Vector3.zero;
    [System.NonSerialized]
    public Vector3 ballShotVelocity = Vector3.zero;
    [System.NonSerialized]
    public Vector3 ballShotAngularVelocity = Vector3.zero;

    [System.NonSerialized]
    public Vector3 ballVelocityOrient = Vector3.forward;
    [System.NonSerialized]
    public Vector3 OutBallFirstVelocityOrient;
    [System.NonSerialized]
    public Vector3 OutBallSecondVelocityOrient;
    [System.NonSerialized]
    public Vector3 secondVelocity = Vector3.zero;

    [System.NonSerialized]
    public Vector2 rotationDisplacement = Vector2.zero;
    [System.NonSerialized]
    public Vector3 hitBallVelocity = Vector3.zero;
    [System.NonSerialized]
    public BallController currentHitBallController = null;
    [System.NonSerialized]
    public Collider hitCollider = null;

    [System.NonSerialized]
    public bool haveFirstCollision = false;
    [System.NonSerialized]
    public bool haveSecondCollision = false;
    [System.NonSerialized]
    public bool haveThrthCollision = false;


    [System.NonSerialized]
    public LayerMask mainBallMask;
    [System.NonSerialized]
    public LayerMask ballMask;
    [System.NonSerialized]
    public LayerMask canvasMask;
    [System.NonSerialized]
    public LayerMask wallMask;
    [System.NonSerialized]
    public LayerMask wallAndBallMask;
    private LayerMask canvasAndBallMask;
    private bool hitCanvas = false;

    [System.NonSerialized]
    public bool is3D = true;

    public Camera3DController camera3DController;
    private bool canDrawLinesAndSphere = false;
    [System.NonSerialized]
    public int ballsAudioPlayingCount = 0;
    private Vector3 cueRotationStrLocalPosition = Vector3.zero;


    private Vector3 cue2DStartPosition = Vector3.zero;

    [System.NonSerialized]
    public float cueForceValue = 1.0f;
    [System.NonSerialized]
    public bool inTouchForceSlider = false;
    [System.NonSerialized]
    public bool cueForceisActive = false;
    [System.NonSerialized]
    public float timeAfterShot = 0.0f;
    [System.NonSerialized]
    public bool networkAllIsSleeping = true;

    //[System.NonSerialized]
    public GameManager gameManager;

    [System.NonSerialized]
    public Vector3 cueRotationLocalPosition = Vector3.zero;
    private Quaternion cuePivotLocalRotation = Quaternion.identity;
    [System.NonSerialized]
    public Vector3 cueBallPivotLocalPosition = Vector3.zero;
    [System.NonSerialized]
    public Vector3 ballMovePosition = Vector3.zero;
    private float sendTime = 0.0f;
    [System.NonSerialized]
    public bool ballsIsCreated = false;

    [System.NonSerialized]
    public bool otherWantToPlayAgain = false;
    private float touchRotateAngle;

    //public UnityEngine.UI.Button btnLeft, btnRight;
    //public UIButton btnLeft, btnRight;

    public CueForce cueForce;
    //Vector3 cuePos;
    void Awake () {
        if(!MenuControllerGenerator.controller)
            return;


        if(ServerController.serverController) {
            //gameManager = GameManager.FindObjectOfType<GameManager>();
            gameManager.ShowGameInfoError ("Chờ đối thủ!");
            enabled = false;
        }
        is3D = PlayerPrefs.GetInt ("Current Camera") == 1;
        lights.enabled = is3D;
        currentCamera = is3D ? camera3D : camera2D;
        currentCamera.enabled = true;

        ballControllers = new List<BallController> (0);

        CreateAndSortBalls ();
        ballsIsCreated = true;

        foreach(BallController item in ballControllers) {
            item.cueController = this;
            item.OnStart ();
        }

        mainBallMask = 1 << LayerMask.NameToLayer ("MainBall");
        ballMask = 1 << LayerMask.NameToLayer ("Ball");
        canvasMask = 1 << LayerMask.NameToLayer ("Canvas");
        wallMask = 1 << LayerMask.NameToLayer ("Wall");
        wallAndBallMask = wallMask | ballMask;
        canvasAndBallMask = canvasMask | ballMask;

        camera3D.enabled = false;
        //camera2D.enabled = false;

        collisionSphere.GetComponent<Renderer> ().sharedMaterial.mainTexture = collisionBall;

        //btnLeft.pressed

        //cuePos = cueForce.transform.position;
        cueCircularSlider.CircularSliderPress += changeRotateCue;
        rotation = cuePivot.localRotation.x;
    }
    void OnEnable () {
        if(ServerController.serverController) {
            if(gameManager)
                gameManager.HideGameInfo ();
        }
    }
    void Start () {
        if(!MenuControllerGenerator.controller)
            return;
        FirstStart ();
        cueFSMController.setMoveInTable ();

        MenuControllerGenerator.controller.canRotateCue = true;
        //sliderCueAngle
        //EventDelegate.Set (sliderCueAngle.onChange, delegate { cueAngle (); });
        //EventDelegate.Set(btnLeft.onClick, delegate{
        //     cueAngleLeft(Time.deltaTime);
        //});
        //EventDelegate.Set(btnRight.onClick, delegate
        //{
        //    cueAngleRight(Time.deltaTime);
        //});
        //btnLeft.onClick.AddListener (delegate {
        //	cueAngleLeft(Time.deltaTime);
        //});
        //btnRight.onClick.AddListener (delegate {
        //	cueAngleRight(Time.deltaTime);	});
    }

    public void cueAngleLeft () {
        if(ServerController.serverController.isMyQueue) {
            //cueRotation.localRotation = Quaternion.Euler (0.0f, dt,0.0f);
            Quaternion vt = cuePivot.transform.localRotation;
            vt.y += Time.deltaTime;
            cuePivot.transform.localRotation = vt;
        }
    }

    public void cueAngleRight () {
        if(ServerController.serverController.isMyQueue) {
            //cueRotation.localRotation = Quaternion.Euler (0.0f, -dt, 0.0f);
            Quaternion vt = cuePivot.transform.localRotation;
            vt.y -= Time.deltaTime;
            cuePivot.transform.localRotation = vt;
        }
    }

    void Update () {
        //if (btnLeft.state == UIButton.State.Pressed) {
        //	cueAngleLeft(Time.deltaTime);
        //}
        //if (btnRight.state == UIButton.State.Pressed) {
        //    cueAngleRight(Time.deltaTime);
        //}	
    }

    void FixedUpdate () {
        if(shotingInProgress) {
            UpdateShotCue ();
        }
        if(!allIsSleeping)
            timeAfterShot += Time.fixedDeltaTime;
    }

    #region FSMController
    void FirstStart () {
        cueRotationStrLocalPosition = cueRotation.localPosition;
        OnShowLineAndSphereFirstTime ();
    }
    //When  player can move the cue ball in  table
    public void MoveInTable () {
        if(oldAllIsSleeping != allIsSleeping) {
            SetOnChengSleeping ();
            cueFSMController.setInMove ();
            if(allIsSleeping && ServerController.serverController) {
                gameManager.StartCalculateShotTime ();
            }
            return;
        }
        if((!ServerController.serverController || (ServerController.serverController.isMyQueue && networkAllIsSleeping)) && menu.GetButtonDown ()) {
            Ray ray = currentCamera.ScreenPointToRay (menu.GetScreenPoint ());
            RaycastHit hit;

            if(Physics.SphereCast (ray, 5.0f * ballRadius, out hit, 1000.0f, mainBallMask)) {
                OnSelectBall (ballController.transform.position);

                if(ServerController.serverController && ServerController.serverController.isMyQueue) {
                    ServerController.serverController.SendRPCToServer ("OnSelectBall", ServerController.serverController.otherNetworkPlayer, ballController.transform.position);
                }
            }
        }
        if(allIsSleeping && canDrawLinesAndSphere && !shotingInProgress/* && !ballIsOut*/)
            DrawLinesAndSphere ();

        canDrawLinesAndSphere = true;
    }

    public void MoveBall () {
        if(ballIsOut) {
            OnBallIsOut (false);
        }

        if(ballController.ballIsSelected) {
            if((!ServerController.serverController || ServerController.serverController.isMyQueue) && menu.MouseIsMove)
                OnMoveBall ();
            if(ServerController.serverController && !ServerController.serverController.isMyQueue) {
                ballController.transform.position = Vector3.Lerp (ballController.transform.position, ballMovePosition, 10.0f * Time.deltaTime);
            }
        }
        if(ballController.ballIsSelected && menu.GetButtonUp ()) {
            OnUnselectBall ();
            if(ServerController.serverController && ServerController.serverController.isMyQueue) {
                ServerController.serverController.SendRPCToServer ("OnUnselectBall", ServerController.serverController.otherNetworkPlayer);
            }
        }

    }
    public void InMove () {
        CheckAllIsSleeping ();

        if(oldAllIsSleeping != allIsSleeping) {
            SetOnChengSleeping ();
            if(allIsSleeping) {
                StopCoroutine ("WaitAndShowLineAndSphere");
                StartCoroutine ("WaitAndShowLineAndSphere", false);
                if(ServerController.serverController) {
                    gameManager.StartCalculateShotTime ();
                }
            } else {
                HideLineAndSphere ();
            }

            foreach(BallController item in ballControllers) {
                item.inMove = !allIsSleeping;
            }
        }
        if(allIsSleeping && !shotingInProgress && !ballIsOut)
            DrawLinesAndSphere ();

        if(!allIsSleeping) {
            ballController.ballReflaction.parent.localScale *= -1.0f;
            ballController.ballReflaction.parent.localScale *= -1.0f;
        }
    }
    #endregion
    //When player select the ball
    public void OnSelectBall (Vector3 ballPosition) {
        HideLineAndSphere ();
        MenuControllerGenerator.controller.canControlCue = false;
        ballMovePosition = ballPosition;
        ballSelectPosition = ballPosition;
        ballCurrentPosition = ballSelectPosition;

        ballController.ballIsSelected = true;



        ballController.GetComponent<Collider> ().enabled = false;
        ballController.GetComponent<Rigidbody> ().useGravity = false;
        ballController.GetComponent<Rigidbody> ().isKinematic = true;

        canMoveBall = true;
        cueFSMController.setMoveBall ();
    }
    //When  player move the cue ball in table
    void OnMoveBall () {
        Transform StartOrMoveCube = isFirsTime ? StartCube : MoveCube;
        Ray ray = currentCamera.ScreenPointToRay (menu.GetScreenPoint ());
        RaycastHit hit;

        if(Physics.SphereCast (ray, 1.0f * ballRadius, out hit, 1000.0f, canvasAndBallMask)) {
            VectorOperator.MoveBallInQuad (StartOrMoveCube, ballRadius, hit.point, ref ballCurrentPosition);
            ballController.transform.position = ballCurrentPosition + 1.5f * ballRadius * Vector3.up;
            if(ServerController.serverController && ServerController.serverController.isMyQueue) {
                sendTime += Time.deltaTime;
                if(sendTime > 1.0f / 10.0f) {
                    sendTime = 0.0f;
                    ServerController.serverController.SendRPCToServer ("SetOnMoveBall", ServerController.serverController.otherNetworkPlayer, ballController.transform.position);
                }
            }
        }
    }
    //When player unselect the ball
    public void OnUnselectBall () {
        MenuControllerGenerator.controller.canControlCue = true;

        Ray ray = new Ray (ballController.transform.position + 3.0f * ballRadius * Vector3.up, -Vector3.up);
        RaycastHit hit;

        if(Physics.SphereCast (ray, 1.0f * ballRadius, out hit, 1000.0f, canvasAndBallMask)) {

            hitCanvas = hit.collider.gameObject.layer == LayerMask.NameToLayer ("Canvas");
        } else {
            hitCanvas = false;
        }

        ballController.ballIsSelected = false;
        canMoveBall = false;
        cueFSMController.setMoveInTable ();


        if(hitCanvas) {
            transform.position = ballController.transform.position;
        } else {
            ballController.RessetPosition (ballSelectPosition, true);
        }

        ballController.GetComponent<Collider> ().enabled = true;
        ballController.GetComponent<Rigidbody> ().useGravity = true;
        ballController.GetComponent<Rigidbody> ().isKinematic = false;

        if(ServerController.serverController && ServerController.serverController.isMyQueue && !MenuControllerGenerator.controller.playWithAI) {
            ServerController.serverController.SendRPCToServer ("ForceSetBallMove", ServerController.serverController.otherNetworkPlayer, ballController.id, ballController.GetComponent<Rigidbody> ().position, Vector3.zero, Vector3.zero);
        }
        StopCoroutine ("WaitAndShowLineAndSphere");
        StartCoroutine ("WaitAndShowLineAndSphere", true);
    }


    void CheckAllIsSleeping () {
        if(checkInProgress)
            return;
        checkOthersIsSleeping = thenInshoting;
        checkAllIsSleeping = thenInshoting;

        if(thenInshoting) {
            foreach(BallController ballC in ballControllers) {
                if(ballC != ballController)
                    checkOthersIsSleeping = checkOthersIsSleeping && ballC.IsSleeping ();
            }
            checkAllIsSleeping = checkOthersIsSleeping && (ballController.IsSleeping () || ballController.ballIsOut);
            StartCoroutine (WaitAndCheckAllIsSleeping ());
        }

        if(!checkAllIsSleeping) {
            ballsAudioPlayingCount = 0;
            foreach(BallController item in ballControllers) {
                if(item.GetComponent<AudioSource> ().isPlaying)
                    ballsAudioPlayingCount++;
            }
        }
    }
    IEnumerator WaitAndCheckAllIsSleeping () {
        checkInProgress = true;
        yield return new WaitForSeconds (1.0f);

        bool _allIsSleeping = thenInshoting;
        bool _othersSleeping = thenInshoting;
        foreach(BallController ballC in ballControllers) {

            if(ballC != ballController) {
                _othersSleeping = _othersSleeping && ballC.IsSleeping ();
                _allIsSleeping = _allIsSleeping && ballC.IsSleeping ();

            } else {
                _allIsSleeping = _allIsSleeping && (ballController.IsSleeping () || ballController.ballIsOut);
            }
        }
        if(_allIsSleeping == checkAllIsSleeping) {
            allIsSleeping = _allIsSleeping;
        }
        if(_othersSleeping == checkOthersIsSleeping) {
            othersSleeping = _othersSleeping;
        }
        checkInProgress = false;
    }
    void CreateAndSortBalls () {
        ballRadius = 0.5f * ballControllerPrefab.transform.lossyScale.x;
        float newBallRadius = ballRadius + ballsDistance;
        ballsCount = ballTextures.Length;

        startBallControllers = new BallController[ballsCount];

        for(int i = 0; i < ballsCount; i++) {
            BallController bc = null;
            float deltaX = deltaPositions[i].x;
            float deltaZ = deltaPositions[i].y;
            Vector3 position = i == 0 ? mainBallPoint.position : firstBallPoint.position +
                new Vector3 (deltaX * Mathf.Sqrt (Mathf.Pow (2.0f * newBallRadius, 2.0f) - Mathf.Pow (newBallRadius, 2.0f)), 0.0f, deltaZ * newBallRadius);


            bc = BallController.Instantiate (ballControllerPrefab) as BallController;

            bc.transform.position = position;
            bc.transform.parent = ballsParent;

            bc.isMain = i == 0;
            if(i == 0) {
                bc.ballType = 0;
                ballController = bc;
                ballController.gameObject.layer = LayerMask.NameToLayer ("MainBall");
            } else {
                bc.gameObject.layer = LayerMask.NameToLayer ("Ball");
                if(!ServerController.serverController._isModeCard) {
                    if(i == 8) {
                        bc.isBlack = true;
                        bc.ballType = 2;
                    } else if(i >= 1 && i <= 7) {
                        bc.ballType = 1;
                    } else {
                        bc.ballType = -1;
                    }
                }
            }
            bc.id = i;
            bc.cueController = this;
            bc.GetComponent<Renderer> ().material.mainTexture = ballTextures[i];
            bc.ballReflaction.GetComponent<Renderer> ().material.mainTexture = ballTextures[i];
            bc.ballReflaction.parent = ballsReflactionParent;
            bc.ballReflaction.localScale = bc.transform.localScale;
            ballControllers.Add (bc);
            startBallControllers[i] = bc;
        }

    }


    IEnumerator WaitAndShowLineAndSphere (bool isFirst) {
        yield return new WaitForEndOfFrame ();
        while(ballIsOut) {
            yield return null;
        }
        if(ServerController.serverController) {
            while(!networkAllIsSleeping) {
                yield return null;
            }
        }
        if(allIsSleeping)
            ShowLineAndSphere (isFirst);
    }
    public void OnBallIsOut (bool isOut) {
        ballIsOut = isOut;
        ballController.ballIsOut = ballIsOut;
    }

    void SetOnChengSleeping () {
        //Set Some On Cheng Sleeping
        oldAllIsSleeping = allIsSleeping;
    }

    void DrawLinesAndSphere () {
        if(!ballController.ballIsSelected && allIsSleeping && (!ServerController.serverController || networkAllIsSleeping || MenuControllerGenerator.controller.playWithAI))
            OnDrawLinesAndSphere ();
    }
    IEnumerator WaitAndUncheckForceSlider () {
        yield return new WaitForSeconds (0.3f);

        if(!cueForceisActive)
            inTouchForceSlider = false;
    }
    IEnumerator WaitAndRotateCue (Vector3 hitPoint) {
        yield return new WaitForSeconds (0.1f);
        if(MenuControllerGenerator.controller.canRotateCue)
            cuePivot.LookAt (hitPoint + ballRadius * Vector3.up);
    }
    public void ControleCueThenDraw () {
        if(MenuControllerGenerator.controller.canControlCue) {
            if(!MenuControllerGenerator.controller.isTouchScreen || menu.GetButton ()) {

                Ray ray = currentCamera.ScreenPointToRay (menu.GetScreenPoint ());
                RaycastHit hit;
                if(Physics.Raycast (ray, out hit, 1000.0f, canvasMask)) {
                    if(menu.GetButtonDown () && allIsSleeping) {
                        if(!MenuControllerGenerator.controller.isTouchScreen) {
                            cue2DStartPosition = hit.point;
                        } else {
                            StartCoroutine (WaitAndRotateCue (hit.point));
                            inTouchForceSlider = true;
                            StartCoroutine (WaitAndUncheckForceSlider ());
                        }
                    }


                    if((!menu.GetButton () || (MenuControllerGenerator.controller.isTouchScreen)) && !menu.GetButtonUp () && allIsSleeping && !inTouchForceSlider) {
                        if(MenuControllerGenerator.controller.isTouchScreen) {
                            Vector3 cuePivotScreenPoint = currentCamera.WorldToScreenPoint (cuePivot.position);
                            float orientY = menu.GetScreenPoint ().y - cuePivotScreenPoint.y > 0.0f ? 1.0f : -1.0f;
                            float orientX = menu.GetScreenPoint ().x - cuePivotScreenPoint.x > 0.0f ? 1.0f : -1.0f;
                            float speed = orientY * menu.MouseScreenSpeed.x - orientX * menu.MouseScreenSpeed.y;

                            touchRotateAngle = Mathf.Lerp (touchRotateAngle,
                                                          touchSensitivity * speed * Mathf.Abs (speed) * Time.deltaTime, 10.0f * Time.deltaTime);

                            cuePivot.Rotate (Vector3.up, touchRotateAngle);
                        } else {
                            cuePivot.LookAt (hit.point + ballRadius * Vector3.up);
                        }
                        cueDisplacement = 0.0f;
                    }

                    if(menu.GetButton ()) {
                        if(!MenuControllerGenerator.controller.isTouchScreen)
                            cueDisplacement = cueMaxDisplacement * Mathf.Clamp01 (Vector3.Dot (hit.point - cue2DStartPosition, (cuePivot.position - ballRadius * Vector3.up - cue2DStartPosition).normalized) / cueMaxDisplacement);
                        cueForceValue = 1.0f;
                    }
                }
            }

            if(menu.GetButtonUp ()) {
                MenuControllerGenerator.controller.canRotateCue = true;
                CheckShotCue ();
            }
        } else {
            cueDisplacement = 0.0f;
        }

        if(menu.GetButtonUp ()) {
            MenuControllerGenerator.controller.canControlCue = true;
        }
    }

    public void CheckShotCue () {
        cueForceValue = Mathf.Clamp (cueDisplacement / cueMaxDisplacement, 0.01f, 1.0f);
        inTouchForceSlider = false;
        cueForceisActive = false;
        if(cueForceValue > 0.011f) {
            ShotCue ();
        } else {
            cueForceValue = 1.0f;
            cueRotation.localPosition = cueRotationStrLocalPosition;
        }
        if(MenuControllerGenerator.controller.isTouchScreen) {
            //(CueForce.FindObjectOfType(typeof(CueForce)) as CueForce).Resset();
            if(cueForce.gameObject.activeInHierarchy)
                cueForce.Resset ();
        }
    }

    void SendCueControlToNetwork () {
        sendTime += Time.deltaTime;
        if(sendTime > 1.0f / 10.0f/*Network.sendRate*/) {
            sendTime = 0.0f;
            if(cueRotationLocalPosition != cueRotation.localPosition || cuePivotLocalRotation != cuePivot.localRotation) {
                cueRotationLocalPosition = cueRotation.localPosition;
                cuePivotLocalRotation = cuePivot.localRotation;
                ServerController.serverController.SendRPCToServer ("SendCueControl", ServerController.serverController.otherNetworkPlayer, cuePivot.localRotation, cueRotation.localPosition, new Vector3 (rotationDisplacement.x, rotationDisplacement.y, 0.0f));
            }
        }
    }
    public void SetCueControlFromNetwork (Quaternion localRotation, Vector3 localPosition, Vector2 displacement) {
        cuePivotLocalRotation = localRotation;
        cueRotationLocalPosition = localPosition;
        cueBallPivotLocalPosition = new Vector3 (displacement.x * cueBallPivot.radius, displacement.y * cueBallPivot.radius, cueBallPivot.transform.localPosition.z);
    }
    void SendCueControl (Quaternion localRotation, Vector3 localPosition) {
        cuePivot.localRotation = Quaternion.Lerp (cuePivot.localRotation, localRotation, 10.0f * Time.deltaTime);
        cueRotation.localPosition = Vector3.Lerp (cueRotation.localPosition, localPosition, 10.0f * Time.deltaTime);
        cueBallPivot.transform.localPosition = Vector3.Lerp (cueBallPivot.transform.localPosition, cueBallPivotLocalPosition, 10.0f * Time.deltaTime);
    }
    public void OnDrawLinesAndSphere () {
        if(!ServerController.serverController || (ServerController.serverController.isMyQueue && networkAllIsSleeping)) {
            ControleCueThenDraw ();
        }
        if(ServerController.serverController) {
            if(!MenuControllerGenerator.controller.playWithAI) {
                if(ServerController.serverController.isMyQueue)
                    SendCueControlToNetwork ();
                else
                    SendCueControl (cuePivotLocalRotation, cueRotationLocalPosition);
            }
            checkMyBall = false;
        }

        currentHitBallController = null;
        if(allIsSleeping)
            transform.position = ballController.transform.position;
        ballVelocityOrient = VectorOperator.getProjectXZ (cuePivot.forward, true);

        float lostEnergy = 1.0f - Mathf.Sqrt ((Mathf.Pow (rotationDisplacement.y, 2.0f) + Mathf.Pow (rotationDisplacement.x, 2.0f)));
        lostEnergy = Mathf.Clamp (lostEnergy, 0.75f, 1.0f);


        ballShotVelocity = ballMaxVelocity * cueForceValue * lostEnergy * ballVelocityOrient;
        ballShotAngularVelocity = ballAngularVelocityCurve.Evaluate (cueForceValue) * Mathf.Pow (cueForceValue, 2.0f) * 250.0f * (Mathf.Clamp (rotationDisplacement.y, -1.0f, 1.0f) * cuePivot.right - rotationDisplacement.x * cuePivot.up); ;

        Ray firstRey = new Ray (cuePivot.position, ballVelocityOrient);
        RaycastHit firstHit;

        if(Physics.SphereCast (firstRey, 0.99f * ballRadius, out firstHit, 1000.0f, wallAndBallMask)) {
            collisionSphere.position = firstHit.point + ballRadius * firstHit.normal;

            Vector3	outVelocity = VectorOperator.getProjectXZ (ballShotVelocity, true);

            secondVelocity = VectorOperator.getBallVelocity (ballRadius, outVelocity, collisionSphere.position,
                                                            wallAndBallMask, 20.0f * ballRadius, ref hitBallVelocity, ref hitCollider, rotationDisplacement.x);

            currentHitBallController = hitCollider.GetComponent<BallController> ();
            ballCollisionLine.enabled = currentHitBallController && allIsSleeping;

            firstCollisionLine.SetVertexCount (2);
            firstCollisionLine.SetPosition (0, ballController.transform.position);
            firstCollisionLine.SetPosition (1, collisionSphere.position);

            secondCollisionLine.SetVertexCount (2);
            secondCollisionLine.SetPosition (0, collisionSphere.position);
            float angle1 = secondVelocity.magnitude / ballMaxVelocity;
            secondCollisionLine.SetPosition (1, collisionSphere.position + (angle1 * ballLineLength + 0.7f * ballRadius) * secondVelocity.normalized);

            if(currentHitBallController) {
                ballCollisionLine.SetVertexCount (2);
                ballCollisionLine.SetPosition (0, currentHitBallController.GetComponent<Rigidbody> ().position);
                Vector3 hbvOrient = (currentHitBallController.GetComponent<Rigidbody> ().position - collisionSphere.position).normalized;
                float angle2 = Mathf.Abs (Vector3.Dot (hbvOrient, cuePivot.forward));
                ballCollisionLine.SetPosition (1, currentHitBallController.GetComponent<Rigidbody> ().position + (angle2 * ballLineLength + 0.99f * ballRadius) * hbvOrient);
            } else {
                ballCollisionLine.SetVertexCount (0);
            }
        }

        if(ServerController.serverController && !ServerController.serverController.isModeCard) {
            if((gameManager.tableIsOpened && (!currentHitBallController || !currentHitBallController.isBlack)) || !currentHitBallController ||
               (currentHitBallController.isBlack && gameManager.ballType != 0 && (ServerController.serverController.isMyQueue ? gameManager.remainedBlackBall : gameManager.otherRemainedBlackBall))) {
                checkMyBall = true;
            } else {
                checkMyBall = !currentHitBallController.isBlack &&
                    (ServerController.serverController.isMyQueue ? currentHitBallController.ballType == gameManager.ballType :
                     currentHitBallController.ballType != gameManager.ballType);
            }

            if(oldCheckMyBall != checkMyBall) {
                oldCheckMyBall = checkMyBall;
                collisionSphere.GetComponent<Renderer> ().sharedMaterial.mainTexture = checkMyBall ? collisionBall : collisionBallRed;
            }
        }
    }
    void HideLineAndSphere () {
        OnHideLineAndSphere ();
    }
    public void OnHideLineAndSphere () {
        collisionSphere.GetComponent<Renderer> ().enabled = false;
        firstCollisionLine.enabled = false;
        secondCollisionLine.enabled = false;
        ballCollisionLine.enabled = false;
        cuePivot.gameObject.SetActive (false);
        cueForce.gameObject.SetActive (false);
        cueCircularSlider.gameObject.SetActive (false);
    }

    void ShowLineAndSphere (bool isFirst) {
        if(ServerController.serverController && ServerController.serverController.isMyQueue) {
            ServerController.serverController.SendRPCToServer ("SendCueControl", ServerController.serverController.otherNetworkPlayer, cuePivot.localRotation, cueRotationStrLocalPosition, Vector3.zero);
        }
        if(isFirst)
            OnShowLineAndSphereFirstTime ();
        else
            OnShowLineAndSphere ();
    }
    public void OnShowLineAndSphereFirstTime () {
        MenuControllerGenerator.controller.canControlCue = true;
        cueForceValue = 1.0f;
        collisionSphere.GetComponent<Renderer> ().enabled = true;
        firstCollisionLine.enabled = true;
        secondCollisionLine.enabled = true;
        ballCollisionLine.enabled = true;

        cuePivot.gameObject.SetActive (true);

        //if(ServerController.serverController.isMyQueue) {
        cueForce.gameObject.SetActive (true);
        cueCircularSlider.gameObject.SetActive (true);
        cueForce.Resset ();
        //}
    }
    public void OnShowLineAndSphere () {
        foreach(BallController item in ballControllers) {
            if(item.isMain)
                StartCoroutine (WaitWhenResse (item));
            else
                item.GetComponent<Rigidbody> ().Sleep ();
        }
        OnShowLineAndSphereFirstTime ();
    }

    IEnumerator WaitWhenResse (BallController item) {
        yield return new WaitForSeconds (0.5f);
        if(allIsSleeping)
            item.GetComponent<Rigidbody> ().Sleep ();
    }
    public void OnControlCue () {
        if(ServerController.serverController && !ServerController.serverController.isMyQueue && !MenuControllerGenerator.controller.playWithAI)
            return;
        float x = ballRadius * cueBallPivot.transform.localPosition.x / cueBallPivot.radius;
        float y = ballRadius * cueBallPivot.transform.localPosition.y / cueBallPivot.radius;

        rotationDisplacement = (1.0f / (ballRadius)) * (new Vector2 (x, y));

        float z = -Mathf.Sqrt (Mathf.Clamp (Mathf.Pow (ballRadius, 2.0f) - (Mathf.Pow (x, 2.0f) + Mathf.Pow (y, 2.0f)), 0.0f, Mathf.Pow (ballRadius, 2.0f)));

        if(!shotingInProgress) {
            checkCuePosition = new Vector3 (x, y, z);
        }


        cueRotation.localPosition = checkCuePosition - cueDisplacement * Vector3.forward;
    }

    void ShotCue () {
        OnShotCue ();
        if(ServerController.serverController && ServerController.serverController.isMyQueue && !MenuControllerGenerator.controller.playWithAI)
            ServerController.serverController.SendRPCToServer ("OnShotCue", ServerController.serverController.otherNetworkPlayer);
    }
    IEnumerator WaitAndSendOnShotCue () {
        yield return new WaitForSeconds (0.5f);
        ServerController.serverController.SendRPCToServer ("OnShotCue", ServerController.serverController.otherNetworkPlayer);
    }
    private float shotTime = 0.0f;
    //Player is shot (for cue)
    public void OnShotCue () {
        if(!allIsSleeping)
            return;
        timeAfterShot = 0.0f;

        shotTime = 0.0f;
        isFirsTime = false;

        HideLineAndSphere ();
        allIsSleeping = false;
        othersSleeping = false;
        inMove = true;

        shotingInProgress = true;
        thenInshoting = false;
        OnBallIsOut (false);
       if(!ServerController.serverController.isModeCard) {
            StopCoroutine ("WaitWhenAllIsSleeping");
            StartCoroutine ("WaitWhenAllIsSleeping");
       } else {
           StopCoroutine ("WaitWhenAllIsSleeping2");
           StartCoroutine ("WaitWhenAllIsSleeping2");
       }

    }

    //When all balls is sleeping 
    IEnumerator WaitWhenAllIsSleeping () {
        if(ServerController.serverController) {
            if(!MenuControllerGenerator.controller.playWithAI) {
                networkAllIsSleeping = false;
            }
            gameManager.ballsInMove = true;
        }
        while(ballController.ballIsSelected || !allIsSleeping || ballIsOut || ballController.ballIsOut) {
            yield return null;
        }

        if(ServerController.serverController) {
            if(!MenuControllerGenerator.controller.playWithAI) {
                ServerController.serverController.SendRPCToServer ("OnChanghAllIsSleeping", ServerController.serverController.otherNetworkPlayer);
            }
            bool isMyQueue = ServerController.serverController.isMyQueue;

            if(gameManager.needToChangeQueue) {
                ServerController.serverController.isMyQueue = false;
            }

            if(MenuControllerGenerator.controller.playWithAI) {
                networkAllIsSleeping = true;
            }
            if(isMyQueue || MenuControllerGenerator.controller.playWithAI) {
                if(!MenuControllerGenerator.controller.playWithAI) {
                    while(!networkAllIsSleeping) {
                        foreach(BallController item in ballControllers) {
                            if(networkAllIsSleeping)
                                break;
                            item.CancelInvokeSetBallCollisionData ();
                            ServerController.serverController.SendRPCToServer ("SetBallSleeping", ServerController.serverController.otherNetworkPlayer, item.id, item.GetComponent<Rigidbody> ().position);
                            yield return null;
                        }
                    }
                }

                if(gameManager.needToForceChangeQueue || gameManager.needToChangeQueue) {

                    while(!networkAllIsSleeping) {
                        yield return null;
                    }
                    if((gameManager.isFirstShot && gameManager.firstShotHitCount < 4) || gameManager.setMoveInTable) {
                        if(isMyQueue) {
                            ServerController.serverController.SendRPCToServer ("SetMoveInTable", ServerController.serverController.otherNetworkPlayer);
                        } else {
                            cueFSMController.setMoveInTable ();
                        }

                        string gameInfoErrorText = "";
                        string  otherGameInfoErrorText = "";

                        if(gameManager.gameInfoErrorText == "") {
                            if(gameManager.isFirstShot && gameManager.firstShotHitCount < 4) {
                                gameInfoErrorText = "Bạn đánh lỗi! Tới lượt " + ServerController.serverController.otherName + ".";
                                otherGameInfoErrorText = (MenuControllerGenerator.controller.playWithAI ? ServerController.serverController.otherName : ServerController.serverController.myName) +
                                    " đánh lỗi! " + "Tới lượt bạn.";
                            } else if(!gameManager.isFirstShot && gameManager.setMoveInTable) {
                                if(gameManager.tableIsOpened) {
                                    gameInfoErrorText = "Bạn đánh lỗi!" + " Tới lượt " + ServerController.serverController.otherName + ".";
                                    otherGameInfoErrorText = (MenuControllerGenerator.controller.playWithAI ? ServerController.serverController.otherName : ServerController.serverController.myName)
                                        + " đánh lỗi" + " Tới lượt bạn.";
                                } else {
                                    gameInfoErrorText = "Đánh lỗi! Tới lượt " + ServerController.serverController.otherName + ".";
                                    otherGameInfoErrorText = "Đánh lỗi! Tới lượt bạn.";
                                }
                            }
                        } else {
                            gameInfoErrorText = "Bạn " + gameManager.gameInfoErrorText + "! Tới lượt " + ServerController.serverController.otherName + ".";
                            otherGameInfoErrorText = (MenuControllerGenerator.controller.playWithAI ? ServerController.serverController.otherName : ServerController.serverController.myName)
                                + "  " + gameManager.gameInfoErrorText + "! Tới lượt bạn.";
                        }
                        if(isMyQueue) {
                            gameManager.ShowGameInfoError (gameInfoErrorText);
                        } else {
                            gameManager.ShowGameInfoError (otherGameInfoErrorText);
                        }
                        if(!MenuControllerGenerator.controller.playWithAI) {
                            ServerController.serverController.SendRPCToServer ("SetErrorText", ServerController.serverController.otherNetworkPlayer, otherGameInfoErrorText);
                        }

                    }
                    if(!(gameManager.blackBallInHolle || gameManager.otherBlackBallInHolle)) {
                        if(MenuControllerGenerator.controller.playWithAI && !isMyQueue)
                            ServerController.serverController.ChangeQueue (true);
                        else
                            ServerController.serverController.ChangeQueue (false);
                    }
                }
            }
            if(gameManager.needToForceChangeQueue || gameManager.needToChangeQueue) {

            } else {

            }

            if(gameManager.blackBallInHolle || gameManager.otherBlackBallInHolle) {
                if((isMyQueue || MenuControllerGenerator.controller.playWithAI)) {
                    if(gameManager.mainBallIsOut) {
                        if(isMyQueue) {
                            gameManager.ShowGameInfoError ("Bạn đánh bóng đen vào lỗ!");
                        } else {
                            gameManager.ShowGameInfoError (ServerController.serverController.otherName + " đánh bóng đen vào lỗ!");
                        }

                        if(!MenuControllerGenerator.controller.playWithAI) {
                            ServerController.serverController.SendRPCToServer ("SetErrorText", ServerController.serverController.otherNetworkPlayer, ServerController.serverController.myName + " đánh bóng đen vào lỗ!");
                        }
                    } else {
                        if(isMyQueue) {
                            if(gameManager.firstHitBall && !gameManager.firstHitBall.isBlack && gameManager.afterRemainedBlackBall && gameManager.ballType != gameManager.firstHitBall.ballType) {
                                gameManager.HideGameInfoError ();
                                gameManager.ShowGameInfoError ("cần đánh 1 " + (gameManager.ballType == 1 ? "trơn" : "sọc") + " hoặc bi đen.");
                                if(!MenuControllerGenerator.controller.playWithAI) {
                                    ServerController.serverController.SendRPCToServer ("SetErrorText", ServerController.serverController.otherNetworkPlayer, ServerController.serverController.myName + " cần đánh 1 " + (gameManager.ballType == 1 ? "trơn" : "sọc") + " hoặc bi đen.");
                                }
                            }
                        } else {
                            if(gameManager.firstHitBall && !gameManager.firstHitBall.isBlack && gameManager.afterOtherRemainedBlackBall && gameManager.ballType == gameManager.firstHitBall.ballType) {
                                gameManager.HideGameInfoError ();
                                gameManager.ShowGameInfoError (ServerController.serverController.otherNetworkPlayer + " cần đánh 1 " + (gameManager.ballType == -1 ? "trơn" : "sọc") + " hoặc bi đen.");
                            }
                        }

                    }
                }

                gameManager.ActivateMenuButtons (true);
            }

            if(gameManager.remainedBlackBall) {
                gameManager.afterRemainedBlackBall = true;
            }
            if(gameManager.otherRemainedBlackBall) {
                gameManager.afterOtherRemainedBlackBall = true;
            }



            gameManager.isFirstShot = false;
            gameManager.setMoveInTable = true;
            gameManager.needToChangeQueue = true;
            gameManager.needToForceChangeQueue = false;
            gameManager.firstHitBall = null;
            gameManager.gameInfoErrorText = "";
            gameManager.ballsInMove = false;

            gameManager.mainBallIsOut = false;
            gameManager.otherMainBallIsOut = false;

            RessetShotOptions ();

            if(MenuControllerGenerator.controller.playWithAI && !isMyQueue) {
                ServerController.serverController.serverMessenger.ShotWithAI ();
            }
        }
    }

    IEnumerator WaitWhenAllIsSleeping2 () {
        if(ServerController.serverController) {
            if(!MenuControllerGenerator.controller.playWithAI) {
                networkAllIsSleeping = false;
            }
            gameManager.ballsInMove = true;
        }
        while(ballController.ballIsSelected || !allIsSleeping || ballIsOut || ballController.ballIsOut) {
            yield return null;
        }

        if(ServerController.serverController) {
            if(!MenuControllerGenerator.controller.playWithAI) {
                ServerController.serverController.SendRPCToServer ("OnChanghAllIsSleeping", ServerController.serverController.otherNetworkPlayer);
            }
            bool isMyQueue = ServerController.serverController.isMyQueue;

            if(gameManager.needToChangeQueue) {
                ServerController.serverController.isMyQueue = false;
            }

            if(MenuControllerGenerator.controller.playWithAI) {
                networkAllIsSleeping = true;
            }
            if(isMyQueue || MenuControllerGenerator.controller.playWithAI) {
                if(!MenuControllerGenerator.controller.playWithAI) {
                    while(!networkAllIsSleeping) {
                        foreach(BallController item in ballControllers) {
                            if(networkAllIsSleeping)
                                break;
                            item.CancelInvokeSetBallCollisionData ();
                            ServerController.serverController.SendRPCToServer ("SetBallSleeping", ServerController.serverController.otherNetworkPlayer, item.id, item.GetComponent<Rigidbody> ().position);
                            yield return null;
                        }
                    }
                }

                if(gameManager.needToForceChangeQueue || gameManager.needToChangeQueue) {

                    while(!networkAllIsSleeping) {
                        yield return null;
                    }
                    if((gameManager.isFirstShot && gameManager.firstShotHitCount < 4) || gameManager.setMoveInTable) {
                        if(isMyQueue) {
                            ServerController.serverController.SendRPCToServer ("SetMoveInTable", ServerController.serverController.otherNetworkPlayer);
                        } else {
                            cueFSMController.setMoveInTable ();
                        }
                    }
                }
            }

            gameManager.isFirstShot = false;
            gameManager.setMoveInTable = true;
            gameManager.needToChangeQueue = true;
            gameManager.needToForceChangeQueue = false;
            gameManager.firstHitBall = null;
            gameManager.gameInfoErrorText = "";
            gameManager.ballsInMove = false;

            gameManager.mainBallIsOut = false;
            gameManager.otherMainBallIsOut = false;

            RessetShotOptions ();

            if(MenuControllerGenerator.controller.playWithAI && !isMyQueue) {
                ServerController.serverController.serverMessenger.ShotWithAI ();
            }
        }
    }


    public void OnPlayBallSound (float volume) {
        ballController.GetComponent<AudioSource> ().volume = volume;
        ballController.GetComponent<AudioSource> ().Play ();
    }
    public void OnPlayCueSound (float volume) {
        GetComponent<AudioSource> ().volume = volume;
        GetComponent<AudioSource> ().Play ();
    }

    void UpdateShotCue () {
        shotTime += Time.deltaTime;
        if(shotingInProgress && Vector3.Distance (cueRotation.localPosition, checkCuePosition + cueRotationStrLocalPosition) < 0.1f * ballRadius) {
            cueRotation.localPosition = checkCuePosition + cueRotationStrLocalPosition;
            shotingInProgress = false;

            cueDisplacement = 0.0f;

            foreach(BallController item in ballControllers) {
                item.inMove = !allIsSleeping;
            }
            StartCoroutine (WaitAndSetThenInshoting ());
            if(!ServerController.serverController || ServerController.serverController.isMyQueue || MenuControllerGenerator.controller.playWithAI) {
                ballController.ShotBall ();
                if(ServerController.serverController && !MenuControllerGenerator.controller.playWithAI)
                    ServerController.serverController.SendRPCToServer ("ShotBall", ServerController.serverController.otherNetworkPlayer, ballShotVelocity, hitBallVelocity, secondVelocity, ballShotAngularVelocity);
            }
        } else {
            cueRotation.localPosition = Vector3.Lerp (cueRotation.localPosition, checkCuePosition + cueRotationStrLocalPosition, 100.0f * (0.9f * cueForceValue + 0.1f) * Time.deltaTime);
        }
    }
    public void SetWhenShotCue () {

    }
    IEnumerator WaitAndSendShotBall () {
        yield return new WaitForSeconds (0.5f + shotTime);
        ServerController.serverController.SendRPCToServer ("ShotBall", ServerController.serverController.otherNetworkPlayer, ballShotVelocity, hitBallVelocity, secondVelocity, ballShotAngularVelocity);
    }
    IEnumerator WaitAndSetThenInshoting () {
        yield return new WaitForSeconds (3.0f);
        thenInshoting = true;
    }
    void SetCamera (Button btn) {
        is3D = btn.state;
        lights.enabled = is3D;
        currentCamera = is3D ? camera3D : camera2D;
        camera3D.enabled = false;
        //camera2D.enabled = false;
        currentCamera.enabled = true;
        cameraCircularSlider.SetActive (btn.state);
        PlayerPrefs.SetInt ("Current Camera", btn.state ? 1 : 0);
    }
    public void RessetShotOptions () {
        cueBallPivot.Reset ();
        if(ballController.ballIsSelected) {
            OnUnselectBall ();
        }
    }

    public CircularSlider cueCircularSlider;
    float rotation = 0.0f;
    public void changeRotateCue (CircularSlider circularSlider) {
        MenuControllerGenerator.controller.canControlCue = false;
        //transform.Rotate (Vector3.up, -rotationSpeed * camera3dSlider.displacementZ * Time.deltaTime);
        rotation -= circularSlider.displacementX * Time.deltaTime;
        //rotation = Mathf.Clamp (rotation, minAngle, maxAngle);
        //rotator.localRotation = Quaternion.Euler (rotation, 0.0f, 0.0f);

        //float orientY = menu.GetScreenPoint ().y - cuePivotScreenPoint.y > 0.0f ? 1.0f : -1.0f;
        //float orientX = menu.GetScreenPoint ().x - cuePivotScreenPoint.x > 0.0f ? 1.0f : -1.0f;
        //float speed = orientY * menu.MouseScreenSpeed.x - orientX * menu.MouseScreenSpeed.y;

        //touchRotateAngle = Mathf.Lerp (touchRotateAngle,
        //                              touchSensitivity * speed * Mathf.Abs (speed) * Time.deltaTime, 10.0f * Time.deltaTime);

        //cuePivot.Rotate (Vector3.up, touchRotateAngle);

       float rotateAngle = Mathf.Lerp (rotation, 10, 45);
        //-5 * circularSlider.displacementX * Time.deltaTime
       cuePivot.Rotate (Vector3.up, rotateAngle);
    }
}
