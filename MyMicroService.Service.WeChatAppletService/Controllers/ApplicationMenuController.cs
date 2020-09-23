using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMicroService.Infrastruct.ResponseModels;
using MyMicroService.Service.WeChatAppletService.APIModels;
using MyMicroService.Service.WeChatAppletService.IService;
using MyMicroService.Service.WeChatAppletService.Models;

namespace MyMicroService.Service.WeChatAppletService.Controllers
{
    /// <summary>
    ///  ApplicationMenuapi接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationMenuController : ControllerBase
    {
        /// <summary>
        /// ApplicationMenu接口
        /// </summary>
        private readonly IApplicationMenuService _applicationMenuService;

        /// <summary>
        /// 映射器
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// ApplicationMenuapi接口
        /// </summary>
        /// <param name="applicationMenuService">任务计划服务接口</param>
        /// <param name="mapper">映射器</param>
        public ApplicationMenuController(IApplicationMenuService applicationMenuService, IMapper mapper)
        {
            this._applicationMenuService = applicationMenuService;
            this._mapper = mapper;
        }

        /// <summary>
        /// 获取所有ApplicationMenu
        /// </summary>
        /// <returns></returns>
        [HttpGet, ApiVersion("1.0")]
        public async Task<ResponseMessage<List<ApplicationMenu_GetListAsync_Response>>> GetListAsync()
        {
            List<ApplicationMenu> applicationMenuList = await this._applicationMenuService.GetListAsync();
            ResponseMessage<List<ApplicationMenu_GetListAsync_Response>> responseMessage = new ResponseMessage<List<ApplicationMenu_GetListAsync_Response>>();
            responseMessage.ReturnData(true, this._mapper.Map<List<ApplicationMenu_GetListAsync_Response>>(applicationMenuList));
            return responseMessage;
        }
    }
}
