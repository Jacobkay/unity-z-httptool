using System.Collections.Generic;

namespace ZTools
{
    /// <summary>
    /// 自定义设置接口返回的公共格式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseResult1<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int code;

        /// <summary>
        /// 消息
        /// </summary>
        public string message;

        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> data { get; set; }

        /// <summary>
        /// 返回数量
        /// </summary>
        public int count { get; set; }
    }
}
