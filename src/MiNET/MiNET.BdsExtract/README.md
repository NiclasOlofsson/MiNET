# MiNET.BdsExtract

Gets MiNET's block data out of a Bedrock Dedicated Server, and out of nothing else.

Everything here comes from the server: half of it read out of the running process, half of it
answered by the server itself through a behaviour pack that asks it about every block. Nothing is
downloaded, nothing is copied from another project's data files, and nothing is reconstructed by
sorting or guessing.

## Why it exists

The block data MiNET shipped for years came from a third party dump. Nobody could reproduce it,
nothing said how it had been produced, and two of its fields turned out to be wrong: its blast
resistance is on the pre-1.8 scale, five times the real value, and its
`requiresCorrectToolForDrops` says stone and diamond ore need no tool while wheat does. Dumps also
lag by weeks and stop at whichever version somebody bothered to publish.

A running server knows all of it. So this asks the server, twice, in two different ways.

## The two halves

They answer different questions and neither can answer the other's.

**Memory** reads the process directly. It gets the palette in the server's own order, which is the
runtime id when hashed ids are off, and the compiled properties: hardness, friction, blast
resistance and the rest. It cannot say what a state *is*: there is no "facing east, upside down"
anywhere in memory that it can attribute.

**Probe** runs a server with a behaviour pack that walks every block. It gets exactly what each
state is, which values each block accepts, its tags, components, item form, and light measured by
experiment. It has no idea what order the palette is in, and cannot: nothing the runtime publishes
carries the index.

They meet on the **network id**, a hash of the name and states. Both sides compute it
independently, and so does the client, so no assumption about ordering ever passes between them. If
one side's state list is wrong its rows simply fail to match, instead of quietly landing on the
wrong block.

The hash is FNV-1a 32 over the little endian NBT of `{ name, states }`, states sorted by name, no
version field.

## Running it

Memory side, against a server that is already up:

    dotnet run --project MiNET.BdsExtract -- --out ..\..\..\temp_auto\extract

    --out     where to write the json files, default the working directory
    --server  which server to read when several are running, matched on executable
              path, for example --server bds\probe

Start the server first and wait for `Server started`. If more than one is running the tool refuses
to guess and asks for `--server`, so a shared machine cannot silently get read from the wrong one.

Probe side, which starts and stops its own server:

    dotnet run --project MiNET.BdsExtract -- --probe ..\..\..\temp_auto\bds\server-1.26.50.26 out

It needs the server's own block documentation, which BDS writes when started once with a
`test_config.json` of `{"generate_documentation": true}`. A full run is about an hour: it places
every block state twice and reads it on the following tick.

Windows only for the memory side, which uses the Win32 process reading API.

## What comes out

From memory, `block_palette.json`, one entry per block state, in the server's order:

    { "index": 2, "name": "minecraft:blue_candle", "nameHash": "0x0081936E3D920400",
      "networkId": 1088625327, "legacyId": 679 }

`index` is the runtime id when the server runs with `block-network-ids-are-hashes=false`,
`networkId` is the runtime id when it runs with `true`. `legacyId` and `nameHash` are per block, so
every state of one block repeats them.

From memory, `block_properties.json`, one entry per block:

    name, nameHash, legacyId, hardness, explosionResistance, friction, thickness,
    translucency, lightEmission, lightDampening, burnOdds, flameOdds, isSolid,
    canContainLiquidSource, liquidReactionOnTouch, tintMethod, mapColor

From the probe, `blockstates-runtime.json`, one row per block state, each carrying everything that
state answered: its states and the values its block accepts, tags, the four liquid answers, the item
form, `isAir` / `isLiquid` / `isWaterlogged`, redstone power, multi-block parts, component names and
every value each component exposes, measured light emission, and light dampening. Plus its network
id, which is what joins it to the memory rows.

