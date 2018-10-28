using System;

namespace RRL.Core.Pay.WxPay
{
    public class WxPayException : Exception
    {
        public WxPayException(string msg) : base(msg)
        {
        }
    }
}