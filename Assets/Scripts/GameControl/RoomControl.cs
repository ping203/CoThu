using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class RoomControl : StageControl {
    public UIToggle toggleRoom;
    public List<TableBehavior> listTableBehavior = new List<TableBehavior> ();

    public UILabel lb_tengame;

    public UILabel displayName;
    public UILabel displayChip;
    public UISprite spriteAvata;

    public InstanListViewControler _instanListViewControler;

    // Use this for initialization
    void Start () {
        updateAvataName ();
        setGameName ();
        for (int i = 0; i < 1; i++)
			{
                TableItem it = new TableItem ();
                it.id = i;
                it.status = 0;
                it.name = i + "";
                it.masid = i + "";
                it.nUser = 1;
                it.maxUser = 2;
                it.money = 20;
                it.needMoney = 1000;
                it.maxMoney = 1000;
                it.Lock = 0;
                it.typeTable = 1;
                it.choinhanh = 0;


                gameControl.listTableItem.Add (it);
			}
        createScollPane (gameControl.listTableItem, 0);
    }

    // Update is called once per frame
    void Update () {
		// displayChip.text = BaseInfo.formatMoneyNormal (BaseInfo.gI ().mainInfo.moneyChip) + Res.MONEY_UNIT;
		displayChip.text = (BaseInfo.formatMoneyNormal (ServerController.serverController.coins) + Res.MONEY_UNIT);

		if(gameObject.activeInHierarchy && Input.GetKeyDown (KeyCode.Escape)) {
            gameControl.disableAllDialog ();
            onBack ();
        }
    }
    void deActive () {
        gameObject.SetActive (false);
    }

    public override void onBack () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.setStage (gameControl.main);
    }
    public void setGameName () {
        string name = "CHỌN GAME";
        switch(gameControl.gameID) {
            case GameID.SOLO:
                name = "So Lo";
                break;
            case GameID.CARD:
                name = "Card";
                break;
        }
        lb_tengame.text = name;
    }
    public void createScollPane (List<TableItem> listTable, int typeRoom) {
        gameControl.panelWaiting.onShow ();
        setGameName ();
        listTableBehavior.Clear ();
        try {
            if(this.gameObject.activeInHierarchy) {
                for(int i = 0; i < listTable.Count; i++) {
                    TableBehavior tableBehavior = new TableBehavior ();
                    tableBehavior.id = listTable[i].id;
                    tableBehavior.status = listTable[i].status;
                    tableBehavior.name = listTable[i].name;
                    tableBehavior.masid = listTable[i].masid;
                    tableBehavior.nUser = listTable[i].nUser;
                    tableBehavior.maxUser = listTable[i].maxUser;
                    tableBehavior.money = listTable[i].money;
                    tableBehavior.needMoney = listTable[i].needMoney;
                    tableBehavior.maxMoney = listTable[i].maxMoney;
                    tableBehavior.Lock = listTable[i].Lock;
                    tableBehavior.typeTable = listTable[i].typeTable;
                    tableBehavior.choinhanh = listTable[i].choinhanh;
                    listTableBehavior.Add (tableBehavior);
                }
                _instanListViewControler.InitTableView (listTableBehavior, 0);
            }
        } catch(Exception e) {
            Debug.LogException (e);
        }
        gameControl.panelWaiting.onHide ();
    }
    public void updateAvataName () {
        string dis = BaseInfo.gI ().mainInfo.displayname;
        if(dis.Length > 6) {
            dis = dis.Substring (0, 5) + "...";
        }
        displayName.text = dis;
        int id = BaseInfo.gI ().mainInfo.idAvata;
        string link_ava = BaseInfo.gI ().mainInfo.link_Avatar;

        if(id >= 0) {
            spriteAvata.GetComponent<UISprite> ().enabled = true;
            spriteAvata.GetComponent<UITexture> ().enabled = false;
            if(id != 0) {
                spriteAvata.spriteName = id + "";
            } else {
                spriteAvata.spriteName = "0";
            }
        } else {
            WWW www = new WWW (link_ava);
            if(www.error != null) {
            } else {
                while(!www.isDone) {
                }
                spriteAvata.GetComponent<UISprite> ().enabled = false;
                spriteAvata.GetComponent<UITexture> ().enabled = true;
                spriteAvata.GetComponent<UITexture> ().mainTexture = www.texture;
            }
        }
    }

    public void clickAvatar () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelInfoPlayer.infoMe ();
        gameControl.panelInfoPlayer.onShow ();
    }

    public void clickButtonLamMoi () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelWaiting.onShow ();
        //SendData.onUpdateRoom ();
    }

    public void sortBanCuoc () {
        //GameControl.instance.sound.startClickButtonAudio ();
        BaseInfo.gI ().sort_giam_dan_bancuoc = !BaseInfo.gI ().sort_giam_dan_bancuoc;
        BaseInfo.gI ().type_sort = 1;
        //SendData.onUpdateRoom ();
    }

    public void sortMucCuoc () {
        //GameControl.instance.sound.startClickButtonAudio ();
        BaseInfo.gI ().sort_giam_dan_muccuoc = !BaseInfo.gI ().sort_giam_dan_muccuoc;
        BaseInfo.gI ().type_sort = 2;
        //SendData.onUpdateRoom ();
    }

    public void sortTrangThai () {
        //GameControl.instance.sound.startClickButtonAudio ();
        BaseInfo.gI ().sort_giam_dan_nguoichoi = !BaseInfo.gI ().sort_giam_dan_nguoichoi;
        BaseInfo.gI ().type_sort = 3;
        //SendData.onUpdateRoom ();
    }

    public void clickButtonChoiNgay () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelWaiting.onShow ();
        //SendData.onAutoJoinTable ();
    }
    public void clickAnBanFull (bool isChecked) {
        gameControl.panelWaiting.onShow ();
        BaseInfo.gI ().isHideTabeFull = isChecked;
        //SendData.onUpdateRoom ();
    }
    public void clickSetting () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelSetting.onShow ();
    }

    public void clickHelp () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panleHelp.onShow ();
    }

    public void clickCreateRoom () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelCreateRoom.onShow ();
    }

    public void clickToiBan () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelToiBan.onShow ();
    }

    public void clickNoti () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelNotiDoiThuong.onShow ();
    }

    public void clickPlayNow () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelWaiting.onShow ();
        //SendData.onAutoJoinTable ();
    }
}
