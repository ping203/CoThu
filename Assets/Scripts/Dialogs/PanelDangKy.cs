using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelDangKy : PanelGame {
    public UIInput ip_name, ip_phone, ip_gift;
    public UIToggle tg_sex;

    public UISprite avata;

    public PanelChangeAvata panelChangeAvata;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void onRefresh () {
        ip_name.value = "";
        ip_phone.value = "";
        ip_gift.value = "";
    }

    public void clickChangeAvata () {
        ////GameControl.instance.sound.startClickButtonAudio ();
        panelChangeAvata.loadAva ();
        panelChangeAvata.onShow ();
    }

    public void onClickDongY () {
        if(ip_name.value.Length <= 0) {
            GameControl.instance.panelThongBao.onShow ("Tên hiển thị không được để trống!", delegate{});
            return;
        }
        if(ip_phone.value.Length <= 0) {
            GameControl.instance.panelThongBao.onShow ("Số điện thoại không được để trống!", delegate { });
            return;
        }

        if(ip_name.value.Length < 8 || ip_name.value.Length > 20) {
            GameControl.instance.panelThongBao.onShow ("Tên hiển thị phải lớn hơn 7 và nhỏ hơn 21 ký tự!", delegate { });
            return;
        }
        if(checkSDT (ip_phone.value) == -1) {
            GameControl.instance.panelThongBao.onShow ("Sai định dạng số điện thoại!", delegate { });
            return;
        }

        if(checkSDT (ip_phone.value) == -3) {
            GameControl.instance.panelThongBao.onShow ("Số điện thoại phải nhiều hơn 9 và ít hơn 12 ký tự!", delegate { });
            return;
        }
        sbyte sex = tg_sex.value ? (sbyte) 1 : (sbyte) 0;
       // //SendData.onLoginfirst (ip_phone.value, ip_name.value, sex, ip_gift.value, BaseInfo.gI().mainInfo.idAvata);
    }

    public int checkSDT (string sdt) {
        if(sdt.Length > 11 || sdt.Length < 10)
            return -3;

        for(int i = 0; i < sdt.Length; i++) {
            char c = sdt[i];
            if(('0' > c) || (c > '9')) {
                return -1;
            }
        }

        return 1;
    }
}
