using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisLibrary.Models.API;
using VisLibrary.Models;
using Microsoft.AspNetCore.Identity;
using VisLibrary.Service.Interface;
using VisLibrary.Models.View;
using VisLibrary.Models.JsonModel;
using VisLibrary.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace VIS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PagesController : ControllerBase
    {
        private readonly ISRole _sRole;
        private readonly ISMenu _sMenu;
        public PagesController(ISRole sRole,ISMenu sMenu)
        {
            _sRole = sRole;
           _sMenu = sMenu;
        }
        [HttpPost("GetPageList")]
        public async Task<IActionResult> GetList()
        {
            var data = await _sRole.Get(1);
            if (data == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, new
                {
                    code = "204",
                    message = "請求成功但無資料",
                    data = new VMenu()
                });
            }
            var data3= Utility.FromJson<List<MAllowMenu>>(data.access);
            var dada2 = await _sMenu.GetList();
            var VMenu = new VMenu
            {
                Menu = dada2,
                allowMenu = data3
            };
            return Ok(VMenu);
        }
        [HttpPost("GetMenuList")]
        public async Task<IActionResult> GetMenu()
        {
            //未來token加入權限管理
            var companyFk = User.FindFirst("companyFk")?.Value;

            var data = await _sRole.Get(1);
            var menuLsit = await _sMenu.GetList();
           
            return Ok(menuLsit);
        }
    }
}
