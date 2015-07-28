using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace App2
{
    class AudioPlayer
    {
        static MediaElement media;
        static CoreDispatcher UiDispatcher;
        static AutoResetEvent mediaEnd = new AutoResetEvent(false);

        public static void SetUp(MediaElement media, CoreDispatcher UiDispatcher)
        {
            AudioPlayer.media = media;
            AudioPlayer.UiDispatcher = UiDispatcher;

            UiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                media.MediaEnded += (s, e) => mediaEnd.Set();
            }).AsTask().Wait();
        }

        public static void playAudio(string path, string mimeType)
        {
            UiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 IRandomAccessStream stream = new FileStream(@"lol.mp3", FileMode.Open, FileAccess.Read).AsRandomAccessStream();
                 media.SetSource(stream, mimeType);
                 media.AutoPlay = true;
                 media.Play();
             }
            ).AsTask().Wait();

            mediaEnd.WaitOne();
        }
    }
}
