using HarmonyLib;
using Verse;

namespace SaveAndQuit;

[StaticConstructorOnStartup]
public static class Startup
{
    static Startup()
    {
        new Harmony("cruesoe.saveandquit").PatchAll();
    }
}
