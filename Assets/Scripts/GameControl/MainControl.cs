using UnityEngine;
using System.Collections;

public class MainControl : StageControl {
    public UILabel lb_textnoti;
    public UILabel lb_name;
    public UILabel lb_chip;
    public UILabel lb_name_game;
    public UILabel lb_name_game2;
    public UISprite spriteAvata;

    public GameObject itemXH;
    public GameObject itemXHTo;
    public UIButton btnNext, btnPre;
    public UIButton btnNext2, btnPre2;

    int indexPage = 0;
    public GameObject[] itemXepHang;
    public GameObject[] itemXepHangTo;
    int[] game_id = { 0, 1, 2, 3, 4, 5, 6, 8 };

    public GameObject xh_to;

    // Use this for initialization
    void Start () {
        updateAvataName ();
        //SendData.onGetTopGame (0);
        xh_to.SetActive (false);
    }

    // Update is called once per frame
    void Update () {
       // lb_chip.text = (BaseInfo.formatMoneyNormal (BaseInfo.gI ().mainInfo.moneyChip) + Res.MONEY_UNIT);
		lb_chip.text = (BaseInfo.formatMoneyNormal (ServerController.serverController.coins) + Res.MONEY_UNIT);
        if(gameObject.activeInHierarchy && Input.GetKeyDown (KeyCode.Escape)) {
            gameControl.disableAllDialog ();
            onBack ();
        }
    }

    public void updateAvataName () {
        string dis = BaseInfo.gI ().mainInfo.displayname;
        if(dis.Length > 6) {
            dis = dis.Substring (0, 5) + "...";
        }
        lb_name.text = dis;
        int id = BaseInfo.gI ().mainInfo.idAvata;
        string link_ava = BaseInfo.gI ().mainInfo.link_Avatar;

        if(id >= 0) {
            spriteAvata.GetComponent<UISprite> ().enabled = true;
            spriteAvata.GetComponent<UITexture> ().enabled = false;
            spriteAvata.spriteName = id + "";
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

    public void onClickGameSolo () {
        onSelectGame (GameID.SOLO);
    }


    public void onClickGameCard () {
        //onSelectGame (GameID.CARD);
        gameControl.toast.setText ("Game đang phát triển!");
    }

    void onSelectGame(int gameid){
        gameControl.gameID = gameid;
        gameControl.room.setGameName ();
        gameControl.setStage (gameControl.room);
    }

    public void clickAvatar () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelInfoPlayer.infoMe ();
        gameControl.panelInfoPlayer.onShow ();
    }

    public override void onBack () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelYesNo.onShow ("Bạn có muốn thoát?", delegate {
            NetworkUtil.GI ().close ();
            gameControl.setStage (gameControl.login);
        });
    }
    public void onClickChoiNgay () {
        gameControl.panelWaiting.onShow ();
        //SendData.onChoingay ();
    }

    public void onClickNhiemVu () {
        gameControl.panelNhiemVu.onShow ();
    }

    public void addItemXepHang (int id, string displayName, string avata, int idavata, int win, int medal) {
        if(id == 8) {
            id = 7;
        }
        GameObject item = Instantiate (itemXH) as GameObject;
        itemXepHang[id].GetComponent<UIGrid> ().AddChild (item.transform);
        item.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
        item.GetComponent<ItemXepHang> ().setInfo (avata, idavata, displayName, win + "", medal);
        EventDelegate.Set (item.GetComponent<UIButton> ().onClick, delegate {
            itemClickXH ();
        });

        GameObject item2 = Instantiate (itemXHTo) as GameObject;
        itemXepHangTo[id].GetComponent<UIGrid> ().AddChild (item2.transform);
        item2.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
        item2.GetComponent<ItemXepHang> ().setInfo (avata, idavata, displayName, win + "", medal);
    }

    public void itemClickXH () {
        xh_to.SetActive (true);
    }

    public void disableXH () {
        xh_to.SetActive (false);
    }

    public void clearGrid (int id) {
        if(id == 8) {
            id = 7;
        }
        foreach(Transform t in itemXepHang[id].transform) {
            itemXepHang[id].GetComponent<UIGrid> ().RemoveChild (t);
        }
        foreach(Transform t in itemXepHangTo[id].transform) {
            itemXepHangTo[id].GetComponent<UIGrid> ().RemoveChild (t);
        }
    }

    public void setActiveXH (int gameID) {
        string name = "";
        switch(gameID) {
            case GameID.SOLO:
                name = "So Lo";
                break;
            case GameID.CARD:
                name = "Card";
                break;
        }
        lb_name_game.text = name;
        lb_name_game2.text = name;
        for(int i = 0; i < itemXepHang.Length; i++) {
            itemXepHang[i].SetActive (false);
        }
        itemXepHang[indexPage].SetActive (true);

        for(int i = 0; i < itemXepHangTo.Length; i++) {
            itemXepHangTo[i].SetActive (false);
        }
        itemXepHangTo[indexPage].SetActive (true);
    }

    public void isEnableButton (bool isEnable) {
        btnNext.enabled = isEnable;
        btnPre.enabled = isEnable;
        btnNext2.enabled = isEnable;
        btnPre2.enabled = isEnable;
    }

    public void onClickNext () {
        //indexPage++;
        //if(indexPage > game_id.Length - 1) {
        //    indexPage = 0;
        //}
        //if(itemXepHang[indexPage].transform.childCount == 0) {
        //    //SendData.onGetTopGame (game_id[indexPage]);
        //    isEnableButton (false);
        //} else {
        //    setActiveXH (game_id[indexPage]);
        //}
    }

    public void onClickNext2 () {
        //indexPage++;
        //if(indexPage > game_id.Length - 1) {
        //    indexPage = 0;
        //}

        //if(itemXepHangTo[indexPage].transform.childCount == 0) {
        //    //SendData.onGetTopGame (game_id[indexPage]);
        //    isEnableButton (false);
        //} else {
        //    setActiveXH (game_id[indexPage]);
        //}
    }

    public void onClickPre () {
        //indexPage--;
        //if(indexPage <= 0) {
        //    indexPage = game_id.Length - 1;
        //}

        //if(itemXepHang[indexPage].transform.childCount == 0) {
        //    //SendData.onGetTopGame (game_id[indexPage]);
        //    isEnableButton (false);
        //} else {
        //    setActiveXH (game_id[indexPage]);
        //}
    }

    public void onClickPre2 () {
        //indexPage--;
        //if(indexPage <= 0) {
        //    indexPage = game_id.Length - 1;
        //}
        //if(itemXepHangTo[indexPage].transform.childCount == 0) {
        //    //SendData.onGetTopGame (game_id[indexPage]);
        //    isEnableButton (false);
        //} else {
        //    setActiveXH (game_id[indexPage]);
        //}
    }

    public void clickNapXu () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelNapChuyenXu.onShow ();

    }

    public void clickDoiThuong () {
        //GameControl.instance.sound.startClickButtonAudio ();
        //gameControl.panelWaiting.onShow ();
        //SendData.onGetInfoGift ();
    }

    public void clickNoti () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelNotiDoiThuong.onShow ();
    }

    public void clickHomThu () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelMail.onShow ();
    }

    public void clickFanpage () {
        //GameControl.instance.sound.startClickButtonAudio ();
        gameControl.panelYesNo.onShow ("Bạn có muốn chuyển đến fanpage game?", delegate {
            Application.OpenURL ("https://www.facebook.com/FacebookVietnam");
        });

    }
}
