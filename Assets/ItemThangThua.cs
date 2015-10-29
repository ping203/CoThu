using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemThangThua : MonoBehaviour {

  public UILabel gameName, info;
    
	// Use this for initialization
	void Start () {
		//pYN = GameObject.Find ("PanelYesNo").GetComponent<PanelYesNo>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   public void setText (string[] thang, string[] thua, int index) {
        //int idGame = int (thang.split ("-")[0]);
       int idGame = int.Parse (thang[index].Split ('-')[0]);
        string namegame = "";
        switch(idGame) {
            case GameID.SOLO:
                namegame = "So Lo";
                break;
            case GameID.CARD:
                namegame = "Card";
                break;
        }

        gameName.text = namegame;
        info.text = thang[index].Split ('-')[1] + " / " + thua[index].Split ('-')[1];
    }
}
