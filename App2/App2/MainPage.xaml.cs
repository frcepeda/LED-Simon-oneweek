using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private class DummyLEDController : ILEDController
        {
            private List<Color> colors = new List<Color>(Constants.PIXELS);

            public void Clear()
            {
                colors.Clear();
                Debug.WriteLine("Clear");
            }

            public void LoseGame()
            {
                Debug.WriteLine("Game lost!");
            }

            public void SetColor(int index, Color color)
            {
                while (colors.Count <= index) colors.Add(Color.Blue);
                colors[index] = color;
                foreach (var c in colors)
                    Debug.Write(c + " ");
                Debug.WriteLine("");
            }

            public void WinGame()
            {
                Debug.WriteLine("Game won!");
            }

            public void WinRound()
            {
                Debug.WriteLine("Round won!");
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            AudioPlayer.SetUp(media, Dispatcher);

            Simon game = new Simon(new DummyLEDController()); // FIXME

            foreach (var c in Constants.Colors)
            {
                new Button(c).ButtonPressed += (s) =>
                {
                    if (game.State != GameState.Playing) return;

                    game.Play(s.Color);
                };
            }
        }
    }
}
