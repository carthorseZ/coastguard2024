using System;
using System.Web;
using System.Web.Security;

public class BaseApplication : HttpApplication
{

	private static readonly Utils.Logger Logger = new Utils.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
	protected void Application_Start(object sender, EventArgs e) 
	{
		Utils.Logger.Configure();
		Logger.Info("Initilising Session");
	}
}