using UnityEngine;
using System.Collections;
using System.IO;
using System;
public class Message {

    public sbyte command;
    //public BinaryReader dis;
    //public BinaryWriter dos;
    //public MemoryStream outStream;
    public BinaryReaderEx iss;
    public BinaryWriterIns os;
    public DataOutputStream dos = null;
    public DataInputStream dis = null;
    private MemoryStream ms = null;
    public Message(sbyte command) {
        this.command = command;
        ms = new MemoryStream();
        os = new BinaryWriterIns(ms);
        dos = new DataOutputStream(os);
    }
    public Message(sbyte command, byte[] data) {

        this.command = command;
        ms = new MemoryStream(data);
        iss = new BinaryReaderEx(ms);
        dis = new DataInputStream(iss);
    }
    public byte[] toByteArray() {
        short datalen = 0;
        byte[] data = null;
        byte[] bytes = null;
        byte[] byteNew = null;
        try {
            if (dos != null) {
                dos.Flush();
                data = ms.ToArray();
                datalen = (short)data.Length;
                dos.Close();
            }
            MemoryStream bos1 = new MemoryStream(datalen + 3);
            DataOutputStream dos1 = new DataOutputStream(new BinaryWriterIns(bos1));
            dos1.WriteByteNew(command);
            dos1.WriteShort(datalen);
            if (datalen > 0) {
                dos1.Write(data);
            }
            bytes = bos1.ToArray();
            byteNew = new byte[bytes.Length - 3];
            int n = byteNew.Length;
            Array.Copy(bytes, 3, byteNew, 0, n);
            byteNew[0] = (byte)command;
            dos1.Close();
        }
        catch (IOException e) {
            Debug.Log(e.ToString());
        }
        return byteNew;
    }
    public override bool Equals(object obj) {
        if (this == obj)
            return true;
        if (obj == null)
            return false;
        if (this.GetType() != obj.GetType())
            return false;
        Message other = (Message)obj;
        return other.command == command;
    }

    public override int GetHashCode() {
        // Which is preferred?

        return command;

        //return this.FooId.GetHashCode();
    }
    public DataInputStream reader() {
        return dis;

    }

    public DataOutputStream writer() {
        return dos;
    }
}
