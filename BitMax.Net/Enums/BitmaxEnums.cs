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

    public enum BitMaxCashOrderSide
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

    public enum BitMaxCashOrderTimeInForce
    {
        GoodTillCanceled,
        ImmediateOrCancel,
    }

    public enum BitMaxCashOrderResponseInstruction
    {
        ACK,
        ACCEPT,
        DONE,
    }

    public enum BitMaxCashAccountType
    {
        Spot,
        Margin,
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
}
