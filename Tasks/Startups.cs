using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Threading;

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
                Variables.ConfigExisted = true;
                if (data == "Save")
                {
                    new XDocument(
                                new XElement("MainSettings",
                                new XElement("Wallet", Variables.walletaddress),
                                new XElement("FiatCurrency", Variables.fiat),
                                new XElement("CryptoCurrency", Variables.crypto)
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
                        Variables.walletaddress = node.SelectSingleNode("Wallet").InnerText;
                        Variables.fiat = Int32.Parse(node.SelectSingleNode("FiatCurrency").InnerText);
                        Variables.crypto = Int32.Parse(node.SelectSingleNode("CryptoCurrency").InnerText);
                    }
                }
            }
            else
            {
                Variables.ConfigExisted = false;
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
                if (Variables.walletaddress != null)
                {
                    //Check if account exists
                    Variables.datamaininfo = "Checking account";
                    string exists = await WebSocket.WebConnector("accountexist", Variables.cryptovalues[Variables.crypto]);
                    JObject status = JObject.Parse(exists);
                    Variables.found = Boolean.Parse(status["status"].ToString());
                    if (status["status"].ToString().ToLower() == "true")
                    {
                        Variables.datamaininfo = "Downloading Data";
                        //Put Valid Address to memory
                        Variables.lastvalidwallet = Variables.walletaddress;
                        await Task.Delay(100);
                        //Get Account Infos
                        string generalinfo = await WebSocket.WebConnector("user", Variables.cryptovalues[Variables.crypto]);
                        string reportedhashrate = await WebSocket.WebConnector("hashratechart", Variables.cryptovalues[Variables.crypto]);
                        string workerslastreportedhash = await WebSocket.WebConnector("reportedhashrates", Variables.cryptovalues[Variables.crypto]);
                        string coinsprice = await WebSocket.WebCoinPricesConnector(Variables.cryptovalues[Variables.crypto]);

                        //Parse Account Infos
                        JObject jsongeneral = JObject.Parse(generalinfo);
                        JObject jsonreportedhash = JObject.Parse(reportedhashrate);
                        JObject jsonworkersreportedhash = JObject.Parse(workerslastreportedhash);
                        JObject jsoncoinprice = JObject.Parse(coinsprice);

                        //Check if account exists again just for same and bugs prevention
                        exists = await WebSocket.WebConnector("accountexist", Variables.cryptovalues[Variables.crypto]);
                        status = JObject.Parse(exists);

                        if (status["status"].ToString().ToLower() == "true")
                        {
                            Variables.datamaininfo = "Processing Main Informations";
                            await Task.Delay(100);
                            //Get Variables and Set them
                            Variables.MainBalance = jsongeneral["data"]["balance"].ToString();
                            Variables.MainCurrentHash = jsongeneral["data"]["hashrate"].ToString();
                            Variables.rigs = Int32.Parse(jsongeneral["data"]["workers"].Count().ToString());

                            //Check if coin has reported hash or average
                            if (Variables.crypto == 0)
                            {
                                float CalculateHash = float.Parse(jsonreportedhash["data"][0]["hashrate"].ToString()) / 1000;
                                Variables.MainReportedHash = Math.Round(CalculateHash, 1).ToString().Replace(",", ".");
                            }
                            else
                            {
                                Variables.MainReportedHash = jsongeneral["data"]["avgHashrate"]["h6"].ToString().Replace(",", ".");
                            }

                            Variables.datamaininfo = "Processing Earnings";
                            await Task.Delay(100);
                            //Get Earnings Info and Declare
                            string mainearnings = await WebSocket.WebRevenueConnector("approximated_earnings", Variables.cryptovalues[Variables.crypto], Variables.MainReportedHash);
                            JObject jsonmainearnings = JObject.Parse(mainearnings);

                            if (Boolean.Parse(jsonmainearnings["status"].ToString()))
                            {
                                Variables.MainCryptoDaily = Math.Round(double.Parse(jsonmainearnings["data"]["day"]["coins"].ToString()), 3).ToString();
                                Variables.MainCryptoWeekly = Math.Round(double.Parse(jsonmainearnings["data"]["week"]["coins"].ToString()), 3).ToString();
                                Variables.MainCryptoMonthly = Math.Round(double.Parse(jsonmainearnings["data"]["month"]["coins"].ToString()), 3).ToString();
                                Variables.MainFiatDaily = jsonmainearnings["data"]["day"][GetFullCurrency(Variables.fiatvalues[Variables.fiat])].ToString().Split(',')[0];
                                Variables.MainFiatWeekly = jsonmainearnings["data"]["week"][GetFullCurrency(Variables.fiatvalues[Variables.fiat])].ToString().Split(',')[0];
                                Variables.MainFiatMonthly = jsonmainearnings["data"]["month"][GetFullCurrency(Variables.fiatvalues[Variables.fiat])].ToString().Split(',')[0];
                            }
                            else
                            {
                                Variables.MainCryptoDaily = String.Empty;
                                Variables.MainCryptoWeekly = String.Empty;
                                Variables.MainCryptoMonthly = String.Empty;
                                Variables.MainFiatDaily = String.Empty;
                                Variables.MainFiatWeekly = String.Empty;
                                Variables.MainFiatMonthly = String.Empty;
                            }

                            Variables.datamaininfo = "Processing Rigs Data";
                            await Task.Delay(100);

                            //Set Coin Values
                            for (int i = 0; i < Variables.fiatvalues.Length; i++)
                            {
                                Variables.CoinPrices[i] = jsoncoinprice["data"]["price_" + Variables.fiatvalues[i].ToLower()].ToString();
                            }

                            for (int i = 0; i < Variables.rigs; i++)
                            {
                                //Get Miner Name
                                Variables.rigname[i] = jsongeneral["data"]["workers"][i]["id"].ToString();
                                Variables.currhash[i] = jsongeneral["data"]["workers"][i]["hashrate"].ToString();
                                Variables.sharesnum[i] = jsongeneral["data"]["workers"][i]["rating"].ToString();

                                for (int j = 0; j < Variables.rigs; j++)
                                {
                                    if (Variables.rigname[i] == jsonworkersreportedhash["data"][j]["worker"].ToString())
                                    {
                                        Variables.rephash[i] = jsonworkersreportedhash["data"][j]["hashrate"].ToString().Replace(",", ".");
                                    }
                                }

                                //Check Miner Status
                                if ((jsongeneral["data"]["workers"][i]["hashrate"].ToString() != "0.0") && (jsongeneral["data"]["workers"][i]["hashrate"].ToString() != "0"))
                                {
                                    Variables.status[i] = 1;
                                }
                                else
                                {
                                    Variables.status[i] = 0;
                                }
                                await Task.Delay(10);
                            }

                        }

                    }
                    else
                    {
                        //Zero account infos
                        Variables.lastvalidwallet = String.Empty;
                        Variables.rigs = 0;
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
