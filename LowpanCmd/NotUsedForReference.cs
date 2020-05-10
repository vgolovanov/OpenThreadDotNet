using System;
using System.Collections.Generic;
using System.Text;

namespace OpenThread
{
    //       +----------+----------------------+---------------------------------+
    //|   Char   | Name                 | Description                     |
    //+----------+----------------------+---------------------------------+
    //|   "."    | DATATYPE_VOID        | Empty data type.Used           |
    //|          |                      | internally.                     |
    //|   "b"    | DATATYPE_BOOL        | Boolean value.Encoded in       |
    //|          |                      | 8-bits as either 0x00 or 0x01.  |
    //|          |                      | All other values are illegal.   |
    //|   "C"    | DATATYPE_UINT8       | Unsigned 8-bit integer.         |
    //|   "c"    | DATATYPE_INT8        | Signed 8-bit integer.           |
    //|   "S"    | DATATYPE_UINT16      | Unsigned 16-bit integer.        |
    //|   "s"    | DATATYPE_INT16       | Signed 16-bit integer.          |
    //|   "L"    | DATATYPE_UINT32      | Unsigned 32-bit integer.        |
    //|   "l"    | DATATYPE_INT32       | Signed 32-bit integer.          |
    //|   "i"    | DATATYPE_UINT_PACKED | Packed Unsigned Integer.See    |
    //|          |                      | Section 3.2.                    |
    //|   "6"    | DATATYPE_IPv6ADDR    | IPv6 Address. (Big-endian)      |
    //|   "E"    | DATATYPE_EUI64       | EUI-64 Address. (Big-endian)    |
    //|   "e"    | DATATYPE_EUI48       | EUI-48 Address. (Big-endian)    |
    //|   "D"    | DATATYPE_DATA        | Arbitrary data.See Section     |
    //|          |                      | 3.3.                            |
    //|   "d"    | DATATYPE_DATA_WLEN   | Arbitrary data with prepended   |
    //|          |                      | length.See Section 3.3.        |
    //|   "U"    | DATATYPE_UTF8        | Zero-terminated UTF8-encoded    |
    //|          |                      | string.                         |
    //| "t(...)" | DATATYPE_STRUCT      | Structured datatype with        |
    //|          |                      | prepended length. See Section   |
    //|          |                      | 3.4.                            |
    //| "A(...)" | DATATYPE_ARRAY       | Array of datatypes.Compound    |
    //|          |                      | type.See Section 3.5.          |
    //+----------+----------------------+---------------------------------+

//    available commands(type help < name > for more information):
//============================================================
//bufferinfo      channel         child              childmax      
//childtimeout    clear           commissioner
//bufferinfo         extaddr       ncp-filter        reset
//channel            extpanid      ncp-ll64          rloc16
//child              h             ncp-ml64          route
//childmax           help          ncp-raw           router
//childtimeout       history       ncp-tun           routerdowngradethreshold
//clear              ifconfig      netdataregister   routerselectionjitter
//commissioner       ipaddr        networkidtimeout  routerupgradethreshold
//contextreusedelay  joiner        networkname       scan
//counter            keysequence   panid             role
//debug              leaderdata    parent            thread
//debug-mem          leaderweight  ping              v
//                                 partition
//diag               macfilter     prefix            version
//discover           masterkey     q
//eidcache           mfg           quit
//exit               mode          releaserouterid
//";

    //  byte aaa = 0xbd;

    //  if (aaa > 0x7f)
    //  {

    //  }

    //  //sbyte ccc = Convert.ToSByte(aaa);

    //  int opa = aaa & 0x7f;
    //  int opa2 = aaa >> 7;

    //  sbyte zzz = -126;

    //  sbyte bbb = (sbyte)(aaa);
    //  Console.WriteLine(sizeof(sbyte));

    ////  Buffer.BlockCopy(aaa,)
    ///
    //byte[] asdas = new byte[1];
    //asdas[0] = BitConverter.GetBytes(byte.Parse("15"))[0];
    //        //    byte[] asdas= BitConverter.GetBytes(byte.Parse( "15"));

    //        byte aaaaa = Convert.ToByte("15");

    //var ports = SerialPort.GetPortNames();

    //SerialPort serialPort = new SerialPort();

    //byte TID = SpinelCommands.HEADER_DEFAULT;

    //byte[] header = BitConverter.GetBytes(TID);

    //// serialPort.

    ////if (args.Length == 0)
    ////{
    ////    Console.WriteLine("Hello - no args");
    ////}
    ////else
    ////{
    ////    for (int i = 0; i < args.Length; i++)
    ////    {
    ////        Console.WriteLine($"arg[{i}] = {args[i]}");
    ////    }
    ////}

    ////    SPINEL_CAP test = Enum.Parse(SPINEL_CAP, "1");

    //byte b = 15;
    //int o = 15 >> 8;
    //int a = 15 << 28;
    //int c = 150 | a;

    //byte[] array = new byte[6];
    //array[0] = 1;
    //        array[1] = 3;
    //        array[2] = 5;
    //        array[3] = 7;
    //        array[4] = 8;
    //        array[5] = 0;

