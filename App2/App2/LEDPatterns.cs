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
            Chase();
        }

        private byte RandomColorValue()
        {
            Random r = new Random();
            return (byte) (r.Next() % 256);
        }

        private Constants.RGB RandomRGB()
        {
            return new Constants.RGB(RandomColorValue(), RandomColorValue(), RandomColorValue());
        }

        private List<Constants.RGB> LEDStripColors = new List<Constants.RGB>(Constants.PIXELS);

        private void DisplayColors()
        {
            //ClearStrip();
            for (int i = 0; i < Constants.PIXELS; i++)
            {
                //SetPixel(i, LEDStripColors[i]);
            }
        }
        private void Chase()
        {
            /*for (int i = 0; i < 5; i++)
            {
                SetPixel(i, Constants.ColorToRGB.Keys[i]);
                LEDStripColors[i] = Constants.ColorToRGB.Keys[i];
            }*/

            InitRandomColors();

            while (true)
            {
                CircleColorsByOnePixel();
                Task.Delay(Constants.MSEC_CHASE_SPEED).Wait();
            }

        }
        private void InitRandomColors()
        {
            for (int i = 0; i < Constants.PIXELS; i++)
            {
                LEDStripColors[i] = RandomRGB();
            }
        }

        private void CircleColorsByOnePixel()
        {
            Constants.RGB last = LEDStripColors.Last();
            for (int i = Constants.PIXELS-1; i > 0; i--)
            {
                LEDStripColors[i] = LEDStripColors[i - 1];
            }
            LEDStripColors[0] = last;
            DisplayColors();
        }

    }

}
