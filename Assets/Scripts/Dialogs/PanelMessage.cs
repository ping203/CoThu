using UnityEngine;
using System.Collections;

public class PanelMessage : PanelGame {
	public UILabel textTu, textLuc, textNoiDung, textTitle;
    public UIScrollView scroll;
    public UISprite iconMail;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setText (string tu, string luc, string noidung) {
        textTitle.text = "TIN NHẮN";
		textTu.gameObject.SetActive (true);
		textLuc.gameObject.SetActive (true);
        iconMail.gameObject.SetActive (true);
		textTu.text = tu;
		textLuc.text = luc;
		textNoiDung.text = noidung;

        scroll.GetComponent<UICenterOnChild> ().enabled = true;
        scroll.GetComponent<UICenterOnChild> ().enabled = false;
	}

    public void setTextSK (string content) {
        textTitle.text = "SỰ KIỆN";
		textTu.gameObject.SetActive (false);
		textLuc.gameObject.SetActive (false);
        iconMail.gameObject.SetActive (false);
		textNoiDung.text = content;

      scroll.GetComponent<UICenterOnChild> ().enabled = true;
      scroll.GetComponent<UICenterOnChild> ().enabled = false;
	}

    public void onHide () {
        base.onHide ();
        scroll.GetComponent<UICenterOnChild> ().enabled = true;
    }
}
