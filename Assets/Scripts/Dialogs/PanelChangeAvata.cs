using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PanelChangeAvata : PanelGame {
	public GameObject tblAva;
	public GameObject btnAva;

	public bool isLoad = true;

	// Use this for initialization
	void Start () {

	}

	public void loadAva(){
		if (isLoad) {
			for (int i = 0; i < 60; i++) {
				GameObject btn = Instantiate (btnAva) as GameObject;
				btn.transform.parent = tblAva.transform;
				btn.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
				btn.transform.localPosition = new Vector3 (0, 0, -14);
				btn.GetComponent<UIButton> ().normalSprite = "" + i;
				btn.name = "" + i;
				EventDelegate.Set (btn.GetComponent<UIButton> ().onClick, delegate{
                    ClickAva (btn);
                });
			}
			isLoad = false;
		}
	}

    public void ClickAva (GameObject obj) {
        ////GameControl.instance.sound.startClickButtonAudio ();
        int index = Convert.ToInt32 (obj.name);
		BaseInfo.gI ().mainInfo.idAvata = index;
		////SendData.onUpdateAvata (index);
		onHide ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
