# MiNET.BdsExtract

Reads a running Bedrock Dedicated Server through `ReadProcessMemory` and writes what it finds as
JSON. Two sources and no third: the live process, and `bedrock_server.exe` read off disk as the
code's own statement about its classes. Nothing here parses a data file the server ships.

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

## The binary states the classes

Before any object is read out of the process, `bedrock_server.exe` is read off disk. What the code
says about its own classes is a second witness, independent of memory, and it goes to
`Data/binary_facts.json`.

The names come from the compiler. Clang bakes the function's own signature into the assertion and
diagnostic paths of templated code, so every instantiation the linker kept leaves a literal naming
both the template and the type it was instantiated with. The two component sides use different
templates and so different spellings: the block side is `BlockComponentStorage` with a parameter
named T, `... [T = BlockChestObstructionComponent]`, and the item side goes through cereal and entt,
whose parameter is named Type, `cereal::internal::TypeSchema<FoodItemComponent> ... [Type =
FoodItemComponent]`. Reading those literals out of the data sections is the complete class list for
the build being read, stated by that build.

A type id is not in the image. Bedrock hands a block component class a 16-bit id the first time
something asks for that component and keeps it in a static beside the templated accessor, so the
image states where the id will be written and never what it is. The run reads that static out of
the process, every run, and a slot still holding nought is a class this world never asked for
rather than a class filed under id nought. A number written down against an earlier run describes a
heap that no longer exists: 7 was redstone conductivity in one such table and is the redstone
component now, and 26 was leashable and is the connection component.

Method tables are the identity a heap object carries. An item component is built by its own schema
loader, so the table stored into its first word is the class's own and its deleting destructor
states the class's size. A block component is built inside a storage node that holds it, so the
table is the node's, and a node holding eight bytes or less is the same node whatever it holds:
eighteen block classes fold onto one table on 1.26.50.26. That is why the block side names those
`nodeVtable` and `nodeSize`, and why any of the eighteen confirms the table.

The bindings are what covers the members. Most components hand `this` to the EnTT binder and touch
nothing themselves, so the getter the binder installs is where a member's offset appears, as the
adjustment it makes to the object pointer, with the member's meta type id stored beside it. That id
is FNV-1a 32 of the type's name, so `0xA6C45D85` is `float`, and it is resolved against the
primitives plus every name the signature inventory already holds. The offsets are whole-object
offsets, and the name the binder registers is the wire name.

Every position in `binary_facts.json` is a relative virtual address of the image on disk. Nothing
in that file is a process address. The live phase adds the module's load address, which is the main
module's base address and no library's, and those addresses stay out of the file: two runs against
one binary write it byte for byte the same, so a difference between them is a difference in what
was read.

The reference is then checked against the code as well as against memory. The memory checks say
whether an offset still reads the value the reference states; this says whether the layout it
declares is the one the build's own code describes, in the same voice and on one line:

```
binary check: 56 classes agree on size, 2 disagree, 3 the binary does not size, 3 name no binary
class; 75 undeclared classes; ... ; 15 of 53 name pairs unmapped
```

Nothing there stops the run, and nothing there is acted on. A class the binary sizes differently
from the reference, a class the binary names and the reference does not declare, a member the
binder places somewhere else: each is reported, counted, and left for Niclas.

Wire names and class names are both recorded and neither is turned into the other. The binary gives
`seconds_to_destroy`; the reference carries the class's own name with the `m` dropped,
`secondsToDestroy`. The unmapped pairs are counted and listed. Mapping one onto the other is a
ruling, not a rule this tool applies.

Two things the reference states that nothing above says. A member may carry a `gate`: where the
byte that says whether the member holds a value sits, counted from the member's own first byte. An
optional whose flag is clear has never had its value written, so its storage holds whatever was
there before, and a gated member with a clear flag comes out null. And a member may be padding,
which the class table states once and the rows never carry, because padding is alignment and a row
saying so is a row of noise; `--show-padding` puts it in the rows as its raw bytes for a debugging
view, never as null, and the run tallies every padding span either way and reports any that is not
zero.

## The code is walked by flow

`Binary/` is the reading of the exe. `PeImage` opens it: sections, the exception directory as one
range per `RUNTIME_FUNCTION` with a chained (cold-split) range naming the primary it belongs to,
the entry point, exports, TLS callbacks, the import table by slot, and the Control Flow Guard
dispatch slot from the load config. The unwind info's handler and chain slot is read from the
bytes, not from the PE library, because the library places it wrongly on some entries.
`CodeIndex` decodes `.text` once, linearly, into a map from every rip-relative target to the
instructions that reference it. `ControlFlow` is what makes a hit in that map evidence: it walks
every `.pdata` function by flow, fall-through, both arms of a branch, jump targets, on past a call,
and marks the instruction starts flow reaches. A `CodeIndex` hit at a byte flow never reaches is
padding or data decoded as garbage and is rejected, not weighed.

