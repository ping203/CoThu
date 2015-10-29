using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class SendData {
    public static void onSendSms9029 (int telco) {
        //Message msg = new Message (CMDClient.CMD_SMS_9029);
        try {
           // msg.writer ().WriteInt (telco);
        } catch(Exception ex) {
            Debug.LogException (ex);
        }
       // NetworkUtil.GI ().sendMessage (msg);
    }
}
