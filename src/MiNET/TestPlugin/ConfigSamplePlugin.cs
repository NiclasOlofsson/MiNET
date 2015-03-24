using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;
using log4net;
using MiNET;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;

namespace TestPlugin
{
	[Plugin]
	public class PlayerLoginPlugin
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (PlayerLoginPlugin));

		[PacketHandler]
		public Package OnLogin(McpeLogin packet, Player newPlayer)
		{
			Log.InfoFormat("Player {0} connected from {1}", newPlayer.Username, newPlayer.EndPoint.Address);

			return packet;
		}
	}


	public class ConfigSamplePlugin : IPlugin
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ConfigSamplePlugin));
		private PluginContext _context;

		[DataContract]
		public class MyPluginConfiguration
		{
			[DataMember]
			public string Message { get; set; }

			[DataMember]
			public List<string> Rules { get; set; }
		}

		public void OnEnable(PluginContext context)
		{
			_context = context;

			// Build a sample config
			var config = new MyPluginConfiguration
			{
				Message = "Hello world of configurations!",
				Rules = new List<string>
				{
					"All rules apply",
					"No rules apply",
					"The only rule that apply, is no rules",
					"JSON rule!"
				}
			};

			string pluginDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

			// JSON FIRST

			// Write it
			using (FileStream stream = new FileStream(Path.Combine(pluginDirectory, "config.json"), FileMode.Create))
			{
				DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof (MyPluginConfiguration));
				ser.WriteObject(stream, config);
			}

			// Read it
			using (FileStream stream = new FileStream(Path.Combine(pluginDirectory, "config.txt"), FileMode.Open))
			{
				DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof (MyPluginConfiguration));
				MyPluginConfiguration readConfig = (MyPluginConfiguration) ser.ReadObject(stream);
				Log.InfoFormat("Message read is {0}", readConfig.Message);
			}

			// NOW XML

			// Write it
			using (FileStream stream = new FileStream(Path.Combine(pluginDirectory, "config.xml"), FileMode.Create))
			{
				var ser = new XmlSerializer(typeof (MyPluginConfiguration));
				ser.Serialize(stream, config);
			}

			// Read it
			using (FileStream stream = new FileStream(Path.Combine(pluginDirectory, "config.xml"), FileMode.Open))
			{
				XmlSerializer ser = new XmlSerializer(typeof (MyPluginConfiguration));
				MyPluginConfiguration readConfig = (MyPluginConfiguration) ser.Deserialize(stream);
				Log.InfoFormat("Message read is {0}", readConfig.Message);
			}
		}

		public void OnDisable()
		{
		}
	}
}