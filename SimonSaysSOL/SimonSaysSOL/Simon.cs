using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimonSaysSOL
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

            Color newMove = randomMove();
            moves.Add(newMove);
            CurrentSlot = 0;

            DisplayRound();

            State = GameState.Playing;

            resetTimer();
        }

        Timer turnTimeout;

        private void resetTimer() {
            TimerCallback timerElapsed = (cancelledRound) =>
            {
                if ((int)cancelledRound == moves.Count && State == GameState.Playing)
                {
                    Play(Color.Black);
                }
            };

            turnTimeout = new Timer(timerElapsed, moves.Count, Constants.MSEC_TURN_TIMEOUT, Timeout.Infinite);
        }

        void DisplayRound()
        {
            LEDController.Clear();

            for (int i = 0; i < moves.Count; i++)
            {
                AudioPlayer.playAudio(Constants.ColorAudio[moves[i]]);
                LEDController.SetColor(i, moves[i]);
                Task.Delay(Constants.MSEC_STEP_FLASH_ON).Wait();
                LEDController.SetColor(i, Color.Black);
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

            resetTimer();

            try
            {
                AudioPlayer.playAudio(Constants.ColorAudio[move]);
            }
            catch (Exception) { };

            LEDController.SetColor(CurrentSlot, moves[CurrentSlot]);

            Task.Delay(10).Wait();

            if (moves[CurrentSlot] != move)
            {
                State = GameState.Ended;
                AudioPlayer.playAudio(Constants.LoseAudio);
                LEDController.LoseGame();
                return;
            }
            else if (CurrentSlot == Constants.PIXELS - 1) // The game was won.
            {
                State = GameState.Ended;
                AudioPlayer.playAudio(Constants.WinAudio);
                LEDController.WinGame();
                return;
            }
            else if (CurrentSlot == moves.Count - 1) // The round was won.
            {
                State = GameState.NotYetStarted;
                AudioPlayer.playAudio(Constants.RoundWonAudio);
                LEDController.WinRound();
                StartRound();
                return;
            }

            CurrentSlot++;
        }
    }
}
