namespace RRL.Core.Utility
{
    internal class SqlExeUtility
    {
        #region 添加用户

        /// <summary>
        /// 添加用户
        /// </summary>
        /// @username:用户名
        /// @spreader:推荐者
        /// @spreader_code:代理商编码
        public const string USER_REGIST_EXEC_STR = @"
INSERT INTO rrl_user (
	username,
	spreader_uid,
	spreader_code
) OUTPUT inserted.id
VALUES
	(
		@username,
		(
			SELECT
				ISNULL(
					(
						SELECT
							id
						FROM
							rrl_user
						WHERE
							username = @spreader
					),
					0
				)
		) ,@spreader_code
	)";

        #endregion 添加用户

        #region 更新短信验证码
        /// <summary>
        /// 更新短信验证码，预设send_status=1,is_used=0
        /// </summary>
        /// @mobile:电话号码
        /// @code:短信验证码
        public const string RECORD_SMS_EXEC_STR = @"
MERGE rrl_sms AS target USING (
	SELECT
		@mobile,
		@code,
		1,
		GETDATE(),
		0
) AS source (
	mobile,
	code,
	send_status,
	addtime,
	is_used
) ON (
	target.mobile = source.mobile
)
WHEN MATCHED THEN
	UPDATE
SET code = source.code,
 send_status = source.send_status,
 addtime = source.addtime,
 is_used = source.is_used
WHEN NOT MATCHED THEN
	INSERT (mobile, code, send_status)
VALUES
	(
		source.mobile,
		source.code,
		source.send_status
	);";
        #endregion 更新短信验证码

        #region 添加收藏
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// @uid:用户id
        /// @favourite_id:收藏内容id
        /// @favourite_type:收藏类型(1:商店;2:商品)
        public const string ADD_FAVOURITE_EXEC_STR = @"
MERGE rrl_favourite AS target USING (
	SELECT
		@uid ,@favourite_id ,@favourite_type
) AS source (
	uid,
	favourite_id,
	favourite_type
) ON (
	target.uid = source.uid
	AND target.favourite_id = source.favourite_id
	AND target.favourite_type = source.favourite_type
)
WHEN MATCHED THEN
	UPDATE
SET deletemark = NULL
WHEN NOT MATCHED THEN
	INSERT (
		uid,
		favourite_id,
		favourite_type
	)
VALUES
	(
		source.uid,
		source.favourite_id,
		source.favourite_type
	);";
        #endregion 添加收藏
    }
}