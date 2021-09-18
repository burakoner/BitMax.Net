using BitMax.Net.CoreObjects;
using BitMax.Net.Enums;
using BitMax.Net.RestObjects;
using CryptoExchange.Net.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace BitMax.Net.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Rest Api Client */
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
            api.SetAccountGroup(account_info.Data.AccountGroup);
            var spot_balances_01 = api.GetSpotBalances();
            var spot_balances_02 = api.GetSpotBalances(showAll: true);
            var spot_balances_03 = api.GetSpotBalances("BTC");
            var spot_balances_04 = api.GetSpotBalances("BTC", true);
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
            var spot_order_01 = api.PlaceSpotOrder("ETH/USDT", 0.1m, BitMaxCashOrderType.Limit, BitMaxOrderSide.Buy, orderPrice: 607.90m);
            var spot_order_02 = api.PlaceSpotOrder("BTC/USDT", 0.1m, BitMaxCashOrderType.Market, BitMaxOrderSide.Buy);
            var spot_order_03 = api.PlaceSpotOrder("BTC/USDT", 0.1m, BitMaxCashOrderType.Limit, BitMaxOrderSide.Buy, 23000.00m);
            var margin_order_01 = api.PlaceMarginOrder("ETH/USDT", 0.1m, BitMaxCashOrderType.Limit, BitMaxOrderSide.Buy, orderPrice: 607.90m);
            var margin_order_02 = api.PlaceMarginOrder("BTC/USDT", 0.1m, BitMaxCashOrderType.Market, BitMaxOrderSide.Buy);
            var margin_order_03 = api.PlaceMarginOrder("BTC/USDT", 0.1m, BitMaxCashOrderType.Limit, BitMaxOrderSide.Buy, 23000.00m);
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

            var spot_orders = new List<BitMaxCashPlaceOrder>();
            spot_orders.Add(new BitMaxCashPlaceOrder
            {
                Symbol = "BTC/USDT",
                Size = 0.1m,
                OrderType = BitMaxCashOrderType.Market,
                OrderSide = BitMaxOrderSide.Buy,
            });
            spot_orders.Add(new BitMaxCashPlaceOrder
            {
                Symbol = "BTC/USDT",
                Size = 0.1m,
                OrderType = BitMaxCashOrderType.Limit,
                OrderSide = BitMaxOrderSide.Buy,
                OrderPrice = 23000.00m,
                PostOnly = false,
                TimeInForce = BitMaxCashOrderTimeInForce.GoodTillCanceled,
            });
            var spot_batch_orders = api.PlaceSpotBatchOrders(spot_orders);

            var spot_orders_to_cancel = new List<BitMaxCashCancelOrder>();
            spot_orders_to_cancel.Add(new BitMaxCashCancelOrder
            {
                Symbol = "BTC/USDT",
                OrderId = "a176a4316ec6U3352487793bethuCafd",
            });
            spot_orders_to_cancel.Add(new BitMaxCashCancelOrder
            {
                Symbol = "BTC/USDT",
                OrderId = "a176a4316ec6U3352487793bethuCafe",
            });
            var spot_cancel_batch_orders = api.CancelSpotBatchOrders(spot_orders_to_cancel);

            /* Futures Api Public Endpoints */
            var futures_assets = api.GetFuturesAssets();
            var futures_contracts = api.GetFuturesContracts();
            var futures_refprices = api.GetFuturesReferencePrices();
            var futures_marketdata_01 = api.GetFuturesMarketData();
            var futures_marketdata_02 = api.GetFuturesMarketData("BTC-PERP");
            var futures_fundingrates = api.GetFuturesFundingRates();

            /* Futures Api Private Endpoints */
            api.SetApiCredentials("XXXXXXXX-API-KEY-XXXXXXXX", "XXXXXXXX-API-SECRET-XXXXXXXX");
            var futures_info = api.GetAccountInfo();
            api.SetAccountGroup(futures_info.Data.AccountGroup);
            var futures_balances = api.GetFuturesBalances();
            var futures_positions = api.GetFuturesPositions();
            var futures_risk = api.GetFuturesRisk();
            var futures_payments = api.GetFuturesFundingPayments();
            var futures_transfer_01 = api.TransferFromCashToFutures("BTC", 0.1m);
            var futures_transfer_02 = api.TransferFromFuturesToCash("BTC", 0.1m);
            var futures_order_01 = api.PlaceFuturesOrder("BTC-PERP", 0.1m, BitMaxFuturesOrderType.Limit, BitMaxOrderSide.Buy, orderPrice: 23000.00m);
            var futures_order_02 = api.PlaceFuturesOrder("BTC-PERP", 0.1m, BitMaxFuturesOrderType.Market, BitMaxOrderSide.Buy);
            var futures_order_03 = api.PlaceFuturesOrder("BTC-PERP", 0.1m, BitMaxFuturesOrderType.Limit, BitMaxOrderSide.Buy, 23000.00m);
            var futures_cancel_order = api.CancelFuturesOrder("BTC-PERP", "a176a4316ec6U3352487793bethuCafd");
            var futures_cancel_all_orders_01 = api.CancelAllFuturesOrders();
            var futures_cancel_all_orders_02 = api.CancelAllFuturesOrders("BTC-PERP");
            var futures_place_batch_orders = api.PlaceFuturesBatchOrders(new List<BitMaxFuturesPlaceOrder> { });
            var futures_cancel_batch_orders = api.CancelFuturesBatchOrders(new List<BitMaxFuturesCancelOrder> { });
            var futures_query = api.GetFuturesOrder("a176a4316ec6U3352487793bethuCafd");
            var futures_open_orders_01 = api.GetFuturesOpenOrders();
            var futures_open_orders_02 = api.GetSpotOpenOrders("BTC-PERP");
            var futures_current_history_orders = api.GetFuturesCurrentHistoryOrders();

            /* Web Socket Api Client */
            var credentials = new CryptoExchange.Net.Authentication.ApiCredentials("XXXXXXXX-API-KEY-XXXXXXXX", "XXXXXXXX-API-SECRET-XXXXXXXX");
            var ws = new BitMaxSocketClient(new BitMaxSocketClientOptions(account_info.Data.AccountGroup, credentials ));
            var auth = ws.Login();

            var sub01 = ws.SubscribeToSummary(new List<string> { "BTC/USDT", "ETH/USDT" }, (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Summary >> {data.Symbol} T:{data.Timestamp} O:{data.Open} H:{data.High} L:{data.Low} C:{data.Close} V:{data.Volume}");
                }
            });

            var sub02 = ws.SubscribeToBestAskBidUpdates(new List<string> { "BTC/USDT", "ETH/USDT" }, (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"BBO >> {data.Symbol} T:{data.Timestamp} AP:{data.BestAsk.Price} AA:{data.BestAsk.Quantity} BP:{data.BestBid.Price} BA:{data.BestBid.Quantity}");
                }
            });

            var sub03 = ws.SubscribeToOrderBookUpdates(new List<string> { "BTC/USDT", "ETH/USDT" }, (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Book >> {data.Symbol} T:{data.Timestamp} AC:{data.Asks.Count()} BC:{data.Bids.Count()}");
                }
            });

            var sub04 = ws.SubscribeToTrades(new List<string> { "BTC/USDT", "ETH/USDT" }, (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Trades >> {data.Symbol} T:{data.Timestamp} P:{data.Price} A:{data.Amount} SN:{data.SequenceNumber} BM:{data.IsBuyerMarketMaker}");
                }
            });

            var sub05 = ws.SubscribeToCandles(new List<string> { "BTC/USDT", "ETH/USDT" }, BitMaxPeriod.OneHour, (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Candle >> {data.Symbol} P:{data.Period} OT:{data.OpenTime} O:{data.Open} H:{data.High} L:{data.Low} C:{data.Close} V:{data.Volume}");
                }
            });

            // Needs Authentication
            var sub06 = ws.SubscribeToSpotBalanceAndOrders((data) =>
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
            
            // Needs Authentication
            var sub07 = ws.SubscribeToMarginBalanceAndOrders((data) =>
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

            // Unsubscribe
            _ = ws.UnsubscribeAsync(sub05.Data);

            ws.SubscribeToFuturesMarketData("BTC-PERP", (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Market Data >> {data.Symbol} OI:{data.OpenInterest} FR:{data.FundingRate} FPF:{data.FundingPaymentFlag} IP:{data.IndexPrice} MP:{data.MarkPrice}");
                }
            });

            // Needs Authentication
            ws.SubscribeToFuturesOrders((data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Order Data >> {data.Symbol} OI:{data.OrderId} OT:{data.OrderType} P:{data.Price} AP:{data.AveragePrice}");
                }
            });


            Console.ReadLine();
        }
    }
}
