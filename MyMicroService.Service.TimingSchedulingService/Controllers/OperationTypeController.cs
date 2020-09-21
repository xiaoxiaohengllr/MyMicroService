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
    /// 操作类型api接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OperationTypeController : ControllerBase
    {
        /// <summary>
        ///  操作类型服务接口
        /// </summary>
        private readonly IOperationTypeService _operationTypeService;

        /// <summary>
        /// 映射器
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        ///  操作类型api接口
        /// </summary>
        /// <param name="operationTypeService">操作类型服务接口</param>
        /// <param name="mapper">映射器</param>
        public OperationTypeController(IOperationTypeService operationTypeService, IMapper mapper)
        {
            this._operationTypeService = operationTypeService;
            this._mapper = mapper;
        }

        /// <summary>
        /// 获取单个操作类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}"), ApiVersion("1.0")]
        public async Task<ResponseMessage<OperationType_GetEntityAsync_Response>> GetEntityAsync(string id)
        {
            OperationType operationType = await this._operationTypeService.GetEntityAsync(id);
            ResponseMessage<OperationType_GetEntityAsync_Response> responseMessage = new ResponseMessage<OperationType_GetEntityAsync_Response>();
            responseMessage.ReturnData(true, this._mapper.Map<OperationType_GetEntityAsync_Response>(operationType));
            return responseMessage;
        }

        /// <summary>
        /// 获取操作类型
        /// </summary>
        /// <returns></returns>
        [HttpGet, ApiVersion("1.0")]
        public async Task<ResponseMessage<List<OperationType_GetListAsync_Response>>> GetListAsync()
        {
            List<OperationType> operationType = await this._operationTypeService.GetListAsync();
            ResponseMessage<List<OperationType_GetListAsync_Response>> responseMessage = new ResponseMessage<List<OperationType_GetListAsync_Response>>();
            responseMessage.ReturnData(true, this._mapper.Map<List<OperationType_GetListAsync_Response>>(operationType));
            return responseMessage;
        }

        /// <summary>
        /// 获取操作类型分页
        /// </summary>
        /// <returns></returns>
        [HttpGet("{page}&{rows}"), ApiVersion("1.0")]
        public async Task<ResponseMessage<PageList<OperationType_GetPageListAsync_Response>>> GetPageListAsync(int page, int rows)
        {
            PageList<OperationType> pageList = await this._operationTypeService.GetPageListAsync(page, rows);
            ResponseMessage<PageList<OperationType_GetPageListAsync_Response>> responseMessage = new ResponseMessage<PageList<OperationType_GetPageListAsync_Response>>();
            PageList<OperationType_GetPageListAsync_Response> response = new PageList<OperationType_GetPageListAsync_Response>();
            response.Data = this._mapper.Map<List<OperationType_GetPageListAsync_Response>>(pageList.Data);
            response.Total = pageList.Total;
            responseMessage.ReturnData(true, response);
            return responseMessage;
        }

        /// <summary>
        /// 添加操作类型
        /// </summary>
        /// <param name="operationType_InsertAsync_Request">添加操作类型</param>
        /// <returns></returns>
        [HttpPost, ApiVersion("1.0")]
        public async Task<ResponseMessage> InsertAsync(OperationType_InsertAsync_Request operationType_InsertAsync_Request)
        {
            string msg = await this._operationTypeService.InsertAsync(this._mapper.Map<OperationType>(operationType_InsertAsync_Request));
            ResponseMessage responseMessage = new ResponseMessage();
            responseMessage.ReturnMessage(string.IsNullOrEmpty(msg), msg);
            return responseMessage;
        }

        /// <summary>
        /// 修改操作类型
        /// </summary>
        /// <param name="operationType_UpdateAsync_Request">修改操作类型</param>
        /// <returns></returns>
        [HttpPut, ApiVersion("1.0")]
        public async Task<ResponseMessage> UpdateAsync(OperationType_UpdateAsync_Request operationType_UpdateAsync_Request)
        {
            string msg = await this._operationTypeService.UpdateAsync(this._mapper.Map<OperationType>(operationType_UpdateAsync_Request));
            ResponseMessage responseMessage = new ResponseMessage();
            responseMessage.ReturnMessage(string.IsNullOrEmpty(msg), msg);
            return responseMessage;
        }

        /// <summary>
        /// 删除操作类型
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete, ApiVersion("1.0")]
        public async Task<ResponseMessage> UpdateAsync(string id)
        {
            string msg = await this._operationTypeService.DeleteAsync(id);
            ResponseMessage responseMessage = new ResponseMessage();
            responseMessage.ReturnMessage(string.IsNullOrEmpty(msg), msg);
            return responseMessage;
        }
    }
}
