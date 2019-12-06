using System.Windows;
using System.Windows.Controls;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private char player = 'x';

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonField_Click(object sender, RoutedEventArgs e)
        {
            MakeAMove(sender as Button);
        }

        private void ButtonPlayAgain_Click(object sender, RoutedEventArgs e)
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

        public void MakeAMove(Button buttonWantToMove)
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

                if (SomeoneWon())
                {
                    MessageBox.Show($"Player {GetPlayer().ToString().ToUpper()} won!");
                }
                else if (NoWayToMove())
                {
                    MessageBox.Show("Draw!");
                }

                if (!SomeoneWon() && !NoWayToMove())
                {
                    ChangePlayer();
                }
            }
        }

        private char GetPlayer() => player;

        private void ChangePlayer()
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

        private bool SomeoneWon()
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

        private bool NoWayToMove()
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
