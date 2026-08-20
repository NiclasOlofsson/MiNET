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
using System.Numerics;
using System.Threading.Tasks;
using log4net;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Camera
{
	/// <summary>
	///     A throwaway bisect harness for the camera packets against a real client. The CameraDemo
	///     config property names ONE step to run, so a disconnect names the packet that caused it.
	///     Steps, in increasing payload:
	///
	///     empty   McpeCameraInstruction with all nine optionals absent: tests the id and framing only
	///     clear   the clear verb alone, one optional bool
	///     fade    a fade, exercising the nested time and colour options
	///     set     a set with nothing but the preset index
	///     setpos  a set with position and facing
	///     setease as setpos, plus the byte-encoded ease
	///     shake   McpeCameraShake, a different packet entirely
	///     all     the whole sequence, once the rest pass
	///
	///     Every step hex-dumps its encoded bytes to the log first, so the wire form can be read
	///     without a client. Delete this file and its call in Player.InitializePlayer when done.
	/// </summary>
	public static class CameraDemo
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(CameraDemo));

		private static readonly Vector3 Black = Vector3.Zero;

		public static void Run(Player player)
		{
			string step = Config.GetProperty("CameraDemo", "off").ToLowerInvariant();
			if (step == "off" || step == "false") return;

			Task.Run(async () =>
			{
				try
				{
					await Play(player, step);
				}
				catch (Exception e)
				{
					Log.Error("Camera demo", e);
				}
			});
		}

		private static async Task Play(Player player, string step)
		{
			// Let the world settle: chunks are still streaming right after join.
			await Task.Delay(3000);
			if (!player.IsConnected) return;

			CameraManager camera = player.CameraManager;
			var eye = new Vector3((float) player.KnownPosition.X, (float) player.KnownPosition.Y + 1.6f, (float) player.KnownPosition.Z);

			// No chat here. The camera packet must be the only thing we send, or a failure cannot
			// be attributed to it.
			Log.Warn($"CAMERA DEMO step '{step}' for {player.Username}");

			switch (step)
			{
				case "empty":
					Send(player, McpeCameraInstruction.CreateObject());
					break;

				case "clear":
				{
					McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
					packet.Clear = true;
					Send(player, packet);
					break;
				}

				case "fade":
				{
					McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
					packet.Fade = new CameraFadeInstruction
					{
						Time = new CameraFadeTime {FadeIn = 0.5f, Hold = 1f, FadeOut = 1f},
						ColorRgb = Black
					};
					Send(player, packet);
					break;
				}

				case "set":
				{
					McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
					packet.Set = new CameraSetInstruction {RuntimeId = IndexOfFree(camera)};
					Send(player, packet);
					break;
				}

				case "setpos":
				{
					McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
					packet.Set = new CameraSetInstruction
					{
						RuntimeId = IndexOfFree(camera),
						Position = eye + new Vector3(-8, 6, -8),
						Facing = eye
					};
					Send(player, packet);
					break;
				}

				case "setease":
				{
					McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
					packet.Set = new CameraSetInstruction
					{
						RuntimeId = IndexOfFree(camera),
						Ease = new CameraEase {Type = CameraEaseType.InOutSine, Duration = 3f},
						Position = eye + new Vector3(9, 3, -6),
						Facing = eye
					};
					Send(player, packet);
					break;
				}

				case "fovset":
				{
					McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
					packet.Fov = new CameraFovInstruction {FieldOfView = 35, EaseTime = 1f, EaseType = CameraEaseType.OutCubic, Clear = false};
					Send(player, packet);
					break;
				}

				case "fovclear":
				{
					McpeCameraInstruction packet = McpeCameraInstruction.CreateObject();
					packet.Fov = new CameraFovInstruction {FieldOfView = 0, EaseTime = 1f, EaseType = CameraEaseType.InOutSine, Clear = true};
					Send(player, packet);
					break;
				}

				case "spline":
				{
					// Splines are documented as only valid on the free preset, so get there first.
					camera.SetCamera(CameraPresets.Free, position: eye + new Vector3(-10, 5, 0), facing: eye, ignoreStartingValues: true);
					await Task.Delay(1000);
					if (!player.IsConnected) return;

					camera.FollowSpline(CircleLookingIn(eye, 10f, 5f, 12, 6f));
					break;
				}

				case "shake":
				{
					McpeCameraShake packet = McpeCameraShake.CreateObject();
					packet.intensity = 0.5f;
					packet.duration = 1.5f;
					packet.type = 1;
					packet.action = 0;
					Send(player, packet);
					break;
				}

				case "bisect":
					await Bisect(player, camera, eye);
					break;

				case "all":
					await All(player, camera, eye);
					break;

				default:
					Log.Warn($"Unknown CameraDemo step '{step}'");
					break;
			}
		}

		/// <summary>
		///     Walks the ladder in one join, three seconds apart, logging before each send. Whichever
		///     step is last in the log is the one the client refused.
		/// </summary>
		private static async Task Bisect(Player player, CameraManager camera, Vector3 eye)
		{
			int free = IndexOfFree(camera);

			var steps = new List<(string Name, Func<Packet> Build)>
			{
				("clear", () =>
				{
					McpeCameraInstruction p = McpeCameraInstruction.CreateObject();
					p.Clear = true;
					return p;
				}),
				("shake", () =>
				{
					McpeCameraShake p = McpeCameraShake.CreateObject();
					p.intensity = 0.5f;
					p.duration = 1.5f;
					p.type = 1;
					p.action = 0;
					return p;
				}),
				("fade", () =>
				{
					McpeCameraInstruction p = McpeCameraInstruction.CreateObject();
					p.Fade = new CameraFadeInstruction {Time = new CameraFadeTime {FadeIn = 0.4f, Hold = 0.3f, FadeOut = 0.6f}, ColorRgb = Black};
					return p;
				}),
				("set", () =>
				{
					McpeCameraInstruction p = McpeCameraInstruction.CreateObject();
					p.Set = new CameraSetInstruction {RuntimeId = free};
					return p;
				}),
				("setpos", () =>
				{
					McpeCameraInstruction p = McpeCameraInstruction.CreateObject();
					p.Set = new CameraSetInstruction {RuntimeId = free, Position = eye + new Vector3(-8, 6, -8), Facing = eye};
					return p;
				}),
				("setease", () =>
				{
					McpeCameraInstruction p = McpeCameraInstruction.CreateObject();
					p.Set = new CameraSetInstruction
					{
						RuntimeId = free,
						Ease = new CameraEase {Type = CameraEaseType.InOutSine, Duration = 3f},
						Position = eye + new Vector3(9, 3, -6),
						Facing = eye
					};
					return p;
				}),
				("fov", () =>
				{
					McpeCameraInstruction p = McpeCameraInstruction.CreateObject();
					p.Fov = new CameraFovInstruction {FieldOfView = 35, EaseTime = 1f, EaseType = CameraEaseType.OutCubic, Clear = false};
					return p;
				}),
				("spline", () =>
				{
					McpeCameraInstruction p = McpeCameraInstruction.CreateObject();
					p.Spline = new CameraSplineInstruction {TotalTime = 6f, Curve = new List<Vector3>(Circle(eye, 10f, 5f, 24))};
					return p;
				}),
				("clear-again", () =>
				{
					McpeCameraInstruction p = McpeCameraInstruction.CreateObject();
					p.Clear = true;
					return p;
				})
			};

			foreach ((string name, Func<Packet> build) in steps)
			{
				if (!player.IsConnected)
				{
					Log.Warn($"CAMERA BISECT stopped: client gone before '{name}'");
					return;
				}

				Log.Warn($"CAMERA BISECT sending '{name}'");

				Packet packet = build();
				try
				{
					byte[] bytes = packet.Encode();
					Log.Warn($"CAMERA BISECT '{name}' {packet.GetType().Name} ({bytes.Length} bytes): {Convert.ToHexString(bytes)}");
				}
				catch (Exception e)
				{
					Log.Error($"CAMERA BISECT '{name}' encode", e);
				}

				player.SendPacket(packet);
				await Task.Delay(3000);
			}

			Log.Warn("CAMERA BISECT survived every step");
		}

		private static async Task All(Player player, CameraManager camera, Vector3 eye)
		{
			camera.Fade(0.3f, 0.4f, 0.8f, Black);
			camera.SetCamera(CameraPresets.Free, position: eye + new Vector3(-8, 6, -8), facing: eye, ignoreStartingValues: true);

			await Task.Delay(2500);
			if (!player.IsConnected) return;

			camera.SetCamera(
				CameraPresets.Free,
				ease: new CameraEase {Type = CameraEaseType.InOutSine, Duration = 4f},
				position: eye + new Vector3(9, 3, -6),
				facing: eye,
				ignoreStartingValues: true);

			await Task.Delay(4500);
			if (!player.IsConnected) return;

			camera.SetFieldOfView(35, 1.2f, CameraEaseType.OutCubic);
			await Task.Delay(2000);
			if (!player.IsConnected) return;

			camera.Shake(0.5f, 1.2f, CameraShakeType.Rotational);
			await Task.Delay(1500);
			if (!player.IsConnected) return;

			camera.ClearFieldOfView(1f, CameraEaseType.InOutSine);
			camera.FollowSpline(CircleLookingIn(eye, 10f, 5f, 12, 6f));

			await Task.Delay(6500);
			if (!player.IsConnected) return;

			camera.Fade(0.4f, 0.2f, 0.7f, Black);
			await Task.Delay(500);
			if (!player.IsConnected) return;

			// Back onto a normal perspective before handing control back.
			camera.SetCamera(CameraPresets.FirstPerson);
			await Task.Delay(500);
			if (!player.IsConnected) return;

			camera.ClearCamera();
			Log.Warn("CAMERA DEMO done");
		}

		/// <summary>
		///     Dumps the encoded bytes, then sends. The dump uses a throwaway encode so the packet
		///     that goes out is untouched.
		/// </summary>
		private static void Send<T>(Player player, T packet) where T : Packet<T>, new()
		{
			try
			{
				byte[] bytes = packet.Encode();
				Log.Warn($"CAMERA DEMO {packet.GetType().Name} ({bytes.Length} bytes): {Convert.ToHexString(bytes)}");
			}
			catch (Exception e)
			{
				Log.Error("Camera demo encode", e);
			}

			player.SendPacket(packet);
		}

		internal static IEnumerable<Vector3> Circle(Vector3 center, float radius, float height, int points)
		{
			for (int i = 0; i <= points; i++)
			{
				double angle = 2 * Math.PI * i / points;
				yield return center + new Vector3((float) (Math.Cos(angle) * radius), height, (float) (Math.Sin(angle) * radius));
			}
		}

		/// <summary>
		///     A ring of control points around a centre, each one turned to look down at it.
		///
		///     Rotation is euler degrees as (pitch, yaw, roll). Positive pitch looks up, which is
		///     the opposite of the usual Minecraft convention. Yaw is unwrapped as it goes: atan2
		///     returns -180 to 180, so a lap crosses a discontinuity the client would interpolate
		///     through as a full extra spin.
		/// </summary>
		private static List<CameraSplinePoint> CircleLookingIn(Vector3 center, float radius, float height, int points, float totalTime)
		{
			var path = new List<CameraSplinePoint>();
			float previousYaw = 0;

			for (int i = 0; i <= points; i++)
			{
				double angle = 2 * Math.PI * i / points;
				var position = center + new Vector3((float) (Math.Cos(angle) * radius), height, (float) (Math.Sin(angle) * radius));

				Vector3 toCenter = center - position;
				var yaw = (float) (Math.Atan2(toCenter.X, toCenter.Z) * 180 / Math.PI) + 180f;
				var pitch = (float) (Math.Atan2(toCenter.Y, new Vector2(toCenter.X, toCenter.Z).Length()) * 180 / Math.PI);

				if (i > 0)
				{
					while (yaw - previousYaw > 180) yaw -= 360;
					while (yaw - previousYaw < -180) yaw += 360;
				}

				previousYaw = yaw;

				path.Add(new CameraSplinePoint
				{
					Position = position,
					Rotation = new Vector3(pitch, yaw, 0),
					Time = (float) i / points * totalTime
				});
			}

			return path;
		}

		private static int IndexOfFree(CameraManager camera)
		{
			for (int i = 0; i < camera.Presets.Count; i++)
			{
				if (camera.Presets[i].name == CameraPresets.Free) return i;
			}

			return 0;
		}
	}
}
