using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace applyRequests.Models
{
    public class FlowItem
    {
        /// <summary>
        /// 流程編號
        /// </summary>
        public int flowItemID
        {
            get;
            set;
        }

        /// <summary>
        /// 流程名稱
        /// </summary>
        public string flowItemName
        {
            get;
            set;
        }

        /// <summary>
        /// 流程頁面
        /// </summary>
        public string flowItemView
        {
            get;
            set;
        }

        /// <summary>
        /// 下一階段的流程
        /// </summary>
        public int flowItemNextID
        {
            get;
            set;
        }

        /// <summary>
        /// 處理角色的函式
        /// </summary>
        public string flowRoleFunc
        {
            get;
            set;
        }

        /// <summary>
        /// 流程動作
        /// </summary>
        public List<flowAction> flowActions
        {
            get;
            set;
        }
    }

    public class flowAction
    {
        public string title
        {
            get;
            set;
        }

        public string value
        {
            get;
            set;
        }
    }
}