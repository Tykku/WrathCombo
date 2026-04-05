using WrathCombo.CustomComboNS.Functions;
using WrathCombo.Extensions;
using WrathCombo.Resources.Localization.JobConfigs;
using static WrathCombo.Window.Functions.UserConfig;
using static WrathCombo.Window.Text;
namespace WrathCombo.Combos.PvE;

internal partial class BLM
{
    internal static class Config
    {
        internal static void Draw(Preset preset)
        {
            switch (preset)
            {
                #region ST

                case Preset.BLM_ST_Opener:
                    DrawHorizontalRadioButton(BLM_SelectedOpener,
                        Generics.StandardOpener, Generics.UsesStandardOpener,
                        0);

                    DrawHorizontalRadioButton(BLM_SelectedOpener,
                        FormatAndCache(Generics.Action_Opener, Flare.ActionName()), FormatAndCache(Generics.Use_0_Opener, Flare.ActionName()),
                        1);

                    DrawBossOnlyChoice(BLM_Balance_Content);
                    break;

                case Preset.BLM_ST_LeyLines:

                    DrawHorizontalRadioButton(BLM_ST_LeyLinesMovement,
                        Generics.StationaryOnly, FormatAndCache(Generics.UseActionOnlyWhileStationary, LeyLines.ActionName()), 0);

                    DrawHorizontalRadioButton(BLM_ST_LeyLinesMovement,
                        Generics.AnyMovement, FormatAndCache(Generics.Uses0RegardlessOfAnyMovementConditions, LeyLines.ActionName()), 1);

                    ImGui.Spacing();
                    if (BLM_ST_LeyLinesMovement == 0)
                    {
                        ImGui.SetCursorPosX(48);
                        DrawSliderFloat(0, 3, BLM_ST_LeyLinesTimeStill,
                            Generics.StationaryDelayCheck, decimals: 1);
                    }

                    ImGui.SetCursorPosX(48);
                    DrawSliderInt(0, 2, BLM_ST_LeyLinesCharges,
                        Generics.HowManyChargesToKeepReady);

                    DrawSliderInt(0, 50, BLM_ST_LeyLinesHPOption,
                        Generics.StopEnemyHpPercent);
                    ImGui.Indent();

                    DrawHorizontalRadioButton(BLM_ST_LeyLinesBossOption,
                        Generics.NonBosses, Generics.HPCheckNonBosses, 0);

                    DrawHorizontalRadioButton(BLM_ST_LeyLinesBossOption,
                        Generics.AllEnemies, Generics.HPCheckAllEnemies, 1);
                    break;

                case Preset.BLM_ST_Movement:
                    DrawHorizontalMultiChoice(BLM_ST_MovementOption,
                        FormatAndCache("use[0]", Despair.ActionName()), BLM_Config.UseDespairWhenBelow1500MP, 7, 0);

                    DrawPriorityInput(BLM_ST_MovementPriority,
                        7, 0, FormatAndCache(Generics.Action_Priority, Despair.ActionName()));

                    DrawHorizontalMultiChoice(BLM_ST_MovementOption,
                        FormatAndCache("Use {0}", Triplecast.ActionName()), BLM_Config.UseTriplecastWhenNoSwiftcast, 7, 1);

                    DrawPriorityInput(BLM_ST_MovementPriority,
                        7, 1, FormatAndCache(Generics.Action_Priority, Triplecast.ActionName()));

                    DrawHorizontalMultiChoice(BLM_ST_MovementOption,
                        FormatAndCache("Use {0}", Paradox.ActionName()), BLM_Config.UseParadoxWhenInAF3, 7, 2);

                    DrawPriorityInput(BLM_ST_MovementPriority,
                        7, 2, FormatAndCache(Generics.Action_Priority, Paradox.ActionName()));

                    DrawHorizontalMultiChoice(BLM_ST_MovementOption,
                        FormatAndCache("Use {0}", Role.Swiftcast.ActionName()), BLM_Config.UseSwiftcastWhenNoTriplecast, 7, 3);

                    DrawPriorityInput(BLM_ST_MovementPriority,
                        7, 3, FormatAndCache(Generics.Action_Priority, Role.Swiftcast.ActionName()));

                    DrawHorizontalMultiChoice(BLM_ST_MovementOption,
                        FormatAndCache(Generics.Use0Or1, Foul.ActionName(), Xenoglossy.ActionName()), BLM_Config.UseFoulOrXenoglossy, 7, 4);

                    DrawPriorityInput(BLM_ST_MovementPriority,
                        7, 4, FormatAndCache(Generics.Action_Priority, Xenoglossy.ActionName()));

                    DrawHorizontalMultiChoice(BLM_ST_MovementOption,
                        FormatAndCache("Use {0}", Fire3.ActionName()), BLM_Config.UseFire3WhenYouHaveFirestarterProc, 7, 5);

                    DrawPriorityInput(BLM_ST_MovementPriority,
                        7, 5, FormatAndCache(Generics.Action_Priority, Fire3.ActionName()));

                    DrawHorizontalMultiChoice(BLM_ST_MovementOption,
                        FormatAndCache("Use {0}", Scathe.ActionName()), BLM_Config.UseScathe, 7, 6);

                    DrawPriorityInput(BLM_ST_MovementPriority,
                        7, 6, FormatAndCache(Generics.Action_Priority, Scathe.ActionName()));
                    break;

                case Preset.BLM_ST_UsePolyglot:
                    if (DrawSliderInt(0, 3, BLM_ST_PolyglotSaveUsage,
                        BLM_Config.HowManyChargesForManualUse))
                        if (BLM_ST_PolyglotMovement > 3 - BLM_ST_PolyglotSaveUsage)
                            BLM_ST_PolyglotMovement.Value = 3 - BLM_ST_PolyglotSaveUsage;

                    if (DrawSliderInt(0, 3, BLM_ST_PolyglotMovement,
                        BLM_Config.HowManyChargesForMovement))
                        if (BLM_ST_PolyglotSaveUsage > 3 - BLM_ST_PolyglotMovement)
                            BLM_ST_PolyglotSaveUsage.Value = 3 - BLM_ST_PolyglotMovement;
                    break;

                case Preset.BLM_ST_Triplecast:
                    DrawHorizontalRadioButton(BLM_ST_Triplecast_WhenToUse,
                        Generics.Always, Generics.UseAlways, 0);

                    DrawHorizontalRadioButton(BLM_ST_Triplecast_WhenToUse,
                        BLM_Config.NotUnderLeylines, BLM_Config.DoNotUseUnderLeylines, 1);

                    if (BLM_ST_MovementOption[0])
                        DrawSliderInt(1, 2, BLM_ST_TriplecastMovementCharges,
                            BLM_Config.HowManyChargesForMovement);
                    break;


                case Preset.BLM_ST_Thunder:

                    DrawSliderInt(0, 100, BLM_ST_ThunderBossOption,
                        Generics.BossOnlyHpPercent);

                    DrawSliderInt(0, 100, BLM_ST_ThunderBossAddsOption,
                        Generics.BossEncounterNonBossHpPercent);

                    DrawSliderInt(0, 100, BLM_ST_ThunderTrashOption,
                        Generics.NonBossHpPercent);

                    DrawRoundedSliderFloat(0, 5, BLM_ST_ThunderRefresh,
                        Generics.DoTSecondsRemainingZeroDisable);
                    break;

                case Preset.BLM_ST_Manaward:
                    DrawSliderInt(0, 100, BLM_ST_ManawardHPThreshold,
                        BLM_Config.ManawardHPThreshold);
                    break;

                #endregion

                #region AoE

                case Preset.BLM_AoE_LeyLines:

                    DrawHorizontalRadioButton(BLM_AoE_LeyLinesMovement,
                        Generics.StationaryOnly, FormatAndCache(Generics.UseActionOnlyWhileStationary, LeyLines.ActionName()), 0);

                    DrawHorizontalRadioButton(BLM_AoE_LeyLinesMovement,
                        Generics.AnyMovement, FormatAndCache(Generics.Uses0RegardlessOfAnyMovementConditions, LeyLines.ActionName()), 1);

                    ImGui.Spacing();
                    if (BLM_AoE_LeyLinesMovement == 0)
                    {
                        ImGui.SetCursorPosX(48);
                        DrawSliderFloat(0, 3, BLM_AoE_LeyLinesTimeStill,
                            Generics.StationaryDelayCheck, decimals: 1);
                    }

                    ImGui.SetCursorPosX(48);
                    DrawSliderInt(0, 2, BLM_AoE_LeyLinesCharges,
                        Generics.HowManyChargesToKeepReady);

                    DrawSliderInt(0, 50, BLM_AoE_LeyLinesOption,
                        Generics.StopEnemyHpPercent);
                    break;

                case Preset.BLM_AoE_Triplecast:
                    DrawSliderInt(0, 1, BLM_AoE_TriplecastHoldCharges,
                        BLM_Config.HowManyChargesTriplecast);
                    break;

                case Preset.BLM_AoE_Thunder:
                    DrawSliderInt(0, 50, BLM_AoE_ThunderHP,
                        BLM_Config.StopUsingThunder2);
                    break;

                case Preset.BLM_Retargetting_Aetherial_Manipulation:
                    DrawAdditionalBoolChoice(BLM_AM_FieldMouseover,
                        Generics.FieldMouseover, Generics.AddFieldMouseoverTargetting);
                    break;

                #endregion

                #region Misc

                case Preset.BLM_Fire1and3:
                    DrawRadioButton(BLM_F1to3,
                        FormatAndCache("Replaces {0}", Fire.ActionName()), BLM_Config.ReplaceFireWithFire3WhenNotInAF3OrCombat, 0);

                    if (BLM_F1to3 == 0)
                    {
                        DrawAdditionalBoolChoice(BLM_Fire1_Despair,
                            FormatAndCache(Despair.ActionName()), BLM_Config.AddDespairWhenBelow2400MP);
                    }

                    DrawRadioButton(BLM_F1to3,
                        FormatAndCache("Replaces {0}", Fire3.ActionName()), BLM_Config.ReplaceFire3WithFireWhenInAF3, 1);
                    break;

                case Preset.BLM_Fire4:
                    DrawAdditionalBoolChoice(BLM_Fire4_FlareStar,
                        FormatAndCache(FlareStar.ActionName()), BLM_Config.AddFlarestarinAF);

                    DrawAdditionalBoolChoice(BLM_Fire4_Fire3,
                        FormatAndCache(Generics.Use0Or1, Fire.ActionName(), Fire3.ActionName()), BLM_Config.AddFireOrFire3WhenInAF);

                    DrawRadioButton(BLM_Fire4_FireAndIce,
                        FormatAndCache("Use {0} in Umbral Ice", Blizzard.ActionName()), BLM_Config.AddBlizzardOr3Or4DependingOnStackAndLevel, 0);

                    DrawRadioButton(BLM_Fire4_FireAndIce,
                        FormatAndCache("Use {0} in Umbral Ice", Fire.ActionName()), BLM_Config.DontChangeFireInUmbralIce, 1);
                    break;

                case Preset.BLM_Flare:
                    DrawAdditionalBoolChoice(BLM_Flare_FlareStar,
                        FormatAndCache(FlareStar.ActionName()), BLM_Config.AddFlarestarinAF);
                    break;

                case Preset.BLM_Blizzard1and3:
                    DrawRadioButton(BLM_B1to3,
                        FormatAndCache("Replaces {0}", Blizzard.ActionName()),
                        BLM_Config.ReplaceBlizzardWithBlizzard3WhenNotInUI3, 0);

                    DrawRadioButton(BLM_B1to3,
                        FormatAndCache("Replaces {0}", Blizzard3.ActionName()),
                        BLM_Config.ReplaceBlizzard3WithBlizzardWhenInUI3, 1);
                    if (BLM_B1to3 == 1)
                    {
                        DrawAdditionalBoolChoice(BLM_Blizzard3_Despair,
                            FormatAndCache(Despair.ActionName()), BLM_Config.AddDespairWhenInAFAndAbove800MP);
                    }
                    break;

                case Preset.BLM_AmplifierXeno:
                    DrawAdditionalBoolChoice(BLM_AmplifierXenoCD,
                        BLM_Config.ShowXenoglossyWhenAmplifierOnCD, BLM_Config.ShowXenoglossyWhenAmplifierIsOnCooldown);
                    break;

                #endregion
            }
        }

