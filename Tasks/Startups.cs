using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace MiningCheck
{
    public class Startups
    {
        public static void CheckConfig(string data)
        {
            string windows = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fullpath = windows + "/minersettings.xml";
            if (File.Exists(fullpath))
            {
                Variables.Checkers.ConfigExisted = true;
                if (data == "Save")
                {
                    new XDocument(
                                new XElement("MainSettings",
                                new XElement("Wallet", Variables.Wallet.walletAddress),
                                new XElement("FiatCurrency", Variables.FiatInfo.fiatSelected),
                                new XElement("CryptoCurrency", Variables.CryptoInfo.cryptoSelected)
                            )
                        )
                        .Save(fullpath);
                }
                else if (data == "Load")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fullpath);
                    XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/MainSettings");
                    foreach (XmlNode node in nodeList)
                    {
                        Variables.Wallet.walletAddress = node.SelectSingleNode("Wallet").InnerText;
                        Variables.FiatInfo.fiatSelected = Int32.Parse(node.SelectSingleNode("FiatCurrency").InnerText);
                        Variables.CryptoInfo.cryptoSelected = Int32.Parse(node.SelectSingleNode("CryptoCurrency").InnerText);
                    }
                }
            }
            else
            {
                Variables.Checkers.ConfigExisted = false;
                new XDocument(
                    new XElement("MainSettings",
                        new XElement("Wallet", ""),
                        new XElement("FiatCurrency", ""),
                        new XElement("CryptoCurrency", "")
                    )
                )
                .Save(fullpath);
            }
        }
        public async static Task FillJsonVariables()
        {
            try
            {
                if (Variables.Wallet.walletAddress != null)
                {
                    //Check if account exists
                    Variables.LoadingWindow.mainDataInfo = "Checking account";
                    string exists = await WebSocket.WebConnector("accountexist", Variables.CryptoInfo.cryptoValues[Variables.CryptoInfo.cryptoSelected]);
                    JObject status = JObject.Parse(exists);
                    Variables.Checkers.found = Boolean.Parse(status["status"].ToString());
                    if (status["status"].ToString().ToLower() == "true")
                    {
                        Variables.LoadingWindow.mainDataInfo = "Downloading Data";
                        //Put Valid Address to memory
                        Variables.Wallet.lastValidWallet = Variables.Wallet.walletAddress;
                        await Task.Delay(100);
                        //Get Account Infos
                        string generalinfo = await WebSocket.WebConnector("user", Variables.CryptoInfo.cryptoValues[Variables.CryptoInfo.cryptoSelected]);
                        string reportedhashrate = await WebSocket.WebConnector("hashratechart", Variables.CryptoInfo.cryptoValues[Variables.CryptoInfo.cryptoSelected]);
                        string workerslastreportedhash = await WebSocket.WebConnector("reportedhashrates", Variables.CryptoInfo.cryptoValues[Variables.CryptoInfo.cryptoSelected]);
                        string coinsprice = await WebSocket.WebCoinPricesConnector(Variables.CryptoInfo.cryptoValues[Variables.CryptoInfo.cryptoSelected]);

                        //Parse Account Infos
                        JObject jsongeneral = JObject.Parse(generalinfo);
                        JObject jsonreportedhash = JObject.Parse(reportedhashrate);
                        JObject jsonworkersreportedhash = JObject.Parse(workerslastreportedhash);
                        JObject jsoncoinprice = JObject.Parse(coinsprice);

                        //Check if account exists again just for same and bugs prevention
                        exists = await WebSocket.WebConnector("accountexist", Variables.CryptoInfo.cryptoValues[Variables.CryptoInfo.cryptoSelected]);
                        status = JObject.Parse(exists);

                        if (status["status"].ToString().ToLower() == "true")
                        {
                            Variables.LoadingWindow.mainDataInfo = "Processing Main Informations";
                            await Task.Delay(100);
                            //Get Variables and Set them
                            Variables.MinerData.MainBalance = jsongeneral["data"]["balance"].ToString();
                            Variables.MinerData.MainCurrentHash = jsongeneral["data"]["hashrate"].ToString();
                            Variables.RigInfo.rigsAmount = Int32.Parse(jsongeneral["data"]["workers"].Count().ToString());

                            //Check if coin has reported hash or average
                            if (Variables.CryptoInfo.cryptoSelected == 0)
                            {
                                float CalculateHash = float.Parse(jsonreportedhash["data"][0]["hashrate"].ToString()) / 1000;
                                Variables.MinerData.MainReportedHash = Math.Round(CalculateHash, 1).ToString().Replace(",", ".");
                            }
                            else
                            {
                                Variables.MinerData.MainReportedHash = jsongeneral["data"]["avgHashrate"]["h6"].ToString().Replace(",", ".");
                            }

                            Variables.LoadingWindow.mainDataInfo = "Processing Earnings";
                            await Task.Delay(100);
                            //Get Earnings Info and Declare
                            string mainearnings = await WebSocket.WebRevenueConnector("approximated_earnings", Variables.CryptoInfo.cryptoValues[Variables.CryptoInfo.cryptoSelected], Variables.MinerData.MainReportedHash);
                            JObject jsonmainearnings = JObject.Parse(mainearnings);

                            if (Boolean.Parse(jsonmainearnings["status"].ToString()))
                            {
                                Variables.RigRevenue.MainCryptoDaily = Math.Round(double.Parse(jsonmainearnings["data"]["day"]["coins"].ToString()), 3).ToString();
                                Variables.RigRevenue.MainCryptoWeekly = Math.Round(double.Parse(jsonmainearnings["data"]["week"]["coins"].ToString()), 3).ToString();
                                Variables.RigRevenue.MainCryptoMonthly = Math.Round(double.Parse(jsonmainearnings["data"]["month"]["coins"].ToString()), 3).ToString();
                                Variables.RigRevenue.MainFiatDaily = jsonmainearnings["data"]["day"][GetFullCurrency(Variables.FiatInfo.fiatValues[Variables.FiatInfo.fiatSelected])].ToString().Split(',')[0];
                                Variables.RigRevenue.MainFiatWeekly = jsonmainearnings["data"]["week"][GetFullCurrency(Variables.FiatInfo.fiatValues[Variables.FiatInfo.fiatSelected])].ToString().Split(',')[0];
                                Variables.RigRevenue.MainFiatMonthly = jsonmainearnings["data"]["month"][GetFullCurrency(Variables.FiatInfo.fiatValues[Variables.FiatInfo.fiatSelected])].ToString().Split(',')[0];
                            }
                            else
                            {
                                Variables.RigRevenue.ResetRevenue();
                            }

                            Variables.LoadingWindow.mainDataInfo = "Processing Rigs Data";
                            await Task.Delay(100);

                            //Set Coin Values
                            for (int i = 0; i < Variables.FiatInfo.fiatValues.Length; i++)
                            {
                                Variables.Price.CoinPrices[i] = jsoncoinprice["data"]["price_" + Variables.FiatInfo.fiatValues[i].ToLower()].ToString();
                            }

                            for (int i = 0; i < Variables.RigInfo.rigsAmount; i++)
                            {
                                //Get Miner Name
                                Variables.RigInfo.rigName[i] = jsongeneral["data"]["workers"][i]["id"].ToString();
                                Variables.RigInfo.currHash[i] = jsongeneral["data"]["workers"][i]["hashrate"].ToString();
                                Variables.RigInfo.sharesNum[i] = jsongeneral["data"]["workers"][i]["rating"].ToString();

                                for (int j = 0; j < Variables.RigInfo.rigsAmount; j++)
                                {
                                    if (Variables.RigInfo.rigName[i] == jsonworkersreportedhash["data"][j]["worker"].ToString())
                                    {
                                        Variables.RigInfo.repHash[i] = jsonworkersreportedhash["data"][j]["hashrate"].ToString().Replace(",", ".");
                                    }
                                }

                                //Check Miner Status
                                if ((jsongeneral["data"]["workers"][i]["hashrate"].ToString() != "0.0") && (jsongeneral["data"]["workers"][i]["hashrate"].ToString() != "0"))
                                {
                                    Variables.RigInfo.status[i] = 1;
                                }
                                else
                                {
                                    Variables.RigInfo.status[i] = 0;
                                }
                                await Task.Delay(10);
                            }

                        }

                    }
                    else
                    {
                        //Zero account infos
                        Variables.Wallet.lastValidWallet = String.Empty;
                        Variables.RigInfo.rigsAmount = 0;
                    }
                }

            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public static string GetFullCurrency(string fiat)
        {
            switch (fiat)
            {
                case "USD":
                    return "dollars";

                case "EUR":
                    return "euros";

                case "GBP":
                    return "pounds";

                case "RUR":
                    return "rubles";

                case "CNY":
                    return "yuan";

                default:
                    return null;
            }
        }
    }
}
