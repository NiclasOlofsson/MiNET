# Captured frames

Three server-to-client packets taken off the wire from vanilla BDS 1.26.50.26 (protocol 2192), the
same build the extraction data under `MiNET.BdsExtract/Data` was read from. They are the check
for what the generator produces from that data, never its source: a generated tree is diffed
against the frame, and every difference is reported.

| File | Packet | Id |
|---|---|---|
| `item_registry-1.26.50.26.bin` | ItemRegistry (`McpeItemComponent`) | 162 |
| `creative_content-1.26.50.26.bin` | CreativeContent (`McpeCreativeContent`) | 145 |
| `startgame-1.26.50.26.bin` | StartGame (`McpeStartGame`) | 11 |

The StartGame frame is the check for the block definitions: its `blockProperties` array holds the
98 data-driven vanilla blocks, and each generated definition has to serialize to that entry's own
bytes. Its `experiments` list is what the generated experiments list is measured against.

Each file is one decompressed Bedrock frame as `MINET_PACKET_DUMP` writes it: the varuint packet
header followed by the packet body.

Server configuration at capture, so a later capture is comparable: a copy of the extraction
asset world (`MiNET.BdsExtract/Assets/flatworld`, its experiments on) with `editorWorldType` set
to 0 in `level.dat` so a normal client can join, `online-mode=false`,
`block-network-ids-are-hashes=false`, default `view-distance`. The client was `MiNET.Client`
with `MINET_XBL=signaling`.

The one known difference from the asset world itself: an Editor world's creative list carries
`editor:map_marker_spawn_egg`, which a normal world does not send, so the creative frame holds
1,980 entries against the extraction's 1,981.

The StartGame frame's world also has a different experiment set from the committed asset world:
the frame lists `data_driven_vanilla_blocks_and_items` where `Assets/flatworld/level.dat` lists
`y_2026_drop_3`, the other five names being the same. The generator reports that difference on
every run.
