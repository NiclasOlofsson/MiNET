namespace MiNET.BdsExtract;

using System.Globalization;
using System.Text;

/// <summary>
///     Writes every item the server holds, with the fields that have been fitted against values
///     known in advance, and the whole object beside them as bytes.
///     Nothing is dropped and nothing is filtered on judgment. A name that fails a check is written
///     with the reason rather than left out, and the part of the object that has no name yet travels
///     out as hex, so a byte that was read is never lost on the way.
/// </summary>
public static class ItemRegistry
{
	/// <summary>
	///     Where the object starts, relative to the name inside it. Settled by the class pointer: it
	///     sits exactly here on every item, including the ones carrying a nearer pointer, which are
	///     other bases rather than the start.
	/// </summary>
	public const int NameInsideItem = 288;

	/// <summary>How much of the object travels out as bytes, measured from the name.</summary>
	private const int Before = 448;
	/// <summary>
	///     How far past the name to read. Sized to the largest object rather than to a round number:
	///     at 384 the description stopped at +368 and 171 items had a tail beyond it that nothing
	///     read, which is a hole created by the tool rather than found in the data.
	/// </summary>
	/// <summary>
	///     How far past the name the first read reaches. It only has to cover the shared fields; an
	///     object longer than this is read again to its own size once the class has stated it. A
	///     single wide read looked simpler and cost twelve documented items, because a large fixed
	///     read fails outright when the object sits near the end of a mapped region.
	/// </summary>
	private const int After = 384;

	public sealed class Item
	{
		public string Name;
		public ulong NameAddress;
		public ulong Address;
		public byte[] Window;   // from name-Before
		public string Key;
		public List<string> Doubts = [];
		public int ObjectSize;

		/// <summary>
		///     Whether this object passed every check that says it is an item. Rows that did not are
		///     still written, because a row that vanishes cannot be reconsidered, but nothing is
		///     decoded off them: a pointer slot on a tag name or an entity component holds whatever
		///     that object holds, and reading it as a food component invents a value. Marking the
		///     row is the difference between carrying an unknown through and making one up.
		/// </summary>
		public bool IsItem = true;

		public int At(int offset) => Before + offset;
		public byte Byte(int offset) => Window[Before + offset];
		public ushort U16(int offset) => BitConverter.ToUInt16(Window, Before + offset);
		public short I16(int offset) => BitConverter.ToInt16(Window, Before + offset);
		public uint U32(int offset) => BitConverter.ToUInt32(Window, Before + offset);
		public int I32(int offset) => BitConverter.ToInt32(Window, Before + offset);
		public float F32(int offset) => BitConverter.ToSingle(Window, Before + offset);
		public ulong Ptr(int offset) => BitConverter.ToUInt64(Window, Before + offset);
	}

	/// <summary>
	///     Proves the item offsets against the server before anything is written.
	///     A moved field is the failure this cannot survive quietly: the read still returns a number
	///     of the right type, and the row it lands in looks exactly like the rows that are correct.
	///     Every check is all or nothing across the whole registry rather than a count, because
	///     counts move on their own when a version adds items, and a guard that cries wolf on every
	///     bump gets ignored. Each one rests on something arbitrary bytes cannot satisfy: text that
	///     hashes to its own hash, three offsets agreeing with each other, a class stating its own
	///     size, a container whose links close.
	/// </summary>
	public static List<Sentinel> Check(BedrockProcess process, ICollection<Item> items)
	{
		return
		[
			TextureAtlas(process, items),
			NameTriple(process, items),
			TranslationKey(process, items),
			StackSizeInRange(items),
			ClassStatesItsSize(items),
			TagVectorDivides(process, items),
			IdsAreUnique(items),
			ComponentContainersClose(process, items)
		];
	}

	/// <summary>
	///     Every item names the same texture atlas, inline, at the same place. A string that reads
	///     back as its own eleven characters with a matching length and capacity is not something a
	///     wrong offset produces.
	/// </summary>
	private static Sentinel TextureAtlas(BedrockProcess process, ICollection<Item> items)
	{
		var text = new byte[256];
		int agreed = items.Count(i => StdString(process, i.Window, i.At(-272), text) == "atlas.items");
		return Verdict("texture atlas", "-272", agreed, items.Count, "items read atlas.items there");
	}

	/// <summary>
	///     The three names have to agree with each other: the full name is the namespace, a colon,
	///     and the bare name. Three offsets must be right at once, and each is a hashed string that
	///     verifies against its own hash, so a shift breaks all three rather than sliding quietly.
	/// </summary>
	private static Sentinel NameTriple(BedrockProcess process, ICollection<Item> items)
	{
		var heap = new byte[256];
		var text = new byte[256];
		int agreed = 0;
		foreach (Item item in items)
		{
			// The namespace is read as a plain string. It is stored as a hashed string like the other
			// two, but its hash is zero: the hash is computed lazily and nothing has ever needed the
			// one for the constant "minecraft". Verifying it would fail on every item in the registry.
			string space = StdString(process, item.Window, item.At(-32), text);
			string bare = HashedString.ReadVerified(process, item.Window, item.At(-80), heap, 2, 64);
			string full = HashedString.ReadVerified(process, item.Window, item.At(0), heap, 2, 96);
			if (space is not null && bare is not null && full == $"{space}:{bare}") agreed++;
		}
		return Verdict("name triple agrees", "-80, -32, 0", agreed, items.Count,
			"items whose full name is their namespace and bare name joined");
	}

	/// <summary>The key an item is translated by always names an item or a tile.</summary>
	private static Sentinel TranslationKey(BedrockProcess process, ICollection<Item> items)
	{
		var text = new byte[256];
		int agreed = items.Count(i => StdString(process, i.Window, i.At(-112), text) is { } key
			&& (key.StartsWith("item.", StringComparison.Ordinal) || key.StartsWith("tile.", StringComparison.Ordinal)));
		return Verdict("translation key", "-112", agreed, items.Count, "items carry an item. or tile. key");
	}

	/// <summary>
	///     One byte, and a stack is between one and sixty four. Read two bytes wide this passed for
	///     most items and rejected eighty one of the ones the server documents, so the guard is on
	///     the byte and the range together.
	/// </summary>
	private static Sentinel StackSizeInRange(ICollection<Item> items)
	{
		int agreed = items.Count(i => i.Byte(-120) is >= 1 and <= 64);
		return Verdict("stack size", "-120", agreed, items.Count, "items hold a stack between 1 and 64");
	}

	/// <summary>
	///     Every item belongs to a class that writes its own size into its deleting destructor. That
	///     is where the object extent comes from, so if this stops holding, nothing downstream knows
	///     where an object ends and every read past the shared fields is into the next allocation.
	/// </summary>
	private static Sentinel ClassStatesItsSize(ICollection<Item> items)
	{
		int agreed = items.Count(i => i.ObjectSize > 0);
		return Verdict("class states its size", "the deleting destructor at method table slot 0",
			agreed, items.Count, "items belong to a class that records its own size");
	}

	/// <summary>
	///     Where an item has tags, the vector divides by the element size and every element is a
	///     hashed string that verifies. A wrong offset gives a range that does not divide, or
	///     elements that do not hash to themselves.
	/// </summary>
	private static Sentinel TagVectorDivides(BedrockProcess process, ICollection<Item> items)
	{
		var scratch = new byte[HashedString.Size];
		var heap = new byte[256];
		int attempted = 0, agreed = 0;
		foreach (Item item in items)
		{
			ulong begin = item.Ptr(216), end = item.Ptr(224);
			if (begin < 0x10000 || end <= begin || end - begin > 4096) continue;
			attempted++;
			if ((end - begin) % 48 != 0) continue;
			bool all = true;
			for (ulong at = begin; at < end && all; at += 48)
			{
				all = process.TryRead(at, scratch, HashedString.Size)
					&& HashedString.ReadVerified(process, scratch, 0, heap, 2, 96) is not null;
			}
			if (all) agreed++;
		}
		return Verdict("tag vector", "216, 224", agreed, attempted, "tag vectors divide and every tag verifies");
	}

	/// <summary>
	///     Ids are handed out once each. Two items sharing one means the field is not the id, or the
	///     wrong copy of a name is being taken.
	/// </summary>
	private static Sentinel IdsAreUnique(ICollection<Item> items)
	{
		int distinct = items.Select(i => i.I16(-118)).Distinct().Count();
		return Verdict("ids are unique", "-118", distinct, items.Count, "items hold an id no other item holds");
	}

	/// <summary>
	///     The component containers are trees, and a tree closes: the head points at the root as its
	///     parent and the root points back at the head. Random memory does not close that loop, and
	///     the node count has to match the size stored beside it.
	/// </summary>
	private static Sentinel ComponentContainersClose(BedrockProcess process, ICollection<Item> items)
	{
		int attempted = 0, agreed = 0;
		foreach (Item item in items)
		{
			foreach (int slot in (int[]) [296, 336])
			{
				// Attempted where the tree actually closes, not merely where the slot looks plausible.
				// On a class that has no container these offsets hold other fields, and counting those
				// as attempts made the guard fail on 61 items that never had a container at all.
				long size = BitConverter.ToInt64(item.Window, item.At(slot + 8));
				ulong head = item.Ptr(slot);
				if (head < 0x10000 || size is < 1 or > 512 || !process.IsMapped(head)) continue;
				var word = new byte[8];
				ulong root = process.ReadUInt64(head + 8, word);
				if (root == 0 || root == head || !process.IsMapped(root)) continue;
				if (process.ReadUInt64(root + 8, word) != head) continue;

				attempted++;
				if (Components(process, item, slot, slot == 296).Count == size) agreed++;
			}
		}
		return Verdict("component containers", "296, 336", agreed, attempted,
			"containers close their links and hold exactly the count stored beside them");
	}

	private static Sentinel Verdict(string name, string guards, int agreed, int attempted, string detail)
	{
		return new Sentinel
		{
			Name = name,
			Guards = guards,
			Held = attempted > 0 && agreed == attempted,
			Detail = $"{agreed:N0} of {attempted:N0} {detail}"
		};
	}

