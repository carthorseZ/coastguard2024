using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
namespace BackEnd
{
    [DataContract]
    public class JSONErrorMessage
    {
        public JSONErrorMessage(Exception error)
        {
            Message = error.Message;
            StackTrace = error.StackTrace;
            Exception = error.GetType().Name;
        }

        [DataMember(Name = "stacktrace")]
        public string StackTrace { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "exception-name")]
        public string Exception { get; set; }
    } 

 }
