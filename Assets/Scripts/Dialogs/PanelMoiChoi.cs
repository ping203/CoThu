using UnityEngine;
using System.Collections;

public class PanelMoiChoi : PanelGame {
	public GameObject tblContaint;
	public GameObject btnIcon;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addIcon (string name, string displayname, long money){
		GameObject btnT = Instantiate (btnIcon) as GameObject;
		tblContaint.GetComponent<UIGrid> ().AddChild (btnT.transform);
		btnT.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		Vector3 vt = btnT.transform.localPosition;
		vt.z = -10;
		btnT.transform.localPosition = vt;

        if(displayname.Length > 17) {
            displayname = displayname.Substring (0, 14) + "...";
        }

		btnT.transform.FindChild ("LabelEmail").GetComponent<UILabel> ().text = displayname;
		btnT.transform.FindChild ("LabelMoney").GetComponent<UILabel> ().text = BaseInfo.formatMoney (money);

        EventDelegate.Set (btnT.GetComponent<UIButton> ().onClick, delegate {
            ClickMoi (btnT, name);
        });
	}
	
	public void ClearParent(){
        tblContaint.GetComponent<UIGrid> ().enabled = true;
		foreach (Transform t in tblContaint.transform) {
			Destroy(t.gameObject);
		}
	}

	public void ClickMoi(GameObject obj, string name){
        //SendData.onInviteFriend (name);
        if(tblContaint.transform.childCount == 1) {
            this.gameObject.SetActive (false);
        }
		tblContaint.GetComponent<UIGrid> ().RemoveChild (obj.transform);
		Destroy (obj);
	}
}
