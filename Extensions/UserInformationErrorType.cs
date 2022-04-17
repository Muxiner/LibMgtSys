using System.Collections.Generic;

namespace LibrariesManageSystem.Extensions
{
    public class UserInformationErrorType
    {
        public static readonly List<string> UserNameErrorType = new List<string>
        {
            " ",
            "OK",
            "用户名可用",
            "用户名不存在",
            "请输入用户名",
            "用户名已存在"
        };
        public static readonly List<string> UserPasswordErrorType = new List<string>
        {
            " ",
            "OK",
            "请输入密码",
            "密码错误",
            "两次输入的密码不一致"
        };
    }
}