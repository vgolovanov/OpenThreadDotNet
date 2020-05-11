# OpenThreadDotNet
.Net Core library is designed to work with the Thread network. In future is plans to port the library to nanoframework https://nanoframework.net/ and TinyClrOs https://www.ghielectronics.com/tinyclr/features/ platforms.

To develop 6LoWPAN applications, you will need a radio module running as an OpenThread Network Co-Processor (NCP). 
As Co-Processor device can be used any module from supported platforms https://openthread.io/platforms with NCP firmware. The library tested on TI CC2652 based board http://www.ti.com/tool/LAUNCHXL-CC26X2R1 and nrf52840 based modules http://www.skylabmodule.com/skylab-125k-ram-industry-grade-low-energy-multiprotocol-5-0-ant-bluetooth-module/

	NCP project is .Net implimenation of Spinel protocol.
	Networking project is high level API to manage Thread network.
	LowpanCmd project is console application to manage Thread network. Similar to https://github.com/openthread/pyspinel
	Samples project is basic samples how to use OpenThreadDotNet library.
	
With OpenThreadDotNet library is possible to scan for nearby wireless networks, join to the wireless networks and form a new wireless mesh network.

Create a new Thread wireless network we need just 4 lines of code and about 10 lines of code to run UDP server.
```csharp
	StreamUART uartStream = new StreamUART("COMxx");
	ncpInterface = new NcpInterface();     
	ncpInterface.Open(uartStream);	

	//ncpInterface.Form(networkname, channel, masterkey, panid);
	ncpInterface.Form("Networkname", 11, "00112233445566778899AABBCCDDEEFF", 1234);
           
	UdpSocket receiver = new UdpSocket();
            
	receiver.Bind(IPv6Address.IPv6Any, 1000);
            
	IPv6EndPoint remoteIp = null;	
			
	while (true)
	{
		if (receiver.Poll(-1, SelectMode.SelectRead))
                {		
			byte[] data = receiver.Receive(ref remoteIp);
                    	string message = Encoding.ASCII.GetString(data);
                    	Console.WriteLine("{0} bytes from {1} {2} {3}", message.Length, remoteIp.Address, remoteIp.Port, message);
                }
	}		
```
Join to existing Thread wireless network and UDP Client sending data packet to another node.
```csharp
	StreamUART uartStream = new StreamUART("COMxx");
	ncpInterface = new NcpInterface();     
	ncpInterface.Open(uartStream);	

	// ncpInterface.Join(networkname, channel, masterkey, xpanid,  panid);	
	ncpInterface.Join("Networkname", 11, "00112233445566778899AABBCCDDEEFF", "DEAD00BEEF00CAFE",  1234);
           
	byte[] data = Encoding.UTF8.GetBytes("Test UDP message.");
	   
	UdpSocket udpClient = new UdpSocket();
	udpClient.Connect("fdde:ad00:beef:0000:488e:85b6:46d6:4436", 1000);
        udpClient.Send(data, data.Length);
        udpClient.Close();
```

In OpenThreadDotNet project used portions of the code, ported code, technical details and ideas from projects:

	https://github.com/netduino/Netduino.IP
	https://github.com/dotnet/core/
	https://github.com/openthread/pyspinel
	https://github.com/openthread/openthread/tree/master/src/ncp
	https://github.com/joakimeriksson/jipv6
	https://github.com/androidthings/sample-lowpan
	https://www.winsocketdotnetworkprogramming.com/
