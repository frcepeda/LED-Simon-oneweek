using Microsoft.Maker.Firmata;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml;

namespace SimonSaysSOL
{
    public class LEDStrip
    {
        private const int NEOPIXEL_SET_COMMAND = 0x42;
        private const int NEOPIXEL_SHOW_COMMAND = 0x44;
        private const int NUMBER_OF_PIXELS = 30;
        private static volatile UwpFirmata firmata;
        private static DispatcherTimer timeout;
        private static IStream connection;

        public delegate void readyDelegate();
        public static event readyDelegate ready;

        public static async void Init()
        {
            var deviceList = await UsbSerial.listAvailableDevicesAsync();
            var device = deviceList.First();
            connection = new UsbSerial(device);
            firmata = new UwpFirmata();
            firmata.begin(connection);
            App.Arduino = new RemoteDevice(firmata);

            connection.ConnectionEstablished += OnConnectionEstablished;
            connection.ConnectionFailed += OnConnectionFailed;
            connection.begin(115200, SerialConfig.SERIAL_8N1);

            //start a timer for connection timeout
            timeout = new DispatcherTimer();
            timeout.Interval = new TimeSpan(0, 0, 30);
            timeout.Tick += Connection_TimeOut;
            timeout.Start();
        }

        /// <summary>
        /// Sets all the pixels to the given color values and calls UpdateStrip() to tell the NeoPixel library to show the set colors.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        private static void SetAllPixelsAndUpdate(byte red, byte green, byte blue)
        {
            SetAllPixels(red, green, blue);
            UpdateStrip();
        }

        /// <summary>
        /// Sets all the pixels to the given color values
        /// </summary>
        /// <param name="red">The amount of red to set</param>
        /// <param name="green">The amount of green to set</param>
        /// <param name="blue">The amount of blue to set</param>
        private static void SetAllPixels(byte red, byte green, byte blue)
        {
            for (byte i = 0; i < NUMBER_OF_PIXELS; ++i)
            {
                SetPixel(i, red, green, blue);
            }
        }

        public static void SetPixel(int pixel, RGB color)
        {
            SetPixel((byte)pixel, color.red, color.green, color.blue);
        }

        public static void CrissCross()
        {
            for(byte i=0; i<15; i++)
            {
                for (byte j = 0; j < 15; i++)
                {
                    if (j < i)
                    {
                        SetPixel(j, 255, 255, 255);
                        SetPixel((byte)(30 - j), 255, 255, 255);
                    }
                    else if(j < (15 - i))
                    {
                        SetPixel(j, 255, 0, 0);
                        SetPixel((byte)(30 - j), 0, 0, 255);
                    }
                    else
                    {
                        SetPixel(j, 255, 0, 255);
                        SetPixel((byte)(30 - j), 255, 0, 255);
                    }
                }
                UpdateStrip();
            }
        }

        /// <summary>
        /// Sets a single pixel to the given color values
        /// </summary>
        /// <param name="red">The amount of red to set</param>
        /// <param name="green">The amount of green to set</param>
        /// <param name="blue">The amount of blue to set</param>
        private static void SetPixel(byte pixel, byte red, byte green, byte blue)
        {
            firmata.beginSysex(NEOPIXEL_SET_COMMAND);
            firmata.appendSysex(pixel);
            firmata.appendSysex(red);
            firmata.appendSysex(green);
            firmata.appendSysex(blue);
            firmata.endSysex();
        }

        /// <summary>
        /// Tells the NeoPixel strip to update its displayed colors.
        /// This function must be called before any colors set to pixels will be displayed.
        /// </summary>
        /// <param name="red">The amount of red to set</param>
        /// <param name="green">The amount of green to set</param>
        /// <param name="blue">The amount of blue to set</param>
        public static void UpdateStrip()
        {
            firmata.beginSysex(NEOPIXEL_SHOW_COMMAND);
            firmata.endSysex();
        }

        public static void FREAKOUT(int freakCount)
        {
            Random r = new Random();
            for (int j = 0; j < freakCount; j++)
            {
                for (byte i = 0; i < NUMBER_OF_PIXELS; i++)
                {
                    SetPixel(i, (byte)r.Next(256), (byte)r.Next(256), (byte)r.Next(256));
                }
                UpdateStrip();
            }
        }

        public static void FREAKOUTLOSE(int freakCount)
        {
            Random r = new Random();
            for (int j = 0; j < freakCount; j++)
            {
                for (byte i = 0; i < NUMBER_OF_PIXELS; i++)
                {
                    SetPixel(i, (byte)r.Next(256), (byte)r.Next(0), (byte)r.Next(0));
                }
                UpdateStrip();
            }
        }

        public static void Clear()
        {
            SetAllPixelsAndUpdate(0,0,0);
        }

        /****************************************************************
         *                  Event callbacks                             *
         ****************************************************************/

        private static void OnConnectionFailed(string message)
        {
            timeout.Stop();
            Debug.WriteLine("ERROR: unable to connect!!");
        }

        private static void OnConnectionEstablished()
        {
            timeout.Stop();
            ready();
            Debug.WriteLine("Success: Connection Established");
        }

        private static void Connection_TimeOut(object sender, object e)
        {
            Debug.WriteLine("ERROR: Connection TImed Out");
        }
    }
}
