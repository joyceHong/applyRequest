using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace applyRequests.Models
{
    public class flowRole
    {
        /// <summary>
        /// 目前處理流程者的uid
        /// </summary>
        public string strRoleUserID
        {
            get;
            set;
        }

        /// <summary>
        /// 目前處理流程者名稱
        /// </summary>
        public string strRoleUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 現階段流程
        /// </summary>
        public string strProcessName
        {
            get;
            set;
        }

        /// <summary>
        /// 角色的email
        /// </summary>
        public string strEmail
        {
            get;
            set;
        }

        /// <summary>
        /// 主管ID
        /// </summary>
        public string strBossID
        {
            get;
            set;
        }

        /// <summary>
        /// 主管Email
        /// </summary>
        public string strBossEmail
        {
            get;
            set;
        }

        public string strProcessType
        {
            get;
            set;
        }
    }
}