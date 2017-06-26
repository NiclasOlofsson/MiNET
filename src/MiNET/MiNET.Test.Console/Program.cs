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
			tests.HighPrecTimeLoadTest();

			System.Console.ReadLine();

		}
	}
}
