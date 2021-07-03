using System;

namespace MiningCheck
{
    class Variables
    {
        public class RigInfo
        {
            public static string[] rigName = new string[10000];
            public static string[] repHash = new string[10000];
            public static string[] currHash = new string[10000];
            public static string[] sharesNum = new string[10000];
            public static int[] status = new int[10000];
            public static bool[] IsOnline = new bool[10000];
            public static int rigsAmount { get; set; }

            public static void ClearRigInfo()
            {
                rigsAmount = 0;
                Array.Clear(IsOnline, 0, IsOnline.Length);
                Array.Clear(rigName, 0, rigName.Length);
                Array.Clear(repHash, 0, repHash.Length);
                Array.Clear(currHash, 0, currHash.Length);
                Array.Clear(sharesNum, 0, sharesNum.Length);
                Array.Clear(status, 0, status.Length);
            }
        }

        public class Price
        {
            public static string[] CoinPrices = new string[5];
        }

        public class FiatInfo
        {
            public static int fiatSelected { get; set; }
            public static string[] fiatValues = { "USD", "EUR", "GBP", "RUR", "CNY" };
        }

        public class CryptoInfo
        {
            public static int cryptoSelected { get; set; }
            public static string[] cryptoValues = { "ETH", "RVN", "ERGO", "ETC", "ZEC", "XMR", "CFX" };
        }

        public class Wallet
        {
            public static string walletAddress { get; set; }
            public static string lastValidWallet = String.Empty;
        }

        public class MinerData
        {
            public static string MainCurrentHash { get; set; }
            public static string MainReportedHash { get; set; }
            public static string MainBalance { get; set; }

            public static void ClearMinerData()
            {
                MainCurrentHash = String.Empty;
                MainReportedHash = String.Empty;
                MainBalance = String.Empty;
            }
        }

        public class Checkers
        {
            public static bool FinishedVariables = false;
            public static bool FinishedVariablesChain = true;
            public static bool FinishedGettingBlockData = false;
            public static bool ChainPageSelected = false;
            public static bool found = false;
            public static bool CheckingCryptoCoin = true;
            public static bool ConfigExisted { get; set; }

            public static void ClearCheckers()
            {
                FinishedVariables = false;
                //Switching to false while executing function GetData() - made to true to let it go in statement
                FinishedVariablesChain = true;
                FinishedGettingBlockData = false;
                ChainPageSelected = false;
                found = false;
                CheckingCryptoCoin = true;
            }
        }
        
        public class RigRevenue
        {
            public static string MainCryptoDaily { get; set; }
            public static string MainCryptoWeekly { get; set; }
            public static string MainCryptoMonthly { get; set; }
            public static string MainFiatDaily { get; set; }
            public static string MainFiatWeekly { get; set; }
            public static string MainFiatMonthly { get; set; }

            public static void ResetRevenue()
            {
                MainCryptoDaily = String.Empty;
                MainCryptoWeekly = String.Empty;
                MainCryptoMonthly = String.Empty;
                MainFiatDaily = String.Empty;
                MainFiatWeekly = String.Empty;
                MainFiatMonthly = String.Empty;
            }
        }

        public class BlockchainData
        {
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

            public static void ClearBlockchainDatas()
            {
                BlockBalance = 0;
                NumberTransactions = 0;
                Array.Clear(BlockNumber, 0, BlockNumber.Length);
                Array.Clear(TransactionDate, 0, TransactionDate.Length);
                Array.Clear(BlockHash, 0, BlockHash.Length);
                Array.Clear(FromWallet, 0, FromWallet.Length);
                Array.Clear(toWallet, 0, toWallet.Length);
                Array.Clear(TransferedValue, 0, TransferedValue.Length);
                Array.Clear(FeeValue, 0, FeeValue.Length);
                Array.Clear(IsError, 0, IsError.Length);
            }
        }

        public class LoadingWindow
        {
            public static string blockchainDataInfo { get; set; }
            public static string mainDataInfo { get; set; }

            public static void ClearLoading()
            {
                blockchainDataInfo = String.Empty;
                mainDataInfo = String.Empty;
            }
        }

        public static void ResetVars()
        {
            Checkers.ClearCheckers();
            LoadingWindow.ClearLoading();
            BlockchainData.ClearBlockchainDatas();
            MinerData.ClearMinerData();
            RigInfo.ClearRigInfo();
            RigRevenue.ResetRevenue();
        }

    }
}
