using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace App2
{
    public enum Color
    {
        Red,
        Blue,
        Green,
        Yellow,
        DarkTurquoise,
        BlueViolet,
        Fuschia,
        Cyan,
        Indigo,
        LawnGreen,
        Black,
        White,
    };

    public struct RGB
    {
        byte red, green, blue;
        public RGB(byte red, byte green, byte blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }
    }

    public struct Audio
    {
        public IRandomAccessStream stream;
        public string mimeType;
        public Audio(string p, string m)
        {
            stream = new FileStream(p, FileMode.Open, FileAccess.Read).AsRandomAccessStream();
            mimeType = m;
        }
    }

    class Constants
    {
        public const int PIXELS = 30;

        public const int MSEC_STEP_FLASH_ON = 200;
        public const int MSEC_STEP_FLASH_OFF = 200;
        public const int MSEC_CHASE_SPEED = 30;

        public const int MSEC_BUTTON_READ_TIME = 10;

        public const int MSEC_TURN_TIMEOUT = 5000;

        public static readonly IEnumerable<Color> Colors = new List<Color> { Color.Blue , Color.Red, Color.Green, Color.Yellow };

        public static readonly IReadOnlyDictionary<Color, int> PinNumber = new Dictionary<Color, int>()
        {
            { Color.Yellow, 5 },
            { Color.Red, 4 },
            { Color.Green, 12 },
            { Color.Blue, 6 },
        };

        public static readonly IReadOnlyDictionary<Color, RGB> ColorToRGB = new Dictionary<Color, RGB>()
        {
            { Color.Yellow, new RGB(255, 255, 0)},
            { Color.Red, new RGB(255, 0, 0)},
            { Color.Green, new RGB(0, 255, 0) },
            { Color.Blue, new RGB(0,0,255)},
            { Color.DarkTurquoise, new RGB(0,206,209)},
            { Color.BlueViolet, new RGB(138,43,226)},
            { Color.Fuschia, new RGB(255,0,255)},
            { Color.Cyan, new RGB(0,255,255)},
            { Color.Indigo, new RGB(75,0,130)},
            { Color.LawnGreen, new RGB(124,252,0)},
            { Color.Black, new RGB(0,0,0)},
            { Color.White, new RGB(255,255,255)},
        };

        public static readonly IReadOnlyDictionary<Color, Audio> ColorAudio = new Dictionary<Color, Audio>()
        {
            { Color.Yellow, new Audio("Assets/Sounds/piano-c.wav", "audio/wav")},
            { Color.Red, new Audio("Assets/Sounds/piano-d.wav", "audio/wav")},
            { Color.Green, new Audio("Assets/Sounds/piano-e.wav", "audio/wav")},
            { Color.Blue, new Audio("Assets/Sounds/piano-f.wav", "audio/wav")},
        };
    }
}
