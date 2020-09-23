using System.ComponentModel.DataAnnotations;

namespace MyMicroService.Service.WeChatAppletService.Models
{
    /// <summary>
    /// ApplicationMenu
    /// </summary>
    public partial class ApplicationMenu
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

    }
}

