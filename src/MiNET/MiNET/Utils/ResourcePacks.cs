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

using System.Collections.Generic;

namespace MiNET.Utils
{
	public class ResourcePackInfos : List<ResourcePackInfo>
	{
	}

	public class ResourcePackInfo
	{
		/// <summary>
		///		The unique identifier for the pack
		/// </summary>
		public string UUID    { get; set; }
		
		/// <summary>
		/// Version is the version of the pack. The client will cache packs sent by the server as
		/// long as they carry the same version. Sending a pack with a different version than previously
		/// </summary>
		public string Version { get; set; }
		
		/// <summary>
		/// Size is the total size in bytes that the texture pack occupies. This is the size of the compressed
		/// archive (zip) of the texture pack.
		/// </summary>
		public ulong  Size    { get; set; }
		
		/// <summary>
		/// ContentKey is the key used to decrypt the behaviour pack if it is encrypted. This is generally the case
		/// for marketplace texture packs.
		/// </summary>
		public string ContentKey      { get; set; }
		
		public string SubPackName { get; set; }
		
		/// <summary>
		/// Size is the total size in bytes that the texture pack occupies. This is the size of the compressed archive (zip) of the texture pack.
		/// </summary>
		public string ContentIdentity { get; set; }
		
		/// <summary>
		///		HasScripts specifies if the texture packs has any scripts in it. A client will only download the behaviour pack if it supports scripts, which, up to 1.11, only includes Windows 10.
		/// </summary>
		public bool   HasScripts      { get; set; }
	}

	public class TexturePackInfos : List<TexturePackInfo>
	{
		
	}
	
	public class TexturePackInfo : ResourcePackInfo
	{
		/// <summary>
		/// RTXEnabled specifies if the texture pack uses the raytracing technology introduced in 1.16.200.
		/// </summary>
		public bool   RtxEnabled      { get; set; }
	}

	public class ResourcePackIdVersions : List<PackIdVersion>
	{
	}

	public class PackIdVersion
	{
		public string Id { get; set; }
		public string Version { get; set; }
		public string SubPackName { get; set; }
	}

	public class ResourcePackIds : List<string>
	{
	}
	
	public enum ResourcePackType : byte
	{
		Addon = 1,
		Cached = 2,
		CopyProtected = 3,
		Behaviour = 4,
		PersonaPiece = 5,
		Resources = 6,
		Skins = 7,
		WorldTemplate = 8
	}
}