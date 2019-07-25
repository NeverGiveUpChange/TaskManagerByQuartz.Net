using Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_Model
{
    public class DbContext
    {
        public SqlSugarClient Db;
        public DbContext()
        {
            Db = new SqlSugarClient(new ConnectionConfig { ConnectionString = @"server=localhost;userid=root;password=chenlong@163.com;persistsecurityinfo=True;database=quartz_net", DbType = DbType.MySql, IsAutoCloseConnection = true, InitKeyType = InitKeyType.SystemTable });
        }
        public DbSet<custom_job_infoes> customer_quartzjobinfoDb { get { return new DbSet<custom_job_infoes>(Db); } }
        public DbSet<qrtz_blob_triggers> qrtz_blob_triggersDb { get { return new DbSet<qrtz_blob_triggers>(Db); } }
        public DbSet<qrtz_calendars> qrtz_calendarsDb { get { return new DbSet<qrtz_calendars>(Db); } }
        public DbSet<qrtz_cron_triggers> qrtz_cron_triggersDb { get { return new DbSet<qrtz_cron_triggers>(Db); } }
        public DbSet<qrtz_fired_triggers> qrtz_fired_triggersDb { get { return new DbSet<qrtz_fired_triggers>(Db); } }
        public DbSet<qrtz_job_details> qrtz_job_detailsDb { get { return new DbSet<qrtz_job_details>(Db); } }
        public DbSet<qrtz_locks> qrtz_locksDb { get { return new DbSet<qrtz_locks>(Db); } }
        public DbSet<qrtz_paused_trigger_grps> qrtz_paused_trigger_grpsDb { get { return new DbSet<qrtz_paused_trigger_grps>(Db); } }
        public DbSet<qrtz_scheduler_state> qrtz_scheduler_stateDb { get { return new DbSet<qrtz_scheduler_state>(Db); } }
        public DbSet<qrtz_simple_triggers> qrtz_simple_triggersDb { get { return new DbSet<qrtz_simple_triggers>(Db); } }

        public DbSet<qrtz_simprop_triggers> qrtz_simprop_triggersDb { get { return new DbSet<qrtz_simprop_triggers>(Db); } }
        public DbSet<qrtz_triggers> qrtz_triggersDb { get { return new DbSet<qrtz_triggers>(Db); } }
    }
    /// <summary>
    /// SimpleClient封装了单表大部分操作，此类为扩展类，以提供自定义的单表扩展方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbSet<T> : SimpleClient<T> where T : class, new()
    {
        public DbSet(SqlSugarClient context) : base(context) { }
    }
}

