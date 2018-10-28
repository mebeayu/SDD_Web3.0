using RRL.Core.Manager;

namespace RRL.Core.Utility
{
    internal class SqlQueryUtility
    {
        #region 查询商品分类

        /// <summary>
        /// 查询商品分类
        /// </summary>
        public const string GOODS_TYPE_LIST_QUERY_STR = @"
SELECT
	id,
	name,
	icon,
	ParentID AS pid,
    recommend_type
FROM
	rrl_shop_cate
WHERE [type] = 0
ORDER BY
	order_id ASC";

        #endregion 查询商品分类

        #region 查询商品列表

        /// <summary>
        /// 查询商品列表
        /// </summary>
        public const string GOODS_LIST_QUERY_STR = @"
SELECT
	TOP ({0}) *
FROM
	(
		SELECT
			ROW_NUMBER () OVER (ORDER BY order_weight DESC) AS RowNum,
			dbo.rrl_goods.id,
			dbo.rrl_shop.shop_type,
			dbo.rrl_goods.price,
			dbo.rrl_goods.local_price,
			dbo.rrl_goods.name,
			dbo.rrl_goods.goods_type,
			dbo.rrl_goods.propaganda,
			(
		        SELECT
			        TOP (1) rrl_goods_pic.pic_id
		        FROM
			        rrl_goods_pic
		        WHERE
			        rrl_goods_pic.goods_id = rrl_goods.id
	        ) AS pic_id,
			dbo.rrl_fee_config.return_money_rate,
            dbo.rrl_goods.return_money_discount AS discount
		FROM
			dbo.rrl_goods
		JOIN dbo.rrl_fee_config ON dbo.rrl_goods.fee_id = dbo.rrl_fee_config.id
		JOIN dbo.rrl_shop ON dbo.rrl_shop.id = dbo.rrl_goods.sid
		WHERE
			dbo.rrl_goods.deletemark IS NULL
        AND dbo.rrl_goods.goods_type NOT IN (SELECT [Value] FROM rrl_config WHERE Item = 'exchange_area')
	) AS TEMP
WHERE
	TEMP.RowNum > {1}";

        #endregion 查询商品列表

        #region 搜索商品列表

        /// <summary>
        /// 搜索商品列表
        /// </summary>
        public const string GOODS_LIST_BY_KEY_WORD_QUERY_STR = @"
SELECT
	TOP ({0}) *
FROM
	(
		SELECT
			ROW_NUMBER () OVER (ORDER BY order_weight DESC,rrl_goods.addtime DESC) AS RowNum,
			dbo.rrl_goods.id,
			dbo.rrl_shop.shop_type,
			dbo.rrl_goods.price,
			dbo.rrl_goods.local_price,
			dbo.rrl_goods.name,
			dbo.rrl_goods.goods_type,
			dbo.rrl_goods.propaganda,
			(
		        SELECT
			        TOP (1) rrl_goods_pic.pic_id
		        FROM
			        rrl_goods_pic
		        WHERE
			        rrl_goods_pic.goods_id = rrl_goods.id
	        ) AS pic_id,
			dbo.rrl_fee_config.return_money_rate,
            dbo.rrl_goods.return_money_discount AS discount
		FROM
			dbo.rrl_goods
		JOIN dbo.rrl_fee_config ON dbo.rrl_goods.fee_id = dbo.rrl_fee_config.id
		JOIN dbo.rrl_shop ON dbo.rrl_shop.id = dbo.rrl_goods.sid
		WHERE
			dbo.rrl_goods.id IN (
				SELECT
					id
				FROM
					rrl_goods
				WHERE
					CONCAT (
						'#',
						name,
						'#',
						propaganda,
						'#',
						STUFF(
							(
								SELECT
									'#' + name
								FROM
									rrl_shop_cate
								WHERE
									id IN (
										goods_type,
										(
											SELECT
												ParentID
											FROM
												rrl_shop_cate
											WHERE
												id = goods_type
										),
										(
											SELECT
												ParentID
											FROM
												rrl_shop_cate
											WHERE
												id = (
													SELECT
														ParentID
													FROM
														rrl_shop_cate
													WHERE
														id = goods_type
												)
										)
									) FOR xml path ('')
							),1,1,''
						),
						'#'
					) LIKE '%{2}%'
			)
		AND dbo.rrl_goods.deletemark IS NULL
	) AS TEMP
WHERE
	TEMP.RowNum > {1}";
        #endregion 搜索商品列表

