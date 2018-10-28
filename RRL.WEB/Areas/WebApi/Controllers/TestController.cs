using RRL.WEB.Models.Response;
using RRL.WEB.Ulity;
using System.Web;
using System.Web.Http;

namespace RRL.WEB.Areas.WebApi.Controllers
{
    /// <summary>
    /// 测试用接口
    /// </summary>
    public class TestController : ApiController
    {
        private RdSession session = new RdSession(HttpContext.Current);

        /// <summary>
        /// 测试接口
        /// </summary>
        [HttpGet]
        [ActionName("Test")]
        public object Test()
        {
            session["test"] = "1234556";
            return "1";
        }

        /// <summary>
        /// 我的测试方法名
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string ReturnString()
        {
            return "start";
        }

        /// <summary>
        /// 返回参数测试方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("returnParam")]
        public int ReturnParamGet(int id)
        {
            return id;
        }

        /// <summary>
        /// 返回参数测试方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public DataResult ReturnData(int id)
        {
            return DataResult.InitFromMessageCode(id);
        }
    }
}