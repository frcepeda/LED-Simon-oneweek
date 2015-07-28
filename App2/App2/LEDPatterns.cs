using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Expected methods
// SetPixel(int index, RGB color);
// ClearStrip();
namespace App2
{
    class LEDPatterns : ILEDController
    {
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void LoseGame()
        {
            throw new NotImplementedException();
        }

        public void SetColor(int index, Color color)
        {
            throw new NotImplementedException();
        }

        public void WinGame()
        {
            throw new NotImplementedException();
        }

        public void WinRound()
        {
            Chase(10, 10);
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
            //ClearStrip();
            for (int i = 0; i < Constants.PIXELS; i++)
            {
                //SetPixel(i, LEDStripColors[i]);
            }
        }
        private void Chase(int repeats, uint wait)
        {
            InitRandomColors();

            for (int i = 0; i < repeats; i++)
            {
                CircleColorsByOnePixel();
                //Delay(wait);
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
                //Delay(wait);
                //ClearStrip();
                //Delay(wait);
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
                        //SetPixel(i+q, Wheel( (i+j) % 255));    //turn every third pixel on
                    }
                    LEDStripShow();

                    //Delay(wait);

                    for (int i = 0; i < Constants.PIXELS - 3; i = i + 3)
                    {
                        //SetPixel(i+q, RGB(0,0,0));        //turn every third pixel off
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
            /*
          WheelPos = 255 - WheelPos;
          if(WheelPos < 85) {
            return new RGB(255 - WheelPos * 3, 0, WheelPos * 3);
          }
          if(WheelPos < 170) {
            WheelPos -= 85;
            return new RGB(0, WheelPos * 3, 255 - WheelPos * 3);
          }
          WheelPos -= 170;
          return new RGB(WheelPos * 3, 255 - WheelPos * 3, 0);
        }
        */
            return new RGB(0,0,0);
        }

    }
}


