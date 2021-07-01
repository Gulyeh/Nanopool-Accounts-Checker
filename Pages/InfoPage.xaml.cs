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
            foreach(string coin in Variables.cryptovalues)
            {
                coinlist += coin + "\n";
            }
            CoinsAvailable.Dispatcher.Invoke(() => CoinsAvailable.Text = coinlist);
        }
    }
}
