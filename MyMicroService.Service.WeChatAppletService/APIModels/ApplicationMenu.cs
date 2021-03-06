﻿using MyMicroService.Infrastruct.Attributes;
using MyMicroService.Infrastruct.Enums;
using MyMicroService.Service.WeChatAppletService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMicroService.Service.WeChatAppletService.APIModels
{
    /// <summary>
    /// ApplicationMenu_GetListAsync_Response返回对象
    /// </summary>
    [AutoMapper(typeof(ApplicationMenu), EnumAutoMapperDirection.Forward)]
    public class ApplicationMenu_GetListAsync_Response
    {
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "", Order = 1)]
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称", Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        [Display(Name = "路由", Order = 3)]
        public string Route { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        [Display(Name = "图片路径", Order = 4)]
        public string ImageUrl { get; set; }
    }
}
