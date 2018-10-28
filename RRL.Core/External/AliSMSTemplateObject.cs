using RRL.Config;

namespace RRL.Core.External
{
    public class SMSTemplate
    {
        //认证短信模板
        public const string AUTH_SMS = PlatFormConfig.SMSTemplate_AUTH_SMS;

        //下单成功通知短信
        public const string ORDER_PAYMENT_SUCCESS_SMS = PlatFormConfig.SMSTemplate_ORDER_PAYMENT_SUCCESS_SMS;

        //提现申请通知短信
        public const string CASH_APPLY_HANDLE_SMS = PlatFormConfig.SMSTemplate_CASH_APPLY_HANDLE_SMS;
    }

    public class AuthSMS
    {
        //验证码
        public string code;
    }

    public class PublicSMS
    {
        
    }

    public class OrderPaymentSuccessSMS
    {
    }

    public class CashApplyHandleSMS
    {
        //提现时间
        public string time;

        //提现金额
        public double money;

        //银行卡尾号
        public string card;
    }
}