        #region 查询分类商品列表

        /// <summary>
        /// 查询分类商品列表
        /// </summary>
        public const string GOODS_LIST_BY_GOODS_TYPE_QUERY_STR = @"
SELECT
	TOP ({0}) *
FROM
	(
		SELECT
			ROW_NUMBER () OVER (ORDER BY order_weight DESC,rrl_goods.addtime DESC) AS RowNum,
			dbo.rrl_goods.id,
			dbo.rrl_shop.shop_type,
			dbo.rrl_goods.price,
			dbo.rrl_goods.local_price,
			dbo.rrl_goods.name,
			dbo.rrl_goods.goods_type,
			dbo.rrl_goods.propaganda,
			(
		        SELECT
			        TOP (1) rrl_goods_pic.pic_id
		        FROM
			        rrl_goods_pic
		        WHERE
			        rrl_goods_pic.goods_id = rrl_goods.id
	        ) AS pic_id,
			dbo.rrl_fee_config.return_money_rate,
            dbo.rrl_goods.return_money_discount AS discount,
            convert(decimal(18,2),isnull(rrl_goods.cash_pay_rate,{2})*rrl_goods.price/100) as cash_price,
            convert(decimal(18,2),isnull(rrl_goods.beans_pay_rate,{3})*rrl_goods.price) as beans_price
		FROM
			dbo.rrl_goods
		JOIN dbo.rrl_fee_config ON dbo.rrl_goods.fee_id = dbo.rrl_fee_config.id
		JOIN dbo.rrl_shop ON dbo.rrl_shop.id = dbo.rrl_goods.sid
		WHERE
			dbo.rrl_goods.deletemark IS NULL
		AND dbo.rrl_goods.goods_type = @GoodsType
	) AS TEMP
WHERE
	TEMP.RowNum > {1}";

        public const string SIMPLE_GOODS_LIST_BY_GOODS_TYPE_QUERY_STR = @"
SELECT
	TOP ({0}) *
FROM
	(
		SELECT
			ROW_NUMBER () OVER (ORDER BY order_weight DESC,rrl_goods.addtime DESC) AS RowNum,
			dbo.rrl_goods.id,
			dbo.rrl_shop.shop_type,
			dbo.rrl_goods.price,
			dbo.rrl_goods.local_price,
			dbo.rrl_goods.name,
			dbo.rrl_goods.goods_type,
			dbo.rrl_goods.propaganda,
			(
		        SELECT
			        TOP (1) rrl_goods_pic.pic_id
		        FROM
			        rrl_goods_pic
		        WHERE
			        rrl_goods_pic.goods_id = rrl_goods.id
	        ) AS pic_id,
			dbo.rrl_fee_config.return_money_rate,
            dbo.rrl_goods.return_money_discount AS discount,
            convert(decimal(18,2),isnull(rrl_goods.cash_pay_rate,{2})*rrl_goods.price/100) as cash_price,
            convert(decimal(18,2),isnull(rrl_goods.beans_pay_rate,{3})*rrl_goods.price) as beans_price
		FROM
			dbo.rrl_goods
		JOIN dbo.rrl_fee_config ON dbo.rrl_goods.fee_id = dbo.rrl_fee_config.id
		JOIN dbo.rrl_shop ON dbo.rrl_shop.id = dbo.rrl_goods.sid
		WHERE
			dbo.rrl_goods.deletemark IS NULL
		AND dbo.rrl_goods.goods_type IN(
            SELECT
	            id
            FROM
	            rrl_shop_cate
            WHERE
	            ParentID IN (
		            SELECT
			            id
		            FROM
			            rrl_shop_cate
		            WHERE
			            ParentID =  @GoodsType
	            )
            )
	) AS TEMP
WHERE
	TEMP.RowNum > {1}";

