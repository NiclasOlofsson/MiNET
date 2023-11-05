﻿#region LICENSE

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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using log4net;
using MiNET.Net.RakNet;

namespace MiNET
{
	public class GreyListManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(GreyListManager));

		private HashSet<IPAddress> _blacklist = new HashSet<IPAddress>();
		private ConcurrentDictionary<IPAddress, DateTime> _greylist = new ConcurrentDictionary<IPAddress, DateTime>();
		public ConnectionInfo ConnectionInfo { get; set; }

		public GreyListManager(ConnectionInfo connectionInfo = null)
		{
			ConnectionInfo = connectionInfo;
		}

		public virtual bool IsWhitelisted(IPEndPoint endPoint)
		{
			return true;
		}

		public virtual bool IsBlacklisted(IPEndPoint endPoint)
		{
			lock (_blacklist)
			{
				return _blacklist.Contains(endPoint.Address);
			}
		}

		public virtual void Blacklist(IPAddress address)
		{
			lock (_blacklist)
			{
				_blacklist.Add(address);
			}
		}

		public virtual void Blacklist(IPEndPoint endPoint)
		{
			Blacklist(endPoint.Address);
		}

		public virtual bool AcceptConnection(IPEndPoint endPoint)
		{
			if (IsWhitelisted(endPoint)) return true;

			ConnectionInfo connectionInfo = ConnectionInfo;
			if (connectionInfo == null) return true;

			if (connectionInfo.NumberOfPlayers >= connectionInfo.MaxNumberOfPlayers || connectionInfo.ConnectionsInConnectPhase >= connectionInfo.MaxNumberOfConcurrentConnects)
			{
				if (Log.IsInfoEnabled)
					Log.InfoFormat("Rejected connection (server full) from: {0}", endPoint);

				return false;
			}

			return true;
		}

		public virtual bool IsGreylisted(IPEndPoint endPoint)
		{
			var address = endPoint.Address;

			if (_greylist.ContainsKey(address))
			{
				if (_greylist[address] > DateTime.UtcNow)
				{
					return true;
				}

				DateTime waste;
				_greylist.TryRemove(address, out waste);
			}
			return false;
		}

		public virtual void Greylist(IPAddress address, int time)
		{
			var dateTime = DateTime.UtcNow.AddMilliseconds(time);
			_greylist.TryAdd(address, dateTime);
		}

		public virtual void Greylist(IPEndPoint endPoint, int time)
		{
			Greylist(endPoint.Address, time);
		}
	}
}