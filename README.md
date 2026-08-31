# Save and Quit

RimWorld 1.6 mod. Commitment mode already replaces the quit buttons with **Save and quit to menu** / **Save and quit to OS**, which save and exit with no prompt. This mod gives every colony those buttons.

- Quitting saves under your colony's name and overwrites the same file each time, so there is one save to come back to.
- No "Really quit? You'll lose unsaved progress." confirmation.
- The **Save** button is untouched, so save-as still works.
- If the colony has no name yet, a default name is generated once and then reused.
- Commitment mode colonies are left alone, and so is the **Quit to OS** button on the main menu.

## Optional "Start" save

Off by default. Enable it under **Options → Mod options → Save and Quit** and every new colony is saved as `Start` shortly after it begins.

The timing is not a fixed delay. Ticks do not run while the scenario intro is on screen, so the wait starts when you dismiss it, and the save then holds until no drop pod is still falling or waiting to open — the moment your starting pawns are actually on the map. Scenarios that use no pods fall through to a timeout instead.

Only new colonies are saved this way; loading an existing save never triggers it. Each new colony overwrites the previous `Start` file.

Requires [Harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077). No DLC needed.

## Install

Copy this folder to `RimWorld\Mods\`, or add it as a local mod in RimSort.

## Build

```
dotnet build Source\SaveAndQuit.csproj -c Debug
```

The DLL is copied to `1.6\Assemblies\SaveAndQuit.dll` and to `RimWorld\Mods\Save and Quit\1.6\Assemblies\` if that folder exists.
