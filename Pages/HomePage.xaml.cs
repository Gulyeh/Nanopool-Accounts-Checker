using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Threading;

namespace MiningCheck
{
    /// <summary>
    /// Logika interakcji dla klasy Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
            RefreshInfo();
            LoadMiners();
            LoadinAnimation();
            DataInfoUpdate();
        }
        int usedrigs = 0;

        private async void DataInfoUpdate()
        {
            try
            {

                await Task.Run(async () =>
                {
                    do
                    {
                        if (Loading.Dispatcher.Invoke(() => Loading.Visibility == Visibility.Visible))
                        {
                            data.Dispatcher.Invoke(() => data.Text = Variables.LoadingWindow.mainDataInfo);
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
        private async void LoadinAnimation()
        {
            try
            {
                await Task.Run(async () =>
                {
    
                    string usedwallet = String.Empty;
                    do
                    {                   
                        if (usedwallet != Variables.Wallet.lastValidWallet && Variables.Checkers.found == true)
                        {
                            usedwallet = Variables.Wallet.lastValidWallet;
                            do
                            {
                                if (Loading.Dispatcher.Invoke(() => Loading.Visibility == Visibility.Hidden))
                                {
                                    Scroll.Dispatcher.Invoke(() => Scroll.IsEnabled = false);
                                    Loading.Dispatcher.Invoke(() => Loading.Visibility = Visibility.Visible);
                                }
                                await Task.Delay(1000);
                            } while (Variables.Checkers.FinishedVariables != true);

                            await Task.Delay(100);
                            Loading.Dispatcher.Invoke(() => Loading.Visibility = Visibility.Hidden);
                            Scroll.Dispatcher.Invoke(() => Scroll.IsEnabled = true);
                        }
                        else if(Variables.Checkers.found == false)
                        {
                            usedwallet = String.Empty;
                        }

                        await Task.Delay(200);                      
                    } while (1 == 1);
                });
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private async void RefreshMinersInfo()
        {
            try
            {
                var cancelToken = new CancellationTokenSource();
                await Task.Run(async () =>
                {
                    int saverigs = Variables.RigInfo.rigsAmount;
                    
                    do
                    {
                        await Task.Delay(10000);
                        for (int i = 0; i < Variables.RigInfo.rigsAmount; i++)
                        {
                            if (Variables.Checkers.found == true)
                            {
                                Application.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    TextBlock name = (TextBlock)this.FindName("textrigname" + i);
                                    TextBlock rephash = (TextBlock)this.FindName("textrephash" + i);
                                    TextBlock currhash = (TextBlock)this.FindName("textcurrhash" + i);
                                    TextBlock shares = (TextBlock)this.FindName("textshares" + i);
                                    name.Text = Variables.RigInfo.rigName[i];
                                    rephash.Text = Variables.RigInfo.repHash[i];
                                    currhash.Text = Variables.RigInfo.currHash[i];
                                    shares.Text = Variables.RigInfo.sharesNum[i];
                                });
                            }
                            else
                            {
                                break;
                            }
                            await Task.Delay(10);
                        }
                    } while (Variables.RigInfo.rigsAmount > 0);


                    for (int i = 0; i < saverigs; i++)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            this.UnregisterName("textrigname" + i);
                            this.UnregisterName("textrephash" + i);
                            this.UnregisterName("textcurrhash" + i);
                            this.UnregisterName("textshares" + i);
                        });
                        await Task.Delay(1);
                    }

                }, cancelToken.Token);

            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private async void RefreshInfo()
        {
            try
            {
                await Task.Run(async () =>
                {
                    do
                    {
                        if (Variables.Checkers.found == true && Variables.MinerData.MainBalance.Length > 0)
                        {
                            //Main Infos
                            if(Variables.Wallet.lastValidWallet.Length > 50)
                            {
                                WalletAdd.Dispatcher.Invoke(() => WalletAdd.Text = Variables.Wallet.lastValidWallet.Substring(0,50) + "...");
                            }else
                            {
                                WalletAdd.Dispatcher.Invoke(() => WalletAdd.Text = Variables.Wallet.lastValidWallet);
                            }
                            MainCryptoName.Dispatcher.Invoke(() => MainCryptoName.Text = Variables.CryptoInfo.cryptoValues[Variables.CryptoInfo.cryptoSelected]);
                            MainFiatName.Dispatcher.Invoke(() => MainFiatName.Text = Variables.FiatInfo.fiatValues[Variables.FiatInfo.fiatSelected]);
                            MainCurrentHash.Dispatcher.Invoke(() => MainCurrentHash.Text = Variables.MinerData.MainCurrentHash);
                            MainReportedHash.Dispatcher.Invoke(() => MainReportedHash.Text = Variables.MinerData.MainReportedHash);
                            MainBalance.Dispatcher.Invoke(() => MainBalance.Text = Variables.MinerData.MainBalance);
                            //Revenues
                            MainDailyCrypto.Dispatcher.Invoke(() => MainDailyCrypto.Text = Variables.RigRevenue.MainCryptoDaily);
                            MainWeeklyCrypto.Dispatcher.Invoke(() => MainWeeklyCrypto.Text = Variables.RigRevenue.MainCryptoWeekly);
                            MainMonthlyCrypto.Dispatcher.Invoke(() => MainMonthlyCrypto.Text = Variables.RigRevenue.MainCryptoMonthly);
                            MainDailyFiat.Dispatcher.Invoke(() => MainDailyFiat.Text = Variables.RigRevenue.MainFiatDaily);
                            MainWeeklyFiat.Dispatcher.Invoke(() => MainWeeklyFiat.Text = Variables.RigRevenue.MainFiatWeekly);
                            MainMonthlyFiat.Dispatcher.Invoke(() => MainMonthlyFiat.Text = Variables.RigRevenue.MainFiatMonthly);
                        }
                        else
                        {
                            //Main infos
                            MainCryptoName.Dispatcher.Invoke(() => MainCryptoName.Text = "-");
                            MainFiatName.Dispatcher.Invoke(() => MainFiatName.Text = "-");
                            WalletAdd.Dispatcher.Invoke(() => WalletAdd.Text = "-");
                            MainCurrentHash.Dispatcher.Invoke(() => MainCurrentHash.Text = "-");
                            MainReportedHash.Dispatcher.Invoke(() => MainReportedHash.Text = "-");
                            MainBalance.Dispatcher.Invoke(() => MainBalance.Text = "-");
                            //Revenues
                            MainDailyCrypto.Dispatcher.Invoke(() => MainDailyCrypto.Text = "-");
                            MainWeeklyCrypto.Dispatcher.Invoke(() => MainWeeklyCrypto.Text = "-");
                            MainMonthlyCrypto.Dispatcher.Invoke(() => MainMonthlyCrypto.Text = "-");
                            MainDailyFiat.Dispatcher.Invoke(() => MainDailyFiat.Text = "-");
                            MainWeeklyFiat.Dispatcher.Invoke(() => MainWeeklyFiat.Text = "-");
                            MainMonthlyFiat.Dispatcher.Invoke(() => MainMonthlyFiat.Text = "-");

                        }
                        await Task.Delay(100);
                    } while (1 == 1);
                });
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        Brush GetStatus(int status)
        {
            if (status == 0)
            {
                return Brushes.Red;
            }else
            {
                return Brushes.Green;
            }
        }
        private void CreateInfo(int value)
        {
            try
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    StackPanel str = new StackPanel();
                    str.Name = "str";
                    str.Height = 133;
                    str.Width = 628;
                    str.Background = Brushes.Transparent;

                    Border brdr = new Border();
                    brdr.BorderThickness = new Thickness(0, 1, 0, 0);
                    brdr.BorderBrush = Brushes.Black;

                    StackPanel insidewindow = new StackPanel();
                    insidewindow.Height = 110;
                    insidewindow.Width = 626;
                    insidewindow.Background = Brushes.White;

                    TextBlock minername = new TextBlock();
                    minername.Text = Variables.RigInfo.rigName[value];
                    minername.Foreground = GetStatus(Variables.RigInfo.status[value]);
                    minername.FontSize = 20;
                    minername.HorizontalAlignment = HorizontalAlignment.Center;
                    minername.Margin = new Thickness(0, 10, 0, 0);
                    this.RegisterName("textrigname" + value, minername);

                    StackPanel one = new StackPanel();
                    one.Orientation = Orientation.Horizontal;
                    one.Margin = new Thickness(0, 20, 0, 0);

                    TextBlock minername0 = new TextBlock();
                    minername0.Text = "Reported Hashrate:";
                    minername0.FontSize = 15;
                    minername0.Margin = new Thickness(20, 0, 0, 0);

                    TextBlock minername1 = new TextBlock();
                    minername1.Text = Variables.RigInfo.repHash[value];
                    minername1.FontSize = 15;
                    minername1.Margin = new Thickness(5, 0, 0, 0);
                    this.RegisterName("textrephash" + value, minername1);

                    TextBlock minername2 = new TextBlock();
                    minername2.Text = "Current Hashrate:";
                    minername2.FontSize = 15;
                    minername2.Margin = new Thickness(45, 0, 0, 0);

                    TextBlock minername3 = new TextBlock();
                    minername3.Text = Variables.RigInfo.currHash[value];
                    minername3.FontSize = 15;
                    minername3.Margin = new Thickness(5, 0, 0, 0);
                    this.RegisterName("textcurrhash" + value, minername3);

                    TextBlock minername4 = new TextBlock();
                    minername4.Text = "Shares:";
                    minername4.FontSize = 15;
                    minername4.Margin = new Thickness(45, 0, 0, 0);

                    TextBlock minername5 = new TextBlock();
                    minername5.Text = Variables.RigInfo.sharesNum[value];
                    minername5.FontSize = 15;
                    minername5.Margin = new Thickness(5, 0, 0, 0);
                    this.RegisterName("textshares" + value, minername5);

                    ShowMiners.Children.Add(str);
                    str.Children.Add(brdr);
                    str.Children.Add(insidewindow);
                    insidewindow.Children.Add(minername);
                    insidewindow.Children.Add(one);
                    one.Children.Add(minername0);
                    one.Children.Add(minername1);
                    one.Children.Add(minername2);
                    one.Children.Add(minername3);
                    one.Children.Add(minername4);
                    one.Children.Add(minername5);

                });
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private async void LoadMiners()
        {
            try
            {

                await Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    string lastusedwallet = String.Empty;
                    bool cleared = false;
                    do
                    {                   
                        if (Variables.RigInfo.rigsAmount > 0)
                        {
                            if (usedrigs < Variables.RigInfo.rigsAmount)
                            {
                                lastusedwallet = Variables.Wallet.lastValidWallet;
                                cleared = false;
                                ShowMiners.Dispatcher.Invoke(() => ShowMiners.Children.Clear());
                                await Task.Delay(1000);
                                for (int i = 0; i < Variables.RigInfo.rigsAmount; i++)
                                {
                                    CreateInfo(i);
                                    usedrigs++;
                                    await Task.Delay(1);
                                }
                                RefreshMinersInfo();
                                Variables.Checkers.FinishedVariables = true;
                                Variables.LoadingWindow.mainDataInfo = "Done";
                            }
                            else if (usedrigs == Variables.RigInfo.rigsAmount)
                            {
                                if (lastusedwallet != Variables.Wallet.lastValidWallet)
                                {
                                    ShowMiners.Dispatcher.Invoke(() => ShowMiners.Children.Clear());
                                    usedrigs = 0;
                                    cleared = true;
                                }
                            }
                            else if (usedrigs > Variables.RigInfo.rigsAmount)
                            {
                                ShowMiners.Dispatcher.Invoke(() => ShowMiners.Children.Clear());
                                usedrigs = 0;
                                cleared = true;
                            }
                        } else if (cleared == false)
                        {
                            ShowMiners.Dispatcher.Invoke(() => ShowMiners.Children.Clear());
                            usedrigs = 0;
                            cleared = true;
                        }
                        await Task.Delay(1000);
                    } while (1 == 1);
                });
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
