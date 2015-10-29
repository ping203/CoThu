using UnityEngine;
using System.Collections;
using System.Globalization;

public class PanelDoiThuong : PanelGame
{
	public GameObject tblContaintGiftTheCao;
	public GameObject tblContaintGiftVatPham;
	public GameObject btnGift;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void addGiftInfo (int id, int type, string name, long price, string link)
	{
		GameObject btnT = Instantiate (btnGift) as GameObject;
		if (type == 1) {
			btnT.transform.parent = tblContaintGiftTheCao.transform;
		} else {
			btnT.transform.parent = tblContaintGiftVatPham.transform;
		}
		
		btnT.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		btnT.transform.localPosition = new Vector3 (0, 0, -10);
		btnT.GetComponent<InfoGift> ().setInfoGift (id, name, link, price);

        EventDelegate.Set (btnT.GetComponent<UIButton> ().onClick, delegate {
            sendGift (btnT);
        });
	}

    public void sendGift (GameObject obj) {
        ////GameControl.instance.sound.startClickButtonAudio ();
        GameObject gift = obj.gameObject;
		int id = gift.GetComponent<InfoGift> ().idGift;
		long priceGift = gift.GetComponent<InfoGift> ().priceGift;
		string name = gift.GetComponent<InfoGift> ().nameGift;
		GameControl.instance.panelYesNo.onShow ("Bạn muốn đổi " + BaseInfo.formatMoneyNormal ( priceGift) + " lấy " + name, delegate {
			////SendData.onSendGift(id, priceGift);
	});
	}
}
