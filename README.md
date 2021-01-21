# OpenthreadDotNet
​	**OpenThreadDotNet** is a .Net library designed to work with the Thread network. To develop 6LoWPAN applications, you will need a radio module running as an OpenThread Network Co-Processor (NCP). More information is available on the webpage  https://openthread.io/platforms/co-processor 

​	Currently library works on Net Core platform and reduced versions of the .NET CLR, nanoFramework (https://nanoframework.net/) and TinyCLR (https://www.ghielectronics.com/tinyclr/features/) platforms. 

​	The end device can be used on any module from supported platforms https://openthread.io/platforms with NCP firmware. The library tested on TI CC2652 based board http://www.ti.com/tool/LAUNCHXL-CC26X2R1 and nrf52840 based modules http://www.skylabmodule.com/skylab-125k-ram-industry-grade-low-energy-multiprotocol-5-0-ant-bluetooth-module/ 

**OpenThreadDotNet** project is .Net implementation of Spinel protocol and high level API to manage Thread network.

**LowpanCmd** project is a console application to manage Thread networks. Similar to https://github.com/openthread/pyspinel

**SamplesNetCore**, **SamplesNanoFramework** and **SamplesTinyCLR** are basic examples on how to use the **OpenThreadDotNet** library.

With OpenThreadDotNet library is possible to scan for nearby wireless networks, join to the wireless networks and form a new wireless mesh network.

Form a new Thread wireless network and UDP server we need just 6 lines of code.

```csharp
        StreamUART uartStream = new StreamUART("COMxx");
           
        ncpInterface = new NcpInterface();     

        ncpInterface.Open(uartStream);	
         
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