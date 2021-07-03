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
            foreach(string value in Variables.FiatInfo.fiatValues)
            {
                CurrencyList.Items.Add(value);
            }
        }
        public async void BlockText()
        {
            try
            {
                await Task.Run(async () =>
                {
                    bool blocked = false;
                    while (!Variables.Checkers.FinishedVariables)
                    {
                        if (CoinName.Dispatcher.Invoke(() => CoinName.Text != String.Empty) && blocked == false && Variables.Checkers.found == true)
                        {
                            WalletAddress.Dispatcher.Invoke(() => WalletAddress.IsEnabled = false);
                            blocked = true;
                        }
                        await Task.Delay(10); 
                    }
                    WalletAddress.Dispatcher.Invoke(() => WalletAddress.IsEnabled = true);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void FiatBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrencyList.Dispatcher.Invoke(() => Variables.FiatInfo.fiatSelected = CurrencyList.SelectedIndex);
        }
        private void textChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            WalletAddress.Dispatcher.Invoke(() => Variables.Wallet.walletAddress = WalletAddress.Text);
            Variables.ResetVars();
            DetectCoin();
            BlockText();
        }
        private void OpenQuestionWindow(object sender, RoutedEventArgs e)
        {
            try
            {
                CoinQuestion question = new CoinQuestion();
                question.Owner = Application.Current.MainWindow;
                question.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                question.ShowDialog();
                Variables.ResetVars();
                if (Variables.CryptoInfo.cryptoSelected == 0)
                {
                    CoinName.Dispatcher.Invoke(() => CoinName.Text = "ETH");
                }
                else if (Variables.CryptoInfo.cryptoSelected == 3)
                {
                    CoinName.Dispatcher.Invoke(() => CoinName.Text = "ETC");
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }       
        private async void DetectCoin()
        {
            try
            {
                Variables.Checkers.CheckingCryptoCoin = true;
                ChangeCoin.Dispatcher.Invoke(() => ChangeCoin.Visibility = Visibility.Hidden);
                if (WalletAddress.Text.ToLower().StartsWith("0x") && WalletAddress.Text.Length == 42)
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

                            if (Variables.CryptoInfo.cryptoSelected == 0)
                            {
                                CoinName.Dispatcher.Invoke(() => CoinName.Text = "ETH");
                            }else if(Variables.CryptoInfo.cryptoSelected == 3)
                            {
                                CoinName.Dispatcher.Invoke(() => CoinName.Text = "ETC");
                            }
                        }
                        else if (Boolean.Parse(type["status"].ToString()) == true)
                        {
                            CoinName.Dispatcher.Invoke(() => CoinName.Text = "ETH");
                            Variables.CryptoInfo.cryptoSelected = 0;
                        }
                        else
                        {
                            Variables.CryptoInfo.cryptoSelected = 3;
                            CoinName.Dispatcher.Invoke(() => CoinName.Text = "ETC");
                        }
                    });
                }
                else if (WalletAddress.Text.ToLower().StartsWith("r") && WalletAddress.Text.Length == 34)
                {
                    CoinName.Dispatcher.Invoke(() => CoinName.Text = "RVN");
                    Variables.CryptoInfo.cryptoSelected = 1;
                }
                else if (WalletAddress.Text.StartsWith("9") && WalletAddress.Text.Length == 51)
                {
                    CoinName.Dispatcher.Invoke(() => CoinName.Text = "ERGO");
                    Variables.CryptoInfo.cryptoSelected = 2;
                }
                else if (WalletAddress.Text.ToLower().StartsWith("t") && WalletAddress.Text.Length == 35)
                {
                    CoinName.Dispatcher.Invoke(() => CoinName.Text = "ZEC");
                    Variables.CryptoInfo.cryptoSelected = 4;
                }
                else if (WalletAddress.Text.ToLower().StartsWith("aa") && WalletAddress.Text.Length == 42)
                {
                    CoinName.Dispatcher.Invoke(() => CoinName.Text = "CFX");
                    Variables.CryptoInfo.cryptoSelected = 6;
                }
                else if (WalletAddress.Text.Length == 95)
                {
                    CoinName.Dispatcher.Invoke(() => CoinName.Text = "XMR");
                    Variables.CryptoInfo.cryptoSelected = 5;
                }
                else
                {
                    CoinName.Dispatcher.Invoke(() => CoinName.Text = String.Empty);
                }
                Variables.Checkers.CheckingCryptoCoin = false;
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
                WalletAddress.Text = Variables.Wallet.walletAddress;
                CurrencyList.SelectedIndex = Variables.FiatInfo.fiatSelected;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
