using UnityEngine;
using System.Collections;

public class PanelThongBao : PanelGame {
    public UILabel lb_mess;
    public UIButton btnOK;
    //public Button btnCancel;
    public delegate void CallBack();
    public CallBack onClickOK;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void onShow(string mess, CallBack clickOK) {
        DoOnMainThread.ExecuteOnMainThread.Enqueue(() => {
            this.lb_mess.text = mess;
            btnOK.gameObject.SetActive(true);
            onClickOK = clickOK;
            onShow();
        });

    }
    public void onShowDCN(string mess, CallBack clickOK) {
        DoOnMainThread.ExecuteOnMainThread.Enqueue(() => {
            this.lb_mess.text = mess;
            btnOK.gameObject.SetActive(true);
            onClickOK = clickOK;
            onShow();
        });
    }
    public void onClickButtonOK() {
        onHide();
        onClickOK.Invoke();
    }

    //public void onShow(string mess) {
    //     lb_mess.text = mess;
    //     onShow();
    // }
}
