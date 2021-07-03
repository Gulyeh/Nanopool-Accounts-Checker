using System.Windows;


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
                Variables.CryptoInfo.cryptoSelected = 0;
            }
            else
            {
                Variables.CryptoInfo.cryptoSelected = 3;
            }
        }
    }
}
