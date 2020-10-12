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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Reflection;
using MiNET.Plugins.Attributes;

namespace MiNET.Plugins
{
	public abstract class Plugin
	{
		protected PluginContext Context { get; set; }

		[ThreadStatic] public static Player.Player CurrentPlayer = null;

		public PluginAttribute Info { get; internal set; }
		protected Plugin()
		{
			Info = LoadPluginInfo();
		}
		
		public void OnEnable(PluginContext context)
		{
			Context = context;
			OnEnable();
		}

		protected virtual void OnEnable()
		{
		}

		public virtual void OnDisable()
		{
		}
		
		#region OpenPlugin Initialisation

		private PluginAttribute LoadPluginInfo()
		{
			var type = GetType();

			//var info = new OpenPluginInfo();
			var info = type.GetCustomAttribute<PluginAttribute>();
			if (info == null) info = new PluginAttribute();

			// Fill info from the plugin's type/assembly
			var assembly = type.Assembly;

			if (string.IsNullOrWhiteSpace(info.Name))
				info.Name = type.FullName;

			if (string.IsNullOrWhiteSpace(info.Version) && !string.IsNullOrEmpty(assembly.Location))
				info.Version = AssemblyName.GetAssemblyName(assembly.Location)?.Version?.ToString() ?? "";

			//info.Version = assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version ?? "";

			if (string.IsNullOrWhiteSpace(info.Description))
				info.Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? "";

			if (string.IsNullOrWhiteSpace(info.Author))
				info.Author = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? "";

			return info;
		}

		#endregion
	}
}