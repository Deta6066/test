namespace VisLibrary.Models.JsonModel
{
    public class MAllowMenu
    {
        public uint menu_pk { get; set; } = 0; // MMenu.pk
        public string controller { get; set; } = ""; // MMenu.controller
        public string extend { get; set; } = ""; // MMenu.extend
    }
}
