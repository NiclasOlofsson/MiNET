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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2021 Niclas Olofsson.
// All Rights Reserved.
#endregion

using System;

namespace MiNET.Net
{
	public class CommandOutputMessage
	{
		public bool IsInternal { get; set; }
		public string MessageId { get; set; }
		public string[] Parameters { get; set; }
		public string RawMessage { get; set; }

		/// <inheritdoc />
		public override string ToString()
		{
			switch (MessageId)
			{
				case "commands.generic.unknown":
					return $"Unknown command: {Parameters[0]}";
			}
			return $"{{MessageId={MessageId}, IsInternal={IsInternal}, Parameters={String.Join(',', Parameters)}}}";
		}
	}

	public enum CommandOutputType
	{
		None = 0,
		Last = 1,
		Silent = 2,
		All = 3,
		DataSet = 4,
	}

	public partial class McpeCommandOutput
	{
		// The wire carries the output type as a name, not as the enum ordinal. Indexed by CommandOutputType.
		private static readonly string[] OutputTypeNames = {"none", "lastoutput", "silent", "alloutput", "dataset"};

		public CommandOriginData OriginData { get; set; }
		public CommandOutputType OutputType { get; set; }
		public uint SuccessCount { get; set; }
		public CommandOutputMessage[] Messages { get; set; }
		public string Data { get; set; }

		partial void AfterEncode()
		{
			Write(OriginData);
			Write(GetOutputTypeName(OutputType));
			Write(SuccessCount);

			CommandOutputMessage[] messages = Messages ?? Array.Empty<CommandOutputMessage>();
			WriteUnsignedVarInt((uint) messages.Length);

			foreach (CommandOutputMessage message in messages)
			{
				WriteCommandOutputMessage(message);
			}

			Write(Data != null);
			if (Data != null)
			{
				Write(Data);
			}
		}

		partial void AfterDecode()
		{
			OriginData = ReadCommandOriginData();
			OutputType = GetOutputType(ReadString());
			SuccessCount = ReadUint();

			var messageCount = ReadUnsignedVarInt();
			Messages = new CommandOutputMessage[messageCount];

			for (int i = 0; i < Messages.Length; i++)
			{
				Messages[i] = ReadCommandOutputMessage();
			}

			if (ReadBool())
			{
				Data = ReadString();
			}
		}

		private static string GetOutputTypeName(CommandOutputType type)
		{
			int index = (int) type;
			return index >= 0 && index < OutputTypeNames.Length ? OutputTypeNames[index] : OutputTypeNames[0];
		}

		private static CommandOutputType GetOutputType(string name)
		{
			for (int i = 0; i < OutputTypeNames.Length; i++)
			{
				if (OutputTypeNames[i] == name) return (CommandOutputType) i;
			}

			return CommandOutputType.None;
		}

		private void WriteCommandOutputMessage(CommandOutputMessage message)
		{
			Write(message.MessageId);
			Write(message.IsInternal);

			string[] parameters = message.Parameters ?? Array.Empty<string>();
			WriteUnsignedVarInt((uint) parameters.Length);

			foreach (string parameter in parameters)
			{
				Write(parameter);
			}

			Write(message.RawMessage != null);
			if (message.RawMessage != null)
			{
				Write(message.RawMessage);
			}
		}

		private CommandOutputMessage ReadCommandOutputMessage()
		{
			CommandOutputMessage result = new CommandOutputMessage();
			result.MessageId = ReadString();
			result.IsInternal = ReadBool();

			var count = ReadUnsignedVarInt();
			result.Parameters = new string[count];

			for (int i = 0; i < result.Parameters.Length; i++)
			{
				result.Parameters[i] = ReadString();
			}

			if (ReadBool())
			{
				result.RawMessage = ReadString();
			}

			return result;
		}
	}
}