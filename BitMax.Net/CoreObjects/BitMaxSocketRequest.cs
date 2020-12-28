using BitMax.Net.Helpers;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BitMax.Net.CoreObjects
{
    internal class BitMaxSocketPingRequest
    {
        [JsonProperty("id")]
        public string RequestId { get; set; }

        [JsonProperty("op")]
        public string Operation { get; set; } = "pong";

        public BitMaxSocketPingRequest(long id, string op="pong")
        {
            RequestId = id.ToString();
            Operation = op;
        }

        public BitMaxSocketPingRequest(string id, string op = "pong")
        {
            RequestId = id;
            Operation = op;
        }
    }
    
    internal class BitMaxSocketAuthRequest
    {
        [JsonProperty("id")]
        public string RequestId { get; set; }

        [JsonProperty("op")]
        public string Operation { get; set; } = "auth";
        
        [JsonProperty("t")]
        public long Timestamp { get; set; }
        
        [JsonProperty("key")]
        public string ApiKey { get; set; }
        
        [JsonProperty("sig")]
        public string Signature { get; set; }

        public BitMaxSocketAuthRequest(long id, string op= "auth")
        {
            RequestId = id.ToString();
            Operation = op;
        }

        public BitMaxSocketAuthRequest(string id, string op = "auth")
        {
            RequestId = id;
            Operation = op;
        }

        public void Sign(string apikey, string apisecret)
        {
            ApiKey = apikey;
            Timestamp = DateTime.UtcNow.ToUnixTimeMilliseconds();

            var encryptor = new HMACSHA256(Encoding.ASCII.GetBytes(apisecret));
            var signtext = $"{Timestamp}+stream";
            Signature = Convert.ToBase64String(encryptor.ComputeHash(Encoding.UTF8.GetBytes(signtext)));
        }
    }

    internal class BitMaxSocketCashChannelRequest
    {
        [JsonProperty("id")]
        public string RequestId { get; set; }

        [JsonProperty("op"), JsonConverter(typeof(SocketCashChannelOperationConverter))]
        public BitMaxSocketCashChannelOperation Operation { get; set; }

        [JsonProperty("ch")]
        public string Channel { get; set; }

        public BitMaxSocketCashChannelRequest(long id, BitMaxSocketCashChannelOperation operation, string channel)
        {
            RequestId = id.ToString();
            Operation = operation;
            Channel = channel;
        }

        public BitMaxSocketCashChannelRequest(string id, BitMaxSocketCashChannelOperation operation, string channel)
        {
            RequestId = id;
            Operation = operation;
            Channel = channel;
        }
    }

    internal class BitMaxSocketCashQueryRequest
    {
        [JsonProperty("id")]
        public string RequestId { get; set; }

        [JsonProperty("op")]
        public string Operation { get; set; } = "req";

        [JsonProperty("action"), JsonConverter(typeof(SocketCashQueryActionConverter))]
        public BitMaxSocketCashQueryAction Action { get; set; }

        [JsonProperty("account"), JsonConverter(typeof(SocketCashQueryAccountConverter))]
        public BitMaxSocketCashQueryAccount Account { get; set; }

        [JsonProperty("args")]
        public Dictionary<string, string> Arguments { get; set; }

        public BitMaxSocketCashQueryRequest(string id, BitMaxSocketCashQueryAction action, BitMaxSocketCashQueryAccount account, Dictionary<string, string> args)
        {
            RequestId = id;
            Action = action;
            Account = account;
            Arguments = args;
        }
    }

    internal enum BitMaxSocketCashChannelOperation
    {
        Subscribe,
        Unsubscribe,
    }

    internal class SocketCashChannelOperationConverter : BaseConverter<BitMaxSocketCashChannelOperation>
    {
        public SocketCashChannelOperationConverter() : this(true) { }
        public SocketCashChannelOperationConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxSocketCashChannelOperation, string>> Mapping => new List<KeyValuePair<BitMaxSocketCashChannelOperation, string>>
        {
            new KeyValuePair<BitMaxSocketCashChannelOperation, string>(BitMaxSocketCashChannelOperation.Subscribe, "sub"),
            new KeyValuePair<BitMaxSocketCashChannelOperation, string>(BitMaxSocketCashChannelOperation.Unsubscribe, "unsub"),
        };
    }

    internal enum BitMaxSocketCashQueryAction
    {
        PlaceOrder,
        CancelOrder,
        CancelAll,
        DepthSnapshot,
        DepthSnapshotTop100,
        MarketTrades,
        Balance,
        OpenOrder,
        MarginRisk,
    }

    internal class SocketCashQueryActionConverter : BaseConverter<BitMaxSocketCashQueryAction>
    {
        public SocketCashQueryActionConverter() : this(true) { }
        public SocketCashQueryActionConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxSocketCashQueryAction, string>> Mapping => new List<KeyValuePair<BitMaxSocketCashQueryAction, string>>
        {
            new KeyValuePair<BitMaxSocketCashQueryAction, string>(BitMaxSocketCashQueryAction.PlaceOrder, "place-order"),
            new KeyValuePair<BitMaxSocketCashQueryAction, string>(BitMaxSocketCashQueryAction.CancelOrder, "cancel-order"),
            new KeyValuePair<BitMaxSocketCashQueryAction, string>(BitMaxSocketCashQueryAction.CancelAll, "cancel-all"),
            new KeyValuePair<BitMaxSocketCashQueryAction, string>(BitMaxSocketCashQueryAction.DepthSnapshot, "depth-snapshot"),
            new KeyValuePair<BitMaxSocketCashQueryAction, string>(BitMaxSocketCashQueryAction.DepthSnapshotTop100, "depth-snapshot-top100"),
            new KeyValuePair<BitMaxSocketCashQueryAction, string>(BitMaxSocketCashQueryAction.MarketTrades, "market-trades"),
            new KeyValuePair<BitMaxSocketCashQueryAction, string>(BitMaxSocketCashQueryAction.Balance, "balance"),
            new KeyValuePair<BitMaxSocketCashQueryAction, string>(BitMaxSocketCashQueryAction.OpenOrder, "open-order"),
            new KeyValuePair<BitMaxSocketCashQueryAction, string>(BitMaxSocketCashQueryAction.MarginRisk, "margin-risk"),
        };
    }

    internal enum BitMaxSocketCashQueryAccount
    {
        Spot,
        Margin,
    }

    internal class SocketCashQueryAccountConverter : BaseConverter<BitMaxSocketCashQueryAccount>
    {
        public SocketCashQueryAccountConverter() : this(true) { }
        public SocketCashQueryAccountConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxSocketCashQueryAccount, string>> Mapping => new List<KeyValuePair<BitMaxSocketCashQueryAccount, string>>
        {
            new KeyValuePair<BitMaxSocketCashQueryAccount, string>(BitMaxSocketCashQueryAccount.Spot, "cash"),
            new KeyValuePair<BitMaxSocketCashQueryAccount, string>(BitMaxSocketCashQueryAccount.Margin, "margin"),
        };
    }

}
