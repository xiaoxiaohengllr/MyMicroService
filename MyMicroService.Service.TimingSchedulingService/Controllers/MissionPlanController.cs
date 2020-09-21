using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyMicroService.Infrastruct.IRepository;
using MyMicroService.Infrastruct.ResponseModels;
using MyMicroService.Service.TimingSchedulingService.APIModels;
using MyMicroService.Service.TimingSchedulingService.IService;
using MyMicroService.Service.TimingSchedulingService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMicroService.Service.TimingSchedulingService.Controllers
{
    /// <summary>
    /// 任务计划api接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MissionPlanController : ControllerBase
    {
        /// <summary>
        /// 任务计划服务接口
        /// </summary>
        private readonly IMissionPlanService _missionPlanService;

        /// <summary>
        /// 映射器
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 任务计划api接口
        /// </summary>
        /// <param name="missionPlanService">任务计划服务接口</param>
        /// <param name="mapper">映射器</param>
        public MissionPlanController(IMissionPlanService missionPlanService, IMapper mapper)
        {
            this._missionPlanService = missionPlanService;
            this._mapper = mapper;
        }

        /// <summary>
        /// 获取单个任务计划
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}"), ApiVersion("1.0")]
        public async Task<ResponseMessage<MissionPlan_GetListAsync_Response>> GetEntityAsync(string id)
        {
            MissionPlan missionPlan = await this._missionPlanService.GetEntityAsync(id);
            ResponseMessage<MissionPlan_GetListAsync_Response> responseMessage = new ResponseMessage<MissionPlan_GetListAsync_Response>();
            responseMessage.ReturnData(true, this._mapper.Map<MissionPlan_GetListAsync_Response>(missionPlan));
            return responseMessage;
        }

        /// <summary>
        /// 获取所有任务计划
        /// </summary>
        /// <returns></returns>
        [HttpGet, ApiVersion("1.0")]
        public async Task<ResponseMessage<List<MissionPlan_GetListAsync_Response>>> GetListAsync()
        {
            List<MissionPlan> missionPlanList = await this._missionPlanService.GetListAsync();
            ResponseMessage<List<MissionPlan_GetListAsync_Response>> responseMessage = new ResponseMessage<List<MissionPlan_GetListAsync_Response>>();
            responseMessage.ReturnData(true, this._mapper.Map<List<MissionPlan_GetListAsync_Response>>(missionPlanList));
            return responseMessage;
        }

        /// <summary>
        /// 获取任务计划分页
        /// </summary>
        /// <returns></returns>
        [HttpGet("{page}&{rows}"), ApiVersion("1.0")]
        public async Task<ResponseMessage<PageList<MissionPlan_GetPageListAsync_Response>>> GetPageListAsync(int page, int rows)
        {
            PageList<MissionPlan> pageList = await this._missionPlanService.GetPageListAsync(page, rows);
            ResponseMessage<PageList<MissionPlan_GetPageListAsync_Response>> responseMessage = new ResponseMessage<PageList<MissionPlan_GetPageListAsync_Response>>();
            PageList<MissionPlan_GetPageListAsync_Response> response = new PageList<MissionPlan_GetPageListAsync_Response>();
            response.Data = this._mapper.Map<List<MissionPlan_GetPageListAsync_Response>>(pageList.Data);
            response.Total = pageList.Total;
            responseMessage.ReturnData(true, response);
            return responseMessage;
        }

        /// <summary>
        /// 添加任务计划
        /// </summary>
        /// <param name="missionPlan_InsertAsync_Request">添加任务计划</param>
        /// <returns></returns>
        [HttpPost, ApiVersion("1.0")]
        public async Task<ResponseMessage> InsertAsync(MissionPlan_InsertAsync_Request missionPlan_InsertAsync_Request)
        {
            string msg = await this._missionPlanService.InsertAsync(this._mapper.Map<MissionPlan>(missionPlan_InsertAsync_Request));
            ResponseMessage responseMessage = new ResponseMessage();
            responseMessage.ReturnMessage(string.IsNullOrEmpty(msg), msg);
            return responseMessage;
        }

        /// <summary>
        /// 修改任务计划
        /// </summary>
        /// <param name="missionPlan_UpdateAsync_Request">修改任务计划</param>
        /// <returns></returns>
        [HttpPut, ApiVersion("1.0")]
        public async Task<ResponseMessage> UpdateAsync(MissionPlan_UpdateAsync_Request missionPlan_UpdateAsync_Request)
        {
            string msg = await this._missionPlanService.UpdateAsync(this._mapper.Map<MissionPlan>(missionPlan_UpdateAsync_Request));
            ResponseMessage responseMessage = new ResponseMessage();
            responseMessage.ReturnMessage(string.IsNullOrEmpty(msg), msg);
            return responseMessage;
        }

        /// <summary>
        /// 删除任务计划
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete, ApiVersion("1.0")]
        public async Task<ResponseMessage> UpdateAsync(string id)
        {
            string msg = await this._missionPlanService.DeleteAsync(id);
            ResponseMessage responseMessage = new ResponseMessage();
            responseMessage.ReturnMessage(string.IsNullOrEmpty(msg), msg);
            return responseMessage;
        }
    }
}
