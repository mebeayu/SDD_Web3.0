using RRL.DB;
using System.Data;

namespace RRL.Core.Models
{
    internal class Dictionary
    {
        #region 缓存BankMain

        private static DataSet _bankMain;

        public static DataSet GetBankMain()
        {
            if (_bankMain == null || _bankMain.Tables[0].Rows.Count == 0)
            {
                InitBankMainNoCache();
            }
            return _bankMain;
        }

        private static void InitBankMainNoCache()
        {
            RRLDB db = new RRLDB();
            _bankMain = db.ExeQuery("select * from bank_main");
            db.Close();
        }

        #endregion 缓存BankMain

        #region 缓存AreaCode

        private static DataSet _areaCode;

        public static DataSet GetAreaCode()
        {
            if (_areaCode == null || _areaCode.Tables[0].Rows.Count == 0)
            {
                InitAreaCodeNoCache();
            }
            return _areaCode;
        }

        private static void InitAreaCodeNoCache()
        {
            RRLDB db = new RRLDB();
            //限定云南省昆明市
            _areaCode = db.ExeQuery("SELECT * FROM area_code");
            db.Close();
        }

        #endregion 缓存AreaCode

        #region 缓存TrackCom

        private static DataSet _trackCom;

        public static DataSet GetTrackCom()
        {
            if (_trackCom == null || _trackCom.Tables[0].Rows.Count == 0)
            {
                InitTrackComNoCache();
            }
            return _trackCom;
        }

        private static void InitTrackComNoCache()
        {
            RRLDB db = new RRLDB();
            _trackCom = db.ExeQuery("select id,name,code from track_com where is_used = 1");
            db.Close();
        }

        #endregion 缓存TrackCom
    }
}