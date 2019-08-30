﻿using System;
using System.Drawing;

namespace BiliAccount
{
    public class Linq
    {
        

        #region Public Methods

        /// <summary>
        /// 检查token可用性
        /// </summary>
        /// <param name="access_token">token</param>
        /// <returns>是否可用</returns>
        public static bool IsTokenAvailable(string access_token)
        {
            return Core.ByPassword.IsTokenAvailable(access_token);
        }

        /// <summary>
        /// 用账号密码登录
        /// </summary>
        /// <param name="username">用户名（邮箱/手机）</param>
        /// <param name="password">密码（明文）</param>
        /// <returns>账号信息实例</returns>
        public static Account LoginByPassword(string username, string password)
        {
            Core.ByPassword.Init();
            Account account = new Account()
            {
                UserName = username,
                Password = password
            };
            string hash, key;
            Core.ByPassword.GetKey(out hash, out key);
            password = Core.ByPassword.EncryptPwd(password, key, hash);
            Core.ByPassword.DoLogin(username, password, ref account);
            return account;
        }

        /// <summary>
        /// 用二维码登录
        /// </summary>
        /// <returns>二维码图片实例</returns>
        public static Bitmap LoginByQrCode()
        {
            return Core.ByQrCode.GetQrcode();
        }


        /// <summary>
        /// 二维码刷新处理程序
        /// </summary>
        /// <param name="newQrCode">新二维码</param>
        public delegate void QrCodeRefresh_Handle(Bitmap newQrCode);

        /// <summary>
        /// 二维码登录状态变更处理程序
        /// </summary>
        /// <param name="status">二维码状态</param>
        /// <param name="account">登录成功时有值，账号信息实例</param>
        public delegate void QrCodeStatus_Changed_Handle(QrCodeStatus status, Account account = null);



        #region Public Events

        /// <summary>
        /// 二维码刷新事件
        /// </summary>
        public static event QrCodeRefresh_Handle QrCodeRefresh;

        /// <summary>
        /// 二维码登录状态变更事件
        /// </summary>
        public static event QrCodeStatus_Changed_Handle QrCodeStatus_Changed;

        #endregion Public Events


        /// <summary>
        /// 二维码登录状态枚举
        /// </summary>
        public enum QrCodeStatus
        {
            Wating,
            Scaned,
            Success
        }

        /// <summary>
        /// token续期
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="refresh_token"></param>
        /// <returns>到期时间</returns>
        public static DateTime? RefreshToken(string access_token, string refresh_token)
        {
            return Core.ByPassword.RefreshToken(access_token, refresh_token);
        }

        /// <summary>
        /// SSO
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns>[0]=>(string)strCookies,[1]=>(string)csrf_token,[2]=>(DateTime)Expiress,[3]=>(CookieCollection)Cookies</returns>
        public static object[] SSO(string access_token)
        {
            return Core.ByPassword.SSO(access_token);
        }

        /// <summary>
        /// 调起二维码登录状态变更
        /// </summary>
        /// <param name="status">二维码状态</param>
        /// <param name="account">登录成功时有值，账号信息实例</param>
        internal static void RaiseQrCodeStatus_Changed(QrCodeStatus status, Account account = null)
        {
            QrCodeStatus_Changed(status, account);
        }

        internal static void RaiseQrCodeRefresh(Bitmap newQrCode)
        {
            QrCodeRefresh(newQrCode);
        }
        #endregion Public Methods
    }
}