using UnityEngine;
using System.Collections;

public class PanelNapChuyenXu : PanelGame {
    public PanelSMS panelSMS;
    public UIToggle tgTheCao;
    // Use this for initialization
    void Start () {
    }

    public void clickTabChuyenXu () {
        if(BaseInfo.gI ().mainInfo.moneyChip <= 0) {
            tgTheCao.value = true;
            GameControl.instance.panelThongBao.onShow ("Bạn không còn " + Res.MONEY_UNIT + " để chuyển!", delegate {
            });
        }
    }
}
