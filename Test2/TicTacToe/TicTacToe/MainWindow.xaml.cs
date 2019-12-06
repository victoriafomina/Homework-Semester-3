using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        /// <summary>
        /// Handles click of field's button.
        /// </summary>
        private void ButtonField_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Content.ToString() == "")
            {
                switch (player)
                {
                    case 'o':
                        (sender as Button).Content = "o";
                        break;
                    case 'x':
                        (sender as Button).Content = "x";
                        break;
                }

                (sender as Button).FontSize = 80;

                if (SomeoneWon())
                {
                    MessageBox.Show($"Player {player.ToString().ToUpper()} won!");
                }
                else if (NoWayToMove())
                {
                    MessageBox.Show("Draw!");
                }

                ChangePlayer();
            }
        }

        /// <summary>
        /// Handles click of the button play again.
        /// </summary>
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

        /// <summary>
        /// Changes current player.
        /// </summary>
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

        /// <returns>true when game has finished with the winning of some player, otherwise renerns false</returns>
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

        /// <returns>true when there is no field that the player can draw his character</returns>
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
