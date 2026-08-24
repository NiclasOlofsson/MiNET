using System.Text.Json.Nodes;

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
	public static int NameInsideItem { get; private set; } = 288;

	/// <summary>
	///     How far the name sits inside the object, measured rather than stated. A candidate start
	///     is a position holding a method table whose class states a size, and the object is then
	///     required to END where that size says: the allocator writes its block header there, and a
	///     pointer that merely happens to sit at the right distance does not have one behind it.
	/// </summary>
	private static void DeriveNameInsideItem(BedrockProcess process, IReadOnlyList<ulong> names)
	{
		// The population is the names that sit inside an object at all, not every verified name in
		// memory: most "minecraft:" strings are tags and block ids that no class contains, and
		// counting them makes any position look like it holds for a fraction of nothing.
		var word = new byte[8];
		var text = new byte[256];
		var window = new byte[32];
		var counts = new Dictionary<int, int>();
		int inside = 0;

		foreach (ulong name in names)
		{
			// Only the names an item carries. The translation key sits at a fixed distance from the
			// name, so it can say which of them is an item before the object start is known, and
			// without it the population is every "minecraft:" string in the process.
			string key = StdString(process, window, 0, text, name + (ulong) ItemLayout.TranslationKey);
			if (key is null) continue;
			if (!key.StartsWith("item.", StringComparison.Ordinal) && !key.StartsWith("tile.", StringComparison.Ordinal)) continue;
			inside++;
			for (int at = 8; at <= Before; at += 8)
			{
				if ((ulong) at > name) continue;
				ulong vtable = process.ReadUInt64(name - (ulong) at, word);
				if (!IsVtable(process, vtable)) continue;

				// The class has to be big enough to contain the name at that distance. A module
				// pointer further back belongs to some other object, and its class does not reach.
				int size = ClassSize(process, vtable);
				if (size < at + HashedString.Size) continue;
				counts[at] = counts.GetValueOrDefault(at) + 1;
			}
		}

		if (counts.Count == 0)
		{
			Console.WriteLine($"  no position starts an object that ends where its class says, keeping name at +{NameInsideItem}");
			return;
		}

		// The furthest position that still holds, because the object starts at its lowest address:
		// a nearer one is a base class inside the same object and also carries a method table.
		var holding = counts.Where(c => c.Value >= Sentinels.Floor * inside).ToList();
		KeyValuePair<int, int> best = holding.Count > 0
			? holding.OrderByDescending(c => c.Key).First()
			: counts.OrderByDescending(c => c.Value).First();
		Console.WriteLine("  object start candidates: "
						+ string.Join(", ", counts.OrderByDescending(c => c.Key).Take(6).Select(c => $"+{c.Key} on {c.Value:N0}")));
		if (best.Value < Sentinels.Floor * inside)
		{
			Console.WriteLine($"  the object start holds for only {best.Value:N0} of {inside:N0} items, keeping name at +{NameInsideItem}");
			return;
		}

		NameInsideItem = best.Key;
		Console.WriteLine($"  the name sits at object+{NameInsideItem}, on {best.Value:N0} of {inside:N0} items");
	}

	/// <summary>How much of the object travels out as bytes, measured from the name.</summary>
	internal const int Before = 448;
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

		/// <summary>Whether the window actually reaches that far, so a short read is not read as a value.</summary>
		public bool Covers(int offset, int bytes) => Before + offset >= 0 && Before + offset + bytes <= Window.Length;
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
			DeriveClassTail(process, items),
			DataDrivenFieldsAgree(process, items)
		];
	}

	/// <summary>
	///     Pins the data-driven field group to its derived place with values two structures state
	///     independently: the damage field must equal the minecraft:damage component the same item
	///     carries, the way the spears were fitted in the first place. Where no item carries the
	///     component there is nothing to compare, and the verdict says so instead of failing on
	///     absence.
	/// </summary>
	private static Sentinel DataDrivenFieldsAgree(BedrockProcess process, ICollection<Item> items)
	{
		if (_tail is null)
		{
			return new Sentinel {Name = "data-driven fields", Guards = "the field group at declared-56", Held = false, Detail = "no derived tail to check against"};
		}

		var scratch = new byte[96];
		int attempted = 0, agreed = 0;
		var failed = new List<string>();
		foreach (Item item in items)
		{
			foreach ((string name, ulong address) in Components(process, item, _tail.Built, false))
			{
				if (name != "minecraft:damage") continue;
				if (address < 0x10000 || !process.TryRead(address, scratch, scratch.Length)) continue;
				int at = BitConverter.ToUInt64(scratch, 8) == item.Address && IsVtable(process, BitConverter.ToUInt64(scratch, 16)) ? 24 : 16;
				attempted++;
				// The component's damage member is a declared short (see the damage decode); the
				// item's own field is a full int the parser stores.
				int component = BitConverter.ToInt16(scratch, at);
				int field = BitConverter.ToInt32(item.Window, item.At(_tail.DataDriven + 8));
				if (component == field) agreed++;
				else failed.Add($"{item.Name} (field {field}, component {component})");
			}
		}

		return new Sentinel
		{
			Name = "data-driven fields",
			Guards = "the field group at declared-56",
			Held = attempted == 0 || agreed == attempted,
			Detail = attempted == 0
				? "no item carries a damage component; nothing to compare"
				: $"{agreed:N0} of {attempted:N0} damage fields equal the damage component beside them"
				+ (failed.Count == 0 ? "" : $"; disagreeing: {string.Join(", ", failed.Take(4))}")
		};
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
			string space = StdString(process, item.Window, item.At(ItemLayout.Namespace), text);
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
		int agreed = items.Count(i => i.Byte(ItemLayout.MaxStackSize) is >= 1 and <= 64);
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
			ulong begin = item.Ptr(ItemLayout.TagVector), end = item.Ptr(ItemLayout.TagVector + 8);
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
		int distinct = items.Select(i => i.I16(ItemLayout.Id)).Distinct().Count();
		return Verdict("ids are unique", "-118", distinct, items.Count, "items hold an id no other item holds");
	}

	/// <summary>
	///     Where the class tail keeps its parts: the declared component set, the built component map,
	///     and the data-driven field group that sits 56 bytes before the declared set. Derived from
	///     the running server rather than carried as constants, because 1.26.50 grew every item class
	///     by eight bytes and the whole tail moved with it; a fixed offset survives that only by luck.
	/// </summary>
	public sealed record ClassTail(int DataDriven, int Declared, int Built);

	private static ClassTail _tail;

	/// <summary>
	///     Finds the two container slots by sweeping the class tail for the container shape, proven
	///     the same way the old fixed offsets were checked: the tree closes (head points at the root
	///     as its parent, the root points back, which random memory does not do) and walking it yields
	///     exactly the count stored beside it with every key verifying. The population decides, not
	///     one item: a slot qualifies only when every closing tree on it walks out whole. The declared
	///     set keys by HashedString and the built map by plain string, so the key kind tells the two
	///     apart, and a slot that closes without either kind walking is reported rather than skipped.
	/// </summary>
	private static Sentinel DeriveClassTail(BedrockProcess process, ICollection<Item> items)
	{
		var word = new byte[8];
		var hashedSlots = new List<(int Slot, int Count)>();
		var plainSlots = new List<(int Slot, int Count)>();
		var other = new List<string>();
		for (int slot = 240; slot + 16 <= After; slot += 8)
		{
			int closes = 0, hashed = 0, plain = 0, ints = 0;
			var failers = new List<string>();
			var intKeys = new SortedSet<long>();
			foreach (Item item in items)
			{
				// Attempted where the tree actually closes, not merely where the slot looks
				// plausible. On a class without a container the slot holds other fields.
				long size = BitConverter.ToInt64(item.Window, item.At(slot + 8));
				ulong head = item.Ptr(slot);
				if (head < 0x10000 || size is < 1 or > 512 || !process.IsMapped(head)) continue;
				ulong root = process.ReadUInt64(head + 8, word);
				if (root == 0 || root == head || !process.IsMapped(root)) continue;
				if (process.ReadUInt64(root + 8, word) != head) continue;

				closes++;
				if (Components(process, item, slot, true).Count == size) hashed++;
				else if (Components(process, item, slot, false).Count == size) plain++;
				else if (IntKeys(process, item, slot, size) is { } keys)
				{
					ints++;
					foreach (long key in keys) intKeys.Add(key);
				}
				else failers.Add(item.Name);
			}
			if (closes == 0) continue;
			if (hashed == closes) hashedSlots.Add((slot, closes));
			else if (plain == closes) plainSlots.Add((slot, closes));
			else if (ints == closes)
			{
				other.Add($"+{slot} ({closes} int-keyed containers, keys {intKeys.Min}..{intKeys.Max}, values unfollowed)");
			}
			else other.Add($"+{slot} ({closes} close, {hashed} walk hashed, {plain} walk plain, {ints} int-keyed; failing: {string.Join(", ", failers.Take(4))}{(failers.Count > 4 ? ", ..." : "")})");
		}

		// A container that closes but walks as neither key kind is a class's own structure, not a
		// component container: the sign classes keep one at the head of their tail. It is reported
		// by name so it stays a known unknown, and its bytes travel out with the class tail, but it
		// is not ambiguity. Ambiguity is two slots claiming the same key kind, and that fails.
		bool held = hashedSlots.Count == 1 && plainSlots.Count == 1;
		_tail = held ? new ClassTail(hashedSlots[0].Slot - 56, hashedSlots[0].Slot, plainSlots[0].Slot) : null;

		string detail = held
			? $"declared set at +{_tail.Declared} ({hashedSlots[0].Count} containers), built map at +{_tail.Built} ({plainSlots[0].Count}), every one walks to its stated count"
			: $"hashed-key slots [{string.Join(", ", hashedSlots.Select(s => $"+{s.Slot}"))}], plain-key slots [{string.Join(", ", plainSlots.Select(s => $"+{s.Slot}"))}]; needed exactly one of each";
		if (other.Count > 0) detail += $"; class-own tree containers: [{string.Join(", ", other)}]";
		return new Sentinel {Name = "component containers", Guards = "the class tail slots, derived", Held = held, Detail = detail};
	}

	/// <summary>
	///     The item class as the run read it, written back out, so this file is the next run's
	///     reference. Offsets are stated from the name, which is where the item side measures
	///     everything from.
	/// </summary>
	/// <summary>
	///     The class the file's objects are, written by the same writer the block files use. Its own
	///     copy of this dropped every bit index, so a reference refreshed from a run came back with
	///     nine flags all reading bit zero, and the check called them broken on the very data they
	///     had just been written from.
	/// </summary>
	private static void ItemClasses(JsonObject document)
	{
		BlockDocument.Classes(document, BlockMembers.Root(BlockMembers.Source.Items), BlockMembers.Source.Items);
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
	/// <summary>
	///     The item half of the one run, on the process the block half already attached to and
	///     checked. It takes no arguments of its own: two halves of one extraction reading two
	///     different servers, or one of them writing somewhere else, is not a thing that should be
	///     expressible.
	/// </summary>
	public static int Run(BedrockProcess process)
	{
		string output = Path.Combine(Program.DefaultOutputDirectory(), "items-runtime.json");
		WorldConfig.Config config = WorldConfig.Read(process.ExecutablePath);
		List<Item> all = Read(process);
		Console.WriteLine($"objects with a verified name and a translation key: {all.Count:N0}");
		Dictionary<string, Item> chosen = ChooseCopies(process, all);
		Console.WriteLine($"distinct items: {chosen.Count:N0}");
		MeasureSizes(process, chosen.Values);

		// Whether the layout the reference states still reads this build, before anything is
		// checked or written. A member reading something else stops the run: everything past it
		// would be nonsense that looks like data.
		if (!Program.Report("item members", BlockMemberDerivation.CheckItems(process, chosen.Values))) return 2;

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

		Source = new JsonObject
		{
			["server"] = config.ServerPath,
			["world"] = config.World,
			["experiments"] = Names(config.Experiments),
			["layoutSound"] = sound,
			["layoutChecks"] = new JsonArray(sentinels.Select(x => (JsonNode) new JsonObject
			{
				["check"] = x.Name,
				["guards"] = x.Guards,
				["held"] = x.Held,
				["detail"] = x.Detail
			}).ToArray())
		};
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
		var names = new List<(string Name, ulong At)>();

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
					names.Add((name, nameAddress));
				}
			}
		}

		// Measured before a single field is read, because every position an item states is stated
		// from the name and the object start is what they are all measured against.
		DeriveNameInsideItem(process, names.Select(n => n.At).ToList());

		foreach ((string name, ulong nameAddress) in names)
		{
			{
				{

					// The lead-in is context, not the object: everything decoded sits at -288 or
					// later. When the wider read crosses out of a mapped region the object is still
					// read, from its own start, with the gap left zero and said out loud. Requiring
					// the whole window silently lost cow_spawn_egg, pink_bundle and five others that
					// happen to be allocated near the front of their region.
					Array.Clear(window);
					string shortfall = null;
					if (!process.TryRead(nameAddress - Before, window, window.Length))
					{
						if (!process.TryRead(nameAddress - (ulong) NameInsideItem, window, Before - NameInsideItem + After)) continue;
						Array.Copy(window, 0, window, Before - NameInsideItem, After + NameInsideItem);
						Array.Clear(window, 0, Before - NameInsideItem);
						shortfall = null;
					}

					string key = StdString(process, window, Before + ItemLayout.TranslationKey, text);
					if (key is null) continue;

					var item = new Item
					{
						Name = name,
						NameAddress = nameAddress,
						Address = nameAddress - (ulong) NameInsideItem,
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
					if (item.Byte(ItemLayout.MaxStackSize) is < 1 or > 64)
					{
						item.Doubts.Add($"stack size {item.Byte(ItemLayout.MaxStackSize)}");
						item.IsItem = false;
					}
					if (!IsModule(item.Ptr(ItemLayout.MethodTable)))
					{
						item.Doubts.Add("no class pointer at the object start");
						item.IsItem = false;
					}

					// A real item is a heap object of a polymorphic class, and such a class writes its
					// own size into its destructor. Something that merely carries a name at the right
					// distance does not. This is what tells the live dirt from the eight other things
					// in memory that answer to the name.
					item.ObjectSize = ClassSize(process, item.Ptr(ItemLayout.MethodTable));

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
					+ string.Join(", ", copies.Select(c => $"{c.I16(ItemLayout.Id)} ({references.GetValueOrDefault(c.Address)} refs)"))
					+ $"; took {pick.I16(ItemLayout.Id)} at 0x{pick.Address:X}");
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

		// Exactly the bytes a node is decoded from: links and flags to 32, the key (a 40 byte
		// HashedString or a 32 byte string) and the value pointer behind it, ending at 80. Reading
		// more looked harmless and was not: a node allocated just under a page whose neighbour page
		// is uncommitted fails the whole straddling read, and WHICH node that is is heap lottery,
		// so one bundle's map walked short on one instance and clean on the next. An allocation
		// never spans into uncommitted memory, so a read that stays inside the node cannot fail
		// this way.
		var node = new byte[80];
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
		(ItemLayout.MethodTable, 8, "pointer", "method table"),
		(ItemLayout.ParseVersion, 4, "i32", "item parse version"),
		(-276, 4, "slack", "uninitialised"),
		(ItemLayout.TextureAtlas, 32, "string", "texture atlas"),
		(ItemLayout.FrameCount, 4, "i32", "frame count"),
		(ItemLayout.AnimatesInToolbar, 1, "bool", "animates in toolbar"),
		(ItemLayout.MirroredArt, 1, "bool", "mirrored art"),
		(ItemLayout.UseAnimation, 1, "u8", "use animation"),
		(-233, 1, "slack", "uninitialised"),
		(ItemLayout.HoverTextColorFormat, 32, "string", "hover text colour format"),
		(-200, 4, "slack", "icon frame, which a server never sets"),
		(-196, 4, "slack", "atlas frame, which a server never sets"),
		(-192, 4, "u32", "atlas total frames"),
		(-188, 4, "slack", "uninitialised"),
		(ItemLayout.IconName, 32, "string", "icon name"),
		(ItemLayout.AtlasName, 32, "string", "atlas name"),
		(ItemLayout.MaxStackSize, 1, "u8", "max stack size"),
		(-119, 1, "slack", "uninitialised"),
		(ItemLayout.Id, 2, "i16", "id"),
		(-116, 4, "slack", "uninitialised"),
		(ItemLayout.TranslationKey, 32, "string", "translation key"),
		(ItemLayout.BareName, 48, "hashed string", "bare name"),
		(ItemLayout.Namespace, 32, "string", "namespace"),
		(0, 48, "hashed string", "full name"),
		(ItemLayout.MaxDurability, 2, "u16", "max durability"),
		(ItemLayout.UseDuration, 4, "u32", "use duration"),
		// The 32 bytes of the minimum base game version, which is one field: a semantic version of
		// three numbers, two flags and two strings, and a flag of its own after it. Written out
		// member by member rather than as four spans named by what the bytes look like.
		(ItemLayout.MinimumVersion, 2, "u16", "minimum game version: major"),
		(ItemLayout.MinimumVersion + 2, 2, "u16", "minimum game version: minor"),
		(ItemLayout.MinimumVersion + 4, 2, "u16", "minimum game version: patch"),
		(ItemLayout.MinimumVersion + 6, 1, "bool", "minimum game version: the version parsed"),
		(ItemLayout.MinimumVersion + 7, 1, "bool", "minimum game version: any version matches"),
		(ItemLayout.MinimumVersion + 8, 8, "pointer", "minimum game version: the pre-release part, a packed pointer to text"),
		(ItemLayout.MinimumVersion + 16, 8, "pointer", "minimum game version: the build metadata part, a packed pointer to text"),
		(ItemLayout.MinimumVersion + 24, 1, "bool", "minimum game version: never compatible"),
		(ItemLayout.MinimumVersion + 25, 7, "slack", "padding to the eight the version is aligned to"),
		(ItemLayout.Block, 8, "pointer", "the block this item places"),
		(ItemLayout.CreativeCategory, 1, "u8", "creative category"),
		(97, 7, "slack", "uninitialised"),
		(ItemLayout.CraftingRemainingItem, 8, "pointer", "the item left behind when this one is crafted with, null on every vanilla item"),
		(ItemLayout.CreativeGroup, 32, "string", "creative group"),
		(ItemLayout.FurnaceFuel, 4, "f32", "furnace fuel duration"),
		(ItemLayout.SmeltingExperience, 4, "f32", "smelting experience"),
		(ItemLayout.HiddenInCommands, 1, "u8", "hidden in commands"),
		(153, 3, "slack", "uninitialised"),
		(ItemLayout.Rarity, 4, "i32", "rarity"),
		(ItemLayout.MineBlockType, 4, "i32", "mine block item effect type"),
		(164, 4, "slack", "uninitialised"),
		(ItemLayout.FoodComponent, 8, "pointer", "food component"),
		(ItemLayout.SeedComponent, 8, "pointer", "seed component"),
		(ItemLayout.CameraComponent, 8, "pointer", "camera component"),
		(ItemLayout.ResetCallbacks, 24, "vector", "the callbacks run when the item's block AI is reset"),
		(ItemLayout.TagVector, 24, "vector", "tag vector")
	];

	/// <summary>
	///     The class tail's rows, positioned from the derived slots rather than declared at fixed
	///     offsets: 1.26.50 moved the whole tail by eight bytes and the declaration has to move with
	///     it or claim bytes it is not reading. The internal shape of the data-driven field group is what
	///     stays: flags, mining speed, damage, the enchantable pair, then class-specific bytes up to
	///     the declared set, and class-specific bytes again between the two containers.
	/// </summary>
	private static IEnumerable<(int Offset, int Length, string Kind, string Name)> TailLayout()
	{
		if (_tail is null) yield break;
		yield return (_tail.DataDriven, 1, "u8", "flags on the data-driven class: can destroy in creative 0x02, liquid clipped 0x08");
		yield return (_tail.DataDriven + 1, 3, "class specific", "decoded per class where the class is known");
		yield return (_tail.DataDriven + 4, 4, "f32", "mining speed, on the data-driven class");
		yield return (_tail.DataDriven + 8, 4, "i32", "damage, on the data-driven class");
		yield return (_tail.DataDriven + 12, 4, "i32", "enchantable slot bitmask, on the data-driven class");
		yield return (_tail.DataDriven + 16, 4, "i32", "enchantable value, on the data-driven class");
		yield return (_tail.DataDriven + 20, _tail.Declared - _tail.DataDriven - 20, "class specific", "decoded per class where the class is known");
		yield return (_tail.Declared, 16, "set", "declared component names, and its count");
		yield return (_tail.Declared + 16, _tail.Built - _tail.Declared - 16, "class specific", "decoded per class where the class is known");
		yield return (_tail.Built, 16, "map", "built components, and its count");
	}

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
		if (SizeByClass.TryGetValue(vtable, out int cached)) return cached;
		List<int> sizes = ClassSizeCandidates(process, vtable);
		int size = sizes.Count == 0 ? 0 : sizes[0];
		SizeByClass[vtable] = size;
		return size;
	}

	/// <summary>
	///     Every size the deleting destructor passes to a call, in the order it passes them. A
	///     destructor that destroys members first hands their sizes to their own deletes before its
	///     own, so a class whose first one is not its own is visible here rather than answered wrong.
	/// </summary>
	public static List<int> ClassSizeCandidates(BedrockProcess process, ulong vtable)
	{
		var sizes = new List<int>();
		if (!IsModule(vtable)) return sizes;
		var scratch = new byte[8];
		ulong destructor = process.ReadUInt64(vtable, scratch);
		if (!IsModule(destructor)) return sizes;

		var code = new byte[1024];
		if (!process.TryRead(destructor, code, code.Length)) return sizes;

		// A slot that only jumps is a thunk, so the destructor is wherever it jumps to.
		if (code[0] == 0xE9)
		{
			ulong target = (ulong) ((long) destructor + 5 + BitConverter.ToInt32(code, 1));
			if (!IsModule(target) || !process.TryRead(target, code, code.Length)) return sizes;
		}

		for (int i = 0; i + 5 <= code.Length; i++)
		{
			if (code[i] != 0xBA) continue;
			int value = BitConverter.ToInt32(code, i + 1);
			if (value is < 16 or > 8192 || value % 8 != 0) continue;
			for (int j = i + 5; j < Math.Min(i + 21, code.Length); j++)
			{
				if (code[j] != 0xE8) continue;
				sizes.Add(value);
				break;
			}
		}

		return sizes;
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
		// Smallest first: a coarser stride that merely happens to divide the range can still find
		// every element it steps to valid, silently reading half as many entries as are really
		// there. Black bundle's banned list is the proof, two shulker box descriptors sixteen bytes
		// apart that a thirty two byte stride reads as one.
		foreach (int stride in (int[]) [16, 24, 32, 80])
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
	///     The block names in a vector&lt;BlockDescriptor&gt;, such as a seed's targetLandBlocks or a
	///     block placer's use_on. Unlike an ItemDescriptor, a BlockDescriptor carries its own
	///     HashedString inline eight bytes in, so there is no second hop. The element size is not
	///     assumed: wheat_seeds' single-entry vector could not tell 176 bytes apart from two 88 byte
	///     elements, and nether_wart proved it wrong, holding two entries in that same 176 bytes. The
	///     smallest stride that divides the range and verifies on every element wins, the same
	///     self-proving shape <see cref="DescriptorNames" /> and <see cref="StringVector" /> use. An
	///     empty vector, which every block placer seen so far carries, is not a failure to resolve: it
	///     returns an empty list because there is nothing to walk.
	/// </summary>
	private static List<string> BlockDescriptorNames(BedrockProcess process, ulong begin, ulong end)
	{
		var found = new List<string>();
		if (begin < 0x10000 || end < begin || end - begin > 1 << 16) return found;
		if (end == begin) return found;
		const int nameAt = 8;
		var node = new byte[HashedString.Size];
		for (int stride = 24; stride <= (int) (end - begin); stride += 8)
		{
			if ((end - begin) % (ulong) stride != 0) continue;
			found.Clear();
			bool every = true;
			for (ulong at = begin; at < end && every; at += (ulong) stride)
			{
				// The default minimum of eleven is tuned for a namespaced "minecraft:" name and
				// wrongly rejects a BlockDescriptor holding the bare form: nether_wart's target land
				// block stores "soul_sand", nine characters, no namespace, and the wire sends it bare
				// too; wheat_seeds' stores and sends the full "minecraft:farmland". The wire reflects
				// whichever form is actually stored rather than a normalised one, so the name travels
				// out exactly as read.
				string name = process.TryRead(at + nameAt, node, node.Length)
					? HashedString.ReadVerified(process, node, 0, new byte[256], 1, 96)
					: null;
				if (name is null) every = false;
				else found.Add(name);
			}
			if (every && found.Count > 0) return found;
		}
		found.Clear();
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
		if (process.TryRead(address + (ulong) MemoryLayout.NameInsideLegacy, scratch, HashedString.Size))
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
	private static JsonObject Component(BedrockProcess process, string name, ulong address, ulong owner, byte[] scratch)
	{
		var fields = new JsonObject();

		// Clipped, because the object's length is not known before reading it: a fixed 96 failed
		// whole when the allocation sat against an uncommitted page, dropping the component by
		// heap lottery. The clip still holds every byte the object really has.
		int have = address < 0x10000 ? 0 : process.ReadClipped(address, scratch, 256);
		if (have < 24) return fields;

		int at = BitConverter.ToUInt64(scratch, 8) == owner && IsVtable(process, BitConverter.ToUInt64(scratch, 16)) ? 24 : 16;
		int I32(int offset) => BitConverter.ToInt32(scratch, at + offset);
		float F32(int offset) => BitConverter.ToSingle(scratch, at + offset);
		ushort U16(int offset) => BitConverter.ToUInt16(scratch, at + offset);

		JsonArray Pair(int a, int b) => new(Real(F32(a)), Real(F32(b)));

		// How far past the field base each kind's decode reaches. Checked against what was read
		// before anything decodes: a kind whose fields do not fit in the bytes the object really
		// has is a layout that does not describe this build, said as such instead of read as
		// neighbouring garbage. Real fields always sit inside the allocation, so this fires only
		// when something is genuinely wrong.
		int extent = name switch
		{
			"minecraft:damage" or "minecraft:max_stack_size" => 2,
			"minecraft:fuel" or "minecraft:swing_duration" or "minecraft:bundle_interaction"
				or "minecraft:compostable" or "minecraft:storage_weight_limit"
				or "minecraft:storage_weight_modifier" or "minecraft:hand_equipped"
				or "minecraft:fire_resistant" or "minecraft:use_animation"
				or "minecraft:use_modifiers" => 4,
			"minecraft:food" => 8,
			"minecraft:durability" => 12,
			"minecraft:icon" or "minecraft:tags" or "minecraft:repairable" => 16,
			"minecraft:throwable" or "minecraft:piercing_weapon" => 24,
			"minecraft:display_name" => 32,
			"minecraft:enchantable" => 40,
			"minecraft:cooldown" or "minecraft:storage_item" => 48,
			"minecraft:block_placer" => 51,
			"minecraft:swing_sounds" => 52,
			"minecraft:projectile" => 136,
			"minecraft:kinetic_weapon" => 84,
			_ => 0
		};
		if (at + extent > have)
		{
			fields["clipped"] = have - at;
			return fields;
		}

		// Where the reference declares the component's class, it is read as that class. The members
		// come from the server's own header, so a field this file never named is not lost and a
		// width it got wrong is not repeated: max_stack_size is one byte there, and reading two gave
		// the black bundle a stack of 8193.
		if (BlockMembers.Component(name) is { } declared)
		{
			return BlockMemberReader.Held(process, address, scratch, declared, at, new byte[256]);
		}

		switch (name)
		{
			// Field names and their order come from the server's own component schemas under
			// docs/json_schemas/server/item, which state each component's fields and a declaration
			// order. The offsets come from memory. Where the two overlap they already agreed on
			// values checked independently: durability 190 on a copper spear, the tier enchantment
			// values, the per tier cooldowns, apple's nutrition and saturation.
			case "minecraft:food":
				fields["nutrition"] = I32(0);
				fields["saturationModifier"] = Real(F32(4));
				break;
			case "minecraft:durability":
				fields["maxDurability"] = I32(0);
				// An IntRange, and it reads (0, 100) on every one of them: a percentage. Taking it
				// from the schema's declaration order put it at +8 and gave [100, 440], where 440 is
				// the next allocation showing through a 32 byte object.
				fields["damageChance"] = new JsonArray(I32(4), I32(8));
				break;
			case "minecraft:enchantable":
				// One byte. Read as sixteen bits the netherite spear came out 51,727, which is 0xCA0F
				// with the true value in the low byte and slack above it; every other spear happened
				// to have a zero there and looked right.
				fields["value"] = (int) scratch[at];
				fields["slot"] = StdString(process, scratch, at + 8, new byte[256]) ?? "";
				break;
			case "minecraft:cooldown":
				fields["duration"] = Real(F32(0));
				// use is 0 and attack is 1, per the server's ItemCooldownType. The spears answer 1 and
				// the wind charge 0, which is what places this at +4 rather than where the schema
				// lists it.
				fields["type"] = I32(4) == 0 ? "use" : "attack";
				fields["category"] = StdString(process, scratch, at + 16, new byte[256]) ?? "";
				break;
			case "minecraft:piercing_weapon":
				fields["reach"] = Pair(0, 4);
				fields["creativeReach"] = Pair(8, 12);
				fields["hitboxMargin"] = Real(F32(20));
				break;
			case "minecraft:fuel":
				fields["duration"] = Real(F32(0));
				break;
			case "minecraft:swing_duration":
				fields["value"] = Real(F32(0));
				break;
			case "minecraft:bundle_interaction":
				fields["numViewableSlots"] = I32(0);
				break;
			case "minecraft:compostable":
				fields["compostingChance"] = I32(0);
				break;
			case "minecraft:storage_weight_limit":
				fields["maxWeightLimit"] = I32(0);
				break;
			case "minecraft:storage_weight_modifier":
				fields["weightInStorageItem"] = I32(0);
				break;
			case "minecraft:storage_item":
			{
				fields["maxSlots"] = I32(0);
				fields["allowNestedStorageItems"] = scratch[at + 4] != 0;
				List<string> banned = DescriptorNames(process, BitConverter.ToUInt64(scratch, at + 8), BitConverter.ToUInt64(scratch, at + 16));
				List<string> allowed = DescriptorNames(process, BitConverter.ToUInt64(scratch, at + 32), BitConverter.ToUInt64(scratch, at + 40));
				if (banned.Count > 0) fields["bannedItems"] = Names(banned);
				if (allowed.Count > 0) fields["allowedItems"] = Names(allowed);
				break;
			}
			// Both members are declared short in the server's own class layout
			// (SharedTypes DamageItemComponent.mDamage, MaxStackSizeItemComponent.mValue), so
			// sixteen bits is the width, not a choice. Reading thirty two published allocator
			// slack: on 1.26.3.1 the copper spear's damage read -285540350, the true 2 in the low
			// half with slack above it. The same lesson as enchantable below.
			// DamageItemComponent.mDamage is a short, so sixteen bits is its width.
			case "minecraft:damage":
				fields["value"] = (int) BitConverter.ToInt16(scratch, at);
				break;

			// MaxStackSizeItemComponent.mMaxStackSize is one byte, not two. Reading it as a short
			// swallowed the byte beside it and gave the black bundle a stack of 8193, which is
			// 0x2001: the true 1 with an earlier occupant's byte above it. Every other bundle read
			// 1 because that neighbour happened to be zero.
			case "minecraft:max_stack_size":
				fields["value"] = (int) scratch[at];
				break;
			case "minecraft:hand_equipped":
			case "minecraft:fire_resistant":
				fields["value"] = I32(0);
				break;
			case "minecraft:use_animation":
				fields["value"] = NameOf(UseAnimationNames, (byte) I32(0)) is { } ua
					? JsonValue.Create(ua)
					: JsonValue.Create(I32(0));
				break;
			case "minecraft:use_modifiers":
				fields["useDuration"] = Real(F32(0));
				break;
			case "minecraft:display_name":
				// Named "value" because that is what the wire calls it. Where a component field has a
				// name on the wire, using a different one here just makes a translation table.
				fields["value"] = StdString(process, scratch, at, new byte[256]) ?? "";
				break;
			case "minecraft:block_placer":
			{
				var probe = new byte[HashedString.Size];
				ulong block = BitConverter.ToUInt64(scratch, at);
				if (block > 0x10000 && process.TryRead(block + (ulong) MemoryLayout.NameInsideLegacy, probe, HashedString.Size))
				{
					string placed = HashedString.ReadVerified(process, probe, 0, new byte[256]);
					if (placed is not null) fields["block"] = placed;
				}

				// use_on is a vector<BlockDescriptor> right after the block pointer; empty on both
				// items observed (red_shrub, shelf_mushroom), which is what a zero begin/end/capacity
				// vector reads as. The same 176-byte, name-at-+8 element shape as the seed component's
				// targetLandBlocks resolves it when it is not empty.
				fields["useOn"] = Names(BlockDescriptorNames(process, BitConverter.ToUInt64(scratch, at + 8), BitConverter.ToUInt64(scratch, at + 16)));

				// canUseBlockAsIcon and replaceBlockItem read as a matching pair of bytes right after
				// the (empty, on these two items) use_on vector, both true on red_shrub and
				// shelf_mushroom, which is what the wire states. alignedPlacement is false on both
				// known items too, and nothing distinguishes its exact byte from the padding beside it
				// by value alone; +50 is where it sits structurally, immediately after the confirmed
				// pair, and it has not been proven against a spear or item where it reads true.
				fields["canUseBlockAsIcon"] = scratch[at + 48] != 0;
				fields["replaceBlockItem"] = scratch[at + 49] != 0;
				fields["alignedPlacement"] = scratch[at + 50] != 0;
				break;
			}
			case "minecraft:kinetic_weapon":
				// Every offset below was found by correlation against the seven spears' known wire
				// values (protocol 2192, MiNET.BlockGen/Captures/item_registry-1.26.50.26.bin), not by
				// the header: KineticDamageSettings's own fields are all anonymous Unk storage. Each of
				// the three condition structs is sixteen bytes: min_speed, then min_relative_speed,
				// then a sixteen bit max_duration, then four bytes of slack this does not read. delay
				// is a short, not the int the old reading here took it for.
				fields["reach"] = Pair(48, 52);
				fields["creativeReach"] = Pair(56, 60);
				fields["hitboxMargin"] = Real(F32(68));
				// damage_modifier reads zero on all seven spears, same as damage_multiplier's own low
				// half; its offset is inferred from sitting directly before the multiplier it is named
				// beside, not independently confirmed by a nonzero value.
				fields["damageModifier"] = Real(F32(72));
				fields["damageMultiplier"] = Real(F32(76));
				fields["delay"] = (int) U16(80);
				fields["damageConditionMinSpeed"] = Real(F32(0));
				fields["damageConditionMinRelativeSpeed"] = Real(F32(4));
				fields["damageConditionMaxDuration"] = (int) U16(8);
				fields["knockbackConditionMinSpeed"] = Real(F32(16));
				fields["knockbackConditionMinRelativeSpeed"] = Real(F32(20));
				fields["knockbackConditionMaxDuration"] = (int) U16(24);
				fields["dismountConditionMinSpeed"] = Real(F32(32));
				fields["dismountConditionMinRelativeSpeed"] = Real(F32(36));
				fields["dismountConditionMaxDuration"] = (int) U16(40);
				break;
			case "minecraft:throwable":
				fields["doSwingAnimation"] = scratch[at] != 0;
				fields["minDrawDuration"] = Real(F32(4));
				fields["maxDrawDuration"] = Real(F32(8));
				fields["launchPowerScale"] = Real(F32(12));
				fields["maxLaunchPower"] = Real(F32(16));
				fields["scalePowerByDrawDuration"] = scratch[at + 20] != 0;
				break;
			case "minecraft:projectile":
			{
				fields["minimumCriticalPower"] = Real(F32(0));
				// The entity is an ActorDefinitionIdentifier: mNamespace inline at +8, mIdentifier at
				// +40, mInitEvent at +72, mFullName at +104, which is where the strings actually
				// verify rather than where the header's own field order would put them. mFullName is
				// what the wire sends: wind_charge's mIdentifier alone reads "wind_charge_projectile",
				// missing the "<>" empty-additional-identifier suffix the wire's projectile_entity
				// carries, and mFullName carries it whole.
				string full = StdString(process, scratch, at + 104, new byte[256]);
				if (full is { Length: > 0 })
				{
					fields["projectileEntity"] = full;
					break;
				}
				string space = StdString(process, scratch, at + 8, new byte[256]);
				string entity = StdString(process, scratch, at + 40, new byte[256]);
				if (entity is { Length: > 0 })
				{
					fields["projectileEntity"] = (space is { Length: > 0 } ? space + ":" : "") + entity;
				}
				break;
			}
			case "minecraft:swing_sounds":
				// Two of the three the schema lists are present and vary per tier; the third is not
				// set on any of these seven, so it is not named rather than guessed at.
				fields["attackMiss"] = I32(0);
				fields["attackHit"] = I32(48);
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

				if (repair.Count > 0) fields["repairItems"] = Names(repair);
				else if (end > begin && end - begin <= 4096) fields["repairItemsBytes"] = end - begin;
				break;
			}
			case "minecraft:icon":
			{
				// The schema calls this a textures object, and in memory it is a list of key and value
				// strings: the key is the state the texture is for and the value is the texture name.
				// Apple holds one entry, "default" to "apple". The pointer at +24 is the list head, so
				// the entries are reached by walking it rather than read at any offset. The node read
				// is exactly what a node decodes to: links, then two strings ending at 80.
				var probe = new byte[80];
				var text = new byte[256];
				ulong head = BitConverter.ToUInt64(scratch, at + 8);
				var textures = new JsonObject();
				ulong node = head > 0x10000 && process.TryRead(head, probe, 16) ? BitConverter.ToUInt64(probe, 0) : 0;
				var walked = new HashSet<ulong>();
				while (node > 0x10000 && node != head && walked.Add(node) && textures.Count < 32)
				{
					if (!process.TryRead(node, probe, probe.Length)) break;
					string key = StdString(process, probe, 16, text);
					string value = StdString(process, probe, 48, text);
					if (key is { Length: > 0 } && value is { Length: > 0 }) textures[key] = value;
					node = BitConverter.ToUInt64(probe, 0);
				}

				if (textures.Count > 0) fields["textures"] = textures;
				break;
			}
			case "minecraft:tags":
			{
				List<string> tagList = StringVector(process, BitConverter.ToUInt64(scratch, at), BitConverter.ToUInt64(scratch, at + 8));
				if (tagList.Count > 0) fields["tags"] = Names(tagList);
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

		return fields;
	}

	/// <summary>
	///     What effect id names what effect, proven across the whole run rather than within one
	///     item: the same id has to name the same effect everywhere it is seen, so a stride that
	///     merely divides evenly on one short vector still fails the moment a second item disagrees
	///     with the first.
	/// </summary>
	private static readonly Dictionary<int, string> _effectNames = new();

	/// <summary>
	///     The legacy food component's mEffects, a vector of FoodItemComponentLegacy::Effect. The
	///     stride is the struct's own alignment sum: id (int, 4) pads to the string's 8 byte
	///     alignment, two 32 byte std::strings, then duration, amplifier and chance packed at 4
	///     each, 84 bytes rounded up to the struct's own 8 byte alignment, 88. Trusted only once
	///     every element in the vector resolves: both strings have to read back as real
	///     std::strings, and the id has to name the same effect everywhere this run has seen it.
	/// </summary>
	private static JsonArray LegacyFoodEffects(BedrockProcess process, byte[] food, byte[] scratch)
	{
		const int stride = 88;
		ulong begin = BitConverter.ToUInt64(food, 128);
		ulong end = BitConverter.ToUInt64(food, 136);
		ulong capacity = BitConverter.ToUInt64(food, 144);
		if (begin == end && capacity >= end) return [];
		if (end <= begin || capacity < end || (end - begin) % stride != 0) return null;

		long count = (long) (end - begin) / stride;
		if (count is < 1 or > 64) return null;

		var elements = new byte[end - begin];
		if (!process.TryRead(begin, elements, elements.Length)) return null;

		var list = new JsonArray();
		for (int e = 0; e + stride <= elements.Length; e += stride)
		{
			int id = BitConverter.ToInt32(elements, e);
			string name = StdString(process, elements, e + 8, scratch);
			string descriptionId = StdString(process, elements, e + 40, scratch);
			if (name is null || descriptionId is null) return null;
			if (_effectNames.TryGetValue(id, out string known)) { if (known != name) return null; }
			else _effectNames[id] = name;

			list.Add(new JsonObject
			{
				["id"] = id,
				["name"] = name,
				["descriptionId"] = descriptionId,
				["duration"] = BitConverter.ToInt32(elements, e + 72),
				["amplifier"] = BitConverter.ToInt32(elements, e + 76),
				["chance"] = Real(BitConverter.ToSingle(elements, e + 80))
			});
		}

		return list;
	}

	/// <summary>
	///     The legacy food component's mRemoveEffects, a plain vector of uint effect ids: no struct
	///     behind it to name a stride, so the well-formed vector shape (begin, end and capacity in
	///     order, four byte stride) is the only proof there is.
	/// </summary>
	private static JsonArray LegacyFoodRemoveEffects(BedrockProcess process, byte[] food)
	{
		ulong begin = BitConverter.ToUInt64(food, 152);
		ulong end = BitConverter.ToUInt64(food, 160);
		ulong capacity = BitConverter.ToUInt64(food, 168);
		if (begin == end && capacity >= end) return [];
		if (end <= begin || capacity < end || (end - begin) % 4 != 0) return null;

		long count = (long) (end - begin) / 4;
		if (count is < 1 or > 64) return null;

		var elements = new byte[end - begin];
		if (!process.TryRead(begin, elements, elements.Length)) return null;

		var list = new JsonArray();
		for (int e = 0; e + 4 <= elements.Length; e += 4) list.Add((long) BitConverter.ToUInt32(elements, e));
		return list;
	}

	/// <summary>
	///     A float as the number it is, rounded where the game states it and never turned into text.
	///     A value that is not finite is null rather than the word NaN, which is not JSON at all.
	/// </summary>
	private static JsonNode Real(float value)
	{
		return float.IsFinite(value) ? JsonValue.Create(Math.Round((double) value, 5)) : null;
	}

	/// <summary>A list of names as the list it is.</summary>
	private static JsonArray Names(IEnumerable<string> names)
	{
		return new JsonArray(names.Select(n => (JsonNode) n).ToArray());
	}

	/// <summary>
	///     The block a pointer leads to, whether it points at the block or at a holder that does.
	///     Both forms occur: the camera holds a holder at +88 and the brewing stand one at +328, and
	///     the name verifies against its own hash either way, so a wrong hop yields nothing.
	/// </summary>
	private static string BlockNameAt(BedrockProcess process, ulong pointer)
	{
		if (pointer < 0x10000 || !process.IsMapped(pointer)) return null;
		if (HashedAt(process, pointer + (ulong) MemoryLayout.NameInsideLegacy) is { } direct) return direct;

		var word = new byte[8];
		ulong held = process.ReadUInt64(pointer, word);
		return held > 0x10000 && process.IsMapped(held)
			? HashedAt(process, held + (ulong) MemoryLayout.NameInsideLegacy)
			: null;
	}

	/// <summary>
	///     The block a pointer to a STATE leads to, such as the seed component's crop result: a Block
	///     (one per palette entry) rather than a BlockLegacy (one per block name), so its name is not
	///     inline the way <see cref="BlockNameAt" /> reads. A state carries a pointer back to its
	///     BlockLegacy at its own state+<see cref="MemoryLayout.BlockLegacyPointer" />, which is the
	///     same back pointer <see cref="BlockPalette" /> follows, and the BlockLegacy's own name sits
	///     at its usual place from there. Verified against wheat_seeds (minecraft:wheat) and
	///     nether_wart (minecraft:nether_wart).
	/// </summary>
	private static string BlockNameAtState(BedrockProcess process, ulong statePointer)
	{
		if (statePointer < 0x10000 || !process.IsMapped(statePointer)) return null;
		var word = new byte[8];
		ulong legacy = process.ReadUInt64(statePointer + (ulong) MemoryLayout.BlockLegacyPointer, word);
		return legacy > 0x10000 && process.IsMapped(legacy)
			? HashedAt(process, legacy + (ulong) MemoryLayout.NameInsideLegacy)
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
	private static JsonObject ArmorMaterial(BedrockProcess process, ulong pointer, int maxDurability)
	{
		if (!IsModule(pointer)) return null;
		var body = new byte[32];
		if (!process.TryRead(pointer, body, body.Length)) return null;
		int Field(int at) => BitConverter.ToInt32(body, at);
		if (Field(0) is < 1 or > 100 || Field(24) is < 0 or > 64) return null;
		// The struct must reproduce the piece's own durability: the multiplier divides it down to
		// the game's per-slot base of 11, 13, 15 or 16. This is what makes a slot sweep safe: a
		// stray module pointer whose target happens to read plausibly still cannot know this item's
		// durability.
		if (maxDurability % Field(0) != 0 || maxDurability / Field(0) is not (11 or 13 or 15 or 16)) return null;

		return new JsonObject
		{
			["durabilityMultiplier"] = Field(0),
			["protection"] = new JsonObject
			{
				["boots"] = Field(4),
				["chestplate"] = Field(8),
				["leggings"] = Field(12),
				["helmet"] = Field(16)
			},
			["toughness"] = Field(20),
			["enchantability"] = Field(24)
		};
	}

	/// <summary>
	///     A tool tier, when the pointer leads to one. The shape is what identifies it, not the
	///     address: five numbers where the level is a small count, the durability is a real one, the
	///     mining speed and the damage bonus are positive, and the enchantment value is a plausible
	///     one. Wood, stone, iron, gold, diamond and netherite each have exactly one of these and
	///     every tool of that material points at the same one.
	/// </summary>
	private static JsonObject Tier(BedrockProcess process, ulong pointer, byte[] scratch, int? mustMatchUses)
	{
		if (!IsModule(pointer) || !process.TryRead(pointer, scratch, 20)) return null;
		int level = BitConverter.ToInt32(scratch, 0);
		int uses = BitConverter.ToInt32(scratch, 4);
		float speed = BitConverter.ToSingle(scratch, 8);
		int damage = BitConverter.ToInt32(scratch, 12);
		int enchantment = BitConverter.ToInt32(scratch, 16);
		// The tier's durability must be the item's own. This is what makes a slot sweep safe: a
		// stray module pointer whose target happens to read plausibly cannot know this item's max
		// durability. Null skips the check, for an item adopting a tier other tools already proved
		// (the mace points at the diamond tier while carrying its own 500 durability).
		if (mustMatchUses is not null && uses != mustMatchUses) return null;
		if (level is < 0 or > 8 || uses is < 1 or > 10000) return null;
		if (!(speed > 0 && speed < 100) || damage is < 0 or > 32 || enchantment is < 0 or > 64) return null;
		return new JsonObject
		{
			["level"] = level,
			["uses"] = uses,
			["speed"] = Real(speed),
			["damageBonus"] = damage,
			["enchantmentValue"] = enchantment
		};
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
			foreach ((string name, ulong address) in _tail is null ? [] : Components(process, item, _tail.Built, false))
			{
				int have = address < 0x10000 ? 0 : process.ReadClipped(address, payload, payload.Length);
				if (have < 16) continue;
				int bound = ClassSize(process, BitConverter.ToUInt64(payload, 0));
				if (bound <= 0) bound = 96;
				bound = Math.Min(bound, have);
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
	public static JsonObject Source = new();

	/// <summary>
	///     The keys of a tree container keyed by integers rather than strings, or null when it does
	///     not walk out that way. The sign classes keep one at the head of their tail (twelve
	///     entries keyed 0 through 11). The proof is the same shape as the component walk: exactly
	///     the stored count of nodes, and every key distinct and small, which arbitrary bytes
	///     surviving the closure test do not produce.
	/// </summary>
	private static List<long> IntKeys(BedrockProcess process, Item item, int slot, long size)
	{
		ulong head = item.Ptr(slot);
		var word = new byte[8];
		ulong root = process.ReadUInt64(head + 8, word);
		var node = new byte[48];
		var keys = new List<long>();
		var visited = new HashSet<ulong>();
		var stack = new Stack<ulong>();
		stack.Push(root);
		while (stack.Count > 0 && keys.Count <= size)
		{
			ulong at = stack.Pop();
			if (at == head || at == 0 || !visited.Add(at) || !process.IsMapped(at)) continue;
			if (!process.TryRead(at, node, node.Length)) continue;
			if (node[25] == 0)
			{
				long key = BitConverter.ToInt64(node, 32);
				if (key is < -65536 or > 65536) return null;
				keys.Add(key);
			}
			stack.Push(BitConverter.ToUInt64(node, 0));
			stack.Push(BitConverter.ToUInt64(node, 16));
		}

		if (keys.Count != size || keys.Distinct().Count() != keys.Count) return null;
		keys.Sort();
		return keys;
	}

	/// <summary>How far into the class tail an item's own slots reach, bounded by its window.</summary>
	private static int TailEnd(Item item) =>
		Math.Min(After - 8, item.ObjectSize > 0 ? item.ObjectSize - NameInsideItem - 8 : After - 8);

	/// <summary>
	///     The tier structs, proven before any row is written: a tool whose max durability equals the
	///     tier's uses anchors that struct, and every material's struct is anchored by several tools.
	///     An item pointing at an anchored struct without the matching durability is adopting it,
	///     which is real data rather than a coincidence an address can produce.
	/// </summary>
	private static readonly HashSet<ulong> _tierStructs = [];

	private static void CollectTierStructs(BedrockProcess process, IEnumerable<Item> items)
	{
		_tierStructs.Clear();
		var scratch = new byte[20];
		foreach (Item item in items)
		{
			if (!item.IsItem || item.U16(ItemLayout.MaxDurability) == 0) continue;
			for (int slot = 240; slot <= TailEnd(item); slot += 8)
			{
				if (Tier(process, item.Ptr(slot), scratch, item.U16(ItemLayout.MaxDurability)) is not null) _tierStructs.Add(item.Ptr(slot));
			}
		}
	}

	public static void Write(BedrockProcess process, IEnumerable<Item> items, string path)
	{
		if (Path.GetDirectoryName(path) is { Length: > 0 } directory) Directory.CreateDirectory(directory);

		var byClass = new Dictionary<ulong, List<string>>();
		List<Item> ordered = items.OrderBy(i => i.Name, StringComparer.Ordinal).ToList();
		CollectTierStructs(process, ordered);
		_effectNames.Clear();
		foreach (Item item in ordered)
		{
			ulong vtable = item.Ptr(ItemLayout.MethodTable);
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
		var layout = new JsonArray();
		foreach ((int offset, int length, string kind, string name) in Layout.Concat(TailLayout()).OrderBy(l => l.Offset))
		{
			layout.Add(new JsonObject
			{
				["offset"] = offset,
				["size"] = length,
				["kind"] = kind,
				["name"] = name
			});
		}

		File.WriteAllText(layoutPath, BlockDocument.Serialize(layout));
		Console.WriteLine($"wrote the object layout to {layoutPath}");

		var rows = new JsonArray();
		var rejected = new List<string>();
		foreach (Item item in ordered)
		{
			JsonObject row = Row(process, item, classNames);
			// Only items reach the data file. What was seen and rejected is not lost, it is in the
			// forensics beside it with the same names and the reasons kept, which is where a reader
			// checking whether a check has gone too strict would look anyway.
			if (item.IsItem) rows.Add(row);
			else rejected.Add(item.Name);
		}

		// A document, not a bare array: the items are one list, the things that share a name with an
		// item but are not one are another, and what produced them is stated at the top rather than
		// in a file beside it. Nothing is dropped; the rejected keep their reasons.
		var document = new JsonObject { ["source"] = Source };
		ItemClasses(document);
		document["items"] = rows;
		File.WriteAllText(path, BlockDocument.Serialize(document));

		string forensicPath = Path.ChangeExtension(path, null) + "-forensics.json";
		File.WriteAllText(forensicPath, BlockDocument.Serialize(new JsonArray(Forensics.Values.ToArray<JsonNode>())));
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
			.GroupBy(i => i.Ptr(ItemLayout.MethodTable))
			.OrderByDescending(g => g.Count());

		var rows = new JsonArray();
		foreach (IGrouping<ulong, Item> group in byClass)
		{
			List<Item> members = group.ToList();
			int size = members[0].ObjectSize;
			var slots = new JsonArray();
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
					slots.Add(Slot(off, 32,
						$"string, reads on {parsed} of {members.Count}{(joined.Length > 0 ? ", e.g. " + joined : ", empty on all")}"));
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
				slots.Add(Slot(off, 8, what));
			}

			rows.Add(new JsonObject
			{
				["class"] = classNames.GetValueOrDefault(group.Key, "unknown"),
				["classPointer"] = $"0x{group.Key:X}",
				["objectSize"] = size,
				["members"] = members.Count,
				["example"] = members[0].Name,
				["tailDescribedTo"] = reach,
				["tail"] = slots
			});
		}

		File.WriteAllText(path, BlockDocument.Serialize(rows));
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
			foreach ((string name, ulong address) in _tail is null ? [] : Components(process, item, _tail.Built, false))
			{
				instances.TryAdd(name, []);
				instances[name].Add((address, item.Address));
			}
		}

		var text = new byte[256];
		var rows = new JsonArray();
		foreach ((string kind, List<(ulong Address, ulong Owner)> list) in instances.OrderByDescending(i => i.Value.Count))
		{
			// Clipped reads, kept when the bytes actually read cover the decode reach. A fixed-size
			// read dropped any instance allocated within 256 bytes of an uncommitted page, by heap
			// lottery; the clip keeps them without ever describing bytes that were not read.
			var candidates = new List<(byte[] Bytes, int Have)>();
			foreach ((ulong address, ulong _) in list)
			{
				var buffer = new byte[256];
				int have = address < 0x10000 ? 0 : process.ReadClipped(address, buffer, buffer.Length);
				if (have >= 16) candidates.Add((buffer, have));
			}
			if (candidates.Count == 0) continue;

			int size = ClassSize(process, BitConverter.ToUInt64(candidates[0].Bytes, 0));
			int reach = size > 0 ? Math.Min(size, 224) : 96;
			List<byte[]> bytes = candidates.Where(c => c.Have >= reach).Select(c => c.Bytes).ToList();
			if (bytes.Count == 0) continue;
			var slots = new JsonArray();
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

			rows.Add(new JsonObject
			{
				["component"] = kind,

				// Where the first instance was read. Only good while that server lives, but it is
				// what lets a follow up probe go straight to the object instead of finding it again.
				["address"] = $"0x{list[0].Address:X}",
				["instances"] = list.Count,
				["objectSize"] = size > 0 ? size : null,
				["describedTo"] = reach,
				["unaccountedBytes"] = Math.Max(0, size - Math.Max(accounted, reach)),
				["fields"] = slots
			});
		}

		File.WriteAllText(path, BlockDocument.Serialize(rows));
		Console.WriteLine($"wrote {rows.Count} component layouts to {path}");
	}

	private static JsonObject Slot(int offset, int size, string reading) =>
		new() { ["offset"] = offset, ["size"] = size, ["reading"] = reading };

	/// <summary>Where the row came from and what of it is still unexplained, kept beside the data.</summary>
	private static readonly SortedDictionary<string, JsonObject> Forensics = new(StringComparer.Ordinal);

	private static JsonObject Forensic(BedrockProcess process, Item item, Dictionary<ulong, string> classNames,
		List<(string Name, ulong Address)> declared, List<(string Name, ulong Address)> built)
	{
		var row = new JsonObject
		{
			["name"] = item.Name,
			["isItem"] = item.IsItem,
			["address"] = $"0x{item.Address:X}",
			["classPointer"] = $"0x{item.Ptr(ItemLayout.MethodTable):X}",
			["class"] = classNames.GetValueOrDefault(item.Ptr(ItemLayout.MethodTable), "unknown_class"),
			["classSlots"] = Slots(item)
		};
		if (item.ObjectSize > 0) row["objectSize"] = item.ObjectSize;
		else row["objectExtentUnknown"] = true;
		// Why a row was not taken as an item. Removing this from the data row took it from here too,
		// and 60 rejections went out with no reason attached, which is the one thing a rejection has
		// to carry.
		if (item.Doubts.Count > 0) row["doubts"] = Names(item.Doubts);

		if (item.ObjectSize > 0)
		{
			var cover = new Coverage(-NameInsideItem, item.ObjectSize - NameInsideItem);
			foreach ((int offset, int length, string kind, string _) in Layout.Concat(TailLayout()))
			{
				if (kind == "class specific") continue;
				if (_tail is not null && (offset == _tail.Declared || offset == _tail.Built) && declared.Count == 0 && built.Count == 0) continue;
				cover.Claim(offset, length);
			}
			int described = Math.Min(item.ObjectSize - NameInsideItem, item.Window.Length - Before - 16);
			if (described > 240) cover.Claim(240, described - 240);
			List<(int From, int Length)> holes = cover.Holes();
			row["tailDescribedTo"] = described;
			row["unaccountedBytes"] = holes.Sum(h => h.Length);
			row["unaccounted"] = new JsonArray(holes
				.Select(h => (JsonNode) new JsonArray(h.From, h.Length)).ToArray());
			row["slackBytes"] = Slack.Sum(r => r.Length);
			row["paddingBytes"] = ZeroPadding.Sum(r => r.Length);

			var whole = new byte[item.ObjectSize];
			row["bytesFrom"] = -NameInsideItem;
			row["bytes"] = process.TryRead(item.Address, whole, whole.Length)
				? Convert.ToHexString(whole)
				: Convert.ToHexString(item.Window, Before - NameInsideItem, After + NameInsideItem);
		}
		else
		{
			row["bytesFrom"] = -NameInsideItem;
			row["bytes"] = Convert.ToHexString(item.Window, Before - NameInsideItem, After + NameInsideItem);
		}

		return row;
	}

	private static JsonObject Row(BedrockProcess process, Item item, Dictionary<ulong, string> classNames)
	{
		var scratch = new byte[256];
		var heap = new byte[256];
		var text = new byte[256];

		// Every member of the class, under the class's own name, read from where the reference says
		// it sits. A member holding something with no class of its own comes out null, so what
		// nothing reads is counted rather than absent.
		var row = new JsonObject();
		foreach (MemberNode node in ItemLayout.Items.Roots)
		{
			// The class states its gaps once, with offsets and lengths. Repeating the same zero
			// padding on every one of 1,933 items adds nothing.
			if (node.Member.Kind is MemberKind.Unknown or MemberKind.Padding) continue;

			int at = item.At(node.At - NameInsideItem);
			row[node.Member.Name] = at >= 0 && at + node.Member.Bytes <= item.Window.Length
				? BlockMemberReader.Node(process, item.Address, item.Window, scratch, at, node.Member)
				: null;
		}

		// The seed component's declared layout reads "result" as a bare pointer and
		// "targetLandBlocks" as an unresolved vector, because neither is a class this reader has: the
		// crop is a live Block (a palette state), not the BlockLegacy the generic Component reader
		// resolves, and a target land block is a BlockDescriptor holding its own name inline rather
		// than a two-hop ItemDescriptor. Both are fixed up here, over the object the generic pass
		// already read the pointer and the vector bounds out of.
		if (row["seedComponent"] is JsonObject seedComponent)
		{
			ulong seedAddress = item.Ptr(ItemLayout.SeedComponent);
			if (seedAddress >= 0x10000 && process.TryRead(seedAddress, scratch, 32))
			{
				string cropResult = BlockNameAtState(process, BitConverter.ToUInt64(scratch, 8));
				if (cropResult is not null) seedComponent["result"] = cropResult;

				List<string> plantAt = BlockDescriptorNames(process, BitConverter.ToUInt64(scratch, 16), BitConverter.ToUInt64(scratch, 24));
				if (plantAt.Count > 0) seedComponent["targetLandBlocks"] = Names(plantAt);
			}
		}

		// The legacy food component's two vectors, mEffects and mRemoveEffects. Neither has a class
		// of its own in the generic member table, so both are read here over the object the generic
		// pass already located and sized: the food component's own address, read whole.
		if (row["foodComponent"] is JsonObject foodComponent)
		{
			ulong foodAddress = item.Ptr(ItemLayout.FoodComponent);
			var food = new byte[176];
			if (foodAddress >= 0x10000 && process.TryRead(foodAddress, food, food.Length))
			{
				if (LegacyFoodEffects(process, food, scratch) is { } effects) foodComponent["effects"] = effects;
				if (LegacyFoodRemoveEffects(process, food) is { } removeEffects) foodComponent["removeEffects"] = removeEffects;
			}
		}

		// What the numbers above mean, where this tool knows: the names behind the enums and the
		// bits behind the flag byte. Derived, so it sits beside the class rather than inside it.
		var decoded = new JsonObject();
		if (NameOf(UseAnimationNames, item.Byte(ItemLayout.UseAnimation)) is { } an) decoded["useAnimation"] = an;
		if (NameOf(RarityNames, item.Byte(ItemLayout.Rarity)) is { } rn) decoded["rarity"] = rn;
		if (NameOf(MineBlockTypeNames, item.Byte(ItemLayout.MineBlockType)) is { } mb) decoded["mineBlockType"] = mb;
		if (NameOf(CreativeCategoryNames, item.Byte(ItemLayout.CreativeCategory)) is { } cn) decoded["creativeCategory"] = cn;

		// The nine flag bits are members of the class now, each under the name the class gives it,
		// so they are not also listed here. Two entries for one fact is a value lost, and the bit
		// the byte could not hold, ignoresPermissions, is the ninth: it sits in the byte after, and
		// reading that byte as a whole value is why it never agreed with the reference.
		if (decoded.Count > 0) row["decoded"] = decoded;

		// The block an item places is held either directly or through a small holder that points at
		// it. Reading only the direct form left the camera, the brewing stand, the cake and the
		// comparator with no block at all, and the holder is what the wire calls minecraft:block.
		if (item.IsItem)
		{
			// Directly at +88, or through the holder the class tail keeps just before the built
			// component map, wherever that map sits on this build.
			foreach (int slot in _tail is null ? (int[]) [88] : [88, _tail.Built - 8])
			{
				if (slot + 8 > item.Window.Length - Before) break;
				string blockName = BlockNameAt(process, item.Ptr(slot));
				if (blockName is null) continue;
				row["block"] = blockName;
				break;
			}
		}


		// Furnace behaviour, both halves: how many items this one burns for, and the experience
		// smelting it gives. Fitted against lava bucket at a hundred, boats at six and sticks at a
		// half, and against gold at one, iron at seven tenths and nuggets at a tenth.
		// Into the member that holds them, not beside it. The vector at that offset is what these
		// were read from, so a second list under another name would be the same fact twice.
		row["tags"] = Names(Tags(process, item, scratch, heap));

		// Every component reaches the reader the same way, whatever the server does internally. The
		// older items keep a typed pointer per kind at a fixed offset, the newer ones keep a map, and
		// a consumer should not have to know which: both end up under "components" keyed by the same
		// names, so food is food wherever the server put it.
		List<(string Name, ulong Address)> declared = _tail is null ? [] : Components(process, item, _tail.Declared, true);
		List<(string Name, ulong Address)> built = _tail is null ? [] : Components(process, item, _tail.Built, false);
		row["componentBased"] = declared.Count > 0;
		if (declared.Count > 0) row["declaredComponents"] = Names(declared.Select(c => c.Name));

		// Walked from both containers, not gated on componentBased: the declared set names 75 items'
		// components and the built map only resolves 31 of them, so a kind that sits in the declared
		// set alone (arrow's minecraft:projectile among them) used to come out with a name and nothing
		// behind it. Built is tried first, because it is the address the spears' damage, cooldown and
		// durability were fitted against and it is proven to hold real component objects. The declared
		// set's own value slot reads zero on every one of the 311 entries measured across all 75
		// componentBased items, on this build: it names a component without holding its address, so a
		// kind found only there cannot be read and is recorded as such rather than as an empty object,
		// which would say the component was read and turned out to hold nothing.
		var components = new JsonObject();
		foreach (var (kind, address) in built.Concat(declared))
		{
			// Ruled out by Niclas, not by this tool: legacy_events is 184 bytes holding a table of
			// sixteen event handler lists, and on the one item that carries it, the apple, every
			// one of those lists is an empty sentinel that points at itself. It describes nothing.
			// Its shape stays in items-runtime-components.json, where the bytes are still counted.
			if (kind == "minecraft:legacy_events") continue;
			if (components.ContainsKey(kind)) continue;
			components[kind] = address >= 0x10000
				? Component(process, kind, address, item.Address, scratch)
				: new JsonObject {["unread"] = "declared, no address; the built map does not carry it"};
		}

		// The legacy food, seed and camera components are not listed here. They are reached through
		// the pointers the item holds for them, so those members carry them: hoisting them into the
		// component map would state them under a name the object does not use.

		if (components.Count > 0) row["components"] = components;

		// The class tail's own fields sit at no fixed place: which slot holds what depends on the
		// class, and the whole tail moves when a version grows the object, so every slot in the tail
		// is tried and the contents decide. Each probe proves itself against a value the item states
		// elsewhere, so a wider sweep cannot invent a hit.
		int tailEnd = TailEnd(item);

		// A tier reads as a level, a durability, a mining speed, a damage bonus and an enchantment
		// value, and its durability must be the item's own: diamond comes back 3, 1561, 8, 3, 10 on
		// a tool whose max durability is 1561. An item may instead adopt a struct other tools proved
		// that way; the mace points at the diamond tier while carrying its own 500 durability.
		if (item.IsItem)
		{
			for (int slot = 240; slot <= tailEnd; slot += 8)
			{
				JsonObject tier = item.U16(ItemLayout.MaxDurability) > 0 ? Tier(process, item.Ptr(slot), scratch, item.U16(ItemLayout.MaxDurability)) : null;
				if (tier is null && _tierStructs.Contains(item.Ptr(slot))) tier = Tier(process, item.Ptr(slot), scratch, null);
				if (tier is null) continue;
				row["tier"] = tier;
				break;
			}
		}

		// A digger names the block tag it can destroy. The hash decides: a hashed string only reads
		// back when its text hashes to the number in front of it, so a wrong slot gives nothing
		// rather than a wrong tag.
		for (int slot = 240; slot <= tailEnd; slot += 8)
		{
			if (HashedAt(process, item.Ptr(slot)) is not { } tag || !tag.Contains("destructible", StringComparison.Ordinal)) continue;
			row["destroysBlocksTagged"] = tag;
			break;
		}

		// Armour keeps its material where every piece of a set points at the same struct, and the
		// struct must reproduce the piece's own durability: the multiplier divides it to the per-slot
		// base the game uses, 11, 13, 15 or 16.
		if (item.IsItem && item.U16(ItemLayout.MaxDurability) > 0)
		{
			for (int slot = 240; slot <= tailEnd; slot += 8)
			{
				if (ArmorMaterial(process, item.Ptr(slot), item.U16(ItemLayout.MaxDurability)) is not { } material) continue;
				row["armorMaterial"] = material;
				break;
			}
		}

		// Whatever the class adds after the shared fields, carried out as its own bytes rather than
		// left inside the window unmentioned. Where it ends is the next object, so this is what
		// belongs to this item and nothing beyond it.


		ulong vtable = item.Ptr(ItemLayout.MethodTable);

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
		// These three sit past +240, which is class specific, so they only mean anything on the class
		// the parser writes them into: the data-driven items. Gated on the object being big enough
		// they were read off every class, and a diamond pickaxe reported a mining speed taken from
		// inside its tier pointer and an enchantable value that was really its attack damage.
		// The gate is componentBased, and it is exact: all 75 such items read a mining speed of 1
		// and no item outside them does.
		if (declared.Count > 0)
		{
			int dd = _tail.DataDriven;
			row["miningSpeed"] = Real(BitConverter.ToSingle(item.Window, item.At(dd + 4)));
			row["liquidClipped"] = (item.Byte(dd) & 0x08) != 0;
			row["canDestroyInCreative"] = (item.Byte(dd) & 0x02) != 0;
			row["enchantableValue"] = BitConverter.ToInt32(item.Window, item.At(dd + 16));
			// Located the same way as the rest: the parser holds the key's text and stores the value
			// beside it. Damage verifies against the damage component on all seven spears, and the
			// slot is a bitmask rather than an index, reading 0x00800000 for melee_spear and 0 for
			// none, so the number goes out rather than a name invented for a bit.
			row["damage"] = BitConverter.ToInt32(item.Window, item.At(dd + 8));
			row["enchantableSlot"] = $"0x{BitConverter.ToInt32(item.Window, item.At(dd + 12)):X}";
		}

		// The forensic half of the row goes to its own file: the bytes it was read from, the holes,
		// and the class it belongs to. A consumer that wants the data should not have to step over
		// a kilobyte of hex to reach it, and a reader checking the extraction should not have to
		// parse the data to reach the evidence. Same rows, same names, two files.
		Forensics[item.Name] = Forensic(process, item, classNames, declared, built);
		return row;
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
		ulong begin = item.Ptr(ItemLayout.TagVector), end = item.Ptr(ItemLayout.TagVector + 8);
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
			item.ObjectSize = ClassSize(process, item.Ptr(ItemLayout.MethodTable));
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

	/// <summary>
	///     Whether a pointer is a method table: in the module, and its first slot holds module code.
	///     The range test alone is not enough where a component's first field sits: a small value
	///     plus the allocator's slack above it can compose a word that lands in the module range on
	///     one instance and not the next, which flipped the field base per run and read a spear's
	///     damage component as string bytes. Slack can fake an address; it cannot also place module
	///     code behind it.
	/// </summary>
	private static bool IsVtable(BedrockProcess process, ulong pointer)
	{
		if (!IsModule(pointer) || !process.IsMapped(pointer)) return false;
		var word = new byte[8];
		return IsModule(process.ReadUInt64(pointer, word));
	}

	private static string Num(float value) => value.ToString("0.#####", CultureInfo.InvariantCulture);


	public static string ReadStdString(BedrockProcess process, byte[] window, int at, byte[] scratch) =>
		StdString(process, window, at, scratch);

	/// <summary>A string read straight from an address, for the reads that happen before any window exists.</summary>
	private static string StdString(BedrockProcess process, byte[] window, int at, byte[] scratch, ulong address)
	{
		return process.TryRead(address, window, 32) ? StdString(process, window, at, scratch) : null;
	}

	internal static string StdString(BedrockProcess process, byte[] window, int at, byte[] scratch)
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
