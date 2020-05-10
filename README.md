# OpenthreadDotNet
.Net Core library is designed to work with the Thread network. In future is plans to port the library to nanoframework https://nanoframework.net/ and TinyClrOs https://www.ghielectronics.com/tinyclr/features/ platforms.

To develop 6LoWPAN applications, you will need a radio module running as an OpenThread Network Co-Processor (NCP). 
As Co-Processor device can be used any module from supported platforms https://openthread.io/platforms with NCP firmware. The library tested on TI CC2652 based board http://www.ti.com/tool/LAUNCHXL-CC26X2R1 and nrf52840 based modules http://www.skylabmodule.com/skylab-125k-ram-industry-grade-low-energy-multiprotocol-5-0-ant-bluetooth-module/

	NCP project is .Net implimenation of Spinel protocol.
	Networking project is high level API to manage Thread network.
	LowpanCmd project is console application to manage Thread network. Similar to https://github.com/openthread/pyspinel
	Samples project is basic samples how to use OpenThreadDotNet library.
	
With OpenThreadDotNet library is possible to scan for nearby wireless networks, join to the wireless networks and form a new wireless mesh network.

Create a new Thread wireless network and create UDP sever we need just 4 lines of code and about 10 lines of code to run UDP server.
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
                    Console.WriteLine("\n");
                    Console.WriteLine("{0} bytes from {1} {2} {3}", message.Length, remoteIp.Address, remoteIp.Port, message);
                    Console.WriteLine(">");
                }
            }		
```
