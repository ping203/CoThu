using UnityEngine;
using System.Collections;

public class PanelNhiemVu : PanelGame {

    public GameObject itemNV;
    public GameObject parent;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void clearParent () {
        if(parent.transform.childCount != 0)
            foreach(Transform t in parent.transform) {
                Destroy (t.gameObject);
            }
    }

    public void addItem (int id, string info, int money, string process, bool isSuccess) {
        GameObject item = Instantiate (itemNV) as GameObject;
        parent.GetComponent<UIGrid> ().AddChild (item.transform);
        item.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
        Vector3 vt = item.transform.localPosition;
        vt.z = -10;
        item.transform.localPosition = vt;

        item.GetComponent<ItemNhiemVu> ().setText (id, money, info, process, isSuccess);
    }

    public void updateItem (int id, string info, bool isSuccess) {
        for(int i = 0; i < parent.transform.childCount; i++) {
            GameObject obj = parent.transform.GetChild (i).gameObject;
            if(obj.GetComponent<ItemNhiemVu> ().id == id) {
                obj.GetComponent<ItemNhiemVu> ().lb_info.text = info;
                obj.GetComponent<ItemNhiemVu> ().buttonNT.isEnabled = isSuccess;
                return;
            }
        }
    }
}
