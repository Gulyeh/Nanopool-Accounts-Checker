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
using System.Windows.Shapes;

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
                if(Variables.IsError[index] == 0)
                {
                    successicon.Visibility = Visibility.Visible;
                }
                else
                {
                    failedicon.Visibility = Visibility.Visible;
                }

                TransHash.Text = Variables.BlockHash[index];
                fromwallet.Text = Variables.FromWallet[index];
                towallet.Text = Variables.toWallet[index];
                DateTime today = DateTime.Now;
                DateTime transdate = Convert.ToDateTime(Variables.TransactionDate[index]);
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

                timestamp.Text = daystimepassed+" "+hourstimepassed+ " " +minutestimepassed+" ("+Variables.TransactionDate[index]+")";
                float transferedprice = Variables.TransferedValue[index] * float.Parse(Variables.CoinPrices[Variables.fiat]);
                transfervalue.Text = Variables.TransferedValue[index].ToString() + " " + Variables.cryptovalues[Variables.crypto] + " ("+transferedprice.ToString("N2")+"$)";
                float feeprice = Variables.FeeValue[index] * float.Parse(Variables.CoinPrices[Variables.fiat]);
                fee.Text = Variables.FeeValue[index].ToString() + " " + Variables.cryptovalues[Variables.crypto]+" (" +feeprice.ToString("N5")+ "$)";

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
