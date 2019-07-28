﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SummaryTable.Helper
{
    /// <summary>
    /// 用于抓取报告中的有效内容
    /// </summary>
    public static class GetUsefulContent
    {

        /// <summary>
        /// 通过特定字符串及截取长度获取目标字符串
        /// </summary>
        /// <param name="content">全文字符串</param>
        /// <param name="StartChars">特定字符串</param>
        /// <param name="Length">截取长度</param>
        /// <returns>目标字符串</returns>
        public static string CommonMethod(string content,string StartChars,int Length)
        {
            if (content.IndexOf(StartChars) < 0)
            {
                return "未能找到";
            }
            int StartIndex = content.IndexOf(StartChars) + (StartChars).Length;

            //如果截取末尾位置索引大于全文长度则截取到全文末尾处
            int distance = content.Length - (StartIndex + Length + 1);
            if (distance < 0)
            {
                return content.Substring(StartIndex, Length + distance);
            }
            return content.Substring(StartIndex, Length);
        }

        /// <summary>
        /// 将中文格式长日期转为YYYY/MM/DD，并去除月份日份小于10的首位0
        /// </summary>
        /// <param name="OriginTime"></param>
        /// <returns></returns>
        public static string TimeConvert(string OriginTime)
        {
            char[] separators = { '年', '月', '日' };
            string[] content = OriginTime.Split(separators);
            string YYYY = content[0];
            string MM = content[1];
            if (content[1].IndexOf("0") >= 0)
            {
                MM = content[1].Replace("0","");
            }
            string DD = content[2];
            if (content[2].Length > 1)
            {
                char[] temp = content[2].ToCharArray();
                if (temp[0] == '0')
                {
                    DD = content[2].Replace("0","");
                }
            }
            if (content[2].IndexOf('0') > 0 && content[2].IndexOf('1') < 0 && content[2].IndexOf('2') < 0 && content[2].IndexOf('3') < 0)
            {
                DD = content[2].Replace("0", "");
            }
            return YYYY + "/" + MM + "/" + DD;
        }


        /// <summary>
        /// 通过正则表达式找出文中符合规则的内容
        /// </summary>
        /// <param name="content">全文字符串</param>
        /// <param name="regexPattern">正则表达式</param>
        /// <returns></returns>
        public static string FindByRegex(string content,string regexPattern)
        {
            Match match = Regex.Match(content, regexPattern);
            return match.Value;
        }

        /// <summary>
        /// 评估编号
        /// </summary>
        public static string getCode(string content){
            string result = CommonMethod(content, "\r致估价委托人函\r", 21);
            return result;
        }
        /// <summary>
        /// 价值时点
        /// </summary>
        public static string getValueTime(string content){
            string result = CommonMethod(content, "价值时点为", 11);
            result = TimeConvert(result);
            return result;
        }
        /// <summary>
        /// 委托方名称
        /// </summary>
        public static string getCustomer(string content){
            string result = FindByRegex(content, @"[\r\n][^人]{2,3}：");//工商模板
            result = result.Replace("\r","").Replace("：","");
            if (string.IsNullOrWhiteSpace(result))
            {
                result = FindByRegex(content, @"对[\u4e00-\u9fa5]{2,3}所属");//中信模板
                result = result.Replace("对","").Replace("所属","");
            }
            return result;
        }
        /// <summary>
        /// 项目坐落
        /// </summary>
        public static string getLocation(string content){
            string result = FindByRegex(content, @"位于.{10,40}住宅房地产");
            result.Replace("位于", "").Replace("住宅房地产", "");
            return result;
        }
        /// <summary>
        /// 建筑面积
        /// </summary>
        public static string getArchitecherArea(string content){
            string result = "";
            int StartIndex = -1, EndIndex = -1, Length = 0;
            StartIndex += content.IndexOf(Environment.NewLine.ToCharArray() + "致估价委托人函");
            Length = StartIndex + 23;
            result = content.Substring(StartIndex, Length);
            return result;
        }
        /// <summary>
        /// 评估单价
        /// </summary>
        public static string getSingleValue(string content){
            string result = "";
            int StartIndex = -1, EndIndex = -1, Length = 0;
            StartIndex += content.IndexOf(Environment.NewLine.ToCharArray() + "致估价委托人函");
            Length = StartIndex + 23;
            result = content.Substring(StartIndex, Length);
            return result;
        }
        /// <summary>
        /// 评估总价
        /// </summary>
        public static string getTotalValue(string content){
            string result = "";
            int StartIndex = -1, EndIndex = -1, Length = 0;
            StartIndex += content.IndexOf(Environment.NewLine.ToCharArray() + "致估价委托人函");
            Length = StartIndex + 23;
            result = content.Substring(StartIndex, Length);
            return result;
        }
    }
}
