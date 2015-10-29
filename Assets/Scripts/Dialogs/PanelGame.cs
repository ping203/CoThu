using UnityEngine;
using System.Collections;

public class PanelGame : MonoBehaviour {
    public GameObject group;
    public bool isShow = false;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
    void Awake () {
        //group.gameObject.transform.position = new Vector3(0, 500, 0);
    }
    public virtual void onShow () {
        OnHideKeyBoard ();
        show ();
    }
    void show () {
        isShow = true;
        this.gameObject.SetActive (true);
    }
    public virtual void onHide () {
        hide ();
    }
    void hide () {
        Invoke ("disApear", 0f);
    }
    void disApear () {
        isShow = false;
        this.gameObject.SetActive (false);
    }

    void FixedUpdate () {
        if(Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
            OnHideKeyBoard ();
        }
    }

    public void OnShowKeyBoard () {
        if(Input.deviceOrientation != DeviceOrientation.Portrait && Input.deviceOrientation != DeviceOrientation.PortraitUpsideDown)
            if(group != null)
                TweenPosition.Begin (group, 0.1f, new Vector3 (0, 160, 0));
    }

    public void OnHideKeyBoard () {
        if(group != null)
            TweenPosition.Begin (group, 0.01f, new Vector3 (0, 0, 0));
    }
}
