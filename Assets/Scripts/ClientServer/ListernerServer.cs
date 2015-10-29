using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ListernerServer : IChatListener {
    GameControl gameControl;

    public void initConnect () {
        NetworkUtil.GI ().registerHandler (ProcessHandler.getInstance ());
        ProcessHandler.setListenner (this);
        PHandler.setListenner (this);
    }

    public ListernerServer (GameControl gameControl) {
        // TODO Auto-generated constructor stub
        this.gameControl = gameControl;
        initConnect ();
    }

    public void onDisConnect () {
        gameControl.panelWaiting.onHide ();
        gameControl.panelThongBao.onShowDCN ("Mất kết nối!", delegate {
            gameControl.disableAllDialog ();
            //SoundManager.Get().pauseAudio(SoundManager.AUDIO_TYPE.COUNT_DOWN);
            gameControl.setStage (gameControl.login);
            gameControl.resetGame ();
            NetworkUtil.GI ().close ();
        });
    }
}