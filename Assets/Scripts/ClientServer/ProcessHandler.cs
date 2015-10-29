using UnityEngine;
using System.Collections;
using System;

public class ProcessHandler : MessageHandler {

    protected override void serviceMessage (Message message, int messageId) {
        try {
            DoOnMainThread.ExecuteOnMainThread.Enqueue(() => {
                int check = 0, card = -1;
                sbyte b;
                switch (messageId) {
                    default:

                        if (secondHandler != null) {
                            secondHandler.processMessage(message);
                        }
                        break;

                }
            });
        }
        catch (Exception ex) {
            Debug.LogException(ex);

        }
    }

    public override void onConnectionFail () {
        throw new System.NotImplementedException ();
    }

    public override void onDisconnected () {
        DoOnMainThread.ExecuteOnMainThread.Enqueue (() => {
            listenner.onDisConnect ();
        });

    }

    public override void onConnectOk () {
        Debug.Log ("Connect OK...");
    }

    private static ProcessHandler instance;
    int send = 0;
    static int step;
    private static IChatListener listenner;

    public ProcessHandler () {

    }

    public static ProcessHandler getInstance () {
        if(instance == null) {
            instance = new ProcessHandler ();
        }

        return instance;
    }

    public static void setListenner (ListernerServer listener) {
        listenner = listener;
    }

    public static void setSecondHandler (MessageHandler handler) {
        secondHandler = null;
        secondHandler = handler;
    }

    private static MessageHandler secondHandler;
}
