using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemNhiemVu : MonoBehaviour {

    public int id;
    public int money;

    public UISprite icon;
    public UILabel lb_info, lb_process, lb_money;

    public UIButton buttonNT;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void onClickNhanThuong () {
        //SendData.onNhanmoneyquest ((sbyte) id);
        buttonNT.isEnabled = false;
    }

    public void setText (int id, int money, string info, string process, bool isSuccess) {
        //icon.spriteName = "icoNV" + id;
        this.id = id;
        this.money = money;
        this.lb_info.text = info;
        this.lb_money.text = money + " Xu";
        this.lb_process.text = process;
        this.buttonNT.isEnabled = isSuccess;
    }
}
