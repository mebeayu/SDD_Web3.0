using RRL.EF;

namespace RRL.Core
{
    public class ServiceGlobal
    {
        public ServiceBase<admin_user> admin_user = new ServiceBase<admin_user>();
        public ServiceBase<bank_main> bank_main = new ServiceBase<bank_main>();
        public ServiceBase<cash_pool> cash_pool = new ServiceBase<cash_pool>();
        public ServiceBase<GOODS_DETAIL_VIEW> GOODS_DETAIL_VIEW = new ServiceBase<GOODS_DETAIL_VIEW>();
        public ServiceBase<GOODS_LIST_VIEW> GOODS_LIST_VIEW = new ServiceBase<GOODS_LIST_VIEW>();
        public ServiceBase<rrl_user> rrl_user = new ServiceBase<rrl_user>();
        public ServiceBase<rrl_cash_apply> rrl_cash_apply = new ServiceBase<rrl_cash_apply>();
        public ServiceBase<rrl_goods> rrl_goods = new ServiceBase<rrl_goods>();
        public ServiceBase<rrl_goods_pic> rrl_goods_pic = new ServiceBase<rrl_goods_pic>();
        public ServiceBase<rrl_shop_cate> rrl_shop_cate = new ServiceBase<rrl_shop_cate>();
        public ServiceBase<rrl_shop_pic> rrl_shop_pic = new ServiceBase<rrl_shop_pic>();
        public ServiceBase<rrl_trans_record> rrl_trans_record = new ServiceBase<rrl_trans_record>();
        public ServiceBase<rrl_user_bank_card> rrl_user_bank_card = new ServiceBase<rrl_user_bank_card>();
    }
}