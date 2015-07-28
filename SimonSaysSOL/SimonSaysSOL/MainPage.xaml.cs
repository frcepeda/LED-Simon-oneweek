using Microsoft.Maker.Firmata;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Maker.Serial;
using Microsoft.Maker.RemoteWiring;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SimonSaysSOL
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        /// <summary>
        /// This page uses advanced features of the Windows Remote Arduino library to carry out custom commands which are
        /// defined in the NeoPixel_StandardFirmata.ino sketch. This is a customization of the StandardFirmata sketch which
        /// implements the Firmata protocol. The customization defines the behaviors of the custom commands invoked by this page.
        /// 
        /// To learn more about Windows Remote Arduino, refer to the GitHub page at: https://github.com/ms-iot/remote-wiring/
        /// To learn more about advanced behaviors of WRA and how to define your own custom commands, refer to the
        /// advanced documentation here: https://github.com/ms-iot/remote-wiring/blob/develop/advanced.md
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            LEDStrip.Init();
        }

        /// <summary>
        /// This button callback is invoked when the buttons are pressed on the UI. It determines which
        /// button is pressed and sets the LEDs appropriately
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FlipOut_Click(object sender, RoutedEventArgs e)
        {
            LEDStrip.FREAKOUT(200);
             //LEDStrip.SetAllPixelsAndUpdate((byte)r.Next(256), (byte)r.Next(256), (byte)r.Next(256));
            //await Task.Delay(100);
        }

        private async void CrissCross_Click(object sender, RoutedEventArgs e)
        {
            //LEDStrip.CrissCross();
        }

        private void Off_Click(object sender, RoutedEventArgs e)
        {
            LEDStrip.Clear();
        }
    }
}
