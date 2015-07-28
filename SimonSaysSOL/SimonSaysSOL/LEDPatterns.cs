﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Expected methods
// SetPixel(int index, RGB color);
// ClearStrip();
namespace SimonSaysSOL
{
    class LEDPatterns : ILEDController
    {
        object lockObj = new object();

        public void Update()
        {
            lock (lockObj)
            {
                LEDStrip.UpdateStrip();
            }
        }

        public void Clear()
        {
            lock (lockObj)
            {
                LEDStrip.Clear();
            }
        }

        public void LoseGame()
        {
            LEDStrip.FREAKOUTLOSE(100);
        }

        public void SetColor(int index, Color color)
        {
            RGB rgb;

            try
            {
                rgb = Constants.ColorToRGB[color];
            }
            catch (Exception)
            {
                rgb = new RGB(0, 0, 0);
            }

            SetColor(index, rgb);
        }

        public void SetColor(int index, RGB rgb)
        {
            lock (lockObj)
            {
                LEDStrip.SetPixel(index, rgb);
                LEDStrip.UpdateStrip();
            }
        }

        public void WinGame()
        {
            LEDStrip.FREAKOUT(100);
        }

        public void WinRound()
        {
            LEDStrip.FREAKOUT(10);
        }

        private byte RandomColorValue()
        {
            Random r = new Random();
            return (byte)(r.Next() % 256);
        }

        private RGB RandomRGB()
        {
            return new RGB(RandomColorValue(), RandomColorValue(), RandomColorValue());
        }

        private List<RGB> LEDStripColors = new List<RGB>(Constants.PIXELS);

        private void LEDStripShow()
        {
            Clear();
            for (int i = 0; i < Constants.PIXELS; i++)
            {
                SetColor(i, LEDStripColors[i]);
            }
            Update();
        }
        private void Chase(int repeats, uint wait)
        {
            InitRandomColors();

            for (int i = 0; i < repeats; i++)
            {
                CircleColorsByOnePixel();
                Task.Delay(100).Wait();
            }

        }
        public void InitRandomColors()
        {
            for (int i = 0; i < Constants.PIXELS; i++)
            {
                LEDStripColors[i] = RandomRGB();
            }
        }

        private void CircleColorsByOnePixel()
        {
            RGB last = LEDStripColors.Last();
            for (int i = Constants.PIXELS - 1; i > 0; i--)
            {
                LEDStripColors[i] = LEDStripColors[i - 1];
            }
            LEDStripColors[0] = last;
            LEDStripShow();
        }


        private void Delay(int millisec)
        {
            Task.Delay(millisec).Wait();
        }


        public void ContinuousBlinking(uint wait, int repeats)
        {
            for (int i = 0; i < repeats; i++)
            {
                LEDStripShow();
                Task.Delay(100).Wait();
                Clear();
                Task.Delay(100).Wait();
            }

        }


        public void theaterChaseRainbow(uint wait)
        {
            for (int j = 0; j < 256; j++)
            {     // cycle all 256 colors in the wheel
                for (int q = 0; q < 3; q++)
                {
                    for (int i = 0; i < Constants.PIXELS; i = i + 3)
                    {
                        SetColor(i + q, Wheel((byte)((i + j) % 255)));    //turn every third pixel on
                    }
                    LEDStripShow();

                    Task.Delay(100).Wait();

                    for (int i = 0; i < Constants.PIXELS - 3; i = i + 3)
                    {
                        SetColor(i + q, new RGB(0, 0, 0));
                    }
                }
            }
        }

        public void RainbowCycle(uint wait)
        {
            uint i, j;

            for (j = 0; j < 256 * 5; j++)
            { // 5 cycles of all colors on wheel
                for (i = 0; i < Constants.PIXELS; i++)
                {
                    //SetPixel(i, Wheel(((i * 256 / Constants.PIXELS) + j) & 255));
                }
                LEDStripShow();
                //Delay(wait);
            }
        }

        // Input a value 0 to 255 to get a color value.
        // The colours are a transition r - g - b - back to r.
        // modified  from Arduino Strandtest code
        private RGB Wheel(byte WheelPos)
        {
            WheelPos = (byte)(255 - WheelPos);
            if (WheelPos < 85)
            {
                return new RGB(255 - WheelPos * 3, 0, WheelPos * 3);
            }
            if (WheelPos < 170)
            {
                WheelPos -= 85;
                return new RGB(0, WheelPos * 3, 255 - WheelPos * 3);
            }
            WheelPos -= 170;
            return new RGB(WheelPos * 3, 255 - WheelPos * 3, 0);
        }
    }

}


