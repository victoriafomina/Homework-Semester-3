using System.Windows;
using System.Windows.Controls;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private char player;

        public App(Button button00, Button button01, Button button02, Button button10, Button button11, Button button12,
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

        public void MakeAMove(Button buttonWantToMove, Button button00, Button button01, Button button02, Button button10, Button button11, Button button12,
                Button button20, Button button21, Button button22)
        {
            if (buttonWantToMove.Content.ToString() == "")
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

                if (SomeoneWon(button00, button01, button02, button10, button11, button12, button20, button21, button22))
                {
                    MessageBox.Show($"Player {GetPlayer().ToString().ToUpper()} won!");
                }
                else if (NoWayToMove(button00, button01, button02, button10, button11, button12, button20, button21, button22))
                {
                    MessageBox.Show("Draw!");
                }

                if (!SomeoneWon(button00, button01, button02, button10, button11, button12, button20, button21, button22) &&
                        !NoWayToMove(button00, button01, button02, button10, button11, button12, button20, button21, button22))
                {
                    ChangePlayer();
                }
            }
        }

        public char GetPlayer()
        {
            return player;
        }

        public void ChangePlayer()
        {
            if (player == 'o')
            {
                player = 'x';
            }
            else if (player == 'x')
            {
                player = 'o';
            }
        }

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
