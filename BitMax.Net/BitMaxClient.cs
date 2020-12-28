using BitMax.Net.Converters;
using BitMax.Net.CoreObjects;
using BitMax.Net.Enums;
using BitMax.Net.Helpers;
using BitMax.Net.Interfaces;
using BitMax.Net.RestObjects;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BitMax.Net
{
    public class BitMaxClient : RestClient, IRestClient, IBitMaxClient
    {
        public int AccountGroup { get; set; } = 1;

        #region Fields
        private static BitMaxClientOptions defaultOptions = new BitMaxClientOptions();
        private static BitMaxClientOptions DefaultOptions => defaultOptions.Copy();

        /* Spot/Margin API Endpoints */
        private const string Endpoints_Cash_MarketData_Assets = "/api/pro/v1/assets";
        private const string Endpoints_Cash_MarketData_Products = "/api/pro/v1/products";
        private const string Endpoints_Cash_MarketData_Ticker = "/api/pro/v1/ticker";
        private const string Endpoints_Cash_MarketData_BarInfo = "/api/pro/v1/barhist/info";
        private const string Endpoints_Cash_MarketData_Bars = "/api/pro/v1/barhist";
        private const string Endpoints_Cash_MarketData_Depth = "/api/pro/v1/depth";
        private const string Endpoints_Cash_MarketData_Trades = "/api/pro/v1/trades";

        private const string Endpoints_Cash_Account_Info = "/api/pro/v1/info";

        private const string Endpoints_Cash_Balance_CashBalance = "/<account-group>/api/pro/v1/cash/balance";
        private const string Endpoints_Cash_Balance_MarginBalance = "/<account-group>/api/pro/v1/margin/balance";
        private const string Endpoints_Cash_Balance_MarginRisk = "/<account-group>/api/pro/v1/margin/risk";
        private const string Endpoints_Cash_Balance_Transfer = "/<account-group>/api/pro/v1/transfer";

        private const string Endpoints_Cash_Wallet_DepositAddresses = "/api/pro/v1/wallet/deposit/address";
        private const string Endpoints_Cash_Wallet_Transactions = "/api/pro/v1/wallet/transactions";

        private const string Endpoints_Cash_Order_PlaceOrder = "/<account-group>/api/pro/v1/{account-category}/order";
        private const string Endpoints_Cash_Order_CancelOrder = "/<account-group>/api/pro/v1/{account-category}/order";
        private const string Endpoints_Cash_Order_CancelAllOrders = "/<account-group>/api/pro/v1/{account-category}/order/all";
        private const string Endpoints_Cash_Order_PlaceBatchOrders = "/<account-group>/api/pro/v1/{account-category}/order/batch";
        private const string Endpoints_Cash_Order_CancelBatchOrders = "/<account-group>/api/pro/v1/{account-category}/order/batch";
        private const string Endpoints_Cash_Order_Query = "/<account-group>/api/pro/v1/{account-category}/order/status";
        private const string Endpoints_Cash_Order_OpenOrders = "/<account-group>/api/pro/v1/{account-category}/order/open";
        private const string Endpoints_Cash_Order_CurrentHistoryOrders = "/<account-group>/api/pro/v1/{account-category}/order/hist/current";
        private const string Endpoints_Cash_Order_HistoryOrders = "/<account-group>/api/pro/v2/order/hist";

        /* Futures API Endpoints */
        private const string Endpoints_Futures_MarketData_CollateralAssets = "/api/pro/v1/futures/collateral";
        private const string Endpoints_Futures_MarketData_Contracts = "/api/pro/v1/futures/contracts";
        private const string Endpoints_Futures_MarketData_ReferencePrices = "/api/pro/v1/futures/ref-px";
        private const string Endpoints_Futures_MarketData_MarketData = "/api/pro/v1/futures/market-data";
        private const string Endpoints_Futures_MarketData_FundingRates = "/api/pro/v1/futures/funding-rates";

        private const string Endpoints_Futures_Balance_CollateralBalance = "/<account-group>/api/pro/v1/futures/collateral-balance";
        private const string Endpoints_Futures_Balance_ContractPosition = "/<account-group>/api/pro/v1/futures/position";
        private const string Endpoints_Futures_Balance_AccountRisk = "/<account-group>/api/pro/v1/futures/risk";
        private const string Endpoints_Futures_Balance_FundingPaymentHistory = "/<account-group>/api/pro/v1/futures/funding-payments";

        private const string Endpoints_Futures_Wallet_TransferFromCashToFuturesAccount = "/<account-group>/api/pro/v1/futures/transfer/deposit";
        private const string Endpoints_Futures_Wallet_TransferFromFuturesToCashAccount = "/<account-group>/api/pro/v1/futures/transfer/withdraw";

        private const string Endpoints_Futures_Order_PlaceOrder = "/<account-group>/api/pro/v1/futures/order";
        private const string Endpoints_Futures_Order_CancelOrder = "/<account-group>/api/pro/v1/futures/order";
        private const string Endpoints_Futures_Order_CancelAllOrders = "/<account-group>/api/pro/v1/futures/order/all";
        private const string Endpoints_Futures_Order_PlaceBatchOrders = "/<account-group>/api/pro/v1/futures/order/batch";
        private const string Endpoints_Futures_Order_CancelBatchOrders = "/<account-group>/api/pro/v1/futures/order/batch";
        private const string Endpoints_Futures_Order_Query = "/<account-group>/api/pro/v1/futures/order/status";
        private const string Endpoints_Futures_Order_OpenOrders = "/<account-group>/api/pro/v1/futures/order/open";
        private const string Endpoints_Futures_Order_CurrentHistoryOrders = "/<account-group>/api/pro/v1/futures/order/hist/current";
        // private const string Endpoints_Futures_Order_HistoryOrders = "/<account-group>/api/pro/v2/order/hist";

        /* Prehash Exceptions */
        private readonly Dictionary<string, string> Endpoints_Prehash_Exceptions = new Dictionary<string, string>
        {
            { "api/pro/v1/margin/risk", "margin/risk" },
            { "api/pro/v1/wallet/deposit/address", "wallet/deposit/address" },
            { "api/pro/v1/wallet/transactions", "wallet/transactions" },
            { "api/pro/v1/cash/order/all", "order/all" },
            { "api/pro/v1/margin/order/all", "order/all" },
            { "api/pro/v1/cash/order/batch", "order/batch" },
            { "api/pro/v1/margin/order/batch", "order/batch" },
            { "api/pro/v1/cash/order/status", "order/status" },
            { "api/pro/v1/margin/order/status", "order/status" },
            { "api/pro/v1/cash/order/open", "order/open" },
            { "api/pro/v1/margin/order/open", "order/open" },
            { "api/pro/v1/cash/order/hist/current", "order/hist/current" },
            { "api/pro/v1/margin/order/hist/current", "order/hist/current" },
            { "api/pro/v2/order/hist", "order/hist" },

            { "api/pro/v1/futures/collateral-balance", "futures/collateral-balance" },
            { "api/pro/v1/futures/position", "futures/position" },
            { "api/pro/v1/futures/risk", "futures/risk" },
            { "api/pro/v1/futures/funding-payments", "futures/funding-payments" },
            { "api/pro/v1/futures/transfer/deposit", "futures/transfer/deposit" },
            { "api/pro/v1/futures/transfer/withdraw", "futures/transfer/withdraw" },
            { "api/pro/v1/futures/order/all", "order/all" },
            { "api/pro/v1/futures/order/batch", "order/batch" },
            { "api/pro/v1/futures/order/status", "order/status" },
            { "api/pro/v1/futures/order/open", "order/open" },
            { "api/pro/v1/futures/order/hist/current", "order/hist/current" },
        };

        #endregion

        #region Constructor / Destructor
        /// <summary>
        /// Create a new instance of BitMaxClient using the default options
        /// </summary>
        public BitMaxClient() : this(DefaultOptions)
        {
            requestBodyFormat = RequestBodyFormat.Json;
        }

        /// <summary>
        /// Create a new instance of the BitMaxClient with the provided options
        /// </summary>
        public BitMaxClient(BitMaxClientOptions options) : base("BitMax", options, options.ApiCredentials == null ? null : new BitMaxAuthenticationProvider(options.ApiCredentials, ArrayParametersSerialization.MultipleValues))
        {
            requestBodyFormat = RequestBodyFormat.Json;
            arraySerialization = ArrayParametersSerialization.MultipleValues;
        }
        #endregion

        #region Core Methods
        /// <summary>
        /// Sets the default options to use for new clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
        public static void SetDefaultOptions(BitMaxClientOptions options)
        {
            defaultOptions = options;
        }

        /// <summary>
        /// Set the API key and secret
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        public void SetApiCredentials(string apiKey, string apiSecret)
        {
            SetAuthenticationProvider(new BitMaxAuthenticationProvider(new ApiCredentials(apiKey, apiSecret), ArrayParametersSerialization.MultipleValues));
        }

        public void SetAccountGroup(int account_group)
        {
            AccountGroup = account_group;
        }
        #endregion

        #region Spot / Margin Api Methods
        /// <summary>
        /// You can obtain a list of all assets listed on the exchange through this API.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxAsset>> GetAssets(CancellationToken ct = default) => GetAssetsAsync(ct).Result;
        /// <summary>
        /// You can obtain a list of all assets listed on the exchange through this API.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxAsset>>> GetAssetsAsync(CancellationToken ct = default)
        {
            var result = await SendRequest<BitMaxApiResponse<IEnumerable<BitMaxAsset>>>(GetUrl(Endpoints_Cash_MarketData_Assets), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: false).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<BitMaxAsset>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<IEnumerable<BitMaxAsset>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<IEnumerable<BitMaxAsset>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// You can obtain a list of all product listed on the exchange through this API.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxProduct>> GetProducts(CancellationToken ct = default) => GetProductsAsync(ct).Result;
        /// <summary>
        /// You can obtain a list of all product listed on the exchange through this API.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxProduct>>> GetProductsAsync(CancellationToken ct = default)
        {
            var result = await SendRequest<BitMaxApiResponse<IEnumerable<BitMaxProduct>>>(GetUrl(Endpoints_Cash_MarketData_Products), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: false).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<BitMaxProduct>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<IEnumerable<BitMaxProduct>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<IEnumerable<BitMaxProduct>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// You can get summary statistics of one or multiple symbols with this API.
        /// This API endpoint accepts one optional string field symbol:
        /// - If you do not specify symbol, the API will responde with tickers of all symbols in a list.
        /// - If you set symbol to be a single symbol, such as BTMX/USDT, the API will respond with the ticker of the target symbol as an object. If you want to wrap the object in a one-element list, append a comma to the symbol, e.g.BTMX/USDT,
        /// - You shall specify symbol as a comma separated symbol list, e.g.BTMX/USDT, BTC/USDT.The API will respond with a list of tickers.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxTicker>> GetTickers(CancellationToken ct = default) => GetTickersAsync(new List<string> { }, ct).Result;
        /// <summary>
        /// You can get summary statistics of one or multiple symbols with this API.
        /// This API endpoint accepts one optional string field symbol:
        /// - If you do not specify symbol, the API will responde with tickers of all symbols in a list.
        /// - If you set symbol to be a single symbol, such as BTMX/USDT, the API will respond with the ticker of the target symbol as an object. If you want to wrap the object in a one-element list, append a comma to the symbol, e.g.BTMX/USDT,
        /// - You shall specify symbol as a comma separated symbol list, e.g.BTMX/USDT, BTC/USDT.The API will respond with a list of tickers.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxTicker>>> GetTickersAsync(CancellationToken ct = default) => await GetTickersAsync(new List<string> { }, ct);
        /// <summary>
        /// You can get summary statistics of one or multiple symbols with this API.
        /// This API endpoint accepts one optional string field symbol:
        /// - If you do not specify symbol, the API will responde with tickers of all symbols in a list.
        /// - If you set symbol to be a single symbol, such as BTMX/USDT, the API will respond with the ticker of the target symbol as an object. If you want to wrap the object in a one-element list, append a comma to the symbol, e.g.BTMX/USDT,
        /// - You shall specify symbol as a comma separated symbol list, e.g.BTMX/USDT, BTC/USDT.The API will respond with a list of tickers.
        /// </summary>
        /// <param name="symbol">Trading Symbol</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxTicker>> GetTickers(string symbol, CancellationToken ct = default) => GetTickersAsync(new List<string> { symbol }, ct).Result;
        /// <summary>
        /// You can get summary statistics of one or multiple symbols with this API.
        /// This API endpoint accepts one optional string field symbol:
        /// - If you do not specify symbol, the API will responde with tickers of all symbols in a list.
        /// - If you set symbol to be a single symbol, such as BTMX/USDT, the API will respond with the ticker of the target symbol as an object. If you want to wrap the object in a one-element list, append a comma to the symbol, e.g.BTMX/USDT,
        /// - You shall specify symbol as a comma separated symbol list, e.g.BTMX/USDT, BTC/USDT.The API will respond with a list of tickers.
        /// </summary>
        /// <param name="symbol">Trading Symbol</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxTicker>>> GetTickersAsync(string symbol, CancellationToken ct = default) => await GetTickersAsync(new List<string> { symbol }, ct);
        /// <summary>
        /// You can get summary statistics of one or multiple symbols with this API.
        /// This API endpoint accepts one optional string field symbol:
        /// - If you do not specify symbol, the API will responde with tickers of all symbols in a list.
        /// - If you set symbol to be a single symbol, such as BTMX/USDT, the API will respond with the ticker of the target symbol as an object. If you want to wrap the object in a one-element list, append a comma to the symbol, e.g.BTMX/USDT,
        /// - You shall specify symbol as a comma separated symbol list, e.g.BTMX/USDT, BTC/USDT.The API will respond with a list of tickers.
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxTicker>> GetTickers(params string[] symbols) => GetTickersAsync(symbols, default).Result;
        /// <summary>
        /// You can get summary statistics of one or multiple symbols with this API.
        /// This API endpoint accepts one optional string field symbol:
        /// - If you do not specify symbol, the API will responde with tickers of all symbols in a list.
        /// - If you set symbol to be a single symbol, such as BTMX/USDT, the API will respond with the ticker of the target symbol as an object. If you want to wrap the object in a one-element list, append a comma to the symbol, e.g.BTMX/USDT,
        /// - You shall specify symbol as a comma separated symbol list, e.g.BTMX/USDT, BTC/USDT.The API will respond with a list of tickers.
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxTicker>>> GetTickersAsync(params string[] symbols) => await GetTickersAsync(symbols, default);
        /// <summary>
        /// You can get summary statistics of one or multiple symbols with this API.
        /// This API endpoint accepts one optional string field symbol:
        /// - If you do not specify symbol, the API will responde with tickers of all symbols in a list.
        /// - If you set symbol to be a single symbol, such as BTMX/USDT, the API will respond with the ticker of the target symbol as an object. If you want to wrap the object in a one-element list, append a comma to the symbol, e.g.BTMX/USDT,
        /// - You shall specify symbol as a comma separated symbol list, e.g.BTMX/USDT, BTC/USDT.The API will respond with a list of tickers.
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxTicker>> GetTickers(IEnumerable<string> symbols, CancellationToken ct = default) => GetTickersAsync(symbols, ct).Result;
        /// <summary>
        /// You can get summary statistics of one or multiple symbols with this API.
        /// This API endpoint accepts one optional string field symbol:
        /// - If you do not specify symbol, the API will responde with tickers of all symbols in a list.
        /// - If you set symbol to be a single symbol, such as BTMX/USDT, the API will respond with the ticker of the target symbol as an object. If you want to wrap the object in a one-element list, append a comma to the symbol, e.g.BTMX/USDT,
        /// - You shall specify symbol as a comma separated symbol list, e.g.BTMX/USDT, BTC/USDT.The API will respond with a list of tickers.
        /// </summary>
        /// <param name="symbols">Trading Symbols List</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxTicker>>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            if (symbols != null && symbols.Count() > 0) parameters.Add("symbol", string.Join(",", symbols) + ","); // Comma Suffix Trick

            var result = await SendRequest<BitMaxApiResponse<IEnumerable<BitMaxTicker>>>(GetUrl(Endpoints_Cash_MarketData_Ticker), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: false, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<BitMaxTicker>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<IEnumerable<BitMaxTicker>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<IEnumerable<BitMaxTicker>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// This API returns a list of all bar intervals supported by the server.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxBarPeriod>> GetBarPeriods(CancellationToken ct = default) => GetBarPeriodsAsync(ct).Result;
        /// <summary>
        /// This API returns a list of all bar intervals supported by the server.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxBarPeriod>>> GetBarPeriodsAsync(CancellationToken ct = default)
        {
            var result = await SendRequest<BitMaxApiResponse<IEnumerable<BitMaxBarPeriod>>>(GetUrl(Endpoints_Cash_MarketData_BarInfo), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: false).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<BitMaxBarPeriod>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<IEnumerable<BitMaxBarPeriod>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<IEnumerable<BitMaxBarPeriod>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// This API returns a list of bars, with each contains the open/close/high/low prices of a symbol for a specific time range.
        /// The requested time range is determined by three parameters - to, from, and n - according to rules below:
        /// - from/to each specifies the start timestamp of the first/last bar.
        /// - to is always honored. If not provided, this field will be set to the current system time.
        /// - For from and to:
        ///   - if only from is provided, then the request range is determined by [from, to], inclusive. However, if the range is too wide, the server will increase from so the number of bars in the response won't exceed 500.
        ///   - if only n is provided, then the server will return the most recent n data bars to time to. However, if n is greater than 500, only 500 bars will be returned.
        ///   - if both from and n are specified, the server will pick one that returns fewer bars.
        /// </summary>
        /// <param name="symbol">e.g. "BTMX/USDT"</param>
        /// <param name="period">a string representing the interval type.</param>
        /// <param name="limit">default 10, number of bars to be returned, this number will be capped at 500</param>
        /// <param name="from">UTC timestamp in milliseconds.</param>
        /// <param name="to">UTC timestamp in milliseconds. If not provided, this field will be set to the current time.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxCandleSerie>> GetCandles(string symbol, BitMaxPeriod period, int limit = 10, DateTime? from = null, DateTime? to = null, CancellationToken ct = default) => GetCandlesAsync(symbol, period, limit, from, to, ct).Result;
        /// <summary>
        /// This API returns a list of bars, with each contains the open/close/high/low prices of a symbol for a specific time range.
        /// The requested time range is determined by three parameters - to, from, and n - according to rules below:
        /// - from/to each specifies the start timestamp of the first/last bar.
        /// - to is always honored. If not provided, this field will be set to the current system time.
        /// - For from and to:
        ///   - if only from is provided, then the request range is determined by [from, to], inclusive. However, if the range is too wide, the server will increase from so the number of bars in the response won't exceed 500.
        ///   - if only n is provided, then the server will return the most recent n data bars to time to. However, if n is greater than 500, only 500 bars will be returned.
        ///   - if both from and n are specified, the server will pick one that returns fewer bars.
        /// </summary>
        /// <param name="symbol">e.g. "BTMX/USDT"</param>
        /// <param name="period">a string representing the interval type.</param>
        /// <param name="limit">default 10, number of bars to be returned, this number will be capped at 500</param>
        /// <param name="from">UTC timestamp in milliseconds.</param>
        /// <param name="to">UTC timestamp in milliseconds. If not provided, this field will be set to the current time.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxCandleSerie>>> GetCandlesAsync(string symbol, BitMaxPeriod period, int limit = 10, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            limit.ValidateIntBetween(nameof(limit), 1, 500);

            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "interval", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
                { "n", limit },
            };
            if (from.HasValue) parameters.AddOptionalParameter("from", from.Value.ToUnixTimeMilliseconds());
            if (to.HasValue) parameters.AddOptionalParameter("to", to.Value.ToUnixTimeMilliseconds());

            var result = await SendRequest<BitMaxApiResponse<IEnumerable<BitMaxCandleSerie>>>(GetUrl(Endpoints_Cash_MarketData_Bars), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: false, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<BitMaxCandleSerie>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<IEnumerable<BitMaxCandleSerie>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<IEnumerable<BitMaxCandleSerie>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// Gets order book
        /// </summary>
        /// <param name="symbol">e.g. "BTMX/USDT"</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitMaxSerie<BitMaxOrderBook>> GetOrderBook(string symbol, CancellationToken ct = default) => GetOrderBookAsync(symbol, ct).Result;
        /// <summary>
        /// Gets order book
        /// </summary>
        /// <param name="symbol">e.g. "BTMX/USDT"</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitMaxSerie<BitMaxOrderBook>>> GetOrderBookAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
            };

            var result = await SendRequest<BitMaxApiResponse<BitMaxSerie<BitMaxOrderBook>>>(GetUrl(Endpoints_Cash_MarketData_Depth), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: false, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<BitMaxSerie<BitMaxOrderBook>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<BitMaxSerie<BitMaxOrderBook>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<BitMaxSerie<BitMaxOrderBook>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// Gets recent trades
        /// </summary>
        /// <param name="symbol">Valid symbol supported by exchange</param>
        /// <param name="limit">any positive integer, capped at 100</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitMaxSerie<IEnumerable<BitMaxTrade>>> GetTrades(string symbol, int limit = 100, CancellationToken ct = default) => GetTradesAsync(symbol, limit, ct).Result;
        /// <summary>
        /// Gets recent trades
        /// </summary>
        /// <param name="symbol">Valid symbol supported by exchange</param>
        /// <param name="limit">any positive integer, capped at 100</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitMaxSerie<IEnumerable<BitMaxTrade>>>> GetTradesAsync(string symbol, int limit = 100, CancellationToken ct = default)
        {
            limit.ValidateIntBetween(nameof(limit), 1, 100);

            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "n", limit },
            };

            var result = await SendRequest<BitMaxApiResponse<BitMaxSerie<IEnumerable<BitMaxTrade>>>>(GetUrl(Endpoints_Cash_MarketData_Trades), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: false, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<BitMaxSerie<IEnumerable<BitMaxTrade>>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<BitMaxSerie<IEnumerable<BitMaxTrade>>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<BitMaxSerie<IEnumerable<BitMaxTrade>>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// Gets account information
        /// </summary>
        /// <param name="setAccountGroup">Sets BitMaxClient.AccountGroup after data</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitMaxAccountInfo> GetAccountInfo(bool setAccountGroup = false, CancellationToken ct = default) => GetAccountInfoAsync(setAccountGroup, ct).Result;
        /// <summary>
        /// Gets account information
        /// </summary>
        /// <param name="setAccountGroup">Sets BitMaxClient.AccountGroup after data</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitMaxAccountInfo>> GetAccountInfoAsync(bool setAccountGroup = false, CancellationToken ct = default)
        {
            var result = await SendRequest<BitMaxApiResponse<BitMaxAccountInfo>>(GetUrl(Endpoints_Cash_Account_Info), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: true).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<BitMaxAccountInfo>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<BitMaxAccountInfo>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            if (setAccountGroup) SetAccountGroup(result.Data.Data.AccountGroup);

            return new WebCallResult<BitMaxAccountInfo>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// Gets Spot Balances
        /// </summary>
        /// <param name="asset">this allow you query single asset balance, e.g. BTC</param>
        /// <param name="showAll">by default, the API will only respond with assets with non-zero balances. Set showAll=true to include all assets in the response.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxCashBalance>> GetCashBalances(string asset = null, bool showAll = false, CancellationToken ct = default) => GetCashBalancesAsync(asset, showAll, ct).Result;
        /// <summary>
        /// Gets Spot Balances
        /// </summary>
        /// <param name="asset">this allow you query single asset balance, e.g. BTC</param>
        /// <param name="showAll">by default, the API will only respond with assets with non-zero balances. Set showAll=true to include all assets in the response.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxCashBalance>>> GetCashBalancesAsync(string asset = null, bool showAll = false, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "showAll", showAll },
            };
            parameters.AddOptionalParameter("asset", asset);

            var result = await SendRequest<BitMaxApiResponse<IEnumerable<BitMaxCashBalance>>>(GetUrl(Endpoints_Cash_Balance_CashBalance), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: true, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<BitMaxCashBalance>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<IEnumerable<BitMaxCashBalance>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<IEnumerable<BitMaxCashBalance>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// Gets Margin Balances
        /// </summary>
        /// <param name="asset">this allow you query single asset balance, e.g. BTC</param>
        /// <param name="showAll">by default, the API will only respond with assets with non-zero balances. Set showAll=true to include all assets in the response.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxMarginBalance>> GetMarginBalances(string asset = null, bool showAll = false, CancellationToken ct = default) => GetMarginBalancesAsync(asset, showAll, ct).Result;
        /// <summary>
        /// Gets Margin Balances
        /// </summary>
        /// <param name="asset">this allow you query single asset balance, e.g. BTC</param>
        /// <param name="showAll">by default, the API will only respond with assets with non-zero balances. Set showAll=true to include all assets in the response.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxMarginBalance>>> GetMarginBalancesAsync(string asset = null, bool showAll = false, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "showAll", showAll },
            };
            parameters.AddOptionalParameter("asset", asset);

            var result = await SendRequest<BitMaxApiResponse<IEnumerable<BitMaxMarginBalance>>>(GetUrl(Endpoints_Cash_Balance_MarginBalance), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: true, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<BitMaxMarginBalance>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<IEnumerable<BitMaxMarginBalance>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<IEnumerable<BitMaxMarginBalance>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// Gets Margin Risk Profile
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitMaxMarginRisk> GetMarginRisk(CancellationToken ct = default) => GetMarginRiskAsync(ct).Result;
        /// <summary>
        /// Gets Margin Risk Profile
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitMaxMarginRisk>> GetMarginRiskAsync(CancellationToken ct = default)
        {
            var result = await SendRequest<BitMaxApiResponse<BitMaxMarginRisk>>(GetUrl(Endpoints_Cash_Balance_MarginRisk), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: true).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<BitMaxMarginRisk>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<BitMaxMarginRisk>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<BitMaxMarginRisk>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// This api allows balance transfer between different accounts of the same user.
        /// </summary>
        /// <param name="from">cash/margin/futures</param>
        /// <param name="to">cash/margin/futures</param>
        /// <param name="asset">Valid asset code</param>
        /// <param name="amount">Positive numerical string</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<bool> AccountTransfer(BitMaxWalletAccount from, BitMaxWalletAccount to, string asset, decimal amount, CancellationToken ct = default) => AccountTransferAsync(from, to, asset, amount, ct).Result;
        /// <summary>
        /// This api allows balance transfer between different accounts of the same user.
        /// </summary>
        /// <param name="from">cash/margin/futures</param>
        /// <param name="to">cash/margin/futures</param>
        /// <param name="asset">Valid asset code</param>
        /// <param name="amount">Positive numerical string</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<bool>> AccountTransferAsync(BitMaxWalletAccount from, BitMaxWalletAccount to, string asset, decimal amount, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
                { "amount", amount.ToString() },
                { "fromAccount", JsonConvert.SerializeObject(from, new WalletAccountConverter(false)) },
                { "toAccount", JsonConvert.SerializeObject(to, new WalletAccountConverter(false)) },
            };

            var result = await SendRequest<BitMaxApiResponse<bool>>(GetUrl(Endpoints_Cash_Balance_Transfer), method: HttpMethod.Post, cancellationToken: ct, checkResult: false, signed: true, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.ErrorCode == 0, null);
        }

        /// <summary>
        /// Gets deposit addresses
        /// </summary>
        /// <param name="asset">this allow you query single asset balance, e.g. BTC</param>
        /// <param name="blockchain">the (optional) blockchain filter</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitMaxDepositAddress> GetDepositAddresses(string asset, string blockchain = null, CancellationToken ct = default) => GetDepositAddressesAsync(asset, blockchain, ct).Result;
        /// <summary>
        /// Gets deposit addresses
        /// </summary>
        /// <param name="asset">this allow you query single asset balance, e.g. BTC</param>
        /// <param name="blockchain">the (optional) blockchain filter</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitMaxDepositAddress>> GetDepositAddressesAsync(string asset, string blockchain = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
            };
            parameters.AddOptionalParameter("blockchain", blockchain); // ERC20, TRC20, Omni

            var result = await SendRequest<BitMaxApiResponse<BitMaxDepositAddress>>(GetUrl(Endpoints_Cash_Wallet_DepositAddresses), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: true, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<BitMaxDepositAddress>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<BitMaxDepositAddress>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<BitMaxDepositAddress>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// Gets wallet transactions
        /// </summary>
        /// <param name="asset">this allow you query single asset balance, e.g. BTC</param>
        /// <param name="txType">add the (optional) transaction type filter</param>
        /// <param name="page">the page number, starting at 1</param>
        /// <param name="pageSize">the page size, must be positive</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitMaxPagedData<BitMaxTransaction>> GetWalletTransactions(string asset = null, BitMaxTransactionType? txType = null, int page = 1, int pageSize = 10, CancellationToken ct = default) => GetWalletTransactionsAsync(asset, txType, page, pageSize, ct).Result;
        /// <summary>
        /// Gets wallet transactions
        /// </summary>
        /// <param name="asset">this allow you query single asset balance, e.g. BTC</param>
        /// <param name="txType">add the (optional) transaction type filter</param>
        /// <param name="page">the page number, starting at 1</param>
        /// <param name="pageSize">the page size, must be positive</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitMaxPagedData<BitMaxTransaction>>> GetWalletTransactionsAsync(string asset = null, BitMaxTransactionType? txType = null, int page = 1, int pageSize = 10, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {"page", page},
                {"pageSize", pageSize},
            };
            parameters.AddOptionalParameter("asset", asset);
            if (txType != null) parameters.AddOptionalParameter("txType", JsonConvert.SerializeObject(txType, new TransactionTypeConverter(false)));

            var result = await SendRequest<BitMaxApiResponse<BitMaxPagedData<BitMaxTransaction>>>(GetUrl(Endpoints_Cash_Wallet_Transactions), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: true, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<BitMaxPagedData<BitMaxTransaction>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<BitMaxPagedData<BitMaxTransaction>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<BitMaxPagedData<BitMaxTransaction>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// Places a Spot Order
        /// Trading and Order related APIs. API path usually depend on account-group and account-category:
        /// - account-group : get your account-group from 'Account Info' API
        /// - account-category: cash or margin
        /// For all order related ack or data, there is orderId field to identify the order.
        /// Extra info on id (Client Order Id)
        /// - id value must satisfy regrex pattern "^\w[\w-]*\w$" (i.e. start and end with word character), and with length up to 32. ("Invalid Client Order id" error for violation)
        /// - id value with length 9 is recommended, since we take the right most 9 chars for order Id generation.
        /// - id value with length < 9 will not be used in order Id generation, but we still echo it back in order ack message (empty string when no id value provided).
        /// - If a valid id value is provided when placing order, order Id from server side is pre-determined. This could be helpful for order status check in case of accidently internet connection issue.
        /// Order Request Criteria
        /// When placing a new limit order, the request parameters must meet all criteria defined in the Products API:
        /// - The order notional must be within range[minNotional, maxNotional]. For limit orders, the order notional is defined as the product of orderPrice and orderQty.
        /// - orderPrice and stopPrice must be multiples of tickSize.
        /// - orderQty must be a multiple of lotSize.
        /// - If you are trading using the margin account, marginTradable must be true.
        /// - For cash trading, you must have sufficient balance to fund the order.
        ///   - for buy orders, if commissionType=Quote, the quote asset balance must be no less than orderPrice * orderQty * (1 + commissionReserveRate). For all other commissionTypes(Base and Received), the quote asset balance must be no less than orderPrice * orderQty
        ///   - for sell orders, if commissionType=Base, your base asset balance must be no less than orderQty * (1 + commissionReserveRate), For other commissionTypes(Received and Quote), the base asset balance must be no less than orderQty.
        /// - For margin trading, you must make sure you have sufficient max sellable amount to fund the order.
        /// </summary>
        /// <param name="symbol">Trading Symbol</param>
        /// <param name="size">Order size. Please set scale properly for each symbol.</param>
        /// <param name="type">Order type</param>
        /// <param name="side">Order Side</param>
        /// <param name="orderPrice">The limit price for limit order. Please set price scale properly.</param>
        /// <param name="stopPrice">Trigger price of stop limit order</param>
        /// <param name="clientOrderId">Optional but recommended. We echo it back to help you match response with request.</param>
        /// <param name="postOnly">Posts Only</param>
        /// <param name="timeInForce">GTC: good-till-canceled; IOC: immediate-or-cancel. GTC by default.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAccept>> PlaceSpotOrder(string symbol, decimal size, BitMaxCashOrderType type, BitMaxCashOrderSide side, decimal? orderPrice = null, decimal? stopPrice = null, string clientOrderId = null, bool postOnly = false, BitMaxCashOrderTimeInForce timeInForce = BitMaxCashOrderTimeInForce.GoodTillCanceled, CancellationToken ct = default) => PlaceOrderAsync(BitMaxCashAccountType.Spot, symbol, size, type, side, orderPrice, stopPrice, clientOrderId, postOnly, timeInForce, ct).Result;
        /// <summary>
        /// Places a Margin Order
        /// Trading and Order related APIs. API path usually depend on account-group and account-category:
        /// - account-group : get your account-group from 'Account Info' API
        /// - account-category: cash or margin
        /// For all order related ack or data, there is orderId field to identify the order.
        /// Extra info on id (Client Order Id)
        /// - id value must satisfy regrex pattern "^\w[\w-]*\w$" (i.e. start and end with word character), and with length up to 32. ("Invalid Client Order id" error for violation)
        /// - id value with length 9 is recommended, since we take the right most 9 chars for order Id generation.
        /// - id value with length < 9 will not be used in order Id generation, but we still echo it back in order ack message (empty string when no id value provided).
        /// - If a valid id value is provided when placing order, order Id from server side is pre-determined. This could be helpful for order status check in case of accidently internet connection issue.
        /// Order Request Criteria
        /// When placing a new limit order, the request parameters must meet all criteria defined in the Products API:
        /// - The order notional must be within range[minNotional, maxNotional]. For limit orders, the order notional is defined as the product of orderPrice and orderQty.
        /// - orderPrice and stopPrice must be multiples of tickSize.
        /// - orderQty must be a multiple of lotSize.
        /// - If you are trading using the margin account, marginTradable must be true.
        /// - For cash trading, you must have sufficient balance to fund the order.
        ///   - for buy orders, if commissionType=Quote, the quote asset balance must be no less than orderPrice * orderQty * (1 + commissionReserveRate). For all other commissionTypes(Base and Received), the quote asset balance must be no less than orderPrice * orderQty
        ///   - for sell orders, if commissionType=Base, your base asset balance must be no less than orderQty * (1 + commissionReserveRate), For other commissionTypes(Received and Quote), the base asset balance must be no less than orderQty.
        /// - For margin trading, you must make sure you have sufficient max sellable amount to fund the order.
        /// </summary>
        /// <param name="symbol">Trading Symbol</param>
        /// <param name="size">Order size. Please set scale properly for each symbol.</param>
        /// <param name="type">Order type</param>
        /// <param name="side">Order Side</param>
        /// <param name="orderPrice">The limit price for limit order. Please set price scale properly.</param>
        /// <param name="stopPrice">Trigger price of stop limit order</param>
        /// <param name="clientOrderId">Optional but recommended. We echo it back to help you match response with request.</param>
        /// <param name="postOnly">Posts Only</param>
        /// <param name="timeInForce">GTC: good-till-canceled; IOC: immediate-or-cancel. GTC by default.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAccept>> PlaceMarginOrder(string symbol, decimal size, BitMaxCashOrderType type, BitMaxCashOrderSide side, decimal? orderPrice = null, decimal? stopPrice = null, string clientOrderId = null, bool postOnly = false, BitMaxCashOrderTimeInForce timeInForce = BitMaxCashOrderTimeInForce.GoodTillCanceled, CancellationToken ct = default) => PlaceOrderAsync(BitMaxCashAccountType.Margin, symbol, size, type, side, orderPrice, stopPrice, clientOrderId, postOnly, timeInForce, ct).Result;
        /// <summary>
        /// Places a Spot Order
        /// Trading and Order related APIs. API path usually depend on account-group and account-category:
        /// - account-group : get your account-group from 'Account Info' API
        /// - account-category: cash or margin
        /// For all order related ack or data, there is orderId field to identify the order.
        /// Extra info on id (Client Order Id)
        /// - id value must satisfy regrex pattern "^\w[\w-]*\w$" (i.e. start and end with word character), and with length up to 32. ("Invalid Client Order id" error for violation)
        /// - id value with length 9 is recommended, since we take the right most 9 chars for order Id generation.
        /// - id value with length < 9 will not be used in order Id generation, but we still echo it back in order ack message (empty string when no id value provided).
        /// - If a valid id value is provided when placing order, order Id from server side is pre-determined. This could be helpful for order status check in case of accidently internet connection issue.
        /// Order Request Criteria
        /// When placing a new limit order, the request parameters must meet all criteria defined in the Products API:
        /// - The order notional must be within range[minNotional, maxNotional]. For limit orders, the order notional is defined as the product of orderPrice and orderQty.
        /// - orderPrice and stopPrice must be multiples of tickSize.
        /// - orderQty must be a multiple of lotSize.
        /// - If you are trading using the margin account, marginTradable must be true.
        /// - For cash trading, you must have sufficient balance to fund the order.
        ///   - for buy orders, if commissionType=Quote, the quote asset balance must be no less than orderPrice * orderQty * (1 + commissionReserveRate). For all other commissionTypes(Base and Received), the quote asset balance must be no less than orderPrice * orderQty
        ///   - for sell orders, if commissionType=Base, your base asset balance must be no less than orderQty * (1 + commissionReserveRate), For other commissionTypes(Received and Quote), the base asset balance must be no less than orderQty.
        /// - For margin trading, you must make sure you have sufficient max sellable amount to fund the order.
        /// </summary>
        /// <param name="symbol">Trading Symbol</param>
        /// <param name="size">Order size. Please set scale properly for each symbol.</param>
        /// <param name="type">Order type</param>
        /// <param name="side">Order Side</param>
        /// <param name="orderPrice">The limit price for limit order. Please set price scale properly.</param>
        /// <param name="stopPrice">Trigger price of stop limit order</param>
        /// <param name="clientOrderId">Optional but recommended. We echo it back to help you match response with request.</param>
        /// <param name="postOnly">Posts Only</param>
        /// <param name="timeInForce">GTC: good-till-canceled; IOC: immediate-or-cancel. GTC by default.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAccept>>> PlaceSpotOrderAsync(string symbol, decimal size, BitMaxCashOrderType type, BitMaxCashOrderSide side, decimal? orderPrice = null, decimal? stopPrice = null, string clientOrderId = null, bool postOnly = false, BitMaxCashOrderTimeInForce timeInForce = BitMaxCashOrderTimeInForce.GoodTillCanceled, CancellationToken ct = default) => await PlaceOrderAsync(BitMaxCashAccountType.Spot, symbol, size, type, side, orderPrice, stopPrice, clientOrderId, postOnly, timeInForce, ct);
        /// <summary>
        /// Places a Margin Order
        /// Trading and Order related APIs. API path usually depend on account-group and account-category:
        /// - account-group : get your account-group from 'Account Info' API
        /// - account-category: cash or margin
        /// For all order related ack or data, there is orderId field to identify the order.
        /// Extra info on id (Client Order Id)
        /// - id value must satisfy regrex pattern "^\w[\w-]*\w$" (i.e. start and end with word character), and with length up to 32. ("Invalid Client Order id" error for violation)
        /// - id value with length 9 is recommended, since we take the right most 9 chars for order Id generation.
        /// - id value with length < 9 will not be used in order Id generation, but we still echo it back in order ack message (empty string when no id value provided).
        /// - If a valid id value is provided when placing order, order Id from server side is pre-determined. This could be helpful for order status check in case of accidently internet connection issue.
        /// Order Request Criteria
        /// When placing a new limit order, the request parameters must meet all criteria defined in the Products API:
        /// - The order notional must be within range[minNotional, maxNotional]. For limit orders, the order notional is defined as the product of orderPrice and orderQty.
        /// - orderPrice and stopPrice must be multiples of tickSize.
        /// - orderQty must be a multiple of lotSize.
        /// - If you are trading using the margin account, marginTradable must be true.
        /// - For cash trading, you must have sufficient balance to fund the order.
        ///   - for buy orders, if commissionType=Quote, the quote asset balance must be no less than orderPrice * orderQty * (1 + commissionReserveRate). For all other commissionTypes(Base and Received), the quote asset balance must be no less than orderPrice * orderQty
        ///   - for sell orders, if commissionType=Base, your base asset balance must be no less than orderQty * (1 + commissionReserveRate), For other commissionTypes(Received and Quote), the base asset balance must be no less than orderQty.
        /// - For margin trading, you must make sure you have sufficient max sellable amount to fund the order.
        /// </summary>
        /// <param name="symbol">Trading Symbol</param>
        /// <param name="size">Order size. Please set scale properly for each symbol.</param>
        /// <param name="type">Order type</param>
        /// <param name="side">Order Side</param>
        /// <param name="orderPrice">The limit price for limit order. Please set price scale properly.</param>
        /// <param name="stopPrice">Trigger price of stop limit order</param>
        /// <param name="clientOrderId">Optional but recommended. We echo it back to help you match response with request.</param>
        /// <param name="postOnly">Posts Only</param>
        /// <param name="timeInForce">GTC: good-till-canceled; IOC: immediate-or-cancel. GTC by default.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAccept>>> PlaceMarginOrderAsync(string symbol, decimal size, BitMaxCashOrderType type, BitMaxCashOrderSide side, decimal? orderPrice = null, decimal? stopPrice = null, string clientOrderId = null, bool postOnly = false, BitMaxCashOrderTimeInForce timeInForce = BitMaxCashOrderTimeInForce.GoodTillCanceled, CancellationToken ct = default) => await PlaceOrderAsync(BitMaxCashAccountType.Margin, symbol, size, type, side, orderPrice, stopPrice, clientOrderId, postOnly, timeInForce, ct);
        private async Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAccept>>> PlaceOrderAsync(BitMaxCashAccountType cashAccountType, string symbol, decimal size, BitMaxCashOrderType type, BitMaxCashOrderSide side, decimal? orderPrice = null, decimal? stopPrice = null, string clientOrderId = null, bool postOnly = false, BitMaxCashOrderTimeInForce timeInForce = BitMaxCashOrderTimeInForce.GoodTillCanceled, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "time", DateTime.UtcNow.ToUnixTimeMilliseconds() },
                { "symbol", symbol.Replace("-", "/") },
                { "orderQty", size.ToString() },
                { "side", JsonConvert.SerializeObject(side, new CashOrderSideConverter(false)) },
                { "orderType", JsonConvert.SerializeObject(type, new CashOrderTypeConverter(false)) },
                { "respInst", JsonConvert.SerializeObject(BitMaxCashOrderResponseInstruction.ACCEPT, new CashOrderResponseInstructionConverter(false)) },
            };

            if (clientOrderId != null)
            {
                if (Regex.IsMatch(clientOrderId, "^(([a-z]|[A-Z]|[0-9]){9,32})$")) parameters.Add("id", clientOrderId);
                else throw new ArgumentException("ClientOrderId supports alphabets (case-sensitive) + numbers, or letters (case-sensitive) between 9-32 characters.");
            }

            if (type == BitMaxCashOrderType.Limit || type == BitMaxCashOrderType.StopLimit)
            {
                if (orderPrice == null) throw new ArgumentException("orderPrice is required for Limit and StopLimit orders");
                else parameters.Add("orderPrice", orderPrice.ToString());
            }

            if (type == BitMaxCashOrderType.Limit)
            {
                parameters.Add("postOnly", postOnly);
                parameters.Add("timeInForce", JsonConvert.SerializeObject(timeInForce, new CashOrderTimeInForceConverter(false)));
            }

            if (type == BitMaxCashOrderType.StopLimit || type == BitMaxCashOrderType.StopMarket)
            {
                if (stopPrice == null) throw new ArgumentException("orderPrice is required for StopLimit and StopMarket orders");
                else parameters.Add("stopPrice", stopPrice.ToString());
            }

            var url = Endpoints_Cash_Order_PlaceOrder.Replace("{account-category}", cashAccountType == BitMaxCashAccountType.Spot ? "cash" : "margin");
            var result = await SendRequest<BitMaxApiResponse<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAccept>>>(GetUrl(url), method: HttpMethod.Post, cancellationToken: ct, checkResult: false, signed: true, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAccept>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAccept>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAccept>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// Cancel an existing open order.
        /// </summary>
        /// <param name="symbol">Symbol of the order to cancel</param>
        /// <param name="orderId">You should set the value to be the orderId of the target order you want to cancel.</param>
        /// <param name="clientOrderId">We echo it back to help you identify the response if provided. This field is optional</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>> CancelSpotOrder(string symbol, string orderId, string clientOrderId = null, CancellationToken ct = default) => CancelOrderAsync(BitMaxCashAccountType.Spot, symbol, orderId, clientOrderId, ct).Result;
        /// <summary>
        /// Cancel an existing open order.
        /// </summary>
        /// <param name="symbol">Symbol of the order to cancel</param>
        /// <param name="orderId">You should set the value to be the orderId of the target order you want to cancel.</param>
        /// <param name="clientOrderId">We echo it back to help you identify the response if provided. This field is optional</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>> CancelMarginOrder(string symbol, string orderId, string clientOrderId = null, CancellationToken ct = default) => CancelOrderAsync(BitMaxCashAccountType.Margin, symbol, orderId, clientOrderId, ct).Result;
        /// <summary>
        /// Cancel an existing open order.
        /// </summary>
        /// <param name="symbol">Symbol of the order to cancel</param>
        /// <param name="orderId">You should set the value to be the orderId of the target order you want to cancel.</param>
        /// <param name="clientOrderId">We echo it back to help you identify the response if provided. This field is optional</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>> CancelSpotOrderAsync(string symbol, string orderId, string clientOrderId = null, CancellationToken ct = default) => await CancelOrderAsync(BitMaxCashAccountType.Spot, symbol, orderId, clientOrderId, ct);
        /// <summary>
        /// Cancel an existing open order.
        /// </summary>
        /// <param name="symbol">Symbol of the order to cancel</param>
        /// <param name="orderId">You should set the value to be the orderId of the target order you want to cancel.</param>
        /// <param name="clientOrderId">We echo it back to help you identify the response if provided. This field is optional</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>> CancelMarginOrderAsync(string symbol, string orderId, string clientOrderId = null, CancellationToken ct = default) => await CancelOrderAsync(BitMaxCashAccountType.Margin, symbol, orderId, clientOrderId, ct);
        private async Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>> CancelOrderAsync(BitMaxCashAccountType cashAccountType, string symbol, string orderId, string clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "time", DateTime.UtcNow.ToUnixTimeMilliseconds() },
                { "symbol", symbol.Replace("-", "/") },
                { "orderId", orderId },
            };
            parameters.AddOptionalParameter("id", clientOrderId);

            var url = Endpoints_Cash_Order_CancelOrder.Replace("{account-category}", cashAccountType == BitMaxCashAccountType.Spot ? "cash" : "margin");
            var result = await SendRequest<BitMaxApiResponse<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>>(GetUrl(url), method: HttpMethod.Delete, cancellationToken: ct, checkResult: false, signed: true, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// Cancel all current open orders for the account specified, and optional symbol.
        /// </summary>
        /// <param name="symbol">If provided, only cancel all orders on this symbol; otherwise, cancel all open orders under this account.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>> CancelAllSpotOrders(string symbol = null, CancellationToken ct = default) => CancelAllOrdersAsync(BitMaxCashAccountType.Spot, symbol, ct).Result;
        /// <summary>
        /// Cancel all current open orders for the account specified, and optional symbol.
        /// </summary>
        /// <param name="symbol">If provided, only cancel all orders on this symbol; otherwise, cancel all open orders under this account.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>> CancelAllMarginOrders(string symbol = null, CancellationToken ct = default) => CancelAllOrdersAsync(BitMaxCashAccountType.Margin, symbol, ct).Result;
        /// <summary>
        /// Cancel all current open orders for the account specified, and optional symbol.
        /// </summary>
        /// <param name="symbol">If provided, only cancel all orders on this symbol; otherwise, cancel all open orders under this account.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>> CancelAllSpotOrdersAsync(string symbol = null, CancellationToken ct = default) => await CancelAllOrdersAsync(BitMaxCashAccountType.Spot, symbol, ct);
        /// <summary>
        /// Cancel all current open orders for the account specified, and optional symbol.
        /// </summary>
        /// <param name="symbol">If provided, only cancel all orders on this symbol; otherwise, cancel all open orders under this account.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>> CancelAllMarginOrdersAsync(string symbol = null, CancellationToken ct = default) => await CancelAllOrdersAsync(BitMaxCashAccountType.Margin, symbol, ct);
        private async Task<WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>> CancelAllOrdersAsync(BitMaxCashAccountType cashAccountType, string symbol = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(symbol))
                parameters.AddOptionalParameter("symbol", symbol.Replace("-", "/"));

            var url = Endpoints_Cash_Order_CancelAllOrders.Replace("{account-category}", cashAccountType == BitMaxCashAccountType.Spot ? "cash" : "margin");
            var result = await SendRequest<BitMaxApiResponse<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>>(GetUrl(url), method: HttpMethod.Delete, cancellationToken: ct, checkResult: false, signed: true, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<BitMaxCashPlacedOrder<BitMaxCashPlacedOrderInfoAck>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// Query order status, either open or history order.
        /// orderId could be a single order Id, or multiple order Ids separated by a comma (,):
        /// - If you set symbol to be a single symbol, such as BTMX/USDT, the API will respond with the target order as an object. If you want to wrap the object in a one-element list, append a comma to the symbol, e.g.BTMX/USDT,.
        /// - You shall specify symbol as a comma separated symbol list, e.g.BTMX/USDT,BTC/USDT.The API will respond with a list of order objects.
        /// </summary>
        /// <param name="orderId">one or more order Ids separated by comma</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitMaxCashPlacedOrderInfoAccept> GetSpotOrder(string orderId, CancellationToken ct = default) => GetOrderAsync(BitMaxCashAccountType.Spot, orderId, ct).Result;
        /// <summary>
        /// Query order status, either open or history order.
        /// orderId could be a single order Id, or multiple order Ids separated by a comma (,):
        /// - If you set symbol to be a single symbol, such as BTMX/USDT, the API will respond with the target order as an object. If you want to wrap the object in a one-element list, append a comma to the symbol, e.g.BTMX/USDT,.
        /// - You shall specify symbol as a comma separated symbol list, e.g.BTMX/USDT,BTC/USDT.The API will respond with a list of order objects.
        /// </summary>
        /// <param name="orderId">one or more order Ids separated by comma</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<BitMaxCashPlacedOrderInfoAccept> GetMarginOrder(string orderId, CancellationToken ct = default) => GetOrderAsync(BitMaxCashAccountType.Margin, orderId, ct).Result;
        /// <summary>
        /// Query order status, either open or history order.
        /// orderId could be a single order Id, or multiple order Ids separated by a comma (,):
        /// - If you set symbol to be a single symbol, such as BTMX/USDT, the API will respond with the target order as an object. If you want to wrap the object in a one-element list, append a comma to the symbol, e.g.BTMX/USDT,.
        /// - You shall specify symbol as a comma separated symbol list, e.g.BTMX/USDT,BTC/USDT.The API will respond with a list of order objects.
        /// </summary>
        /// <param name="orderId">one or more order Ids separated by comma</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitMaxCashPlacedOrderInfoAccept>> GetSpotOrderAsync(string orderId, CancellationToken ct = default) => await GetOrderAsync(BitMaxCashAccountType.Spot, orderId, ct);
        /// <summary>
        /// Query order status, either open or history order.
        /// orderId could be a single order Id, or multiple order Ids separated by a comma (,):
        /// - If you set symbol to be a single symbol, such as BTMX/USDT, the API will respond with the target order as an object. If you want to wrap the object in a one-element list, append a comma to the symbol, e.g.BTMX/USDT,.
        /// - You shall specify symbol as a comma separated symbol list, e.g.BTMX/USDT,BTC/USDT.The API will respond with a list of order objects.
        /// </summary>
        /// <param name="orderId">one or more order Ids separated by comma</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<BitMaxCashPlacedOrderInfoAccept>> GetMarginOrderAsync(string orderId, CancellationToken ct = default) => await GetOrderAsync(BitMaxCashAccountType.Margin, orderId, ct);
        private async Task<WebCallResult<BitMaxCashPlacedOrderInfoAccept>> GetOrderAsync(BitMaxCashAccountType cashAccountType, string orderId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "orderId", orderId }
            };

            var url = Endpoints_Cash_Order_Query.Replace("{account-category}", cashAccountType == BitMaxCashAccountType.Spot ? "cash" : "margin");
            var result = await SendRequest<BitMaxApiResponse<BitMaxCashPlacedOrderInfoAccept>>(GetUrl(url), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: true, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<BitMaxCashPlacedOrderInfoAccept>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<BitMaxCashPlacedOrderInfoAccept>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<BitMaxCashPlacedOrderInfoAccept>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }
        
        /// <summary>
        /// This API returns all current open orders for the account specified.
        /// </summary>
        /// <param name="symbol">A valid symbol</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>> GetSpotOpenOrders(string symbol = null, CancellationToken ct = default) => GetOpenOrdersAsync(BitMaxCashAccountType.Spot, symbol, ct).Result;
        /// <summary>
        /// This API returns all current open orders for the account specified.
        /// </summary>
        /// <param name="symbol">A valid symbol</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>> GetMarginOpenOrders(string symbol = null, CancellationToken ct = default) => GetOpenOrdersAsync(BitMaxCashAccountType.Margin, symbol, ct).Result;
        /// <summary>
        /// This API returns all current open orders for the account specified.
        /// </summary>
        /// <param name="symbol">A valid symbol</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetSpotOpenOrdersAsync(string symbol = null, CancellationToken ct = default) => await GetOpenOrdersAsync(BitMaxCashAccountType.Spot, symbol, ct);
        /// <summary>
        /// This API returns all current open orders for the account specified.
        /// </summary>
        /// <param name="symbol">A valid symbol</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetMarginOpenOrdersAsync(string symbol = null, CancellationToken ct = default) => await GetOpenOrdersAsync(BitMaxCashAccountType.Margin, symbol, ct);
        private async Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetOpenOrdersAsync(BitMaxCashAccountType cashAccountType, string symbol = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbol", symbol);

            var url = Endpoints_Cash_Order_OpenOrders.Replace("{account-category}", cashAccountType == BitMaxCashAccountType.Spot ? "cash" : "margin");
            var result = await SendRequest<BitMaxApiResponse<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>>(GetUrl(url), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: true, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// This API returns all current history orders for the account specified. If you need earlier data or more filter, please refer to Order History API.
        /// </summary>
        /// <param name="symbol">symbol filter, e.g. "BTMX/USDT"</param>
        /// <param name="executedOnly">if True, include orders with non-zero filled quantities only.</param>
        /// <param name="limit">maximum number of orders to be included in the response</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>> GetSpotCurrentHistoryOrders(string symbol = null, bool executedOnly = false, int limit = 100, CancellationToken ct = default) => GetCurrentHistoryOrdersAsync(BitMaxCashAccountType.Spot, symbol, executedOnly, limit, ct).Result;
        /// <summary>
        /// This API returns all current history orders for the account specified. If you need earlier data or more filter, please refer to Order History API.
        /// </summary>
        /// <param name="symbol">symbol filter, e.g. "BTMX/USDT"</param>
        /// <param name="executedOnly">if True, include orders with non-zero filled quantities only.</param>
        /// <param name="limit">maximum number of orders to be included in the response</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>> GetMarginCurrentHistoryOrders(string symbol = null, bool executedOnly = false, int limit = 100, CancellationToken ct = default) => GetCurrentHistoryOrdersAsync(BitMaxCashAccountType.Margin, symbol, executedOnly, limit, ct).Result;
        /// <summary>
        /// This API returns all current history orders for the account specified. If you need earlier data or more filter, please refer to Order History API.
        /// </summary>
        /// <param name="symbol">symbol filter, e.g. "BTMX/USDT"</param>
        /// <param name="executedOnly">if True, include orders with non-zero filled quantities only.</param>
        /// <param name="limit">maximum number of orders to be included in the response</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetSpotCurrentHistoryOrdersAsync(string symbol = null, bool executedOnly = false, int limit = 100, CancellationToken ct = default) => await GetCurrentHistoryOrdersAsync(BitMaxCashAccountType.Spot, symbol, executedOnly, limit, ct);
        /// <summary>
        /// This API returns all current history orders for the account specified. If you need earlier data or more filter, please refer to Order History API.
        /// </summary>
        /// <param name="symbol">symbol filter, e.g. "BTMX/USDT"</param>
        /// <param name="executedOnly">if True, include orders with non-zero filled quantities only.</param>
        /// <param name="limit">maximum number of orders to be included in the response</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetMarginCurrentHistoryOrdersAsync(string symbol = null, bool executedOnly = false, int limit = 100, CancellationToken ct = default) => await GetCurrentHistoryOrdersAsync(BitMaxCashAccountType.Margin, symbol, executedOnly, limit, ct);
        private async Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetCurrentHistoryOrdersAsync(BitMaxCashAccountType cashAccountType, string symbol = null, bool executedOnly = false, int limit = 100, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "n", limit },
                { "executedOnly", executedOnly },
            };
            parameters.AddOptionalParameter("symbol", symbol);

            var url = Endpoints_Cash_Order_CurrentHistoryOrders.Replace("{account-category}", cashAccountType == BitMaxCashAccountType.Spot ? "cash" : "margin");
            var result = await SendRequest<BitMaxApiResponse<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>>(GetUrl(url), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: true, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// This API returns history orders according to specified parameters (up to 500 records). You have access to at least 30 days of order history.
        /// Please note seqNum increases regirously but not continuously in response.
        /// Rule for combination usage of startTime, endTime, and seqNum:
        /// - If at least one of seqNum and startTime is provided, the search will start from the oldest possible record(after given startTime or seqNum).
        /// - If neither seqNum nor startTime is provided, the search will start from the latest record(or endTime if provided) to the oldest.
        /// To retrieve the full history of orders, it is recommended to use seqNum and follow the procedure below:
        /// - Query the first batch of orders by sending a query with only startTime specifying.
        /// - Let n be the largest seqNum of orders obtained from the previous query, query the next batch by only setting seqNum = n + 1.
        /// - repeat the previous step until you have reached the latest record.
        /// </summary>
        /// <param name="symbol">symbol filter, e.g. BTMX/USDT</param>
        /// <param name="startTime">start time in milliseconds.</param>
        /// <param name="endTime">end time in milliseconds.</param>
        /// <param name="seqNum">the seqNum to search from. All records in the response have seqNum no less than the input argument.</param>
        /// <param name="limit">number of records to return. default 500, max 1000.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>> GetSpotHistoryOrders(string symbol = null, DateTime? startTime = null, DateTime? endTime = null, long? seqNum = null, int limit = 500, CancellationToken ct = default) => GetCurrentOrdersAsync(BitMaxCashAccountType.Spot, symbol, startTime, endTime, seqNum, limit, ct).Result;
        /// <summary>
        /// This API returns history orders according to specified parameters (up to 500 records). You have access to at least 30 days of order history.
        /// Please note seqNum increases regirously but not continuously in response.
        /// Rule for combination usage of startTime, endTime, and seqNum:
        /// - If at least one of seqNum and startTime is provided, the search will start from the oldest possible record(after given startTime or seqNum).
        /// - If neither seqNum nor startTime is provided, the search will start from the latest record(or endTime if provided) to the oldest.
        /// To retrieve the full history of orders, it is recommended to use seqNum and follow the procedure below:
        /// - Query the first batch of orders by sending a query with only startTime specifying.
        /// - Let n be the largest seqNum of orders obtained from the previous query, query the next batch by only setting seqNum = n + 1.
        /// - repeat the previous step until you have reached the latest record.
        /// </summary>
        /// <param name="symbol">symbol filter, e.g. BTMX/USDT</param>
        /// <param name="startTime">start time in milliseconds.</param>
        /// <param name="endTime">end time in milliseconds.</param>
        /// <param name="seqNum">the seqNum to search from. All records in the response have seqNum no less than the input argument.</param>
        /// <param name="limit">number of records to return. default 500, max 1000.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>> GetMarginHistoryOrders(string symbol = null, DateTime? startTime = null, DateTime? endTime = null, long? seqNum = null, int limit = 500, CancellationToken ct = default) => GetCurrentOrdersAsync(BitMaxCashAccountType.Margin, symbol, startTime, endTime, seqNum, limit, ct).Result;
        /// <summary>
        /// This API returns history orders according to specified parameters (up to 500 records). You have access to at least 30 days of order history.
        /// Please note seqNum increases regirously but not continuously in response.
        /// Rule for combination usage of startTime, endTime, and seqNum:
        /// - If at least one of seqNum and startTime is provided, the search will start from the oldest possible record(after given startTime or seqNum).
        /// - If neither seqNum nor startTime is provided, the search will start from the latest record(or endTime if provided) to the oldest.
        /// To retrieve the full history of orders, it is recommended to use seqNum and follow the procedure below:
        /// - Query the first batch of orders by sending a query with only startTime specifying.
        /// - Let n be the largest seqNum of orders obtained from the previous query, query the next batch by only setting seqNum = n + 1.
        /// - repeat the previous step until you have reached the latest record.
        /// </summary>
        /// <param name="symbol">symbol filter, e.g. BTMX/USDT</param>
        /// <param name="startTime">start time in milliseconds.</param>
        /// <param name="endTime">end time in milliseconds.</param>
        /// <param name="seqNum">the seqNum to search from. All records in the response have seqNum no less than the input argument.</param>
        /// <param name="limit">number of records to return. default 500, max 1000.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetSpotHistoryOrdersAsync(string symbol = null, DateTime? startTime = null, DateTime? endTime = null, long? seqNum = null, int limit = 500, CancellationToken ct = default) => await GetCurrentOrdersAsync(BitMaxCashAccountType.Spot, symbol, startTime, endTime, seqNum, limit, ct);
        /// <summary>
        /// This API returns history orders according to specified parameters (up to 500 records). You have access to at least 30 days of order history.
        /// Please note seqNum increases regirously but not continuously in response.
        /// Rule for combination usage of startTime, endTime, and seqNum:
        /// - If at least one of seqNum and startTime is provided, the search will start from the oldest possible record(after given startTime or seqNum).
        /// - If neither seqNum nor startTime is provided, the search will start from the latest record(or endTime if provided) to the oldest.
        /// To retrieve the full history of orders, it is recommended to use seqNum and follow the procedure below:
        /// - Query the first batch of orders by sending a query with only startTime specifying.
        /// - Let n be the largest seqNum of orders obtained from the previous query, query the next batch by only setting seqNum = n + 1.
        /// - repeat the previous step until you have reached the latest record.
        /// </summary>
        /// <param name="symbol">symbol filter, e.g. BTMX/USDT</param>
        /// <param name="startTime">start time in milliseconds.</param>
        /// <param name="endTime">end time in milliseconds.</param>
        /// <param name="seqNum">the seqNum to search from. All records in the response have seqNum no less than the input argument.</param>
        /// <param name="limit">number of records to return. default 500, max 1000.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public async Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetMarginHistoryOrdersAsync(string symbol = null, DateTime? startTime = null, DateTime? endTime = null, long? seqNum = null, int limit = 500, CancellationToken ct = default) => await GetCurrentOrdersAsync(BitMaxCashAccountType.Margin, symbol, startTime, endTime, seqNum, limit, ct);
        private async Task<WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>> GetCurrentOrdersAsync(BitMaxCashAccountType cashAccountType, string symbol = null, DateTime? startTime = null, DateTime? endTime = null, long? seqNum = null, int limit = 500, CancellationToken ct = default)
        {
            limit.ValidateIntBetween(nameof(limit), 1, 1000);
            var parameters = new Dictionary<string, object>
            {
                { "account", cashAccountType == BitMaxCashAccountType.Spot ? "cash" : "margin" },
                { "limit", limit },
            };
            parameters.AddOptionalParameter("symbol", symbol);
            parameters.AddOptionalParameter("seqNum", seqNum);
            if (startTime.HasValue) parameters.AddOptionalParameter("startTime", startTime.Value.ToUnixTimeMilliseconds());
            if (startTime.HasValue) parameters.AddOptionalParameter("endTime", endTime.Value.ToUnixTimeMilliseconds());

            var url = Endpoints_Cash_Order_HistoryOrders.Replace("{account-category}", cashAccountType == BitMaxCashAccountType.Spot ? "cash" : "margin");
            var result = await SendRequest<BitMaxApiResponse<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>>(GetUrl(url), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: true, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<IEnumerable<BitMaxCashPlacedOrderInfoAccept>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        #endregion

        #region Futures Api Methods

        public WebCallResult<IEnumerable<BitMaxAsset>> GetFuturesAssets(CancellationToken ct = default) => GetFuturesAssetsAsync(ct).Result;
        public async Task<WebCallResult<IEnumerable<BitMaxAsset>>> GetFuturesAssetsAsync(CancellationToken ct = default)
        {
            var result = await SendRequest<BitMaxApiResponse<IEnumerable<BitMaxAsset>>>(GetUrl(Endpoints_Cash_MarketData_Assets), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: false).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<BitMaxAsset>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data.ErrorCode > 0) return WebCallResult<IEnumerable<BitMaxAsset>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new BitMaxError(result.Data));

            return new WebCallResult<IEnumerable<BitMaxAsset>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        #endregion

        #region Protected Methods

        protected override Error ParseErrorResponse(JToken error)
        {
            if (error["code"] == null || error["message"] == null || error["reason"] == null)
                return new ServerError(error.ToString());

            var reason = "";
            var info = "";
            if (error["reason"] != null) reason = " REASON: " + (string)error["reason"];
            if (error["info"] != null) info = " INFO: " + (string)error["info"];
            return new ServerError((int)error["code"], (string)error["message"] + reason + info);
        }

        protected Uri GetUrl(string endpoint)
        {
            return new Uri($"{BaseAddress.TrimEnd('/')}{endpoint.Replace("<account-group>", AccountGroup.ToString())}");
        }

        protected override IRequest ConstructRequest(Uri uri, HttpMethod method, Dictionary<string, object> parameters, bool signed, PostParameters postPosition, ArrayParametersSerialization arraySerialization, int requestId)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();

            var uriString = uri.ToString();
            if (authProvider != null)
                parameters = authProvider.AddAuthenticationToParameters(uriString, method, parameters, signed, postPosition, arraySerialization);

            if ((method == HttpMethod.Get || postPosition == PostParameters.InUri) && parameters?.Any() == true)
                uriString += "?" + parameters.CreateParamString(true, arraySerialization);

            var contentType = requestBodyFormat == RequestBodyFormat.Json ? Constants.JsonContentHeader : Constants.FormContentHeader;
            var request = RequestFactory.Create(method, uriString, requestId);
            request.Accept = Constants.JsonContentHeader;

            var headers = new Dictionary<string, string>();
            if (authProvider != null)
            {
                // headers = authProvider.AddAuthenticationToHeaders(uriString, method, parameters!, signed, postPosition, arraySerialization);
                headers = AddAuthenticationToHeaders(uriString, method, parameters!, signed, postPosition, arraySerialization);
            }

            foreach (var header in headers)
                request.AddHeader(header.Key, header.Value);

            if ((method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Delete) && postPosition != PostParameters.InUri)
            {
                if (parameters?.Any() == true)
                    WriteParamBody(request, parameters, contentType);
                else
                    request.SetContent(requestBodyEmptyContent, contentType);
            }

            return request;
        }

        private Dictionary<string, string> AddAuthenticationToHeaders(string uri, HttpMethod method, Dictionary<string, object> parameters, bool signed, PostParameters postParameterPosition, ArrayParametersSerialization arraySerialization)
        {
            if (!signed)
                return new Dictionary<string, string>();

            if (authProvider.Credentials == null || authProvider.Credentials.Key == null || authProvider.Credentials.Secret == null)
                throw new ArgumentException("No valid API credentials provided. Key/Secret/PassPhrase needed.");

            var encryptor = new HMACSHA256(Encoding.ASCII.GetBytes(authProvider.Credentials.Secret.GetString()));

            var key = authProvider.Credentials.Key.GetString();
            var time = DateTime.UtcNow.ToUnixTimeMilliseconds().ToString();
            var signtext = $"{time}+{GetPrehashPath(uri)}";
            var signature = Convert.ToBase64String(encryptor.ComputeHash(Encoding.UTF8.GetBytes(signtext)));

            return new Dictionary<string, string> {
                { "x-auth-key", key },
                { "x-auth-timestamp", time },
                { "x-auth-signature", signature },
            };
        }

        private string GetPrehashPath(string uri)
        {
            foreach (var kvp in Endpoints_Prehash_Exceptions)
            {
                if (uri.Contains(kvp.Key)) return kvp.Value;
            }
            return new Uri(uri).LocalPath.Trim().Split('/').LastOrDefault();
        }

        #endregion

    }
}