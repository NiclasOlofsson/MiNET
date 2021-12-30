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
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json;

namespace MiNET.Utils.Skins
{
	public class Description : ICloneable
	{
		public string Identifier { get; set; }

		[JsonProperty(PropertyName = "texture_height")]
		public int TextureHeight { get; set; }

		[JsonProperty(PropertyName = "texture_width")]
		public int TextureWidth { get; set; }

		[JsonProperty(PropertyName = "visible_bounds_height")]
		public int VisibleBoundsHeight { get; set; }

		[JsonProperty(PropertyName = "visible_bounds_offset")]
		public int[] VisibleBoundsOffset { get; set; }

		[JsonProperty(PropertyName = "visible_bounds_width")]
		public int VisibleBoundsWidth { get; set; }

		public object Clone()
		{
			var clone = (Description) MemberwiseClone();
			clone.VisibleBoundsOffset = VisibleBoundsOffset?.Clone() as int[];
			return clone;
		}
	}

	public class Geometry : ICloneable
	{
		[JsonIgnore] public string Name { get; set; }

		public Description Description { get; set; }

		[JsonIgnore] public string BaseGeometry { get; set; }

		public List<Bone> Bones { get; set; }

		[JsonProperty(PropertyName = "META_BoneType")]
		public string BoneType { get; set; }

		[JsonProperty(PropertyName = "META_ModelVersion")]
		public string ModelVersion { get; set; }

		[JsonProperty(PropertyName = "rigtype")]
		public string RigType { get; set; }

		[JsonProperty(PropertyName = "texturewidth")]
		public int TextureWidth { get; set; }

		[JsonProperty(PropertyName = "textureheight")]
		public int TextureHeight { get; set; }

		public bool AnimationArmsDown { get; set; }
		public bool AnimationArmsOutFront { get; set; }
		public bool AnimationStatueOfLibertyArms { get; set; }
		public bool AnimationSingleArmAnimation { get; set; }
		public bool AnimationStationaryLegs { get; set; }
		public bool AnimationSingleLegAnimation { get; set; }
		public bool AnimationNoHeadBob { get; set; }
		public bool AnimationDontShowArmor { get; set; }
		public bool AnimationUpsideDown { get; set; }
		public bool AnimationInvertedCrouch { get; set; }

		public object Clone()
		{
			var geometry = (Geometry) MemberwiseClone();
			geometry.Description = (Description) Description.Clone();

			if (Bones != null)
			{
				geometry.Bones = new List<Bone>();
				foreach (var bone in Bones)
				{
					geometry.Bones.Add((Bone) bone.Clone());
				}
			}

			return geometry;
		}

		public const float Gravity = 0.08f;
		public const float Drag = 0.02f;
		public const double CubeFilterFactor = 1.3;
		public const float ZTearFactor = 0.01f;

		public static Geometry Subdivide(Geometry geometry, bool packInBody = true, bool keepHead = true, bool renderSkin = true, bool renderSkeleton = false)
		{
			Geometry cloned = (Geometry) geometry.Clone();
			cloned.Subdivide(packInBody, keepHead, renderSkin, renderSkeleton);
			return cloned;
		}

