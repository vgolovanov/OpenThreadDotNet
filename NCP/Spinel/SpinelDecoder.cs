using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace OpenThreadDotNet.Spinel
{
    public class SpinelDecoder
    {
        private byte[] frameBuffer; // Frame buffer.
        private int frameLength; // Frame length (number of bytes).
        private int frameIndex; // Current read index.
        private int frameEndIndex; // Current end index (end of struct if in a struct, or end of buffer otherwise).

        public byte FrameHeader { get; private set; }

        public uint FrameCommand { get; private set; }

        public uint FramePropertyId { get; private set; }

        public SpinelDecoder()
        {
            this.frameLength = 0;
            this.frameIndex = 0;
            this.frameEndIndex = 0;
        }

        public void Init(byte [] FrameIn)
        {
            frameBuffer = FrameIn;
            frameLength = FrameIn.Length;
            Reset();

            FrameHeader = ReadUint8();
            FrameCommand= ReadUintPacked();

            if(FrameCommand != SpinelCommands.RSP_PROP_VALUE_IS && FrameCommand != SpinelCommands.RSP_PROP_VALUE_INSERTED && FrameCommand!= SpinelCommands.RSP_PROP_VALUE_REMOVED)
            {
                throw new SpinelFormatException("Property Unknown.");
            }

            FramePropertyId = ReadUintPacked();
        }

        public void Reset()
        {
            frameIndex = 0;
            frameEndIndex=frameLength;
        }

        public byte[] GetFrameLoad()
        {
            byte[] frameLoad = new byte[frameBuffer.Length - frameIndex];
            Array.Copy(frameBuffer, frameIndex, frameLoad, 0, frameLoad.Length);
            return frameLoad;
        }

        public string ReadUtf8()
        {
            Encoding utf8 = Encoding.UTF8;

            int indexOfNull = Array.IndexOf(frameBuffer, (byte)0, frameIndex);

            if (indexOfNull == -1)
            {
                throw new SpinelFormatException("String dosn't contains zero-termination character.");
            }

            int arrayLength = indexOfNull - frameIndex;

            byte[] segment = new byte[arrayLength];

            Array.Copy(frameBuffer, frameIndex, segment, 0, arrayLength);

            frameIndex += arrayLength + 1; // +1 \0 char removed from the array

            return utf8.GetString(segment,0,segment.Length);
        }

        public uint ReadUintPacked()
        {
            uint valueDecoded = 0;
            var value_mul = 1;
            var value_len_max = frameIndex + 4;

            while (frameIndex < value_len_max)
            {
                var packet = frameBuffer[frameIndex];

                valueDecoded += (uint)((packet & 0x7F) * value_mul);
                frameIndex += sizeof(byte);

                if (packet < 0x80)
                {
                    break;
                }

                value_mul *= 0x80;
            }

            return valueDecoded;
        }

        public byte[] ReadData()
        {
            int aDataLen = frameEndIndex - frameIndex;
            return ReadItems(aDataLen);
        }

        public byte[] ReadDataWithLen()
        {
            int aDataLen = ReadUint16();
            return ReadItems(aDataLen);
        }

        public bool ReadBool()
        {
            byte valueToDecode = ReadUint8();

            if (valueToDecode == 0x00)
            {
                return false;
            }
            else if (valueToDecode == 0x01)
            {
                return true;
            }
            else
            {
                throw new SpinelFormatException( "Parsing boolean value error.");
            }
        }

        public byte ReadUint8()
        {
            byte decodedValue = frameBuffer[frameIndex];
            frameIndex += sizeof(byte);

            return decodedValue;
        }

        public sbyte Readint8()
        {            
            return (sbyte)ReadUint8();
        }

        public ushort ReadUint16()
        {
            ushort aUint16 = (ushort)(frameBuffer[frameIndex] | (frameBuffer[frameIndex + 1] << 8));

            frameIndex += sizeof(ushort);

            return aUint16;
        }

        public short ReadInt16()
        {
            return (short)ReadUint16();
        }

        public uint ReadUint32()
        {
            uint aUint32 = (uint)((frameBuffer[frameIndex + 3] << 24) | (frameBuffer[frameIndex + 2] << 16) | (frameBuffer[frameIndex + 1] << 8) | frameBuffer[frameIndex]);

            frameIndex += sizeof(uint);

            return aUint32;
        }

        public int ReadInt32()
        {
            return (int)ReadUint32();
        }

        public SpinelIPv6Address ReadIp6Address()
        {
            SpinelIPv6Address ipAddress = new SpinelIPv6Address();
            ipAddress.bytes = ReadItems(ipAddress.bytes.Length);
            return ipAddress;
        }

        public SpinelEUI64 ReadEui64()
        {
            SpinelEUI64 eui64 = new SpinelEUI64();

            eui64.bytes = ReadItems(eui64.bytes.Length);
            return eui64;
        }

        public SpinelEUI48 ReadEui48()
        {
            SpinelEUI48 eui48 = new SpinelEUI48();
            eui48.bytes = ReadItems(eui48.bytes.Length);
            return eui48;
        }


        private object SpinelDatatypeValueUnpack(char spinel_format)
        {
            switch (spinel_format)
            {
                case 'b':
                    return ReadBool();

                case 'c':
                    return Readint8();

                case 'C':
                    return ReadUint8();

                case 's':
                    return ReadInt16();

                case 'S':
                    return ReadUint16();

                case 'L':
                    return ReadUint32();

                case 'l':
                    return ReadInt32();

                case '6':
                    return ReadIp6Address();

                case 'E':
                    return ReadEui64();

                case 'e':
                    return ReadEui48();

                case 'U':
                    return ReadUtf8();

                case 'D':
                    return ReadData();

                case 'd':
                    return ReadDataWithLen();

                case 'i':
                    return ReadUintPacked();

                default:
                    throw new SpinelFormatException("Parsing frame data error.");
            }
        }
       
     

        private byte[] ReadItems (int SizeToRead)
        {
            byte[] tempArray = new byte[SizeToRead];

            for (int i = 0; i < SizeToRead; i++)
            {
                tempArray[i] = ReadUint8();
            }

            return tempArray;
        }

        public ArrayList ReadFields(string spinelFormat)
        {
            int indexFormat = 0;
            ArrayList result = new ArrayList();

            while (indexFormat < spinelFormat.Length)
            {
                char format = spinelFormat[indexFormat];

                if (format == 'A')
                {
                    if (spinelFormat[indexFormat + 1] != '(')
                    {
                        throw new SpinelFormatException("Incorrect structure format.");
                    }

                    int array_end = GetIndexOfEndingBrace(spinelFormat, indexFormat + 1);
                    string array_format = spinelFormat.Substring(indexFormat + 2, array_end - 2 + indexFormat);

                    ArrayList array = new ArrayList();

                    while (frameIndex <= frameBuffer.Length - 1)
                    {
                        array.AddRange(ReadFields(array_format));
                    }

                    indexFormat = array_end + 1;
                    result.AddRange(array);
                }
                else if (format == 't')
                {
                    if (spinelFormat[indexFormat + 1] != '(')
                    {
                        throw new SpinelFormatException( "Incorrect structure format.");
                    }

                    int struct_end = GetIndexOfEndingBrace(spinelFormat, indexFormat + 1);
                    string struct_format = spinelFormat.Substring(indexFormat + 2, struct_end-indexFormat-2);
                    ReadUint16();
                    //ushort struct_len = ReadUint16();             
                    result.Add(ReadFields(struct_format));
                    indexFormat = struct_end + 1;
                 
                }
                else
                {
                    result.Add(SpinelDatatypeValueUnpack(format));
                    indexFormat += 1;
                }
            }

            return result;
        }

        private int GetIndexOfEndingBrace(string spinel_format,int idx)
        {
            int count = 1;

            while (count > 0 && idx < spinel_format.Length-1)
            {
                idx += 1;

                if (spinel_format[idx] == ')')
                {
                    count -= 1;
                }

                if (spinel_format[idx] == '(')
                {
                    count += 1;
                }
            }

            if (count != 0)
            {
                throw new SpinelFormatException("Unbalanced parenthesis in format string {0}" + spinel_format + ", idx=" +  idx);
            }

            return idx;
        }
    }
}
