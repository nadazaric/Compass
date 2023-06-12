using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using travel_agent.WindowsAndPages;

namespace travel_agent.Help
{
	[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
	[ComVisible(true)]
	public class JavaScriptControlHelp
	{
		MainWindow prozor;
		public JavaScriptControlHelp(MainWindow w)
		{
			prozor = w;
		}

		public void RunFromJavascript(string param)
		{
			
		}

	}
}
