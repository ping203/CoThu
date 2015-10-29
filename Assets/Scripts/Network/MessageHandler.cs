using UnityEngine;
using System.Collections;
using System;
public abstract class MessageHandler
{

    protected abstract void serviceMessage(Message message, int messageId);

    public MessageHandler()
    {
    }

    public void processMessage(Message message)
    {
        int messageType = message.command;//lay cmd cho tung msg
        try
        {
            serviceMessage(message, messageType);
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public abstract void onConnectionFail();

    public abstract void onDisconnected();

    public abstract void onConnectOk();
}
