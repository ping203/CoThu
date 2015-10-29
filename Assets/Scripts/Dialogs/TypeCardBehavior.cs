using UnityEngine;
using System.Collections;

public class TypeCardBehavior : MonoBehaviour {
    public UISprite sprite;
    public UILabel label;
    private TYPE type;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public enum TYPE {
        sms10, sms15, vietel, mobi, vina, vtc, fpt, mega, ongame
    }
    public void init(TYPE type) {
        this.type = type;
        switch (type) {
            case TYPE.sms10:
                label.text = "MS $4,000";
                sprite.spriteName = "btn_sms";
                sprite.MakePixelPerfect();
                break;
            case TYPE.sms15:
                label.text = "MS $6,000";
                sprite.spriteName = "btn_sms";
                sprite.MakePixelPerfect();
                break;
            case TYPE.vietel:
                label.text = "Viettel";
                sprite.spriteName = "viettel";
                sprite.MakePixelPerfect();
                break;
            case TYPE.mobi:
                label.text = "Mobifone";
                sprite.spriteName = "mobifone";
                sprite.MakePixelPerfect();
                break;
            case TYPE.vina:
                label.text = "Vinaphone";
                sprite.spriteName = "vinaphone";
                sprite.MakePixelPerfect();
                break;
            case TYPE.vtc:
                label.text = "VTC";
                sprite.spriteName = "vtc";
                sprite.MakePixelPerfect();
                break;
            case TYPE.fpt:
                label.text = "FPT";
                sprite.spriteName = "fpt";
                sprite.MakePixelPerfect();
                break;
            case TYPE.mega:
                label.text = "Megacard";
                sprite.spriteName = "megacard";
                sprite.MakePixelPerfect();
                break;
            case TYPE.ongame:
                label.text = "Ongame";
                sprite.spriteName = "ongame";
                sprite.MakePixelPerfect();
                break;
        }
    }
    public void onClick() {
        //switch (type) {
        //    case TYPE.sms10:
        //        GameControl.instance.panelNotification.onShow("Nhắn tin để nạp $4,000 (phí 10k)?", delegate {
        //                string mobile = BaseInfo.gI().port10;
        //                GameControl.sendSMS(BaseInfo.gI().port10, BaseInfo.gI().syntax10 + " " + BaseInfo.gI().mainInfo.nick);
        //            });
        //        break;
        //    case TYPE.sms15:
        //        GameControl.instance.panelNotification.onShow("Nhắn tin để nạp $6,000 (phí 15k)?", delegate {
        //            GameControl.sendSMS(BaseInfo.gI().port15, BaseInfo.gI().syntax15 + " " + BaseInfo.gI().mainInfo.nick);
        //        });
        //        break;
        //    case TYPE.vietel:
        //        GameControl.instance.panelNapXu.setTypeCard(2);
        //        break;
        //    case TYPE.mobi:
        //        GameControl.instance.panelNapXu.setTypeCard(0);
        //        break;
        //    case TYPE.vina:
        //        GameControl.instance.panelNapXu.setTypeCard(1);
        //        break;
        //    case TYPE.vtc:
        //        GameControl.instance.panelNapXu.setTypeCard(3);
        //        break;
        //    case TYPE.fpt:
        //        GameControl.instance.panelNapXu.setTypeCard(4);
        //        break;
        //    case TYPE.mega:
        //        GameControl.instance.panelNapXu.setTypeCard(5);
        //        break;
        //    case TYPE.ongame:
        //        GameControl.instance.panelNapXu.setTypeCard(6);
        //        break;
        //}
    }

}
