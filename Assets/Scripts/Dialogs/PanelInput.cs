using UnityEngine;
using System.Collections;

public class PanelInput : PanelGame {
    public UILabel lb_title, lb_display_1, lb_display_2;
    public UIInput ip_enter;

    public UIButton btnOK;
    public delegate void CallBack ();
    public CallBack onClickOK;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onShow (string title, string display1, string display2, CallBack clickOK) {
         DoOnMainThread.ExecuteOnMainThread.Enqueue (() => {
        lb_title.text = title;
        lb_display_1.text = display1;
        lb_display_2.text = display2;
        btnOK.gameObject.SetActive (true);
        onClickOK = clickOK;
        base.onShow ();
         });
    }

    public void onClickButtonOK () {
        //GameControl.instance.sound.startClickButtonAudio ();
       // onHide ();
        onClickOK.Invoke ();
    }

    public int checkSDT (string sdt) {
        if(sdt.Length > 11 || sdt.Length < 10)
            return -3;

        for(int i = 0; i < sdt.Length; i++) {
            char c = sdt[i];
            if(('0' > c) || (c > '9')) {
                return -1;
            }
        }

        return 1;
    }

    void FixedUpdate () {
#if UNITY_WP8
        if(Input.GetButtonDown ("Fire1") && (this.transform.localPosition.y == 160)) {
            OnHideKeyBoard ();
        }

        if(Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
            OnHideKeyBoard ();
        }
#endif
    }

    public void OnShowKeyBoard () {
#if UNITY_WP8
        if(Input.deviceOrientation != DeviceOrientation.Portrait && Input.deviceOrientation != DeviceOrientation.PortraitUpsideDown)
            TweenPosition.Begin (this.gameObject, 0.1f, new Vector3 (0, 160, 0));
#endif
    }

    public void OnHideKeyBoard () {
        TweenPosition.Begin (this.gameObject, 0.01f, new Vector3 (0, 0, 0));
    }
}
