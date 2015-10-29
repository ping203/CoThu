using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item9029 : MonoBehaviour {

    public UILabel lb_vnd, lb_xu;
    public string name, sys;
       // vnd, xu;
    public short port;
    public long money;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   public void setText (string name, string sys, short port, long money) {
        this.name = name;
        this.sys = sys;
        this.port = port;
        this.money = money;

        lb_vnd.text = BaseInfo.formatMoneyDetailDot(long.Parse(name)) + " vnđ";
        lb_xu.text = BaseInfo.formatMoneyDetailDot(money)+  " xu";
    }
}
