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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Numerics;
using MiNET.Plotter;
using MiNET.Utils;
using NUnit.Framework;

namespace MiNET
{
	[TestFixture]
	public class PlotterTest
	{
		[Test, Ignore("")]
		public void PlotCoordinatesTest()
		{
			Assert.AreEqual(new PlotCoordinates(1, 1), (PlotCoordinates) new BlockCoordinates(PlotWorldGenerator.RoadWidth, 0, PlotWorldGenerator.RoadWidth));
			Assert.AreEqual(new PlotCoordinates(-1, -1), (PlotCoordinates) new BlockCoordinates(-1, 0, -1));

			Assert.AreEqual(
				new BlockCoordinates(PlotWorldGenerator.RoadWidth, PlotWorldGenerator.PlotHeight, PlotWorldGenerator.RoadWidth),
				PlotManager.ConvertToBlockCoordinates(new PlotCoordinates(1, 1)));

			Assert.AreEqual(
				new BlockCoordinates(-PlotWorldGenerator.PlotWidth, PlotWorldGenerator.PlotHeight, -PlotWorldGenerator.PlotWidth),
				PlotManager.ConvertToBlockCoordinates(new PlotCoordinates(-1, -1)));


			//BoundingBox bbox = BoundingBox.CreateFromPoints(new[] {new Vector3(1f, 1f, 1f), new Vector3(-1f, -1f, -1f)});
			//Assert.AreEqual(-1, bbox.Min.X);


			Vector3 offset = new BlockCoordinates(-1, 0, -1);
			Vector3 to = offset + (Vector3) new BlockCoordinates(PlotWorldGenerator.PlotWidth*Math.Sign(offset.X), 0, PlotWorldGenerator.PlotDepth*Math.Sign(offset.Z));
			BoundingBox bbox = BoundingBox.CreateFromPoints(new[] {offset, to});
			Assert.AreEqual(-PlotWorldGenerator.PlotWidth - 1, bbox.Min.X);
			Assert.AreEqual(-1, bbox.Max.X);
		}
	}
}