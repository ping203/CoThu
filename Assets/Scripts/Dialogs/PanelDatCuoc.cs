using UnityEngine;
using System.Collections;

public class PanelDatCuoc : PanelGame {
    public UISlider sliderMoney;
    public UILabel inputMoney;
    private long money;
    float rateVIP, rateFREE;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public void onChangeMoney() {
    }

    public void clickOK () {
       // //GameControl.instance.sound.startClickButtonAudio ();
       // //SendData.onChangeBetMoney(money);
        this.onHide();
    }
    public void onShow() {
        sliderMoney.value = 0;
        onChangeMoney();
        base.onShow();
    }
}
