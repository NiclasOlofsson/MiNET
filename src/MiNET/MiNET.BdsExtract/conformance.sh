#!/bin/bash
# Conformance: run both extractions (blocks, items) against each BDS version, every one
# brought to the canonical configuration in Assets first, so the only variable is the
# server build. Every guard must hold on every version; a BROKEN or FAILED verdict is the
# finding, not a nuisance. Outputs land under temp_auto/conformance/<version>/.
#
#   ./conformance.sh                  # the standing version list below
#   ./conformance.sh server-1.26.44   # just one
set -u
ROOT="$(cd "$(dirname "$0")/../../.." && pwd)"
TOOL="$ROOT/src/MiNET/MiNET.BdsExtract"
OUT="$ROOT/temp_auto/conformance"
mkdir -p "$OUT"

VERSIONS=("$@")
[ ${#VERSIONS[@]} -eq 0 ] && VERSIONS=(server-1.26.30 server-1.26.40-exp server-1.26.44 server-1.26.50.24 server-1.26.50.26)

stop_server() {
	powershell -NoProfile -Command "Get-CimInstance Win32_Process -Filter \"Name='bedrock_server.exe'\" | Where-Object { \$_.ExecutablePath -like '*\\$1\\*' } | ForEach-Object { Stop-Process -Id \$_.ProcessId -Force }" 2>/dev/null
}

dotnet build "$TOOL/MiNET.BdsExtract.csproj" > /dev/null || exit 1

for v in "${VERSIONS[@]}"; do
	dir="$ROOT/temp_auto/bds/$v"
	echo "=== $v ==="
	if [ ! -x "$dir/bedrock_server.exe" ]; then
		echo "    missing server folder: $dir"
		continue
	fi

	stop_server "$v"
	sleep 2

	if ! dotnet run --project "$TOOL" --no-build -- --prepare "$dir" > "$OUT/$v-prepare.log" 2>&1; then
		echo "    PREPARE FAILED (see $OUT/$v-prepare.log)"
		continue
	fi

	# BDS resolves every path against its working directory; start it from its own folder.
	(cd "$dir" && ./bedrock_server.exe > "$OUT/$v-server.log" 2>&1) &
	started=0
	for _ in $(seq 1 120); do
		if grep -q "Server started" "$OUT/$v-server.log" 2>/dev/null; then started=1; break; fi
		sleep 1
	done
	if [ "$started" != 1 ]; then
		echo "    SERVER FAILED TO START; last lines:"
		tail -5 "$OUT/$v-server.log" | sed 's/^/      /'
		stop_server "$v"
		continue
	fi

	mkdir -p "$OUT/$v"
	dotnet run --project "$TOOL" --no-build -- --server "$v" --out "$OUT/$v" > "$OUT/$v-blocks.log" 2>&1
	blocks=$?
	dotnet run --project "$TOOL" --no-build -- --items --server "$v" --out "$OUT/$v/items-runtime.json" > "$OUT/$v-items.log" 2>&1
	items=$?

	stop_server "$v"

	echo "    blocks exit $blocks, items exit $items"
	grep -E "^  (held|BROKEN|FAILED)" "$OUT/$v-blocks.log" | sed 's/^/    blocks /'
	grep -E "^  (held  |FAILED)" "$OUT/$v-items.log" | sed 's/^/    items  /'
done
