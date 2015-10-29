using UnityEngine;
using System.Collections;
using System;

public class PHandler : MessageHandler{
    private static IChatListener listenner;
    private static PHandler instance;

    public PHandler()
    {

    }
    public static PHandler getInstance()
    {
        if (instance == null)
            instance = new PHandler();
        return instance;
    }

    public static void setListenner(ListernerServer listener)
    {
        listenner = listener;
    }

    protected override void serviceMessage(Message message, int messageId)
    {
        	try {
			int card = -1;
			string from = "", to = "";
            switch(messageId) {
            }
		} catch (Exception ex) {
            Debug.LogException(ex);
		}
    }

    public override void onConnectionFail()
    {
        throw new System.NotImplementedException();
    }

    public override void onDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public override void onConnectOk()
    {
        throw new System.NotImplementedException();
    }
}
