using UnityEngine;
using System.Collections;

public class PanelSMS : PanelGame {
    //public GameControl gameControl;
    public UILabel lb20, lb30;
    public GameObject item9022;
    public GameObject parent;
    // Use this for initialization
    void Start () {
        if(lb20 != null || lb30 != null) {
            lb20.text = BaseInfo.formatMoneyDetailDot (BaseInfo.gI ().sms10) + " " + Res.MONEY_UNIT;
            lb30.text = BaseInfo.formatMoneyDetailDot (BaseInfo.gI ().sms15) + " " + Res.MONEY_UNIT;
        }
    }

    // Update is called once per frame
    void Update () {

    }

    public void NhanTin (string sms, string dauso, string thongbao) {
        GameControl.instance.panelYesNo.onShow (thongbao, delegate {
#if UNITY_EDITOR
            GameControl.instance.panelThongBao.onShow ("Soạn tin theo cú pháp: " + sms + " gửi đến " + dauso, delegate { });
#else
			GameControl.sendSMS(dauso, sms);
#endif
        });
    }

    public void onClick10 () {
        ////GameControl.instance.sound.startClickButtonAudio ();
        string tb = "Nhắn tin để nạp " + Res.MONEY_UNIT + " " + BaseInfo.formatMoneyDetailDot (BaseInfo.gI ().sms10) + " (phí 10k)?";
        string sms = BaseInfo.gI ().syntax10 + " " + BaseInfo.gI ().mainInfo.userid;
        string ds = BaseInfo.gI ().port10;
        this.NhanTin (sms, ds, tb);
    }

    public void onClick15 () {
        ////GameControl.instance.sound.startClickButtonAudio ();
        string tb = "Nhắn tin để nạp " + Res.MONEY_UNIT + " " + BaseInfo.formatMoneyDetailDot (BaseInfo.gI ().sms15) + " (phí 15k)?";
        string sms = BaseInfo.gI ().syntax15 + " " + BaseInfo.gI ().mainInfo.userid;
        string ds = BaseInfo.gI ().port15;
        this.NhanTin (sms, ds, tb);
    }

    public bool checkCount () {
        if(parent.transform.childCount == 0)
            return true;

        return false;
    }

    public void clear9029 () {
        foreach(Transform t in parent.transform) {
            Destroy (t.gameObject);
        }
    }

    public void onClick9029 (GameObject obj) {
       // //GameControl.instance.sound.startClickButtonAudio ();

        //GameObject obj = UICamera.currentTouch.pressed;

        string tb = "Nhắn tin để nạp " + BaseInfo.formatMoneyDetailDot (obj.GetComponent<Item9029> ().money) + " (phí " + obj.GetComponent<Item9029> ().name + ")?";
        string sms = obj.GetComponent<Item9029> ().sys + " " + BaseInfo.gI ().mainInfo.userid;
        string ds = obj.GetComponent<Item9029> ().port + "";
        this.NhanTin (sms, ds, tb);
    }

    public void add9022 (string name, string sys, short port, long money) {
        GameObject btnT = Instantiate (item9022) as GameObject;
        //btnT.transform.parent = tblContaint.transform;
        parent.GetComponent<UIGrid> ().AddChild (btnT.transform);
        btnT.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
        Vector3 vt = btnT.transform.localPosition;
        vt.z = -10;
        btnT.transform.localPosition = vt;

        btnT.GetComponent<Item9029> ().setText (name, sys, port, money);

        EventDelegate.Set (btnT.GetComponent<UIButton> ().onClick, delegate {
            onClick9029 (btnT);
        });
    }

}
