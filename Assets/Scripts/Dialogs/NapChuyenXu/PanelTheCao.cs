using UnityEngine;
using System.Collections;
using System;

public class PanelTheCao : PanelGame {
    public UIInput ip_masothe, ip_serithe;
    public UILabel lb_menh_gia_the;

    // Update is called once per frame
    void Update () {
    }

    void Start () {
    }

    public void clickNapTheCao (string tenMang) {
        //////GameControl.instance.sound.startClickButtonAudio ();
        //	OnSubmit ();
        int typeCard = -1;
        switch(tenMang) {
            case "Mobiphone":
                typeCard = 0;
                break;
            case "Vinaphone":
                typeCard = 1;
                break;
            case "Viettel":
                typeCard = 2;
                break;
        }
        if(ip_masothe.value == null
            || ip_masothe.value.Trim ().Equals ("")
            || ip_masothe.value.Length > 15) {
            GameControl.instance.panelThongBao
                    .onShow ("Mã số thẻ không hợp lệ!", delegate { });
            return;
        }

        if(/*typeCard != 4 &&*/ (ip_serithe.value.Trim ().Equals (""))) {
            GameControl.instance.panelThongBao
                    .onShow ("Bạn hãy nhập vào số Serial", delegate { });
            return;
        }
        GameControl.instance.panelThongBao
                .onShow ("Hệ thống đang xử lý!", delegate { });
    }

    public void TuChoi () {
        ////GameControl.instance.sound.startClickButtonAudio ();
        ip_masothe.value = "";
        ip_serithe.value = "";
    }
}