A `.pdata` range is the boundary the walk trusts. What it points at outside every range, a call or
tail-jump target, a pointer a data section holds into `.text` (a vtable slot, a function table),
an exception handler, the entry point, is walked as a discovered function. Every one carries its
origin and its witness, the instruction or slot that named it, and either verifies (every path
ends in a ret, a no-return call, a tail jump or a trap, inside bytes no other function owns) or
is listed with the fault, never dropped. A discovered function owns only the contiguous run of
instructions from its entry; its blocks may lie beyond, with other functions between. An import
thunk, a lone jmp through the import table, carries the import's name. The leaf getters vtables
point at, two instructions each, are the functions `.pdata` does not list, and they are most of
what is discovered.

A switch is read from its own table: `add R,Base; mov R32,[Base+Index*4+disp]; jmp R`, with Base
either `__ImageBase` and RVA entries or the table itself and table-relative entries, the lea hoisted
to the entry block. The case count comes from the check on the index: `cmp` in any width with the
branch that follows deciding N or N+1, an `and` mask, or the byte table a two-level switch indexes
through, with the check possibly on a register the index was copied from in the block before.
Without a check the entries are read while they land inside the function, and that is the one
place the walk still invents: an entry past the real table that happens to land inside the
function becomes a block that is not one. Such tables are marked `Range`, counted, and the faults
they cause (an invalid instruction inside a `.pdata` function) are listed against them. A jump
through the CFG dispatcher, `mov R,[__guard_dispatch_icall_fptr]; jmp R`, or through any memory
operand, is an indirect tail call with an unknowable target, and is counted as that, not as a
table the walk failed to read.

No-return is a fixpoint with two sources. The walk's own finding: no path out of the function
returns, no ret, no fault, no indirect tail call, every direct tail jump onto a no-return function
or import. And the compiler's marks: a call that a `.pdata` range ends on, or that flow falls from
into an `int3`, names a callee that does not return, because the compiler emitted nothing after it.
In this image a trap flow reaches is, in all but a handful of cases, the instruction after a call;
the exceptions are `ud2` after a branch, the unreachable marker. Callers of what qualifies are
walked again, so a ret only reachable by falling through such a call stops counting, and the final
walk redoes every function against the final set. `__std_terminate` and `_CxxThrowException` are
import thunks with no `.pdata` entry; the EH funclet that calls one and traps is the shape of
almost every no-return function.

The real-image test writes what a run establishes and what it does not to
`temp_auto/controlflow-report.txt`: functions by origin, verified and unverified, tables by bound
kind, unresolved jumps with the instructions above them, no-return callees by caller count, every
faulting `.pdata` function with the instruction it fell from. Those counts are the holes; each is
a defect until ruled otherwise.

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

# one run: blocks and states, items, the creative inventory, and the sound event table
dotnet run --project src/MiNET/MiNET.BdsExtract -- --server server-1.26.20.5

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
- `binary_facts.json`: what `bedrock_server.exe` states about itself, read from the file on disk
  before the process is touched. The component classes each storage family names, the block side's
  type-id slots, method tables, class sizes and networked verdicts, the enum tables the reference
  seeds, the member accesses the serializers perform, and the reflection bindings the EnTT binder
  registers (wire name, owner, offset, entt type id and its name, width). Every position is an RVA
  of the image, never a process address, and the file carries the exe's SHA-256 so it is tied to the
  binary that produced it. Nothing is dropped: what did not resolve is counted under `unresolved`
  with the reason.

## What is still unread

Most of a block object. `BlockType` measures 888 bytes on 1.26.50.26, and every one of them is
accounted for: 184 are read as values and 704 are containers declared with their offset and size
and read no further. Past that the derived block class adds its own tail, and 497 of the 1,477
blocks have none at all, so their object is the base class exactly; the rest add 8 to 416 bytes on
top and no layout exists for any of it.

The object's size is found by scanning forward for the allocator's block header, which fails
outright on a fifth of blocks. The class's own size is exact and reachable: it is baked into the
deleting destructor, which the method table points at, and the item side already reads it that way.

## House rules

- No new files without asking. Adding one beside the existing thing rather than replacing it is how
  the block object ended up being read twice by two mechanisms.
- One reading of one object. If there are two, one of them is wrong and nobody knows which.
- Do not restate a measurement as a cause. If you cannot name the command or the line of code that
  produced a number, say it was not measured rather than explaining it.
