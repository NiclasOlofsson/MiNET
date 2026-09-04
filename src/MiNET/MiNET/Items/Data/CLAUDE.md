# Items/Data: where these files come from

Embedded resources (`MiNET.Items.Data.*`), read at runtime through `ResourceUtil.ReadResource<T>(name, typeof(Item), "Data")`. Only one of the three files is generated.

The item classes and registry are generated C# written next to this folder, not into it: `MiNET.BlockGen` produces `Items/ItemRegistryData.generated.cs` and `Items/ItemData.generated.cs` from the BDS memory extraction ([MiNET.BdsExtract/Data/items-runtime.json](../../../MiNET.BdsExtract/Data/items-runtime.json)). The registry file carries each item's component tree as fNbt construction code with the wire field names, and [ItemRegistry.Add](../ItemRegistry.cs) serializes it once at startup into the network NBT the item_registry packet sends.

| File | Read by | Source |
|---|---|---|
| `creative_groups.json` | [InventoryUtils.cs:78](../../InventoryUtils.cs#L78) | **GENERATED.** [CreativeGenerator.cs](../../../MiNET.BlockGen/CreativeGenerator.cs), invoked from [MiNET.BlockGen/Program.cs](../../../MiNET.BlockGen/Program.cs), reading `creative_items.json` from the BDS extraction. Stack user data is typed JSON NBT in the extraction's own shape, read by [TypedNbtJson](../../Utils/TypedNbtJson.cs). Rerun `dotnet run --project src/MiNET/MiNET.BlockGen` to refresh. Do not hand-edit. |
| `item_id_map.json` | [BlockFactory.cs:390](../../Blocks/BlockFactory.cs#L390), [LegacyItemUpgrader.cs:91](../LegacyItemUpgrader.cs#L91) | Committed data, not generated. Last content change 735f451b ("Initial 1.18.10 support"). Upstream origin is outside this repo's history. A duplicate copy sits unused in [Blocks/Data/](../../Blocks/Data/item_id_map.json). |
| `r16_to_current_item_map.json` | [ItemFactory.cs:125](../ItemFactory.cs#L125), [ItemFactory.cs:147](../ItemFactory.cs#L147) | pmmp/BedrockData legacy mapping (r16 = 1.16 item names to current, plus the registry's renames in its `simple` section). Committed data, not generated, not a submodule. pmmp lags Mojang releases, so it ages behind the item registry. |

## The proof

When frames captured off vanilla BDS are present under [MiNET.BlockGen/Captures](../../../MiNET.BlockGen/Captures), `MiNET.BlockGen` measures everything it emits here against them and prints every difference; the extraction is the source and is written either way, so a frame from an older build reports the version gap rather than blocking the newer data. [ItemRegistryCaptureTests](../../../MiNET.Test/ItemRegistryCaptureTests.cs) measures the compiled result against the same frames, which is the half a generator cannot check: the emitted construction code, the registry's serialization, and the typed JSON this folder stores. Those tests hold only while the frames and the extraction come from the same build.

A component leaf the extraction cannot read yet comes from the frame through a named gap in [ItemGenerator.Gaps](../../../MiNET.BlockGen/ItemGenerator.cs), and every run prints the list with a count and a reason. A gap disappears by itself once the extraction supplies the value.

## Rules

- After rerunning the extraction, rerun `MiNET.BlockGen`. That refreshes `creative_groups.json` and the generated .cs files. It does **not** touch the other two files here.
- `item_id_map.json` and `r16_to_current_item_map.json` have no generator, so a protocol bump leaves them behind with no error. Sourcing them is manual.
- Recipes are NOT here and are NOT generated from the extraction: see [../../Data/CLAUDE.md](../../Data/CLAUDE.md).
