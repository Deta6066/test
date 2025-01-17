namespace VisLibrary.Models
{
    public class department
    {
        #region 

        /******************************************************************************************
        
        department.code = form["code"].Text();
        department.name = form["name"].Text();

        //-----------------------------------------------------------------------------------------

        code = "",
        name = "",

        ******************************************************************************************/

        public uint pk { get; set; } = 0; // int(10) unsigned 
        public string code { get; set; } = ""; // varchar(50) 部門代號
        public string name { get; set; } = ""; // varchar(100) 部門名稱

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////

        public department()
        {
        }

    }
}