        #region Variables

        public static UserInt

            //ST
            BLM_SelectedOpener = new("BLM_SelectedOpener"),
            BLM_Balance_Content = new("BLM_Balance_Content", 1),
            BLM_ST_LeyLinesCharges = new("BLM_ST_LeyLinesCharges", 1),
            BLM_ST_LeyLinesMovement = new("BLM_ST_LeyLinesMovement"),
            BLM_ST_LeyLinesHPOption = new("BLM_ST_LeyLinesOption", 25),
            BLM_ST_LeyLinesBossOption = new("BLM_ST_LeyLinesSubOption"),
            BLM_ST_ThunderBossOption = new("BLM_ST_ThunderBossOption"),
            BLM_ST_ThunderBossAddsOption = new("BLM_ST_ThunderBossAddsOption", 10),
            BLM_ST_ThunderTrashOption = new("BLM_ST_ThunderTrashOption", 25),
            BLM_ST_Triplecast_WhenToUse = new("BLM_ST_Triplecast_WhenToUse", 1),
            BLM_ST_TriplecastMovementCharges = new("BLM_ST_TriplecastMovementCharges", 1),
            BLM_ST_PolyglotMovement = new("BLM_ST_PolyglotMovement", 1),
            BLM_ST_PolyglotSaveUsage = new("BLM_ST_PolyglotSaveUsage"),
            BLM_ST_ManawardHPThreshold = new("BLM_ST_ManawardHPThreshold", 25),

