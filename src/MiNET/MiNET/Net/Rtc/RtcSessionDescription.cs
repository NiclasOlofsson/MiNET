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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace MiNET.Net.Rtc
{
	public class RtcSessionDescription
	{
		public ulong SessionId { get; set; }
		public string IceUfrag { get; set; }
		public string IcePassword { get; set; }
		public bool IceLite { get; set; }
		public string FingerprintSha256 { get; set; }
		public string Setup { get; set; } = "actpass";
		public int SctpPort { get; set; } = 5000;
		public List<IPEndPoint> Candidates { get; set; } = new List<IPEndPoint>();

		public static RtcSessionDescription Parse(string sdp)
		{
			var description = new RtcSessionDescription();
			var candidates = new List<IPEndPoint>();

			foreach (string rawLine in sdp.Split('\n'))
			{
				string line = rawLine.TrimEnd('\r');

				if (line.StartsWith("o=", StringComparison.Ordinal))
				{
					string[] parts = line.Split(' ');
					if (parts.Length > 1 && ulong.TryParse(parts[1], out ulong sessionId)) description.SessionId = sessionId;
				}
				else if (line.StartsWith("a=ice-lite", StringComparison.Ordinal))
				{
					description.IceLite = true;
				}
				else if (line.StartsWith("a=ice-ufrag:", StringComparison.Ordinal))
				{
					description.IceUfrag = line.Substring("a=ice-ufrag:".Length);
				}
				else if (line.StartsWith("a=ice-pwd:", StringComparison.Ordinal))
				{
					description.IcePassword = line.Substring("a=ice-pwd:".Length);
				}
				else if (line.StartsWith("a=fingerprint:", StringComparison.Ordinal))
				{
					string value = line.Substring("a=fingerprint:".Length);
					int space = value.IndexOf(' ');
					description.FingerprintSha256 = (space >= 0 ? value.Substring(space + 1) : value).ToUpperInvariant();
				}
				else if (line.StartsWith("a=setup:", StringComparison.Ordinal))
				{
					description.Setup = line.Substring("a=setup:".Length);
				}
				else if (line.StartsWith("a=sctp-port:", StringComparison.Ordinal))
				{
					if (int.TryParse(line.Substring("a=sctp-port:".Length), out int sctpPort)) description.SctpPort = sctpPort;
				}
				else if (line.StartsWith("a=candidate:", StringComparison.Ordinal))
				{
					string[] parts = line.Substring("a=candidate:".Length).Split(' ');
					if (parts.Length >= 6 && IPAddress.TryParse(parts[4], out IPAddress address)
							&& int.TryParse(parts[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out int port))
					{
						candidates.Add(new IPEndPoint(address, port));
					}
				}
			}

			if (string.IsNullOrEmpty(description.IceUfrag) || string.IsNullOrEmpty(description.IcePassword) || string.IsNullOrEmpty(description.FingerprintSha256))
			{
				// The rejected SDP travels in the exception: a format drift (26.50 reshaped
				// signaling) is diagnosed from what was actually received, not from which field
				// came up empty.
				throw new FormatException($"SDP is missing ice-ufrag, ice-pwd or fingerprint. Received:\n{sdp}");
			}

			description.Candidates = candidates;
			return description;
		}

		public string ToSdp()
		{
			var sb = new StringBuilder();
			sb.Append("v=0\r\n");
			sb.Append("o=- ").Append(SessionId).Append(" 2 IN IP4 127.0.0.1\r\n");
			sb.Append("s=-\r\n");
			sb.Append("t=0 0\r\n");
			sb.Append("a=group:BUNDLE 0\r\n");
			if (IceLite) sb.Append("a=ice-lite\r\n");
			sb.Append("m=application 9 UDP/DTLS/SCTP webrtc-datachannel\r\n");
			sb.Append("c=IN IP4 0.0.0.0\r\n");
			sb.Append("a=ice-ufrag:").Append(IceUfrag).Append("\r\n");
			sb.Append("a=ice-pwd:").Append(IcePassword).Append("\r\n");
			sb.Append("a=fingerprint:sha-256 ").Append(FingerprintSha256).Append("\r\n");
			sb.Append("a=setup:").Append(Setup).Append("\r\n");
			sb.Append("a=mid:0\r\n");
			sb.Append("a=sctp-port:").Append(SctpPort).Append("\r\n");
			sb.Append("a=max-message-size:262144\r\n");

			for (int i = 0; i < Candidates.Count; i++)
			{
				IPEndPoint candidate = Candidates[i];
				int foundation = i + 1;
				long priority = 2130706431L - i;
				sb.Append("a=candidate:").Append(foundation).Append(" 1 udp ").Append(priority).Append(' ')
						.Append(candidate.Address).Append(' ').Append(candidate.Port).Append(" typ host generation 0\r\n");
			}

			return sb.ToString();
		}
	}
}