#define nanoFramework
using System;
using System.Diagnostics;
using System.Text;
using OpenThreadDotNet;
using OpenThreadDotNet.Networking.Lowpan;
using OpenThreadDotNet.Networking.Sockets;
using Windows.Devices.Gpio;
using nanoFramework.Hardware.Esp32;

namespace SamplesNanoFramework
{
    // Browse our samples repository: https://github.com/nanoframework/samples
    // Check our documentation online: https://docs.nanoframework.net/
    // Join our lively Discord community: https://discord.gg/gCyBu8T

    public class Program
    {
        private static NcpInterface ncpInterface = new NcpInterface();
        private static ushort port = 1234;
        private static string networkname = "OpenThreadNano";  
        private static string masterkey = "00112233445566778899aabbccddeeff";       
        private static byte channel = 11;
        private static ushort panid = 1000;
        private static GpioPin ledGreen;
        private static GpioController gpioController = new GpioController();

        private static void OnLastStatus(LastStatus lastStatus)
        {
            if (lastStatus.ToString().ToLower() != "ok")
            {              
                Debug.WriteLine(lastStatus.ToString());
            }
        }

        public static void Main()
        {
            Configuration.SetPinFunction(Gpio.IO16, DeviceFunction.COM2_TX);
            Configuration.SetPinFunction(Gpio.IO17, DeviceFunction.COM2_RX);
          
            SerialStream serialStream = new SerialStream("COM2");
   
            ledGreen = gpioController.OpenPin(Gpio.IO18);
            ledGreen.SetDriveMode(GpioPinDriveMode.Output);
            ncpInterface.OnLastStatusHandler += OnLastStatus;
            ncpInterface.Open(serialStream);
            
            try
            {
                ncpInterface.Form(networkname, channel, masterkey, panid);
            }
            catch (Exception ex)
            {                               
                Debug.WriteLine(ex.Message);
                return;
            }

            UdpSocket receiver = new UdpSocket();
            receiver.Bind(IPv6Address.IPv6Any, port);
            IPv6EndPoint remoteIp = null;
         
            while (true)
            {
                if (receiver.Poll(-1, SelectMode.SelectRead))
                {
                    byte[] data = receiver.Receive(ref remoteIp);
                    string message = Encoding.UTF8.GetString(data, 0, data.Length) ;
                    Debug.WriteLine("\n");
                    Debug.WriteLine(message.Length + " bytes from " + remoteIp.Address + " " + remoteIp.Port + " " + message);
                    Debug.WriteLine(">");      
                    
                    if (message.ToLower() == "ledon")
                    {
                        ledGreen.Write(GpioPinValue.High);
                    }
                    else if (message.ToLower() == "ledoff")
                    {
                        ledGreen.Write(GpioPinValue.Low);
                    }
                }
            }
        }
    }
}

