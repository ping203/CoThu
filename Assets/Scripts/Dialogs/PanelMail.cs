using UnityEngine;
using System.Collections;

public class PanelMail : PanelGame {	
	
	public GameObject tblContaintSuKien;
	public GameObject tblContaintTinNhan;
	public GameObject itemSKTN;
	
	public PanelMessage panelMessage;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onHide(){
		ClearParent ();
		base.onHide ();
	}

	public void addIconTinNhan (int id, string guiTu, string guiLuc, string noiDung, sbyte isRead){
		GameObject btnT = Instantiate (itemSKTN) as GameObject;

		tblContaintTinNhan.GetComponent<UIGrid> ().AddChild (btnT.transform);
		btnT.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		Vector3 vt = btnT.transform.localPosition;
		vt.z = -10;
		btnT.transform.localPosition = vt;
		
		btnT.GetComponent<ItemSKTN>().setIconItemTN (id, guiTu, guiLuc, noiDung, isRead);
		
		EventDelegate.Set (btnT.GetComponent<UIButton> ().onClick, delegate{
            ClickDocTN (btnT);
        });
       //ventDelegate even = new EventDelegate (this, ClickDocTN);

	}

	public void addIconSuKien (int id, string title, string content){
		GameObject btnT2 = Instantiate (itemSKTN) as GameObject;
		
		tblContaintSuKien.GetComponent<UIGrid> ().AddChild (btnT2.transform);
		btnT2.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		Vector3 vt = btnT2.transform.localPosition;
		vt.z = -10;
		btnT2.transform.localPosition = vt;
		
		btnT2.GetComponent<ItemSKTN>().setIconItemSK (id, title, content);
		
		//EventDelegate.Set (btnT2.GetComponent<UIButton> ().onClick, ClickDocSK(btnT2));
        EventDelegate.Set (btnT2.GetComponent<UIButton> ().onClick, delegate {
            ClickDocSK (btnT2);
        });
	}
	
	public void ClearParent(){
        tblContaintTinNhan.GetComponent<UIGrid> ().enabled = true;
		foreach (Transform t in tblContaintTinNhan.transform) {
			Destroy(t.gameObject);
		}
	}

	public void clearSuKien(){
        tblContaintSuKien.GetComponent<UIGrid> ().enabled = true;
		foreach (Transform t in tblContaintSuKien.transform) {
			Destroy(t.gameObject);
		}
	}
	
	public void ClickDocTN(GameObject obj){
		//GameObject obj = UICamera.currentTouch.pressed;

		int id = obj.GetComponent<ItemSKTN>().idMess;
		string guiluc = obj.GetComponent<ItemSKTN>().labelGuiLuc.text;
		string guitu = obj.GetComponent<ItemSKTN>().labelGuiTu.text;
		string nd = obj.GetComponent<ItemSKTN> ().noiDungMess;
		UISprite icon = obj.GetComponent<ItemSKTN> ().iconMail;
		icon.spriteName = "icon_thu_mo";

		panelMessage.setText (guitu, guiluc, nd);
		panelMessage.onShow ();
		
		//SendData.onReadMessage (id);
	}

    public void ClickDocSK (GameObject obj) {
		//GameObject obj = UICamera.currentTouch.pressed;
		
		int id = obj.GetComponent<ItemSKTN>().idEvent;
		string nd = obj.GetComponent<ItemSKTN> ().ndEvent;
		
		panelMessage.setTextSK (nd);
		panelMessage.onShow ();
		
		////SendData.onReadMessage (id);
	}

	public void onDelMess(int id, GameObject obj){
		GameControl.instance.panelYesNo.onShow ("Bạn muốn xóa tin nhắn này?", delegate {
			//SendData.onDelMessage(id);
			obj.transform.parent.GetComponent<UIGrid> ().RemoveChild (obj.transform);
			Destroy (obj);
	});
	}
}
