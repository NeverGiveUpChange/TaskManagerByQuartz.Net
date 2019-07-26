using Quartz.Net_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 更新数据库
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new DbContext();
            db.Db.DbFirst.CreateClassFile(@"E:\TaskManagerByQuartz.Net\Quartz.Net_Model\Models");
        }
    }
}
