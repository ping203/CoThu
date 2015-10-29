using UnityEngine;
using System.Collections;

public class PanelWaiting : PanelGame
{
    float timeShow;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isShow)
        {
            timeShow = timeShow + Time.deltaTime;
            if (timeShow >= 15)
            {
                onHide();
                timeShow = 0;
            }
        }


    }

    public override void onShow()
    {
        isShow = true;
        this.gameObject.SetActive(true);
        timeShow = 0;
    }
    public override void onHide()
    {
        isShow = false;
        this.gameObject.SetActive(false);
    }
}
