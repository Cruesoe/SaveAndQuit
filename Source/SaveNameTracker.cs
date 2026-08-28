using RimWorld;
using Verse;

namespace SaveAndQuit;

/// <summary>
/// Remembers the file name this game saves to when quitting.
///
/// A named colony just uses its own name, so renaming the colony moves the save with it.
/// An unnamed one needs a generated name, and that has to be remembered - vanilla's
/// <see cref="SaveGameFilesUtility.UnusedDefaultFileName"/> hands out a fresh unused name
/// every call, which would leave a trail of Colony 1, Colony 2, ... behind.
/// </summary>
public class SaveNameTracker : GameComponent
{
    private string? generatedName;

    public SaveNameTracker(Game game)
    {
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref generatedName, "generatedName");
    }

    /// <summary>Whether there is a player faction to take a save name from yet.</summary>
    public static bool CanResolveSaveName()
    {
        return Current.Game?.World != null && Find.FactionManager != null && Faction.OfPlayer != null;
    }

    /// <summary>The file name to save under, matching what the vanilla save dialog would offer.</summary>
    public static string SaveFileName()
    {
        Faction player = Faction.OfPlayer;
        string name = player.HasName ? player.Name : GeneratedName(player);
        return GenFile.SanitizedFileName(name);
    }

    private static string GeneratedName(Faction player)
    {
        SaveNameTracker? tracker = Current.Game?.GetComponent<SaveNameTracker>();
        if (tracker == null)
        {
            return SaveGameFilesUtility.UnusedDefaultFileName(player.def.LabelCap);
        }

        if (tracker.generatedName.NullOrEmpty())
        {
            tracker.generatedName = SaveGameFilesUtility.UnusedDefaultFileName(player.def.LabelCap);
        }

        return tracker.generatedName!;
    }
}
