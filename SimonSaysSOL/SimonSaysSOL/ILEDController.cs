using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimonSaysSOL
{
    interface ILEDController
    {
        void Clear();
        void SetColor(int index, Color color);

        void WinRound();
        void WinGame();
        void LoseGame();
    }
}
