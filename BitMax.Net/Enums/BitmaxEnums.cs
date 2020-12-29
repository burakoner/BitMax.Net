namespace BitMax.Net.Enums
{
    public enum BitMaxAssetStatus
    {
        Normal,
        NoDeposit,
        NoWithdraw,
        NoTransaction,
    }
    
    public enum BitMaxProductStatus
    {
        Normal,
        NoTrading,
    }

    public enum BitMaxProductCommissionType
    {
        Base,
        Quote,
        Received,
    }

    public enum BitMaxPeriod
    {
        OneMinute,
        FiveMinutes,
        FifteenMinutes,
        ThirtyMinutes,
        OneHour,
        TwoHours,
        FourHours,
        SixHours,
        TwelveHours,
        OneDay,
        OneWeek,
        OneMonth,
    }

    public enum BitMaxWalletAccount
    {
        Cash,
        Margin,
        Futures,
    }

    public enum BitMaxTransactionType
    {
        Deposit,
        Withdrawal,
    }

    public enum BitMaxTransactionStatus
    {
        Pending,
        Reviewing,
        Confirmed,
        Rejected,
        Canceled,
        Failed
    }

    public enum BitMaxOrderSide
    {
        Buy,
        Sell,
    }

    public enum BitMaxCashOrderType
    {
        Limit,
        Market,
        StopLimit,
        StopMarket,
    }
    
    public enum BitMaxFuturesOrderType
    {
        Limit,
        Market,
    }

    public enum BitMaxCashOrderTimeInForce
    {
        GoodTillCanceled,
        ImmediateOrCancel,
    }

    public enum BitMaxFuturesOrderTimeInForce
    {
        GoodTillCanceled,
        ImmediateOrCancel,
    }

    public enum BitMaxOrderResponseInstruction
    {
        ACK,
        ACCEPT,
        DONE,
        ERROR,
    }

    public enum BitMaxAccountType
    {
        Spot,
        Margin,
        Futures,
    }

    public enum BitMaxCashOrderStatus
    {
        New,
        PendingNew,
        Filled, 
        PartiallyFilled, 
        Cancelled, 
        Reject,
    }

    public enum BitMaxFuturesOrderStatus
    {
        New,
        PendingNew,
        Filled, 
        PartiallyFilled, 
        Cancelled, 
        Reject,
    }
}
