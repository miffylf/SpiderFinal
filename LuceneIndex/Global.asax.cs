using Quartz;
using Quartz.Impl;
using ServiceStack.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace LuceneIndex
{
    public class Global : System.Web.HttpApplication
    {
        private static ILog logger = LogManager.GetLogger(typeof(Global));
        private IScheduler sched;

        protected void Application_Start(object sender, EventArgs e)
        {
            //控制台就放在Main
            logger.Debug("Application_Start");
            log4net.Config.XmlConfigurator.Configure();
            //从配置中读取任务启动时间
            int indexStartHour = Convert.ToInt32(ConfigurationManager.AppSettings["IndexStartHour"]);
            int indexStartMin = Convert.ToInt32(ConfigurationManager.AppSettings["IndexStartMin"]);


            ISchedulerFactory sf = new StdSchedulerFactory();
            sched = sf.GetScheduler();
            JobDetail job = new JobDetail("job1", "group1", typeof(IndexJob));//IndexJob为实现了IJob接口的类
            //Trigger trigger = TriggerUtils.MakeDailyTrigger("tigger1", indexStartHour, indexStartMin);//每天10点3分执行
            Trigger trigger = TriggerUtils.MakeImmediateTrigger("tigger1", 1, new TimeSpan(0, 0, 20));
            trigger.JobName = "job1";
            trigger.JobGroup = "group1";
            trigger.Group = "group1";

            sched.AddJob(job, true);
            sched.ScheduleJob(trigger);
            //IIS启动了就不会来了
            sched.Start();

        }
    }
}