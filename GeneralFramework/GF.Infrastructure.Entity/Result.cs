using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GF.Infrastructure.Entity
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Code
        /// </summary>
        public ResultCode Code { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// Msg
        /// </summary>
        public string Msg { get; set; }
    }
    public enum ResultCode
    {
        /// <summary>
        /// 失败
        /// </summary>
        Failure = 101,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 102
    }
}
