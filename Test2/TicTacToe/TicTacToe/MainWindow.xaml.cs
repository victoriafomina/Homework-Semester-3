using System.Windows;
using System.Windows.Controls;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TicTacToeLogic ticTacToe;
        private bool gameEnded;

        /// <summary>
        /// Initializes an instance of the MainWindow.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            ticTacToe = new TicTacToeLogic(button00, button01, button02, button10, button11, button12, button20, button21, button22);
            gameEnded = false;
        }

        /// <summary>
        /// Handles field's button click.
        /// </summary>
        private void ButtonField_Click(object sender, RoutedEventArgs e)
        {
            if (!gameEnded)
            {
                ticTacToe.MakeAMove(sender as Button, button00, button01, button02, button10, button11, button12, button20, button21, button22);
                if (ticTacToe.SomeoneWon(button00, button01, button02, button10, button11, button12, button20, button21, button22) ||
                        ticTacToe.NoWayToMove(button00, button01, button02, button10, button11, button12, button20, button21, button22))
                {
                    gameEnded = true;
                }

                if (gameEnded)
                {
                    if (ticTacToe.SomeoneWon(button00, button01, button02, button10, button11, button12, button20, button21, button22))
                    {
                        MessageBox.Show($"Player {ticTacToe.GetPlayer().ToString().ToUpper()} won!");
                    }
                    else if (ticTacToe.NoWayToMove(button00, button01, button02, button10, button11, button12, button20, button21, button22))
                    {
                        MessageBox.Show("Draw!");
                    }

                    playAgain.Visibility = Visibility.Visible;
                }
            }            
        }

        /// <summary>
        /// Handles play again button click.
        /// </summary>
        private void ButtonPlayAgain_Click(object sender, RoutedEventArgs e)
        {
            ticTacToe.PlayAgain(button00, button01, button02, button10, button11, button12, button20, button21, button22);
            playAgain.Visibility = Visibility.Collapsed;
            gameEnded = false;
        }
    }
}
