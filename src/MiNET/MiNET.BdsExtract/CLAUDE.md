# MiNET.BdsExtract

Reads a running Bedrock Dedicated Server through `ReadProcessMemory` and writes what it finds as
JSON. Nothing here parses a file the server ships. Every value comes out of the live process.

## The first rule: no loss, no invention

Every byte read is declared, identified and emitted.

- A value that was read reaches the output. Not most of it, not the part that fit the shape chosen.
- A field that is not understood is emitted saying so, with its offset and size, so the hole is
  countable. Omitting it makes the output look complete when it is not.
- Nothing is filtered on judgment. What is interesting is the reader's call, not the tool's.
- Nothing is invented. A read that failed is reported as failed. The tool may fail to find a thing;
  it may never write something it did not read.

The second half of that rule is the one that gets broken quietly. A hole is visible. A field emitted
under a name that belongs to something else is not, and no reader can detect it from the output.
Three fields were wrong that way at once: `material` was the render layer, `solid` and
`requiresCorrectToolForDrops` were two bits of a float, and every one of them matched the reference
perfectly, because the reference had been produced by the same wrong read.

## Names come from the class

The block object is `BlockType` and the item object is `Item`. LeviLamina publishes generated
headers for both, recovered from the binary and its PDB symbols, stating each member's type, size
and alignment in declaration order. That is a complete layout: a member starts at the running total
rounded up to its own alignment. Nothing in those headers is an offset, and none is needed.

Field names are the class's own with the `m` dropped: `mSolid` becomes `solid`, `mLightBlock`
becomes `lightBlock`, `mID` becomes `id`. A name this tool made up is a name that can be wrong.

LeviLamina's `xmake.lua` pins which build its headers are generated against. At the time of writing
that is `bedrockdata v26.20.5-server`, so 1.26.20.5 is the reference build and no other. Check the
pin before trusting the headers against a different build.

The headers are a reconstruction, not a first-party source, so by the repo's trust order they settle
nothing alone. What makes them usable is agreement: six of the item flag bits already matched names
pinned from behaviour before the header was read, and thirteen block offsets landed exactly on a
member. Two independent confirmations, not one list.

## The reference states the layout, the run checks it

Offsets live in the reference and the tool reads them. Nothing searches for a position, nothing
moves one, and no offset is compiled into the source.

Every offset in the reference is relative to the class that declares it. A class holding another
class states its own offset and nothing about what is inside it, so a member's place in the object
is its own offset added up through whatever holds it. That is why `nameInfo` shrinking from 176 to
160 bytes on 1.26.50.26 moves everything after it and nothing inside it.

The run then checks that layout against the server in front of it. Every member is read where the
reference puts it and counted against the value the reference states for that same object, matched
by what the object is: a block by its full name, a state by its block's name and its property
values.

- At or above 0.95, the offset still reads the field. The run goes on and says nothing.
- Below 0.95, it has slipped. The run says so and goes on.
- Below 0.50, the offset is reading something else. The run stops before writing anything.

Both numbers are Niclas's. Do not add a third, do not put either on the command line.

When a member breaks, its offset is corrected in the reference by hand and the run is repeated until
the new layout reads. That is the working loop for a new build.

A round trip is not evidence of a layout. The run reads the file's own offsets, reads memory at
them, and writes them back, so identical bytes come out whether the offsets are right or wrong. What
the check reports is the evidence.

## The reference

`Assets/reference-blocks.json`, `Assets/reference-block-states.json` and
`Assets/reference-items.json` are extractions whose checks all passed, committed and never written
by a run. A run reads them; it does not update them.

They carry their own member list, class size and build in the header, so the output of a run can
become the next reference without any table living in source.

Pointing the reference at the live output makes the tool trust its own last answer, and one bad
extraction then poisons every run after it. When a value stops matching, find the reason first and
replace the file deliberately. That is the difference between the game changing and the reader
breaking.

## Reporting

Every difference between a run and the reference is reported, in full, every time. Do not filter, do
not rank, do not call one cosmetic or expected, and never drop one from a summary. Classifying a
difference as acceptable is Niclas's call.

A count the tool prints must be a count the tool acts on. Printing a second number for the same fact
is how a wrong story gets told: the network id probe verified a position against all 15,845 states,
printed `0 hold something else`, then returned a counter capped at 223, and the caller rejected the
answer it had just proved.

A number in a log is not evidence until you know which code emitted it. Two outputs disagreeing is a
defect to report, not a choice between them.

## Floats

A float is written to the number of decimals the game states the field to: three for friction,
because blue ice is 0.989, two everywhere else. Rounded as a double, because a fixed decimal format
applied to a `float` caps at seven significant digits and turns barrier's blast resistance
3600000.75 into 3600001.

The same routine writes the file and compares against it, so a value cannot be stated one way and
read back another.

## JSON

Two entries under one key is a value lost. A reader keeps the last and never sees the first, and
nothing in the file says it happened. This has cost real data twice: property constraints once, and
the block class member `tags` and `properties` colliding with the derived lists of the same name.

## Running it

BDS resolves every path against its working directory, so start it from its own folder or it writes
its world and its docs wherever it was launched.

```bash
# bring a server folder to the canonical configuration in Assets
dotnet run --project src/MiNET/MiNET.BdsExtract -- --prepare temp_auto/bds/server-1.26.20.5

# start it from its own folder
(cd temp_auto/bds/server-1.26.20.5 && ./bedrock_server.exe > ../../bds.log 2>&1 &)

# blocks and states
dotnet run --project src/MiNET/MiNET.BdsExtract -- --server server-1.26.20.5

# items
dotnet run --project src/MiNET/MiNET.BdsExtract -- --items --server server-1.26.20.5

# every build, both extractions, every guard
bash src/MiNET/MiNET.BdsExtract/conformance.sh
```

The world and `server.properties` in `Assets` are the canonical configuration and the startup guard
refuses to run without them: `block-network-ids-are-hashes=false`, and the experiments on, because a
world with them off comes out short and every id after the gap is silently wrong.

One command, one purpose. A run writes to a log; reading the log is a separate step. Never pipe an
extraction through `grep` and report what came out: that is choosing what the reader sees.

## Output

- `blocks.json`: one row per block, every member of the class under its own name, plus what the tool
  derives around it (class, geometry, tags, state properties, legacy data values, address, object
  size, unread spans). It states its own member list in the header.
- `block_layout.json`: the object as the run actually read it. Every member at the position it was
  read from, how that position was arrived at, every stretch between them that no member covers, and
  the derived class tail past the base class.
- `block_states.json`, `block_upgrade_rules.json`, `component_names.json`, `items-runtime*.json`.

## What is still unread

Roughly two thirds of a block object. Of the 992-byte base class, 403 bytes are read as values, 536
are containers declared with their offset and size, and 53 are alignment padding. Past that, the
derived block class adds its own tail, 8 bytes on most blocks and up to 3,096 on a few, and no
layout exists for any of it.

The object's size is found by scanning forward for the allocator's block header, which fails
outright on a fifth of blocks. The class's own size is exact and reachable: it is baked into the
deleting destructor, which the method table points at, and the item side already reads it that way.

## House rules

- No new files without asking. Adding one beside the existing thing rather than replacing it is how
  the block object ended up being read twice by two mechanisms.
- One reading of one object. If there are two, one of them is wrong and nobody knows which.
- Do not restate a measurement as a cause. If you cannot name the command or the line of code that
  produced a number, say it was not measured rather than explaining it.
