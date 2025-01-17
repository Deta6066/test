namespace VisLibrary.Models
{
    public class MMenu
    {
        #region 

        /******************************************************************************************
        
        MMenu.sort = form["sort"].Text();
        MMenu.node1 = form["node1"].Text();
        MMenu.node2 = form["node2"].Text();
        MMenu.icon = form["icon"].Text();
        MMenu.controller = form["controller"].Text();
        MMenu.extend = form["extend"].Text();
        MMenu.status = form["status"].Text();

        //-----------------------------------------------------------------------------------------

        sort = "",
        node1 = "",
        node2 = "",
        icon = "",
        controller = "",
        extend = "",
        status = "",

        ******************************************************************************************/

        public int pk { get; set; } = 0; // int(10) unsigned 
        public string sort { get; set; } = ""; // int(11) 
        public string node1 { get; set; } = ""; // varchar(100) 父單元
        public string node2 { get; set; } = ""; // varchar(100) 子單元
        public string icon { get; set; } = ""; // varchar(50) icon圖片檔名
        public string controller { get; set; } = ""; // varchar(100) 
        public string extend { get; set; } = ""; // varchar(50) 延伸功能
        public string status { get; set; } = ""; // char(2) 管理者控制, V:開啟, X:關閉
        public string code { get; set; } = "";

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////
        public override string ToString()
        {
            return $"pk: {pk}, sort: {sort}, node1: {node1}, node2: {node2}, icon: {icon}, controller: {controller}, extend: {extend}, status: {status}";
        }
        public MMenu()
        {
        }

    }
}