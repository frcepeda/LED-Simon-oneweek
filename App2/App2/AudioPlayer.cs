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

        public static void SetUp(MediaElement media, CoreDispatcher UiDispatcher)
        {
            AudioPlayer.media = media;
            AudioPlayer.UiDispatcher = UiDispatcher;
        }

        public static void playAudio(Audio audio)
        {
            Task shutUpCompiler = UiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 media.SetSource(audio.stream, audio.mimeType);
                 media.AutoPlay = true;
                 media.Play();
             }
            ).AsTask();
        }
    }
}
