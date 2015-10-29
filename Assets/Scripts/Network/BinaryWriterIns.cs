using UnityEngine;
using System;
using System.Text;
using System.Globalization;
using System.Security;
using System.Runtime.InteropServices;

namespace System.IO
{
    [Serializable]
    [ComVisible(true)]
    public class BinaryWriterIns : IDisposable
    {

        // Null is a BinaryWriter with no backing store.
        public static readonly BinaryWriterIns Null = new BinaryWriterIns();

        protected Stream OutStream;
        private Encoding m_encoding;
        private byte[] buffer;
        byte[] stringBuffer;
        int maxCharsPerRound;
        bool disposed;

        protected BinaryWriterIns()
            : this(Stream.Null, Encoding.UTF8)
        {
        }

        public BinaryWriterIns(Stream output)
            : this(output, Encoding.UTF8)
        {
        }


        const bool leave_open = false;

        public BinaryWriterIns(Stream output, Encoding encoding)

        {
            if (output == null)
                throw new ArgumentNullException("output");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            if (!output.CanWrite)
                throw new ArgumentException("Stream does not support writing or already closed.");


            OutStream = output;
            m_encoding = encoding;
            buffer = new byte[16];
        }

        public virtual Stream BaseStream
        {
            get
            {
                Flush();
                return OutStream;
            }
        }

        public virtual void Close()
        {
            Dispose(true);
        }


        void IDisposable.Dispose()
         {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && OutStream != null && !leave_open)
                OutStream.Close();

            buffer = null;
            m_encoding = null;
            disposed = true;
        }

        public virtual void Flush()
        {
            OutStream.Flush();
        }

        public virtual long Seek(int offset, SeekOrigin origin)
        {

            return OutStream.Seek(offset, origin);
        }

        public virtual void Write(bool value)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            buffer[0] = (byte)(value ? 1 : 0);
            OutStream.Write(buffer, 0, 1);
        }

        public virtual void Write(byte value)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            OutStream.WriteByte(value);
        }

        public virtual void Write(byte[] buffer)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            if (buffer == null)
                throw new ArgumentNullException("buffer");
            OutStream.Write(buffer, 0, buffer.Length);
        }

        public virtual void Write(byte[] buffer, int index, int count)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            if (buffer == null)
                throw new ArgumentNullException("buffer");
            OutStream.Write(buffer, index, count);
        }

        public virtual void Write(char ch)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            char[] dec = new char[1];
            dec[0] = ch;
            byte[] enc = m_encoding.GetBytes(dec, 0, 1);
            OutStream.Write(enc, 0, enc.Length);
        }

        public virtual void Write(char[] chars)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            if (chars == null)
                throw new ArgumentNullException("chars");
            byte[] enc = m_encoding.GetBytes(chars, 0, chars.Length);
            OutStream.Write(enc, 0, enc.Length);
        }

        public virtual void Write(char[] chars, int index, int count)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            if (chars == null)
                throw new ArgumentNullException("chars");
            byte[] enc = m_encoding.GetBytes(chars, index, count);
            OutStream.Write(enc, 0, enc.Length);
        }

         public virtual void Write(decimal value)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");
            OutStream.Write(buffer, 0, 16);
        }

        public virtual void Write(double value)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            OutStream.Write(BitConverter.GetBytes(value), 0, 8);
        }

        public virtual void Write(short value)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            OutStream.Write(buffer, 0, 2);
        }

        public virtual void Write(int value)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            buffer[2] = (byte)(value >> 16);
            buffer[3] = (byte)(value >> 24);
            OutStream.Write(buffer, 0, 4);
        }


        public void WriteByte(int value)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            buffer[0] = (byte)value;
          // buffer[1] = (byte)(value >> 8);
            //buffer[2] = (byte)(value >> 16);
            //buffer[3] = (byte)(value >> 24);
            OutStream.Write(buffer, 0, 1);
        }

        public virtual void Write(long value)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            for (int i = 0, sh = 0; i < 8; i++, sh += 8)
                buffer[i] = (byte)(value >> sh);
            OutStream.Write(buffer, 0, 8);
        }

        [CLSCompliant(false)]
        public virtual void Write(sbyte value)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            buffer[0] = (byte)value;
            OutStream.Write(buffer, 0, 1);
        }

        public virtual void Write(float value)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            OutStream.Write(BitConverter.GetBytes(value), 0, 4);
        }

        public virtual void Write(string value)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            int len = m_encoding.GetByteCount(value);
            Write7BitEncodedInt(len);

            if (stringBuffer == null)
            {
                stringBuffer = new byte[512];
                maxCharsPerRound = 512 / m_encoding.GetMaxByteCount(1);
            }

            int chpos = 0;
            int chrem = value.Length;
            while (chrem > 0)
            {
                int cch = (chrem > maxCharsPerRound) ? maxCharsPerRound : chrem;
                int blen = m_encoding.GetBytes(value, chpos, cch, stringBuffer, 0);
                OutStream.Write(stringBuffer, 0, blen);

                chpos += cch;
                chrem -= cch;
            }
        }

        [CLSCompliant(false)]
        public virtual void Write(ushort value)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            OutStream.Write(buffer, 0, 2);
        }

        [CLSCompliant(false)]
        public virtual void Write(uint value)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            buffer[2] = (byte)(value >> 16);
            buffer[3] = (byte)(value >> 24);
            OutStream.Write(buffer, 0, 4);
        }

        [CLSCompliant(false)]
        public virtual void Write(ulong value)
        {

            if (disposed)
                throw new ObjectDisposedException("BinaryWriter", "Cannot write to a closed BinaryWriter");

            for (int i = 0, sh = 0; i < 8; i++, sh += 8)
                buffer[i] = (byte)(value >> sh);
            OutStream.Write(buffer, 0, 8);
        }

        protected void Write7BitEncodedInt(int value)
        {
            do
            {
                int high = (value >> 7) & 0x01ffffff;
                byte b = (byte)(value & 0x7f);

                if (high != 0)
                {
                    b = (byte)(b | 0x80);
                }

                Write(b);
                value = high;
            } while (value != 0);
        }
    }
}


