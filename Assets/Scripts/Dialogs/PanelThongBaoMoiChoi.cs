using UnityEngine;
using System.Collections;

public class PanelThongBaoMoiChoi : PanelGame {
    public UILabel label;
    public UIButton btnOK;
    public UIButton btnCancel;
    public delegate void CallBack ();
    public CallBack onClickOK;
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
    public void onShow (string mess) {
        DoOnMainThread.ExecuteOnMainThread.Enqueue (() => {
            label.text = mess;
            btnCancel.gameObject.SetActive (true);
            btnOK.gameObject.SetActive (false);
            onShow ();
        });

    }

    public void onShow (string mess, CallBack clickOK) {
        DoOnMainThread.ExecuteOnMainThread.Enqueue (() => {
            if(!GameControl.instance.login.gameObject.activeInHierarchy) {
                label.text = mess;
                //btnCancel.gameObject.SetActive(true);
                btnOK.gameObject.SetActive (true);
                onClickOK = clickOK;
                onShow ();
            }
        });
    }
    public void onShowDCN (string mess, CallBack clickOK) {
        DoOnMainThread.ExecuteOnMainThread.Enqueue (() => {
            label.text = mess;
            btnCancel.gameObject.SetActive (false);
            btnOK.gameObject.SetActive (true);
            onClickOK = clickOK;
            onShow ();
        });

    }

    public void onClickButtonOK () {
        //GameControl.instance.sound.startClickButtonAudio ();
        onHide ();
        onClickOK.Invoke ();
    }

    public void onClickCancelAll () {
        //GameControl.instance.sound.startClickButtonAudio ();
        BaseInfo.gI ().isNhanLoiMoiChoi = false;
        onHide ();
    }
}
