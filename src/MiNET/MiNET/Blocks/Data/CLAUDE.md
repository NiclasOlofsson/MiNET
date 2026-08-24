# Blocks/Data: where these files come from

Embedded resources (`MiNET.Blocks.Data.*`), read at runtime through `ResourceUtil.ReadResource<T>(name, typeof(Block), "Data")`. **Nothing in this folder is generated.** Do not "regenerate" it, and do not assume running `MiNET.BlockGen` refreshes it.

The generator writes C# next to this folder, never into it. `MiNET.BlockGen` produces `Blocks/BlockData.generated.cs`, `Blocks/PartialBlocks.cs` and `Blocks/BlockPaletteData.generated.cs` from the BDS memory extraction committed under `MiNET.BdsExtract/Data` (`block_states.json`, `blocks.json`, `creative_items.json`; build 1.26.50.26), and refuses to write unless every state reproduces its own network hash. The palette lives in those generated .cs files, not here.

| File | Read by | Source |
|---|---|---|
| `block_id_map.json` | [BlockFactory.cs:389](../BlockFactory.cs#L389) | Committed data, not generated. Last content change 4de3312a ("Fix missing block variants in creative inventory"), predating the BlockGen pipeline. Upstream origin is outside this repo's history. |
| `r12_to_current_block_map.bin` | [BlockFactory.cs:400](../BlockFactory.cs#L400) | pmmp/BedrockData legacy mapping (r12 = 1.12 ids/metadata to modern block states). Committed binary, not generated, not tracked as a submodule. pmmp lags Mojang releases, so this file ages behind the palette. [LegacyBlockMappingTests.cs](../../../MiNET.Test/LegacyBlockMappingTests.cs) exists to catch that: it fails when fewer than 3000 legacy pairs still resolve. |
| `item_id_map.json` | **nothing** | Byte-identical duplicate of [Items/Data/item_id_map.json](../../Items/Data/item_id_map.json) (both 29423 bytes). Every reader resolves it through `typeof(Item)`, which is the Items copy. This copy is dead weight. |
| `legacy_id_map.json` | **nothing** | No reader anywhere in the solution. Dead file. |

## Rules

- Changing block ids or palette content means running `MiNET.BdsExtract` against the target BDS (which rewrites `MiNET.BdsExtract/Data`) and then `MiNET.BlockGen`, which rewrites the generated .cs files. Neither touches this folder.
- `block_id_map.json` and `r12_to_current_block_map.bin` have no generator, so a protocol bump does not update them. When they go stale they must be sourced by hand, and the failure is silent apart from the legacy-mapping test.
- Before claiming a file here is generated, or stale, or the cause of a wire difference, check this table. Two of the four files are not read at all.
