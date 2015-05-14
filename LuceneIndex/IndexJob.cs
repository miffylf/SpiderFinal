using Quartz;
//======================================================================
// 所属项目：Spider
// 创 建 人：lifei
// 创建日期：2015/5/2
// 用    途：实现job自动执行
//====================================================================== 
using ServiceStack.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;

namespace LuceneIndex
{
    public class IndexJob:IJob
    {
        private ILog logger = LogManager.GetLogger(typeof(IndexJob));
        public void Execute(JobExecutionContext context)
        {
            try
            {
                logger.Debug("索引开始");
                LuceneLogic temp = new LuceneLogic();
                temp.CreateIndex();
                logger.Debug("索引结束");
            }
            catch (Exception ex)
            {
                logger.Debug("启动索引任务异常", ex);
            }
        }

    }
}