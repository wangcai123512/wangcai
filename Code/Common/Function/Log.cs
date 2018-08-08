using System;
using Common.UserIdentityVerify;
using PermissionSys.Models;
using SSOModel;

namespace Common.CommonFunction
{
    public class Log
    {
        /// <summary>
        /// 登录日志
        /// </summary>
        /// <param name="dat">人员信息</param>
        /// <param name="logicGuid">日志配对标识</param>
        /// <param name="hostIP">localhost地址</param>
        /// <param name="url">RM系统地址</param>
        /// <param name="timeOut">Session过期时间</param>
        /// <param name="type">T:Login F:LogOff</param>
        public void Loger(UserData dat, string logicGuid, string hostIP, string url, int timeOut, bool type)
        {
            T_EmployeeLoginOutLog empLog = new T_EmployeeLoginOutLog
            {
                EmployeeFullName = dat.LoginFullName,
                EmployeeGUID = dat.GUID,
                EmployeeNumber = dat.LoginName,
                GUID = Guid.NewGuid().ToString(),
                LogicGUID = logicGuid,
                Time = DateTime.Now,
                TimeUTC = DateTime.UtcNow,
                Type = type,
                SessionTimeOut = type ? timeOut : 0,
                LoginAddressIP = hostIP
            };
            new UserVerify(url).CreateLog(empLog);
        }

        ///// <summary>
        ///// 登出日志
        ///// </summary>
        ///// <param name="dat">人员信息</param>
        ///// <param name="logicGuid">日志配对标识</param>
        ///// <param name="hostIP">localhost地址</param>
        ///// <param name="url">RM系统地址</param>
        ///// <param name="timeOut">Session过期时间</param>
        //public void LogLogOff(UserData dat, string logicGuid, string hostIP, string url)
        //{
        //    T_EmployeeLoginOutLog empLog = new T_EmployeeLoginOutLog()
        //    {
        //        EmployeeFullName = dat.LoginFullName,
        //        EmployeeGUID = dat.GUID,
        //        EmployeeNumber = dat.LoginName,
        //        GUID = Guid.NewGuid().ToString(),
        //        LogicGUID = logicGuid,
        //        SessionTimeOut = 0,
        //        Time = DateTime.Now,
        //        TimeUTC = DateTime.UtcNow,
        //        LoginAddressIP = hostIP,
        //        Type = false
        //    };
        //    new UserVerify(url).CreateLog(empLog);
        //}
    }
}