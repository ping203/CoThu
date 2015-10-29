using UnityEngine;
using System.Collections;

public class InfoGift : MonoBehaviour {
    public int idGift;
    public long priceGift;
    public string nameGift;
    public UITexture Gift;
    public UILabel Price, Name;
    WWW www;
    // Use this for initialization
    void Start () {

    }

    bool isSet = false;

    // Update is called once per frame
    void Update () {
        if(www != null) {
            if(www.isDone && !isSet) {
                Gift.mainTexture = www.texture;
                isSet = true;
            }
        }
    }

    internal void setInfoGift (int id, string name, string linkGift, long longPrice) {
        idGift = id;
        priceGift = longPrice;
        nameGift = name;

        Price.text = BaseInfo.formatMoneyNormal (longPrice) + "Xu";
        Name.text = name;
        www = new WWW (linkGift);
        if(www.error != null) {
            Debug.Log ("Image WWW ERROR: " + www.error);
        } else {

        }
    }
}
