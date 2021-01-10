using System;
using Quartz;

namespace BumpBotWorker.Utility
{
    public static class QuartzServiceUtilities
    {
        public static void StartSimpleJob<TJob>(IScheduler scheduler, TimeSpan runInterval)
            where TJob : IJob
        {
            var jobName = typeof(TJob).FullName;
            var job = JobBuilder.Create<TJob>()
                .WithIdentity(jobName)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{jobName}.trigger")
                .StartAt(new DateTimeOffset(DateTime.UtcNow.AddSeconds(10)))
                .WithSimpleSchedule(scheduleBuilder =>
                    scheduleBuilder
                        .WithInterval(runInterval)
                        .RepeatForever())
                .Build();
            scheduler.ScheduleJob(job, trigger);
        }

        public static void StartCronJob<TJob>(IScheduler scheduler, string cron)
            where TJob : IJob
        {
            var jobName = typeof(TJob).FullName;
            var job = JobBuilder.Create<TJob>()
                .WithIdentity(jobName)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{jobName}.trigger")
                .WithSchedule(CronScheduleBuilder.CronSchedule(cron))
                .Build();
            scheduler.ScheduleJob(job, trigger);
        }
    }
}
