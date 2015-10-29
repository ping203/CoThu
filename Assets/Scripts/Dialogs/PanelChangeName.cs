using UnityEngine;
using System.Collections;

public class PanelChangeName : PanelGame {
    public UILabel oldName;
    // Use this for initialization
    void Start () {

    }

    public void onShow (string name) {
        OnHideKeyBoard ();
        oldName.text = name;
        base.onShow ();
    }

    public void changeName (string tenmoi) {
        ////GameControl.instance.sound.startClickButtonAudio ();
        if(tenmoi != "") {
            if(tenmoi.Length >= 4 && tenmoi.Length <= 20) {
               // //SendData.onChangeName (tenmoi);
                onHide ();
            } else {
                GameControl.instance.panelThongBao.onShow ("Tên phải nhiều hơn 4 và ít hơn 20 kí tự.", delegate { });
            }
        } else {
            GameControl.instance.panelThongBao.onShow ("Nhập với tên mới.", delegate { });
        }
    }
}
