//======================================================================
// 所属项目：Spider
// 创 建 人：lifei
// 创建日期：2015/5/2
// 用    途：redis缓存
//====================================================================== 
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

        #region 获取配置文件信息
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
        #endregion

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

 
        #endregion

    }
}
