using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GF.Infrastructure.Config
{
    /// <summary>
    /// 配置文件
    /// name.xml
    /// </summary>
    public class XmlConfig
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        private string FileName { get; set; }

        /// <summary>
        /// 配置结果
        /// </summary>
        public List<Dictionary<string, string>> ConfigValue { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">xml文件完整路径</param>
        public XmlConfig(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName should not be null or empty");

            this.FileName = fileName;
            this.Init();
        }

        private void Init()
        {
            XElement doc = XElement.Load(this.FileName);

        }
    }
}