        public const string EX_GOODS_LIST_BY_GOODS_TYPE_QUERY_STR = @"
SELECT
	TOP ({0}) *
FROM
	(
		SELECT
			ROW_NUMBER () OVER (ORDER BY dbo.rrl_goods.{2} {3}) AS RowNum,
			dbo.rrl_goods.id,
			dbo.rrl_shop.shop_type,
			dbo.rrl_goods.price,
			dbo.rrl_goods.local_price,
			dbo.rrl_goods.name,
			dbo.rrl_goods.goods_type,
			dbo.rrl_goods.propaganda,
			(
		        SELECT
			        TOP (1) rrl_goods_pic.pic_id
		        FROM
			        rrl_goods_pic
		        WHERE
			        rrl_goods_pic.goods_id = rrl_goods.id
	        ) AS pic_id,
			dbo.rrl_fee_config.return_money_rate,
            dbo.rrl_goods.return_money_discount AS discount
		FROM
			dbo.rrl_goods
		JOIN dbo.rrl_fee_config ON dbo.rrl_goods.fee_id = dbo.rrl_fee_config.id
		JOIN dbo.rrl_shop ON dbo.rrl_shop.id = dbo.rrl_goods.sid
		WHERE
			dbo.rrl_goods.deletemark IS NULL
		AND dbo.rrl_goods.goods_type = @GoodsType
	) AS TEMP
WHERE
	TEMP.RowNum > {1}";

        public const string EXCHANGE_AREA_GOODS_LIST_QUERY_STR = @"
SELECT
	TOP ({0}) *
FROM
	(
		SELECT
			ROW_NUMBER () OVER (ORDER BY dbo.rrl_goods.{2} {3}) AS RowNum,
			dbo.rrl_goods.id,
			dbo.rrl_shop.shop_type,
			dbo.rrl_goods.price,
			dbo.rrl_goods.local_price,
			dbo.rrl_goods.name,
			dbo.rrl_goods.goods_type,
			dbo.rrl_goods.propaganda,
			(
		        SELECT
			        TOP (1) rrl_goods_pic.pic_id
		        FROM
			        rrl_goods_pic
		        WHERE
			        rrl_goods_pic.goods_id = rrl_goods.id
	        ) AS pic_id,
			dbo.rrl_fee_config.return_money_rate,
            dbo.rrl_goods.return_money_discount AS discount
		FROM
			dbo.rrl_goods
		JOIN dbo.rrl_fee_config ON dbo.rrl_goods.fee_id = dbo.rrl_fee_config.id
		JOIN dbo.rrl_shop ON dbo.rrl_shop.id = dbo.rrl_goods.sid
		WHERE
			dbo.rrl_goods.deletemark IS NULL
		AND dbo.rrl_goods.goods_type = (SELECT [Value] FROM rrl_config WHERE Item = 'exchange_area')
	) AS TEMP
WHERE
	TEMP.RowNum > {1}";

        #endregion 查询分类商品列表

        #region 查询推荐商品列表

        /// <summary>
        /// 查询推荐商品列表
        /// </summary>
        /// recommend_type = 1:主要推荐
        /// recommend_type = 2:热门推荐
        public const string RECOMMEND_GOODS_QUERY_STR = @"
SELECT
	TOP ({0}) *
FROM
	(
		SELECT
			ROW_NUMBER () OVER (ORDER BY dbo.rrl_goods.order_weight DESC) AS RowNum,
			dbo.rrl_goods.id,
			dbo.rrl_goods.price,
			dbo.rrl_goods.local_price,
			dbo.rrl_goods.name,
			dbo.rrl_goods.propaganda,
			dbo.rrl_goods.pic_id,
			dbo.rrl_fee_config.return_money_rate
		FROM
			dbo.rrl_goods
		JOIN dbo.rrl_fee_config ON dbo.rrl_goods.fee_id = dbo.rrl_fee_config.id
		WHERE
			dbo.rrl_goods.recommend_type = @RecommendType
		AND dbo.rrl_goods.deletemark IS NULL
	) AS TEMP
WHERE
	TEMP.RowNum > {1}";

        #endregion 查询推荐商品列表

        #region 查询轮播图

