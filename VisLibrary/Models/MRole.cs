
using VisLibrary.Utilities;

namespace VisLibrary.Models
{
    public class MRole
    {
        #region 

        /******************************************************************************************
        
        MRole.name = form["name"].Text();
        MRole.access = form["access"].Text();

        //-----------------------------------------------------------------------------------------

        name = "",
        access = "",

        ******************************************************************************************/

        public uint pk { get; set; } = 0; // int(10) unsigned 
        public string name { get; set; } = ""; // varchar(50) 角色名稱
        public string access { get; set; } = ""; // longtext json:可瀏覽的menu

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////

        public JsonModel.MAllowMenu[] MAllowMenu() => Utility.FromJson<JsonModel.MAllowMenu[]>(access) ?? Array.Empty<JsonModel.MAllowMenu>();
        public JsonModel.MAllowMenu? MAllowMenu(string controller) => MAllowMenu().Where(x => x.controller == controller).SingleOrDefault();

        public MRole()
        {
        }

    }
}