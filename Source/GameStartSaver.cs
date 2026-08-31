using RimWorld;
using Verse;

namespace SaveAndQuit;

/// <summary>
/// Saves a new colony as "Start" once it has finished arriving.
///
/// The wait is not a fixed delay. Ticks do not run while the scenario's intro
/// dialog is up, so counting only begins once the player dismisses it, and the
/// save then holds until no drop pod is still falling or waiting to open - that
/// is the point where the starting pawns are actually standing on the map.
/// </summary>
public class GameStartSaver : GameComponent
{
    public const string StartSaveName = "Start";

    // Settle time after the last pod clears, and a cap so a scenario that never
    // uses pods (or leaves one parked) still gets its save.
    private const int SettleTicks = 60;
    private const int MaxWaitTicks = 2500;

    private bool savePending;
    private int ticksWaited;
    private int ticksSettled;

    public GameStartSaver(Game game)
    {
    }

    public override void StartedNewGame()
    {
        if (!SaveAndQuitMod.Settings.saveOnGameStart)
        {
            return;
        }

        savePending = true;
        ticksWaited = 0;
        ticksSettled = 0;
    }

    public override void GameComponentTick()
    {
        if (!savePending)
        {
            return;
        }

        ticksWaited++;
        ticksSettled = ArrivalFinished() ? ticksSettled + 1 : 0;

        if (ticksSettled < SettleTicks && ticksWaited < MaxWaitTicks)
        {
            return;
        }

        if (GameDataSaveLoader.SavingIsTemporarilyDisabled)
        {
            return;
        }

        savePending = false;
        string fileName = GenFile.SanitizedFileName(StartSaveName);

        // Deferred rather than saved inline, the way vanilla's autosave does it.
        LongEventHandler.QueueLongEvent(delegate
        {
            GameDataSaveLoader.SaveGame(fileName);
            Messages.Message("SAQ.SavedStartFile".Translate(fileName), MessageTypeDefOf.SilentInput, false);
        }, "SavingLongEvent", false, null);
    }

    private static bool ArrivalFinished()
    {
        foreach (Map map in Find.Maps)
        {
            if (map.listerThings.ThingsOfDef(ThingDefOf.DropPodIncoming).Count > 0)
            {
                return false;
            }

            if (map.listerThings.ThingsOfDef(ThingDefOf.ActiveDropPod).Count > 0)
            {
                return false;
            }
        }

        return true;
    }
}
