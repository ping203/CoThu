using UnityEngine;
using System.Collections;

public class PanelRutTien : PanelGame {
    public UISlider slider;
    public UILabel lb_current;
    public UIToggle checkbox;
    int idtable, idroom, idgame, type;
    int typeRoom;
    long tienmin = 0, tienmax, tienchon;
    public GameControl gameControl;
    //public UILabel lb_rut;
    public bool tuDongRutTien = false;
    public int soTienRut;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void clickButtonOk () {
        //GameControl.instance.sound.startClickButtonAudio ();
        if(checkbox.value) {
            BaseInfo.gI ().tuDongRutTien = true;
            BaseInfo.gI ().soTienRut = tienchon;
        } else {
            BaseInfo.gI ().tuDongRutTien = false;
        }
        switch(type) {
            case 0:
                gameControl.panelWaiting.onShow ();
                //SendData.onJoinTablePlay (idtable,
                 //       "", tienchon);
                break;
            case 1:
                //SendData.onAcceptInviteFriend ((sbyte) idgame,
                  //      (short) idroom, (short) idtable, tienchon);
                break;
            case 2:
                //SendData.onSendGetMoney (tienchon);
                break;
            case 3:
                //SendData.onSendGetMoney (tienchon);
                break;
        }
        onHide ();
    }

    public void show (long tienMin, long tienMax, int type,
             int idTable, int roomID, int gameID, int typeRoom) {
        long temp;
        string moneyName;
        //if(typeRoom == 1) {
            temp = BaseInfo.gI ().mainInfo.moneyChip;
            moneyName = Res.MONEY_UNIT;
        //} else {
        //    temp = BaseInfo.gI ().mainInfo.moneyXu;
        //    moneyName = Res.MONEY_UNIT;
        //}

        if(tienMin > temp) {
            BaseInfo.gI ().currentMinMoney = tienMin;
            BaseInfo.gI ().currentMaxMoney = tienMax;
            //SendData.onJoinTablePlay (idTable, "",
              //      temp);
        } else {
            BaseInfo.gI ().currentMinMoney = tienMin;
            BaseInfo.gI ().currentMaxMoney = tienMax;

            if(tienMax > temp) {
                tienMax = temp;
            }
            if(temp < tienMin) {
                tienMin = temp;
            } else {

            }
            this.tienmin = tienMin;
            this.tienmax = tienMax;
            this.tienchon = tienMin;
            //lb_min.text = BaseInfo.formatMoney(tienMin)+moneyName;
            //lb_max.text = BaseInfo.formatMoney(tienMax)+moneyName;
            this.lb_current.text = BaseInfo.formatMoneyDetailDot (tienchon);
            slider.value = 0;

            //lb_rut.text = BaseInfo.formatMoney(tienchon);
            this.type = type;
            this.idtable = idTable;
            this.idroom = roomID;
            this.idgame = gameID;
            onShow ();
        }
    }

    public void onValueChange () {
        tienchon = (int) (slider.value * (tienmax));
        if(tienchon < tienmin) {
            tienchon = tienmin;
        }
        //lb_rut.text =  BaseInfo.formatMoney(tienchon);
        lb_current.text = BaseInfo.formatMoneyDetailDot (tienchon);
    }
}
