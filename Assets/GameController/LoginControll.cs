using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginControll : StageControl {
    //public MenuControllerGenerator mcg;

    public GameObject buttonGroup;
    public GameObject inputGroup;

    public UIInput inputUser, inputPass;


    void Awake () {
        inputUser.value = PlayerPrefs.GetString ("username");
        inputPass.value = PlayerPrefs.GetString ("password");
    }

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if(gameObject.activeInHierarchy && Input.GetKeyDown (KeyCode.Escape)) {
            onBack ();
        }
    }

    void login (string username, string pass) {

    }

    public void onClickChoiNgay () {
       // mcg.onClickAI ();
        gameControl.setStage (gameControl.main);
    }

    public void onClickLogin () {
        if(inputGroup.activeInHierarchy) {
            string inf = checkName (inputUser.value, inputPass.value);
            if(inf != "") {
                gameControl.panelThongBao.onShow (inf, delegate { });
            } else {
                gameControl.setStage (gameControl.main);
                PlayerPrefs.GetString ("username", inputUser.value);
                PlayerPrefs.SetString ("password", inputPass.value);
                PlayerPrefs.Save ();
            }
        } else {
            inputGroup.SetActive (true);
            buttonGroup.SetActive (false);
        }
    }

    public void onClickBack () {
        buttonGroup.SetActive (true);
        inputGroup.SetActive (false);
    }
    public void clickOnFacebook () {
        ////GameControl.instance.sound.startClickButtonAudio ();

    }

    public void clickSetting () {
        ////GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelSetting.onShow ();
    }

    private string checkName (string username, string pass) {
        string info = "";
        if(username == "" || pass == "")
            info = "Không được để trống!";
        //do dai phai nho hon 10 va lon hon 4
        if(username.Length > 10 || username.Length < 4)
            info = "Tên đăng nhập phải lớn hơn 4 và nhỏ hơn 10.";

        //ko co ky tu dac biet
        for(int i = 0; i < username.Length; i++) {
            char c = username[i];
            if(((c < '0') || (c > '9')) && (('A' > c) || (c > 'Z'))
                && (('a' > c) || (c > 'z'))) {
                info = "Không được có ký tự đặc biệt!";
            }
        }
        return info;
    }
    public override void onBack () {
        base.onBack ();
        Application.Quit ();
    }

    public void OnShowKeyBoard () {
#if UNITY_WP8
        if(Input.deviceOrientation != DeviceOrientation.Portrait && Input.deviceOrientation != DeviceOrientation.PortraitUpsideDown)
            TweenPosition.Begin (group, 0.1f, new Vector3 (260, 160, 0));
#endif
    }

    public void OnHideKeyBoard () {
        TweenPosition.Begin (inputGroup, 0.01f, new Vector3 (260, 0, 0));
    }
}
