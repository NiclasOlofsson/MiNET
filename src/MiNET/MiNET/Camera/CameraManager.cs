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
using System.Linq;
using System.Numerics;
using MiNET.Entities;
using MiNET.Net;

namespace MiNET.Camera
{
	/// <summary>Whether a camera shake rocks the view or moves it up and down. Vanilla's byte values.</summary>
	public enum CameraShakeType : byte
	{
		Positional = 0,
		Rotational = 1
	}

	/// <summary>
	///     One control point on a camera path: where the camera is, where it looks, when it gets
	///     there, and how it eases in.
	/// </summary>
	public class CameraSplinePoint
	{
		public Vector3 Position { get; set; }

		/// <summary>
		///     Euler degrees as pitch, yaw, roll, positive pitch looking up. To aim at a target,
		///     with d = target - <see cref="Position" />: pitch is atan2(d.Y, length(d.X, d.Z)),
		///     yaw is atan2(d.X, d.Z) plus 180. Unwrap yaw against the previous point so
		///     consecutive values stay within 180 of each other.
		/// </summary>
		public Vector3 Rotation { get; set; }

		/// <summary>Seconds from the start of the path. The last point's time is the total duration.</summary>
		public float Time { get; set; }

		public CameraEaseType Ease { get; set; } = CameraEaseType.Linear;
	}

	/// <summary>
	///     One player's camera: the preset list this player holds, and the instructions that put
	///     them on a preset and move it. Presets are per player, so two people in the same world
	///     can have different cameras.
	///
	///     <see cref="SendPresets" /> must reach the client before any <see cref="SetCamera" />,
	///     since an instruction names a preset by its index in that registry. Changing the list
	///     means sending it again.
	///
	///     <code>
	///     var eye = new Vector3(x, y + 1.6f, z);
	///
	///     camera.Fade(0.3f, 0.4f, 0.8f, Vector3.Zero);
	///     camera.SetCamera(CameraPresets.Free,
	///         position: eye + new Vector3(-8, 6, -8), facing: eye, ignoreStartingValues: true);
	///
	///     camera.SetCamera(CameraPresets.Free,                  // same preset, eased elsewhere
	///         ease: new CameraEase {Type = CameraEaseType.InOutSine, Duration = 4f},
	///         position: eye + new Vector3(9, 3, -6), facing: eye, ignoreStartingValues: true);
	///
	///     camera.SetFieldOfView(35, 1.2f, CameraEaseType.OutCubic);
	///     camera.Shake(0.5f, 1.2f, CameraShakeType.Rotational);
	///     camera.ClearFieldOfView(1f, CameraEaseType.InOutSine);
	///     camera.FollowSpline(path);                            // free preset only
	///
	///     camera.SetCamera(CameraPresets.FirstPerson);
	///     camera.ClearCamera();
	///     </code>
	///
	///     A malformed instruction kills the client without a packet violation or a disconnect
	///     reason, so the signal that one survived is whether McpePlayerAuthInput keeps arriving.
	/// </summary>
	public class CameraManager
	{
		public Player Player { get; set; }

		/// <summary>
		///     The presets this player has, in the order the client will index them. Starts as the six
		///     vanilla built-ins, which carry almost no values because the client already implements
		///     them: naming <see cref="CameraPresets.FirstPerson" /> is the whole instruction.
		/// </summary>
		public List<CameraPreset> Presets { get; } = new List<CameraPreset>(CameraPresets.Vanilla);

		/// <summary>The preset the player was last put on, or null if the camera is theirs again.</summary>
		public CameraPreset CurrentPreset { get; private set; }

		public CameraManager(Player player)
		{
			Player = player;
		}

		public CameraPreset GetPreset(string name)
		{
			return Presets.FirstOrDefault(p => string.Equals(p.name, name, StringComparison.OrdinalIgnoreCase));
		}

