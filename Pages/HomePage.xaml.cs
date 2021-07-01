using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading.Tasks;

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
            RefreshMinersInfo();
            LoadMiners();
            LoadinAnimation();
            DataInfoUpdate();
        }
        int usedrigs = 0;

        private async Task DataInfoUpdate()
        {
            try
            {

                await Task.Run(async () =>
                {
                    do
                    {
                        if (Loading.Dispatcher.Invoke(() => Loading.Visibility == Visibility.Visible))
                        {
                            data.Dispatcher.Invoke(() => data.Text = Variables.datamaininfo);
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
        private async Task LoadinAnimation()
        {
            try
            {
                await Task.Run(async () =>
                {
    
                    string usedwallet = String.Empty;
                    do
                    {                   
                        if (usedwallet != Variables.lastvalidwallet && Variables.found == true)
                        {
                            usedwallet = Variables.lastvalidwallet;
                            do
                            {
                                if (Loading.Dispatcher.Invoke(() => Loading.Visibility == Visibility.Hidden))
                                {
                                    Scroll.Dispatcher.Invoke(() => Scroll.IsEnabled = false);
                                    Loading.Dispatcher.Invoke(() => Loading.Visibility = Visibility.Visible);
                                }
                                await Task.Delay(1000);
                            } while (Variables.FinishedVariables != true);

                            await Task.Delay(100);
                            Loading.Dispatcher.Invoke(() => Loading.Visibility = Visibility.Hidden);
                            Scroll.Dispatcher.Invoke(() => Scroll.IsEnabled = true);
                        }
                        else if(Variables.found == false)
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
        private async Task RefreshMinersInfo()
        {
            try
            {
                await Task.Run(async () =>
                {
                    do
                    {
                        await Task.Delay(2000);
                        if (Variables.rigs > 0)
                        {
                            usedrigs = 0;
                        }
                    } while (1 == 1);
                });
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private async Task RefreshInfo()
        {
            try
            {
                await Task.Run(async () =>
                {
                    do
                    {
                        if (Variables.lastvalidwallet != String.Empty)
                        {
                            //Main Infos
                            if(Variables.lastvalidwallet.Length > 50)
                            {
                                WalletAdd.Dispatcher.Invoke(() => WalletAdd.Text = Variables.lastvalidwallet.Substring(0,50) + "...");
                            }else
                            {
                                WalletAdd.Dispatcher.Invoke(() => WalletAdd.Text = Variables.lastvalidwallet);
                            }
                            MainCryptoName.Dispatcher.Invoke(() => MainCryptoName.Text = Variables.cryptovalues[Variables.crypto]);
                            MainFiatName.Dispatcher.Invoke(() => MainFiatName.Text = Variables.fiatvalues[Variables.fiat]);
                            MainCurrentHash.Dispatcher.Invoke(() => MainCurrentHash.Text = Variables.MainCurrentHash);
                            MainReportedHash.Dispatcher.Invoke(() => MainReportedHash.Text = Variables.MainReportedHash);
                            MainBalance.Dispatcher.Invoke(() => MainBalance.Text = Variables.MainBalance);
                            //Revenues
                            MainDailyCrypto.Dispatcher.Invoke(() => MainDailyCrypto.Text = Variables.MainCryptoDaily);
                            MainWeeklyCrypto.Dispatcher.Invoke(() => MainWeeklyCrypto.Text = Variables.MainCryptoWeekly);
                            MainMonthlyCrypto.Dispatcher.Invoke(() => MainMonthlyCrypto.Text = Variables.MainCryptoMonthly);
                            MainDailyFiat.Dispatcher.Invoke(() => MainDailyFiat.Text = Variables.MainFiatDaily);
                            MainWeeklyFiat.Dispatcher.Invoke(() => MainWeeklyFiat.Text = Variables.MainFiatWeekly);
                            MainMonthlyFiat.Dispatcher.Invoke(() => MainMonthlyFiat.Text = Variables.MainFiatMonthly);
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
                    insidewindow.Name = "insidewindow";
                    insidewindow.Height = 110;
                    insidewindow.Width = 626;
                    insidewindow.Background = Brushes.White;

                    TextBlock minername = new TextBlock();
                    minername.Text = Variables.rigname[value];
                    minername.Foreground = GetStatus(Variables.status[value]);
                    minername.FontSize = 20;
                    minername.HorizontalAlignment = HorizontalAlignment.Center;
                    minername.Margin = new Thickness(0, 10, 0, 0);

                    StackPanel one = new StackPanel();
                    one.Orientation = Orientation.Horizontal;
                    one.Margin = new Thickness(0, 20, 0, 0);

                    TextBlock minername0 = new TextBlock();
                    minername0.Text = "Reported Hashrate:";
                    minername0.FontSize = 15;
                    minername0.Margin = new Thickness(20, 0, 0, 0);

                    TextBlock minername1 = new TextBlock();
                    minername1.Text = Variables.rephash[value];
                    minername1.FontSize = 15;
                    minername1.Margin = new Thickness(5, 0, 0, 0);

                    TextBlock minername2 = new TextBlock();
                    minername2.Text = "Current Hashrate:";
                    minername2.FontSize = 15;
                    minername2.Margin = new Thickness(45, 0, 0, 0);

                    TextBlock minername3 = new TextBlock();
                    minername3.Text = Variables.currhash[value];
                    minername3.FontSize = 15;
                    minername3.Margin = new Thickness(5, 0, 0, 0);

                    TextBlock minername4 = new TextBlock();
                    minername4.Text = "Shares:";
                    minername4.FontSize = 15;
                    minername4.Margin = new Thickness(45, 0, 0, 0);

                    TextBlock minername5 = new TextBlock();
                    minername5.Text = Variables.sharesnum[value];
                    minername5.FontSize = 15;
                    minername5.Margin = new Thickness(5, 0, 0, 0);


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
        private async Task LoadMiners()
        {
            try
            {

                await Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    string lastusedwallet = String.Empty;
                    string firstrigname = String.Empty;
                    bool cleared = false;
                    do
                    {
                        if (usedrigs < Variables.rigs)
                        {
                            lastusedwallet = Variables.lastvalidwallet;
                            firstrigname = Variables.rigname[0];
                            cleared = false;
                            ShowMiners.Dispatcher.Invoke(() => ShowMiners.Children.Clear());

                            for (int i = 0; i < Variables.rigs; i++)
                            {
                                CreateInfo(i);
                                usedrigs++;
                            }
                            await Task.Delay(1000);
                            Variables.FinishedVariables = true;
                            Variables.datamaininfo = "Done";
                        }
                        else if ((usedrigs > Variables.rigs) || (((usedrigs == Variables.rigs) && (Variables.lastvalidwallet != lastusedwallet) && (firstrigname != Variables.rigname[0])) && usedrigs > 0) )
                        {
                            ShowMiners.Dispatcher.Invoke(() => ShowMiners.Children.Clear());
                            usedrigs = 0;
                        }
                        else if(Variables.lastvalidwallet == String.Empty && cleared == false)
                        {
                            ShowMiners.Dispatcher.Invoke(() => ShowMiners.Children.Clear());
                            cleared = true;
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
