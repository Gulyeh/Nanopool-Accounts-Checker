using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MiningCheck
{
    public class WebSocket
    {
        public static Task<string> WebConnector(string type, string coin)
        {
            try
            {
                string url = "https://api.nanopool.org/v1/" + coin.ToLower() + "/" + type + "/" + Variables.walletaddress;
                var webClient = new WebClient { };
                var response = webClient.DownloadString(url);             
                return Task.Run(() => response);
            }catch(System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
                return Task.Run(() => string.Empty);
            }
        }

        public static Task<string> WebRevenueConnector(string type, string coin, string hash)
        {
            try
            {
                string url = "https://api.nanopool.org/v1/" + coin.ToLower() + "/" + type + "/" + hash;
                var webClient = new WebClient { };
                var response = webClient.DownloadString(url);            
                return Task.Run(() => response);
            }
            catch(System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
                return Task.Run(() => string.Empty);
            }
        }

        public static Task<string> WebCoinPricesConnector(string coin)
        {
            try
            {

                string url = "https://api.nanopool.org/v1/" + coin.ToLower() + "/prices";
                var webClient = new WebClient { };
                var response = webClient.DownloadString(url);
                
                return Task.Run(() => response);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
                return Task.Run(() => string.Empty);
            }
        }

        public static Task<string> WebEthBlockchainConnector(string type)
        {
            try
            {
                var url = string.Empty;
                switch(type)
                {
                    case "account":
                        url = "https://api.etherscan.io/api?module=account&action=balance&address=" + Variables.lastvalidwallet + "&tag=latest&apikey=XTGQY8TVBJBM4D2IH6X21CCRC8WF4BJX1J";
                        break;
                    case "transactions":
                        url = "https://api.etherscan.io/api?module=account&action=txlist&address="+ Variables.lastvalidwallet + "&startblock=0&endblock=99999999&page=1&offset=20&sort=desc&apikey=XTGQY8TVBJBM4D2IH6X21CCRC8WF4BJX1J";
                        break;
                    default:
                        return Task.Run(() => string.Empty);
                }
                var webClient = new WebClient { };
                var response = webClient.DownloadString(url);
                
                return Task.Run(() => response);
            }
            catch(System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
                return Task.Run(() => string.Empty);
            }
        }
      
        public static Task<string> WebRvnBlockchainConnector(string type, string tx)
        {
            try
            {
                var url = string.Empty;
                switch (type)
                {
                    case "account":
                        url = "https://ravencoin.network/api/addr/" + Variables.lastvalidwallet;
                        break;
                    case "transactions":
                        url = "https://ravencoin.network/api/tx/" + tx;
                        break;
                    default:
                        return Task.Run(() => string.Empty);
                }
                var webClient = new WebClient { };
                var response = webClient.DownloadString(url);
                
                return Task.Run(() => response);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
                return Task.Run(() => string.Empty);
            }
        }

        public static Task<string> WebEtcBlockchainConnector(string type)
        {
            try
            {
                var url = string.Empty;
                switch (type)
                {
                    case "account":
                        url = "https://blockscout.com/etc/mainnet/api?module=account&action=balance&address=" + Variables.lastvalidwallet;
                        break;
                    case "transactions":
                        url = "https://blockscout.com/etc/mainnet/api?module=account&action=txlist&address=" + Variables.lastvalidwallet + "&offset=20";
                        break;
                    default:
                        return Task.Run(() => string.Empty);
                }
                var webClient = new WebClient { };
                var response = webClient.DownloadString(url);
                return Task.Run(() => response);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
                return Task.Run(() => string.Empty);
            }
        }

        public static Task<string> WebZecBlockchainConnector(string type)
        {
            try
            {
                var url = string.Empty;
                switch (type)
                {
                    case "account":
                        url = "https://api.zcha.in/v2/mainnet/accounts/" + Variables.lastvalidwallet;
                        break;
                    case "transaction1":
                        url = "https://api.zcha.in/v2/mainnet/accounts/" + Variables.lastvalidwallet + "/recv?limit=10&offset=0&sort=blockHeight&direction=descending";
                        break;
                    case "transaction2":
                        url = "https://api.zcha.in/v2/mainnet/accounts/" + Variables.lastvalidwallet + "/sent?limit=10&offset=0&sort=blockHeight&direction=descending";
                        break;
                    default:
                        return Task.Run(() => string.Empty);
                }
                var webClient = new WebClient { };
                var response = webClient.DownloadString(url);
                return Task.Run(() => response);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
                return Task.Run(() => string.Empty);
            }
        }

        public static Task<string> WebErgoBlockchainConnector(string type)
        {
            try
            {
                var url = string.Empty;
                switch (type)
                {
                    case "account":
                        url = "https://api.ergoplatform.com/api/v1/addresses/" + Variables.lastvalidwallet + "/balance/confirmed";
                        break;
                    case "transactions":
                        url = "https://api.ergoplatform.com/api/v1/addresses/" + Variables.lastvalidwallet + "/transactions?limit=20";
                        break;
                    default:
                        url = string.Empty;
                        return null;
                }
                var webClient = new WebClient { };
                var response = webClient.DownloadString(url);
                return Task.Run(() => response);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
                return Task.Run(() => string.Empty);
            }
        }
 
    }
}
