using UnityEngine;
using System.Collections;
using System;
public class PanelToiBan : PanelGame {
    public UIInput inputID;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void toiBan(){
        try {
            //GameControl.instance.sound.startClickButtonAudio();
            string stringtbid = inputID.value;
            if (stringtbid == "") {
                GameControl.instance.panelThongBao.onShow("Bạn chưa nhập tên bàn.", delegate { });
                return;
            }
            int tbid = int.Parse(stringtbid);
            //SendData.onJoinTableForView(tbid, "");
            onHide();
        }
        catch (Exception e) {
            GameControl.instance.toast.setText("Định dạng bàn không đúng!");
            //Debug.LogException(e);
        }
	}
}
