using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Runtime.Serialization.Json;
using System.Web;


using System.Collections.ObjectModel;  
using System.Diagnostics;  

using System.Net;

namespace BackEnd
{

    public class ErrorHandler : IErrorHandler,IServiceBehavior
    {

             public void AddBindingParameters(  

             ServiceDescription serviceDescription,  

             ServiceHostBase serviceHostBase,  

             Collection<ServiceEndpoint> endpoints,  

             BindingParameterCollection bindingParameters)  

         {  

         } 


        private static readonly Utils.Logger Logger = new Utils.Logger(typeof(ErrorHandler));
         public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)  

         {  

             IErrorHandler errorHandler = new ErrorHandler();  

     

             foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)  

             {  

                 ChannelDispatcher channelDispatcher = channelDispatcherBase as ChannelDispatcher;  

     

                 if (channelDispatcher != null)  

                 {  

                     channelDispatcher.ErrorHandlers.Add(errorHandler);  

                 }  

             }  

         }  
         public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)  

         {  

         } 

        public bool HandleError(Exception error)
        {

            Logger.ErrorEx(error, "Caught by Top level");
            //Tell the system that we handle all errors here. 
            return true;
        }

        public void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            Logger.Debug("Calling Provide Fault");
            if (error is FaultException<int>)
            {
                FaultException<int> fe = (FaultException<int>)error;

                //Detail for the returned value 
                int faultCode = fe.Detail;
                string cause = fe.Message;

                //The json serializable object 
                JSONErrorMessage msErrObject = new JSONErrorMessage(error);
                
                //The fault to be returned 
                fault = Message.CreateMessage(version, "", msErrObject, new DataContractJsonSerializer(msErrObject.GetType()));

                // tell WCF to use JSON encoding rather than default XML 
                WebBodyFormatMessageProperty wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);

                // Add the formatter to the fault 
                fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);

                //Modify response 
                HttpResponseMessageProperty rmp = new HttpResponseMessageProperty();

                // return custom error code, 400. 
                rmp.StatusCode = System.Net.HttpStatusCode.BadRequest;
                rmp.StatusDescription = "Bad request";
                
                //Mark the jsonerror and json content 
                rmp.Headers[HttpResponseHeader.ContentType] = "application/json";
                rmp.Headers["jsonerror"] = "true";

                //Add to fault 
                fault.Properties.Add(HttpResponseMessageProperty.Name, rmp);
            }
            else
            {
                //Arbitraty error 
                JSONErrorMessage msErrObject = new JSONErrorMessage(error);

                // create a fault message containing our FaultContract object 
                fault = Message.CreateMessage(version, "", msErrObject, new DataContractJsonSerializer(msErrObject.GetType()));

                // tell WCF to use JSON encoding rather than default XML 
                var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);
                fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);

                //Modify response 
                var rmp = new HttpResponseMessageProperty();

                rmp.Headers[HttpResponseHeader.ContentType] = "application/json";
                rmp.Headers["jsonerror"] = "true";

                //Internal server error with exception mesasage as status. 
                rmp.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                rmp.StatusDescription = error.Message;

                fault.Properties.Add(HttpResponseMessageProperty.Name, rmp);
            }
        }



    }
}
