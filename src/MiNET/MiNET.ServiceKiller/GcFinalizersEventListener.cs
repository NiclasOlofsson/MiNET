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

using System.Diagnostics.Tracing;
using log4net;

namespace MiNET.ServiceKiller
{
	public class GcFinalizersEventListener : EventListener
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(GcFinalizersEventListener));

		// from https://docs.microsoft.com/en-us/dotnet/framework/performance/garbage-collection-etw-events
		private const int GC_KEYWORD = 0x0000001;
		private const int TYPE_KEYWORD = 0x0080000;
		private const int GCHEAPANDTYPENAMES_KEYWORD = 0x1000000;

		protected override void OnEventSourceCreated(EventSource eventSource)
		{
			Log.Warn($"{eventSource.Guid} | {eventSource.Name}");

			// look for .NET Garbage Collection events
			if (eventSource.Name.Equals("Microsoft-Windows-DotNETRuntime"))
			{
				EnableEvents(
					eventSource,
					EventLevel.Verbose,
					(EventKeywords) (GC_KEYWORD | GCHEAPANDTYPENAMES_KEYWORD | TYPE_KEYWORD)
				);
			}
		}

		// from https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-net-core-2-2/
		// Called whenever an event is written.
		protected override void OnEventWritten(EventWrittenEventArgs eventData)
		{
			if (eventData.EventId == 10) return;
			if (eventData.EventId == 33) return;
			if (eventData.EventId == 200) return;

			if (eventData.EventId == 1)
			{
				for (int i = 0; i < eventData.Payload.Count; i++)
				{
					//string payloadString = eventData.Payload[i] != null ? eventData.Payload[i].ToString() : string.Empty;
					//sb.Append($"    Name = \"{eventData.PayloadNames[i]}\" Value = \"{payloadString}\"");
					if (eventData.PayloadNames[i] == "Type" && eventData.Payload[i].ToString() != "0") return;
				}
			}
			else if (eventData.EventId == 2)
			{
				//for (int i = 0; i < eventData.Payload.Count; i++)
				//{
				//	if (eventData.PayloadNames[i] == "Type" && eventData.Payload[i].ToString() != "0") return;
				//}
			}
			Log.Warn($"ThreadID = {eventData.OSThreadId} ID = {eventData.EventId} Name = {eventData.EventName}");

			// Write the contents of the event

		}
	}
}