		public void Subdivide(bool packInBody = true, bool keepHead = true, bool renderSkin = true, bool renderSkeleton = false)
		{
			List<Cube> newCubes = new List<Cube>();
			var random = new Random();

			foreach (var bone in Bones)
			{
				if (bone.NeverRender) continue;
				if (bone.Cubes == null || bone.Cubes.Count == 0) continue;

				var cubes = bone.Cubes.ToArray();
				bone.Cubes.Clear();
				foreach (var cube in cubes)
				{
					int width = (int) cube.Size[0];
					int height = (int) cube.Size[1];
					int depth = (int) cube.Size[2];

					float u = cube.Uv[0];
					float v = cube.Uv[1];

					//inside
					if (renderSkeleton)
					{
						for (int w = 0; w < width; w++)
						{
							for (int d = 0; d < depth; d++)
							{
								for (int h = 0; h < height; h++)
								{
									if ((w > 0 && w < width - 1) && (d > 0 && d < depth - 1) && (h > 0 && h < height - 1))
									{
										var cubeOrigin = cube.Origin;

										Cube c = new Cube
										{
											Face = Face.Inside,
											Size = new[] {1f, 1f, 1f},
											Origin = new[] {cubeOrigin[0] + w, cubeOrigin[1] + h, cubeOrigin[2] + d},
											Uv = new float[] {20, 4},
											Velocity = Vector3.Zero
										};
										{
											bool isHead = bone.Name.Equals(BoneName.Head.ToString(), StringComparison.InvariantCultureIgnoreCase);// == BoneName.Head.ToString();
											if (packInBody)
											{
												if (keepHead && isHead)
													bone.Cubes.Add(c);
												else
													newCubes.Add(c);
											}
											else
											{
												bone.Cubes.Add(c);
											}
										}
									}
								}
							}
						}
					}

					if (renderSkin)
					{
						//front
						for (int w = 0; w < width; w++)
						{
							float uvx = u + depth - 1;
							if (bone.Mirror)
								uvx = u + depth + width - 2;
							for (int d = 0; d < 1; d++)
							{
								float uvy = v + depth + height - 2;
								for (int h = 0; h < height; h++)
								{
									if ((w > 0 && w < width - 1) && (d > 0 && d < depth - 1) && (h > 0 && h < height - 1))
									{
										uvy--;
										continue;
									}

									var cubeOrigin = cube.Origin;

									Cube c = new Cube
									{
										Face = Face.Front,
										Size = new[] {1f, 1f, 1f},
										Origin = new[] {cubeOrigin[0] + w, cubeOrigin[1] + h, cubeOrigin[2] + d - ZTearFactor},
										Uv = bone.Mirror ? new[] {uvx - w, uvy--} : new[] {uvx + w, uvy--},
										Velocity = new Vector3(0, (float) (random.NextDouble() * -0.01), 0)
									};
									bool isHead = bone.Name == BoneName.Head.ToString();
									if (isHead || random.NextDouble() < CubeFilterFactor)
									{
										if (packInBody)
										{
											if (keepHead && isHead)
											{
												c.Velocity = Vector3.Zero;
												bone.Cubes.Add(c);
											}
											else
												newCubes.Add(c);
										}
										else
										{
											bone.Cubes.Add(c);
										}
									}
								}
							}
						}

						//back
						for (int w = 0; w < width; w++)
						{
							float uvx = u + depth + width + depth - 3;
							if (!bone.Mirror)
								uvx = u + depth + width + depth + width - 4;
							for (int d = depth - 1; d < depth; d++)
							{
								float uvy = v + depth + height - 2;
								for (int h = 0; h < height; h++)
								{
									if ((w > 0 && w < width - 1) && (d > 0 && d < depth - 1) && (h > 0 && h < height - 1))
									{
										uvy--;
										continue;
									}

									var cubeOrigin = cube.Origin;

									Cube c = new Cube
									{
										Face = Face.Back,
										Size = new[] {1f, 1f, 1f},
										Origin = new[] {cubeOrigin[0] + w, cubeOrigin[1] + h, cubeOrigin[2] + d + ZTearFactor},
										Uv = !bone.Mirror ? new[] {uvx - w, uvy--} : new[] {uvx + w, uvy--},
										Velocity = new Vector3(0, (float) (random.NextDouble() * -0.01), 0)
									};
									if (random.NextDouble() < CubeFilterFactor)
									{
										bool isHead = bone.Name == BoneName.Head.ToString();
										if (packInBody)
										{
											if (keepHead && isHead)
												bone.Cubes.Add(c);
											else
												newCubes.Add(c);
										}
										else
										{
											bone.Cubes.Add(c);
										}
									}
								}
							}
						}
						// top
						for (int w = 0; w < width; w++)
						{
							float uvx = u + depth - 1;
							if (!bone.Mirror)
								uvx = u + depth + width - 2;
							float uvy = v + depth - 1;
							for (int d = 0; d < depth; d++)
							{
								for (int h = height - 1; h < height; h++)
								{
									if ((w > 0 && w < width - 1) && (d > 0 && d < depth - 1) && (h > 0 && h < height - 1))
									{
										uvy--;
										continue;
									}

									var cubeOrigin = cube.Origin;

									Cube c = new Cube
									{
										Face = Face.Top,
										Size = new[] {1f, 1f, 1f},
										Origin = new[] {cubeOrigin[0] + w, cubeOrigin[1] + h + ZTearFactor, cubeOrigin[2] + d},
										Uv = !bone.Mirror ? new[] {uvx - w, uvy--} : new[] {uvx + w, uvy--},
										Velocity = new Vector3(0, (float) (random.NextDouble() * -0.01), 0)
									};
									if (random.NextDouble() < CubeFilterFactor)
									{
										bool isHead = bone.Name == BoneName.Head.ToString();
										if (packInBody)
										{
											if (keepHead && isHead)
												bone.Cubes.Add(c);
											else
												newCubes.Add(c);
										}
										else
										{
											bone.Cubes.Add(c);
										}
									}
								}
							}
						}
						// bottom
						for (int w = 0; w < width; w++)
						{
							float uvx = u + depth + width - 2;
							float uvy = v + depth - 1;
							for (int d = 0; d < depth; d++)
							{
								for (int h = 0; h < 1; h++)
								{
									if ((w > 0 && w < width - 1) && (d > 0 && d < depth - 1) && (h > 0 && h < height - 1))
									{
										uvy--;
										continue;
									}

									var cubeOrigin = cube.Origin;

									Cube c = new Cube
									{
										Face = Face.Bottom,
										Size = new[] {1f, 1f, 1f},
										Origin = new[] {cubeOrigin[0] + w, cubeOrigin[1] + h - ZTearFactor, cubeOrigin[2] + d},
										Uv = new[] {uvx + w, uvy--},
										Velocity = new Vector3(0, (float) (random.NextDouble() * -0.01), 0)
									};
									if (random.NextDouble() < CubeFilterFactor)
									{
										bool isHead = bone.Name == BoneName.Head.ToString();
										if (packInBody)
										{
											if (keepHead && isHead)
												bone.Cubes.Add(c);
											else
												newCubes.Add(c);
										}
										else
										{
											bone.Cubes.Add(c);
										}
									}
								}
							}
						}
						// Right
						for (int w = 0; w < 1; w++)
						{
							float uvx = u;
							if (!bone.Mirror)
								uvx = u + depth - 1;
							for (int d = 0; d < depth; d++)
							{
								float uvy = v + depth + height - 2;
								for (int h = 0; h < height; h++)
								{
									if ((w > 0 && w < width - 1) && (d > 0 && d < depth - 1) && (h > 0 && h < height - 1))
									{
										uvy--;
										continue;
									}

									var cubeOrigin = cube.Origin;

									Cube c = new Cube
									{
										Face = Face.Right,
										Mirror = bone.Mirror,
										Size = new[] {1f, 1f, 1f},
										Origin = new[] {cubeOrigin[0] + w - ZTearFactor, cubeOrigin[1] + h, cubeOrigin[2] + d},
										Uv = !bone.Mirror ? new[] {uvx - d, uvy--} : new[] {uvx + d, uvy--},
										Velocity = new Vector3(0, (float) (random.NextDouble() * -0.01), 0)
									};
									if (random.NextDouble() < CubeFilterFactor)
									{
										bool isHead = bone.Name == BoneName.Head.ToString();
										if (packInBody)
										{
											if (keepHead && isHead)
												bone.Cubes.Add(c);
											else
												newCubes.Add(c);
										}
										else
										{
											bone.Cubes.Add(c);
										}
									}
								}
							}
						}
						// Left
						for (int w = width - 1; w < width; w++)
						{
							float uvx = u + depth + width - 2;
							if (bone.Mirror)
								uvx = u + depth - 1;
							for (int d = 0; d < depth; d++)
							{
								float uvy = v + depth + height - 2;
								for (int h = 0; h < height; h++)
								{
									if ((w > 0 && w < width - 1) && (d > 0 && d < depth - 1) && (h > 0 && h < height - 1))
									{
										uvy--;
										continue;
									}

									var cubeOrigin = cube.Origin;

									Cube c = new Cube
									{
										Face = Face.Left,
										Mirror = bone.Mirror,
										Size = new[] {1f, 1f, 1f},
										Origin = new[] {cubeOrigin[0] + w + ZTearFactor, cubeOrigin[1] + h, cubeOrigin[2] + d},
										Uv = bone.Mirror ? new[] {uvx - d, uvy--} : new[] {uvx + d, uvy--},
										Velocity = new Vector3(0, (float) (random.NextDouble() * -0.01), 0)
									};
									if (random.NextDouble() < CubeFilterFactor)
									{
										bool isHead = bone.Name == BoneName.Head.ToString();
										if (packInBody)
										{
											if (keepHead && isHead)
												bone.Cubes.Add(c);
											else
												newCubes.Add(c);
										}
										else
										{
											bone.Cubes.Add(c);
										}
									}
								}
							}
						}
					}
					// done bones
				}
			}

			if (packInBody)
			{
				Bone newBone = new Bone();
				newBone.Name = BoneName.Body.ToString();
				newBone.Pivot = new float[3];
				newBone.Cubes = newCubes;

				Bone head = Bones.SingleOrDefault(b => b.Name == BoneName.Head.ToString());
				Bones = new List<Bone> {newBone};
				if (keepHead && head != null)
				{
					Bones.Add(head);
				}
			}
		}
	}
}