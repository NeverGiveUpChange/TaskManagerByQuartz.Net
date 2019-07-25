using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class qrtz_locks
    {
           public qrtz_locks(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string SCHED_NAME {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string LOCK_NAME {get;set;}

    }
}
