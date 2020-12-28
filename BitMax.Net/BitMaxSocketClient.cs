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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitMax.Net
{
    public partial class BitMaxSocketClient : SocketClient, ISocketClient, IBitMaxSocketClient
    {
        public bool Authendicated { get; private set; }

        #region Client Options
        private static BitMaxSocketClientOptions defaultOptions = new BitMaxSocketClientOptions();
        private static BitMaxSocketClientOptions DefaultOptions => defaultOptions.Copy();
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
        /// You can subscribe to updates of best bid/offer data stream only. Once subscribed, you will receive BBO message whenever the price and/or size changes at the top of the order book.
        /// Each BBO message contains price and size data for exactly one bid level and one ask level.
        /// </summary>
        /// <param name="symbol">Trading Symbol</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public CallResult<UpdateSubscription> SubscribeToBestAskBidUpdates(string symbol, Action<BitMaxSocketBBO> onData) => SubscribeToBestAskBidUpdatesAsync(new List<string> { symbol }, onData).Result;
        /// <summary>
        /// You can subscribe to updates of best bid/offer data stream only. Once subscribed, you will receive BBO message whenever the price and/or size changes at the top of the order book.
        /// Each BBO message contains price and size data for exactly one bid level and one ask level.
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public CallResult<UpdateSubscription> SubscribeToBestAskBidUpdates(IEnumerable<string> symbols, Action<BitMaxSocketBBO> onData) => SubscribeToBestAskBidUpdatesAsync(symbols, onData).Result;
        /// <summary>
        /// You can subscribe to updates of best bid/offer data stream only. Once subscribed, you will receive BBO message whenever the price and/or size changes at the top of the order book.
        /// Each BBO message contains price and size data for exactly one bid level and one ask level.
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToBestAskBidUpdatesAsync(IEnumerable<string> symbols, Action<BitMaxSocketBBO> onData)
        {
            var internalHandler = new Action<BitMaxSocketCashChannelResponse<BitMaxSocketBBO>>(data =>
            {
                data.Data.Symbol = data.Symbol;
                onData(data.Data);
            });

            var request = new BitMaxSocketCashChannelRequest(NextRequestId(), BitMaxSocketCashChannelOperation.Subscribe, "bbo:" + string.Join(",", symbols));
            return await Subscribe(request, "bbo", false, internalHandler).ConfigureAwait(false);
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
        public CallResult<UpdateSubscription> SubscribeToOrderBookUpdates(string symbol, Action<BitMaxSocketOrderBook> onData) => SubscribeToOrderBookUpdatesAsync(new List<string> { symbol }, onData).Result;
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
        public CallResult<UpdateSubscription> SubscribeToOrderBookUpdates(IEnumerable<string> symbols, Action<BitMaxSocketOrderBook> onData) => SubscribeToOrderBookUpdatesAsync(symbols, onData).Result;
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, Action<BitMaxSocketOrderBook> onData)
        {
            var internalHandler = new Action<BitMaxSocketCashChannelResponse<BitMaxSocketOrderBook>>(data =>
            {
                data.Data.Symbol = data.Symbol;
                onData(data.Data);
            });

            var request = new BitMaxSocketCashChannelRequest(NextRequestId(), BitMaxSocketCashChannelOperation.Subscribe, "depth:" + string.Join(",", symbols));
            return await Subscribe(request, "depth", false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// The data field is a list containing one or more trade objects. The server may combine consecutive trades with the same price and bm value into one aggregated item. Each trade object contains the following fields:
        /// </summary>
        /// <param name="symbol">Trading Symbol</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public CallResult<UpdateSubscription> SubscribeToTrades(string symbol, Action<BitMaxSocketTrade> onData) => SubscribeToTradesAsync(new List<string> { symbol }, onData).Result;
        /// <summary>
        /// The data field is a list containing one or more trade objects. The server may combine consecutive trades with the same price and bm value into one aggregated item. Each trade object contains the following fields:
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public CallResult<UpdateSubscription> SubscribeToTrades(IEnumerable<string> symbols, Action<BitMaxSocketTrade> onData) => SubscribeToTradesAsync(symbols, onData).Result;
        /// <summary>
        /// The data field is a list containing one or more trade objects. The server may combine consecutive trades with the same price and bm value into one aggregated item. Each trade object contains the following fields:
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradesAsync(IEnumerable<string> symbols, Action<BitMaxSocketTrade> onData)
        {
            var internalHandler = new Action<BitMaxSocketCashChannelResponse<IEnumerable<BitMaxSocketTrade>>>(data =>
            {
                foreach (var d in data.Data)
                {
                    d.Symbol = data.Symbol;
                    onData(d);
                }
            });

            var request = new BitMaxSocketCashChannelRequest(NextRequestId(), BitMaxSocketCashChannelOperation.Subscribe, "trades:" + string.Join(",", symbols));
            return await Subscribe(request, "trades", false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// The data field is a list containing one or more trade objects. The server may combine consecutive trades with the same price and bm value into one aggregated item. Each trade object contains the following fields:
        /// </summary>
        /// <param name="symbol">Trading Symbol</param>
        /// <param name="period">Candle Period</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public CallResult<UpdateSubscription> SubscribeToCandles(string symbol, BitMaxPeriod period, Action<BitMaxSocketCandle> onData) => SubscribeToCandlesAsync(new List<string> { symbol }, period, onData).Result;
        /// <summary>
        /// The data field is a list containing one or more trade objects. The server may combine consecutive trades with the same price and bm value into one aggregated item. Each trade object contains the following fields:
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="period">Candle Period</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public CallResult<UpdateSubscription> SubscribeToCandles(IEnumerable<string> symbols, BitMaxPeriod period, Action<BitMaxSocketCandle> onData) => SubscribeToCandlesAsync(symbols, period, onData).Result;
        /// <summary>
        /// The data field is a list containing one or more trade objects. The server may combine consecutive trades with the same price and bm value into one aggregated item. Each trade object contains the following fields:
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="period">Candle Period</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToCandlesAsync(IEnumerable<string> symbols, BitMaxPeriod period, Action<BitMaxSocketCandle> onData)
        {
            var internalHandler = new Action<BitMaxSocketCashBarChannelResponse<BitMaxSocketCandle>>(data =>
            {
                data.Data.Symbol = data.Symbol;
                onData(data.Data);
            });

            var period_s = JsonConvert.SerializeObject(period, new PeriodConverter(false));
            var request = new BitMaxSocketCashChannelRequest(NextRequestId(), BitMaxSocketCashChannelOperation.Subscribe, "bar:" + period_s + ":" + string.Join(",", symbols));
            return await Subscribe(request, "bar", false, internalHandler).ConfigureAwait(false);
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
        public CallResult<UpdateSubscription> SubscribeToSpotBalanceAndOrders(Action<BitMaxSocketSpotBalanceExt> onSpotBalanceData, Action<BitMaxSocketCashOrderExt> onOrderData) => SubscribeToBalanceAndOrdersAsync(BitMaxCashAccountType.Spot, onSpotBalanceData, null, onOrderData).Result;
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
        public CallResult<UpdateSubscription> SubscribeToMarginBalanceAndOrders(Action<BitMaxSocketMarginBalanceExt> onMarginBalanceData, Action<BitMaxSocketCashOrderExt> onOrderData) => SubscribeToBalanceAndOrdersAsync(BitMaxCashAccountType.Margin, null, onMarginBalanceData, onOrderData).Result;
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToSpotBalanceAndOrdersAsync(Action<BitMaxSocketSpotBalanceExt> onSpotBalanceData, Action<BitMaxSocketCashOrderExt> onOrderData) => await SubscribeToBalanceAndOrdersAsync(BitMaxCashAccountType.Spot, onSpotBalanceData, null, onOrderData);
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
        public async Task<CallResult<UpdateSubscription>> SubscribeToMarginBalanceAndOrdersAsync(Action<BitMaxSocketMarginBalanceExt> onMarginBalanceData, Action<BitMaxSocketCashOrderExt> onOrderData) => await SubscribeToBalanceAndOrdersAsync(BitMaxCashAccountType.Margin, null, onMarginBalanceData, onOrderData);
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
        /// <param name="onOrderData">On Data Handler</param>
        /// <returns></returns>
        private async Task<CallResult<UpdateSubscription>> SubscribeToBalanceAndOrdersAsync(BitMaxCashAccountType cashAccountType, Action<BitMaxSocketSpotBalanceExt> onSpotBalanceData, Action<BitMaxSocketMarginBalanceExt> onMarginBalanceData, Action<BitMaxSocketCashOrderExt> onOrderData)
        {
            var internalHandler = new Action<BitMaxSocketAccountResponse<object>>(data =>
            {
                var data_s = data.Data.ToString();
                if (data.Method == "balance")
                {
                    if (data.AccountType == BitMaxCashAccountType.Spot)
                    {
                        var balance = JsonConvert.DeserializeObject<BitMaxSocketSpotBalanceExt>(data_s);
                        balance.AccountId = data.AccountId;
                        balance.AccountType = data.AccountType;
                        if (onSpotBalanceData != null) onSpotBalanceData(balance);
                    }
                    else if (data.AccountType == BitMaxCashAccountType.Margin)
                    {
                        var balance = JsonConvert.DeserializeObject<BitMaxSocketMarginBalanceExt>(data_s);
                        balance.AccountId = data.AccountId;
                        balance.AccountType = data.AccountType;
                        if (onMarginBalanceData != null) onMarginBalanceData(balance);
                    }
                }
                else if (data.Method == "order")
                {
                    var order = JsonConvert.DeserializeObject<BitMaxSocketCashOrderExt>(data_s);
                    order.AccountId = data.AccountId;
                    order.AccountType = data.AccountType;
                    if (onSpotBalanceData != null) onOrderData(order);
                }

                //data.Data.Symbol = data.Symbol;
                //onData(data.Data);
            });

            var request = new BitMaxSocketCashChannelRequest(NextRequestId(), BitMaxSocketCashChannelOperation.Subscribe, "order:" + (cashAccountType == BitMaxCashAccountType.Spot ? "cash" : "margin"));
            return await Subscribe(request, "order", true, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Login Method
        /// </summary>
        /// <returns></returns>
        public CallResult<bool> Login() => LoginAsync().Result;
        /// <summary>
        /// Login Method
        /// </summary>
        /// <returns></returns>
        public async Task<CallResult<bool>> LoginAsync()
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

            var result = await Query<BitMaxSocketCashAuthResponse>(request, false).ConfigureAwait(true);
            Authendicated = result != null && result.Data != null && result.Data.Code == 0;
            return new CallResult<bool>(Authendicated, Authendicated ? null : new ServerError(result.Data != null ? result.Data.Error : ""));
        }

        #region Core Methods
        private void PingHandler(SocketConnection connection, JToken data)
        {
            if (data["m"] != null && (string)data["m"] == "ping")
                connection.Send(new BitMaxSocketPingRequest(NextRequestId()));
        }

        protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        {
            callResult = new CallResult<T>(default, null);

            if (data["m"] != null && (string)data["m"] == "auth" && data["id"] != null)
            {
                if (request is BitMaxSocketAuthRequest req)
                {
                    var desResult = Deserialize<T>(data, false);
                    if (!desResult)
                    {
                        log.Write(LogVerbosity.Warning, $"Failed to deserialize data: {desResult.Error}. Data: {data}");
                        callResult = new CallResult<T>(default, desResult.Error);
                        return true;
                    }

                    callResult = new CallResult<T>(desResult.Data, null);
                    return true;
                }
            }

            return true;
        }

        protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object> callResult)
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
                        log.Write(LogVerbosity.Debug, "Subscription failed: " + info);
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
                        log.Write(LogVerbosity.Debug, "Subscription completed");
                        callResult = new CallResult<object>(true, null);
                        return true;
                    }
                }
            }

            return false;
        }

        protected override bool MessageMatchesHandler(JToken message, object request)
        {
            if (request is BitMaxSocketCashChannelRequest req)
            {
                // Check Point
                if (message["m"] == null)
                    return false;

                /* Variables */
                var m = (string)message["m"]!;

                // Private Feeds
                if (req.Operation == BitMaxSocketCashChannelOperation.Subscribe || message["accountId"] != null)
                {
                    if (m == "balance" || m == "order")
                        return true;
                }

                else
                {
                    if (req.Operation != BitMaxSocketCashChannelOperation.Subscribe || (message["s"] == null && message["symbol"] == null))
                        return false;

                    /* Variables */
                    var ac = ""; if (message["ac"] != null) ac = (string)message["ac"];
                    var symbol = "";
                    if (message["symbol"] != null) symbol = (string)message["symbol"];
                    else if (message["s"] != null) symbol = (string)message["s"];

                    /* Public */
                    if (m == "bbo" && req.Channel.StartsWith("bbo") && req.Channel.Contains(symbol))
                        return true;
                    if (m == "depth" && req.Channel.StartsWith("depth") && req.Channel.Contains(symbol))
                        return true;
                    if (m == "trades" && req.Channel.StartsWith("trades") && req.Channel.Contains(symbol))
                        return true;
                    if (m == "bar" && req.Channel.StartsWith("bar") && req.Channel.Contains(symbol))
                        return true;
                }
            }

            return false;
        }

        protected override bool MessageMatchesHandler(JToken message, string identifier)
        {
            if ((string)message["m"] == "connected")
                return true;

            if ((string)message["m"] == "auth")
                return true;

            if (identifier == "Ping" && (string)message["m"] == "ping")
                return true;

            return false;
        }

        protected override async Task<CallResult<bool>> AuthenticateSocket(SocketConnection s)
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
            await s.SendAndWait(request, ResponseTimeout, data =>
            {
                if (data["m"] == null || (string)data["m"] != "auth")
                    return false;

                if (data["code"] == null || (int)data["code"] > 0)
                {
                    var code = (int)data["code"];
                    var err = data["err"] != null ? (string)data["err"] : "";
                    log.Write(LogVerbosity.Warning, "Authorization failed: " + err);
                    result = new CallResult<bool>(false, new ServerError(code, err));
                    return true;
                }

                log.Write(LogVerbosity.Debug, "Authorization completed");
                result = new CallResult<bool>(true, null);
                Authendicated = true;
                return true;
            });

            return result;
        }

        protected override async Task<bool> Unsubscribe(SocketConnection connection, SocketSubscription s)
        {
            if (s == null || s.Request == null)
                return false;

            var id = NextRequestId().ToString();
            var request = new BitMaxSocketCashChannelRequest(id, BitMaxSocketCashChannelOperation.Unsubscribe, ((BitMaxSocketCashChannelRequest)s.Request).Channel);
            await connection.SendAndWait(request, ResponseTimeout, data =>
            {
                if (data.Type != JTokenType.Object)
                    return false;

                if (data["m"] != null && (string)data["m"] == "unsub" && data["id"] != null && (string)data["id"] == id)
                    return data["code"] == null || (int)data["code"] == 0;

                return false;
            });
            return false;
        }

        private long iterator = 0;
        protected long NextRequestId()
        {
            return ++iterator;
        }

        #endregion

    }
}
