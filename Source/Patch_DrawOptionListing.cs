using System;
using System.Collections.Generic;
using HarmonyLib;
using Verse;
using Verse.Profile;

namespace SaveAndQuit;

/// <summary>
/// Turns the in-game menu's quit options into the save-and-quit pair that commitment mode gets.
///
/// The options are rewritten on their way to be drawn rather than where they are built, so
/// anything else added to the menu is left alone. Commitment games are skipped - vanilla
/// already gives them these options, using their own permadeath save name.
/// </summary>
[HarmonyPatch(typeof(OptionListingUtility), nameof(OptionListingUtility.DrawOptionListing))]
public static class Patch_OptionListingUtility_DrawOptionListing
{
    private static readonly Action SaveAndQuitToMainMenuAction = SaveAndQuitToMainMenu;
    private static readonly Action SaveAndQuitToOSAction = SaveAndQuitToOS;

    private static LoadedLanguage? labelsCachedFor;
    private static string quitToMainMenu = string.Empty;
    private static string quitToOS = string.Empty;
    private static string saveAndQuitToMainMenu = string.Empty;
    private static string saveAndQuitToOS = string.Empty;

    public static void Prefix(List<ListableOption> optList)
    {
        if (!ShouldReplaceQuitOptions())
        {
            return;
        }

        RefreshLabels();

        foreach (ListableOption option in optList)
        {
            if (option.label == quitToMainMenu)
            {
                option.label = saveAndQuitToMainMenu;
                option.action = SaveAndQuitToMainMenuAction;
            }
            else if (option.label == quitToOS)
            {
                option.label = saveAndQuitToOS;
                option.action = SaveAndQuitToOSAction;
            }
        }
    }

    private static bool ShouldReplaceQuitOptions()
    {
        if (Current.ProgramState != ProgramState.Playing)
        {
            return false;
        }

        Game game = Current.Game;
        if (game?.Info == null || game.Info.permadeathMode)
        {
            return false;
        }

        if (GameDataSaveLoader.SavingIsTemporarilyDisabled)
        {
            return false;
        }

        return SaveNameTracker.CanResolveSaveName();
    }

    // The labels are matched in the active language, the same way vanilla built them.
    private static void RefreshLabels()
    {
        if (labelsCachedFor == LanguageDatabase.activeLanguage)
        {
            return;
        }

        labelsCachedFor = LanguageDatabase.activeLanguage;
        quitToMainMenu = "QuitToMainMenu".Translate();
        quitToOS = "QuitToOS".Translate();
        saveAndQuitToMainMenu = "SaveAndQuitToMainMenu".Translate();
        saveAndQuitToOS = "SaveAndQuitToOS".Translate();
    }

    private static void SaveAndQuitToMainMenu()
    {
        string fileName = SaveNameTracker.SaveFileName();
        LongEventHandler.QueueLongEvent(delegate
        {
            GameDataSaveLoader.SaveGame(fileName);
            MemoryUtility.ClearAllMapsAndWorld();
        }, "Entry", "SavingLongEvent", false, null);
    }

    private static void SaveAndQuitToOS()
    {
        string fileName = SaveNameTracker.SaveFileName();
        LongEventHandler.QueueLongEvent(delegate
        {
            GameDataSaveLoader.SaveGame(fileName);
            LongEventHandler.ExecuteWhenFinished(Root.Shutdown);
        }, "SavingLongEvent", false, null);
    }
}
