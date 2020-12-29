using BitMax.Net.Enums;
using BitMax.Net.RestObjects;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BitMax.Net.Interfaces
{
    public interface IBitMaxClient
    {
        int AccountGroup { get; set; }
        void SetAccountGroup(int account_group);
        void SetApiCredentials(string apiKey, string apiSecret);
        WebCallResult<bool> AccountTransfer(BitMaxWalletAccount from, BitMaxWalletAccount to, string asset, decimal amount, CancellationToken ct = default);
        Task<WebCallResult<bool>> AccountTransferAsync(BitMaxWalletAccount from, BitMaxWalletAccount to, string asset, decimal amount, CancellationToken ct = default);
        WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>> CancelAllMarginOrders(string symbol = null, CancellationToken ct = default);
        Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>> CancelAllMarginOrdersAsync(string symbol = null, CancellationToken ct = default);
        WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>> CancelAllSpotOrders(string symbol = null, CancellationToken ct = default);
        Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>> CancelAllSpotOrdersAsync(string symbol = null, CancellationToken ct = default);
        WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>> CancelMarginOrder(string symbol, string orderId, string clientOrderId = null, CancellationToken ct = default);
        Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>> CancelMarginOrderAsync(string symbol, string orderId, string clientOrderId = null, CancellationToken ct = default);
        WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>> CancelSpotOrder(string symbol, string orderId, string clientOrderId = null, CancellationToken ct = default);
        Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>> CancelSpotOrderAsync(string symbol, string orderId, string clientOrderId = null, CancellationToken ct = default);
        WebCallResult<BitMaxAccountInfo> GetAccountInfo(bool setAccountGroup = false, CancellationToken ct = default);
        Task<WebCallResult<BitMaxAccountInfo>> GetAccountInfoAsync(bool setAccountGroup = false, CancellationToken ct = default);
        WebCallResult<IEnumerable<BitMaxCashAsset>> GetAssets(CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitMaxCashAsset>>> GetAssetsAsync(CancellationToken ct = default);
        WebCallResult<IEnumerable<BitMaxBarPeriod>> GetBarPeriods(CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitMaxBarPeriod>>> GetBarPeriodsAsync(CancellationToken ct = default);
        WebCallResult<IEnumerable<BitMaxCandleSerie>> GetCandles(string symbol, BitMaxPeriod period, int limit = 10, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitMaxCandleSerie>>> GetCandlesAsync(string symbol, BitMaxPeriod period, int limit = 10, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
        WebCallResult<BitMaxDepositAddress> GetDepositAddresses(string asset, string blockchain = null, CancellationToken ct = default);
        Task<WebCallResult<BitMaxDepositAddress>> GetDepositAddressesAsync(string asset, string blockchain = null, CancellationToken ct = default);
        WebCallResult<IEnumerable<BitMaxMarginBalance>> GetMarginBalances(string asset = null, bool showAll = false, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitMaxMarginBalance>>> GetMarginBalancesAsync(string asset = null, bool showAll = false, CancellationToken ct = default);
        WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>> GetMarginCurrentHistoryOrders(string symbol = null, bool executedOnly = false, int limit = 100, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetMarginCurrentHistoryOrdersAsync(string symbol = null, bool executedOnly = false, int limit = 100, CancellationToken ct = default);
        WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>> GetMarginHistoryOrders(string symbol = null, DateTime? startTime = null, DateTime? endTime = null, long? seqNum = null, int limit = 500, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetMarginHistoryOrdersAsync(string symbol = null, DateTime? startTime = null, DateTime? endTime = null, long? seqNum = null, int limit = 500, CancellationToken ct = default);
        WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>> GetMarginOpenOrders(string symbol = null, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetMarginOpenOrdersAsync(string symbol = null, CancellationToken ct = default);
        WebCallResult<BitMaxCashPlacedOrderInfoAccept> GetMarginOrder(string orderId, CancellationToken ct = default);
        Task<WebCallResult<BitMaxCashPlacedOrderInfoAccept>> GetMarginOrderAsync(string orderId, CancellationToken ct = default);
        WebCallResult<BitMaxMarginRisk> GetMarginRisk(CancellationToken ct = default);
        Task<WebCallResult<BitMaxMarginRisk>> GetMarginRiskAsync(CancellationToken ct = default);
        WebCallResult<BitMaxSerie<BitMaxOrderBook>> GetOrderBook(string symbol, CancellationToken ct = default);
        Task<WebCallResult<BitMaxSerie<BitMaxOrderBook>>> GetOrderBookAsync(string symbol, CancellationToken ct = default);
        WebCallResult<IEnumerable<BitMaxProduct>> GetProducts(CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitMaxProduct>>> GetProductsAsync(CancellationToken ct = default);
        WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>> GetSpotCurrentHistoryOrders(string symbol = null, bool executedOnly = false, int limit = 100, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetSpotCurrentHistoryOrdersAsync(string symbol = null, bool executedOnly = false, int limit = 100, CancellationToken ct = default);
        WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>> GetSpotHistoryOrders(string symbol = null, DateTime? startTime = null, DateTime? endTime = null, long? seqNum = null, int limit = 500, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetSpotHistoryOrdersAsync(string symbol = null, DateTime? startTime = null, DateTime? endTime = null, long? seqNum = null, int limit = 500, CancellationToken ct = default);
        WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>> GetSpotOpenOrders(string symbol = null, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetSpotOpenOrdersAsync(string symbol = null, CancellationToken ct = default);
        WebCallResult<BitMaxCashPlacedOrderInfoAccept> GetSpotOrder(string orderId, CancellationToken ct = default);
        Task<WebCallResult<BitMaxCashPlacedOrderInfoAccept>> GetSpotOrderAsync(string orderId, CancellationToken ct = default);
        WebCallResult<IEnumerable<BitMaxTicker>> GetTickers(CancellationToken ct = default);
        WebCallResult<IEnumerable<BitMaxTicker>> GetTickers(IEnumerable<string> symbols, CancellationToken ct = default);
        WebCallResult<IEnumerable<BitMaxTicker>> GetTickers(params string[] symbols);
        WebCallResult<IEnumerable<BitMaxTicker>> GetTickers(string symbol, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitMaxTicker>>> GetTickersAsync(CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitMaxTicker>>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<BitMaxTicker>>> GetTickersAsync(params string[] symbols);
        Task<WebCallResult<IEnumerable<BitMaxTicker>>> GetTickersAsync(string symbol, CancellationToken ct = default);
        WebCallResult<BitMaxSerie<IEnumerable<BitMaxCashTrade>>> GetTrades(string symbol, int limit = 100, CancellationToken ct = default);
        Task<WebCallResult<BitMaxSerie<IEnumerable<BitMaxCashTrade>>>> GetTradesAsync(string symbol, int limit = 100, CancellationToken ct = default);
        WebCallResult<BitMaxPagedData<BitMaxTransaction>> GetWalletTransactions(string asset = null, BitMaxTransactionType? txType = null, int page = 1, int pageSize = 10, CancellationToken ct = default);
        Task<WebCallResult<BitMaxPagedData<BitMaxTransaction>>> GetWalletTransactionsAsync(string asset = null, BitMaxTransactionType? txType = null, int page = 1, int pageSize = 10, CancellationToken ct = default);
        WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAccept>> PlaceMarginOrder(string symbol, decimal size, BitMaxCashOrderType type, BitMaxOrderSide side, decimal? orderPrice = null, decimal? stopPrice = null, string clientOrderId = null, bool postOnly = false, BitMaxCashOrderTimeInForce timeInForce = BitMaxCashOrderTimeInForce.GoodTillCanceled, CancellationToken ct = default);
        Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAccept>>> PlaceMarginOrderAsync(string symbol, decimal size, BitMaxCashOrderType type, BitMaxOrderSide side, decimal? orderPrice = null, decimal? stopPrice = null, string clientOrderId = null, bool postOnly = false, BitMaxCashOrderTimeInForce timeInForce = BitMaxCashOrderTimeInForce.GoodTillCanceled, CancellationToken ct = default);
        WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAccept>> PlaceSpotOrder(string symbol, decimal size, BitMaxCashOrderType type, BitMaxOrderSide side, decimal? orderPrice = null, decimal? stopPrice = null, string clientOrderId = null, bool postOnly = false, BitMaxCashOrderTimeInForce timeInForce = BitMaxCashOrderTimeInForce.GoodTillCanceled, CancellationToken ct = default);
        Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAccept>>> PlaceSpotOrderAsync(string symbol, decimal size, BitMaxCashOrderType type, BitMaxOrderSide side, decimal? orderPrice = null, decimal? stopPrice = null, string clientOrderId = null, bool postOnly = false, BitMaxCashOrderTimeInForce timeInForce = BitMaxCashOrderTimeInForce.GoodTillCanceled, CancellationToken ct = default);
    }
}
