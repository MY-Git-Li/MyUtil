﻿//************************************************************************
//      author:     yuzhengyang
//      date:       2018.3.27 - 2018.6.3
//      desc:       工具描述
//      Copyright (c) yuzhengyang. All rights reserved.
//************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace MyUtil.Hardware
{
    public static class PatchInfoTool
    {
        /// <summary>
        /// 获取计算机所有补丁
        /// </summary>
        /// <returns></returns>
        public static List<string> Get()
        {
            List<string> rs = new List<string>();
            try
            {
                var searchQFE = new ManagementObjectSearcher("Select * from Win32_QuickFixEngineering");
                foreach (var item in searchQFE.Get())
                {
                    string _HotFixID = item.GetPropertyValue("HotFixID").ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(_HotFixID) && !rs.Contains(_HotFixID))
                    {
                        rs.Add(_HotFixID.ToUpper());
                    }
                }
            }
            catch { }
            return rs;
        }
        /// <summary>
        /// 检查是否存在补丁
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Exist(string name)
        {
            List<string> list = Get();
            if (!string.IsNullOrWhiteSpace(name) && !IsNullOrEmpty(list))
            {
                if (list.Any(x => x == name.ToUpper().Trim())) return true;
            }
            return false;
        }

        static bool IsNullOrEmpty<T>(IEnumerable<T> list)
        {
            if (list != null && list.Count() > 0)
                return false;
            return true;
        }

    }
}
