using UnityEngine;
using System.Collections;

public class PanelInfoPlayer : PanelGame {
    public UILabel id;
    public UILabel name;
   // public UILabel xu;
    public UILabel chip;
    public UISprite spriteAvata;
    public PanelChangePassword panelChangePassword;
    public PanelChangeName panelChangeName;
    public PanelChangeAvata panelChangeAvata;
    public GameObject changePass, changeName, changeAvata;
    public UIInput ip_email, ip_phone;

    public GameObject itemTT, parentGrid;

    // Use this for initialization
    void Start () {
    }
    // Update is called once per frame
    void Update () {

    }

    public void clickChangePass () {
        ////GameControl.instance.sound.startClickButtonAudio ();
        panelChangePassword.onShow ();
    }

    public void clickChangeName () {
        //GameControl.instance.sound.startClickButtonAudio ();
        panelChangeName.onShow (BaseInfo.gI ().mainInfo.displayname);
    }

    public void clickChangeAvata () {
        //GameControl.instance.sound.startClickButtonAudio ();
        panelChangeAvata.loadAva ();
        panelChangeAvata.onShow ();
    }

    public void onClickEditMail () {
        string email = ip_email.value;
        if(email.Equals ("")) return;

        ////SendData.onUpdateProfile (email, BaseInfo.gI ().mainInfo.phoneNumber);
        BaseInfo.gI ().mainInfo.email = email;
    }

    public void onClickEditPhone () {
        string phone = ip_phone.value;
        if(phone.Equals ("")) return;
        if(phone.Length == 10 || phone.Length == 11
                            || phone.Length == 12) {
            //SendData.onUpdateProfile (BaseInfo.gI ().mainInfo.email, phone);
            BaseInfo.gI ().mainInfo.phoneNumber = phone;
        } else {
            GameControl.instance.panelThongBao.onShow ("Số điện thoại không đúng!", delegate {
            });
        }
    }

    public void infoMe () {
        string n = BaseInfo.gI ().mainInfo.displayname;
        long uid = BaseInfo.gI ().mainInfo.userid;
        long xuMe = 5000;//BaseInfo.gI ().mainInfo.moneyXu;
		long chipMe = ServerController.serverController.coins;//BaseInfo.gI ().mainInfo.moneyChip;
        string slt = BaseInfo.gI ().mainInfo.soLanThang;
        string slth = BaseInfo.gI ().mainInfo.soLanThua;
        int idAva = BaseInfo.gI ().mainInfo.idAvata;
        string link_ava = BaseInfo.gI ().mainInfo.link_Avatar;
        string email = BaseInfo.gI ().mainInfo.email;
        string phone = BaseInfo.gI ().mainInfo.phoneNumber;

        infoProfile (n, uid, xuMe, chipMe, slt, slth, link_ava, idAva, email, phone);
    }

    public void updateAvata () {
        int id = BaseInfo.gI ().mainInfo.idAvata;
        if(id != 0) {
            spriteAvata.spriteName = id + "";
        }
    }

    public void infoProfile (string nameinfo, long userid, long xuinfo, long chipinfo,
        string slthang, string slthua, string link_avata, int idAvata,
        string email, string phone) {
        bool isMe = false;
        if(nameinfo == BaseInfo.gI ().mainInfo.displayname) {
            isMe = true;
        }
        changePass.SetActive (isMe);
        changeName.SetActive (isMe);
        changeAvata.SetActive (isMe);
        ip_email.gameObject.SetActive (isMe);
        ip_phone.gameObject.SetActive (isMe);

        name.text = "Tên: " + nameinfo;
        id.text = "ID: " + userid;
       // xu.text = Res.MONEY_UNIT + ": " + BaseInfo.formatMoneyDetailDot (xuinfo);
        chip.text = Res.MONEY_UNIT + ": " + BaseInfo.formatMoneyDetailDot (chipinfo);

        ip_email.value = email;
        ip_phone.value = phone;

        //if(parentGrid.transform.childCount == 0) {
        //    string[] slt = slthang.Split (',');
        //    string[] slth = slthua.Split (',');
        //    for(int i = 0; i < slt.Length; i++) {
        //        GameObject obj = Instantiate (itemTT) as GameObject;
        //        parentGrid.GetComponent<UIGrid> ().AddChild (obj.transform);

        //        obj.transform.localScale = new Vector3 (1, 1, 1);
        //        obj.transform.localPosition = new Vector3 (0, 0, 0);

        //        obj.GetComponent<ItemThangThua> ().setText (slt, slth, i);
        //        //string[] kq = st [i].Split ('-');
        //        //label [i].text = st [i];
        //    }
        //}
        if(idAvata < 0) {
            WWW www = new WWW (link_avata);
            if(www.error != null) {
            } else {
                while(!www.isDone) {
                }
                spriteAvata.GetComponent<UISprite> ().enabled = false;
                spriteAvata.GetComponent<UITexture> ().enabled = true;
                spriteAvata.GetComponent<UITexture> ().mainTexture = www.texture;
            }
        } else {
            spriteAvata.GetComponent<UISprite> ().enabled = true;
            spriteAvata.GetComponent<UITexture> ().enabled = false;
            spriteAvata.spriteName = idAvata + "";
        }
    }
}
