using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json.Linq;

namespace MiningCheck
{
    /// <summary>
    /// Logika interakcji dla klasy Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            FillLists();
            GetInfoFromXML();
        }

        public void FillLists()
        {
            foreach(string value in Variables.fiatvalues)
            {
                CurrencyList.Items.Add(value);
            }
        }
        private void FiatBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrencyList.Dispatcher.Invoke(() => Variables.fiat = CurrencyList.SelectedIndex);
        }
        private void textChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            WalletAddress.Dispatcher.Invoke(() => Variables.walletaddress = WalletAddress.Text);
            Variables.ResetVars();
            Variables.ClearArrayDatas();
            DetectCoin();
        }
        private void OpenQuestionWindow(object sender, RoutedEventArgs e)
        {
            CoinQuestion question = new CoinQuestion();
            question.Owner = Application.Current.MainWindow;
            question.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            question.ShowDialog();
            Variables.ResetVars();
            Variables.ClearArrayDatas();
            if (Variables.crypto == 0)
            {
                CoinName.Dispatcher.Invoke(() => CoinName.Text = "ETH");
            }
            else if (Variables.crypto == 3)
            {
                CoinName.Dispatcher.Invoke(() => CoinName.Text = "ETC");
            }
        }       
        private async Task DetectCoin()
        {
            try
            {
                Variables.CheckingCryptoCoin = true;
                ChangeCoin.Dispatcher.Invoke(() => ChangeCoin.Visibility = Visibility.Hidden);
                if (WalletAddress.Text.ToLower().StartsWith("0x"))
                {
                    await Task.Run(async() =>
                    {
                        string whatcoin = await WebSocket.WebConnector("user", "eth");
                        string whatcoin2 = await WebSocket.WebConnector("user", "etc");
                        JObject type = JObject.Parse(whatcoin);
                        JObject type2 = JObject.Parse(whatcoin2);
                        if(Boolean.Parse(type["status"].ToString()) == true && (Boolean.Parse(type2["status"].ToString()) == true))
                        {
                            Application.Current.Dispatcher.Invoke((Action)delegate {
                                CoinQuestion question = new CoinQuestion();
                                ChangeCoin.Dispatcher.Invoke(() => ChangeCoin.Visibility = Visibility.Visible);
                                question.Dispatcher.Invoke(() => question.Owner = Application.Current.MainWindow);
                                question.Dispatcher.Invoke(() => question.WindowStartupLocation = WindowStartupLocation.CenterOwner);
                                question.Dispatcher.Invoke(() => question.ShowDialog());
                                question.Close();
                            });

                            if (Variables.crypto == 0)
                            {
                                CoinName.Dispatcher.Invoke(() => CoinName.Text = "ETH");
                            }else if(Variables.crypto == 3)
                            {
                                CoinName.Dispatcher.Invoke(() => CoinName.Text = "ETC");
                            }
                        }
                        else if (Boolean.Parse(type["status"].ToString()) == true)
                        {
                            CoinName.Dispatcher.Invoke(() => CoinName.Text = "ETH");
                            Variables.crypto = 0;
                        }
                        else
                        {
                            Variables.crypto = 3;
                            CoinName.Dispatcher.Invoke(() => CoinName.Text = "ETC");
                        }
                    });
                }
                else if (WalletAddress.Text.ToLower().StartsWith("r"))
                {
                    CoinName.Dispatcher.Invoke(() => CoinName.Text = "RVN");
                    Variables.crypto = 1;
                }
                else if (WalletAddress.Text.StartsWith("9"))
                {
                    CoinName.Dispatcher.Invoke(() => CoinName.Text = "ERGO");
                    Variables.crypto = 2;
                }
                else if (WalletAddress.Text.ToLower().StartsWith("t"))
                {
                    CoinName.Dispatcher.Invoke(() => CoinName.Text = "ZEC");
                    Variables.crypto = 4;
                }
                else if (WalletAddress.Text.ToLower().StartsWith("aa"))
                {
                    CoinName.Dispatcher.Invoke(() => CoinName.Text = "CFX");
                    Variables.crypto = 6;
                }
                else if(WalletAddress.Text == null)
                {
                    CoinName.Dispatcher.Invoke(() => CoinName.Text = String.Empty);
                }
                else
                {
                    CoinName.Dispatcher.Invoke(() => CoinName.Text = "XMR");
                    Variables.crypto = 5;
                }
                Variables.CheckingCryptoCoin = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void GetInfoFromXML()
        {
            try
            {
                WalletAddress.Text = Variables.walletaddress;
                CurrencyList.SelectedIndex = Variables.fiat;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
