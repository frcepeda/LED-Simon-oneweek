using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace SimonSaysSOL
{
    class Button
    {
        public Color Color { get; private set; }

        public delegate void ButtonPressedHandler(Button sender);

        public event ButtonPressedHandler ButtonPressed;

        GpioPin pin;

        private GpioPinValue lastValue = GpioPinValue.Low;

        public Button(Color c)
        {
            Color = c;

            pin = GpioController.GetDefault().OpenPin(Constants.PinNumber[c]);
            pin.SetDriveMode(GpioPinDriveMode.Input);

            Task.Run(WatchPin); // see how to dispatch this
        }

        private async Task WatchPin()
        {
            while (true)
            {
                await Task.Delay(Constants.MSEC_BUTTON_READ_TIME);

                GpioPinValue currValue = pin.Read();

                if (currValue == GpioPinValue.High && lastValue == GpioPinValue.Low)
                {
                    if (ButtonPressed != null)
                    {
                        ButtonPressed(this);
                    }
                }

                lastValue = currValue;
            }
        }
    }
}
