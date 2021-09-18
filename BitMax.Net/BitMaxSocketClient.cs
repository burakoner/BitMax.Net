using BitMax.Net.Converters;
using BitMax.Net.CoreObjects;
using BitMax.Net.Enums;
using BitMax.Net.Interfaces;
using BitMax.Net.RestObjects;
using BitMax.Net.SocketObjects;
using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitMax.Net
{
    public partial class BitMaxSocketClient : SocketClient, ISocketClient, IBitMaxSocketClient
    {
        public bool Authendicated { get; protected set; }

        #region Client Options
        protected static BitMaxSocketClientOptions defaultOptions = new BitMaxSocketClientOptions();
        protected static BitMaxSocketClientOptions DefaultOptions => defaultOptions.Copy();
        #endregion

        #region Constructor/Destructor
        /// <summary>
        /// Create a new instance of BitMaxSocketClient with default options
        /// </summary>
        public BitMaxSocketClient() : this(DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of BitMaxSocketClient using provided options
        /// </summary>
        /// <param name="options">The options to use for this client</param>
        public BitMaxSocketClient(BitMaxSocketClientOptions options) : base("BitMax", options, options.ApiCredentials == null ? null : new BitMaxAuthenticationProvider(options.ApiCredentials, ArrayParametersSerialization.Array))
        {
            AddGenericHandler("Ping", PingHandler);
        }
        #endregion

        #region Common Methods
        /// <summary>
        /// Set the default options to be used when creating new socket clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
        public static void SetDefaultOptions(BitMaxSocketClientOptions options)
        {
            defaultOptions = options;
        }
        #endregion

        /// <summary>
        /// Subscribe to summary (ticker) data
        /// </summary>
        /// <param name="symbol">Trading Symbol</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToSummary(string symbol, Action<BitMaxSocketSummary> onData) => SubscribeToSummaryAsync(new List<string> { symbol }, onData).Result;
        /// <summary>
        /// Subscribe to summary (ticker) data
        /// </summary>
        /// <param name="symbols">Trading Symbols</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToSummary(IEnumerable<string> symbols, Action<BitMaxSocketSummary> onData) => SubscribeToSummaryAsync(symbols, onData).Result;
        /// <summary>
        /// Subscribe to summary (ticker) data
        /// </summary>
        /// <param name="symbols">Trading Symbols</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToSummaryAsync(IEnumerable<string> symbols, Action<BitMaxSocketSummary> onData)
        {
            var internalHandler = new Action<DataEvent<BitMaxSocketBarChannelResponse<BitMaxSocketSummary>>>(data =>
            {
                data.Data.Symbol = data.Data.Data.Symbol;
                onData(data.Data.Data);
            });

            var request = new BitMaxSocketCashChannelRequest(NextRequestId(), BitMaxSocketCashChannelOperation.Subscribe, "summary:" + string.Join(",", symbols));
            return await SubscribeAsync(request, "summary", false, internalHandler).ConfigureAwait(false);

        }

        /// <summary>
        /// You can subscribe to updates of best bid/offer data stream only. Once subscribed, you will receive BBO message whenever the price and/or size changes at the top of the order book.
        /// Each BBO message contains price and size data for exactly one bid level and one ask level.
        /// </summary>
        /// <param name="symbol">Trading Symbol</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToBestAskBidUpdates(string symbol, Action<BitMaxSocketBBO> onData) => SubscribeToBestAskBidUpdatesAsync(new List<string> { symbol }, onData).Result;
        /// <summary>
        /// You can subscribe to updates of best bid/offer data stream only. Once subscribed, you will receive BBO message whenever the price and/or size changes at the top of the order book.
        /// Each BBO message contains price and size data for exactly one bid level and one ask level.
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToBestAskBidUpdates(IEnumerable<string> symbols, Action<BitMaxSocketBBO> onData) => SubscribeToBestAskBidUpdatesAsync(symbols, onData).Result;
        /// <summary>
        /// You can subscribe to updates of best bid/offer data stream only. Once subscribed, you will receive BBO message whenever the price and/or size changes at the top of the order book.
        /// Each BBO message contains price and size data for exactly one bid level and one ask level.
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToBestAskBidUpdatesAsync(IEnumerable<string> symbols, Action<BitMaxSocketBBO> onData)
        {
            var internalHandler = new Action<DataEvent<BitMaxSocketChannelResponse<BitMaxSocketBBO>>>(data =>
            {
                data.Data.Symbol = data.Data.Data.Symbol;
                onData(data.Data.Data);
            });

            var request = new BitMaxSocketCashChannelRequest(NextRequestId(), BitMaxSocketCashChannelOperation.Subscribe, "bbo:" + string.Join(",", symbols));
            return await SubscribeAsync(request, "bbo", false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// If you want to keep track of the most recent order book snapshot in its entirety, the most efficient way is to subscribe to the depth channel.
        /// Each depth message contains a bids list and an asks list in its data field. Each list contains a series of [price, size] pairs that you can use to update the order book snapshot. In the message, price is always positive and size is always non-negative.
        /// if size is positive and the price doesn't exist in the current order book, you should add a new level [price, size].
        /// if size is positive and the price exists in the current order book, you should update the existing level to [price, size].
        /// if size is zero, you should delete the level at price.
        /// </summary>
        /// <param name="symbol">Trading Symbol</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToOrderBookUpdates(string symbol, Action<BitMaxSocketOrderBook> onData) => SubscribeToOrderBookUpdatesAsync(new List<string> { symbol }, onData).Result;
        /// <summary>
        /// If you want to keep track of the most recent order book snapshot in its entirety, the most efficient way is to subscribe to the depth channel.
        /// Each depth message contains a bids list and an asks list in its data field. Each list contains a series of [price, size] pairs that you can use to update the order book snapshot. In the message, price is always positive and size is always non-negative.
        /// if size is positive and the price doesn't exist in the current order book, you should add a new level [price, size].
        /// if size is positive and the price exists in the current order book, you should update the existing level to [price, size].
        /// if size is zero, you should delete the level at price.
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToOrderBookUpdates(IEnumerable<string> symbols, Action<BitMaxSocketOrderBook> onData) => SubscribeToOrderBookUpdatesAsync(symbols, onData).Result;
        /// <summary>
        /// If you want to keep track of the most recent order book snapshot in its entirety, the most efficient way is to subscribe to the depth channel.
        /// Each depth message contains a bids list and an asks list in its data field. Each list contains a series of [price, size] pairs that you can use to update the order book snapshot. In the message, price is always positive and size is always non-negative.
        /// if size is positive and the price doesn't exist in the current order book, you should add a new level [price, size].
        /// if size is positive and the price exists in the current order book, you should update the existing level to [price, size].
        /// if size is zero, you should delete the level at price.
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, Action<BitMaxSocketOrderBook> onData)
        {
            var internalHandler = new Action<DataEvent<BitMaxSocketChannelResponse<BitMaxSocketOrderBook>>>(data =>
            {
                data.Data.Symbol = data.Data.Data.Symbol;
                onData(data.Data.Data);
            });

            var request = new BitMaxSocketCashChannelRequest(NextRequestId(), BitMaxSocketCashChannelOperation.Subscribe, "depth:" + string.Join(",", symbols));
            return await SubscribeAsync(request, "depth", false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// The data field is a list containing one or more trade objects. The server may combine consecutive trades with the same price and bm value into one aggregated item. Each trade object contains the following fields:
        /// </summary>
        /// <param name="symbol">Trading Symbol</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToTrades(string symbol, Action<BitMaxSocketTrade> onData) => SubscribeToTradesAsync(new List<string> { symbol }, onData).Result;
        /// <summary>
        /// The data field is a list containing one or more trade objects. The server may combine consecutive trades with the same price and bm value into one aggregated item. Each trade object contains the following fields:
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToTrades(IEnumerable<string> symbols, Action<BitMaxSocketTrade> onData) => SubscribeToTradesAsync(symbols, onData).Result;
        /// <summary>
        /// The data field is a list containing one or more trade objects. The server may combine consecutive trades with the same price and bm value into one aggregated item. Each trade object contains the following fields:
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToTradesAsync(IEnumerable<string> symbols, Action<BitMaxSocketTrade> onData)
        {
            var internalHandler = new Action<DataEvent<BitMaxSocketChannelResponse<IEnumerable<BitMaxSocketTrade>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                {
                    d.Symbol = data.Data.Symbol;
                    onData(d);
                }
            });

            var request = new BitMaxSocketCashChannelRequest(NextRequestId(), BitMaxSocketCashChannelOperation.Subscribe, "trades:" + string.Join(",", symbols));
            return await SubscribeAsync(request, "trades", false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// The data field is a list containing one or more trade objects. The server may combine consecutive trades with the same price and bm value into one aggregated item. Each trade object contains the following fields:
        /// </summary>
        /// <param name="symbol">Trading Symbol</param>
        /// <param name="period">Candle Period</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToCandles(string symbol, BitMaxPeriod period, Action<BitMaxSocketCandle> onData) => SubscribeToCandlesAsync(new List<string> { symbol }, period, onData).Result;
        /// <summary>
        /// The data field is a list containing one or more trade objects. The server may combine consecutive trades with the same price and bm value into one aggregated item. Each trade object contains the following fields:
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="period">Candle Period</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToCandles(IEnumerable<string> symbols, BitMaxPeriod period, Action<BitMaxSocketCandle> onData) => SubscribeToCandlesAsync(symbols, period, onData).Result;
        /// <summary>
        /// The data field is a list containing one or more trade objects. The server may combine consecutive trades with the same price and bm value into one aggregated item. Each trade object contains the following fields:
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="period">Candle Period</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToCandlesAsync(IEnumerable<string> symbols, BitMaxPeriod period, Action<BitMaxSocketCandle> onData)
        {
            var internalHandler = new Action<DataEvent<BitMaxSocketBarChannelResponse<BitMaxSocketCandle>>>(data =>
            {
                data.Data.Data.Symbol = data.Data.Symbol;
                onData(data.Data.Data);
            });

            var period_s = JsonConvert.SerializeObject(period, new PeriodConverter(false));
            var request = new BitMaxSocketCashChannelRequest(NextRequestId(), BitMaxSocketCashChannelOperation.Subscribe, "bar:" + period_s + ":" + string.Join(",", symbols));
            return await SubscribeAsync(request, "bar", false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Note: once you subscribe to the order channel, you will start receiving messages from the balance channel automatically. If you unsubscribe from the order channel, you will simultaneously unsubscribe from the balance channel.
        /// You need to specify the account when subscribing to the order channel. You could specify account category cash, margin, or specific account id.
        /// Order Messages:
        /// - You can track the state change of each order thoughout its life cycle with the order update message (m=order). The data field is a single order udpate object. Each order update object contains the following fields:
        /// Balance Messages:
        /// - You will also receive balance update message (m=balance) for the asset balance updates not caused by orders. For instance, when you make wallet deposits/withdrawals, or when you transfer asset from the cash account to the margin account, you will receive balance update message.
        /// </summary>
        /// <param name="onSpotBalanceData">On Data Handler</param>
        /// <param name="onOrderData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToSpotBalanceAndOrders(Action<BitMaxSocketSpotBalanceExt> onSpotBalanceData, Action<BitMaxSocketCashOrderExt> onOrderData) => SubscribeToBalanceAndOrdersAsync(BitMaxAccountType.Spot, onSpotBalanceData, null, onOrderData).Result;
        /// <summary>
        /// Note: once you subscribe to the order channel, you will start receiving messages from the balance channel automatically. If you unsubscribe from the order channel, you will simultaneously unsubscribe from the balance channel.
        /// You need to specify the account when subscribing to the order channel. You could specify account category cash, margin, or specific account id.
        /// Order Messages:
        /// - You can track the state change of each order thoughout its life cycle with the order update message (m=order). The data field is a single order udpate object. Each order update object contains the following fields:
        /// Balance Messages:
        /// - You will also receive balance update message (m=balance) for the asset balance updates not caused by orders. For instance, when you make wallet deposits/withdrawals, or when you transfer asset from the cash account to the margin account, you will receive balance update message.
        /// </summary>
        /// <param name="onMarginBalanceData">On Data Handler</param>
        /// <param name="onOrderData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToMarginBalanceAndOrders(Action<BitMaxSocketMarginBalanceExt> onMarginBalanceData, Action<BitMaxSocketCashOrderExt> onOrderData) => SubscribeToBalanceAndOrdersAsync(BitMaxAccountType.Margin, null, onMarginBalanceData, onOrderData).Result;
        /// <summary>
        /// Note: once you subscribe to the order channel, you will start receiving messages from the balance channel automatically. If you unsubscribe from the order channel, you will simultaneously unsubscribe from the balance channel.
        /// You need to specify the account when subscribing to the order channel. You could specify account category cash, margin, or specific account id.
        /// Order Messages:
        /// - You can track the state change of each order thoughout its life cycle with the order update message (m=order). The data field is a single order udpate object. Each order update object contains the following fields:
        /// Balance Messages:
        /// - You will also receive balance update message (m=balance) for the asset balance updates not caused by orders. For instance, when you make wallet deposits/withdrawals, or when you transfer asset from the cash account to the margin account, you will receive balance update message.
        /// </summary>
        /// <param name="onSpotBalanceData">On Data Handler</param>
        /// <param name="onOrderData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToSpotBalanceAndOrdersAsync(Action<BitMaxSocketSpotBalanceExt> onSpotBalanceData, Action<BitMaxSocketCashOrderExt> onOrderData) => await SubscribeToBalanceAndOrdersAsync(BitMaxAccountType.Spot, onSpotBalanceData, null, onOrderData);
        /// <summary>
        /// Note: once you subscribe to the order channel, you will start receiving messages from the balance channel automatically. If you unsubscribe from the order channel, you will simultaneously unsubscribe from the balance channel.
        /// You need to specify the account when subscribing to the order channel. You could specify account category cash, margin, or specific account id.
        /// Order Messages:
        /// - You can track the state change of each order thoughout its life cycle with the order update message (m=order). The data field is a single order udpate object. Each order update object contains the following fields:
        /// Balance Messages:
        /// - You will also receive balance update message (m=balance) for the asset balance updates not caused by orders. For instance, when you make wallet deposits/withdrawals, or when you transfer asset from the cash account to the margin account, you will receive balance update message.
        /// </summary>
        /// <param name="onMarginBalanceData">On Data Handler</param>
        /// <param name="onOrderData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToMarginBalanceAndOrdersAsync(Action<BitMaxSocketMarginBalanceExt> onMarginBalanceData, Action<BitMaxSocketCashOrderExt> onOrderData) => await SubscribeToBalanceAndOrdersAsync(BitMaxAccountType.Margin, null, onMarginBalanceData, onOrderData);
        /// <summary>
        /// Note: once you subscribe to the order channel, you will start receiving messages from the balance channel automatically. If you unsubscribe from the order channel, you will simultaneously unsubscribe from the balance channel.
        /// You need to specify the account when subscribing to the order channel. You could specify account category cash, margin, or specific account id.
        /// Order Messages:
        /// - You can track the state change of each order thoughout its life cycle with the order update message (m=order). The data field is a single order udpate object. Each order update object contains the following fields:
        /// Balance Messages:
        /// - You will also receive balance update message (m=balance) for the asset balance updates not caused by orders. For instance, when you make wallet deposits/withdrawals, or when you transfer asset from the cash account to the margin account, you will receive balance update message.
        /// </summary>
        /// <param name="cashAccountType">Cash Account Type</param>
        /// <param name="onSpotBalanceData">On Data Handler</param>
        /// <param name="onMarginBalanceData">On Data Handler</param>
        /// <param name="onCashOrderData">On Data Handler</param>
        /// <returns></returns>
        protected virtual async Task<CallResult<UpdateSubscription>> SubscribeToBalanceAndOrdersAsync(BitMaxAccountType cashAccountType, Action<BitMaxSocketSpotBalanceExt> onSpotBalanceData, Action<BitMaxSocketMarginBalanceExt> onMarginBalanceData, Action<BitMaxSocketCashOrderExt> onCashOrderData)
        {
            var internalHandler = new Action<DataEvent<BitMaxSocketAccountResponse<object>>>(data =>
            {
                var data_s = data.Data.Data.ToString();
                if (data.Data.Method == "balance")
                {
                    if (data.Data.AccountType == BitMaxAccountType.Spot)
                    {
                        var balance = JsonConvert.DeserializeObject<BitMaxSocketSpotBalanceExt>(data_s);
                        balance.AccountId = data.Data.AccountId;
                        balance.AccountType = data.Data.AccountType;
                        if (onSpotBalanceData != null) onSpotBalanceData(balance);
                    }
                    else if (data.Data.AccountType == BitMaxAccountType.Margin)
                    {
                        var balance = JsonConvert.DeserializeObject<BitMaxSocketMarginBalanceExt>(data_s);
                        balance.AccountId = data.Data.AccountId;
                        balance.AccountType = data.Data.AccountType;
                        if (onMarginBalanceData != null) onMarginBalanceData(balance);
                    }
                }
                else if (data.Data.Method == "order")
                {
                    var order = JsonConvert.DeserializeObject<BitMaxSocketCashOrderExt>(data_s);
                    order.AccountId = data.Data.AccountId;
                    order.AccountType = data.Data.AccountType;
                    if (onCashOrderData != null) onCashOrderData(order);
                }

                //data.Data.Symbol = data.Symbol;
                //onData(data.Data);
            });

            var request = new BitMaxSocketCashChannelRequest(NextRequestId(), BitMaxSocketCashChannelOperation.Subscribe, "order:" + (cashAccountType == BitMaxAccountType.Spot ? "cash" : "margin"));
            return await SubscribeAsync(request, "order", true, internalHandler).ConfigureAwait(false);
        }

        public virtual CallResult<UpdateSubscription> SubscribeToFuturesMarketData(string symbol, Action<BitMaxSocketFuturesMarketData> onData) => SubscribeToFuturesMarketDataAsync(new List<string> { symbol }, onData).Result;
        public virtual CallResult<UpdateSubscription> SubscribeToFuturesMarketData(IEnumerable<string> symbols, Action<BitMaxSocketFuturesMarketData> onData) => SubscribeToFuturesMarketDataAsync(symbols, onData).Result;
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToFuturesMarketDataAsync(IEnumerable<string> symbols, Action<BitMaxSocketFuturesMarketData> onData)
        {
            var internalHandler = new Action<DataEvent<BitMaxSocketBarChannelResponse<BitMaxSocketFuturesMarketData>>>(data =>
            {
                data.Data.Symbol = data.Data.Symbol;
                onData(data.Data.Data);
            });

            var request = new BitMaxSocketCashChannelRequest(NextRequestId(), BitMaxSocketCashChannelOperation.Subscribe, "futures-market-data:" + string.Join(",", symbols));
            return await SubscribeAsync(request, "futures-market-data", false, internalHandler).ConfigureAwait(false);
        }

        public virtual CallResult<UpdateSubscription> SubscribeToFuturesOrders(Action<BitMaxSocketFuturesOrderExt> onOrderData) => SubscribeToFuturesOrdersAsync(onOrderData).Result;
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToFuturesOrdersAsync(Action<BitMaxSocketFuturesOrderExt> onOrderData)
        {
            var internalHandler = new Action<DataEvent<BitMaxSocketAccountResponse<object>>>(data =>
            {
                var data_s = data.Data.Data.ToString();
                if (data.Data.Method == "order")
                {
                    var order = JsonConvert.DeserializeObject<BitMaxSocketFuturesOrderExt>(data_s);
                    order.AccountId = data.Data.AccountId;
                    order.AccountType = data.Data.AccountType;
                    if (onOrderData != null) onOrderData(order);
                }

                //data.Data.Symbol = data.Symbol;
                //onData(data.Data);
            });

            var request = new BitMaxSocketCashChannelRequest(NextRequestId(), BitMaxSocketCashChannelOperation.Subscribe, "order:futures");
            return await SubscribeAsync(request, "order:futures", true, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Login Method
        /// </summary>
        /// <returns></returns>
        public virtual CallResult<bool> Login() => LoginAsync().Result;
        /// <summary>
        /// Login Method
        /// </summary>
        /// <returns></returns>
        public virtual async Task<CallResult<bool>> LoginAsync()
        {
            if (authProvider == null)
                return new CallResult<bool>(false, new NoApiCredentialsError());

            var ap = (BitMaxAuthenticationProvider)authProvider;
            var key = ap.Credentials.Key.GetString();
            var secret = ap.Credentials.Secret.GetString();
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(secret))
                return new CallResult<bool>(false, new NoApiCredentialsError());

            var id = NextRequestId();
            var request = new BitMaxSocketAuthRequest(id);
            request.Sign(key, secret);

            var result = await QueryAsync<BitMaxSocketAuthResponse>(request, false).ConfigureAwait(true);
            Authendicated = result != null && result.Data != null && result.Data.Code == 0;
            return new CallResult<bool>(Authendicated, Authendicated ? null : new ServerError(result.Data != null ? result.Data.Error : ""));
        }

        #region Private Core Methods
        protected virtual void PingHandler(MessageEvent messageEvent)
        {
            if (messageEvent.JsonData["m"] != null && (string)messageEvent.JsonData["m"] == "ping")
                messageEvent.Connection.Send(new BitMaxSocketPingRequest(NextRequestId()));
        }

        protected long iterator = 0;
        protected virtual long NextRequestId()
        {
            return ++iterator;
        }
        #endregion

        #region Override Methods
        protected override SocketConnection GetSocketConnection(string address, bool authenticated)
        {
            return this.BitMaxSocketConnection(address, authenticated);
        }
        protected virtual SocketConnection BitMaxSocketConnection(string address, bool authenticated)
        {
            address = address.TrimEnd('/');
            var socketResult = sockets.Where(s =>
                s.Value.Socket.Url.TrimEnd('/') == address.TrimEnd('/') &&
                (s.Value.Authenticated == authenticated || !authenticated) &&
                s.Value.Connected).OrderBy(s => s.Value.SubscriptionCount).FirstOrDefault();
            var result = socketResult.Equals(default(KeyValuePair<int, SocketConnection>)) ? null : socketResult.Value;

            if (result != null)
            {
                if (result.SubscriptionCount < SocketCombineTarget || (sockets.Count >= MaxSocketConnections && sockets.All(s => s.Value.SubscriptionCount >= SocketCombineTarget)))
                {
                    // Use existing socket if it has less than target connections OR it has the least connections and we can't make new
                    return result;
                }
            }

            // Create new socket
            var socket = CreateSocket(address);
            var socketConnection = new SocketConnection(this, socket);
            socketConnection.UnhandledMessage += HandleUnhandledMessage;
            foreach (var kvp in genericHandlers)
            {
                var handler = SocketSubscription.CreateForIdentifier(NextId(), kvp.Key, false, kvp.Value);
                socketConnection.AddSubscription(handler);
            }

            return socketConnection;
        }

        protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        {
            return this.BitMaxHandleQueryResponse<T>(s, request, data, out callResult);
        }
        protected virtual bool BitMaxHandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        {
            callResult = new CallResult<T>(default, null);

            if (data["m"] != null && (string)data["m"] == "auth" && data["id"] != null)
            {
                if (request is BitMaxSocketAuthRequest req)
                {
                    var desResult = Deserialize<T>(data, false);
                    if (!desResult)
                    {
                        log.Write(LogLevel.Warning, $"Failed to deserialize data: {desResult.Error}. Data: {data}");
                        callResult = new CallResult<T>(default, desResult.Error);
                        return true;
                    }

                    callResult = new CallResult<T>(desResult.Data, null);
                    return true;
                }
            }

            return false;
        }

        protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object> callResult)
        {
            return this.BitMaxHandleSubscriptionResponse(s, subscription, request, message, out callResult);
        }
        protected virtual bool BitMaxHandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object> callResult)
        {
            callResult = null;

            // Check for Error
            if (message["m"] != null && (string)message["m"]! == "error" && message["id"] != null)
            {
                if (request is BitMaxSocketCashChannelRequest req)
                {
                    if (req.RequestId == (string)message["id"] && message["code"] != null && (int)message["code"] > 0)
                    {
                        var code = (int)message["code"];
                        var reason = ""; if (message["reason"] != null) reason = (string)message["reason"];
                        var info = ""; if (message["info"] != null) info = (string)message["info"];
                        log.Write(LogLevel.Debug, "Subscription failed: " + info);
                        callResult = new CallResult<object>(null, new ServerError(code, $"{reason}: {info}"));
                        return true;
                    }
                }
            }

            // Check for Success
            if (message["m"] != null && (string)message["m"] == "sub" && message["id"] != null && message["ch"] != null)
            {
                if (request is BitMaxSocketCashChannelRequest req)
                {
                    if (req.RequestId == (string)message["id"] && message["code"] != null && (int)message["code"] == 0)
                    {
                        log.Write(LogLevel.Debug, "Subscription completed");
                        callResult = new CallResult<object>(true, null);
                        return true;
                    }
                }
            }

            return false;
        }

        protected override bool MessageMatchesHandler(JToken message, object request)
        {
            return this.BitMaxMessageMatchesHandler(message, request);
        }
        protected virtual bool BitMaxMessageMatchesHandler(JToken message, object request)
        {
            if (request is BitMaxSocketCashChannelRequest req)
            {
                // Check Point
                if (message["m"] == null)
                    return false;

                /* Variables */
                var m = (string)message["m"]!;

                // Private Feeds
                if (req.Operation == BitMaxSocketCashChannelOperation.Subscribe && message["accountId"] != null)
                {
                    if (m == "balance" || m == "order")
                        return true;
                }

                else
                {
                    if (req.Operation != BitMaxSocketCashChannelOperation.Subscribe || (message["s"] == null && message["symbol"] == null))
                        return false;

                    /* Variables */
                    var ac = "";
                    var symbol = "";
                    if (message["ac"] != null) ac = (string)message["ac"];
                    if (message["s"] != null) symbol = (string)message["s"];
                    else if (message["symbol"] != null) symbol = (string)message["symbol"];

                    /* Public */
                    if (m == "summary" && req.Channel.StartsWith("summary"))
                        return true;
                    if (m == "bbo" && req.Channel.StartsWith("bbo") && req.Channel.Contains(symbol))
                        return true;
                    if (m == "depth" && req.Channel.StartsWith("depth") && req.Channel.Contains(symbol))
                        return true;
                    if (m == "trades" && req.Channel.StartsWith("trades") && req.Channel.Contains(symbol))
                        return true;
                    if (m == "bar" && req.Channel.StartsWith("bar") && req.Channel.Contains(symbol))
                        return true;
                    if (m == "futures-market-data" && req.Channel.StartsWith("futures-market-data") && req.Channel.Contains(symbol))
                        return true;
                }
            }

            return false;
        }

        protected override bool MessageMatchesHandler(JToken message, string identifier)
        {
            return this.BitMaxMessageMatchesHandler(message, identifier);
        }
        protected virtual bool BitMaxMessageMatchesHandler(JToken message, string identifier)
        {
            if ((string)message["m"] == "connected")
                return true;

            if ((string)message["m"] == "auth")
                return true;

            if (identifier == "Ping" && (string)message["m"] == "ping")
                return true;

            return false;
        }

        protected override async Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection s)
        {
            return await this.BitMaxAuthenticateSocket(s);
        }
        protected virtual async Task<CallResult<bool>> BitMaxAuthenticateSocket(SocketConnection s)
        {
            if (authProvider == null)
                return new CallResult<bool>(false, new NoApiCredentialsError());

            var ap = (BitMaxAuthenticationProvider)authProvider;
            var key = ap.Credentials.Key.GetString();
            var secret = ap.Credentials.Secret.GetString();
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(secret))
                return new CallResult<bool>(false, new NoApiCredentialsError());

            var id = NextRequestId();
            var request = new BitMaxSocketAuthRequest(id);
            request.Sign(key, secret);

            var result = new CallResult<bool>(false, new ServerError("No response from server"));
            await s.SendAndWaitAsync(request, ResponseTimeout, data =>
            {
                if (data["m"] == null || (string)data["m"] != "auth")
                    return false;

                if (data["code"] == null || (int)data["code"] > 0)
                {
                    var code = (int)data["code"];
                    var err = data["err"] != null ? (string)data["err"] : "";
                    log.Write(LogLevel.Warning, "Authorization failed: " + err);
                    result = new CallResult<bool>(false, new ServerError(code, err));
                    return true;
                }

                log.Write(LogLevel.Debug, "Authorization completed");
                result = new CallResult<bool>(true, null);
                Authendicated = true;
                return true;
            });

            return result;
        }

        protected override async Task<bool> UnsubscribeAsync(SocketConnection connection, SocketSubscription s)
        {
            return await this.BitMaxUnsubscribe(connection, s);
        }
        protected virtual async Task<bool> BitMaxUnsubscribe(SocketConnection connection, SocketSubscription s)
        {
            if (s == null || s.Request == null)
                return false;

            var id = NextRequestId().ToString();
            var request = new BitMaxSocketCashChannelRequest(id, BitMaxSocketCashChannelOperation.Unsubscribe, ((BitMaxSocketCashChannelRequest)s.Request).Channel);
            await connection.SendAndWaitAsync(request, ResponseTimeout, data =>
            {
                if (data.Type != JTokenType.Object)
                    return false;

                if (data["m"] != null && (string)data["m"] == "unsub" && data["id"] != null && (string)data["id"] == id)
                    return data["code"] == null || (int)data["code"] == 0;

                return false;
            });
            return false;
        }

        #endregion

    }
}
