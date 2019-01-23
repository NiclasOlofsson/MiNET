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
using System.Collections.Generic;
using log4net;
using Newtonsoft.Json;

namespace MiNET.UI
{
	public class CustomForm : Form
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(CustomForm));

		public CustomForm()
		{
			Type = "custom_form";
		}

		public List<CustomElement> Content { get; set; }

		public override void FromJson(string json, Player player)
		{
			var jsonSerializerSettings = new JsonSerializerSettings
			{
				PreserveReferencesHandling = PreserveReferencesHandling.None,
				Formatting = Formatting.Indented,
			};

			var parsedResult = JsonConvert.DeserializeObject<List<object>>(json);
			Log.Debug($"Form JSON\n{JsonConvert.SerializeObject(parsedResult, jsonSerializerSettings)}");

			if (parsedResult == null) return; // Pressed [x]

			for (var i = 0; i < Content.Count; i++)
			{
				var element = Content[i];
				if (element is Input) ((Input) element).Value = (string) parsedResult[i];
				else if (element is Toggle) ((Toggle) element).Value = (bool) parsedResult[i];
				else if (element is Slider) ((Slider) element).Value = (float) (double) parsedResult[i];
				else if (element is StepSlider) ((StepSlider) element).Value = (int) (long) parsedResult[i];
				else if (element is Dropdown) ((Dropdown) element).Value = (int) (long) parsedResult[i];
			}

			Execute(player);
		}

		[JsonIgnore] public Action<Player, CustomForm> ExecuteAction { get; set; }

		public void Execute(Player player)
		{
			ExecuteAction?.Invoke(player, this);
		}
	}
}