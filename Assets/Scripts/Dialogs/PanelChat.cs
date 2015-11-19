using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PanelChat : PanelGame {
    public GameObject tblSmile;
    public GameObject btnSmile;
    public GameObject tblText;
    public GameObject btnText;

    //public UIInput textChat;
    public GameManager gameMa;
    public static String[] textChats = { "Bạn ơi, đánh nhanh lên được không",
		"Tớ đánh siêu chưa?", " Sợ tớ chưa? Hehe",
		"Thắng ván này tớ mời cậu đi XXX luôn", "Chết mày nè!", "Ảo vl",
		"Huhu, sao đen đủi vậy...:(", "Đánh hay ghê!",
		"Mạng lag quá, bạn thông cảm nhé!"};

    // Use this for initialization
    void Start () {
        for(int i = 0; i < 62; i++) {
            GameObject btn = Instantiate (btnSmile) as GameObject;
            btn.transform.parent = tblSmile.transform;
            btn.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
            btn.transform.localPosition = new Vector3 (0, 0, -14);
            btn.GetComponent<UIButton> ().normalSprite = "" + (i + 1);
            btn.name = "" + (i + 1);
            EventDelegate.Set (btn.GetComponent<UIButton> ().onClick, delegate {
                ClickSmile (btn);
            });
        }

        for(int i = 0; i < textChats.Length; i++) {
            GameObject btnT = Instantiate (btnText) as GameObject;
            btnT.transform.parent = tblText.transform;
            btnT.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
            btnT.transform.localPosition = new Vector3 (0, 0, -14);
            btnT.transform.FindChild ("Label").GetComponent<UILabel> ().text = textChats[i];
            EventDelegate.Set (btnT.GetComponent<UIButton> ().onClick, delegate {
                ClickText (btnT);
            });
        }
    }

    // Update is called once per frame
    void Update () {
    }

    public void sendChatQuick () {
        ////GameControl.instance.sound.startClickButtonAudio ();
        //string text = textChat.value;
        //if(text != "") {
        onHide ();
        ////SendData.onSendMsgChat (text);
        //textChat.value = "";
        //}
    }

    public void ClickSmile (GameObject obj) {
        ////GameControl.instance.sound.startClickButtonAudio ();
        //string text = "";
        int index = int.Parse (obj.name);

        Debug.Log ("+++++++++++++++++ " + index);
        //text = Chat.smileys[index];
        ////SendData.onSendMsgChat (text);
        gameMa.myProfileNew.setChat ("", index);

        onHide ();
    }

    public void ClickText (GameObject obj) {
        ////GameControl.instance.sound.startClickButtonAudio ();
        string text = obj.transform.FindChild ("Label").GetComponent<UILabel> ().text;

        // //SendData.onSendMsgChat (text);
        gameMa.myProfileNew.setChat (text, -1);

        onHide ();
    }
}
