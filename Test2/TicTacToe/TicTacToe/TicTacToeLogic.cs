using System.Windows.Controls;

namespace TicTacToe
{
    /// <summary>
    /// Implements tic tac toe logic.
    /// </summary>
    public partial class TicTacToeLogic
    {
        /// <summary>
        /// Current player.
        /// </summary>
        private char player;

        /// <summary>
        /// Initializes an instance of TicTacToeLogic.
        /// </summary>
        public TicTacToeLogic(Button button00, Button button01, Button button02, Button button10, Button button11, Button button12,
                Button button20, Button button21, Button button22)
        {
            StartGame(button00, button01, button02, button10, button11, button12, button20, button21, button22);
        }

        /// <summary>
        /// Starts the game again.
        /// </summary>
        public void PlayAgain(Button button00, Button button01, Button button02, Button button10, Button button11, Button button12,
                Button button20, Button button21, Button button22)
        {
            StartGame(button00, button01, button02, button10, button11, button12, button20, button21, button22);
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        private void StartGame(Button button00, Button button01, Button button02, Button button10, Button button11, Button button12,
                Button button20, Button button21, Button button22)
        {
            player = 'x';
            button00.Content = "";
            button01.Content = "";
            button02.Content = "";
            button10.Content = "";
            button11.Content = "";
            button12.Content = "";
            button20.Content = "";
            button21.Content = "";
            button22.Content = "";
        }

        /// <summary>
        /// Current player makes a move.
        /// </summary>
        public void MakeAMove(Button buttonWantToMove, Button button00, Button button01, Button button02, Button button10, Button button11, Button button12,
                Button button20, Button button21, Button button22)
        {
            if (buttonWantToMove.Content.ToString() == "" && !SomeoneWon(button00, button01, button02, button10, button11, button12, button20, button21, button22) &&
                    !NoWayToMove(button00, button01, button02, button10, button11, button12, button20, button21, button22))
            {
                switch (player)
                {
                    case 'o':
                        buttonWantToMove.Content = "o";
                        break;
                    case 'x':
                        buttonWantToMove.Content = "x";
                        break;
                }

                buttonWantToMove.FontSize = 80;

                if (!SomeoneWon(button00, button01, button02, button10, button11, button12, button20, button21, button22) &&
                    !NoWayToMove(button00, button01, button02, button10, button11, button12, button20, button21, button22))
                {
                    ChangePlayer();
                }
            }
        }

        /// <returns>current player</returns>
        public char GetPlayer()
        {
            return player;
        }

        /// <summary>
        /// Changes player.
        /// </summary>
        public void ChangePlayer()
        {
            if (player == 'o')
            {
                player = 'x';
            }
            else
            {
                player = 'o';
            }
        }

        /// <returns>true if someone won. Otherwise returns false.</returns>
        public bool SomeoneWon(Button button00, Button button01, Button button02, Button button10, Button button11, Button button12,
                Button button20, Button button21, Button button22)
        {
            if (button00.Content.ToString() != "" && button00.Content == button01.Content && button01.Content == button02.Content)
            {
                return true;
            }
            else if (button10.Content.ToString() != "" && button10.Content == button11.Content && button11.Content == button12.Content)
            {
                return true;
            }
            else if (button20.Content.ToString() != "" && button20.Content == button21.Content && button21.Content == button22.Content)
            {
                return true;
            }
            else if (button00.Content.ToString() != "" && button00.Content == button10.Content && button10.Content == button20.Content)
            {
                return true;
            }
            else if (button01.Content.ToString() != "" && button01.Content == button11.Content && button11.Content == button21.Content)
            {
                return true;
            }
            else if (button02.Content.ToString() != "" && button02.Content == button12.Content && button12.Content == button22.Content)
            {
                return true;
            }
            else if (button00.Content.ToString() != "" && button00.Content == button11.Content && button11.Content == button22.Content)
            {
                return true;
            }
            else if (button20.Content.ToString() != "" && button20.Content == button11.Content && button11.Content == button02.Content)
            {
                return true;
            }

            return false;
        }

        /// <returns>true if there is no way to move.</returns>
        public bool NoWayToMove(Button button00, Button button01, Button button02, Button button10, Button button11, Button button12,
                Button button20, Button button21, Button button22)
        {
            if (button00.Content.ToString() != "" && button01.Content.ToString() != "" && button02.Content.ToString() != "" && button10.Content.ToString() != "" &&
                    button11.Content.ToString() != "" && button12.Content.ToString() != "" && button20.Content.ToString() != "" &&
                    button21.Content.ToString() != "" && button22.Content.ToString() != "")
            {
                return true;
            }

            return false;
        }
    }
}