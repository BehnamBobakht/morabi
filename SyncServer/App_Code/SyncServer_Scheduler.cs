using DataLayer.Schedules;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SyncServer
{
    public class SyncServer_Scheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            SmsScheduler(scheduler);
            EmailScheduler(scheduler);
            ShrinkDatabaseScheduler(scheduler);
            //AlexaScheduler(scheduler);
            GoldPriceScheduler(scheduler);
            ExameScheduler(scheduler);
            DiscountScheduler(scheduler);
            CartScheduler(scheduler);
            OrderToArchiveScheduler(scheduler);
        }
        public static void ExameScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_Exame>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInMinutes(1)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
        public static void DiscountScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_Discount>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInMinutes(2)
                        .RepeatForever())
                .StartNow()
                .Build();
            scheduler.ScheduleJob(job, trigger);
        }
        public static void CartScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_Cart>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInHours(10)
                        .RepeatForever())
                .StartNow()
                .Build();
            scheduler.ScheduleJob(job, trigger);
        }    
        public static void OrderToArchiveScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_OrderToArchive>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInHours(12)
                        .RepeatForever())
                .StartNow()
                .Build();
            scheduler.ScheduleJob(job, trigger);
        }
        public static void AlexaScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_Alexa>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInHours(1)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }



        public static void ShrinkDatabaseScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_ShrinkDatabase>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInHours(24)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }



        public static void EmailScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_Email>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInSeconds(10)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static void SmsScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_Sms>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInSeconds(1)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }


        public static void GoldPriceScheduler(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<Sync_GoldPrice>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(p =>
                    p.WithIntervalInMinutes(30)
                    .RepeatForever())
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}