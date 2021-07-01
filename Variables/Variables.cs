
using System;

namespace MiningCheck
{
    public class Variables
    {
        //Variables for Miners Info
        public static string[] rigname = new string[1000];
        public static string[] rephash = new string[1000];
        public static string[] currhash = new string[1000];
        public static string[] sharesnum = new string[1000];
        public static int[] status = new int[1000];
        public static string[] CoinPrices = new string[5];
        public static bool[] IsOnline = new bool[1000];

        //General Variables
        public static string[] fiatvalues = { "USD", "EUR", "GBP", "RUR", "CNY" };
        public static string[] cryptovalues = { "ETH", "RVN", "ERGO", "ETC", "ZEC", "XMR", "CFX" };
        public static int rigs { get; set; }
        public static string walletaddress { get; set; }
        public static string lastvalidwallet = String.Empty;
        public static int crypto { get; set; }
        public static int fiat { get; set; }
        public static string MainCurrentHash { get; set; }
        public static string MainReportedHash { get; set; }
        public static string MainBalance { get; set; }
        public static bool ConfigExisted { get; set; }
        public static bool found = false;
        public static bool CheckingCryptoCoin = true;

        //Loop Bools
        public static bool FinishedVariables = false;
        public static bool FinishedVariablesChain = false;
        public static bool FinishedGettingBlockData = false;
        public static bool ChainPageSelected = false;

        //Main HomePage Revenues
        public static string MainCryptoDaily { get; set; }
        public static string MainCryptoWeekly { get; set; }
        public static string MainCryptoMonthly { get; set; }
        public static string MainFiatDaily { get; set; }
        public static string MainFiatWeekly { get; set; }
        public static string MainFiatMonthly { get; set; }

        //BlockChain Variables
        public static int NumberTransactions = 0;
        public static float BlockBalance = 0;
        public static float[] TransferedValue = new float[20];
        public static string[] BlockNumber = new string[20];
        public static string[] TransactionDate = new string[20];
        public static string[] BlockHash = new string[20];
        public static string[] FromWallet = new string[20];
        public static string[] toWallet = new string[20];
        public static float[] FeeValue = new float[20];
        public static int[] IsError = new int[20];

        //Loading Info
        public static string datainfo { get; set; }
        public static string datamaininfo { get; set; }

        public static void ClearArrayDatas()
        {
            Variables.BlockBalance = 0;
            Variables.NumberTransactions = 0;
            Array.Clear(Variables.BlockNumber, 0, Variables.BlockNumber.Length);
            Array.Clear(Variables.TransactionDate, 0, Variables.TransactionDate.Length);
            Array.Clear(Variables.BlockHash, 0, Variables.BlockHash.Length);
            Array.Clear(Variables.FromWallet, 0, Variables.FromWallet.Length);
            Array.Clear(Variables.toWallet, 0, Variables.toWallet.Length);
            Array.Clear(Variables.TransferedValue, 0, Variables.TransferedValue.Length);
            Array.Clear(Variables.FeeValue, 0, Variables.FeeValue.Length);
            Array.Clear(Variables.IsError, 0, Variables.IsError.Length);
            Array.Clear(Variables.IsOnline, 0, Variables.IsOnline.Length);
        }

        public static void ResetVars()
        {
            Variables.found = false;
            Variables.lastvalidwallet = String.Empty;
            Variables.NumberTransactions = 0;
            Variables.FinishedGettingBlockData = false;
            Variables.FinishedVariables = false;
            Variables.FinishedVariablesChain = false;
            Variables.datainfo = String.Empty;
            Variables.datamaininfo = String.Empty;
        }

    }
}
