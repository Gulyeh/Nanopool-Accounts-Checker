using System;
using System.Windows.Controls;

namespace MiningCheck
{
    /// <summary>
    /// Logika interakcji dla klasy Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
            CoinAvailable();
        }

        private void CoinAvailable()
        {
            string coinlist = String.Empty;
            foreach(string coin in Variables.CryptoInfo.cryptoValues)
            {
                coinlist += coin + "\n";
            }
            CoinsAvailable.Dispatcher.Invoke(() => CoinsAvailable.Text = coinlist);
        }
    }
}
