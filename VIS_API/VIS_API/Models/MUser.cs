namespace VIS_API.Models
{
    public class MUser
    {
        #region 

        /******************************************************************************************
        
        user.department_fk = form["department_fk"].Text();
        user.role_fk = form["role_fk"].Text();
        user.acc = form["acc"].Text();
        user.name = form["name"].Text();
        user.pwd = form["pwd"].Text();
        user.email = form["email"].Text();
        user.phone = form["phone"].Text();
        user.factory = form["factory"].Text();
        user.color = form["color"].Text();
        user.status = form["status"].Text();
        user.created_at = form["created_at"].Text();
        user.updated_at = form["updated_at"].Text();

        //-----------------------------------------------------------------------------------------

        department_fk = "",
        role_fk = "",
        acc = "",
        name = "",
        pwd = "",
        email = "",
        phone = "",
        factory = "",
        color = "",
        status = "",
        created_at = "",
        updated_at = "",

        ******************************************************************************************/

        public uint department_fk { get; set; } = 0; // int(10) unsigned 
        public uint role_fk { get; set; } = 0; // int(10) unsigned 
        public int pk { get; set; } = 0; // int(10) unsigned 
        public string acc { get; set; } = ""; // varchar(255) 
        public string name { get; set; } = ""; // varchar(100) 
        public string pwd { get; set; } = ""; // varchar(255) 
        public string email { get; set; } = ""; // varchar(255) 
        public string phone { get; set; } = ""; // varchar(50) 
        public string factory { get; set; } = ""; // varchar(50) 
        public uint color { get; set; } = 0; // int(11) 版面顏色 1：淡藍色，2：淡紫色
        public string status { get; set; } = ""; // char(2) V:開啟, X:關閉
        public string created_at { get; set; } = ""; // datetime 
        public string updated_at { get; set; } = ""; // datetime
        public int company_fk { get; set; } // int(11)


        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////

        public MUser()
        {
        }

    }
}