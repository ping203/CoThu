using UnityEngine;
using System.Collections;

public class PanelChangePassword : PanelGame {
    // Use this for initialization
    void Start () {

    }

    public void ok (string oldPass, string newPass1, string newPass2) {
        ////GameControl.instance.sound.startClickButtonAudio ();
        if(oldPass == "" || newPass1 == "" || newPass2 == "") {
            GameControl.instance.panelThongBao.onShow ("Bạn hãy nhập đủ thông tin.", delegate { });
            return;
        }

        if(oldPass != BaseInfo.gI ().pass) {
            GameControl.instance.panelThongBao.onShow ("Mật khẩu cũ không đúng.", delegate { });
            return;
        }

        if(newPass1 != newPass2) {
            GameControl.instance.panelThongBao.onShow ("Mật khẩu không giống nhau.", delegate { });
            return;
        }

        GameControl.instance.panelYesNo.onShow ("Bạn muốn gửi tin nhắn để đổi mật khẩu.", delegate {
            ////SendData.onGetPass (BaseInfo.gI ().mainInfo.nick);
        });
        onHide ();
    }
}
