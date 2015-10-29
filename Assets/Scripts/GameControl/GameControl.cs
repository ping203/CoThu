using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
public class GameControl : MonoBehaviour {
    public static void sendSMS (string port, string content) {
#if UNITY_WP8
        UnityPluginForWindowPhone.Class1.sendSMS (port, content);
#else
        string str = content;
        if(content.Contains ("#")) {
            str = content.Replace ("#", "%23");
        }
        Application.OpenURL ("sms:" + port + @"?body=" + str);

#endif
    }
    private static GameControl _instance;
    public static GameControl instance {
        get {
            if(_instance == null) {
                _instance = new GameControl ();
                //_instance = GameObject.Find("bkgChung").GetComponent<GameControl>();

                //Tell unity not to destroy this object when loading a new scene!
                DontDestroyOnLoad (_instance.gameObject);
            }

            return _instance;
        }
    }

    public PanelWaiting panelWaiting;
    public PanelThongBao panelThongBao;
    public PanelYesNo panelYesNo;
    public PanelSetting panelSetting;
    public PanelInfoPlayer panelInfoPlayer;
    public PanelHelp panleHelp;
    public PanelNapChuyenXu panelNapChuyenXu;
    public PanelDoiThuong panelDoiThuong;
    public PanelMail panelMail;
    public PanelCreateRoom panelCreateRoom;
    public PanelToiBan panelToiBan;
    public PanelMoiChoi PanelMoiChoi;
    public PanelThongBaoMoiChoi panelThongBaoMoiChoi;
    public PanelChat panelChat;
    public PanelDatCuoc panelDatCuoc;
    public PanelRutTien panelRutTien;
    public PanelInput panelInput;
    public PanelCuoc panelCuoc;
    public PanelNotiDoiThuong panelNotiDoiThuong;
    public PanelDangKy panelDangKy;
    public PanelNhiemVu panelNhiemVu;

    public Toast toast;
    public LoginControll login;
    public RoomControl room;
    public MainControl main;

    public StageControl currenStage;
    public StageControl backState;

    public List<TableItem> listTableItem = new List<TableItem> ();

    public int gameID;

    public bool cancelAllInvite = false;

    public MenuController mcg;
    void Awake () {
        if(_instance == null) {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad (this);
        } else {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if(this != _instance)
                Destroy (this.gameObject);
        }
        //SoundManager.Get().startAudio(SoundManager.AUDIO_TYPE.BKG_MUSIC);
    }
    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 80;
        new ListernerServer (this);
        currenStage = login;
        login.gameObject.SetActive (true);
        main.gameObject.SetActive (false);
        room.gameObject.SetActive (false);
        // NetworkUtil.GI ().connect (//SendData.onGetPhoneCSKH ());
    }

    // Update is called once per frame
    void Update () {
    }
    public void setStage (StageControl stage) {
        if(currenStage != stage) {
            backState = currenStage;
        }
        if(currenStage != null) {
            currenStage.DisAppear ();
        }

        currenStage = stage;
        stage.Appear ();
    }

    internal void disableAllDialog () {
        panelWaiting.onHide ();
        panelThongBao.onHide ();
        panelYesNo.onHide ();
        panelSetting.onHide ();
        panelInfoPlayer.onHide ();
        panleHelp.onHide ();
        panelNapChuyenXu.onHide ();
        panelDoiThuong.onHide ();
        panelMail.onHide ();
        panelCreateRoom.onHide ();
        panelToiBan.onHide ();
        PanelMoiChoi.onHide ();
        panelThongBaoMoiChoi.onHide ();
        panelChat.onHide ();
        panelRutTien.onHide ();
        panelInput.onHide ();
        panelNotiDoiThuong.onHide ();
        panelCuoc.onHide ();
        panelDatCuoc.onHide ();
        panelDangKy.onHide ();
        panelNhiemVu.onHide ();
    }

    void OnApplicationQuit () {

    }

    public void resetGame () {
        main.gameObject.SetActive (false);
        room.gameObject.SetActive (false);
    }

    void OnApplicationPause (bool pauseStatus) {
        NetworkUtil.GI ().resume (pauseStatus);
    }

    void resume () {
        //NetworkUtil.GI().resume();
    }
}
