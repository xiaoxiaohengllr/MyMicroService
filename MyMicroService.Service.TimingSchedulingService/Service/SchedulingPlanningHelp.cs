using MyMicroService.Infrastruct.HttpWeb;
using MyMicroService.Infrastruct.ILog;
using MyMicroService.Infrastruct.JsonSerialize;
using MyMicroService.Infrastruct.RegularExpression;
using System;
using System.Text;

namespace MyMicroService.Service.TimingSchedulingService.Service
{
    /// <summary>
    /// 调度计划帮助类
    /// </summary>
    public class SchedulingPlanningHelp
    {
        /// <summary>
        /// http请求帮助接口
        /// </summary>
        private readonly IHttpWebHelper _httpWebHelper;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogFactory _logFactory;

        /// <summary>
        /// 调度计划帮助构造
        /// </summary>
        /// <param name="httpWebHelper">http请求帮助接口</param>
        /// <param name="logFactory">日志</param>
        public SchedulingPlanningHelp(IHttpWebHelper httpWebHelper, ILogFactory logFactory)
        {
            this._httpWebHelper = httpWebHelper;
            this._logFactory = logFactory;
        }

        #region HttpGet
        /// <summary>
        /// HttpGet方式模型
        /// </summary>
        public class HttpGetModel
        {
            /// <summary>
            /// 请求URL
            /// </summary>
            public string URL { get; set; }
        }

        /// <summary>
        /// 验证HttpGet方式
        /// </summary>
        /// <param name="operationData">操作数据josn</param>
        public string ValidationHttpGet(string operationData)
        {
            HttpGetModel httpGetModel;
            try
            {
                httpGetModel = operationData.ToObject<HttpGetModel>();
            }
            catch (Exception)
            {
                return "操作数据有误";
            }
            if (httpGetModel == null)
                return "操作数据有误";
            if (string.IsNullOrEmpty(httpGetModel.URL))
                return "必须填写URL";
            if (!httpGetModel.URL.IsURL())
                return "URL填写不正确";
            return "";
        }

        /// <summary>
        /// 执行HttpGet方式
        /// </summary>
        /// <param name="operationData">操作数据josn</param>
        public void ExecutionHttpGet(string operationData)
        {
            HttpGetModel httpGetModel = operationData.ToObject<HttpGetModel>();
            try
            {
                string result = this._httpWebHelper.HttpGet(httpGetModel.URL, Encoding.UTF8);
                this._logFactory.LogInformation($"任务调度执行HttpGet成功返回值为({result})");
            }
            catch (Exception e)
            {
                this._logFactory.LogError($"任务调度执行HttpGet发生错误({e.Message})");
                throw e;
            }
        }
        #endregion

        #region HttpPost
        /// <summary>
        /// HttpGet方式模型
        /// </summary>
        public class HttpPostModel
        {
            /// <summary>
            /// 请求URL
            /// </summary>
            public string URL { get; set; }

            /// <summary>
            /// 请求数据
            /// </summary>
            public string Data { get; set; }
        }

        /// <summary>
        /// 验证HttpPost方式
        /// </summary>
        /// <param name="operationData">操作数据josn</param>
        public string ValidationHttpPost(string operationData)
        {
            HttpPostModel httpPostModel;
            try
            {
                httpPostModel = operationData.ToObject<HttpPostModel>();
            }
            catch (Exception)
            {
                return "操作数据有误";
            }
            if (httpPostModel == null)
                return "操作数据有误";
            if (string.IsNullOrEmpty(httpPostModel.URL))
                return "必须填写URL";
            if (!httpPostModel.URL.IsURL())
                return "URL填写不正确";
            return "";
        }

        /// <summary>
        /// 执行HttpPost方式
        /// </summary>
        /// <param name="operationData">操作数据josn</param>
        public void ExecutionHttpPost(string operationData)
        {
            HttpPostModel httpPostModel = operationData.ToObject<HttpPostModel>();
            try
            {
                string result = this._httpWebHelper.HttpPost(httpPostModel.URL, httpPostModel.Data, Encoding.UTF8);
                this._logFactory.LogInformation($"任务调度执行HttpPost成功返回值为({result})");
            }
            catch (Exception e)
            {
                this._logFactory.LogError($"任务调度执行HttpPost发生错误({e.Message})");
                throw e;
            }
        }
        #endregion
    }
}
