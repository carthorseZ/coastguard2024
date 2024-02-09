using System;
using System.ServiceModel.Configuration; 

namespace BackEnd
{
     public class ErrorHandlerElement : BehaviorExtensionElement  

     {  

         protected override object CreateBehavior()  

         {  

             return new ErrorHandler();  

         }  

     

         public override Type BehaviorType  

         {  

             get 

             {  

                 return typeof(ErrorHandler);  

             }  

         }  
     } 
}
