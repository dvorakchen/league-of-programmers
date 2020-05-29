using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Domain.Administrator
{
    /// <summary>
    /// 角色，一般给后台管理员使用
    /// </summary>
    class Role
    {
        /// <summary>
        /// 角色分类
        /// </summary>
        [Flags]
        public enum Categories
        {
            Admin = 1 << 0,
            Auditor = 2 << 1,
        }
    }
}
