using System;
using log4net;
using log4net.Config;

namespace catalogue.Service.Listener {
	class Program {
		static void Main(string[] args) {
			
			var appHost = new AppHost();
			appHost.Init();
			appHost.Start("http://localhost:2211/");

			Console.Read();
		}
	}
}