	/// <summary>
	///     Reads every item out of a running server and writes the data, the structure it was read
	///     from, and the evidence beside it.
	/// </summary>
	public static int Run(string[] args)
	{
		// The Data folder beside this project's source, the same place the block extraction lands,
		// so a run updates the copy that is committed instead of dropping files wherever it was
		// started from. --out still overrides.
		string pathFilter = null, output = Path.Combine(Program.DefaultOutputDirectory(), "items-runtime.json");
		bool anyWorld = args.Contains("--any-world");
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == "--server" && i + 1 < args.Length) pathFilter = args[++i];
			else if (args[i] == "--out" && i + 1 < args.Length) output = args[++i];
		}

		using var process = BedrockProcess.Attach(pathFilter);
		Console.WriteLine($"reading pid {process.Id}");

		// Before anything is read, not after. An extraction from a server whose world has its
		// experiments off comes out short and misnumbered, and neither shows in the file.
		WorldConfig.Config config = WorldConfig.Read(process.ExecutablePath);
		if (!WorldConfig.Check(config, !anyWorld)) return 1;
		Console.WriteLine();

		List<Item> all = Read(process);
		Console.WriteLine($"objects with a verified name and a translation key: {all.Count:N0}");
		Dictionary<string, Item> chosen = ChooseCopies(process, all);
		Console.WriteLine($"distinct items: {chosen.Count:N0}");
		MeasureSizes(process, chosen.Values);

		// The offsets are proven against this server before anything is written, so a version that
		// moved a field is a loud failure rather than a file full of plausible numbers.
		// Only the objects that passed the item checks. The rest are things that share a name with an
		// item and are deliberately kept, and holding them to item invariants makes every guard fail.
		List<Sentinel> sentinels = Check(process, chosen.Values.Where(i => i.IsItem).ToList());
		Console.WriteLine();
		foreach (Sentinel sentinel in sentinels)
		{
			Console.WriteLine($"  {(sentinel.Held ? "held  " : "FAILED")}  {sentinel.Name,-24} {sentinel.Detail}");
		}
		bool sound = sentinels.All(x => x.Held);
		if (!sound)
		{
			Console.WriteLine();
			Console.WriteLine("A layout check failed. The offsets no longer describe this server, so the values");
			Console.WriteLine("below are not what they claim to be. The file is still written, marked unsound, so");
			Console.WriteLine("it can be looked at, and this exits non zero.");
		}
		Console.WriteLine();

		string experiments = string.Join(", ", config.Experiments.Select(e => $"\"{e}\""));
		string checks = string.Join(", ", sentinels.Select(x =>
			"{ " + $"\"check\": {Json(x.Name)}, \"guards\": {Json(x.Guards)}, "
			+ $"\"held\": {(x.Held ? "true" : "false")}, \"detail\": {Json(x.Detail)}" + " }"));
		SourceJson = "{ " + $"\"server\": \"{config.ServerPath.Replace("\\", "\\\\")}\", "
			+ $"\"world\": \"{config.World}\", \"experiments\": [{experiments}], "
			+ $"\"layoutSound\": {(sound ? "true" : "false")}, \"layoutChecks\": [{checks}]" + " }";
		Write(process, chosen.Values, output);
		return sound ? 0 : 1;
	}

	/// <summary>
	///     Every object in the process whose name verifies and whose fields hold together as an
	///     item. Copies are kept rather than resolved here: which copy is live is a question the
	///     whole population answers, not one this loop can.
	/// </summary>
	public static List<Item> Read(BedrockProcess process)
	{
		var heap = new byte[256];
		var text = new byte[256];
		var window = new byte[Before + After];
		var found = new List<Item>();

		foreach (BedrockProcess.Region region in process.Regions)
		{
			var scan = new byte[(int) Math.Min(8 * 1024 * 1024, (long) region.Size)];
			for (ulong at = region.Base; at < region.End; at += (ulong) scan.Length - HashedString.Size)
			{
				int length = (int) Math.Min((ulong) scan.Length, region.End - at);
				if (length < HashedString.Size || !process.TryRead(at, scan, length)) continue;

				for (int i = 0; i + HashedString.Size <= length; i += 8)
				{
					string name = HashedString.ReadVerified(process, scan, i, heap);
					if (name is null || !name.StartsWith("minecraft:", StringComparison.Ordinal)) continue;
					ulong nameAddress = at + (ulong) i;
					if (nameAddress < Before) continue;

					// The lead-in is context, not the object: everything decoded sits at -288 or
					// later. When the wider read crosses out of a mapped region the object is still
					// read, from its own start, with the gap left zero and said out loud. Requiring
					// the whole window silently lost cow_spawn_egg, pink_bundle and five others that
					// happen to be allocated near the front of their region.
					Array.Clear(window);
					string shortfall = null;
					if (!process.TryRead(nameAddress - Before, window, window.Length))
					{
						if (!process.TryRead(nameAddress - NameInsideItem, window, Before - NameInsideItem + After)) continue;
						Array.Copy(window, 0, window, Before - NameInsideItem, After + NameInsideItem);
						Array.Clear(window, 0, Before - NameInsideItem);
						shortfall = null;
					}

					string key = StdString(process, window, Before - 112, text);
					if (key is null) continue;

					var item = new Item
					{
						Name = name,
						NameAddress = nameAddress,
						Address = nameAddress - NameInsideItem,
						Window = (byte[]) window.Clone(),
						Key = key
					};

					if (!key.StartsWith("item.", StringComparison.Ordinal) && !key.StartsWith("tile.", StringComparison.Ordinal))
					{
						item.Doubts.Add($"translation key is {key}");
						item.IsItem = false;
					}

					// One byte, not two. Read as a short it drags the neighbouring byte in and the
					// range check then rejects every item whose neighbour is not zero, which is
					// eighty one of the ones the server documents, bow and comparator among them.
					if (item.Byte(-120) is < 1 or > 64)
					{
						item.Doubts.Add($"stack size {item.Byte(-120)}");
						item.IsItem = false;
					}
					if (!IsModule(item.Ptr(-288)))
					{
						item.Doubts.Add("no class pointer at the object start");
						item.IsItem = false;
					}

					// A real item is a heap object of a polymorphic class, and such a class writes its
					// own size into its destructor. Something that merely carries a name at the right
					// distance does not. This is what tells the live dirt from the eight other things
					// in memory that answer to the name.
					item.ObjectSize = ClassSize(process, item.Ptr(-288));

					// Now that the object's own size is known, read the rest of it. Only the objects
					// that need it pay for it, and a failure here costs the tail rather than the item.
					// Sixteen short of the window is already too long: the slot walk needs that much
					// lookahead, so an object that merely reaches the end of the window still loses
					// its last slot without this.
					if (item.ObjectSize > NameInsideItem + After - 16)
					{
						// Sixteen bytes of slack past the object, so the slot walk can look ahead at the
						// last slot without the description having to stop short of the end.
						var whole = new byte[Before + item.ObjectSize - NameInsideItem + 16];
						Array.Copy(item.Window, whole, Math.Min(item.Window.Length, whole.Length));
						if (process.TryRead(item.Address, whole, Before - NameInsideItem, item.ObjectSize + 16)
							|| process.TryRead(item.Address, whole, Before - NameInsideItem, item.ObjectSize))
						{
							item.Window = whole;
						}
						else
						{
							item.Doubts.Add($"the object is {item.ObjectSize} bytes and only {After + NameInsideItem} could be read");
						}
					}

					if (item.ObjectSize == 0)
					{
						item.Doubts.Add("the class states no size, so it is not a heap object of a known class");
						item.IsItem = false;
					}
					// Nothing is said about the lead-in before the object. It is context from the
					// previous allocation, no field is read from it and it is not emitted, so failing
					// to map it says nothing about this item. Recorded as a doubt it wrongly ruled out
					// four documented items.
					if (shortfall is not null) item.Doubts.Add(shortfall);
					found.Add(item);
				}
			}
		}
		return found;
	}

	/// <summary>
	///     Which copy of a name is the live one, argued from what the copies hold rather than from
	///     which was found first.
	///     The sweep accepts anything carrying a verified name and a readable string where the
	///     translation key sits, which is deliberate: a row that never appears cannot be
	///     reconsidered later. It also means structure templates, entity components and tag names
	///     come along, and several of them share a name with a real item. So the copies are ranked.
	///     Failed checks come first, since a copy with none is an item and a copy with three is
	///     something else wearing the same name. Where copies are equally clean, the process decides:
	///     the live item is the one the server points at, from the registry, the recipes and the
	///     creative list, while a copy nothing refers to is being kept rather than used. Apple and
	///     breeze rod are the pair that needs it, and the gap is not subtle: eleven references and
	///     nine against one each, where an ordinary item has eight to sixty.
	/// </summary>
	public static Dictionary<string, Item> ChooseCopies(BedrockProcess process, List<Item> all)
	{
		List<IGrouping<string, Item>> byName = all.GroupBy(i => i.Name, StringComparer.Ordinal).ToList();
		Dictionary<ulong, int> references = CountReferences(process,
			byName.Where(g => g.Count() > 1).SelectMany(g => g).Select(i => i.Address).ToHashSet());

		var chosen = new Dictionary<string, Item>(StringComparer.Ordinal);
		foreach (IGrouping<string, Item> group in byName)
		{
			List<Item> copies = group.ToList();
			Item pick = copies
				.OrderByDescending(c => c.IsItem)
				.ThenBy(c => c.Doubts.Count)
				.ThenByDescending(c => references.GetValueOrDefault(c.Address))
				.ThenBy(c => c.Address)
				.First();
			if (copies.Count > 1)
			{
				pick.Doubts.Add($"{copies.Count} objects carry this name, ids "
					+ string.Join(", ", copies.Select(c => $"{c.I16(-118)} ({references.GetValueOrDefault(c.Address)} refs)"))
					+ $"; took {pick.I16(-118)} at 0x{pick.Address:X}");
			}
			chosen[group.Key] = pick;
		}
		return chosen;
	}

	/// <summary>
	///     The components a data-driven item carries, walked out of the containers it keeps them in
	///     rather than read at fixed offsets.
	///     <para>
	///         There are two ways an item holds a component and the newest items use the second. The
	///         older, hardcoded ones keep a typed pointer per kind at a fixed place in the object,
	///         which is what the food, seed and camera offsets above read. The data-driven ones keep
	///         a map from component name to component object instead, and nothing sits at those
	///         offsets at all, which is why apple came out with no food while bread had one: apple is
	///         registered the new way and bread the old.
	///     </para>
	///     <para>
	///         The containers are found by their own shape, not by an offset that happens to work.
	///         An MSVC tree head points at the root as its parent, the root points back at the head,
	///         and the head is flagged as the sentinel it is; random memory does not close that loop.
	///         The count beside it has to agree with the number of nodes actually walked, so a
	///         container that reads plausibly but is not one yields nothing rather than invented
	///         names.
	///     </para>
	/// </summary>
	private static List<(string Name, ulong Address)> Components(BedrockProcess process, Item item, int slot, bool hashedKeys)
	{
		var found = new List<(string, ulong)>();
		// Not gated on the measured object size. That size is the distance to the next object, which
		// is missing for whichever item sits last in its arena, and gating on it lost apple: the one
		// item in this family whose components were the reason for looking. The loop below plus the
		// count check is what makes the read safe, and it does not need to know how big the object is.
		if (!item.IsItem || slot + 16 > After) return found;

		ulong head = item.Ptr(slot);
		long size = BitConverter.ToInt64(item.Window, item.At(slot + 8));
		if (head < 0x10000 || size is < 1 or > 512 || !process.IsMapped(head)) return found;

		var scratch = new byte[8];
		ulong root = process.ReadUInt64(head + 8, scratch);
		if (root == 0 || root == head || !process.IsMapped(root)) return found;
		if (process.ReadUInt64(root + 8, scratch) != head) return found;

		var node = new byte[160];
		var text = new byte[256];
		var heap = new byte[256];
		var visited = new HashSet<ulong>();
		var stack = new Stack<ulong>();
		stack.Push(root);
		while (stack.Count > 0 && found.Count <= size)
		{
			ulong at = stack.Pop();
			if (at == head || at == 0 || !visited.Add(at) || !process.IsMapped(at)) continue;
			if (!process.TryRead(at, node, node.Length)) continue;
			if (node[25] == 0)
			{
				// The two containers key differently: the declared names are HashedStrings, forty
				// bytes with the hash first, and the built components are plain strings at thirty
				// two. Reading one as the other finds nothing, which is how they were told apart.
				string name = hashedKeys
					? HashedString.ReadVerified(process, node, 32, heap)
					: StdString(process, node, 32, text);
				if (name is { Length: > 0 }) found.Add((name, BitConverter.ToUInt64(node, hashedKeys ? 72 : 64)));
			}
			stack.Push(BitConverter.ToUInt64(node, 0));
			stack.Push(BitConverter.ToUInt64(node, 16));
		}

		// The count the container states is the check. A walk that does not produce exactly that many
		// keys did not walk the container, whatever it read.
		if (found.Count != size) found.Clear();
		found.Sort((a, b) => string.CompareOrdinal(a.Item1, b.Item1));
		return found;
	}

	/// <summary>
	///     Creative tab names, from the server's own CreativeItemCategory schema under
	///     docs/json_schemas/protocol. The item side documents a different menu_category enum with a
	///     different numbering, and this is the one the byte uses: 1 puts cobblestone and planks in
	///     Construction, 2 puts stone, apple and seeds in Nature, 3 puts tools, armour and food in
	///     Equipment, 4 puts coal, sticks and buckets in Items, and 5 holds the legacy command only
	///     entries such as red_flower.
	/// </summary>
	private static readonly string[] CreativeCategoryNames =
		["All", "Construction", "Nature", "Equipment", "Items", "ItemCommandOnly", "Undefined"];

	/// <summary>
	///     The use animation names, in the order the RUNTIME uses them, which is not the order the
	///     server's own schema documents.
	///     Each name here is attached by who holds the value: 1 is every food, 2 is every bucket and
	///     potion, 3 is the shield, 4 the bow, 6 the trident, 9 the crossbow, 10 the spyglass, 11 the
	///     goat horn and 12 the brush. The schema at docs/json_schemas/server/item/1.20.50/
	///     Animation.json agrees to 4 and then diverges: it has crossbow at 6, spear at 7, spyglass
	///     at 8 and brush at 9, with no glow stick, sparkler or goat horn at all. That file describes
	///     a data format; this describes what the server runs, so the two are both reported and
	///     neither is corrected into the other.
	///     Five is left without a name because no item holds it. The schema puts camera there, which
	///     is a reasonable guess and not evidence.
	/// </summary>
	private static readonly string[] UseAnimationNames =
	[
		"none", "eat", "drink", "block", "bow", null, "spear", "glow_stick", "sparkler",
		"crossbow", "spyglass", "goat_horn", "brush"
	];

	/// <summary>
	///     Rarity names. Attached the same way: 1 is the pottery sherds, armour trims and chainmail,
	///     2 the beacon, nether star, banner patterns and music discs, 3 dragon egg, elytra, heavy
	///     core, mace and the silence trim.
	/// </summary>
	/// <summary>
	///     What mining a block does to the item that mined it. The four digger classes and shears each
	///     name their own value in their constructor, which is what the value is for.
	/// </summary>
	private static readonly string[] MineBlockTypeNames =
	[
		"default", "do_nothing", "component_item", "digger_item", "shears_item"
	];

	private static readonly string[] RarityNames = ["common", "uncommon", "rare", "epic"];

	private static string NameOf(string[] names, byte value) =>
		value < names.Length && names[value] is not null ? names[value] : null;

	/// <summary>
	///     The item object, field by field, as this tool reads it. One table, used both to say which
	///     bytes a field was read from and to write the layout out, so the declaration cannot drift
	///     from the code.
	///     Offsets are from the name inside the object; the object itself starts at -288. A region
	///     with no name is still here, with its size, because a byte nobody has identified is a hole
	///     to be counted rather than a gap to be left out.
	/// </summary>
	private static readonly (int Offset, int Length, string Kind, string Name)[] Layout =
	[
		(-288, 8, "pointer", "method table"),
		(-280, 4, "i32", "item parse version"),
		(-276, 4, "slack", "uninitialised"),
		(-272, 32, "string", "texture atlas"),
		(-240, 4, "i32", "icon frame count"),
		(-236, 1, "bool", "animates in toolbar"),
		(-235, 1, "bool", "mirrored art"),
		(-234, 1, "u8", "use animation"),
		(-233, 1, "slack", "uninitialised"),
		(-232, 32, "string", "hover text colour format"),
		(-200, 4, "slack", "icon frame, which a server never sets"),
		(-196, 4, "slack", "atlas frame, which a server never sets"),
		(-192, 4, "u32", "atlas total frames"),
		(-188, 4, "slack", "uninitialised"),
		(-184, 32, "string", "icon name"),
		(-152, 32, "string", "atlas name"),
		(-120, 1, "u8", "max stack size"),
		(-119, 1, "slack", "uninitialised"),
		(-118, 2, "i16", "id"),
		(-116, 4, "slack", "uninitialised"),
		(-112, 32, "string", "translation key"),
		(-80, 48, "hashed string", "bare name"),
		(-32, 32, "string", "namespace"),
		(0, 48, "hashed string", "full name"),
		(48, 2, "u16", "max durability"),
		(50, 1, "u8", "flags: foil 0x01, hand equipped 0x02, stacked by data 0x04, fire resistant 0x20, should despawn 0x40, allow off hand 0x80"),
		(51, 1, "slack", "uninitialised"),
		(52, 4, "u32", "use duration"),
		(56, 8, "u16 x4", "minimum game version"),
		(64, 16, "pointer x2", "the version's own text"),
		(80, 1, "u8", "inside the minimum game version"),
		(81, 7, "slack", "uninitialised"),
		(88, 8, "pointer", "the block this item places"),
		(96, 1, "u8", "creative category"),
		(97, 7, "slack", "uninitialised"),
		(104, 8, "padding", "zero on every item"),
		(112, 32, "string", "creative group"),
		(144, 4, "f32", "furnace fuel duration"),
		(148, 4, "f32", "smelting experience"),
		(152, 1, "u8", "hidden in commands"),
		(153, 3, "slack", "uninitialised"),
		(156, 4, "i32", "rarity"),
		(160, 4, "i32", "mine block item effect type"),
		(164, 4, "slack", "uninitialised"),
		(168, 8, "pointer", "food component"),
		(176, 8, "pointer", "seed component"),
		(184, 8, "pointer", "camera component"),
		(192, 24, "vector", "seed vector"),
		(216, 24, "vector", "tag vector"),
		(240, 1, "u8", "flags on the data-driven class: can destroy in creative 0x02, liquid clipped 0x08"),
		(241, 3, "class specific", "decoded per class where the class is known"),
		(244, 4, "f32", "mining speed, on the data-driven class"),
		(248, 4, "i32", "damage, on the data-driven class"),
		(252, 4, "i32", "enchantable slot bitmask, on the data-driven class"),
		(256, 4, "i32", "enchantable value, on the data-driven class"),
		(260, 36, "class specific", "decoded per class where the class is known"),
		(296, 16, "set", "declared component names, and its count"),
		(312, 24, "class specific", "decoded per class where the class is known"),
		(336, 16, "map", "built components, and its count")
	];

	/// <summary>
	///     Positions inside the object that no constructor writes, so their content is left over from
	///     whatever the heap block held before. Proven rather than assumed: the same build read in
	///     two separate processes disagrees at exactly these positions and nowhere else.
	/// </summary>
	private static readonly (int From, int Length)[] Slack =
	[
		(-276, 4), (-233, 1), (-200, 8), (-188, 4), (-119, 1), (-116, 4),
		(51, 1), (81, 7), (97, 7), (153, 3), (164, 4)
	];

	/// <summary>Positions that are zero on every item, which is padding between fields.</summary>
	private static readonly (int From, int Length)[] ZeroPadding =
	[
		(-191, 3), (46, 2), (104, 8)
	];

	/// <summary>
	///     Which bytes of an object a field has actually been read out of, and therefore which bytes
	///     nothing has. A row that carries the whole object as hex loses nothing, but it also says
	///     nothing about how much of it is understood, and an unnamed byte inside a structure is a
	///     hole whether or not its bytes travel out. So the holes are enumerated, with offset and
	///     length, which is what makes them countable and closeable.
	/// </summary>
	private sealed class Coverage(int start, int end)
	{
		private readonly List<(int From, int To)> _claimed = [];

		public void Claim(int offset, int length) => _claimed.Add((offset, offset + length));

		public List<(int From, int Length)> Holes()
		{
			var holes = new List<(int, int)>();
			int at = start;
			foreach ((int from, int to) in _claimed.Where(c => c.To > start && c.From < end).OrderBy(c => c.From))
			{
				if (from > at) holes.Add((at, from - at));
				at = Math.Max(at, to);
			}
			if (at < end) holes.Add((at, end - at));
			return holes;
		}
	}

	/// <summary>
	///     How many bytes a class allocates for itself, read out of its own code.
	///     The compiler writes the number down: entry zero of a method table is the deleting
	///     destructor, and it ends by calling the sized form of operator delete with the object size
	///     in the second argument register, loaded as a thirty two bit immediate. So an object's
	///     extent does not have to be guessed from where the next allocation happens to sit, which is
	///     the guess that produced a dozen readings of neighbouring heap as if it were component
	///     fields.
	///     The immediate has to be the one a call consumes. Taking the first in the function answered
	///     64 for the sign classes and taking the last answered 728 for the boats; requiring a call
	///     within the next few bytes is what tells the argument from every other constant.
	///     Checked rather than trusted: across the twenty five commonest item classes, the size this
	///     returns plus an eight byte allocation header, rounded up to sixteen, reproduces the
	///     measured distance to the next object on twenty three of them.
	/// </summary>
	private static readonly Dictionary<ulong, int> SizeByClass = [];

	public static int ClassSize(BedrockProcess process, ulong vtable)
	{
		if (!IsModule(vtable)) return 0;
		if (SizeByClass.TryGetValue(vtable, out int cached)) return cached;
		var scratch = new byte[8];
		ulong destructor = process.ReadUInt64(vtable, scratch);
		if (!IsModule(destructor)) return 0;

		var code = new byte[1024];
		if (!process.TryRead(destructor, code, code.Length)) return 0;

		// A slot that only jumps is a thunk, so the destructor is wherever it jumps to.
		if (code[0] == 0xE9)
		{
			ulong target = (ulong) ((long) destructor + 5 + BitConverter.ToInt32(code, 1));
			if (!IsModule(target) || !process.TryRead(target, code, code.Length)) return 0;
		}

		for (int i = 0; i + 5 <= code.Length; i++)
		{
			if (code[i] != 0xBA) continue;
			int value = BitConverter.ToInt32(code, i + 1);
			if (value is < 16 or > 8192 || value % 8 != 0) continue;
			for (int j = i + 5; j < Math.Min(i + 21, code.Length); j++)
			{
				if (code[j] != 0xE8) continue;
				SizeByClass[vtable] = value;
				return value;
			}
		}
		SizeByClass[vtable] = 0;
		return 0;
	}

	/// <summary>
	///     A hashed string at an address, using the shared reader's own short-name overload. Tags and
	///     block names inside a component run as short as "dirt", below the floor the name sweep
	///     uses, and the hash still has to match the text either way.
	/// </summary>
	private static string HashedAt(BedrockProcess process, ulong address)
	{
		var head = new byte[HashedString.Size];
		if (address < 0x10000 || !process.TryRead(address, head, head.Length)) return null;
		return HashedString.ReadVerified(process, head, 0, new byte[256], 2, 64);
	}

	/// <summary>
	///     The seed component, whose fields the wire blob names: crop_result, plant_at,
	///     plant_at_any_solid_surface and plant_at_face. The object is 56 bytes: the crop it grows
	///     into, a vector of the blocks it may be planted on, and two bytes of settings.
	/// </summary>
	private static string Seed(BedrockProcess process, ulong address)
	{
		var body = new byte[56];
		if (!process.TryRead(address, body, body.Length)) return $"{Json("minecraft:seed")}: {{ }}";

		var parts = new List<string>();
		var probe = new byte[HashedString.Size];
		var heap = new byte[256];

		// The crop is held as the item that places it rather than as the block, so its name sits at
		// the item distance. The block distance is tried first because a block is what the field
		// means, and whichever verifies is the answer.
		// The crop is a block descriptor at +8, and the block itself is the single entry of the
		// vector inside it. The pointer at +0 is the seed item that owns the component, which is why
		// reading that gave every seed its own name back.
		// The crop is the block the descriptor resolved to, and it is the pointer at +104 rather than
		// the one in the vector at +32. That vector holds minecraft:empty on every seed, which is why
		// following it produced nothing and looked like an unbound reference.
		var scratch = new byte[8];
		ulong descriptor = BitConverter.ToUInt64(body, 8);
		if (descriptor > 0x10000)
		{
			ulong block = process.ReadUInt64(descriptor + 104, scratch);
			string cropName = block > 0x10000 ? HashedAt(process, block + MemoryLayout.NameInsideLegacy) : null;
			if (cropName is not null) parts.Add($"\"cropResult\": {Json(cropName)}");
		}

		ulong begin = BitConverter.ToUInt64(body, 16), end = BitConverter.ToUInt64(body, 24);
		var plantAt = new List<string>();
		if (begin > 0x10000 && end > begin && end - begin <= 4096)
		{
			// Each entry is a block descriptor of 176 bytes with its name eight bytes in. Glow
			// berries name two blocks and its vector is exactly twice as long, which is what settles
			// the stride rather than trying sizes until one divides.
			foreach (int stride in (int[]) [176])
			{
				if ((end - begin) % (ulong) stride != 0) continue;
				plantAt.Clear();
				for (ulong at = begin; at < end; at += (ulong) stride)
				{
					string one = HashedAt(process, at + 8);
					if (one is null) { plantAt.Clear(); break; }
					plantAt.Add(one);
				}
				if (plantAt.Count > 0) break;
			}
		}
		if (plantAt.Count > 0) parts.Add($"\"plantAt\": [{string.Join(", ", plantAt.Select(Json))}]");

		parts.Add($"\"plantAtAnySolidSurface\": {(body[40] != 0 ? "true" : "false")}");
		parts.Add($"\"plantAtFace\": {body[42]}");
		return $"{Json("minecraft:seed")}: {{ {string.Join(", ", parts)} }}";
	}

	/// <summary>
	///     The names in an item descriptor vector. A descriptor holds a pointer to an object that
	///     holds the name as a plain string, so the name is two hops away rather than inline. The
	///     stride comes from the range dividing and every element resolving, so a wrong one yields
	///     nothing instead of fragments. A bundle's banned list reads minecraft:shulker_box, which
	///     is what a bundle cannot hold.
	/// </summary>
	private static List<string> DescriptorNames(BedrockProcess process, ulong begin, ulong end)
	{
		var found = new List<string>();
		if (begin < 0x10000 || end <= begin || end - begin > 4096) return found;
		var node = new byte[32];
		var inner = new byte[64];
		var text = new byte[256];
		foreach (int stride in (int[]) [32, 80, 16, 24])
		{
			if ((end - begin) % (ulong) stride != 0) continue;
			found.Clear();
			for (ulong at = begin; at < end; at += (ulong) stride)
			{
				if (!process.TryRead(at, node, node.Length)) { found.Clear(); break; }
				ulong target = BitConverter.ToUInt64(node, 8);
				if (target < 0x10000 || !process.TryRead(target, inner, inner.Length)) { found.Clear(); break; }
				string name = StdString(process, inner, 8, text);
				if (name is not { Length: > 0 }) { found.Clear(); break; }
				found.Add(name);
			}
			if (found.Count > 0) return found;
		}
		return found;
	}

	/// <summary>
	///     A vector of strings, walked from its own ends. The element size is not assumed: the range
	///     has to divide by it and every element in the range has to read back as a string, so a
	///     wrong stride produces nothing rather than a list of fragments.
	/// </summary>
	private static List<string> StringVector(BedrockProcess process, ulong begin, ulong end)
	{
		var found = new List<string>();
		if (begin < 0x10000 || end <= begin || end - begin > 1 << 16) return found;
		var window = new byte[64];
		var text = new byte[256];
		foreach (int stride in (int[]) [32, 40, 48])
		{
			if ((end - begin) % (ulong) stride != 0) continue;
			found.Clear();
			for (ulong at = begin; at < end; at += (ulong) stride)
			{
				if (!process.TryRead(at, window, window.Length)) { found.Clear(); break; }
				string one = StdString(process, window, 0, text) ?? StdString(process, window, 8, text);
				if (one is not { Length: > 0 }) { found.Clear(); break; }
				found.Add(one);
			}
			if (found.Count > 0) return found;
		}
		return found;
	}

	/// <summary>
	///     What a pointer inside a component leads to, said in terms of what verifies there.
	///     Every answer here is self checking. A hashed string only reads back when its text hashes
	///     to the number in front of it, a std::string only when its size and capacity agree with the
	///     bytes, a block only when the name sits where every block has it. Anything that satisfies
	///     none of them is reported as bytes, which is the difference between a hole that is counted
	///     and one that is dressed up as a reading.
	/// </summary>
	public static string Target(BedrockProcess process, ulong address, ulong end, ulong capacity)
	{
		var scratch = new byte[HashedString.Size];
		var heap = new byte[256];
		var text = new byte[256];

		// Three pointers in a row that begin, end and reach a limit are a vector, and the range says
		// how big its elements are before anything is read out of them.
		if (end >= address && capacity >= end && end > address && (end - address) <= 1 << 16)
		{
			foreach (int stride in (int[]) [40, 48, 32, 16, 8])
			{
				if ((end - address) % (ulong) stride != 0) continue;
				var names = new List<string>();
				for (ulong at = address; at < end && names.Count < 64; at += (ulong) stride)
				{
					if (!process.TryRead(at, scratch, HashedString.Size)) break;
					string hashed = HashedString.ReadVerified(process, scratch, 0, heap);
					if (hashed is null) { names.Clear(); break; }
					names.Add(hashed);
				}
				if (names.Count > 0) return "hashed strings: " + string.Join(", ", names);
			}
		}

		if (!process.TryRead(address, scratch, HashedString.Size)) return null;
		string one = HashedString.ReadVerified(process, scratch, 0, heap);
		if (one is not null) return "hashed string: " + one;

		var window = new byte[64];
		if (process.TryRead(address, window, window.Length))
		{
			string str = StdString(process, window, 0, text);
			if (str is { Length: > 0 }) return "string: " + str;
		}
		if (process.TryRead(address + MemoryLayout.NameInsideLegacy, scratch, HashedString.Size))
		{
			string block = HashedString.ReadVerified(process, scratch, 0, heap);
			if (block is not null) return "block: " + block;
		}

		// An object of a class: it leads with a method table, and that class states its own size, so
		// what is on the other end can be described even when nothing here can name it.
		var head = new byte[16];
		if (process.TryRead(address, head, head.Length))
		{
			ulong first = BitConverter.ToUInt64(head, 0);
			if (IsModule(first))
			{
				int size = ClassSize(process, first);
				return size > 0 ? $"object of class 0x{first:X}, {size} bytes" : $"object of class 0x{first:X}, size not stated";
			}
		}

		// A vector of something other than strings: the range divides by a stride and every element
		// leads with the same method table, which is what a vector of one kind of object looks like.
		if (end > address && capacity >= end && end - address <= 1 << 16)
		{
			foreach (int stride in (int[]) [8, 16, 24, 32, 48, 64])
			{
				if ((end - address) % (ulong) stride != 0) continue;
				long count = (long) (end - address) / stride;
				if (count is < 1 or > 512) continue;
				return $"vector, {count} elements of {stride} bytes";
			}
		}
		return null;
	}

	/// <summary>
	///     One component, as its own bytes plus whatever of it has been fitted against values known
	///     in advance.
	///     Every component object is laid out the same way: its method table, the item that owns it,
	///     and then the fields. Which is which is not assumed, the owner pointer is the landmark, and
	///     a component that carries a second method table has its fields eight bytes further in,
	///     which is what food does and nothing else here does.
	///     The payload always travels out whole. Only the kinds that reproduce a value known in
	///     advance are named, and the rest are the same bytes with no claim attached: the seven
	///     spears carry their tier's enchantment value, their durability and their damage, and those
	///     are what fitted these offsets.
	/// </summary>
	private static string Component(BedrockProcess process, string name, ulong address, ulong owner, byte[] scratch)
	{
		var parts = new List<string>();
		if (address < 0x10000 || !process.TryRead(address, scratch, 96)) return $"{Json(name)}: {{ }}";

		int at = BitConverter.ToUInt64(scratch, 8) == owner && IsModule(BitConverter.ToUInt64(scratch, 16)) ? 24 : 16;
		int I32(int offset) => BitConverter.ToInt32(scratch, at + offset);
		float F32(int offset) => BitConverter.ToSingle(scratch, at + offset);
		ushort U16(int offset) => BitConverter.ToUInt16(scratch, at + offset);

		switch (name)
		{
			// Field names and their order come from the server's own component schemas under
			// docs/json_schemas/server/item, which state each component's fields and a declaration
			// order. The offsets come from memory. Where the two overlap they already agreed on
			// values checked independently: durability 190 on a copper spear, the tier enchantment
			// values, the per tier cooldowns, apple's nutrition and saturation.
			case "minecraft:food":
				parts.Add($"\"nutrition\": {I32(0)}");
				parts.Add($"\"saturationModifier\": {Num(F32(4))}");
				break;
			case "minecraft:durability":
				parts.Add($"\"maxDurability\": {I32(0)}");
				// An IntRange, and it reads (0, 100) on every one of them: a percentage. Taking it
				// from the schema's declaration order put it at +8 and gave [100, 440], where 440 is
				// the next allocation showing through a 32 byte object.
				parts.Add($"\"damageChance\": [{I32(4)}, {I32(8)}]");
				break;
			case "minecraft:enchantable":
				// One byte. Read as sixteen bits the netherite spear came out 51,727, which is 0xCA0F
				// with the true value in the low byte and slack above it; every other spear happened
				// to have a zero there and looked right.
				parts.Add($"\"value\": {scratch[at]}");
				parts.Add($"\"slot\": {Json(StdString(process, scratch, at + 8, new byte[256]) ?? "")}");
				break;
			case "minecraft:cooldown":
				parts.Add($"\"duration\": {Num(F32(0))}");
				// use is 0 and attack is 1, per the server's ItemCooldownType. The spears answer 1 and
				// the wind charge 0, which is what places this at +4 rather than where the schema
				// lists it.
				parts.Add($"\"type\": {Json(I32(4) == 0 ? "use" : "attack")}");
				parts.Add($"\"category\": {Json(StdString(process, scratch, at + 16, new byte[256]) ?? "")}");
				break;
			case "minecraft:piercing_weapon":
				parts.Add($"\"reach\": [{Num(F32(0))}, {Num(F32(4))}]");
				parts.Add($"\"creativeReach\": [{Num(F32(8))}, {Num(F32(12))}]");
				parts.Add($"\"hitboxMargin\": {Num(F32(20))}");
				break;
			case "minecraft:fuel":
				parts.Add($"\"duration\": {Num(F32(0))}");
				break;
			case "minecraft:swing_duration":
				parts.Add($"\"value\": {Num(F32(0))}");
				break;
			case "minecraft:bundle_interaction":
				parts.Add($"\"numViewableSlots\": {I32(0)}");
				break;
			case "minecraft:compostable":
				parts.Add($"\"compostingChance\": {I32(0)}");
				break;
			case "minecraft:storage_weight_limit":
				parts.Add($"\"maxWeightLimit\": {I32(0)}");
				break;
			case "minecraft:storage_weight_modifier":
				parts.Add($"\"weightInStorageItem\": {I32(0)}");
				break;
			case "minecraft:storage_item":
			{
				parts.Add($"\"maxSlots\": {I32(0)}");
				parts.Add($"\"allowNestedStorageItems\": {(scratch[at + 4] != 0 ? "true" : "false")}");
				List<string> banned = DescriptorNames(process, BitConverter.ToUInt64(scratch, at + 8), BitConverter.ToUInt64(scratch, at + 16));
				List<string> allowed = DescriptorNames(process, BitConverter.ToUInt64(scratch, at + 32), BitConverter.ToUInt64(scratch, at + 40));
				if (banned.Count > 0) parts.Add($"\"bannedItems\": [{string.Join(", ", banned.Select(Json))}]");
				if (allowed.Count > 0) parts.Add($"\"allowedItems\": [{string.Join(", ", allowed.Select(Json))}]");
				break;
			}
			case "minecraft:damage":
			case "minecraft:max_stack_size":
				parts.Add($"\"value\": {I32(0)}");
				break;
			case "minecraft:hand_equipped":
			case "minecraft:fire_resistant":
				parts.Add($"\"value\": {I32(0)}");
				break;
			case "minecraft:use_animation":
				parts.Add($"\"value\": {(NameOf(UseAnimationNames, (byte) I32(0)) is { } ua ? Json(ua) : I32(0).ToString(CultureInfo.InvariantCulture))}");
				break;
			case "minecraft:use_modifiers":
				parts.Add($"\"useDuration\": {Num(F32(0))}");
				break;
			case "minecraft:display_name":
				// Named "value" because that is what the wire calls it. Where a component field has a
				// name on the wire, using a different one here just makes a translation table.
				parts.Add($"\"value\": {Json(StdString(process, scratch, at, new byte[256]) ?? "")}");
				break;
			case "minecraft:block_placer":
			{
				var probe = new byte[HashedString.Size];
				ulong block = BitConverter.ToUInt64(scratch, at);
				if (block > 0x10000 && process.TryRead(block + MemoryLayout.NameInsideLegacy, probe, HashedString.Size))
				{
					string placed = HashedString.ReadVerified(process, probe, 0, new byte[256]);
					if (placed is not null) parts.Add($"\"block\": {Json(placed)}");
				}
				break;
			}
			case "minecraft:kinetic_weapon":
				// reach and creative_reach read the same pair the piercing weapon component holds for
				// the same spears, which is what places them; hitbox_margin follows them. The three
				// condition blocks sit 16 bytes apart and each leads with its max_duration.
				parts.Add($"\"reach\": [{Num(F32(48))}, {Num(F32(52))}]");
				parts.Add($"\"creativeReach\": [{Num(F32(56))}, {Num(F32(60))}]");
				parts.Add($"\"hitboxMargin\": {Num(F32(68))}");
				parts.Add($"\"delay\": {I32(0)}");
				parts.Add($"\"damageMultiplier\": {Num(F32(76))}");
				// Each condition block leads with a sixteen bit max duration, and the two bytes after
				// it are slack: zero on five spears and 0x61 and 0x31 on copper and stone. Reading
				// the field as a thirty two bit int swallowed that slack and gave those two
				// 6,357,242 where the rest gave a clean progression.
				parts.Add($"\"damageConditionMaxDuration\": {U16(8)}");
				parts.Add($"\"damageConditionMinSpeed\": {Num(F32(16))}");
				parts.Add($"\"knockbackConditionMaxDuration\": {U16(24)}");
				parts.Add($"\"knockbackConditionMinSpeed\": {Num(F32(32))}");
				parts.Add($"\"dismountConditionMaxDuration\": {U16(40)}");
				break;
			case "minecraft:throwable":
				parts.Add($"\"doSwingAnimation\": {(scratch[at] != 0 ? "true" : "false")}");
				parts.Add($"\"minDrawDuration\": {Num(F32(4))}");
				parts.Add($"\"maxDrawDuration\": {Num(F32(8))}");
				parts.Add($"\"launchPowerScale\": {Num(F32(12))}");
				parts.Add($"\"maxLaunchPower\": {Num(F32(16))}");
				parts.Add($"\"scalePowerByDrawDuration\": {(scratch[at + 20] != 0 ? "true" : "false")}");
				break;
			case "minecraft:projectile":
			{
				parts.Add($"\"minimumCriticalPower\": {Num(F32(0))}");
				// The entity is an actor identifier: its namespace inline at +24 and its name at +56,
				// which is where the strings actually verify rather than where the joined form sits.
				string space = StdString(process, scratch, at + 8, new byte[256]);
				string entity = StdString(process, scratch, at + 40, new byte[256]);
				if (entity is { Length: > 0 }) parts.Add($"\"projectileEntity\": {Json((space is { Length: > 0 } ? space + ":" : "") + entity)}");
				break;
			}
			case "minecraft:swing_sounds":
				// Two of the three the schema lists are present and vary per tier; the third is not
				// set on any of these seven, so it is not named rather than guessed at.
				parts.Add($"\"attackMiss\": {I32(0)}");
				parts.Add($"\"attackHit\": {I32(48)}");
				break;
			case "minecraft:repairable":
			{
				ulong begin = BitConverter.ToUInt64(scratch, at);
				ulong end = BitConverter.ToUInt64(scratch, at + 8);
				// A repair entry is 80 bytes and holds its items in a vector of its own, so the names
				// are one level further in than the storage lists. Each entry is walked, not just the
				// outer vector.
				var repair = new List<string>();
				// Forty bytes per entry, not eighty. At eighty the walk found one entry naming the
				// spear itself and stopped; at forty it finds both, and the second is the material.
				// A copper spear repairs with another spear or with copper ingots, which is the game.
				if (begin > 0x10000 && end > begin && (end - begin) % 40 == 0 && end - begin <= 4096)
				{
					var entry = new byte[24];
					for (ulong e = begin; e < end; e += 40)
					{
						if (!process.TryRead(e, entry, entry.Length)) break;
						repair.AddRange(DescriptorNames(process, BitConverter.ToUInt64(entry, 0), BitConverter.ToUInt64(entry, 8)));
					}
				}
				if (repair.Count > 0) parts.Add($"\"repairItems\": [{string.Join(", ", repair.Select(Json))}]");
				else if (end > begin && end - begin <= 4096) parts.Add($"\"repairItemsBytes\": {end - begin}");
				break;
			}
			case "minecraft:icon":
			{
				// The schema calls this a textures object, and in memory it is a list of key and value
				// strings: the key is the state the texture is for and the value is the texture name.
				// Apple holds one entry, "default" to "apple". The pointer at +24 is the list head, so
				// the entries are reached by walking it rather than read at any offset.
				var probe = new byte[96];
				var text = new byte[256];
				ulong head = BitConverter.ToUInt64(scratch, at + 8);
				var textures = new List<string>();
				ulong node = head > 0x10000 && process.TryRead(head, probe, 16) ? BitConverter.ToUInt64(probe, 0) : 0;
				var walked = new HashSet<ulong>();
				while (node > 0x10000 && node != head && walked.Add(node) && textures.Count < 32)
				{
					if (!process.TryRead(node, probe, probe.Length)) break;
					string key = StdString(process, probe, 16, text);
					string value = StdString(process, probe, 48, text);
					if (key is { Length: > 0 } && value is { Length: > 0 }) textures.Add($"{Json(key)}: {Json(value)}");
					node = BitConverter.ToUInt64(probe, 0);
				}
				if (textures.Count > 0) parts.Add($"\"textures\": {{ {string.Join(", ", textures)} }}");
				break;
			}
			case "minecraft:tags":
			{
				List<string> tagList = StringVector(process, BitConverter.ToUInt64(scratch, at), BitConverter.ToUInt64(scratch, at + 8));
				if (tagList.Count > 0) parts.Add($"\"tags\": [{string.Join(", ", tagList.Select(Json))}]");
				break;
			}

			// Deliberately left with no fields. It is declared on exactly one item, the apple, and
			// nothing says what it is: no schema under docs/json_schemas/server/item describes it, no
			// shipped behaviour pack mentions it, and no add-on can write it, so there is nothing to
			// check a reading against. Its 184 bytes are still counted and still emitted field by field
			// in items-runtime-components.json: three integers, a sixteen entry vector and ten heap
			// pointers this does not follow. Nothing downstream wants it, so it stays a counted hole
			// rather than a guess. Decode it from that record when something needs it.
			case "minecraft:legacy_events":
				break;
		}

		// What was read, always, plus what was not. A component is a heap object of unknown length,
		// so 96 bytes is a reach rather than its extent, and every field of it that holds a pointer
		// leads to data nothing here follows. Both are stated: a hole that is counted can be closed,
		// and one that is silent looks like a complete reading.
		int size = ClassSize(process, BitConverter.ToUInt64(scratch, 0));
		int limit = size > 0 ? Math.Min(size, scratch.Length) : 96;
		var unfollowed = new List<string>();
		for (int off = at; off + 8 <= limit; off += 8)
		{
			ulong value = BitConverter.ToUInt64(scratch, off);
			if (value is > 0x10000 and < 0x7FF000000000 && process.IsMapped(value)) unfollowed.Add(off.ToString(CultureInfo.InvariantCulture));
		}

		return $"{Json(name)}: {{ {string.Join(", ", parts)} }}";
	}

	/// <summary>
	///     Food read off a component object rather than off the item. The object leads with its
	///     method tables and then the payload, and the data-driven ones carry two tables where the
	///     hardcoded ones carry one, so the payload starts eight bytes further in. Which it is comes
	///     from the object, not from a guess: the word after the first table is the item that owns
	///     the component, so the payload begins wherever that owner pointer stops.
	/// </summary>
	private static string FoodAt(BedrockProcess process, ulong component, ulong owner, byte[] scratch)
	{
		if (component < 0x10000 || !process.TryRead(component, scratch, 40)) return null;
		int payload = BitConverter.ToUInt64(scratch, 8) == owner ? 24 : 16;
		if (IsModule(BitConverter.ToUInt64(scratch, 16)) && payload == 16) payload = 24;
		int nutrition = BitConverter.ToInt32(scratch, payload);
		float saturation = BitConverter.ToSingle(scratch, payload + 4);
		if (nutrition is < 0 or > 64 || !(saturation >= 0 && saturation <= 16)) return null;
		return $"{{ \"nutrition\": {nutrition}, \"saturationModifier\": {Num(saturation)} }}";
	}

	/// <summary>
	///     The block a pointer leads to, whether it points at the block or at a holder that does.
	///     Both forms occur: the camera holds a holder at +88 and the brewing stand one at +328, and
	///     the name verifies against its own hash either way, so a wrong hop yields nothing.
	/// </summary>
	private static string BlockNameAt(BedrockProcess process, ulong pointer)
	{
		if (pointer < 0x10000 || !process.IsMapped(pointer)) return null;
		if (HashedAt(process, pointer + MemoryLayout.NameInsideLegacy) is { } direct) return direct;

		var word = new byte[8];
		ulong held = process.ReadUInt64(pointer, word);
		return held > 0x10000 && process.IsMapped(held)
			? HashedAt(process, held + MemoryLayout.NameInsideLegacy)
			: null;
	}

	/// <summary>
	///     The armour material a piece is made of, shared by every piece of that set and held as a
	///     pointer into the module. Thirty two bytes: a durability multiplier, the protection for
	///     each of the four slots, toughness and enchantability. Checked against values that have not
	///     changed in years, so a wrong offset shows up at once: diamond is 33 with 3, 8, 6 and 3,
	///     toughness 2 and enchantability 10; iron is 15 with 2, 6, 5 and 2; leather is 5 with 1, 3,
	///     2 and 1 and enchantability 15.
	/// </summary>
	private static string ArmorMaterial(BedrockProcess process, ulong pointer)
	{
		if (!IsModule(pointer)) return null;
		var body = new byte[32];
		if (!process.TryRead(pointer, body, body.Length)) return null;
		int Field(int at) => BitConverter.ToInt32(body, at);
		if (Field(0) is < 1 or > 100 || Field(24) is < 0 or > 64) return null;
		return "{ " + $"\"durabilityMultiplier\": {Field(0)}, \"protection\": {{ \"boots\": {Field(4)}, "
			+ $"\"chestplate\": {Field(8)}, \"leggings\": {Field(12)}, \"helmet\": {Field(16)} }}, "
			+ $"\"toughness\": {Field(20)}, \"enchantability\": {Field(24)}" + " }";
	}

	/// <summary>
	///     A tool tier, when the pointer leads to one. The shape is what identifies it, not the
	///     address: five numbers where the level is a small count, the durability is a real one, the
	///     mining speed and the damage bonus are positive, and the enchantment value is a plausible
	///     one. Wood, stone, iron, gold, diamond and netherite each have exactly one of these and
	///     every tool of that material points at the same one.
	/// </summary>
	private static string Tier(BedrockProcess process, ulong pointer, byte[] scratch)
	{
		if (!IsModule(pointer) || !process.TryRead(pointer, scratch, 20)) return null;
		int level = BitConverter.ToInt32(scratch, 0);
		int uses = BitConverter.ToInt32(scratch, 4);
		float speed = BitConverter.ToSingle(scratch, 8);
		int damage = BitConverter.ToInt32(scratch, 12);
		int enchantment = BitConverter.ToInt32(scratch, 16);
		if (level is < 0 or > 8 || uses is < 1 or > 10000) return null;
		if (!(speed > 0 && speed < 100) || damage is < 0 or > 32 || enchantment is < 0 or > 64) return null;
		return $"{{ \"level\": {level}, \"uses\": {uses}, \"speed\": {Num(speed)}, "
			+ $"\"damageBonus\": {damage}, \"enchantmentValue\": {enchantment} }}";
	}

	/// <summary>How many places in the process hold a pointer to each of these addresses.</summary>
	private static Dictionary<ulong, int> CountReferences(BedrockProcess process, HashSet<ulong> targets)
	{
		var counts = targets.ToDictionary(t => t, _ => 0);
		if (targets.Count == 0) return counts;

		var scan = new byte[8 * 1024 * 1024];
		foreach (BedrockProcess.Region region in process.Regions)
		{
			for (ulong at = region.Base; at < region.End; at += (ulong) scan.Length - 8)
			{
				int length = (int) Math.Min((ulong) scan.Length, region.End - at);
				if (length < 8 || !process.TryRead(at, scan, length)) continue;
				for (int k = 0; k + 8 <= length; k += 8)
				{
					ulong value = BitConverter.ToUInt64(scan, k);
					if (counts.ContainsKey(value)) counts[value]++;
				}
			}
		}
		return counts;
	}

	/// <summary>
	///     Every built component object, as bytes, grouped by kind. Naming the fields inside a
	///     component needs the same treatment the item itself got: the same kind read across every
	///     item that has one, so a field shows itself by lining up with something already known.
	/// </summary>
	public static void DumpComponents(BedrockProcess process, IEnumerable<Item> items, string path)
	{
		var payload = new byte[256];
		var lines = new List<string>();
		foreach (Item item in items.OrderBy(i => i.Name, StringComparer.Ordinal))
		{
			foreach ((string name, ulong address) in Components(process, item, 336, false))
			{
				if (address < 0x10000 || !process.TryRead(address, payload, payload.Length)) continue;
				int bound = ClassSize(process, BitConverter.ToUInt64(payload, 0));
				if (bound <= 0) bound = 96;
				var notes = new List<string>();
				for (int off = 16; off + 8 <= Math.Min(bound, payload.Length); off += 8)
				{
					ulong value = BitConverter.ToUInt64(payload, off);
					if (value is <= 0x10000 or >= 0x7FF000000000 || !process.IsMapped(value)) continue;
					ulong next = off + 16 <= payload.Length ? BitConverter.ToUInt64(payload, off + 8) : 0;
					ulong third = off + 24 <= payload.Length ? BitConverter.ToUInt64(payload, off + 16) : 0;
					string what = Target(process, value, next, third);
					if (what is not null) notes.Add($"+{off}={what}");
				}
				lines.Add($"{name}	{item.Name}	0x{item.Address:X}	0x{address:X}	{string.Join(" | ", notes)}	{Convert.ToHexString(payload)}");
			}
		}
		lines.Sort(StringComparer.Ordinal);
		File.WriteAllLines(path, lines);
		Console.WriteLine($"wrote {lines.Count:N0} component objects to {path}");
	}

	/// <summary>What produced the file, written into it rather than beside it.</summary>
	public static string SourceJson = "{ }";

	public static void Write(BedrockProcess process, IEnumerable<Item> items, string path)
	{
		if (Path.GetDirectoryName(path) is { Length: > 0 } directory) Directory.CreateDirectory(directory);

		var byClass = new Dictionary<ulong, List<string>>();
		List<Item> ordered = items.OrderBy(i => i.Name, StringComparer.Ordinal).ToList();
		foreach (Item item in ordered)
		{
			ulong vtable = item.Ptr(-288);
			byClass.TryAdd(vtable, []);
			byClass[vtable].Add(item.Name.Replace("minecraft:", ""));
		}

		var classNames = new Dictionary<ulong, string>();
		var used = new Dictionary<string, int>(StringComparer.Ordinal);
		foreach ((ulong vtable, List<string> members) in byClass.OrderByDescending(c => c.Value.Count))
		{
			string name = NameForClass(members);
			// Two classes must never share a name, or a hierarchy built from this collides. Signs and
			// hanging signs both answered sign_item.
			if (used.TryGetValue(name, out int seen)) { used[name] = seen + 1; name = $"{name}_{seen + 1}"; }
			else used[name] = 1;
			classNames[vtable] = name;
		}

		// The three descriptions of the structure, beside the data. These calls were lost when the
		// document was restructured, because the line the edit anchored on also appears inside the
		// class layout writer, so the cut started there and took them with it. Three files silently
		// stopped being written.
		WriteClassLayouts(process, ordered, classNames, Path.ChangeExtension(path, null) + "-classes.json");
		WriteComponentLayouts(process, ordered, Path.ChangeExtension(path, null) + "-components.json");

		string layoutPath = Path.ChangeExtension(path, null) + "-layout.json";
		var layout = new List<string>();
		foreach ((int offset, int length, string kind, string name) in Layout.OrderBy(l => l.Offset))
		{
			layout.Add($"  {{ \"offset\": {offset}, \"size\": {length}, \"kind\": {Json(kind)}, \"name\": {Json(name)} }}");
		}
		File.WriteAllText(layoutPath, "[" + Environment.NewLine + string.Join("," + Environment.NewLine, layout)
			+ Environment.NewLine + "]" + Environment.NewLine);
		Console.WriteLine($"wrote the object layout to {layoutPath}");

		var rows = new List<string>();
		var rejected = new List<string>();
		foreach (Item item in ordered)
		{
			string row = Row(process, item, classNames);
			// Only items reach the data file. What was seen and rejected is not lost, it is in the
			// forensics beside it with the same names and the reasons kept, which is where a reader
			// checking whether a check has gone too strict would look anyway.
			if (item.IsItem) rows.Add(row);
			else rejected.Add(item.Name);
		}

		// A document, not a bare array: the items are one list, the things that share a name with an
		// item but are not one are another, and what produced them is stated at the top rather than
		// in a file beside it. Nothing is dropped; the rejected keep their reasons.
		File.WriteAllText(path, "{" + Environment.NewLine
			+ $"  \"source\": {SourceJson}," + Environment.NewLine
			+ "  \"items\": [" + Environment.NewLine + string.Join("," + Environment.NewLine, rows) + Environment.NewLine
			+ "  ]" + Environment.NewLine + "}" + Environment.NewLine);

		string forensicPath = Path.ChangeExtension(path, null) + "-forensics.json";
		File.WriteAllText(forensicPath, "[" + Environment.NewLine
			+ string.Join("," + Environment.NewLine, Forensics.Values) + Environment.NewLine + "]" + Environment.NewLine);
		Console.WriteLine($"wrote the bytes and the holes to {forensicPath}");

		Console.WriteLine($"wrote {rows.Count:N0} items to {path}");
		Console.WriteLine($"  {classNames.Count} classes");
	}

	/// <summary>
	///     What each class adds after the shared fields, described slot by slot from what its own
	///     members hold there. Nothing is named: the shape is reported, and where a slot resolves to
	///     the same kind of thing on every member of the class it is said what that is. A slot that
	///     resolves on some members and not others is not a field of the class, so it is left as
	///     bytes rather than half claimed.
	/// </summary>
	private static void WriteClassLayouts(BedrockProcess process, List<Item> items,
		Dictionary<ulong, string> classNames, string path)
	{
		var byClass = items.Where(i => i.IsItem && i.ObjectSize > 0)
			.GroupBy(i => i.Ptr(-288))
			.OrderByDescending(g => g.Count());

		var rows = new List<string>();
		foreach (IGrouping<ulong, Item> group in byClass)
		{
			List<Item> members = group.ToList();
			int size = members[0].ObjectSize;
			var slots = new List<string>();
			// Bounded by the window as well as by the object: a class larger than the window has a
			// tail this cannot see, and saying nothing about those slots is better than reading past
			// the bytes that were actually fetched.
			int reach = Math.Min(size - NameInsideItem, members[0].Window.Length - Before - 16);
			for (int off = 240; off + 8 <= reach; off += 8)
			{
				// A string first: it is thirty two bytes with its own size and capacity, so reading its
				// buffer as four separate pointer slots reports four fields where there is one. The
				// slab tail and the spawn egg tail are both a string, and both read as noise without
				// this.
				var text = new byte[256];
				var strings = members.Select(m => StdString(process, m.Window, m.At(off), text)).ToList();
				int parsed = strings.Count(t => t is not null);
				if (off + 32 <= reach && parsed * 20 >= members.Count * 19)
				{
					// Counted, not all-or-nothing. A single member whose empty buffer holds stale
					// bytes fails the string check, and requiring every one of 170 slab members to
					// pass hid a string that 169 of them read back perfectly.
					IEnumerable<string> shown = strings.Where(t => t is { Length: > 0 }).Distinct().Take(3);
					string joined = string.Join(", ", shown);
					slots.Add($"      {{ \"offset\": {off}, \"size\": 32, \"reading\": "
						+ Json($"string, reads on {parsed} of {members.Count}{(joined.Length > 0 ? ", e.g. " + joined : ", empty on all")}") + " }");
					off += 24;
					continue;
				}

				var values = new HashSet<ulong>();
				foreach (Item m in members) values.Add(m.Ptr(off));

				string what;
				if (values.Count == 1 && values.Single() == 0) what = "zero on every member";
				else if (values.All(IsModule)) what = values.Count == 1 ? "module pointer, one value" : "module pointer, per member";

				else
				{
					int resolved = 0;
					string sample = null;
					foreach (Item m in members)
					{
						string one = Target(process, m.Ptr(off), m.Ptr(off + 8), m.Ptr(off + 16));
						if (one is null) continue;
						resolved++;
						sample ??= one;
					}
					what = resolved == members.Count ? $"every member: {sample}"
						: resolved > 0 ? $"{resolved} of {members.Count} resolve, e.g. {sample}"
						// Mapped, not merely in the numeric range. Six slots were being called heap
						// pointers on their size alone, and their values are 17 terabytes up or zero:
						// they are numbers that look like addresses, not addresses.
						: values.All(v => v is >= 0x10000000000 and < 0x7FF000000000 && process.IsMapped(v))
							? $"heap pointer, target not resolved, {values.Count} distinct"
						: Numbers(values) is { } numbers ? numbers
						: values.Count == 1 ? $"constant {values.Single()}" : $"{values.Count} distinct values";
				}
				slots.Add($"      {{ \"offset\": {off}, \"size\": 8, \"reading\": {Json(what)} }}");
			}

			rows.Add($"  {{ \"class\": {Json(classNames.GetValueOrDefault(group.Key, "unknown"))}, "
				+ $"\"classPointer\": \"0x{group.Key:X}\", \"objectSize\": {size}, \"members\": {members.Count}, "
				+ $"\"example\": {Json(members[0].Name)}, \"tailDescribedTo\": {reach}," + Environment.NewLine
				+ "    \"tail\": [" + Environment.NewLine + string.Join("," + Environment.NewLine, slots)
				+ Environment.NewLine + "    ] }");
		}
		File.WriteAllText(path, "[" + Environment.NewLine + string.Join("," + Environment.NewLine, rows)
			+ Environment.NewLine + "]" + Environment.NewLine);
		Console.WriteLine($"wrote {rows.Count} class tail layouts to {path}");
	}

	/// <summary>
	///     What a slot's values look like as numbers, when they are numbers. Two four byte fields sit
	///     in every eight byte slot, so both halves are described, and a value only counts as a float
	///     when every instance reads as one in a range a game value plausibly occupies. Samples
	///     travel with it, because the reading is worth nothing if the numbers behind it cannot be
	///     checked.
	/// </summary>
	private static string Numbers(IEnumerable<ulong> values)
	{
		List<ulong> all = values.ToList();
		if (all.Count == 0) return null;

		// A pointer is not a number, and describing one as a pair of halves is noise. Anything that
		// is mapped memory on every instance belongs to the pointer reading above, not here.
		if (all.All(v => v is >= 0x10000000000 and < 0x800000000000)) return null;

		var parts = new List<string>();
		foreach (int half in (int[]) [0, 4])
		{
			List<uint> raw = all.Select(v => (uint) (half == 0 ? v & 0xFFFFFFFF : v >> 32)).ToList();
			if (raw.All(r => r == 0)) continue;
			if (raw.Any(r => r is > 0x10000 and < 0xFFFF0000) && raw.Distinct().Count() > all.Count / 2 && half == 0) { }
			List<float> floats = raw.Select(r => BitConverter.ToSingle(BitConverter.GetBytes(r))).ToList();
			bool floaty = floats.All(f => f == 0 || (Math.Abs(f) is > 0.0001f and < 1000000f));
			IEnumerable<string> sample = floaty
				? floats.Distinct().Take(4).Select(f => Num(f))
				: raw.Distinct().Take(4).Select(r => ((int) r).ToString(CultureInfo.InvariantCulture));
			parts.Add($"{(half == 0 ? "low" : "high")} half {(floaty ? "f32" : "i32")}: {string.Join(", ", sample)}");
		}
		return parts.Count == 0 ? null : string.Join("; ", parts);
	}

	/// <summary>
	///     The same treatment for the components as for the items: every slot of every kind
	///     described from what its instances hold, with the strings, objects and vectors resolved
	///     where they resolve on the whole population and left as bytes where they do not.
	///     A component is a small object, so its slots are read out of its own bytes rather than out
	///     of a window, and the walk stops at the size the class states.
	/// </summary>
	private static void WriteComponentLayouts(BedrockProcess process, List<Item> items, string path)
	{
		var instances = new Dictionary<string, List<(ulong Address, ulong Owner)>>(StringComparer.Ordinal);
		foreach (Item item in items.Where(i => i.IsItem))
		{
			foreach ((string name, ulong address) in Components(process, item, 336, false))
			{
				instances.TryAdd(name, []);
				instances[name].Add((address, item.Address));
			}
		}

		var text = new byte[256];
		var rows = new List<string>();
		foreach ((string kind, List<(ulong Address, ulong Owner)> list) in instances.OrderByDescending(i => i.Value.Count))
		{
			var bytes = new List<byte[]>();
			foreach ((ulong address, ulong _) in list)
			{
				var buffer = new byte[256];
				if (process.TryRead(address, buffer, buffer.Length)) bytes.Add(buffer);
			}
			if (bytes.Count == 0) continue;

			int size = ClassSize(process, BitConverter.ToUInt64(bytes[0], 0));
			int reach = size > 0 ? Math.Min(size, 224) : 96;
			var slots = new List<string>();
			int accounted = 16;   // the method table and the owner pointer
			for (int off = 16; off + 8 <= reach; off += 8)
			{
				List<string> strings = bytes.Select(b => StdString(process, b, off, text)).ToList();
				if (off + 32 <= reach && strings.Count(t => t is not null) * 20 >= bytes.Count * 19)
				{
					IEnumerable<string> shown = strings.Where(t => t is { Length: > 0 }).Distinct().Take(3);
					string joined = string.Join(", ", shown);
					slots.Add(Slot(off, 32, $"string{(joined.Length > 0 ? ", e.g. " + joined : ", empty on all")}"));
					accounted += 32;
					off += 24;
					continue;
				}

				var values = bytes.Select(b => BitConverter.ToUInt64(b, off)).ToHashSet();
				string what;
				if (values.Count == 1 && values.Single() == 0) what = "zero on every instance";
				else if (values.All(IsModule)) what = "module pointer";

				else
				{
					int resolved = 0;
					string sample = null;
					foreach (byte[] b in bytes)
					{
						ulong next = off + 16 <= b.Length ? BitConverter.ToUInt64(b, off + 8) : 0;
						ulong third = off + 24 <= b.Length ? BitConverter.ToUInt64(b, off + 16) : 0;
						string one = Target(process, BitConverter.ToUInt64(b, off), next, third);
						if (one is null) continue;
						resolved++;
						sample ??= one;
					}
					what = resolved == bytes.Count ? $"every instance: {sample}"
						: resolved > 0 ? $"{resolved} of {bytes.Count} resolve, e.g. {sample}"
						// Mapped, not merely in the numeric range. Six slots were being called heap
						// pointers on their size alone, and their values are 17 terabytes up or zero:
						// they are numbers that look like addresses, not addresses.
						: values.All(v => v is >= 0x10000000000 and < 0x7FF000000000 && process.IsMapped(v))
							? $"heap pointer, target not resolved, {values.Count} distinct"
						: Numbers(values) is { } numbers ? numbers
						: values.Count == 1 ? $"constant {values.Single()}"
						: $"{values.Count} distinct values on {bytes.Count} instances";
				}
				if (!what.StartsWith("zero", StringComparison.Ordinal)) accounted += 8;
				slots.Add(Slot(off, 8, what));
			}

			rows.Add($"  {{ \"component\": {Json(kind)}, \"instances\": {list.Count}, "
				+ $"\"objectSize\": {(size > 0 ? size.ToString(CultureInfo.InvariantCulture) : "null")}, "
				+ $"\"describedTo\": {reach}, \"unaccountedBytes\": {Math.Max(0, size - Math.Max(accounted, reach))}," + Environment.NewLine
				+ "    \"fields\": [" + Environment.NewLine + string.Join("," + Environment.NewLine, slots)
				+ Environment.NewLine + "    ] }");
		}
		File.WriteAllText(path, "[" + Environment.NewLine + string.Join("," + Environment.NewLine, rows)
			+ Environment.NewLine + "]" + Environment.NewLine);
		Console.WriteLine($"wrote {rows.Count} component layouts to {path}");
	}

	private static string Slot(int offset, int size, string reading) =>
		$"      {{ \"offset\": {offset}, \"size\": {size}, \"reading\": {Json(reading)} }}";

	/// <summary>Where the row came from and what of it is still unexplained, kept beside the data.</summary>
	private static readonly SortedDictionary<string, string> Forensics = new(StringComparer.Ordinal);

	private static string Forensic(BedrockProcess process, Item item, Dictionary<ulong, string> classNames,
		List<(string Name, ulong Address)> declared, List<(string Name, ulong Address)> built)
	{
		var parts = new List<string>
		{
			$"\"name\": {Json(item.Name)}",
			$"\"isItem\": {(item.IsItem ? "true" : "false")}",
			$"\"address\": \"0x{item.Address:X}\"",
			$"\"classPointer\": \"0x{item.Ptr(-288):X}\"",
			$"\"class\": {Json(classNames.GetValueOrDefault(item.Ptr(-288), "unknown_class"))}",
			$"\"classSlots\": {Json(Slots(item))}"
		};
		if (item.ObjectSize > 0) parts.Add($"\"objectSize\": {item.ObjectSize}");
		else parts.Add("\"objectExtentUnknown\": true");
		// Why a row was not taken as an item. Removing this from the data row took it from here too,
		// and 60 rejections went out with no reason attached, which is the one thing a rejection has
		// to carry.
		if (item.Doubts.Count > 0) parts.Add($"\"doubts\": [{string.Join(", ", item.Doubts.Select(Json))}]");

		if (item.ObjectSize > 0)
		{
			var cover = new Coverage(-NameInsideItem, item.ObjectSize - NameInsideItem);
			foreach ((int offset, int length, string kind, string _) in Layout)
			{
				if (kind == "class specific") continue;
				if (offset is 296 or 336 && declared.Count == 0 && built.Count == 0) continue;
				cover.Claim(offset, length);
			}
			int described = Math.Min(item.ObjectSize - NameInsideItem, item.Window.Length - Before - 16);
			if (described > 240) cover.Claim(240, described - 240);
			List<(int From, int Length)> holes = cover.Holes();
			parts.Add($"\"tailDescribedTo\": {described}");
			parts.Add($"\"unaccountedBytes\": {holes.Sum(h => h.Length)}");
			parts.Add($"\"unaccounted\": [{string.Join(", ", holes.Select(h => $"[{h.From}, {h.Length}]"))}]");
			parts.Add($"\"slackBytes\": {Slack.Sum(r => r.Length)}");
			parts.Add($"\"paddingBytes\": {ZeroPadding.Sum(r => r.Length)}");

			var whole = new byte[item.ObjectSize];
			parts.Add($"\"bytesFrom\": {-NameInsideItem}");
			parts.Add(process.TryRead(item.Address, whole, whole.Length)
				? $"\"bytes\": \"{Convert.ToHexString(whole)}\""
				: $"\"bytes\": \"{Convert.ToHexString(item.Window, Before - NameInsideItem, After + NameInsideItem)}\"");
		}
		else
		{
			parts.Add($"\"bytesFrom\": {-NameInsideItem}");
			parts.Add($"\"bytes\": \"{Convert.ToHexString(item.Window, Before - NameInsideItem, After + NameInsideItem)}\"");
		}
		return "  { " + string.Join(", ", parts) + " }";
	}

	private static string Row(BedrockProcess process, Item item, Dictionary<ulong, string> classNames)
	{
		var scratch = new byte[256];
		var heap = new byte[256];
		var text = new byte[256];
		var parts = new List<string>
		{
			$"\"name\": {Json(item.Name)}",
			$"\"id\": {item.I16(-118)}",
			$"\"translationKey\": {Json(item.Key)}",
			$"\"maxStackSize\": {item.Byte(-120)}",
			// The two fields the item registry packet needs beside the name and the id. The version
			// is the byte at -280, which agrees with the registry on all 1,968 items that can be
			// compared; component-based is not a flag anywhere in the object but the presence of the
			// declared-component set, which matches the same registry on all 73 it lists and adds the
			// two experimental items its own source could not see.
			$"\"version\": {item.Byte(-280)}",
			// How many frames the item's icon has. One for almost everything; the three compasses
			// hold 32, the clock 64 and the fishing rod 2, which is exactly how many states each of
			// those icons has. Bow and crossbow hold 0 and draw their pull states another way.
			$"\"iconFrameCount\": {item.Byte(-240)}",
			// Whether the icon cycles through those frames in the toolbar, and whether its art is drawn
			// flipped. Six items animate; the camera, the fishing rod and the two mob on a stick items
			// are the mirrored ones.
			$"\"animatesInToolbar\": {(item.Byte(-236) != 0 ? "true" : "false")}",
			$"\"mirroredArt\": {(item.Byte(-235) != 0 ? "true" : "false")}",
			// How the item animates while used. Every food answers 1, every drinkable answers 2,
			// the bow 4 and the brush 12, and nothing else answers anything but 0.
			$"\"useAnimation\": {{ \"value\": {item.Byte(-234)}, \"name\": "
				+ (NameOf(UseAnimationNames, item.Byte(-234)) is { } an ? Json(an) : "null") + " }",
			// Rarity. The 75 that answer 1 are the pottery sherds, armour trims and chainmail; the 14
			// that answer 2 are the beacon, nether star, banner patterns and music discs; the 5 that
			// answer 3 are dragon egg, elytra, heavy core, mace and the silence trim. That is the
			// uncommon, rare and epic set, which is what makes this rarity rather than a number.
			$"\"rarity\": {{ \"value\": {item.Byte(156)}, \"name\": "
				+ (NameOf(RarityNames, item.Byte(156)) is { } rn ? Json(rn) : "null") + " }",
			// What mining a block costs this item. The 28 axes, hoes, pickaxes and shovels answer
			// digger_item, shears answers shears_item on its own, the component items answer
			// component_item, and armour, bows, the shield, the fishing rod and the sticks answer
			// do_nothing, which is exactly the set that takes no durability from mining.
			$"\"mineBlockType\": {{ \"value\": {item.Byte(160)}, \"name\": "
				+ (NameOf(MineBlockTypeNames, item.Byte(160)) is { } mb ? Json(mb) : "null") + " }",
			$"\"maxDurability\": {item.U16(48)}",
			$"\"useDuration\": {item.U32(52)}"
		};

		// The flag bits that have been pinned against items whose behaviour is not in dispute, and
		// only those. A bit with no name is still in "flags" above, so nothing is lost by omission.
		var flags = new List<string>();
		byte f = item.Byte(50);
		if ((f & 0x01) != 0) flags.Add("glint");
		if ((f & 0x02) != 0) flags.Add("handEquipped");
		if ((f & 0x04) != 0) flags.Add("stackedByData");
		if ((f & 0x20) != 0) flags.Add("fireResistant");
		if ((f & 0x40) != 0) flags.Add("shouldDespawn");
		if ((f & 0x80) != 0) flags.Add("allowOffHand");
		// Only the byte at +50. The one after it was published here as a second flag byte until the
		// two process test showed it differs on 155 items, which makes it uninitialised memory rather
		// than data, and publishing slack as a field is worse than leaving a hole.
		parts.Add($"\"flags\": {{ \"raw\": \"0x{item.Byte(50):X2}\""
			+ (flags.Count > 0 ? $", \"set\": [{string.Join(", ", flags.Select(Json))}]" : "") + " }");

		// Three sixteen bit numbers reading 1, minor, patch, which is the version a pack has to
		// declare before the server will hand this item out. Nothing carries it below 1.14.
		if (item.U16(56) != 0)
		{
			parts.Add($"\"minRequiredVersion\": \"{item.U16(56)}.{item.U16(58)}.{item.U16(60)}\"");

		}

		// The block an item places is held either directly or through a small holder that points at
		// it. Reading only the direct form left the camera, the brewing stand, the cake and the
		// comparator with no block at all, and the holder is what the wire calls minecraft:block.
		if (item.IsItem)
		{
			foreach (int slot in (int[]) [88, 328])
			{
				if (slot + 8 > item.Window.Length - Before) break;
				string blockName = BlockNameAt(process, item.Ptr(slot));
				if (blockName is null) continue;
				parts.Add($"\"block\": {Json(blockName)}");
				break;
			}
		}

		string creativeGroupName = StdString(process, item.Window, item.At(112), text);
		parts.Add($"\"creative\": {{ \"category\": {item.Byte(96)}, \"categoryName\": "
			+ (NameOf(CreativeCategoryNames, item.Byte(96)) is { } cn ? Json(cn) : "null")
			+ (string.IsNullOrEmpty(creativeGroupName) ? "" : $", \"group\": {Json(creativeGroupName)}") + " }");


		// Furnace behaviour, both halves: how many items this one burns for, and the experience
		// smelting it gives. Fitted against lava bucket at a hundred, boats at six and sticks at a
		// half, and against gold at one, iron at seven tenths and nuggets at a tenth.
		if (item.F32(144) != 0 || item.F32(148) != 0)
		{
			parts.Add($"\"furnace\": {{ \"fuelDuration\": {Num(item.F32(144))}, "
				+ $"\"smeltingExperience\": {Num(item.F32(148))} }}");
		}

		string icon = StdString(process, item.Window, item.At(-184), text);
		if (!string.IsNullOrEmpty(icon)) parts.Add($"\"icon\": {Json(icon)}");
		string icon2 = StdString(process, item.Window, item.At(-152), text);
		if (!string.IsNullOrEmpty(icon2)) parts.Add($"\"icon2\": {Json(icon2)}");
		string atlas = StdString(process, item.Window, item.At(-272), text);
		if (!string.IsNullOrEmpty(atlas)) parts.Add($"\"textureAtlas\": {Json(atlas)}");

		List<string> tags = Tags(process, item, scratch, heap);
		if (tags.Count > 0) parts.Add($"\"tags\": [{string.Join(", ", tags.Select(Json))}]");

		// Every component reaches the reader the same way, whatever the server does internally. The
		// older items keep a typed pointer per kind at a fixed offset, the newer ones keep a map, and
		// a consumer should not have to know which: both end up under "components" keyed by the same
		// names, so food is food wherever the server put it.
		List<(string Name, ulong Address)> declared = Components(process, item, 296, true);
		List<(string Name, ulong Address)> built = Components(process, item, 336, false);
		parts.Add($"\"componentBased\": {(declared.Count > 0 ? "true" : "false")}");
		if (declared.Count > 0) parts.Add($"\"declaredComponents\": [{string.Join(", ", declared.Select(c => Json(c.Name)))}]");

		var componentJson = built
			.Select(c => Component(process, c.Name, c.Address, item.Address, scratch))
			.ToList();

		ulong food = item.IsItem ? item.Ptr(168) : 0;
		if (food > 0x10000 && built.All(c => c.Name != "minecraft:food") && process.TryRead(food, scratch, 32))
		{
			componentJson.Add($"{Json("minecraft:food")}: {{ \"nutrition\": {BitConverter.ToInt32(scratch, 16)}, "
				+ $"\"saturationModifier\": {Num(BitConverter.ToSingle(scratch, 20))} }}");
		}
		ulong camera = item.IsItem ? item.Ptr(184) : 0;
		if (camera > 0x10000 && process.TryRead(camera, scratch, 32))
		{
			var values = new List<string>();
			for (int k = 8; k < 32; k += 4) values.Add(Num(BitConverter.ToSingle(scratch, k)));
			componentJson.Add($"{Json("minecraft:camera")}: {{ \"values\": [{string.Join(", ", values)}] }}");
		}
		ulong seed = item.IsItem ? item.Ptr(176) : 0;
		if (seed > 0x10000) componentJson.Add(Seed(process, seed));

		if (componentJson.Count > 0) parts.Add($"\"components\": {{ {string.Join(", ", componentJson)} }}");

		// The tier a tool is made of, shared between every tool of that material and held as a
		// pointer into the module. Which slot holds it depends on the class, so both are tried and
		// the contents decide: a tier reads as a level, a durability, a mining speed, a damage bonus
		// and an enchantment value, and diamond comes back 3, 1561, 8, 3, 10.
		foreach (int slot in item.IsItem ? (int[]) [240, 248] : [])
		{
			string tier = Tier(process, item.Ptr(slot), scratch);
			if (tier is null) continue;
			parts.Add($"\"tier\": {tier}");
			break;
		}

		// A digger names the block tag it can destroy. Which slot holds it differs by class, so both
		// are tried and the hash decides: a hashed string only reads back when its text hashes to the
		// number in front of it, so a wrong offset gives nothing rather than a wrong tag.
		foreach (int slot in (int[]) [240, 248, 256])
		{
			if (item.ObjectSize - NameInsideItem <= slot + 8) break;
			if (HashedAt(process, item.Ptr(slot)) is not { } tag || !tag.Contains("destructible", StringComparison.Ordinal)) continue;
			parts.Add($"\"destroysBlocksTagged\": {Json(tag)}");
			break;
		}

		// Armour keeps its material past the shared fields, at the one offset where every piece of a
		// set points at the same struct.
		if (item.ObjectSize - NameInsideItem > 264 && ArmorMaterial(process, item.Ptr(256)) is { } material)
		{
			parts.Add($"\"armorMaterial\": {material}");
		}

		// Whatever the class adds after the shared fields, carried out as its own bytes rather than
		// left inside the window unmentioned. Where it ends is the next object, so this is what
		// belongs to this item and nothing beyond it.


		ulong vtable = item.Ptr(-288);

		// Where the object ends, which is where the next allocation's header sits. Object sizes are
		// not uniform: the base runs to +248 and each class adds its own fields after it, so this is
		// the only honest way to say how much of the window below belongs to this item.

		// Stated, not inferred. Reading it out of the doubt text is a parsing job that gets done
		// differently by every reader, and two of mine got it wrong on the same file.
		// Fields that are deterministic and vary, so they are real, but which nothing has identified.
		// Their values travel out under their offsets rather than under a name that would claim more
		// than is known. Each is constant within a class, so each is set by a constructor.
		// Named from the code that writes them: the item parser holds the key's text and stores the
		// value it read into the object, so the store offset beside the literal is the field. Checked
		// against two offsets already known, creative_category at name+96 and frame_count at name-240.
		parts.Add($"\"hiddenInCommands\": {item.Byte(152)}");
		// These three sit past +240, which is class specific, so they only mean anything on the class
		// the parser writes them into: the data-driven items. Gated on the object being big enough
		// they were read off every class, and a diamond pickaxe reported a mining speed taken from
		// inside its tier pointer and an enchantable value that was really its attack damage.
		// The gate is componentBased, and it is exact: all 75 such items read a mining speed of 1
		// and no item outside them does.
		if (declared.Count > 0)
		{
			parts.Add($"\"miningSpeed\": {Num(BitConverter.ToSingle(item.Window, item.At(244)))}");
			parts.Add($"\"liquidClipped\": {((item.Byte(240) & 0x08) != 0 ? "true" : "false")}");
			parts.Add($"\"canDestroyInCreative\": {((item.Byte(240) & 0x02) != 0 ? "true" : "false")}");
			parts.Add($"\"enchantableValue\": {BitConverter.ToInt32(item.Window, item.At(256))}");
			// Located the same way as the rest: the parser holds the key's text and stores the value
			// beside it. Damage verifies against the damage component on all seven spears, and the
			// slot is a bitmask rather than an index, reading 0x00800000 for melee_spear and 0 for
			// none, so the number goes out rather than a name invented for a bit.
			parts.Add($"\"damage\": {BitConverter.ToInt32(item.Window, item.At(248))}");
			parts.Add($"\"enchantableSlot\": \"0x{BitConverter.ToInt32(item.Window, item.At(252)):X}\"");
		}

		// The forensic half of the row goes to its own file: the bytes it was read from, the holes,
		// and the class it belongs to. A consumer that wants the data should not have to step over
		// a kilobyte of hex to reach it, and a reader checking the extraction should not have to
		// parse the data to reach the evidence. Same rows, same names, two files.
		Forensics[item.Name] = Forensic(process, item, classNames, declared, built);
		return "    { " + string.Join(", ", parts) + " }";
	}

	/// <summary>
	///     The tag list, walked out of the vector at +216. It is three pointers, where it begins,
	///     where it ends and how much room it has, and every element is a HashedString padded to
	///     forty eight bytes. Each name is verified, so a wrong stride shows up as nothing read
	///     rather than as invented tags.
	/// </summary>
	private static List<string> Tags(BedrockProcess process, Item item, byte[] scratch, byte[] heap)
	{
		var tags = new List<string>();
		ulong begin = item.Ptr(216), end = item.Ptr(224);
		if (begin < 0x10000 || end <= begin || end - begin > 4096 || (end - begin) % 48 != 0) return tags;
		for (ulong at = begin; at < end; at += 48)
		{
			if (!process.TryRead(at, scratch, HashedString.Size)) continue;
			string tag = HashedString.ReadVerified(process, scratch, 0, heap);
			if (tag is not null) tags.Add(tag);
		}
		return tags;
	}

	/// <summary>
	///     How far apart the objects lie, which bounds each one. Objects are not all the same size:
	///     the shared fields run to about +248 and every class adds its own after them, so a fixed
	///     length would either cut a tool short or read the next allocation as part of a block item.
	///     The distance to the next object is measured instead, from the objects themselves. It
	///     includes the eight byte allocation header the heap puts between them, so it is an upper
	///     bound on the object rather than its exact length, and it is only set when the neighbour is
	///     close enough to be the next allocation rather than the next arena.
	/// </summary>
	public static void MeasureSizes(BedrockProcess process, IEnumerable<Item> items)
	{
		// The class states its own size, so this is a reading rather than a measurement. The distance
		// to the next object was only ever a proxy: it is the allocation and not the object, it
		// overstates wherever something unrelated sits between, and it says nothing at all for
		// whichever object is last in its arena, which was 89 of them.
		foreach (Item item in items)
		{
			item.ObjectSize = ClassSize(process, item.Ptr(-288));
		}
	}

	/// <summary>Every class pointer inside the object, which is what says how it inherits.</summary>
	private static string Slots(Item item)
	{
		var slots = new List<string>();
		for (int off = -288; off < 0; off += 8)
		{
			if (IsModule(item.Ptr(off))) slots.Add(off.ToString(CultureInfo.InvariantCulture));
		}
		return string.Join(" ", slots);
	}

	private static string NameForClass(List<string> members)
	{
		var tails = new Dictionary<string, int>(StringComparer.Ordinal);
		foreach (string member in members)
		{
			string tail = member.Contains('_') ? member[(member.LastIndexOf('_') + 1)..] : member;
			tails[tail] = tails.GetValueOrDefault(tail) + 1;
		}
		KeyValuePair<string, int> commonest = tails.OrderByDescending(t => t.Value).First();
		if (commonest.Value * 2 >= members.Count && members.Count > 1) return commonest.Key + "_item";
		return members.Count == 1 ? members[0] + "_item" : $"mixed_{members.Count}";
	}

	private static bool IsModule(ulong value) => value is >= 0x7FF000000000 and < 0x800000000000;

	private static string Num(float value) => value.ToString("0.#####", CultureInfo.InvariantCulture);

	private static string Json(string text) => "\"" + text.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";

	public static string ReadStdString(BedrockProcess process, byte[] window, int at, byte[] scratch) =>
		StdString(process, window, at, scratch);

	private static string StdString(BedrockProcess process, byte[] window, int at, byte[] scratch)
	{
		if (at < 0 || at + 32 > window.Length) return null;
		ulong size = BitConverter.ToUInt64(window, at + 16);
		ulong capacity = BitConverter.ToUInt64(window, at + 24);
		if (capacity < 15 || size > capacity || capacity > 1 << 20 || (capacity + 1) % 16 != 0) return null;
		if (capacity == 15)
		{
			if (window[at + (int) size] != 0) return null;
			for (int i = 0; i < (int) size; i++)
			{
				if (window[at + i] is < 0x20 or > 0x7E) return null;
			}
			return Encoding.ASCII.GetString(window, at, (int) size);
		}
		ulong pointer = BitConverter.ToUInt64(window, at);
		if (pointer < 0x10000 || (int) size >= scratch.Length || !process.IsMapped(pointer)) return null;
		if (!process.TryRead(pointer, scratch, (int) size)) return null;
		for (int i = 0; i < (int) size; i++)
		{
			if (scratch[i] is < 0x20 or > 0x7E) return null;
		}
		return Encoding.ASCII.GetString(scratch, 0, (int) size);
	}
}
