
namespace Common.Models
{
    public class User
    {
        public string U_GUID
        { get; set; }

        public string C_GUID
        { get; set; }

        public string UserName
        { get; set; }

        public string Password
        { get; set; }
        public string LoginName
        {
            get;
            set;
        }
        public string NickName
        {
            get;
            set;
        }
        public int State
        { get; set; }

        public string Language
        { get; set; }
        public string TelName
        { get; set; }
    }
}
