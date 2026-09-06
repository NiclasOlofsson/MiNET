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
using System.Numerics;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MiNET.AgentClient
{
	/// <summary>
	///     A voxel raycaster with a cartoon look: one flat colour per block, three fixed tones by
	///     face axis, a dark outline on every block edge, no lighting. Each pixel is one ray
	///     stepped through the grid (Amanatides and Woo) until it meets a block with alpha, and
	///     translucent blocks blend once and let the ray continue.
	/// </summary>
	public class CartoonRenderer
	{
		private readonly ClientWorld _world;
		private readonly BlockColors _colors;

		public int MaxDistance { get; set; } = 160;

		public CartoonRenderer(ClientWorld world, BlockColors colors)
		{
			_world = world;
			_colors = colors;
		}

		/// <summary>Perspective view from eye along yaw and pitch, Bedrock convention: yaw 0 looks toward +Z, pitch positive looks down.</summary>
		public Image<Rgba32> RenderPerspective(Vector3 eye, float yawDegrees, float pitchDegrees, int width, int height, float fovDegrees)
		{
			float yaw = yawDegrees * MathF.PI / 180f;
			float pitch = pitchDegrees * MathF.PI / 180f;
			var forward = Vector3.Normalize(new Vector3(-MathF.Sin(yaw) * MathF.Cos(pitch), -MathF.Sin(pitch), MathF.Cos(yaw) * MathF.Cos(pitch)));
			Vector3 right = Vector3.Normalize(Vector3.Cross(forward, Vector3.UnitY));
			if (float.IsNaN(right.X)) right = Vector3.UnitX; // straight up or down
			Vector3 up = Vector3.Cross(right, forward);

			float tan = MathF.Tan(fovDegrees * MathF.PI / 360f);
			float aspect = (float) width / height;
			float pixelAngle = 2f * tan / height;

			var image = new Image<Rgba32>(width, height);
			_ = _colors.Table;
			Parallel.For(0, height, py =>
			{
				var ray = new RayState(_world);
				float v = (1f - 2f * (py + 0.5f) / height) * tan;
				for (int px = 0; px < width; px++)
				{
					float u = (2f * (px + 0.5f) / width - 1f) * tan * aspect;
					Vector3 direction = Vector3.Normalize(forward + right * u + up * v);
					image[px, py] = Trace(ref ray, eye, direction, MaxDistance, pixelAngle, 0f);
				}
			});
			return image;
		}

		/// <summary>
		///     One ray, narrated: every cell it visits with the section it read from, until the
		///     first block with alpha. What a pixel of a perspective render saw, for reading a
		///     picture that disagrees with a probe.
		/// </summary>
		public string TraceDebug(Vector3 origin, float yawDegrees, float pitchDegrees, float maxDistance)
		{
			float yaw = yawDegrees * MathF.PI / 180f;
			float pitch = pitchDegrees * MathF.PI / 180f;
			var direction = Vector3.Normalize(new Vector3(-MathF.Sin(yaw) * MathF.Cos(pitch), -MathF.Sin(pitch), MathF.Cos(yaw) * MathF.Cos(pitch)));
			var lines = new System.Text.StringBuilder();
			var ray = new RayState(_world);

			int x = (int) MathF.Floor(origin.X), y = (int) MathF.Floor(origin.Y), z = (int) MathF.Floor(origin.Z);
			int stepX = Math.Sign(direction.X), stepY = Math.Sign(direction.Y), stepZ = Math.Sign(direction.Z);
			float deltaX = stepX == 0 ? float.MaxValue : MathF.Abs(1f / direction.X);
			float deltaY = stepY == 0 ? float.MaxValue : MathF.Abs(1f / direction.Y);
			float deltaZ = stepZ == 0 ? float.MaxValue : MathF.Abs(1f / direction.Z);
			float maxX = stepX == 0 ? float.MaxValue : ((stepX > 0 ? x + 1 - origin.X : origin.X - x) * deltaX);
			float maxY = stepY == 0 ? float.MaxValue : ((stepY > 0 ? y + 1 - origin.Y : origin.Y - y) * deltaY);
			float maxZ = stepZ == 0 ? float.MaxValue : ((stepZ > 0 ? z + 1 - origin.Z : origin.Z - z) * deltaZ);
			float distance = 0;
			while (distance < maxDistance)
			{
				int runtimeId = ray.Get(x, y, z);
				string name = runtimeId < 0 ? "(no section)" : Blocks.BlockFactory.GetBlockName(runtimeId) ?? runtimeId.ToString();
				int direct = _world.GetRuntimeId(x, y, z);
				string check = direct == runtimeId ? "" : $" DIRECT {direct}";
				lines.Append(string.Create(System.Globalization.CultureInfo.InvariantCulture, $"{x},{y},{z} chunk {x >> 4},{y >> 4},{z >> 4} d {distance:0.0} {name}{check}\n"));
				if (runtimeId >= 0 && _colors.Get(runtimeId).A == 255) break;

				if (maxX < maxY && maxX < maxZ) { x += stepX; distance = maxX; maxX += deltaX; }
				else if (maxY < maxZ) { y += stepY; distance = maxY; maxY += deltaY; }
				else { z += stepZ; distance = maxZ; maxZ += deltaZ; }
			}
			return lines.ToString();
		}

		/// <summary>Plan view straight down over a square of the world, one pixel per scale-th of a block.</summary>
		public Image<Rgba32> RenderTop(int centerX, int centerZ, int halfSize, int pixelsPerBlock)
		{
			return RenderOrtho(new Vector3(centerX, 319.5f, centerZ), -Vector3.UnitY, Vector3.UnitX, Vector3.UnitZ, halfSize, pixelsPerBlock, 400, centerX - halfSize, centerZ - halfSize);
		}

		/// <summary>
		///     One layer seen from above: the plan view with rays that stop after a block, so only
		///     the blocks at that height are drawn, over sky. What a build's floor plan at a given
		///     height looks like, for placing blocks by coordinate.
		/// </summary>
		public Image<Rgba32> RenderSlice(int centerX, int y, int centerZ, int halfSize, int pixelsPerBlock)
		{
			// Start in the cell above the layer: the cell a ray starts in is never drawn, and the
			// reach ends as the ray enters the cell below the layer.
			return RenderOrtho(new Vector3(centerX, y + 1.5f, centerZ), -Vector3.UnitY, Vector3.UnitX, Vector3.UnitZ, halfSize, pixelsPerBlock, 1.5f, centerX - halfSize, centerZ - halfSize);
		}

		/// <summary>
		///     Elevation: a side view with parallel rays, looking along the given horizontal
		///     direction at a square of the world around the centre. Heights count exactly on it.
		/// </summary>
		public Image<Rgba32> RenderElevation(Vector3 center, Vector3 viewDirection, int halfSize, int pixelsPerBlock)
		{
			Vector3 right = Vector3.Normalize(Vector3.Cross(viewDirection, Vector3.UnitY));
			Vector3 origin = center - viewDirection * 200;
			// Image x runs along "right", image y runs downward in world y.
			return RenderOrtho(origin, viewDirection, right, -Vector3.UnitY, halfSize, pixelsPerBlock, 400, 0, 0, tickOrigin: center);
		}

		private Image<Rgba32> RenderOrtho(Vector3 origin, Vector3 direction, Vector3 imageX, Vector3 imageY, int halfSize, int pixelsPerBlock, float maxDistance, int tickX, int tickY, Vector3? tickOrigin = null)
		{
			int size = halfSize * 2 * pixelsPerBlock;
			var image = new Image<Rgba32>(size, size);
			_ = _colors.Table;
			float pixelSize = 1f / pixelsPerBlock;

			Parallel.For(0, size, py =>
			{
				var ray = new RayState(_world);
				for (int px = 0; px < size; px++)
				{
					float u = -halfSize + (px + 0.5f) / pixelsPerBlock;
					float v = -halfSize + (py + 0.5f) / pixelsPerBlock;
					Vector3 eye = origin + imageX * u + imageY * v;
					Rgba32 pixel = Trace(ref ray, eye, direction, maxDistance, 0f, pixelSize);

					// Tick lines every 8 blocks of world coordinate along both image axes, for
					// counting and for reading coordinates off the picture.
					float wx = Vector3.Dot(eye, imageX), wy = Vector3.Dot(eye, imageY);
					bool tick = IsTick(wx, pixelSize) || IsTick(wy, pixelSize);
					if (tick) pixel = new Rgba32((byte) (pixel.R * 0.6f), (byte) (pixel.G * 0.6f), (byte) (pixel.B * 0.6f), 255);
					image[px, py] = pixel;
				}
			});
			return image;
		}

		private static bool IsTick(float worldCoordinate, float pixelSize)
		{
			float f = worldCoordinate - MathF.Floor(worldCoordinate / 8f) * 8f;
			return f < pixelSize;
		}

		private struct RayState
		{
			private readonly ClientWorld _world;
			private int _chunkX, _sectionY, _chunkZ;
			private int[] _grid;
			private bool _valid;

			public RayState(ClientWorld world)
			{
				_world = world;
				_chunkX = _sectionY = _chunkZ = int.MinValue;
				_grid = null;
				_valid = false;
			}

			public int Get(int x, int y, int z)
			{
				int cx = x >> 4, sy = y >> 4, cz = z >> 4;
				if (!_valid || cx != _chunkX || sy != _sectionY || cz != _chunkZ)
				{
					_chunkX = cx;
					_sectionY = sy;
					_chunkZ = cz;
					_grid = _world.GetSection(cx, sy, cz);
					_valid = true;
				}
				if (_grid == null) return -1;
				return _grid[((x & 15) << 8) | ((z & 15) << 4) | (y & 15)];
			}
		}

		/// <param name="pixelAngle">Perspective: the angle one pixel spans, so the outline width scales with distance.</param>
		/// <param name="pixelSize">Orthographic: world units per pixel, a constant outline width.</param>
		private Rgba32 Trace(ref RayState ray, Vector3 origin, Vector3 direction, float maxDistance, float pixelAngle, float pixelSize)
		{
			int x = (int) MathF.Floor(origin.X), y = (int) MathF.Floor(origin.Y), z = (int) MathF.Floor(origin.Z);
			int stepX = Math.Sign(direction.X), stepY = Math.Sign(direction.Y), stepZ = Math.Sign(direction.Z);

			float deltaX = stepX == 0 ? float.MaxValue : MathF.Abs(1f / direction.X);
			float deltaY = stepY == 0 ? float.MaxValue : MathF.Abs(1f / direction.Y);
			float deltaZ = stepZ == 0 ? float.MaxValue : MathF.Abs(1f / direction.Z);

			float maxX = stepX == 0 ? float.MaxValue : ((stepX > 0 ? x + 1 - origin.X : origin.X - x) * deltaX);
			float maxY = stepY == 0 ? float.MaxValue : ((stepY > 0 ? y + 1 - origin.Y : origin.Y - y) * deltaY);
			float maxZ = stepZ == 0 ? float.MaxValue : ((stepZ > 0 ? z + 1 - origin.Z : origin.Z - z) * deltaZ);

			// Accumulated translucent layers, front to back.
			float accR = 0, accG = 0, accB = 0, accA = 0;
			int layers = 0;

			float distance = 0;
			int axis = -1;
			while (distance < maxDistance)
			{
				if (y >= -64 && y < 320)
				{
					int runtimeId = ray.Get(x, y, z);
					if (runtimeId >= 0)
					{
						BlockColors.Rgba color = _colors.Get(runtimeId);
						BlockColors.BlockShape shape = _colors.Shape(runtimeId);
						if (color.A > 0 && axis >= 0 && shape != BlockColors.BlockShape.Cube)
						{
							// A stand-in box inside the cell. The ray is tested against it between
							// entering and leaving the cell; a miss passes through the cell as if it
							// were air, which is what a flower next to the camera should do.
							float exit = MathF.Min(maxX, MathF.Min(maxY, maxZ));
							if (HitBox(shape, x, y, z, origin, direction, distance, exit, out float hitDistance, out int hitAxis, out int hitSide))
							{
								float tone = hitAxis switch
								{
									1 => hitSide < 0 ? 1.0f : 0.5f,
									0 => 0.8f,
									_ => 0.65f
								};
								float remaining = 1f - accA;
								return new Rgba32((byte) (accR + color.R * tone * remaining), (byte) (accG + color.G * tone * remaining), (byte) (accB + color.B * tone * remaining), 255);
							}
						}
						else if (color.A > 0 && axis >= 0)
						{
							float width = pixelSize > 0 ? pixelSize * 1.5f : distance * pixelAngle * 0.75f;
							Shade(color, axis, stepY, origin, direction, distance, width, out float r, out float g, out float b);

							if (color.A == 255 || layers >= 4)
							{
								float remaining = 1f - accA;
								return new Rgba32((byte) (accR + r * remaining), (byte) (accG + g * remaining), (byte) (accB + b * remaining), 255);
							}

							float alpha = color.A / 255f * (1f - accA);
							accR += r * alpha;
							accG += g * alpha;
							accB += b * alpha;
							accA += alpha;
							layers++;
						}
					}
				}
				else if (y < -64 && stepY <= 0) break;
				else if (y >= 320 && stepY >= 0) break;

				if (maxX < maxY && maxX < maxZ)
				{
					x += stepX;
					distance = maxX;
					maxX += deltaX;
					axis = 0;
				}
				else if (maxY < maxZ)
				{
					y += stepY;
					distance = maxY;
					maxY += deltaY;
					axis = 1;
				}
				else
				{
					z += stepZ;
					distance = maxZ;
					maxZ += deltaZ;
					axis = 2;
				}
			}

			// Sky: pale blue up high, whiter toward the horizon.
			float t = Math.Clamp(direction.Y * 0.5f + 0.5f, 0f, 1f);
			float skyR = 200 - 70 * t, skyG = 225 - 45 * t, skyB = 255;
			float rem = 1f - accA;
			return new Rgba32((byte) (accR + skyR * rem), (byte) (accG + skyG * rem), (byte) (accB + skyB * rem), 255);
		}

		/// <summary>
		///     Ray against the stand-in box of a non-cube block, in world units, limited to the ray's
		///     stretch inside the cell. Reports the hit distance, the axis of the face hit and the
		///     sign of travel on that axis (negative means the ray came from above for axis 1).
		/// </summary>
		private static bool HitBox(BlockColors.BlockShape shape, int x, int y, int z, Vector3 origin, Vector3 direction, float enter, float exit, out float hitDistance, out int hitAxis, out int hitSide)
		{
			Vector3 min, max;
			switch (shape)
			{
				case BlockColors.BlockShape.Post:
					min = new Vector3(x + 0.35f, y, z + 0.35f);
					max = new Vector3(x + 0.65f, y + 0.6f, z + 0.65f);
					break;
				case BlockColors.BlockShape.Slab:
					min = new Vector3(x, y, z);
					max = new Vector3(x + 1, y + 0.5f, z + 1);
					break;
				case BlockColors.BlockShape.Thin:
					min = new Vector3(x + 0.375f, y, z + 0.375f);
					max = new Vector3(x + 0.625f, y + 1, z + 0.625f);
					break;
				default:
					min = new Vector3(x, y, z);
					max = new Vector3(x + 1, y + 0.125f, z + 1);
					break;
			}

			float tMin = enter, tMax = exit;
			hitAxis = -1;
			hitSide = 0;
			for (int a = 0; a < 3; a++)
			{
				float o = a == 0 ? origin.X : a == 1 ? origin.Y : origin.Z;
				float d = a == 0 ? direction.X : a == 1 ? direction.Y : direction.Z;
				float lo = a == 0 ? min.X : a == 1 ? min.Y : min.Z;
				float hi = a == 0 ? max.X : a == 1 ? max.Y : max.Z;

				if (MathF.Abs(d) < 1e-6f)
				{
					if (o < lo || o > hi) { hitDistance = 0; return false; }
					continue;
				}

				float t1 = (lo - o) / d, t2 = (hi - o) / d;
				int side = d > 0 ? -1 : 1;
				if (t1 > t2) (t1, t2) = (t2, t1);
				if (t1 > tMin)
				{
					tMin = t1;
					hitAxis = a;
					hitSide = side;
				}
				if (t2 < tMax) tMax = t2;
				if (tMin > tMax) { hitDistance = 0; return false; }
			}

			hitDistance = tMin;
			if (hitAxis < 0) hitAxis = 1; // entered the cell already inside the box: treat as its top
			return true;
		}

		private static void Shade(BlockColors.Rgba color, int axis, int stepY, Vector3 origin, Vector3 direction, float distance, float outlineWidth, out float r, out float g, out float b)
		{
			// Three tones by face axis. Bottom faces darker still, since they are only seen from below.
			float tone = axis switch
			{
				1 => stepY < 0 ? 1.0f : 0.5f,
				0 => 0.8f,
				_ => 0.65f
			};

			// Outline: the hit point's distance to the nearest edge of the face, in the two axes
			// that lie in the face. The width is about one and a half pixels at the hit, clamped so
			// the line never vanishes up close nor swallows a block far away.
			Vector3 hit = origin + direction * distance;
			float a1 = axis == 0 ? hit.Y : hit.X;
			float a2 = axis == 2 ? hit.Y : hit.Z;
			float f1 = a1 - MathF.Floor(a1);
			float f2 = a2 - MathF.Floor(a2);
			float edge = MathF.Min(MathF.Min(f1, 1f - f1), MathF.Min(f2, 1f - f2));
			if (edge < Math.Clamp(outlineWidth, 0.015f, 0.12f)) tone *= 0.35f;

			r = color.R * tone;
			g = color.G * tone;
			b = color.B * tone;
		}
	}
}