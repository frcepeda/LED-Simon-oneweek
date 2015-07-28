using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    public enum GameState
    {
        NotYetStarted,
        Playing,
        Ended,
    };

    class Simon
    {
        public GameState State { get; private set; }
        public IReadOnlyList<Color> Slots { get { return moves; } }
        public int CurrentSlot { get; private set; }
        List<Color> moves = new List<Color>(Constants.PIXELS);
        ILEDController LEDController;

        public Simon(ILEDController controller)
        {
            LEDController = controller;
            State = GameState.NotYetStarted;
            StartRound();
        }

        private Color randomMove()
        {
            return (Color)(new Random().Next() % 4); // sue me.
        }

        /// <summary>
        /// Starts a new round.
        /// </summary>
        /// <returns>The new color for this round.</returns>
        public void StartRound()
        {
            if (State != GameState.NotYetStarted || moves.Count == Constants.PIXELS)
            {
                throw new InvalidOperationException("wat");
            }

            State = GameState.Playing;

            Color newMove = randomMove();
            moves.Add(newMove);
            CurrentSlot = 0;

            DisplayRound();
        }

        void DisplayRound()
        {
            LEDController.Clear();

            for (int i = 0; i < moves.Count; i++)
            {
                LEDController.SetColor(i, moves[i]);
                Task.Delay(Constants.MSEC_STEP_FLASH_ON).Wait();
                LEDController.Clear();
                Task.Delay(Constants.MSEC_STEP_FLASH_OFF).Wait();
            }
        }

        /// <summary>
        /// Plays a color.
        /// </summary>
        /// <returns>Whether the answer was correct.</returns>
        public void Play(Color move)
        {
            if (State != GameState.Playing || CurrentSlot >= moves.Count)
            {
                throw new InvalidOperationException("wat");
            }

            if (moves[CurrentSlot] != move)
            {
                State = GameState.Ended;
                LEDController.LoseGame();
                return;
            }
            else if (CurrentSlot == Constants.PIXELS - 1) // The game was won.
            {
                State = GameState.Ended;
                LEDController.WinGame();
                return;
            }
            else if (CurrentSlot == moves.Count - 1) // The round was won.
            {
                State = GameState.NotYetStarted;
                LEDController.WinRound();
                StartRound();
                return;
            }
            else // Light up the new correct answer.
            {
                LEDController.SetColor(CurrentSlot, moves[CurrentSlot]);
            }

            CurrentSlot++;
        }
    }
}
