using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PanelCreateRoom : PanelGame {
	public UISlider sliderMoney;
    public UIInput inputMoney;
    public UIInput inputMaxPlayer;

	float rateVIP, rateFREE;

	// Use this for initialization
	void Start () {
		EventDelegate.Set (sliderMoney.onChange, onChangeMoney);
	}

	public void onChangeMoney(){
	}

    public void createTableGame () {
	}

	public void onShow(){
		sliderMoney.value = 0;
		onChangeMoney();
		base.onShow ();
	}
}
