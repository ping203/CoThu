using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PanelChat : PanelGame {
    public GameObject tblSmile;
    public GameObject btnSmile;
    public GameObject tblText;
    public GameObject btnText;

    public UIInput textChat;

	public static String[] textChats = { "Bạn ơi, đánh nhanh lên được không", "Bắt đầu đi",
		"Sẵn sàng đi", "Cho tớ chơi với",
		"Tớ đánh siêu chưa?", " Sợ tớ chưa? Hehe",
		"Tăng tiền cược lên bạn nhé?",
		"Thắng ván này tớ mời cậu đi XXX luôn",
		"Cậu khóa bàn lại, chiến tay bo đi", "Chết mày nè!", "Ảo vl",
		"Huhu, sao đen đủi vậy...:(", "Chơi nhỏ chán quá!",
		"Mày hả bưởi...:D", "Tất tay đi nào!", "Đánh hay ghê!",
		"Mạng lag quá, bạn thông cảm nhé", "Cho đánh với nào!"};

    // Use this for initialization
    void Start () {
        for(int i = 0; i < 28; i++) {
            GameObject btn = Instantiate (btnSmile) as GameObject;
            btn.transform.parent = tblSmile.transform;
            btn.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
            btn.transform.localPosition = new Vector3 (0, 0, -14);
            btn.GetComponent<UIButton> ().normalSprite = "a" + (i + 1);
            btn.name = "" + i;
            EventDelegate.Set (btn.GetComponent<UIButton> ().onClick, delegate{
                ClickSmile (btn);
            });
        }

        for(int i = 0; i < textChats.Length; i++) {
            GameObject btnT = Instantiate (btnText) as GameObject;
            btnT.transform.parent = tblText.transform;
            btnT.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
            btnT.transform.localPosition = new Vector3 (0, 0, -14);
            btnT.transform.FindChild ("Label").GetComponent<UILabel> ().text = textChats[i];
            EventDelegate.Set (btnT.GetComponent<UIButton> ().onClick, delegate{
                ClickText (btnT);
            });
        }
    }

    // Update is called once per frame
    void Update () {
    }

    public void sendChatQuick () {
        ////GameControl.instance.sound.startClickButtonAudio ();
        string text = textChat.value;
        if(text != "") {
            onHide ();
            ////SendData.onSendMsgChat (text);
            textChat.value = "";
        }
    }

    public void ClickSmile (GameObject obj) {
        ////GameControl.instance.sound.startClickButtonAudio ();
        string text = "";
        int index = Convert.ToInt32 (obj.name);
        text = Chat.smileys[index];
        onHide ();
        ////SendData.onSendMsgChat (text);
    }

    public void ClickText (GameObject obj) {
        ////GameControl.instance.sound.startClickButtonAudio ();
        string text = "";
        UILabel label = obj.transform.FindChild ("Label").GetComponent<UILabel> ();
        text = label.text;
        onHide ();
       // //SendData.onSendMsgChat (text);
    }
}
