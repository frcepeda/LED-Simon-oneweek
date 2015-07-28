using Microsoft.Maker.Firmata;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Maker.Serial;
using Microsoft.Maker.RemoteWiring;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        private class DummyLEDController : ILEDController
        {
            private List<Color> colors = new List<Color>(Constants.PIXELS);
            private ILEDController led = new LEDPatterns();

            public void Clear()
            {
                colors.Clear();
                led.Clear();
                Debug.WriteLine("Clear");
            }

            public void LoseGame()
            {
                Debug.WriteLine("Game lost!");
                //led.LoseGame();
                Task.Delay(1000).Wait();
            }

            public void SetColor(int index, Color color)
            {
                while (colors.Count <= index) colors.Add(Color.Black);
                colors[index] = color;
                foreach (var c in colors)
                    Debug.Write(c + " ");
                Debug.WriteLine("");
                led.SetColor(index, color);
            }

            public void WinGame()
            {
                Debug.WriteLine("Game won!");
                //led.WinGame();
                Task.Delay(1000).Wait();
            }

            public void WinRound()
            {
                Debug.WriteLine("Round won!");
                //led.WinRound();
                Task.Delay(1000).Wait();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            AudioPlayer.SetUp(Dispatcher);

            LEDStrip.Init();

            LEDStrip.ready += () =>
            {
                LEDStrip.Clear();

                Simon game = new Simon(new DummyLEDController());

                foreach (var c in Constants.Colors)
                {
                    new Button(c).ButtonPressed += (s) =>

                    {
                        if (game.State != GameState.Playing) return;

                        game.Play(s.Color);
                    };
                }

                game.StartRound();
            };
        }

        private async void FlipOut_Click(object sender, RoutedEventArgs e)
        {
            //LEDStrip.FREAKOUT(10);
             //LEDStrip.SetAllPixelsAndUpdate((byte)r.Next(256), (byte)r.Next(256), (byte)r.Next(256));
            //await Task.Delay(100);
        }


        private void Off_Click(object sender, RoutedEventArgs e)
        {
            LEDStrip.Clear();
        }
    }
}
