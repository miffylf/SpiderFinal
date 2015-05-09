using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using System.Configuration;
namespace Spider
{
    public class RedisCommon
    {
        static RedisClient mRedisClient;
        public RedisCommon()
        {
            string host = GetConfig("RedisConnection").ToString();
            int port = Convert.ToInt32(GetConfig("Port"));
            mRedisClient = new RedisClient(host, port);
        }
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static object GetConfig(string Key)
        {
            if (string.IsNullOrEmpty(Key))
            {
                return string.Empty;
            }
            object temp = ConfigurationManager.AppSettings[Key];
            return temp;
        }

        #region redis操作
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="Value"></param>
        public void SetValue(string Url, string Value)
        {
            try
            {
                bool result = mRedisClient.Set<string>(Url, Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public bool HashValue(string Url, string Value)
        //{
        //    try
        //    {
                
        //    }
        //}
        #endregion

    }
}
