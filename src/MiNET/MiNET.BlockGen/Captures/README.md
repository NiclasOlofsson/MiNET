# Captured frames

Three server-to-client packets taken off the wire from vanilla BDS 1.26.70.20 (protocol 2235), the
same build the extraction data under `MiNET.BdsExtract/Data` was read from. They are witnesses for
what the generator produces from that data, never its source: a generated tree is diffed against
the frame and every difference is reported. The generator writes from the extraction whether a
frame is present or not; a frame from another build reports the version gap on every run.

| File | Packet | Id |
|---|---|---|
| `item_registry-1.26.70.20.bin` | ItemRegistry (`McpeItemComponent`) | 162 |
| `creative_content-1.26.70.20.bin` | CreativeContent (`McpeCreativeContent`) | 145 |
| `startgame-1.26.70.20.bin` | StartGame (`McpeStartGame`) | 11 |

The StartGame frame is the check for the block definitions: its `blockProperties` array holds the
98 data-driven vanilla blocks, and each generated definition has to serialize to that entry's own
bytes. Its `experiments` list is what the generated experiments list is measured against.

Each file is one decompressed Bedrock frame as `MINET_PACKET_DUMP` writes it: the varuint packet
header followed by the packet body. `MiNET.BlockGen` finds them by name pattern
(`startgame-*.bin`, `item_registry-*.bin`, `creative_content-*.bin`) and measures against the last
in name order when several are present, so a newer capture beside an older one wins without a
code edit.

Server configuration at capture, so a later capture is comparable: the extraction's canonical
config (`MiNET.BdsExtract --prepare`, which copies `Assets/flatworld` in with its experiments on)
with `editorWorldType` set to 0 in the copied world's `level.dat` so a normal client can join,
`online-mode=false`, `block-network-ids-are-hashes=false`, default `view-distance`. The client was
`MiNET.Client` with `MINET_XBL=1`: since 1.26.60.30 BDS disconnects a signaling-only offline login with
`NotAuthenticated` even with `online-mode=false`. The committed asset world is never edited; the flip is
applied to the copy under the server folder, which `--prepare` overwrites on its next run.

The one known difference from the asset world itself: an Editor world's creative list carries
`editor:map_marker_spawn_egg`, which a normal world does not send, so the creative frame holds
1,993 entries against the extraction's 1,994.

The StartGame frame's experiment set is one short of the committed asset world's: the frame lists
six names and `Assets/flatworld/level.dat` has the same six plus `y_2026_drop_3`, which BDS
1.26.70.20 does not put on the wire. The generator reports that difference on every run. The
1.26.60.23 frame carried two more, `poi` and `creator_world_clocks`, which the world does not
declare; 1.26.60.30 and later no longer send them.
