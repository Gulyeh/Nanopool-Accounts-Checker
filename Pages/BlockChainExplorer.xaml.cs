using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;

namespace MiningCheck
{
    /// <summary>
    /// Logika interakcji dla klasy BlockChainExplorer.xaml
    /// </summary>
    public partial class BlockChainExplorer : Page
    {
        public BlockChainExplorer()
        {
            InitializeComponent();
            RefreshWalletInfo();
            LoadTransactions();
            LoadinAnimation();
            DataInfoUpdate();
        }

        private async void DataInfoUpdate()
        {
            try
            {               
                await Task.Run(async () =>
                {
                    do
                    {                     
                        if (LoadingChain.Dispatcher.Invoke(() => LoadingChain.Visibility == Visibility.Visible))
                        {
                            dataChain.Dispatcher.Invoke(() => dataChain.Text = Variables.LoadingWindow.blockchainDataInfo);
                        }
                        await Task.Delay(100);
                    } while (1 == 1);
                });
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void LoadinAnimation()
        {
            try
            {
                await Task.Run(async () =>
                {                    
                    string usedwallet = String.Empty;
                    do
                    {
                        
                        if (Variables.Checkers.ChainPageSelected == true)
                        {
                            if (usedwallet != Variables.Wallet.lastValidWallet && Variables.Checkers.found == true && Variables.Checkers.FinishedVariablesChain == false)
                            {
                                usedwallet = Variables.Wallet.lastValidWallet;
                                do
                                {
                                    if (LoadingChain.Dispatcher.Invoke(() => LoadingChain.Visibility == Visibility.Hidden))
                                    {
                                        ScrollChain.Dispatcher.Invoke(() => ScrollChain.ScrollToVerticalOffset(0));
                                        ScrollChain.Dispatcher.Invoke(() => ScrollChain.IsEnabled = false);
                                        LoadingChain.Dispatcher.Invoke(() => LoadingChain.Visibility = Visibility.Visible);
                                    }
                                    await Task.Delay(500);
                                } while (Variables.Checkers.FinishedVariablesChain == false);

                                LoadingChain.Dispatcher.Invoke(() => LoadingChain.Visibility = Visibility.Hidden);
                                ScrollChain.Dispatcher.Invoke(() => ScrollChain.IsEnabled = true);
                            }
                        }

                        if (Variables.Checkers.found == false)
                        {
                            usedwallet = String.Empty;
                        }

                        await Task.Delay(100);
                    } while (1 == 1);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DetailedChain_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = ((Button)sender).Name;
                int objvalue = Int32.Parse(name.Substring(13, name.Length-13));
                BlockChainDetailed details = new BlockChainDetailed(objvalue);
                details.Owner = Application.Current.MainWindow;
                details.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                details.ShowDialog();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ToolTip_MouseMove(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).IsMouseOver)
            {
                ((Button)sender).ToolTip = "Detailed Transaction Informations";
            }
        }

        private void TransactionHistory(int i)
        {
            try
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    StackPanel Background = new StackPanel();
                    Background.Name = "Background";
                    Background.Height = 40;
                    Background.Width = 650;
                    Background.Background = Brushes.White;

                    Border brdr = new Border();
                    brdr.BorderThickness = new Thickness(0, 0.6, 0, 0);
                    brdr.BorderBrush = Brushes.Black;

                    StackPanel one = new StackPanel();
                    one.Orientation = Orientation.Horizontal;
                    one.Height = 50;
                    one.Width = 650;

                    Button btn = new Button();
                    btn.Name = "DetailedChain" + i.ToString();
                    btn.Background = Brushes.White;
                    btn.Width = 15;
                    btn.Height = 15;
                    btn.HorizontalAlignment = HorizontalAlignment.Left;
                    btn.Margin = new Thickness(10, -10, 0, 0);
                    btn.Content = new Image { Source = new BitmapImage(new Uri("C:/Users/Gulyeh/Desktop/Icon/eye.png")) };
                    btn.Click += DetailedChain_Click;
                    btn.MouseMove += new MouseEventHandler(ToolTip_MouseMove);

                    TextBlock txn = new TextBlock();
                    txn.Text = Variables.BlockchainData.BlockHash[i].Substring(0, 11) + "...";
                    txn.Margin = new Thickness(15, 11, 0, 10);

                    TextBlock block = new TextBlock();
                    block.Text = Variables.BlockchainData.BlockNumber[i];
                    block.Margin = new Thickness(52, 11, 0, 0);

                    TextBlock date = new TextBlock();
                    date.Text = Variables.BlockchainData.TransactionDate[i];
                    date.Margin = new Thickness(25, 11, 0, 0);

                    TextBlock from = new TextBlock();
                    from.Text = Variables.BlockchainData.FromWallet[i].Substring(0, 13) + "...";
                    from.Margin = new Thickness(10, 11, 0, 0);

                    TextBlock to = new TextBlock();
                    to.Text = Variables.BlockchainData.toWallet[i].Substring(0, 13) + "...";
                    to.Margin = new Thickness(22, 11, 0, 0);

                    TextBlock value = new TextBlock();
                    value.Text = Variables.BlockchainData.TransferedValue[i].ToString();
                    value.Margin = new Thickness(26, 11, 0, 0);

                    ShowTransactions.Children.Add(Background);
                    Background.Children.Add(brdr);
                    Background.Children.Add(one);
                    one.Children.Add(btn);
                    one.Children.Add(txn);
                    one.Children.Add(block);
                    one.Children.Add(date);
                    one.Children.Add(from);
                    one.Children.Add(to);
                    one.Children.Add(value);

                });
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void LoadTransactions()
        {
            try
            {
                
                await Task.Run(async () =>
                {
                    bool cleared = false;
                    string lastwallet = String.Empty;
                    int transactions = 0;
                    do
                    {
                        await Task.Delay(2000);
                        if(Variables.Checkers.ChainPageSelected == true && Variables.BlockchainData.NumberTransactions > 0 && Variables.Checkers.FinishedVariables == true && Variables.Checkers.FinishedGettingBlockData == true && Variables.Checkers.found == true)
                        {
                            if(transactions < Variables.BlockchainData.NumberTransactions)
                            {
                                ShowTransactions.Dispatcher.Invoke(() => ShowTransactions.Children.Clear());
                                cleared = false;
                                lastwallet = Variables.Wallet.lastValidWallet;
                                Thread.Sleep(1000);
                                for (int i = 0; i < Variables.BlockchainData.NumberTransactions; i++)
                                {
                                    if (Variables.BlockchainData.BlockHash[i] != null)
                                    {
                                        transactions++;
                                        TransactionHistory(i);
                                    }
                                    await Task.Delay(10);
                                }
                                Variables.LoadingWindow.blockchainDataInfo = "Done";
                                await Task.Delay(1000);
                            }
                            else if(transactions == Variables.BlockchainData.NumberTransactions)
                            {
                                if (Variables.Wallet.lastValidWallet != lastwallet)
                                {
                                    ShowTransactions.Dispatcher.Invoke(() => ShowTransactions.Children.Clear());
                                    transactions = 0;
                                }
                            }else if(transactions > Variables.BlockchainData.NumberTransactions)
                            {
                                ShowTransactions.Dispatcher.Invoke(() => ShowTransactions.Children.Clear());
                                transactions = 0;
                            }

                            if (ShowTransactions.Dispatcher.Invoke(() => ShowTransactions.Children.Count > 0))
                            {
                                Variables.Checkers.FinishedVariablesChain = true;
                            }
                        }
                        else if(Variables.BlockchainData.NumberTransactions < 1 && cleared == false)
                        {
                            ShowTransactions.Dispatcher.Invoke(() => ShowTransactions.Children.Clear());
                            transactions = 0;
                            cleared = true;
                        }
                                             
                    } while (1 == 1);
                });

                }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void RefreshWalletInfo()
        {
            try
            {      
                await Task.Run(async () =>
                {
                    do
                    {  
                        if (Variables.Checkers.ChainPageSelected == true)
                        {
                            if (Variables.Checkers.found == true)
                            {
                                WalletBalance.Dispatcher.Invoke(() => WalletBalance.Text = Variables.BlockchainData.BlockBalance.ToString("N7") + " " + Variables.CryptoInfo.cryptoValues[Variables.CryptoInfo.cryptoSelected]);
                                CoinPrice.Dispatcher.Invoke(() => CoinPrice.Text = Variables.Price.CoinPrices[Variables.FiatInfo.fiatSelected] + " " + Variables.FiatInfo.fiatValues[Variables.FiatInfo.fiatSelected]);
    
                                if (Variables.BlockchainData.BlockBalance != 0 && float.Parse(Variables.Price.CoinPrices[Variables.FiatInfo.fiatSelected]) >= 0)
                                {
                                    float price = Variables.BlockchainData.BlockBalance * float.Parse(Variables.Price.CoinPrices[Variables.FiatInfo.fiatSelected]);
                                    WalletValue.Dispatcher.Invoke(() => WalletValue.Text = price.ToString("N2") + " " + Variables.FiatInfo.fiatValues[Variables.FiatInfo.fiatSelected]);
                                }
                                else
                                {
                                    WalletValue.Dispatcher.Invoke(() => WalletValue.Text = "0 " + Variables.FiatInfo.fiatValues[Variables.FiatInfo.fiatSelected]);
                                }
                            }
                            else
                            {
                                WalletBalance.Dispatcher.Invoke(() => WalletBalance.Text = "-");
                                CoinPrice.Dispatcher.Invoke(() => CoinPrice.Text = "-");
                                WalletValue.Dispatcher.Invoke(() => WalletValue.Text = "-");
                            }
                        }
                        await Task.Delay(100);
                    } while (1 == 1);
                });
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