Dampening is written as the value, with the corridor reading kept beside it. The mapping was
calibrated once against a known table and is exact: `dampening = 14 - reading`. Light loses a level
crossing any block at all, so an empty corridor reads 13 and a block that dampens by three reads 11.
Every block the table calls 15 read nothing at all, 577 of them, no exceptions. Where a reading
cannot be taken the row says why instead of implying a precision it does not have.

From the probe, `blockstates.json`, the same set reduced to name, states and network id.

Nothing is summarised, collapsed or reported only when it changes. Every row says what that state
is, so reading it never requires knowing a fallback rule. That matters more than the file size: the
old data recorded light per block rather than per state, which is why every candle in MiNET was
unlit. Lighting a candle is `lit` plus `candles`, and the palette leads each candle's run with the
unlit one.

## How the memory side works

### Finding the blocks

Bedrock stores block names as a *hashed string*: the hash of the text, immediately followed by the
text. That pairing is what makes this possible at all.

The tool sweeps memory looking for it: a number, then text, where the text actually hashes to that
number. Random bytes do not satisfy that, so the search cannot invent a block or misread a name. It
can only fail to find one, which shows up as a missing entry rather than a wrong one. There is no
list of expected names anywhere in this tool.

Each block then points at a second object holding what it takes to destroy it, and the properties
are read from both at the offsets in `MemoryLayout`.

### Finding the palette

The order is the whole point, so it must not be reconstructed.

The server keeps the palette in a vector, and a vector in memory is three pointers: where it begins,
where it ends, and how much room it has. Finding that header settles where the palette starts,
because that is what "begins" means, and the distance between the first two gives the entry count.
Nothing is sorted and no threshold decides where the first slot is.

The header is recognised by what it points at. Every entry between begin and end has to be one of
the block state objects found earlier. Only the palette satisfies that. Chunk storage looks similar
but holds exactly 4096 entries, one per position in a section, so it is never mistaken for it.

Each entry is then resolved through its own object rather than through any shared lookup, so one
unreadable block cannot cost unrelated entries their names.

### Checking itself

The dangerous failure is silent. If a field moves in a new server build, the tool still reads a
number, and a wrong number does not look wrong.

So every run checks things the real palette cannot fail: every slot named, every name hashing to the
hash stored beside it, all states of a block adjacent, blocks in ascending order by name hash,
network ids unique, and one legacy id per block. The tool exits non-zero if any of these fail. They
cost nothing and they turn a silent wrong answer into a loud one.

## How the probe works

### Which states are real

The server's documentation module lists, per block, the states that travel on the wire. This is not
optional detail. The runtime still reports states that predate the flattening, `wall_block_type` on
every wall with fourteen values, and enumerating what the runtime reports rather than what the
documentation lists produces 68,972 block states against the real 22,055.

The tool generates that list into the pack before deploying it, so the pack varies only states that
exist.

### Which values a block accepts

The documentation publishes one shared range per state *name*, across every block that uses it, so
`age` reads 0 to 15 even for blocks that take far less.

`BlockPermutation.resolve` does not throw for a value a block does not accept. It quietly returns
the state's default. So asking for each value in turn and checking whether the answer is what was
asked is an exact test, it runs in memory, and it takes milliseconds. Cocoa accepts 0 to 2.

### Two rooms

Light needs the block to exist somewhere, and the two light questions need opposite conditions.

The **dark room** is a pocket sealed in deepslate. The block goes in, and the cell beside it reads
what the block emits, one step of decay away, so emission is that reading plus one.

The **lit room** is a sealed corridor with a lamp at one end, the block in the middle and the
reading taken at the far end, so what arrives has crossed exactly one block.

Both get the same permutation in the same tick and both are read on the next, so one pass answers
both. The corridor reading is kept alongside the value derived from it, so the measurement and the
interpretation of it stay separable.

### Surviving the server

The server dies during this, and the ways it dies are worth knowing before changing anything.

**Placements wear it out.** After roughly four thousand placements in one spot it begins crashing
every twenty to fifty placements, for the rest of that world's life, whatever is being placed.
Restarting does not recover it. So the two rooms move, a chunk at a time, a fresh pair every 250
placements. This is the difference between finishing and not: every run without it exhausted a
hundred restarts around the halfway mark, and the first run with it completed with zero crashes.

