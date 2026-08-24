# Captured frames

Two server-to-client packets taken off the wire from vanilla BDS 1.26.50.26 (protocol 2192), the
same build the extraction data under `MiNET.BdsExtract/Data` was read from. They are the check
for what the generator produces from that data, never its source: a generated tree is diffed
against the frame, and every difference is reported.

| File | Packet | Id |
|---|---|---|
| `item_registry-1.26.50.26.bin` | ItemRegistry (`McpeItemComponent`) | 162 |
| `creative_content-1.26.50.26.bin` | CreativeContent (`McpeCreativeContent`) | 145 |

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