    //        int index1 = Array.IndexOf(array, (byte)0);

    //if (Environment.OSVersion.Platform == PlatformID.Unix)
    //{



    //StreamMock streamMock = new StreamMock();
    //streamMock.AddMockData(mockData);

    //  StreamUART uartStream = new StreamUART("COM11");
    //StreamUART uartStream = new StreamUART("COM7");
    //  StreamUART uartStream = new StreamUART("COM4");

    //Hashtable mockData = new Hashtable()
    //        {
    //            { "81-02-36", "81-06-36-ff-ff"}, //get panid = 65535
    //            { "81-02-43", "81-06-43-00"},//get state = detached
    //         };
    //public enum IpProtocols
    //{
    //    ProtocolHopOpts = 0,  ///< IPv6 Hop-by-Hop Option
    //    ProtocolTcp = 6,  ///< Transmission Control Protocol
    //    ProtocolUdp = 17, ///< User Datagram
    //    ProtocolIp6 = 41, ///< IPv6 encapsulation
    //    ProtocolRouting = 43, ///< Routing Header for IPv6
    //    ProtocolFragment = 44, ///< Fragment Header for IPv6
    //    ProtocolIcmp6 = 58, ///< ICMP for IPv6
    //    ProtocolNone = 59, ///< No Next Header for IPv6
    //    ProtocolDstOpts = 60, ///< Destination Options for IPv6
    //}

    //private Hashtable payloadSize = new Hashtable()
    //{
    //    {SpinelDatatype.SPINEL_DATATYPE_BOOL_C, 1},
    //    {SpinelDatatype.SPINEL_DATATYPE_UINT8_C, 1},
    //    {SpinelDatatype.SPINEL_DATATYPE_INT8_C, 1},
    //    {SpinelDatatype.SPINEL_DATATYPE_UINT16_C, 2},
    //    {SpinelDatatype.SPINEL_DATATYPE_INT16_C, 2},
    //    {SpinelDatatype.SPINEL_DATATYPE_UINT32_C, 4},
    //    {SpinelDatatype.SPINEL_DATATYPE_INT32_C, 4},
    //    {SpinelDatatype.SPINEL_DATATYPE_UINT64_C, 8},
    //    {SpinelDatatype.SPINEL_DATATYPE_INT64_C, 8},
    //    {SpinelDatatype.SPINEL_DATATYPE_IPv6ADDR_C, 16},
    //    {SpinelDatatype.SPINEL_DATATYPE_EUI64_C, 8},
    //    {SpinelDatatype.SPINEL_DATATYPE_EUI48_C, 6},
    //};

    //public class SpinelDatatype
    //{
    //    //  public const char SPINEL_DATATYPE_NULL_C = '0';
    //    public const char SPINEL_DATATYPE_VOID_C = '.';
    //    public const char SPINEL_DATATYPE_BOOL_C = 'b';
    //    public const char SPINEL_DATATYPE_UINT8_C = 'C';
    //    public const char SPINEL_DATATYPE_INT8_C = 'c';
    //    public const char SPINEL_DATATYPE_UINT16_C = 'S';
    //    public const char SPINEL_DATATYPE_INT16_C = 's';
    //    public const char SPINEL_DATATYPE_UINT32_C = 'L';
    //    public const char SPINEL_DATATYPE_INT32_C = 'l';
    //    public const char SPINEL_DATATYPE_UINT64_C = 'X';
    //    public const char SPINEL_DATATYPE_INT64_C = 'x';
    //    public const char SPINEL_DATATYPE_UINT_PACKED_C = 'i';
    //    public const char SPINEL_DATATYPE_IPv6ADDR_C = '6';
    //    public const char SPINEL_DATATYPE_EUI64_C = 'E';
    //    public const char SPINEL_DATATYPE_EUI48_C = 'e';
    //    public const char SPINEL_DATATYPE_DATA_WLEN_C = 'd';
    //    public const char SPINEL_DATATYPE_DATA_C = 'D';
    //    public const char SPINEL_DATATYPE_UTF8_C = 'U'; //!< Zero-Terminated UTF8-Encoded String
    //    public const char SPINEL_DATATYPE_STRUCT_C = 't';
    //    public const char SPINEL_DATATYPE_ARRAY_C = 'A';
    //}

    //private SpinelEUI64 ReadEui64()
    //{
    //    SpinelEUI64 eui64 = new SpinelEUI64();

    //    eui64.bytes = ReadItems(eui64.bytes.Length);
    //    return eui64;

    //    //SpinelEUI64 spinel_eui64 = new SpinelEUI64();
    //    //Array.Copy(payload, spinel_eui64.bytes, 8);

    //    //return spinel_eui64;
    //}

    //private SpinelEUI48 ReadEui48()
    //{
    //    SpinelEUI48 eui48 = new SpinelEUI48();
    //    eui48.bytes = ReadItems(eui48.bytes.Length);
    //    return eui48;

    //    //SpinelEUI48 spinel_eui48 = new SpinelEUI48();
    //    //Array.Copy(payload, spinel_eui48.bytes, 6);

    //    //return spinel_eui48;
    //}


}