		/// <summary>
		///     Adds a preset, or replaces the one of the same name. Does not send anything: the client
		///     only learns about it on the next <see cref="SendPresets" />, and indices shift when a
		///     preset is replaced, so send before instructing.
		/// </summary>
		public virtual CameraPreset AddPreset(CameraPreset preset)
		{
			if (preset?.name == null) throw new ArgumentException("A camera preset needs a name", nameof(preset));

			int existing = Presets.FindIndex(p => string.Equals(p.name, preset.name, StringComparison.OrdinalIgnoreCase));
			if (existing >= 0)
			{
				Presets[existing] = preset;
			}
			else
			{
				Presets.Add(preset);
			}

			return preset;
		}

		public virtual void SendPresets()
		{
			McpeCameraPresets packet = McpeCameraPresets.CreateObject();
			packet.presets = new List<CameraPreset>(Presets);
			Player.SendPacket(packet);
		}

		/// <summary>
		///     Puts the player on a preset. The optional arguments override what the preset itself
		///     declares, which is how one preset serves many shots.
		/// </summary>
		/// <param name="name">A preset name, e.g. <see cref="CameraPresets.Free" />.</param>
		/// <param name="ease">How to move there. Null cuts instantly.</param>
		/// <param name="position">Absolute position. Only the free camera is placed absolutely.</param>
		/// <param name="rotation">Pitch and yaw, in degrees.</param>
		/// <param name="facing">A point to look at, instead of a rotation.</param>
		/// <param name="ignoreStartingValues">Drops the preset's own position and rotation, leaving only what is passed here.</param>
		public virtual void SetCamera(
			string name,
			CameraEase ease = null,
			Vector3? position = null,
			Vector2? rotation = null,
			Vector3? facing = null,
			bool ignoreStartingValues = false)
		{
			int index = Presets.FindIndex(p => string.Equals(p.name, name, StringComparison.OrdinalIgnoreCase));
			if (index < 0) throw new ArgumentException($"No camera preset named {name} for this player", nameof(name));

			McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
			packet.Set = new CameraSetInstruction
			{
				RuntimeId = index,
				Ease = ease,
				Position = position,
				Rotation = rotation,
				Facing = facing,
				RemoveIgnoreStartingValues = ignoreStartingValues
			};
			Player.SendPacket(packet);

			CurrentPreset = Presets[index];
		}

		/// <summary>Gives the camera back to the player.</summary>
		public virtual void ClearCamera()
		{
			McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
			packet.Clear = true;
			Player.SendPacket(packet);

			CurrentPreset = null;
		}

		/// <summary>
		///     Fades the screen to a colour and back. Runs on its own, independent of which camera the
		///     player is on, so it works as a cut between shots.
		/// </summary>
		public virtual void Fade(float fadeIn, float hold, float fadeOut, Vector3 color)
		{
			McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
			packet.Fade = new CameraFadeInstruction
			{
				Time = new CameraFadeTime {FadeIn = fadeIn, Hold = hold, FadeOut = fadeOut},
				ColorRgb = color
			};
			Player.SendPacket(packet);
		}

		public virtual void Fade(float fadeIn, float hold, float fadeOut, byte red, byte green, byte blue)
		{
			Fade(fadeIn, hold, fadeOut, new Vector3(red / 255f, green / 255f, blue / 255f));
		}

		/// <summary>Keeps the camera pointed at an entity as it moves.</summary>
		public virtual void TargetEntity(Entity entity, Vector3? offset = null)
		{
			McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
			packet.Target = new CameraTargetInstruction {EntityUniqueId = entity.EntityId, Offset = offset};
			Player.SendPacket(packet);
		}

		public virtual void RemoveTarget()
		{
			McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
			packet.RemoveTarget = true;
			Player.SendPacket(packet);
		}

		/// <summary>Pins the camera to an entity, so it rides along rather than merely looking at it.</summary>
		public virtual void AttachToEntity(Entity entity)
		{
			McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
			packet.AttachToEntity = entity.EntityId;
			Player.SendPacket(packet);
		}

