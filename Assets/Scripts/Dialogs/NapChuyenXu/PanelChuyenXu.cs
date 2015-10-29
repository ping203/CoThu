using UnityEngine;
using System.Collections;

public class PanelChuyenXu : PanelGame {

	public UIInput ip_userId, ip_xu;
	public UISlider sliderSoXu;

	// Use this for initialization
	void Start () {
		EventDelegate.Set (sliderSoXu.onChange, onChangeValue);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onChangeValue(){
		//ip_xu.value = (int)(BaseInfo.gI ().mainInfo.moneyXu * sliderSoXu.value) + "";
	}

    public void onClickChuyenXu () {
		if (!BaseInfo.gI().checkNumber(ip_userId.value.Trim()) || !BaseInfo.gI().checkNumber(ip_xu.value.Trim())) {
            GameControl.instance.panelThongBao.onShow ("Nhập sai!", delegate { });
			return;

		}
		if (ip_userId.value.Trim ().Equals ("") || ip_xu.value.Trim ().Equals ("")) {
            GameControl.instance.panelThongBao.onShow ("Vui lòng nhập đầy đủ thông tin!", delegate { });
			return;
		}

		long userid = long.Parse (ip_userId.value.Trim());
		long xu = long.Parse (ip_xu.value.Trim());

		////SendData.onXuToNick (userid, xu);
	}

    public void TuChoi () {
       // //GameControl.instance.sound.startClickButtonAudio ();
		ip_userId.value = "";
		ip_xu.value = "";
		sliderSoXu.value = 0;
	}
}
