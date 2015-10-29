using UnityEngine;
using System.Collections;

public class PanelNotiDoiThuong : PanelGame {

	/*public UILabel[] textContents;
	public UILabel[] textTitles;
	public UIPanel[] panels;*/

    public GameObject tgTemp;
    public GameObject lbTemp;

    public GameObject tgParent;
    public GameObject lbParent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setActiveTab(string title, string content)
    {
            GameObject pn = Instantiate(tgTemp) as GameObject;
            GameObject lb = Instantiate(lbTemp) as GameObject;

            pn.transform.parent = tgParent.transform;
            lb.transform.parent = lbParent.transform;

            pn.transform.localPosition = new Vector3(0, 0, 0);
            float s = lb.GetComponent<UILabel>().height;
            lb.transform.localPosition = new Vector3(0, 160 - s, 0);


            pn.transform.localScale = new Vector3(1, 1, 1);
            lb.transform.localScale = new Vector3(1, 1, 1);

            pn.GetComponent<UIToggledObjects>().activate.Add(lb);

            pn.transform.FindChild("LabelTitle").GetComponent<UILabel>().text = title;
            lb.GetComponent<UILabel>().text = content;
	}
}
