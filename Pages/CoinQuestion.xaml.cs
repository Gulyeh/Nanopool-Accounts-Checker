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
using System.Windows.Shapes;

namespace MiningCheck
{
    /// <summary>
    /// Logika interakcji dla klasy CoinQuestion.xaml
    /// </summary>
    public partial class CoinQuestion : Window
    {
        public CoinQuestion()
        {
            InitializeComponent();
        }

        private void ExitWindow(object sender, RoutedEventArgs e)
        {
            CheckAnswer();
            this.Close();
        }

        private void CheckAnswer()
        {
            if(SelectCoin.SelectedIndex == 0)
            {
                Variables.crypto = 0;
            }
            else
            {
                Variables.crypto = 3;
            }
        }
    }
}
