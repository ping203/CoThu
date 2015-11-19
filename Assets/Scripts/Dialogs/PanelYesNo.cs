using UnityEngine;
using System.Collections;

public class PanelYesNo : PanelGame {
    public UILabel label;
    public UIButton btnOK;
    public UIButton btnCancel;
    public delegate void CallBack();
    public CallBack onClickOK;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public void onShow(string mess) {
        DoOnMainThread.ExecuteOnMainThread.Enqueue(() => {
            label.text = mess;
            btnCancel.gameObject.SetActive(true);
            btnOK.gameObject.SetActive(false);
            onShow();
        });

    }

    public void onShow(string mess, CallBack clickOK) {
        DoOnMainThread.ExecuteOnMainThread.Enqueue(() => {
            label.text = mess;
            btnCancel.gameObject.SetActive(true);
            btnOK.gameObject.SetActive(true);
            onClickOK = clickOK;
            onShow();
        });

    }
    public void onShowDCN(string mess, CallBack clickOK) {
        DoOnMainThread.ExecuteOnMainThread.Enqueue(() => {
            label.text = mess;
            btnCancel.gameObject.SetActive(false);
            btnOK.gameObject.SetActive(true);
            onClickOK = clickOK;
            onShow();
        });

    }
    public void onClickButtonOK () {
        //GameControl.instance.sound.startClickButtonAudio ();
        onHide();
        onClickOK.Invoke();
    }
}
