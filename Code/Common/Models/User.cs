
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
        public int State
        { get; set; }
    }
}
