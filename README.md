# Save and Quit

RimWorld 1.6 mod. Commitment mode already replaces the quit buttons with **Save and quit to menu** / **Save and quit to OS**, which save and exit with no prompt. This mod gives every colony those buttons.

- Quitting saves under your colony's name and overwrites the same file each time, so there is one save to come back to.
- No "Really quit? You'll lose unsaved progress." confirmation.
- The **Save** button is untouched, so save-as still works.
- If the colony has no name yet, a default name is generated once and then reused.
- Commitment mode colonies are left alone, and so is the **Quit to OS** button on the main menu.

Requires [Harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077). No DLC needed.

## Install

Copy this folder to `RimWorld\Mods\`, or add it as a local mod in RimSort.

## Build

```
dotnet build Source\SaveAndQuit.csproj -c Debug
```

The DLL is copied to `1.6\Assemblies\SaveAndQuit.dll` and to `RimWorld\Mods\Save and Quit\1.6\Assemblies\` if that folder exists.
