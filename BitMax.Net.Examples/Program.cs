using BitMax.Net.CoreObjects;
using BitMax.Net.Enums;
using CryptoExchange.Net.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BitMax.Net.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            // var api = new BitMaxClient(new BitMaxClientOptions { LogVerbosity = LogVerbosity.Debug });
            var api = new BitMaxClient();

            /* Cash(Spot) / Margin Api Public Endpoints */
            var assets = api.GetAssets();
            var products = api.GetProducts();
            var tickers_01 = api.GetTickers();
            var tickers_02 = api.GetTickers("BTC/USDT");
            var tickers_03 = api.GetTickers("BTC/USDT", "ETH/USDT", "LTC/USDT");
            var tickers_04 = api.GetTickers(new List<string> { "BTC/USDT", "ETH/USDT", "LTC/USDT" });
            var periods = api.GetBarPeriods();
            var candles_01 = api.GetCandles("BTC/USDT", BitMaxPeriod.OneHour);
            var candles_02 = api.GetCandles("BTC/USDT", BitMaxPeriod.OneMonth, 10);
            var orderbook = api.GetOrderBook("BTC/USDT");
            var trades_01 = api.GetTrades("BTC/USDT");
            var trades_02 = api.GetTrades("BTC/USDT", 50);

            /* Cash(Spot) / Margin Api Private Endpoints */
            api.SetApiCredentials("XXXXXXXX-API-KEY-XXXXXXXX", "XXXXXXXX-API-SECRET-XXXXXXXX");
            var account_info = api.GetAccountInfo();
            api.SetAccountGroup(1);
            var spot_balances_01 = api.GetCashBalances();
            var spot_balances_02 = api.GetCashBalances(showAll: true);
            var spot_balances_03 = api.GetCashBalances("BTC");
            var spot_balances_04 = api.GetCashBalances("BTC", true);
            var margin_balances_01 = api.GetMarginBalances(showAll: false);
            var margin_balances_02 = api.GetMarginBalances(showAll: true);
            var margin_balances_03 = api.GetMarginBalances("BTC");
            var margin_balances_04 = api.GetMarginBalances("BTC", true);
            var margin_risk = api.GetMarginRisk();
            var transfer = api.AccountTransfer(BitMaxWalletAccount.Cash, BitMaxWalletAccount.Margin, "BTC", 0.1m);
            var deposit_address_01 = api.GetDepositAddresses("USDT");
            var deposit_address_02 = api.GetDepositAddresses("USDT", "ERC20");
            var wallet_transactions_01 = api.GetWalletTransactions();
            var wallet_transactions_02 = api.GetWalletTransactions("BTC");
            var wallet_transactions_03 = api.GetWalletTransactions("ETH", BitMaxTransactionType.Deposit, 1, 100);
            var spot_order_01 = api.PlaceSpotOrder("ETH/USDT", 0.1m, BitMaxCashOrderType.Limit, BitMaxCashOrderSide.Buy, orderPrice: 607.90m);
            var spot_order_02 = api.PlaceSpotOrder("BTC/USDT", 0.1m, BitMaxCashOrderType.Market, BitMaxCashOrderSide.Buy);
            var spot_order_03 = api.PlaceSpotOrder("BTC/USDT", 0.1m, BitMaxCashOrderType.Limit, BitMaxCashOrderSide.Buy, 23000.00m);
            var margin_order_01 = api.PlaceMarginOrder("ETH/USDT", 0.1m, BitMaxCashOrderType.Limit, BitMaxCashOrderSide.Buy, orderPrice: 607.90m);
            var margin_order_02 = api.PlaceMarginOrder("BTC/USDT", 0.1m, BitMaxCashOrderType.Market, BitMaxCashOrderSide.Buy);
            var margin_order_03 = api.PlaceMarginOrder("BTC/USDT", 0.1m, BitMaxCashOrderType.Limit, BitMaxCashOrderSide.Buy, 23000.00m);
            var spot_cancel_order = api.CancelSpotOrder("BTC/USDT", "a176a4316ec6U3352487793bethuCafd");
            var spot_cancel_all_orders_01 = api.CancelAllSpotOrders();
            var spot_cancel_all_orders_02 = api.CancelAllSpotOrders("ETH/USDT");    
            var margin_cancel_all_orders_01 = api.CancelAllMarginOrders();
            var margin_cancel_all_orders_02 = api.CancelAllMarginOrders("ETH/USDT");
            var spot_query = api.GetSpotOrder("a176a4316ec6U3352487793bethuCafd");
            var margin_query = api.GetSpotOrder("a176a4316ec6U3352487793bethuCafd");
            var spot_open_orders_01 = api.GetSpotOpenOrders();
            var spot_open_orders_02 = api.GetSpotOpenOrders("ETH/USDT");
            var spot_current_history_orders = api.GetSpotCurrentHistoryOrders();
            var spot_history_orders = api.GetSpotHistoryOrders();

            /* Futures Api Public Endpoints */
            var f_assets = api.GetFuturesAssets();

            /* Web Socket Feed Api */
            var credentials = new CryptoExchange.Net.Authentication.ApiCredentials("XXXXXXXX-API-KEY-XXXXXXXX", "XXXXXXXX-API-SECRET-XXXXXXXX");
            var ws = new BitMaxSocketClient(new BitMaxSocketClientOptions { ApiCredentials = credentials });
            var auth = ws.Login();

            var sub01 = ws.SubscribeToBestAskBidUpdates(new List<string> { "BTC/USDT", "ETH/USDT" }, (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"BBO >> {data.Symbol} T:{data.Timestamp} AP:{data.BestAsk.Price} AA:{data.BestAsk.Quantity} BP:{data.BestBid.Price} BA:{data.BestBid.Quantity}");
                }
            });

            var sub02 = ws.SubscribeToOrderBookUpdates(new List<string> { "BTC/USDT", "ETH/USDT" }, (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Book >> {data.Symbol} T:{data.Timestamp} AC:{data.Asks.Count()} BC:{data.Bids.Count()}");
                }
            });

            var sub03 = ws.SubscribeToTrades(new List<string> { "BTC/USDT", "ETH/USDT" }, (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Trades >> {data.Symbol} T:{data.Timestamp} P:{data.Price} A:{data.Amount} SN:{data.SequenceNumber} BM:{data.IsBuyerMarketMaker}");
                }
            });

            var sub04 = ws.SubscribeToCandles(new List<string> { "BTC/USDT", "ETH/USDT" }, BitMaxPeriod.OneHour, (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Candle >> {data.Symbol} P:{data.Period} OT:{data.OpenTime} O:{data.Open} H:{data.High} L:{data.Low} C:{data.Close} V:{data.Volume}");
                }
            });

            // Needs Authentication
            var sub05 = ws.SubscribeToSpotBalanceAndOrders((data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Balance >> {data.Asset} AB:{data.AvailableBalance} TB:{data.TotalBalance}");
                }
            }, (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Order >> {data.Symbol} OT:{data.OrderType} P:{data.Price} SP:{data.StopPrice}");
                }
            });
            _ = ws.Unsubscribe(sub05.Data);


            Console.ReadLine();
        }
    }
}
