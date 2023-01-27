using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace NetSerializer
{
    public static class Primitives
    {
        [ThreadStatic]
        private static Primitives.StringHelper s_stringHelper;

        private readonly static byte[] s_emptyByteArray;

        static Primitives()
        {
            Primitives.s_emptyByteArray = new byte[0];
        }

        private static int DecodeZigZag32(uint n)
        {
            return (int)(n >> 1 ^ -(n & 1));
        }

        private static long DecodeZigZag64(ulong n)
        {
            return (long)(n >> 1) ^ -((long)n & 1L);
        }

        private static uint EncodeZigZag32(int n)
        {
            return (uint)(n << 1 ^ n >> 31);
        }

        private static ulong EncodeZigZag64(long n)
        {
            return (ulong)(n << 1 ^ n >> 63);
        }

        public static MethodInfo GetReaderPrimitive(Type type)
        {
            Type type1 = typeof(Primitives);
            Type[] typeArray = new Type[] { typeof(Stream), type.MakeByRefType() };
            return type1.GetMethod("ReadPrimitive", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, typeArray, null);
        }

        public static MethodInfo GetWritePrimitive(Type type)
        {
            Type type1 = typeof(Primitives);
            Type[] typeArray = new Type[] { typeof(Stream), type };
            return type1.GetMethod("WritePrimitive", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, typeArray, null);
        }

        public static void ReadPrimitive(Stream stream, out bool value)
        {
            value = stream.ReadByte() != 0;
        }

        public static void ReadPrimitive(Stream stream, out byte value)
        {
            value = (byte)stream.ReadByte();
        }

        public static void ReadPrimitive(Stream stream, out sbyte value)
        {
            value = (sbyte)stream.ReadByte();
        }

        public static void ReadPrimitive(Stream stream, out char value)
        {
            value = (char)Primitives.ReadVarint32(stream);
        }

        public static void ReadPrimitive(Stream stream, out ushort value)
        {
            value = (ushort)Primitives.ReadVarint32(stream);
        }

        public static void ReadPrimitive(Stream stream, out short value)
        {
            value = (short)Primitives.DecodeZigZag32(Primitives.ReadVarint32(stream));
        }

        public static void ReadPrimitive(Stream stream, out uint value)
        {
            value = Primitives.ReadVarint32(stream);
        }

        public static void ReadPrimitive(Stream stream, out int value)
        {
            value = Primitives.DecodeZigZag32(Primitives.ReadVarint32(stream));
        }

        public static void ReadPrimitive(Stream stream, out ulong value)
        {
            value = Primitives.ReadVarint64(stream);
        }

        public static void ReadPrimitive(Stream stream, out long value)
        {
            value = Primitives.DecodeZigZag64(Primitives.ReadVarint64(stream));
        }

        public static void ReadPrimitive(Stream stream, out float value)
        {
            unsafe
            {
                value = (float)Primitives.ReadVarint32(stream);
            }
        }

        public static void ReadPrimitive(Stream stream, out double value)
        {
            unsafe
            {
                value = (double)Primitives.ReadVarint64(stream);
            }
        }

        public static void ReadPrimitive(Stream stream, out DateTime value)
        {
            long num;
            Primitives.ReadPrimitive(stream, out num);
            value = DateTime.FromBinary(num);
        }

        public static void ReadPrimitive(Stream stream, out string value)
        {
            unsafe
            {
                uint num;
                uint num1;
                char[] chrArray;
                int num2;
                int num3;
                Primitives.ReadPrimitive(stream, out num);
                if (num == 0)
                {
                    value = null;
                    return;
                }
                if (num == 1)
                {
                    value = string.Empty;
                    return;
                }
                num--;
                Primitives.ReadPrimitive(stream, out num1);
                Primitives.StringHelper sStringHelper = Primitives.s_stringHelper;
                if (sStringHelper == null)
                {
                    Primitives.StringHelper stringHelper = new Primitives.StringHelper();
                    sStringHelper = stringHelper;
                    Primitives.s_stringHelper = stringHelper;
                }
                Decoder decoder = sStringHelper.Decoder;
                byte[] byteBuffer = sStringHelper.ByteBuffer;
                chrArray = (num1 > 128 ? new char[num1] : sStringHelper.CharBuffer);
                int num4 = (int)num;
                int num5 = 0;
                while (num4 > 0)
                {
                    int num6 = stream.Read(byteBuffer, 0, Math.Min((int)byteBuffer.Length, num4));
                    if (num6 == 0)
                    {
                        throw new EndOfStreamException();
                    }
                    num4 -= num6;
                    bool flag = (num4 == 0 ? true : false);
                    bool flag1 = false;
                    int num7 = 0;
                    while (!flag1)
                    {
                        decoder.Convert(byteBuffer, num7, num6 - num7, chrArray, num5, (int)(num1 - num5), flag, out num3, out num2, out flag1);
                        num7 += num3;
                        num5 += num2;
                    }
                }
                value = new string(chrArray, 0, (int)num1);
            }
        }

        public static void ReadPrimitive(Stream stream, out byte[] value)
        {
            unsafe
            {
                uint num;
                int num1 = 0;
                Primitives.ReadPrimitive(stream, out num);
                if (num == 0)
                {
                    value = null;
                    return;
                }
                if (num == 1)
                {
                    value = Primitives.s_emptyByteArray;
                    return;
                }
                num--;
                value = new byte[num];
                for (int i = 0; i < num; i += num1)
                {
                    num1 = stream.Read(value, i, (int)(num - i));
                    if (num1 == 0)
                    {
                        throw new EndOfStreamException();
                    }
                }
            }
        }

        private static uint ReadVarint32(Stream stream)
        {
            int num = 0;
            for (int i = 0; i < 32; i += 7)
            {
                int num1 = stream.ReadByte();
                if (num1 == -1)
                {
                    throw new EndOfStreamException();
                }
                num = num | (num1 & 127) << (i & 31);
                if ((num1 & 128) == 0)
                {
                    return (uint)num;
                }
            }
            throw new InvalidDataException();
        }

        private static ulong ReadVarint64(Stream stream)
        {
            long num = (long)0;
            for (int i = 0; i < 64; i += 7)
            {
                int num1 = stream.ReadByte();
                if (num1 == -1)
                {
                    throw new EndOfStreamException();
                }
                num = num | (long)(num1 & 127) << (i & 63);
                if ((num1 & 128) == 0)
                {
                    return (ulong)num;
                }
            }
            throw new InvalidDataException();
        }

        public static void WritePrimitive(Stream stream, bool value)
        {
            stream.WriteByte((byte)((value ? 1 : 0)));
        }

        public static void WritePrimitive(Stream stream, byte value)
        {
            stream.WriteByte(value);
        }

        public static void WritePrimitive(Stream stream, sbyte value)
        {
            stream.WriteByte((byte)value);
        }

        public static void WritePrimitive(Stream stream, char value)
        {
            Primitives.WriteVarint32(stream, value);
        }

        public static void WritePrimitive(Stream stream, ushort value)
        {
            Primitives.WriteVarint32(stream, value);
        }

        public static void WritePrimitive(Stream stream, short value)
        {
            Primitives.WriteVarint32(stream, Primitives.EncodeZigZag32(value));
        }

        public static void WritePrimitive(Stream stream, uint value)
        {
            Primitives.WriteVarint32(stream, value);
        }

        public static void WritePrimitive(Stream stream, int value)
        {
            Primitives.WriteVarint32(stream, Primitives.EncodeZigZag32(value));
        }

        public static void WritePrimitive(Stream stream, ulong value)
        {
            Primitives.WriteVarint64(stream, value);
        }

        public static void WritePrimitive(Stream stream, long value)
        {
            Primitives.WriteVarint64(stream, Primitives.EncodeZigZag64(value));
        }

        public static void WritePrimitive(Stream stream, float value)
        {
            unsafe
            {
                Primitives.WriteVarint32(stream, (uint)value);
            }
        }

        public static void WritePrimitive(Stream stream, double value)
        {
            unsafe
            {
                Primitives.WriteVarint64(stream, (ulong)value);
            }
        }

        public static void WritePrimitive(Stream stream, DateTime value)
        {
            Primitives.WritePrimitive(stream, value.ToBinary());
        }

        public static unsafe void WritePrimitive(Stream stream, string value)
        {
            if (value == null)
                Primitives.WritePrimitive(stream, 0U);
            else if (value.Length == 0)
            {
                Primitives.WritePrimitive(stream, 1U);
            }
            else
            {
                Primitives.StringHelper stringHelper = Primitives.s_stringHelper;
                if (stringHelper == null)
                    Primitives.s_stringHelper = stringHelper = new Primitives.StringHelper();
                Encoder encoder = stringHelper.Encoder;
                byte[] byteBuffer = stringHelper.ByteBuffer;
                int length = value.Length;
                int byteCount;
                fixed (char* chars = value)
                    byteCount = encoder.GetByteCount(chars, length, true);
                Primitives.WritePrimitive(stream, (uint)(byteCount + 1));
                Primitives.WritePrimitive(stream, (uint)length);
                int num = 0;
                bool completed = false;
                while (!completed)
                {
                    int charsUsed;
                    int bytesUsed;
                    fixed (char* chPtr = value)
                    fixed (byte* bytes = byteBuffer)
                        encoder.Convert(chPtr + num, length - num, bytes, byteBuffer.Length, true, out charsUsed, out bytesUsed, out completed);
                    stream.Write(byteBuffer, 0, bytesUsed);
                    num += charsUsed;
                }
            }
        }

        public static void WritePrimitive(Stream stream, byte[] value)
        {
            if (value == null)
            {
                Primitives.WritePrimitive(stream, (uint)0);
                return;
            }
            Primitives.WritePrimitive(stream, (uint)((int)value.Length + 1));
            stream.Write(value, 0, (int)value.Length);
        }

        private static void WriteVarint32(Stream stream, uint value)
        {
            while (value >= 128)
            {
                stream.WriteByte((byte)(value | 128));
                value >>= 7;
            }
            stream.WriteByte((byte)value);
        }

        private static void WriteVarint64(Stream stream, ulong value)
        {
            while (value >= (long)128)
            {
                stream.WriteByte((byte)(value | (long)128));
                value >>= 7;
            }
            stream.WriteByte((byte)value);
        }

        private sealed class StringHelper
        {
            public const int BYTEBUFFERLEN = 256;

            public const int CHARBUFFERLEN = 128;

            private Encoder m_encoder;

            private Decoder m_decoder;

            private byte[] m_byteBuffer;

            private char[] m_charBuffer;

            public byte[] ByteBuffer
            {
                get
                {
                    if (this.m_byteBuffer == null)
                    {
                        this.m_byteBuffer = new byte[256];
                    }
                    return this.m_byteBuffer;
                }
            }

            public char[] CharBuffer
            {
                get
                {
                    if (this.m_charBuffer == null)
                    {
                        this.m_charBuffer = new char[128];
                    }
                    return this.m_charBuffer;
                }
            }

            public Decoder Decoder
            {
                get
                {
                    if (this.m_decoder == null)
                    {
                        this.m_decoder = this.Encoding.GetDecoder();
                    }
                    return this.m_decoder;
                }
            }

            public Encoder Encoder
            {
                get
                {
                    if (this.m_encoder == null)
                    {
                        this.m_encoder = this.Encoding.GetEncoder();
                    }
                    return this.m_encoder;
                }
            }

            public UTF8Encoding Encoding
            {
                get;
                private set;
            }

            public StringHelper()
            {
                this.Encoding = new UTF8Encoding(false, true);
            }
        }
    }
}