		public virtual void DetachFromEntity()
		{
			McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
			packet.DetachFromEntity = true;
			Player.SendPacket(packet);
		}

		public virtual void SetFieldOfView(float fieldOfView, float easeTime = 0, CameraEaseType easeType = CameraEaseType.Linear)
		{
			McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
			packet.Fov = new CameraFovInstruction
			{
				FieldOfView = fieldOfView,
				EaseTime = easeTime,
				EaseType = easeType,
				Clear = false
			};
			Player.SendPacket(packet);
		}

		public virtual void ClearFieldOfView(float easeTime = 0, CameraEaseType easeType = CameraEaseType.Linear)
		{
			McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
			packet.Fov = new CameraFovInstruction
			{
				FieldOfView = 0,
				EaseTime = easeTime,
				EaseType = easeType,
				Clear = true
			};
			Player.SendPacket(packet);
		}

		/// <summary>
		///     Flies the camera along a path of control points, the same model the Editor's camera
		///     tool uses: each point carries a position, a rotation, a timestamp and an easing.
		///
		///     The three wire lists are parallel, one entry per control point, and none of them may
		///     be empty. Total time comes from the last point's <see cref="CameraSplinePoint.Time" />.
		///     The player must already be on <see cref="CameraPresets.Free" />: splines are only
		///     valid on the free preset.
		/// </summary>
		public virtual void FollowSpline(IReadOnlyList<CameraSplinePoint> points, CameraSplineType type = CameraSplineType.CatmullRom)
		{
			if (points == null || points.Count == 0) throw new ArgumentException("A spline needs control points", nameof(points));

			// Vanilla's own minimums. Fewer points than this and the curve cannot be built.
			int minimum = type == CameraSplineType.CatmullRom ? 4 : 3;
			if (points.Count < minimum) throw new ArgumentException($"A {type} spline needs at least {minimum} control points, got {points.Count}", nameof(points));

			float totalTime = points[points.Count - 1].Time;

			var spline = new CameraSplineInstruction {TotalTime = totalTime, Type = type};

			foreach (CameraSplinePoint point in points)
			{
				float progress = totalTime <= 0 ? 0 : point.Time / totalTime;

				spline.Curve.Add(point.Position);
				spline.ProgressKeyFrames.Add(new CameraProgressKeyFrame {Value = progress, Time = point.Time, Ease = point.Ease});
				spline.RotationOptions.Add(new CameraRotationKeyFrame {Value = point.Rotation, Time = point.Time, Ease = point.Ease});
			}

			McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
			packet.Spline = spline;
			Player.SendPacket(packet);
		}

		/// <summary>Flies a spline the client already holds, by name, from the spline registry.</summary>
		public virtual void FollowSpline(float totalTime, string splineIdentifier)
		{
			McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
			packet.Spline = new CameraSplineInstruction
			{
				TotalTime = totalTime,
				SplineIdentifier = splineIdentifier,
				LoadFromJson = true
			};
			Player.SendPacket(packet);
		}

		/// <summary>
		///     Shakes the view. Not part of the preset system: it applies to whatever camera the player
		///     is on, including their own.
		/// </summary>
		public virtual void Shake(float intensity, float duration, CameraShakeType type = CameraShakeType.Positional)
		{
			McpeCameraShake packet = McpeCameraShake.CreateObject();
			packet.intensity = intensity;
			packet.duration = duration;
			packet.type = (byte) type;
			packet.action = 0; // add
			Player.SendPacket(packet);
		}

		public virtual void StopShake()
		{
			McpeCameraShake packet = McpeCameraShake.CreateObject();
			packet.intensity = 0;
			packet.duration = 0;
			packet.type = (byte) CameraShakeType.Positional;
			packet.action = 1; // stop
			Player.SendPacket(packet);
		}
	}
}
