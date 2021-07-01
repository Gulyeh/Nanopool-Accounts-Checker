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
using System.Threading;
using System.Threading.Tasks;

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
        int transactions = 0;

        private async Task DataInfoUpdate()
        {
            try
            {               
                await Task.Run(async () =>
                {
                    do
                    {                     
                        if (LoadingChain.Dispatcher.Invoke(() => LoadingChain.Visibility == Visibility.Visible))
                        {
                            dataChain.Dispatcher.Invoke(() => dataChain.Text = Variables.datainfo);
                        }
                        await Task.Delay(100);
                    } while (1 == 1);
                });
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async Task LoadinAnimation()
        {
            try
            {
                await Task.Run(async () =>
                {                    
                    string usedwallet = String.Empty;
                    do
                    {
                        
                        if (Variables.ChainPageSelected == true)
                        {
                            if (usedwallet != Variables.lastvalidwallet && Variables.found == true)
                            {
                                usedwallet = Variables.lastvalidwallet;
                                do
                                {
                                    if (LoadingChain.Dispatcher.Invoke(() => LoadingChain.Visibility == Visibility.Hidden))
                                    {
                                        ScrollChain.Dispatcher.Invoke(() => ScrollChain.ScrollToVerticalOffset(0));
                                        ScrollChain.Dispatcher.Invoke(() => ScrollChain.IsEnabled = false);
                                        LoadingChain.Dispatcher.Invoke(() => LoadingChain.Visibility = Visibility.Visible);
                                    }
                                    await Task.Delay(500);
                                } while (Variables.FinishedVariablesChain != true);

                                LoadingChain.Dispatcher.Invoke(() => LoadingChain.Visibility = Visibility.Hidden);
                                ScrollChain.Dispatcher.Invoke(() => ScrollChain.IsEnabled = true);
                            }
                            else if (Variables.found == false)
                            {
                                usedwallet = String.Empty;
                            }
                        }
                        await Task.Delay(2000);
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
                    txn.Text = Variables.BlockHash[i].Substring(0, 11) + "...";
                    txn.Margin = new Thickness(15, 11, 0, 10);

                    TextBlock block = new TextBlock();
                    block.Text = Variables.BlockNumber[i];
                    block.Margin = new Thickness(52, 11, 0, 0);

                    TextBlock date = new TextBlock();
                    date.Text = Variables.TransactionDate[i];
                    date.Margin = new Thickness(25, 11, 0, 0);

                    TextBlock from = new TextBlock();
                    from.Text = Variables.FromWallet[i].Substring(0, 13) + "...";
                    from.Margin = new Thickness(10, 11, 0, 0);

                    TextBlock to = new TextBlock();
                    to.Text = Variables.toWallet[i].Substring(0, 13) + "...";
                    to.Margin = new Thickness(22, 11, 0, 0);

                    TextBlock value = new TextBlock();
                    value.Text = Variables.TransferedValue[i].ToString();
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

        private async Task LoadTransactions()
        {
            try
            {
                
                await Task.Run(async () =>
                {
                    bool cleared = false;
                    string firstblock = String.Empty;
                    do
                    {
                        await Task.Delay(2000);

                        if (Variables.ChainPageSelected == true && Variables.FinishedVariables == true && Variables.FinishedGettingBlockData == true)
                        {
                            if (Variables.NumberTransactions > 0 && Variables.lastvalidwallet != String.Empty)
                            {

                                if (transactions < Variables.NumberTransactions && Variables.found == true && Variables.FinishedGettingBlockData == true)
                                {
                                    ShowTransactions.Dispatcher.Invoke(() => ShowTransactions.Children.Clear());
                                    Thread.Sleep(1000);
                                    cleared = false;
                                    for (int i = 0; i < Variables.NumberTransactions; i++)
                                    {
                                        if (Variables.BlockHash[i] != null)
                                        {
                                            transactions++;
                                            TransactionHistory(i);
                                        }
                                        await Task.Delay(10);
                                    }
                                    Variables.datainfo = "Done";
                                    await Task.Delay(500);
                                    if (ShowTransactions.Dispatcher.Invoke(() => ShowTransactions.Children.Count > 0))
                                    {
                                        Variables.FinishedVariablesChain = true;
                                    }
                                    firstblock = Variables.BlockNumber[0];
                                }
                                if ((transactions > Variables.NumberTransactions) || (transactions == Variables.NumberTransactions && transactions > 0 && firstblock != Variables.BlockNumber[0]))
                                {
                                    ShowTransactions.Dispatcher.Invoke(() => ShowTransactions.Children.Clear());
                                    transactions = 0;
                                }
                                else if (Variables.lastvalidwallet == String.Empty && cleared == false)
                                {
                                    ShowTransactions.Dispatcher.Invoke(() => ShowTransactions.Children.Clear());
                                    transactions = 0;
                                    cleared = true;
                                }
                            }
                            else if ((Variables.NumberTransactions == 0 || Variables.lastvalidwallet == String.Empty) && cleared == false)
                            {
                                MessageBox.Show("here");
                                ShowTransactions.Dispatcher.Invoke(() => ShowTransactions.Children.Clear());
                                Variables.FinishedVariablesChain = true;
                                transactions = 0;
                                cleared = true;
                            }
                            else if(Variables.found == false)
                            {
                                Variables.FinishedVariablesChain = true;
                                transactions = 0;
                            }
                        }else if(Variables.NumberTransactions == 0 && Variables.ChainPageSelected == true && cleared == false)
                        {
                            cleared = true;
                            ShowTransactions.Dispatcher.Invoke(() => ShowTransactions.Children.Clear());
                        }

                    } while (1 == 1);
                });

                }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async Task RefreshWalletInfo()
        {
            try
            {      
                await Task.Run(async () =>
                {
                    do
                    {  
                        if (Variables.ChainPageSelected == true)
                        {
                            if (Variables.lastvalidwallet != String.Empty)
                            {
                                WalletBalance.Dispatcher.Invoke(() => WalletBalance.Text = Variables.BlockBalance.ToString("N7") + " " + Variables.cryptovalues[Variables.crypto]);
                                CoinPrice.Dispatcher.Invoke(() => CoinPrice.Text = Variables.CoinPrices[Variables.fiat] + " " + Variables.fiatvalues[Variables.fiat]);
    
                                if (Variables.BlockBalance != 0 && float.Parse(Variables.CoinPrices[Variables.fiat]) >= 0)
                                {
                                    float price = Variables.BlockBalance * float.Parse(Variables.CoinPrices[Variables.fiat]);
                                    WalletValue.Dispatcher.Invoke(() => WalletValue.Text = price.ToString("N2") + " " + Variables.fiatvalues[Variables.fiat]);
                                }
                                else
                                {
                                    WalletValue.Dispatcher.Invoke(() => WalletValue.Text = "0 " + Variables.fiatvalues[Variables.fiat]);
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
