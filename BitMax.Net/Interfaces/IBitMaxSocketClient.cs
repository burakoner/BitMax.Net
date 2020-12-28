using BitMax.Net.Enums;
using BitMax.Net.RestObjects;
using BitMax.Net.SocketObjects;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitMax.Net.Interfaces
{
    public interface IBitMaxSocketClient
    {
        bool Authendicated { get; }

        CallResult<bool> Login();
        Task<CallResult<bool>> LoginAsync();
        CallResult<UpdateSubscription> SubscribeToBestAskBidUpdates(IEnumerable<string> symbols, Action<BitMaxSocketBBO> onData);
        CallResult<UpdateSubscription> SubscribeToBestAskBidUpdates(string symbol, Action<BitMaxSocketBBO> onData);
        Task<CallResult<UpdateSubscription>> SubscribeToBestAskBidUpdatesAsync(IEnumerable<string> symbols, Action<BitMaxSocketBBO> onData);
        CallResult<UpdateSubscription> SubscribeToCandles(IEnumerable<string> symbols, BitMaxPeriod period, Action<BitMaxSocketCandle> onData);
        CallResult<UpdateSubscription> SubscribeToCandles(string symbol, BitMaxPeriod period, Action<BitMaxSocketCandle> onData);
        Task<CallResult<UpdateSubscription>> SubscribeToCandlesAsync(IEnumerable<string> symbols, BitMaxPeriod period, Action<BitMaxSocketCandle> onData);
        CallResult<UpdateSubscription> SubscribeToMarginBalanceAndOrders(Action<BitMaxSocketMarginBalanceExt> onMarginBalanceData, Action<BitMaxSocketCashOrderExt> onOrderData);
        Task<CallResult<UpdateSubscription>> SubscribeToMarginBalanceAndOrdersAsync(Action<BitMaxSocketMarginBalanceExt> onMarginBalanceData, Action<BitMaxSocketCashOrderExt> onOrderData);
        CallResult<UpdateSubscription> SubscribeToOrderBookUpdates(IEnumerable<string> symbols, Action<BitMaxSocketOrderBook> onData);
        CallResult<UpdateSubscription> SubscribeToOrderBookUpdates(string symbol, Action<BitMaxSocketOrderBook> onData);
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, Action<BitMaxSocketOrderBook> onData);
        CallResult<UpdateSubscription> SubscribeToSpotBalanceAndOrders(Action<BitMaxSocketSpotBalanceExt> onSpotBalanceData, Action<BitMaxSocketCashOrderExt> onOrderData);
        Task<CallResult<UpdateSubscription>> SubscribeToSpotBalanceAndOrdersAsync(Action<BitMaxSocketSpotBalanceExt> onSpotBalanceData, Action<BitMaxSocketCashOrderExt> onOrderData);
        CallResult<UpdateSubscription> SubscribeToTrades(IEnumerable<string> symbols, Action<BitMaxSocketTrade> onData);
        CallResult<UpdateSubscription> SubscribeToTrades(string symbol, Action<BitMaxSocketTrade> onData);
        Task<CallResult<UpdateSubscription>> SubscribeToTradesAsync(IEnumerable<string> symbols, Action<BitMaxSocketTrade> onData);
    }
}