        /// <summary>
        /// 查询轮播图
        /// </summary>
        /// CarouselType=1:精选页轮播
        /// CarouselType=2:主页轮播
        public const string CAROUSEL_FIGURE_QUERY_STR = @"
SELECT TOP 1000
	dbo.rrl_carousel_figure.id,
    dbo.rrl_carousel_figure.type,
	dbo.rrl_carousel_figure.pic_id,
	dbo.rrl_carousel_figure.url,
	dbo.rrl_carousel_figure.direct_type
FROM
	dbo.rrl_carousel_figure
 where dbo.rrl_carousel_figure.deletemark IS NULL
ORDER BY dbo.rrl_carousel_figure.index_order desc";

        #endregion 查询轮播图

        #region 查询分类轮播图

        /// <summary>
        /// 根据cate_id查询轮播图
        /// </summary>
        /// type=4分类轮播
        public const string SHOP_CATE_CAROUSEL_FIGURE_QUERY_STR = @"
SELECT TOP (10)
	dbo.rrl_carousel_figure.id,
	dbo.rrl_carousel_figure.pic_id,
	dbo.rrl_carousel_figure.url,
	dbo.rrl_carousel_figure.direct_type,
    dbo.rrl_carousel_figure.cate_id
FROM
	dbo.rrl_carousel_figure
WHERE
	dbo.rrl_carousel_figure.type = 4
AND dbo.rrl_carousel_figure.deletemark IS NULL
ORDER BY dbo.rrl_carousel_figure.index_order ASC";

        #endregion 查询分类轮播图

        #region 查询安卓版本信息

        /// <summary>
        /// 查询安卓版本升级信息
        /// </summary>
        public const string ANDROID_APP_VERSION_QUERY_STR = @"
SELECT
	(
		SELECT
			CAST([Value] AS INT)
		FROM
			rrl_config
		WHERE
			[Item] = 'android_app_version'
	) AS version,
	(
		SELECT
			[Value]
		FROM
			rrl_config
		WHERE
			[Item] = 'android_app_download_url'
	) AS url,
	(
		SELECT
			[Value]
		FROM
			rrl_config
		WHERE
			[Item] = 'android_update_info'
	) AS info";

        #endregion 查询安卓版本信息


        #region 查询安卓版本信息

        /// <summary>
        /// 查询苹果版本升级信息
        /// </summary>
        public const string APPLE_APP_VERSION_QUERY_STR = @"
SELECT
	(
		SELECT
			 [Value]  
		FROM
			rrl_config
		WHERE
			[Item] = 'ios_app_version'
	) AS version,
	'' AS url,
	(
		SELECT
			[Value]
		FROM
			rrl_config
		WHERE
			[Item] = 'ios_update_info'
	) AS info";

        #endregion 查询安卓版本信息

        /// <summary>
        /// 获取新品首发
        /// </summary>
        public const string SHOP_CATE_GOODTYPE_STR= @"
 SELECT
			dbo.rrl_goods.id,
			dbo.rrl_goods.price,
			dbo.rrl_goods.local_price,
			dbo.rrl_goods.name,
			dbo.rrl_goods.propaganda,
			dbo.rrl_goods.pic_id,
           dbo.rrl_goods.is_newp
            
		FROM
			dbo.rrl_goods
		WHERE
        dbo.rrl_goods.is_newp=1
        ORDER BY rrl_goods.order_weight desc
        OFFSET {1} ROWS 
        FETCH NEXT {0} ROWS ONLY";

        /// <summary>
        /// 获取人气推荐
        /// </summary>
        public const string SHOP_CATE_GOODTYPE_STRPop = @"
     SELECT
			dbo.rrl_goods.id,
			dbo.rrl_goods.price,
			dbo.rrl_goods.local_price,
			dbo.rrl_goods.name,
			dbo.rrl_goods.propaganda,
			dbo.rrl_goods.pic_id,
            dbo.rrl_goods.is_pop
		FROM
			dbo.rrl_goods
		WHERE
        dbo.rrl_goods.is_pop=1
        ORDER BY rrl_goods.order_weight desc
        OFFSET {1} ROWS 
        FETCH NEXT {0} ROWS ONLY";

 
    }
}