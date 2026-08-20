import * as api from "@minecraft/server";
// Generated into this pack by MiNET.BlockGen from the server's own documentation, because the
// runtime's state list cannot tell a live state from one that predates the flattening.
import { WIRE_STATES } from "./wire-states.js";

// Extracts everything the runtime will say about every block, one block per two ticks.
//
// Most of it comes off a BlockPermutation and needs no world at all: tags, states, the four liquid
// answers, and the item form. Components do not: getComponents lives on a placed Block, so each
// block is set into a sealed pocket and read there. Light emission comes from the same placement,
// read off the neighbouring cell (one step of decay, so emission is that plus one).
//
// Results go to the log, one compact line per block. The debugger cannot be used to collect them:
// stopping at a breakpoint stops the very ticks the sweep runs on.
let mc = api;
let world = api.world;
let ticks = 0;

const Y = -58;

// One pocket. Light is not measured here at all: the memory side reads emission and dampening as
// the server stores them, per state, exactly, including the two values light propagation cannot
// tell apart. Placement is still needed for one thing only, components, which exist on a block that
// is really somewhere and nowhere else.
// The rooms move. Whatever the readings cost the server, it accumulates where the blocks land: it
// takes about four thousand placements in one spot to start crashing, and then it crashes every
// fifty or so for the rest of that world's life. Saving is per chunk, so the rooms move a chunk at
// a time and each one is used briefly and left alone.
const ROTATE_EVERY = 250;
let placements = 0;
let slot = -1;
let TEST = { x: 1, y: Y, z: 0 };

let work = [];
let index = 0;
let placing = true;
let started = false;
let finished = false;
let emitted = 0;
let beginAt = null;

function d() { return world.getDimension("overworld"); }

function fill(x0, x1, z0, z1, permutation) {
	for (let x = x0; x <= x1; x++)
		for (let y = Y - 2; y <= Y + 2; y++)
			for (let z = z0; z <= z1; z++)
				d().getBlock({ x, y, z })?.setPermutation(permutation);
}

function carve() {
	slot++;
	// A chunk each, inside the ticking area, with the lit room well clear of the dark one so its
	// lamp cannot reach it.
	// Nine chunks across, seven down, both rooms inside the one radius four area: anything wider
	// simply does not load, whatever the ticking area claims.
	const x0 = ((slot % 9) - 4) * 16 + 4;
	const z0 = ((Math.floor(slot / 9) % 7) - 4) * 16 + 2;
	TEST = { x: x0 + 1, y: Y, z: z0 };

	if (!d().getBlock(TEST)) { console.warn(`[BX] pocket at ${x0},${z0} is not loaded`); return; }

	const deepslate = mc.BlockPermutation.resolve("minecraft:deepslate");
	const air = mc.BlockPermutation.resolve("minecraft:air");

	fill(x0 - 1, x0 + 4, z0 - 2, z0 + 2, deepslate);
	d().getBlock(TEST).setPermutation(air);
}

function usePocket() {
	if (placements++ % ROTATE_EVERY === 0) carve();
}

function stateValues(id, p) {
	const out = {};
	for (const n of Object.keys(p.getAllStates())) {
		let st, domain;
		try { st = mc.BlockStates.get(n); domain = st && st.validValues; }
		catch (e) { out[n] = "no-state " + e; continue; }
		if (!domain || typeof domain.length !== "number") {
			// Say what the state type DOES carry, so a wrong property name shows itself once
			// rather than turning every block into ERR.
			let shape = "?";
			try { shape = Object.keys(st).concat(Object.getOwnPropertyNames(Object.getPrototypeOf(st) || {})).join("|"); } catch (e) {}
			out[n] = "no-domain [" + shape + "]";
			continue;
		}
		const good = [];
		for (let i = 0; i < domain.length; i++) {
			const v = domain[i];
			try { if (mc.BlockPermutation.resolve(id, { [n]: v }).getState(n) === v) good.push(v); }
			catch (e) { }
		}
		out[n] = good;
	}
	return out;
}

// Everything readable without placing the block.
function fromPermutation(id, p) {
	const row = { id };
	const grab = (key, fn) => { try { const v = fn(); if (v !== undefined) row[key] = v; } catch (e) { row[key] = "ERR"; } };

	grab("loc", () => p.localizationKey);
	grab("states", () => p.getAllStates());
	// Which of each state's declared values THIS block accepts. resolve falls back to the
	// state default for a value the block does not take, so comparing what comes back is an
	// exact test. The domain is the runtime's own registry, so nothing external is consulted.
	try { row.values = stateValues(id, p); } catch (e) { row.values = "ERR " + e; }
	grab("tags", () => p.getTags());
	grab("canContainLiquid", () => p.canContainLiquid("Water"));
	grab("isLiquidBlocking", () => p.isLiquidBlocking("Water"));
	grab("destroyedByLiquid", () => p.canBeDestroyedByLiquidSpread("Water"));
	grab("liquidCausesSpawn", () => p.liquidSpreadCausesSpawn("Water"));

	grab("item", () => {
		const it = p.getItemStack();
		if (!it) return undefined;
		return {
			maxAmount: it.maxAmount, stackable: it.isStackable, weight: it.weight,
			keepOnDeath: it.keepOnDeath, lockMode: it.lockMode,
			tags: it.getTags(), components: it.getComponents().map(c => c.typeId)
		};
	});
	return row;
}

