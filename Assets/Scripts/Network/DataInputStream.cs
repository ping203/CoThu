using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class DataInputStream {
    private byte[] ReadBuffer = new byte[8];

    private BinaryReaderEx ClientInput;

    public DataInputStream(BinaryReaderEx _clientInput) {
        ClientInput = _clientInput;
    }

    public int Read(byte[] b) {
        return ClientInput.Read(b, 0, b.Length);
    }
    public void Read(sbyte[] b) {
        int n = b.Length;
        for (int i = 0; i < n; i++) {
            b[i] = (sbyte)ClientInput.ReadByte();
        }
        //return ClientInput.Read(b, 0, b.Length);
    }
    public int Read(byte[] b, int off, int len) {
        return ClientInput.Read(b, off, len);
    }

    public void ReadFully(byte[] b) {
        ReadFully(b, 0, b.Length);
    }

    public void ReadFully(byte[] b, int off, int len) {
        if (len < 0) {
            throw new IndexOutOfRangeException();
        }

        int n = 0;
        while (n < len) {
            int count = ClientInput.Read(b, off + n, len - n);
            if (count < 0) {
                throw new EndOfStreamException();
            }
            n += count;
        }
    }

    public sbyte ReadByte() {
        return (sbyte)ClientInput.ReadByte();
    }

    public bool ReadBoolean() {
        return (ClientInput.ReadByte() != 0);
    }

    public String ReadUTF() {
        int utflen = this.ReadUnsignedShort();
        if (utflen >= 1000) {
            utflen = 0;
        }
        if (utflen > 30000) {
            Debug.Log("uuuuuuuuuuuuuuuuuu " + utflen);
        }
        byte[] bytearr = new byte[utflen * 2];
        char[] chararr = new char[utflen * 2];

        int c, char2, char3;
        int count = 0;
        int chararr_count = 0;

        this.ReadFully(bytearr, 0, utflen);

        while (count < utflen) {
            c = (int)bytearr[count] & 0xff;
            if (c > 127) break;
            count++;
            chararr[chararr_count++] = (char)c;
        }

        while (count < utflen) {
            c = (int)bytearr[count] & 0xff;
            switch (c >> 4) {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    /* 0xxxxxxx*/
                    count++;
                    chararr[chararr_count++] = (char)c;
                    break;
                case 12:
                case 13:
                    /* 110x xxxx   10xx xxxx*/
                    count += 2;
                    if (count > utflen)
                        throw new Exception(
                            "malformed input: partial character at end");

                    char2 = (int)bytearr[count - 1];
                    if ((char2 & 0xC0) != 0x80)
                        throw new Exception(
                            "malformed input around byte " + count);

                    chararr[chararr_count++] = (char)(((c & 0x1F) << 6) |
                                                      (char2 & 0x3F));
                    break;
                case 14:
                    /* 1110 xxxx  10xx xxxx  10xx xxxx */
                    count += 3;
                    if (count > utflen)
                        throw new Exception(
                            "malformed input: partial character at end");

                    char2 = (int)bytearr[count - 2];
                    char3 = (int)bytearr[count - 1];
                    if (((char2 & 0xC0) != 0x80) || ((char3 & 0xC0) != 0x80))
                        throw new Exception(
                            "malformed input around byte " + (count - 1));

                    chararr[chararr_count++] = (char)(((c & 0x0F) << 12) |
                                                      ((char2 & 0x3F) << 6) |
                                                      ((char3 & 0x3F) << 0));
                    break;
                default:
                    /* 10xx xxxx,  1111 xxxx */
                    throw new Exception(
                        "malformed input around byte " + count);
            }
        }
        // The number of chars produced may be less than utflen
        return new String(chararr, 0, chararr_count);
    }

    public double ReadDouble() {
        return BitConverter.Int64BitsToDouble(ReadLong());
    }

    public float ReadFloat() {
        return (float)ReadDouble();
    }

    public int ReadInt() {
        sbyte b1 = (sbyte)ClientInput.ReadByte();
        sbyte b2 = (sbyte)ClientInput.ReadByte();
        sbyte b3 = (sbyte)ClientInput.ReadByte();
        sbyte b4 = (sbyte)ClientInput.ReadByte();
        return b1 << 24 | (b2 & 0xff) << 16 | (b3 & 0xff) << 8 | (b4 & 0xff);
    }

    public long ReadLong() {
        ReadFully(ReadBuffer, 0, 8);
        return (((long)ReadBuffer[0] << 56) +
                ((long)(ReadBuffer[1] & 255) << 48) +
                ((long)(ReadBuffer[2] & 255) << 40) +
                ((long)(ReadBuffer[3] & 255) << 32) +
                ((long)(ReadBuffer[4] & 255) << 24) +
                ((ReadBuffer[5] & 255) << 16) +
                ((ReadBuffer[6] & 255) << 8) +
                ((ReadBuffer[7] & 255) << 0));
    }

    public short ReadShort() {
        sbyte a1 = (sbyte)ClientInput.ReadByte();
        sbyte a2 = (sbyte)ClientInput.ReadByte();
        return (short)(((a1 & 0xff) << 8) | (a2 & 0xff));
    }

    public ushort ReadUnsignedShort() {
        sbyte a1 = (sbyte)ClientInput.ReadByte();
        sbyte a2 = (sbyte)ClientInput.ReadByte();
        return (ushort)(((a1 & 0xff) << 8) | (a2 & 0xff));
    }
    public void Close() {
        ClientInput.Close();
    }
}