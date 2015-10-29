using UnityEngine;
using System.Collections;

public class TableBehavior : MonoBehaviour {
    public UILabel lb_ban;
    public UILabel lb_muccuoc;
    public UISlider slide_tinhtrang;
    public UILabel numPlayer;
    public UISprite spriteLock;

    //control
    public UISprite backgroundSprite;
    public UIPanel panel;
    public InstanListViewControler listPopulator;
    public int itemNumber;
    public int itemDataIndex;
    private bool isVisible = true;

    public int id;
    public int status;
    public string name = "";
    public string masid = "";
    public int nUser;
    public int maxUser;
    public long money;
    public long needMoney;
    public long maxMoney;
    public int Lock = 0;
    public int typeTable;
    public int choinhanh = 0;
    void Start () {
        transform.localScale = new Vector3 (1, 1, 1);
    }

    // Update is called once per frame
    void Update () {
        if(Mathf.Abs (listPopulator.draggablePanel.currentMomentum.y) > 0) {
            CheckVisibilty ();
        }
    }
    public void setInFo(int staste) {
        if(staste % 2 == 0) {
            backgroundSprite.spriteName = "bgChan";
        } else {
            backgroundSprite.spriteName = "bgLe";
        }

        lb_ban.text = "Bàn " + id;
        lb_muccuoc.text = "$"
                        + BaseInfo.formatMoney(money) + " ("
                        + BaseInfo.formatMoney(needMoney) + ")";
        if (typeTable == Res.ROOMFREE) {
            lb_muccuoc.text = BaseInfo.formatMoneyDetailDot (money) + " " + Res.MONEY_UNIT;
        }
        else {
            lb_muccuoc.text = BaseInfo.formatMoneyDetailDot (money) + " " + Res.MONEY_UNIT;
        }
        float tt = (float)nUser / maxUser;
        slide_tinhtrang.value = tt;

        spriteLock.gameObject.SetActive (false);
        if(Lock == 1) {
            spriteLock.gameObject.SetActive (true);
        }
        numPlayer.text = nUser + "/" + maxUser;
    }

    public void clickTable() {
		//GameControl.instance.mcg.onClickAI ();
		GameControl.instance.mcg.onClickCN ();
    }


    public bool verifyVisibility () {
        return (panel.IsVisible (backgroundSprite));
    }

    void CheckVisibilty () {
        bool currentVisibilty = panel.IsVisible (backgroundSprite);
        if(currentVisibilty != isVisible) {
            isVisible = currentVisibilty;

            if(!isVisible) {
                StartCoroutine (listPopulator.ItemIsInvisible (itemNumber));
            }
        }
    }
}
