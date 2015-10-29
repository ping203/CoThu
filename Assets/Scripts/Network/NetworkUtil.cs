using System.IO;
using System.Threading;
using System;
using UnityEngine;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Net.Sockets;

public class NetworkUtil {
    private MessageHandler messageHandler;
    public bool connected;
    protected static NetworkUtil instance;
    private Message message;
    Socket _socket = null;
    ManualResetEvent _clientDone = new ManualResetEvent(false);
    SocketAsyncEventArgs EventArgSend;
    SocketAsyncEventArgs EventArgRead;
    SocketAsyncEventArgs EventArgConnect;
    protected Thread connectThread;
    private int maxRetry = 1;
    public static NetworkUtil GI() {
        if (instance == null) {
            instance = new NetworkUtil();
        }
        return instance;

    }

    public bool isConnected() {

        return connected;
    }

    public void registerHandler(MessageHandler messageHandler) {
        this.messageHandler = messageHandler;
    }

    public void connect(Message message) {
        if (!connected) {
            if (connectThread != null) {
                if (connectThread.IsAlive) {
                    return;
                }
            }
            this.message = message;
            connectThread = new Thread(new ThreadStart(runConnect));
            connectThread.Start();
        }
        else {
            if (message != null) {
                sendMessage(message);
            }

        }
    }

    private void runConnect() {
        //try {
        //    EndPoint hostEntry = UnityPluginForWindowPhone.Class1.getEndPoint(Res.IP, Res.PORT);
        //    _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    EventArgSend = new SocketAsyncEventArgs();
        //    EventArgRead = new SocketAsyncEventArgs();
        //    EventArgConnect = new SocketAsyncEventArgs();
        //    EventArgRead.Completed += new EventHandler<SocketAsyncEventArgs>(Read_Completed);
        //    EventArgConnect.Completed += new EventHandler<SocketAsyncEventArgs>(Connect_Completed);
        //    EventArgConnect.RemoteEndPoint = hostEntry;
        //    byte[] readBuffer = new byte[64 * 1024];
        //    EventArgRead.RemoteEndPoint = hostEntry;
        //    EventArgRead.SetBuffer(readBuffer, 0, readBuffer.Length);
        //    EventArgRead.UserToken = _socket;

        //    EventArgSend.RemoteEndPoint = hostEntry;
        //    EventArgSend.UserToken = null;
        //}
        //catch (Exception ex) {
        //    Debug.Log("Unable to connect to internet!" + ex);
        //    return;
        //}
        //Connect();
    }

    public void Connect() {
        //Debug.Log("========try connect=========== : " + Res.IP);
        //_socket.ConnectAsync(EventArgConnect);
        //_clientDone.WaitOne(5000);
        //if (!connected && maxRetry < 4) {
        //    maxRetry++;
        //    Connect();
        //}
        //else if (!connected && maxRetry >= 4) {
        //    close();
        //}
    }
    private void Connect_Completed(object sender, SocketAsyncEventArgs e) {
        switch (e.LastOperation) {
            case SocketAsyncOperation.Connect:
                ProcessConnect(e);
                break;
        }
    }
    private void Read_Completed(object sender, SocketAsyncEventArgs e) {
        switch (e.LastOperation) {
            case SocketAsyncOperation.Receive:
                ProcessReceive(e);
                break;
        }
    }
    private void ProcessConnect(SocketAsyncEventArgs e) {
        if (e.SocketError == SocketError.Success) {
            connected = true;
            maxRetry = 1;
            _clientDone.Set();
            messageHandler.onConnectOk();
            _socket.NoDelay = true;
            //_socket.ReceiveTimeout = 60000;
            _socket.ReceiveAsync(EventArgRead);
            if (message != null) {
                sendMessage(message);
            }
        }
    }

    private void ProcessReceive(SocketAsyncEventArgs e) {
        if (e.BytesTransferred > 0) {
            if (e.SocketError == SocketError.Success) {
                // Retrieve the data from the buffer
                processMsgFromData(e.Buffer, e.BytesTransferred);
                byte[] readBuffer = new byte[64 * 1024];
                e.SetBuffer(readBuffer, 0, readBuffer.Length);
                _socket.ReceiveAsync(e);
            }
            else if (e.SocketError == SocketError.ConnectionAborted
                || e.SocketError == SocketError.ConnectionReset
                || e.SocketError == SocketError.TimedOut) {
                if (connected) {
                    if (messageHandler != null) {
                        messageHandler.onDisconnected();
                    }
                    cleanNetwork();
                }
            }
        }
        else {
            if (connected) {
                if (messageHandler != null) {
                    messageHandler.onDisconnected();
                }
                cleanNetwork();
            }
        }
    }

    public void sendMessage(Message msg) {
        try {
            byte[] bytes = msg.toByteArray();
            EventArgSend.SetBuffer(bytes, 0, bytes.Length);
            Debug.Log("Send : " + msg.command);
            _socket.SendAsync(EventArgSend);
        }
        catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    private void processMsgFromData(byte[] data, int range) {
        //List<Message> listMsg = new List<Message>();
        sbyte command = 0;
        int count = 0;
        int size = 0;
        try {
            if (range <= 0)
                return;
            Message msg;
            do {
                command = (sbyte)data[count];
                count++;
                sbyte a1 = (sbyte)data[count];
                count++;
                sbyte a2 = (sbyte)data[count];
                count++;
                size = ((a1 & 0xff) << 8) | (a2 & 0xff);
                byte[] subdata = new byte[size];
                Debug.Log("Read == " + command);
                Buffer.BlockCopy(data, count, subdata, 0, size);
                count += size;
                msg = new Message(command, subdata);
                //listMsg.Add(msg);
                messageHandler.processMessage(msg);
                //Debug.Log("11111111111111 " + System.cu);
                //Debug.Log("CMDDDDDDDD ");
                //Debug.Log("COUNTTTTTT ");
                //Debug.Log("RANGEEEEEE ");
                //Debug.Log("2222222222222 " + DateTime.UtcNow);
                Thread.Sleep(70);
            } while (count < range);
            //foreach (Message m in listMsg) {
            //    messageHandler.processMessage(m);
            //}
        }
        catch (Exception ex) {
            Debug.LogException(ex);
        }
    }

    public void close() {
        Debug.Log("Close current socket!");
        cleanNetwork();
    }

    public void cleanNetwork() {
        try {
            connected = false;
            if (_socket != null) {
                try {
                    _socket.Close();
                }
                catch (SocketException ex) {
                    Debug.LogException(ex);
                }

            }
            if (EventArgRead != null) {
                EventArgRead.Dispose();
            }
            if (EventArgSend != null) {
                EventArgSend.Dispose();
            }
            if (EventArgConnect != null) {
                EventArgConnect.Dispose();
            }
            maxRetry = 1;
            connectThread = null;
            _clientDone.Close();
            _clientDone = new ManualResetEvent(false);
        }
        catch (Exception e) {
            Debug.LogException(e);
        }
        finally {
            if (connectThread != null && connectThread.IsAlive) {
                connectThread.Abort();
            }
        }
    }

    public void resume(bool pausestatus) {
        //if (pausestatus) {

        //}
        //else {
        //    if (GameControl.instance.currenStage != GameControl.instance.login) {
        //        GameControl.instance.setStage(GameControl.instance.login);
        //        GameControl.instance.disableAllDialog();
        //        close();
        //        if (!BaseInfo.gI().username.Equals("")) {
        //            GameControl.instance.login.doLogin(BaseInfo.gI().username, BaseInfo.gI().pass);
        //        }
        //    }
        //}

    }
}
