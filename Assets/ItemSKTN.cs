using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSKTN : MonoBehaviour {

	public int idMess, idEvent;
	public string noiDungMess, ndEvent;
	public UISprite iconMail;
	public UILabel labelGuiTu = null, labelGuiLuc = null, labelNoiDungTinNhan = null, labelNoiDungSuKien = null;
	public UIButton del;

	//PanelYesNo pYN;

	// Use this for initialization
	void Start () {
		//pYN = GameObject.Find ("PanelYesNo").GetComponent<PanelYesNo>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setIconItemTN (int id, string guiTu, string guiLuc, string noiDung, sbyte isRead){
		idMess = id;
		noiDungMess = noiDung;

		labelNoiDungSuKien.gameObject.SetActive (false);
		labelNoiDungTinNhan.gameObject.SetActive (true);
		labelGuiTu.gameObject.SetActive (true);
		labelGuiLuc.gameObject.SetActive (true);
		del.gameObject.SetActive (true);

		if (isRead == 0) {
			iconMail.spriteName = "icon_thu_dong";
		} else {
			iconMail.spriteName = "icon_thu_mo";
		}
		labelGuiTu.text = "Từ: " + guiTu;
		labelGuiLuc.text = "Lúc: " + guiLuc.Substring(0, 10);
		if(noiDung.Length > 30) {
			labelNoiDungTinNhan.text = (noiDung.Substring (0, 30) + "...");
		} else {
			labelNoiDungTinNhan.text = (noiDung);
		}
	}

	public void setIconItemSK (int id, string tilte, string content){
		idEvent = id;
		ndEvent = content;

		labelNoiDungSuKien.gameObject.SetActive (true);
		labelNoiDungTinNhan.gameObject.SetActive (false);
		labelGuiTu.gameObject.SetActive (false);
		labelGuiLuc.gameObject.SetActive (false);
		del.gameObject.SetActive (false);
		iconMail.spriteName = "icon_thu_dong";

		if(content.Length > 30) {
			labelNoiDungSuKien.text = (content.Substring (0, 30) + "...");
		} else {
			labelNoiDungSuKien.text = (content);
		}
	}

	public void delMess(){
		PanelMail pm = GameObject.Find ("PanelMail").GetComponent<PanelMail>();
		Debug.Log ("GameObject: " + pm);
		pm.onDelMess (idMess, gameObject);
	}
}
