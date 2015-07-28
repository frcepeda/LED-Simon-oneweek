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
        static CoreDispatcher UiDispatcher;

        static int curr = 0;
        static IList<MediaElement> mediaElements = new List<MediaElement>();

        public static void SetUp(MediaElement media, CoreDispatcher UiDispatcher)
        {
            for (int i = 0; i < Constants.AUDIO_CHANNELS; i++)
                mediaElements.Add(new MediaElement());
            AudioPlayer.UiDispatcher = UiDispatcher;
        }

        public static void playAudio(Audio audio)
        {
            Task shutUpCompiler = UiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 var media = mediaElements[curr++];
                 curr = (curr + 1) % Constants.AUDIO_CHANNELS;
                 media.SetSource(audio.stream, audio.mimeType);
                 media.AutoPlay = true;
                 media.Play();
             }
            ).AsTask();
        }
    }
}
