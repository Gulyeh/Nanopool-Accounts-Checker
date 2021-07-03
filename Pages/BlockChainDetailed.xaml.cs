using System;
using System.Windows;


namespace MiningCheck
{
    /// <summary>
    /// Logika interakcji dla klasy BlockChainDetailed.xaml
    /// </summary>
    public partial class BlockChainDetailed : Window
    {
        public BlockChainDetailed(int index)
        {
            InitializeComponent();
            InitializeData(index);
        }       
        private void InitializeData(int index)
        {
            try
            {
                if(Variables.BlockchainData.IsError[index] == 0)
                {
                    successicon.Visibility = Visibility.Visible;
                }
                else
                {
                    failedicon.Visibility = Visibility.Visible;
                }

                TransHash.Text = Variables.BlockchainData.BlockHash[index];
                fromwallet.Text = Variables.BlockchainData.FromWallet[index];
                towallet.Text = Variables.BlockchainData.toWallet[index];
                DateTime today = DateTime.Now;
                DateTime transdate = Convert.ToDateTime(Variables.BlockchainData.TransactionDate[index]);
                string daystimepassed = (today - transdate).ToString().Split('.')[0] + " days";
                string minutestimepassed = String.Empty;
                string hourstimepassed = String.Empty;

                if (daystimepassed.Contains(":"))
                {
                    daystimepassed = "0 days";
                    hourstimepassed = (today - transdate).ToString().Split(':')[0] + " hours";
                    minutestimepassed = (today - transdate).ToString().Split(':')[1] + " minutes ago";

                }
                else
                {
                    hourstimepassed = (today - transdate).ToString().Split('.')[1].Split(':')[0] + " hours";
                    minutestimepassed = (today - transdate).ToString().Split('.')[1].Split(':')[1] + " minutes ago";
                }

                timestamp.Text = daystimepassed+" "+hourstimepassed+ " " +minutestimepassed+" ("+Variables.BlockchainData.TransactionDate[index]+")";
                float transferedprice = Variables.BlockchainData.TransferedValue[index] * float.Parse(Variables.Price.CoinPrices[Variables.FiatInfo.fiatSelected]);
                transfervalue.Text = Variables.BlockchainData.TransferedValue[index].ToString() + " " + Variables.CryptoInfo.cryptoValues[Variables.CryptoInfo.cryptoSelected] + " ("+transferedprice.ToString("N2")+"$)";
                float feeprice = Variables.BlockchainData.FeeValue[index] * float.Parse(Variables.Price.CoinPrices[Variables.FiatInfo.fiatSelected]);
                fee.Text = Variables.BlockchainData.FeeValue[index].ToString() + " " + Variables.CryptoInfo.cryptoValues[Variables.CryptoInfo.cryptoSelected]+" (" +feeprice.ToString("N5")+ "$)";

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
