using UnityEngine;
using System.Collections;

public class PanelCuoc : PanelGame {
    public UISlider slider;
    public UILabel currentMoney;
    long tienmin = 0, tienmax, tienchon;
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
    public void onValueChange () {
        tienchon = (int) (slider.value * (tienmax));
        if(tienchon < tienmin) {
            tienchon = tienmin;
        }
        //lb_rut.text =  BaseInfo.formatMoney(tienchon);
        currentMoney.text = BaseInfo.formatMoney (tienchon);
    }

    public void clickBtnOk () {
        ////GameControl.instance.sound.startClickButtonAudio ();
        this.gameObject.SetActive (false);
        ////SendData.onSendCuocBC(tienchon);
    }
    public void onShow (long min, long max) {
        slider.value = 0;
        long temp = 0;
        temp = BaseInfo.gI ().mainInfo.moneyChip;

        this.tienmin = min;
        this.tienmax = max;
        if(temp < min) {
            min = temp;
            max = temp;
        }

        if(tienmax > temp) {
            tienmax = temp;
        }
        this.gameObject.SetActive (true);
    }
}
