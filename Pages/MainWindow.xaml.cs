using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MiningCheck
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Page1 infopage;
        Page2 settingspage;
        Page3 homepage;
        BlockChainExplorer blockchain;

        public MainWindow()
        {
            InitializeComponent();
            Startups.CheckConfig("Load");
            VariablesChecking();
            GetBlockchainData();
            CheckIfOffline();
            LoadPages();
            if (Variables.ConfigExisted)
            {
                MainPanel.Content = homepage;
                Home_Button.IsSelected = true;
            }
            else
            {
                MainPanel.Content = settingspage;
                Settings_Button.IsSelected = true;
            }
        }


        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Minimized:
                    NotifyIcon.Visibility = Visibility.Visible;
                    NotifyIcon.ShowBalloonTip("Minimalized", "I'am still watching!", BalloonIcon.None);
                    NotifyIcon.TrayLeftMouseDown += _notifyIcon_DoubleClick;
                    this.ShowInTaskbar = false;
                    break;
                case WindowState.Normal:
                    NotifyIcon.Visibility = Visibility.Hidden;
                    this.ShowInTaskbar = true;
                    break;
                default:
                    NotifyIcon.Visibility = Visibility.Hidden;
                    this.ShowInTaskbar = true;
                    break;
            }
        }

        private void _notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void NotifyExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async Task CheckIfOffline()
        {
            try
            {
                await Task.Run(async () =>
                {

                    do
                    {
                        string offlinerigs = String.Empty;
                        if (Variables.rigs > 0 && Variables.found == true)
                        {
                            for (int i = 0; i < Variables.rigs; i++)
                            {
                                if (Variables.status[i] == 1)
                                {
                                    Variables.IsOnline[i] = true;
                                }
                                else if (Variables.status[i] == 0 && Variables.IsOnline[i] == true)
                                {
                                    Variables.IsOnline[i] = false;
                                    offlinerigs += Variables.rigname[i] + "\n";
                                }
                                await Task.Delay(100);
                            }

                            if (offlinerigs != String.Empty)
                            {
                                NotifyIcon.Dispatcher.Invoke(() => NotifyIcon.Visibility = Visibility.Visible);
                                NotifyIcon.ShowBalloonTip("Nanopool Mining Stats - Found Offline Rigs", offlinerigs, BalloonIcon.None);
                                await Task.Delay(4000);
                                NotifyIcon.Dispatcher.Invoke(() => NotifyIcon.Visibility = Visibility.Hidden);
                            }
                        }
                        await Task.Delay(5000);
                    } while (1 == 1);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async Task VariablesChecking()
        {
            try
            {
                await Task.Run(async () =>
                {
                    //Fill Variables once before executing loop to prevent unexpected crashes
                    do
                    {
                        //Looping to keep filling variables if user is in database
                        if (Variables.CheckingCryptoCoin == false)
                        {
                            if (Variables.walletaddress != Variables.lastvalidwallet)
                            {
                                Variables.FinishedVariables = false;
                            }
                            await Startups.FillJsonVariables();
                            await Task.Delay(10000);
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

        private async Task GetBlockchainData()
        {
            try
            {
                await Task.Run(async () =>
                {
                    do
                    {
                        await Task.Delay(5000);
                        if (Variables.ChainPageSelected == true && Variables.FinishedVariables == true)
                        {
                            await BlockChainData.GetData(Variables.cryptovalues[Variables.crypto]);
                        }
                    } while (1 == 1);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoadPages()
        {
            infopage = new Page1();
            settingspage = new Page2();
            homepage = new Page3();
            blockchain = new BlockChainExplorer();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Startups.CheckConfig("Save");
        }

        private void Home_Button_Selected(object sender, RoutedEventArgs e)
        {
            MainPanel.Content = homepage;
            Variables.ChainPageSelected = false;
        }

        private void Settings_Button_Selected(object sender, RoutedEventArgs e)
        {
            MainPanel.Content = settingspage;
            Variables.ChainPageSelected = false;
        }

        private void Info_Button_Selected(object sender, RoutedEventArgs e)
        {
            MainPanel.Content = infopage;
            Variables.ChainPageSelected = false;
        }

        private void Block_Button_Selected(object sender, RoutedEventArgs e)
        {
            MainPanel.Content = blockchain;
            Variables.ChainPageSelected = true;
        }

    }
}