// Everything that needs the block to exist somewhere.
function fromPlacedBlock(row, b) {
	const grab = (key, fn) => { try { const v = fn(); if (v !== undefined) row[key] = v; } catch (e) { row[key] = "ERR"; } };

	grab("isAir", () => b.isAir);
	grab("isLiquid", () => b.isLiquid);
	grab("isWaterlogged", () => b.isWaterlogged);
	grab("redstone", () => b.getRedstonePower());
	// Multi-block structures (doors, beds) report their pieces here; most blocks return nothing.
	grab("parts", () => { const p = b.getParts(); return p ? p.map(x => x.typeId ?? String(x)) : undefined; });

	// Read whatever each component exposes rather than a hardcoded list: every getter, and every
	// method that takes no arguments. Anything needing arguments is named but not called.
	const comps = {};
	const names = [];
	for (const c of b.getComponents()) {
		const cid = c.typeId ?? String(c);
		names.push(cid);
		const values = {};
		const proto = Object.getPrototypeOf(c);
		for (const m of Object.getOwnPropertyNames(proto)) {
			if (m === "constructor" || m.startsWith("__")) continue;
			const desc = Object.getOwnPropertyDescriptor(proto, m);
			try {
				if (desc.get) values[m] = c[m];
				else if (typeof desc.value === "function") {
					// A native binding reports length 0 whatever its real arity, so arity cannot be
					// used to decide whether a method is safe to call. Known argument shapes are
					// supplied here; anything else is named rather than called blind.
					if (m === "getInstrumentName") values[m] = c[m]("Up");
					else if (m === "getText" || m === "getRawText" || m === "getTextDyeColor") values[m] = c[m]("Front");
					else if (m.startsWith("set") || m.startsWith("play") || m.startsWith("add") || m.startsWith("eject") || m.startsWith("pause")) values[m] = "(mutator, not called)";
					else values[m] = c[m]();
				}
			} catch (e) { values[m] = "ERR: " + String(e).slice(0, 40); }
		}
		// An object value (a colour is {red,green,blue,alpha}) is flattened one level rather than
		// reduced to its class name, which would throw the numbers away.
		for (const k of Object.keys(values)) {
			const v = values[k];
			if (typeof v === "object" && v !== null && !Array.isArray(v)) {
				const flat = {};
				let any = false;
				for (const kk of Object.getOwnPropertyNames(v)) {
					try {
						const vv = v[kk];
						if (typeof vv !== "function") { flat[kk] = vv; any = true; }
					} catch (e) { /* getter that needs context */ }
				}
				values[k] = any ? flat : "[" + (v.constructor ? v.constructor.name : "obj") + "]";
			}
		}
		comps[cid] = values;
	}
	row.componentNames = names;
	row.components = comps;
}

// Every block state, in one pass, each one measured and reported in full.
function permutationsOf(id) {
	let base;
	try { base = mc.BlockPermutation.resolve(id); } catch (e) { return []; }

	const states = base.getAllStates();
	// Only the states that travel. Asking for a legacy state next to the one that replaced it is not
	// an error the engine reports, it is one it dies on.
	// Connection states are not set, they are worked out from what is next to the block. Forcing
	// them on a block sealed in stone asks for a shape it cannot hold, and the server dies rather
	// than refusing: every wall, fence and pane that killed a run died on exactly these. They are
	// still enumerated for the palette, they are simply not placed, and they cannot affect light.
	const names = (WIRE_STATES[id] || Object.keys(states))
		.filter(n => n in states)
		.filter(n => !n.includes("connection"));
	if (!names.length) return [{}];

	const axes = [];
	for (const n of names) {
		let domain;
		try { domain = mc.BlockStates.get(n).validValues; } catch (e) { return [{}]; }
		if (!domain || typeof domain.length !== "number") return [{}];
		const good = [];
		for (let i = 0; i < domain.length; i++) {
			const v = domain[i];
			try { if (mc.BlockPermutation.resolve(id, { [n]: v }).getState(n) === v) good.push(v); } catch (e) {}
		}
		if (!good.length) return [{}];
		axes.push([n, good]);
	}

	let out = [{}];
	for (const [n, vs] of axes) {
		const next = [];
		for (const acc of out) for (const v of vs) next.push(Object.assign({}, acc, { [n]: v }));
		out = next;
	}
	return out;
}

