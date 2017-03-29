using log4net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class ElderGuardian : HostileMob, IAgeable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ElderGuardian));

		public ElderGuardian(Level level) : base(EntityType.ElderGuardian, level)
		{
			Width = Length = 1.9;
			Height = 1.9;
			IsElder = true;
		}

		// Metadata: 
		//[0] long 7 536870912, 
		//[1] int 2 1, 
		//[2] int 2 0, 
		//[3] byte 0 0, 
		//[4] string 4 , 
		//[5] long 7 1, 
		//[6] long 7 0, 
		//[7] short 1 300, 
		//[8] int 2 0, 
		//[9] byte 0 0, 
		//[38] long 7 0, 
		//[39] float 3 1, 
		//[44] short 1 300, 
		//[45] int 2 0, 
		//[46] byte 0 0, 
		//[47] int 2 0, 
		//[53] float 3 1,99, 
		//[54] float 3 1,99, 
		//[56] vector3 8 <0  0  0>, 
		//[57] byte 0 0, 
		//[58] float 3 0, 
		//[59] float 3 0

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = base.GetMetadata();
			//MetadataDictionary metadata = new MetadataDictionary();
			metadata[0] = new MetadataLong(GetDataValue());
			metadata[39] = new MetadataFloat(1.0f);
			metadata[44] = new MetadataShort(300);
			metadata[53] = new MetadataFloat(1.99f);
			metadata[54] = new MetadataFloat(1.99f);
			metadata[57] = new MetadataByte(0);
			metadata[58] = new MetadataFloat(0);
			metadata[59] = new MetadataFloat(0);

			return metadata;
		}
	}
}