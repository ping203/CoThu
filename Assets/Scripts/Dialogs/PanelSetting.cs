using UnityEngine;
using System.Collections;

public class PanelSetting : PanelGame {
    public UIToggle nhacnen, rung, nhanloimoichoi;
    // Use this for initialization
    void Start () {

    }
    void Awake () {
        //nhacnen.value = PlayerPrefs.GetInt("nhacnen") == 1;
        // rung.value = PlayerPrefs.GetInt("rung") == 1;
        // nhanloimoichoi.value = PlayerPrefs.GetInt("nhanloimoichoi") == 1;
        nhanloimoichoi.value = BaseInfo.gI ().isNhanLoiMoiChoi;
        nhacnen.value = BaseInfo.gI ().isSound;
        rung.value = BaseInfo.gI ().isVibrate;
    }

    public void onChangeVL () {
        nhanloimoichoi.value = BaseInfo.gI ().isNhanLoiMoiChoi;
    }
    // Update is called once per frame
    void Update () {
        onChangeVL ();
    }
    public void clickNhacNen () {
        PlayerPrefs.SetInt ("sound", nhacnen.value ? 0 : 1);
        PlayerPrefs.Save ();
        BaseInfo.gI ().isSound = nhacnen.value;
        //GameControl.instance.sound.startClickButtonAudio ();
        /*if (nhacnen.value)
        {
             //SoundManager.Get().startAudio(SoundManager.AUDIO_TYPE.BKG_MUSIC);
        }
        else
        {
            //SoundManager.Get().pauseAudio(SoundManager.AUDIO_TYPE.BKG_MUSIC);
        }*/
    }
    public void clickRung () {
        PlayerPrefs.SetInt ("rung", rung.value ? 0 : 1);
        PlayerPrefs.Save ();

        BaseInfo.gI ().isVibrate = rung.value;
        //GameControl.instance.sound.startClickButtonAudio ();
    }

    public void clickNhanLoiMoiChoi () {
        BaseInfo.gI ().isNhanLoiMoiChoi = nhanloimoichoi.value;
        //GameControl.instance.sound.startClickButtonAudio ();
    }
}
