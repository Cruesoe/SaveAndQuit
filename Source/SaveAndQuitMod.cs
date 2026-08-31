using UnityEngine;
using Verse;

namespace SaveAndQuit;

public class SaveAndQuitMod : Mod
{
    public static SaveAndQuitSettings Settings = null!;

    public SaveAndQuitMod(ModContentPack content) : base(content)
    {
        Settings = GetSettings<SaveAndQuitSettings>();
    }

    public override string SettingsCategory()
    {
        return "SAQ.SettingsCategory".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Settings.DoWindowContents(inRect);
    }
}

public class SaveAndQuitSettings : ModSettings
{
    public bool saveOnGameStart;

    public void DoWindowContents(Rect inRect)
    {
        Listing_Standard list = new Listing_Standard();
        list.Begin(inRect);
        list.CheckboxLabeled("SAQ.SaveOnGameStart".Translate(), ref saveOnGameStart, "SAQ.SaveOnGameStartTip".Translate());
        list.End();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref saveOnGameStart, "saveOnGameStart", false);
    }
}
