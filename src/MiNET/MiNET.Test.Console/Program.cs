using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Test.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			//MinetAnvilTest test = new MinetAnvilTest();
			//test.LoadFullAnvilRegionLoadTest();

			MinetServerTest tests = new MinetServerTest();
			tests.HighPrecTimerSignalingLoadTest();
			System.Console.WriteLine("Running ...");

			System.Console.WriteLine("<Enter> to ABORT");
			System.Console.ReadLine();
			tests.cancel.Cancel();

			tests.PrintResults();

			System.Console.WriteLine("<Enter> to exit");
			System.Console.ReadLine();
		}
	}
}
