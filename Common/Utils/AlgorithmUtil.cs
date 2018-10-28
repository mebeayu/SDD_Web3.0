using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public class AlgorithmUtilEqualTotalMoneyItem
    {
        /// <summary>
        /// 借款期数
        /// </summary>
        public int LoanPeriodNumber { get; set; }

        /// <summary>
        /// 还款日期
        /// </summary>
        public DateTime RepaymentDate { get; set; }

        /// <summary>
        /// 还款总额
        /// </summary>
        public decimal TheTotalAmountOfRepayment { get; set; }

        /// <summary>
        /// 还款利息
        /// </summary>
        public decimal RepaymentOfInterest { get; set; }

        /// <summary>
        /// 还款本金
        /// </summary>
        public decimal RepaymentOfPrincipal { get; set; }

        /// <summary>
        /// 剩余本金
        /// </summary>
        public decimal TheRemainingPrincipal { get; set; }
    }

    /// <summary>
    /// 算法库
    /// </summary>
    public static class AlgorithmUtil
    {

        /// <summary>
        /// 等额本金，等额本息
        /// </summary>
        /// <param name="fMoney">贷款总数</param>
        /// <param name="nDeadline">期数</param>
        /// <param name="yRate">年利率</param>
        /// <param name="type">0:等额本金/1:等额本息</param>
        /// <returns></returns>
        public static List<AlgorithmUtilEqualTotalMoneyItem> EqualTotalMoney(double fMoney, double yRate, DateTime startDate, int nDeadline, int type)
        {
            int a = Convert.ToInt32(nDeadline);//期数
            double b = Convert.ToDouble(yRate);//年利率
            double c = Convert.ToDouble(fMoney);//贷款总数
            double d = Convert.ToDouble(1);//优惠幅度
            double e = (b * d) / (12 * 100);//月利率
            double f = c * e;//首期利息
            double g = Math.Pow((1 + e), a);//取次方
            double hk = 0;//每月还款数
            double bj = 0;//每期本金数
            DateTime dt = startDate;//首次还款日期
            double slx = 0;
            double sbj = 0;

            List<AlgorithmUtilEqualTotalMoneyItem> list = new List<AlgorithmUtilEqualTotalMoneyItem>();

            if (type == 0)//等额本金还款法
            {
                hk = 0;
                bj = 0;
            }
            else//等额本息还款法
            {
                hk = c * e * g / (g - 1);
                bj = hk - f;
            }
            for (int i = 0; i < a; i++)
            {
                if (type == 0)//等额本金还款法
                {
                    f = (c - sbj) * e;
                    sbj += c / a;
                    slx += f;
                    hk = f + c / a;
                }
                else//等额本息还款法
                {
                    if (i != 0)
                    {
                        f = (c - sbj) * e;
                        bj = hk - (c - sbj) * e;
                    }
                    slx += f;
                    sbj += bj;
                }

                if (i != 0)
                {
                    dt = dt.AddMonths(1);
                }
                list.Add(new AlgorithmUtilEqualTotalMoneyItem()
                {
                    LoanPeriodNumber = i + 1,
                    RepaymentDate = dt,
                    RepaymentOfInterest = Convert.ToDecimal(f),
                    RepaymentOfPrincipal = Convert.ToDecimal(hk - f),
                    TheRemainingPrincipal = Convert.ToDecimal(c - sbj),
                    TheTotalAmountOfRepayment = Convert.ToDecimal(hk)
                });
            }

            return list;
        }
    }
}