**Connection states cannot be forced.** Fences and panes carry `minecraft:connection_east` and its
siblings, walls carry `wall_connection_type_*`. The engine works those out from what is next to the
block, and demanding that a wall claim it joins something while sealed in stone is a shape it
cannot hold. It dies rather than refusing. Those states are still enumerated for the palette, they
are simply never placed, and a connection cannot affect how much light a block emits.

**Legacy and current states together are fatal** for the same reason: `small_amethyst_bud` carries
both `facing_direction` and `minecraft:block_face`, and asking for both independently describes a
block that cannot exist. Using the documented state list avoids this.

**The world is scaffolding.** Nothing in it is kept, so the run holds the save and turns off every
game rule it can: tile and entity drops, mob spawning and loot, fire tick, random ticks, weather and
daylight. Drops were a good suspect, since a great many blocks cannot stand alone and break the
instant they land. Measured, the entity count stays at zero for a whole run, so that one is ruled
out rather than assumed.

**A ticking area larger than radius 4 never loads**, whatever the command reports, and everything
the sweep touches has to sit within about 64 blocks of the middle.

### The supervisor

The tool starts the server, reads its output, and stops it through its own console so the run ends
cleanly.

Every row the pack emits carries its index. When the server dies the tool restarts it and says
`scriptevent minet:begin <index>` to resume past the row that killed it, and the pack starts there.
Progress is not kept in the world: it belongs to whoever is reading the output, otherwise a fresh
run cannot be told from a resumed one. Readings lost to a crash are counted and listed at the end,
never quietly dropped.

The tool also brings `server.properties` to what the sweep needs, naming every change it makes:
`allow-cheats` for the ticking area command, the content log settings because the output *is* the
result, and a fixed `level-seed` so two runs read the same world.

## What is left open

The two halves between them answer everything MiNET needs, which is the reason there are two. The
palette order is the clearest case: nothing the runtime publishes carries the index, and no ordering
rule reproduces it, not lexical, not a hash of the name, not the documentation order, not the
runtime's own. Three blocks even rank the same pair of states in opposite orders. The memory side
simply reads it, and the probe side never has to care.

What genuinely is not answered:

**Dampening of zero and one cannot be told apart by measurement**, and no rig can fix that: Bedrock
propagates light losing `max(1, dampening)` per block, so the two behave identically and the game
itself does not distinguish them. Leaves, powder snow and water are the blocks in that gap, and the
exact number comes from the memory side, which reads the stored value rather than its effect.

Two more readings are refused rather than guessed. A block that **emits light** lights the far cell
itself, and every one of them reads exactly its own emission minus one: crying obsidian 9 for its
10, a lit furnace 12 for its 13. A block that **did not stay where it was put**, which is every
liquid and anything needing support, was never there to be measured, so the reading is of whatever
replaced it.

**Fourteen blocks are absent from the documentation** and are named on every run rather than
dropped: chalkboard, the deprecated purpur blocks, `info_update`, `glowingobsidian`, `end_gateway`
and the rest of the education and legacy set. No Mojang published source describes them, so their
states are whatever the runtime reports.

**Components are missing for blocks that break on placement.** They only exist on a block that is
really somewhere, so a block that cannot stand alone has none.

## When a new server version breaks it

The offsets in `MemoryLayout` are version specific. When the checks above start failing, a field has
moved.

They were originally found by taking blocks whose values are known, looking at every position in
memory around them, and keeping the one position that agreed with every block at once. A wrong
offset does not fit a thousand blocks by accident. Re-derive them the same way rather than nudging
numbers until something compiles.

The probe side does not have offsets to break. What breaks it is the game changing what a block is:
a new state that behaves like a connection, or a new pairing of a legacy state with its replacement.
Both show up as the server dying on particular blocks, and the crash list at the end of a run names
them.