            //AoE
            BLM_AoE_TriplecastHoldCharges = new("BLM_AoE_TriplecastHoldCharges"),
            BLM_AoE_LeyLinesCharges = new("BLM_AoE_LeyLinesCharges"),
            BLM_AoE_LeyLinesMovement = new("BLM_AoE_LeyLinesMovement"),
            BLM_AoE_LeyLinesOption = new("BLM_AoE_LeyLinesOption", 25),
            BLM_AoE_ThunderHP = new("BLM_AoE_ThunderHP", 25),

            //Misc
            BLM_B1to3 = new("BLM_B1to3"),
            BLM_F1to3 = new("BLM_F1to3"),
            BLM_Fire4_FireAndIce = new("BLM_Fire4_FireAndIce");

        public static UserFloat
            BLM_ST_LeyLinesTimeStill = new("BLM_ST_LeyLinesTimeStill", 2.5f),
            BLM_AoE_LeyLinesTimeStill = new("BLM_AoE_LeyLinesTimeStill", 2.5f),
            BLM_ST_ThunderRefresh = new("BLM_ST_ThunderUptime_Threshold", 2.5f);

        public static UserBool
            BLM_AM_FieldMouseover = new("BLM_AM_FieldMouseover"),
            BLM_AmplifierXenoCD = new("BLM_AmplifierXenoCD"),
            BLM_Fire4_FlareStar = new("BLM_Fire4_FlareStar"),
            BLM_Flare_FlareStar = new("BLM_Flare_FlareStar"),
            BLM_Fire1_Despair = new("BLM_Fire1_Despair"),
            BLM_Blizzard3_Despair = new("BLM_Blizzard3_Despair"),
            BLM_Fire4_Fire3 = new("BLM_Fire4_Fire3");

        public static UserBoolArray
            BLM_ST_MovementOption = new("BLM_ST_MovementOption");

        public static UserIntArray
            BLM_ST_MovementPriority = new("BLM_ST_MovementPriority");

        #endregion
    }
}
