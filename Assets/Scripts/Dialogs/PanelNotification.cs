using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PanelNotification : PanelGame
{
    public UILabel label;
    public UIButton btnOK;
    //public Button btnCancel;
    public delegate void CallBack();
    public CallBack onClickOK;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void onShow(string mess)
    {
        DoOnMainThread.ExecuteOnMainThread.Enqueue(() =>
        {
            label.text = mess;
            //btnCancel.gameObject.SetActive(true);
            btnOK.gameObject.SetActive(true);
            onShow();
        });
        
    }

    public void onShow(string mess, CallBack clickOK)
    {
        DoOnMainThread.ExecuteOnMainThread.Enqueue(() =>
        {
            label.text = mess;
            //btnCancel.gameObject.SetActive(true);
            btnOK.gameObject.SetActive(true);
            onClickOK = clickOK;
            onShow();
        });
      
    }
    public void onShowDCN(string mess, CallBack clickOK)
    {
        DoOnMainThread.ExecuteOnMainThread.Enqueue(() =>
        {
            label.text = mess;
            //btnCancel.gameObject.SetActive(false);
            btnOK.gameObject.SetActive(true);
            onClickOK = clickOK;
            onShow();
        });
      
    }
    public void onClickButtonOK()
    {
        onHide();
        onClickOK.Invoke();
    }
}
