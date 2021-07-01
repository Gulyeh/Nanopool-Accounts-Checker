using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Windows;
using Newtonsoft.Json;

namespace MiningCheck
{
    class BlockChainData
    {
        public async static Task GetData(string coin)
        {
            await Task.Run(async () =>
            {
                try
                {
                    Variables.FinishedGettingBlockData = false;
                    switch (coin.ToLower())
                    {
                        case "eth":
                            await GetEthBlockData();
                            break;
                        case "etc":
                            await GetEtcBlockData();
                            break;
                        case "rvn":
                            await GetRvnBlockData();
                            break;
                        case "ergo":
                            await GetErgoBlockData();
                            break;
                        case "zec":
                            await GetZecBlockData();
                            break;
                        case "xmr":
                            Variables.datainfo = "XMR doesn't let this function";
                            Variables.FinishedGettingBlockData = true;
                            break;
                        case "cfx":
                            Variables.datainfo = "CFX doesn't let this function";
                            Variables.FinishedGettingBlockData = true;
                            break;
                        default:
                            break;
                    }                 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
        }
        private async static Task GetEthBlockData()
        {
            try
            {
                if (Variables.walletaddress != null)
                {
                    //Check if account exists in nanopool database
                    Variables.datainfo = "Checking Account";
                    string exists = await WebSocket.WebConnector("accountexist", Variables.cryptovalues[Variables.crypto]);
                    JObject status = JObject.Parse(exists);
                    Variables.found = Boolean.Parse(status["status"].ToString());
                    if (Boolean.Parse(status["status"].ToString()) == true && Variables.lastvalidwallet != String.Empty)
                    {
                        Variables.lastvalidwallet = Variables.walletaddress;
                        await Task.Delay(100);
                        Variables.datainfo = "Downloading Data";
                        //Get account data from blockchain
                        string account = await WebSocket.WebEthBlockchainConnector("account");
                        string transactions = await WebSocket.WebEthBlockchainConnector("transactions");

                        //Change float to real account balance
                        JObject balance = JObject.Parse(account);
                        float balancecalc = float.Parse(balance["result"].ToString());
                        Variables.BlockBalance = balancecalc / 1000000000000000000;

                        //Transactions info
                        await Task.Delay(100);
                        Variables.datainfo = "Downloading Transactions";
                        JObject trans = JObject.Parse(transactions);

                        for (int i = 0; i < trans["result"].Count(); i++)
                        {
                            //Calc Transfer Value to real amount
                            float transfervalue = float.Parse(trans["result"][i]["value"].ToString());
                            float finalvalue = transfervalue / 1000000000000000000;
                            Variables.TransferedValue[i] = finalvalue;

                            //Fill Other Variables
                            Variables.BlockNumber[i] = trans["result"][i]["blockNumber"].ToString();
                            var date = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(trans["result"][i]["timeStamp"].ToString())).DateTime;
                            Variables.TransactionDate[i] = date.ToString();
                            Variables.BlockHash[i] = trans["result"][i]["hash"].ToString();
                            Variables.FromWallet[i] = trans["result"][i]["from"].ToString();
                            Variables.toWallet[i] = trans["result"][i]["to"].ToString();
                            float GasUsed = float.Parse(trans["result"][i]["gasUsed"].ToString());
                            float GasPrice = float.Parse(trans["result"][i]["gasPrice"].ToString())/1000000000000000000;
                            Variables.FeeValue[i] =  GasUsed * GasPrice;
                            Variables.IsError[i] = Int32.Parse(trans["result"][i]["isError"].ToString());
                            await Task.Delay(50);
                        }
                        Variables.NumberTransactions = trans["result"].Count();
                        Variables.FinishedGettingBlockData = true;
                    }
                    else
                    {
                        Variables.ClearArrayDatas();
                    }
                }
                else
                {
                    Variables.ClearArrayDatas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private async static Task GetRvnBlockData()
        {
            try
            {
                if (Variables.walletaddress != null)
                {
                    //Check if account exists in nanopool database
                    int transiter = 0;
                    Variables.datainfo = "Checking Account";
                    string exists = await WebSocket.WebConnector("accountexist", Variables.cryptovalues[Variables.crypto]);
                    JObject status = JObject.Parse(exists);
                    Variables.found = Boolean.Parse(status["status"].ToString());
 
                    if (Boolean.Parse(status["status"].ToString()) == true && Variables.lastvalidwallet != String.Empty)
                    {
                        Variables.lastvalidwallet = Variables.walletaddress;
                        await Task.Delay(100);
                        Variables.datainfo = "Downloading Data";
                        //Get account data from blockchain
                        string account = await WebSocket.WebRvnBlockchainConnector("account", "");
                        JObject accdetails = JObject.Parse(account);
                        Variables.BlockBalance = float.Parse(accdetails["balance"].ToString());
                        await Task.Delay(100);
                        Variables.datainfo = "Downloading Transactions";
                        if (accdetails["transactions"].Count() < 20)
                        {
                            for (int i = 0; i < accdetails["transactions"].Count(); i++)
                            {
                                string transactions = await WebSocket.WebRvnBlockchainConnector("transactions", accdetails["transactions"][i].ToString());
                                JObject transinfo = JObject.Parse(transactions);

                                Variables.TransferedValue[i] = float.Parse(transinfo["valueIn"].ToString());
                                Variables.BlockNumber[i] = transinfo["blockheight"].ToString();
                                var date = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(transinfo["time"].ToString())).DateTime;
                                Variables.TransactionDate[i] = date.ToString();
                                Variables.BlockHash[i] = accdetails["transactions"][i].ToString();
                                Variables.FromWallet[i] = accdetails["addrStr"].ToString();
                                if (transinfo["vout"].Count() > 1)
                                {
                                    Variables.toWallet[i] = "Multiple Wallets";
                                }
                                else if (transinfo["vout"].Count() == 1)
                                {
                                    Variables.toWallet[i] = transinfo["vout"][0]["scriptPubKey"]["addresses"][0].ToString();
                                }
                                Variables.FeeValue[i] = float.Parse(transinfo["fees"].ToString());
                                if (Int32.Parse(transinfo["confirmations"].ToString()) > 0)
                                {
                                    Variables.IsError[i] = 0;
                                }
                                else
                                {
                                    Variables.IsError[i] = 1;
                                }
                                transiter = i + 1;
                                await Task.Delay(50);
                            }
                        }else
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                string transactions = await WebSocket.WebRvnBlockchainConnector("transactions", accdetails["transactions"][i].ToString());
                                JObject transinfo = JObject.Parse(transactions);

                                Variables.TransferedValue[i] = float.Parse(transinfo["valueIn"].ToString());
                                Variables.BlockNumber[i] = transinfo["blockheight"].ToString();
                                var date = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(transinfo["time"].ToString())).DateTime;
                                Variables.TransactionDate[i] = date.ToString();
                                Variables.BlockHash[i] = accdetails["transactions"][i].ToString();
                                Variables.FromWallet[i] = accdetails["addrStr"].ToString();
                                if (transinfo["vout"].Count() > 1)
                                {
                                    Variables.toWallet[i] = "Multiple Wallets";
                                }
                                else if(transinfo["vout"].Count() == 1)
                                {
                                    Variables.toWallet[i] = transinfo["vout"][0]["scriptPubKey"]["addresses"][0].ToString();
                                }
                                Variables.FeeValue[i] = float.Parse(transinfo["fees"].ToString());
                                if (Int32.Parse(transinfo["confirmations"].ToString()) > 0)
                                {
                                    Variables.IsError[i] = 0;
                                }
                                else
                                {
                                    Variables.IsError[i] = 1;
                                }
                                await Task.Delay(50);
                            }
                            transiter = 20;
                        }
                        Variables.NumberTransactions = transiter;
                        Variables.FinishedGettingBlockData = true;
                    }
                    else
                    {
                        Variables.ClearArrayDatas();
                    }
                }
                else
                {
                    Variables.ClearArrayDatas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }         
        private async static Task GetEtcBlockData()
        {
            try
            {
                if (Variables.walletaddress != String.Empty)
                {
                    Variables.datainfo = "Checking Account";
                    string exists = await WebSocket.WebConnector("accountexist", Variables.cryptovalues[Variables.crypto]);
                    JObject status = JObject.Parse(exists);
                    Variables.found = Boolean.Parse(status["status"].ToString());

                    if (Boolean.Parse(status["status"].ToString()) == true && Variables.lastvalidwallet != String.Empty)
                    {
                        Variables.lastvalidwallet = Variables.walletaddress;
                        await Task.Delay(100);
                        Variables.datainfo = "Downloading Data";
                        string account = await WebSocket.WebEtcBlockchainConnector("account");
                        string transactions = await WebSocket.WebEtcBlockchainConnector("transactions");
                        JObject balance = JObject.Parse(account);
                        JObject tx = JObject.Parse(transactions);

                        float balancecalc = float.Parse(balance["result"].ToString());
                        Variables.BlockBalance = balancecalc / 1000000000000000000;

                        await Task.Delay(100);
                        Variables.datainfo = "Downloading Transactions";
                
                        for (int i = 0; i < tx["result"].Count(); i++)
                        {
                            //Calc Transfer Value to real amount
                            float transfervalue = float.Parse(tx["result"][i]["value"].ToString());
                            float finalvalue = transfervalue / 1000000000000000000;
                            Variables.TransferedValue[i] = finalvalue;

                            //Fill Other Variables
                            Variables.BlockNumber[i] = tx["result"][i]["blockNumber"].ToString();
                            var date = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(tx["result"][i]["timeStamp"].ToString())).DateTime;
                            Variables.TransactionDate[i] = date.ToString();
                            Variables.BlockHash[i] = tx["result"][i]["hash"].ToString();
                            Variables.FromWallet[i] = tx["result"][i]["from"].ToString();
                            Variables.toWallet[i] = tx["result"][i]["to"].ToString();
                            float GasUsed = float.Parse(tx["result"][i]["gasUsed"].ToString());
                            float GasPrice = float.Parse(tx["result"][i]["gasPrice"].ToString()) / 1000000000000000000;
                            Variables.FeeValue[i] = GasUsed * GasPrice;
                            Variables.IsError[i] = Int32.Parse(tx["result"][i]["isError"].ToString());

                            await Task.Delay(50);
                        }
                        Variables.NumberTransactions = tx["result"].Count();
                        Variables.FinishedGettingBlockData = true;
                    }
                    else
                    {
                        Variables.ClearArrayDatas();
                    }
                }
                else
                {
                    Variables.ClearArrayDatas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private async static Task GetZecBlockData()
        {
            try
            {
                if (Variables.walletaddress != null)
                {
                    Variables.datainfo = "Checking Account";
                    string exists = await WebSocket.WebConnector("accountexist", Variables.cryptovalues[Variables.crypto]);
                    JObject status = JObject.Parse(exists);
                    Variables.found = Boolean.Parse(status["status"].ToString());

                    if (Boolean.Parse(status["status"].ToString()) == true && Variables.lastvalidwallet != String.Empty)
                    {
                        Variables.lastvalidwallet = Variables.walletaddress;
                        await Task.Delay(100);
                        Variables.datainfo = "Downloading Data";
                        string account = await WebSocket.WebZecBlockchainConnector("account");
                        await Task.Delay(100);
                        Variables.datainfo = "Downloading Transactions";
                        string transsent = await WebSocket.WebZecBlockchainConnector("transaction2");
                        string transrcvd = await WebSocket.WebZecBlockchainConnector("transaction1");

                        JObject accdetails = JObject.Parse(account);
                        JArray sent = (JArray)JsonConvert.DeserializeObject(transsent);
                        JArray rcvd = (JArray)JsonConvert.DeserializeObject(transrcvd);

                        Variables.BlockBalance = float.Parse(accdetails["balance"].ToString());
                        
                        int transnumber = sent.Count() + rcvd.Count();

                        for(int i=0;i<sent.Count(); i++)
                        {
                            Variables.TransferedValue[i] = float.Parse(sent[i]["value"].ToString());

                            Variables.BlockNumber[i] = sent[i]["blockHeight"].ToString();
                            var date = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(sent[i]["timestamp"].ToString())).DateTime;
                            Variables.TransactionDate[i] = date.ToString();
                            Variables.BlockHash[i] = sent[i]["hash"].ToString();

                            if (sent[i]["vin"].Count() > 1)
                            {
                                Variables.toWallet[i] = "Multiple Wallets";
                            }
                            else if (sent[i]["vin"].Count() == 1)
                            {
                                Variables.toWallet[i] = sent[i]["vin"][i]["retrievedVout"]["scriptPubKey"]["addresses"][0].ToString();
                            }

                            Variables.FromWallet[i] = sent[i]["vout"][0]["scriptPubKey"]["addresses"][0].ToString();
                            Variables.FeeValue[i] = float.Parse(sent[i]["fee"].ToString());
                            Variables.IsError[i] = 0;
                            await Task.Delay(10);
                        }

                        for (int i = sent.Count(); i < transnumber; i++)
                        {
                            int j = i - sent.Count();

                            Variables.TransferedValue[i] = float.Parse(rcvd[j]["value"].ToString());

                            Variables.BlockNumber[i] = rcvd[j]["blockHeight"].ToString();
                            var date = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(rcvd[j]["timestamp"].ToString())).DateTime;
                            Variables.TransactionDate[i] = date.ToString();
                            Variables.BlockHash[i] = rcvd[j]["hash"].ToString();

                            if (rcvd[j]["vout"].Count() > 1)
                            {
                                Variables.FromWallet[i] = "Multiple Wallets";
                            }
                            else if (rcvd[j]["vout"].Count() == 1)
                            {
                                Variables.FromWallet[i] = rcvd[j]["vout"][0]["scriptPubKey"]["addresses"][0].ToString();
                            }

                            Variables.toWallet[i] = accdetails["address"].ToString();
                            Variables.FeeValue[i] = float.Parse(rcvd[j]["fee"].ToString());
                            Variables.IsError[i] = 0;
                            await Task.Delay(10);
                        }

                        Variables.NumberTransactions = transnumber;
                        Variables.FinishedGettingBlockData = true;                     
                    }
                    else
                    {
                        Variables.ClearArrayDatas();
                    }
                }
                else
                {
                    Variables.ClearArrayDatas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private async static Task GetErgoBlockData()
        {
            try
            {
                if (Variables.walletaddress != null)
                {
                    Variables.datainfo = "Checking Account";
                    string exists = await WebSocket.WebConnector("accountexist", Variables.cryptovalues[Variables.crypto]);
                    JObject status = JObject.Parse(exists);
                    Variables.found = Boolean.Parse(status["status"].ToString());

                    if (Boolean.Parse(status["status"].ToString()) == true && Variables.lastvalidwallet != String.Empty)
                    {
                        Variables.lastvalidwallet = Variables.walletaddress;
                        await Task.Delay(100);
                        Variables.datainfo = "Downloading Data";
                        string account = await WebSocket.WebErgoBlockchainConnector("account");
                        await Task.Delay(100);
                        Variables.datainfo = "Downloading Transactions";
                        string transactions = await WebSocket.WebErgoBlockchainConnector("transactions");

                        JObject acc = JObject.Parse(account);
                        JObject trans = JObject.Parse(transactions);
                        Variables.NumberTransactions = trans["items"].Count();
                        float calc = float.Parse(acc["nanoErgs"].ToString()) / 1000000000;
                        Variables.BlockBalance = calc;

                        for (int i = 0; i < trans["items"].Count(); i++)
                        {
                            Variables.BlockHash[i] = trans["items"][i]["id"].ToString();
                            calc = float.Parse(trans["items"][i]["outputs"][0]["value"].ToString()) / 1000000000;
                            Variables.TransferedValue[i] = calc;
                            var date = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(trans["items"][i]["timestamp"].ToString().Substring(0,trans["items"][i]["timestamp"].ToString().Length - 3))).DateTime;
                            Variables.TransactionDate[i] = date.ToString();
                            Variables.BlockNumber[i] = trans["items"][i]["inclusionHeight"].ToString();

                            if (Int32.Parse(trans["items"][i]["numConfirmations"].ToString()) > 0)
                            {
                                Variables.IsError[i] = 0;
                            }
                            else
                            {
                                Variables.IsError[i] = 1;
                            }

                            if (trans["items"][i]["inputs"].Count() > 1)
                            {
                                Variables.FromWallet[i] = "Multiple Wallets";
                            }
                            else
                            {
                                Variables.FromWallet[i] = trans["items"][i]["inputs"][0]["address"].ToString();
                            }


                            if (trans["items"][i]["outputs"].Count() > 1)
                            {
                                Variables.toWallet[i] = "Multiple Wallets";
                            }
                            else
                            {
                                Variables.toWallet[i] = trans["items"][i]["outputs"][0]["address"].ToString();
                            }

                            Variables.FeeValue[i] = 0;
                            await Task.Delay(50);
                        }

                        Variables.FinishedGettingBlockData = true;
                    }
                    else
                    {
                        Variables.ClearArrayDatas();
                    }
                }
                else
                {
                    Variables.ClearArrayDatas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
       
    }
}
