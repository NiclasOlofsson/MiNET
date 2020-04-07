#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using log4net;
using log4net.Core;

namespace MiNET.Utils
{
	public static class LogHelpers
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(LogHelpers));

		// These are all the levels built into log4net
		//public static readonly Level Off = new Level(int.MaxValue, "OFF");
		//public static readonly Level Log4Net_Debug = new Level(120000, "log4net:DEBUG");
		//public static readonly Level Emergency = new Level(120000, "EMERGENCY");
		//public static readonly Level Fatal = new Level(110000, "FATAL");
		//public static readonly Level Alert = new Level(100000, "ALERT");
		//public static readonly Level Critical = new Level(90000, "CRITICAL");
		//public static readonly Level Severe = new Level(80000, "SEVERE");
		//public static readonly Level Error = new Level(70000, "ERROR");
		//public static readonly Level Warn = new Level(60000, "WARN");
		//public static readonly Level Notice = new Level(50000, "NOTICE");
		//public static readonly Level Info = new Level(40000, "INFO");
		//public static readonly Level Debug = new Level(30000, "DEBUG");
		//public static readonly Level Fine = new Level(30000, "FINE");
		//public static readonly Level Trace = new Level(20000, "TRACE");
		//public static readonly Level Finer = new Level(20000, "FINER");
		//public static readonly Level Verbose = new Level(10000, "VERBOSE");
		//public static readonly Level Finest = new Level(10000, "FINEST");
		//public static readonly Level All = new Level(int.MinValue, "ALL");

		private static readonly Type _declaringType = typeof(LogHelpers);

		public static bool IsTraceEnabled(this ILog log)
		{
			return log.Logger.IsEnabledFor(Level.Trace);
		}

		public static void Trace(this ILog log, string message, Exception exception)
		{
			log.Logger.Log(_declaringType, Level.Trace, message, exception);
		}

		public static void Trace(this ILog log, string message)
		{
			log.Trace(message, null);
		}

		public static bool IsVerboseEnabled(this ILog log)
		{
			return log.Logger.IsEnabledFor(Level.Verbose);
		}

		public static void Verbose(this ILog log, string message, Exception exception)
		{
			log.Logger.Log(_declaringType, Level.Verbose, message, exception);
		}

		public static void Verbose(this ILog log, string message)
		{
			log.Verbose(message, null);
		}
	}
}