function buildWork() {
	work = [];
	for (const type of mc.BlockTypes.getAll()) {
		for (const states of permutationsOf(type.id)) work.push([type.id, states]);
	}
}

function step() {
	if (index >= work.length) {
		finished = true;
		console.warn(`[BX] END ${index} block states`);
		return;
	}

	const [id, states] = work[index];

	if (placing) {
		usePocket();
		try {
			d().getBlock(TEST).setPermutation(mc.BlockPermutation.resolve(id, states));
			placing = false;
		} catch (e) {
			console.warn(`[BX] ${JSON.stringify({ i: index, id, states, unplaceable: String(e).slice(0, 40) })}`);
			index++;
		}
		return;
	}

	// One complete row per block state. Nothing is remembered between rows and nothing is reported
	// only when it changes: whoever reads this should never have to know a fallback rule to find out
	// what a given state is, which is exactly the mistake that left every candle unlit.
	let row;
	try {
		const permutation = mc.BlockPermutation.resolve(id, states);
		row = fromPermutation(id, permutation);
		const b = d().getBlock(TEST);
		// Whether the block was still there when it was read. Liquids flow away and anything needing
		// support breaks on landing, and a reading taken off the air that replaced it is not a
		// reading of the block.
		row.stayed = !!(b && b.typeId === id);
		if (row.stayed) fromPlacedBlock(row, b);
	} catch (e) {
		row = { id, error: String(e).slice(0, 60) };
	}

	// The states that travel, as varied here. What the runtime reports on top of them is kept
	// separately rather than mixed in: it includes states that predate the flattening.
	row.allStates = row.states;
	row.states = states;
	row.i = index;

	console.warn(`[BX] ${JSON.stringify(row)}`);
	index++;
	placing = true;

	// Whether the world is filling up with debris is a fact worth having, not a hope.
	if (index % 500 === 0) {
		try {
			const items = d().getEntities({ type: "minecraft:item" }).length;
			const all = d().getEntities().length;
			console.warn(`[BX] entities: ${all}, of them items: ${items}`);
		} catch (e) { console.warn(`[BX] could not count entities: ${e}`); }
	}
}

api.system.afterEvents.scriptEventReceive.subscribe(event => {
	if (event.id !== "minet:begin") return;
	// One pass now, so the supervisor only has to say where in it to start.
	beginAt = parseInt(event.message.split(":").pop(), 10);
	console.warn(`[BX] BEGIN AT ${beginAt}`);
});

function probeTick() {
	ticks++;

	if (ticks === 3) {
		try { d().runCommand("tickingarea add circle 0 -58 0 4 probe"); }
		catch (e) { console.warn(`[BX] ticking area refused: ${e}`); }

		// Nothing here is worth keeping: the world is scaffolding for the readings. Holding the save
		// takes the chunk writes out of the picture.
		try { d().runCommand("save hold"); console.warn("[BX] saving held"); }
		catch (e) { console.warn(`[BX] could not hold the save: ${e}`); }

		// Everything the world does on its own is switched off. Most of it is simply waste here, but
		// tile drops are the one that matters: a great many of the blocks placed cannot stand alone,
		// they break the instant they land, and a break drops an item. Twenty thousand placements
		// leave a heap of entities behind, all of them ticked, which is the shape of a server that
		// runs well for thousands of placements and then degrades and never recovers.
		for (const rule of [
			"doTileDrops false",       // the one that matters: no item from a block that breaks
			"doEntityDrops false",
			"doMobSpawning false",
			"doMobLoot false",
			"doFireTick false",
			"doDayLightCycle false",
			"doWeatherCycle false",
			"randomTickSpeed 0",       // no crops growing, no fire spreading, no ice melting
			"mobGriefing false",
			"tntExplodes false",
			"doInsomnia false",
			"doLimitedCrafting false",
			"showCoordinates false",
			"sendCommandFeedback false",
			"commandBlockOutput false",
			"doImmediateRespawn true",
			"fallDamage false",
			"fireDamage false",
			"drowningDamage false",
			"freezeDamage false",
			"naturalRegeneration false"
		]) {
			try { d().runCommand(`gamerule ${rule}`); }
			catch (e) { console.warn(`[BX] gamerule ${rule} refused: ${e}`); }
		}
		console.warn("[BX] game rules off");
		return;
	}

	if (!started && ticks === 200) console.warn("[BX] the probe position never loaded; is the ticking area there?");

	if (!started) {
		if (ticks < 6 || !d().getBlock(TEST)) return;
		carve();
		buildWork();
		started = true;
		if (beginAt) index = beginAt;
		console.warn(`[BX] BEGIN ${work.length} block states from ${index}`);
		return;
	}

	if (!finished) step();
}

api.system.runInterval(probeTick, 1);
