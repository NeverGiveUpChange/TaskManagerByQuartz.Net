using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class qrtz_blob_triggers
    {
           public qrtz_blob_triggers(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TRIGGER_GROUP {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public byte[] BLOB_DATA {get;set;}

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
           public string TRIGGER_NAME {get;set;}

    }
}
