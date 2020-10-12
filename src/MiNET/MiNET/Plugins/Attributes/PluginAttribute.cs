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

namespace MiNET.Plugins.Attributes
{
	/// <summary>
	///     An attribute containing information about a plugin.
	///     It can be applied on any classes implementing the Plugin <see cref="Plugin"/> class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class PluginAttribute : Attribute
	{
		/// <summary>
		/// The name of the plugin
		/// <example>MyPlugin</example>
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The version of the plugin
		/// <example>1.4.1</example>
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		/// A human friendly description of the functionality this plugin provides
		/// <example>This plugin is so 1337. You can set yourself on fire.</example>
		/// </summary>
		public string Description { get; set; }


		/// <summary>
		/// Uniqely identifies who developed this plugin
		/// <example>TruDan</example>
		/// <example>TruDan &lt;trudan@example.com&gt;</example>
		/// </summary>
		public string Author { get; set; }

		/// <summary>
		/// Allows you to list multiple authors, if it is a collaborative project.
		/// <seealso cref="Author"/>
		/// </summary>
		public string[] Authors { get; set; }


		/// <summary>
		/// The plugin's or author's website.
		/// <example>example.com/MyAwesomePlugin</example>
		/// </summary>
		public string Website { get; set; }
	}
}