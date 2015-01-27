using Craft.Net.Common;
using MiNET.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Worlds;

namespace MiNET
{
    class Explosion
    {
        List<Block> afectedBlocks = new List<Block>();
        const double stepLen = 0.3;
        const int rays = 16;
        const int mRays = 15;
        private double Size = 0;
        private readonly Level World;
        private Coordinates3D _centerCoordinates;

        public Explosion(Level world, Coordinates3D centerCoordinates, double size)
        {
            Size = size;
            _centerCoordinates = centerCoordinates;
            World = world;
        }

        public bool Explode()
        {
            if (ExplosionA())
            {
                return ExplosionB();
            }

            return false;
        }

        private bool ExplosionA()
        {
            if (Size < 0.1) return false;

            var vector = new Vector3(0, 0, 0);
            var bvector = new Vector3(0, 0, 0);
            for (var i = 0; i < rays; i++)
            {
                for (var j = 0; j < rays; j++)
                {
                    for (var k = 0; k < rays; k++)
                    {
                        if (i == 0 || i == mRays || j == 0 || j == mRays || k == 0 || k == mRays)
                        {
                            vector.X = i / mRays * 2 - 1;
                            vector.Y = j / mRays * 2 - 1;
                            vector.Z = k / mRays * 2 - 1;
                            vector.X = (vector.X / VectorLength(vector)) * stepLen;
                            vector.Y = (vector.Y / VectorLength(vector)) * stepLen;
                            vector.Z = (vector.Z / VectorLength(vector)) * stepLen;
                            double pointerX = _centerCoordinates.X;
                            double pointerY = _centerCoordinates.Y;
                            double pointerZ = _centerCoordinates.Z;
                            for (var blastForce = Size * ((new Random().NextDouble() * (1300 - 700) + 700) / 1000); blastForce > 0; blastForce -= stepLen * 0.75)
                            {
                                var x = pointerX;
                                var y = pointerY;
                                var z = pointerZ;
                                bvector.X = pointerX >= x ? x : x - 1;
                                bvector.Y = pointerY >= y ? y : y - 1;
                                bvector.Z = pointerZ >= z ? z : z - 1;
                                if (bvector.Y < 0 || bvector.Y > 127)
                                {
                                    break;
                                }
                                var block = World.GetBlock(new Coordinates3D((int)bvector.X, (int)bvector.Y, (int)bvector.Z));
                                if (block.Id != 0)
                                {
                                    blastForce -= (block.Hardness / 5 + 0.3) * stepLen;
                                    if (blastForce > 0)
                                    {
                                        if (!afectedBlocks.Contains(block))
                                        {
                                            afectedBlocks.Add(block);
                                        }
                                    }
                                }
                                pointerX += vector.X;
                                pointerY += vector.Y;
                                pointerZ += vector.Z;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool ExplosionB()
        {
            Vector3 source = new Vector3(_centerCoordinates.X, _centerCoordinates.Y, _centerCoordinates.Z).Floor();
            var yield = (1 / Size) * 100;
            var explosionSize = Size * 2;
            var minX = Math.Floor(_centerCoordinates.X - explosionSize - 1);
            var maxX = Math.Floor(_centerCoordinates.X + explosionSize + 1);
            var minY = Math.Floor(_centerCoordinates.Y - explosionSize - 1);
            var maxY = Math.Floor(_centerCoordinates.Y + explosionSize + 1);
            var minZ = Math.Floor(_centerCoordinates.Z - explosionSize - 1);
            var maxZ = Math.Floor(_centerCoordinates.Z + explosionSize + 1);
            var explosionBB = new BoundingBox(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));
            var Air = new BlockAir();

            foreach (var block in afectedBlocks)
            {
                World.SetBlock(new BlockAir() { Coordinates = block.Coordinates});
            }
            return true;
        }

        private double VectorLength(Vector3 vector)
        {
            return Math.Sqrt(vector.X * 2 + vector.Y * 2 + vector.Z * 2);
        }
    }
}
