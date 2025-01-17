namespace VisLibrary.Models
{
    public class error_log
    {
        #region 

        /******************************************************************************************
        
        error_log.title = form["title"].Text();
        error_log.message = form["message"].Text();
        error_log.created_at = form["created_at"].Text();

        //-----------------------------------------------------------------------------------------

        title = "",
        message = "",
        created_at = "",

        ******************************************************************************************/

        public uint pk { get; set; } = 0; // int(10) unsigned 
        public string title { get; set; } = ""; // varchar(1000) 訊息標題
        public string message { get; set; } = ""; // varchar(5000) 訊息內容
        public string created_at { get; set; } = ""; // datetime

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////

        public error_log()
        {
        }

    }
}