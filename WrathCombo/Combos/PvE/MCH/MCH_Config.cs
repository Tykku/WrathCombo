using Dalamud.Interface.Colors;
using WrathCombo.CustomComboNS.Functions;
using WrathCombo.Extensions;
using WrathCombo.Resources.Localization.JobConfigs;
using static WrathCombo.Window.Functions.UserConfig;
namespace WrathCombo.Combos.PvE;

internal partial class MCH
{
    internal static class Config
    {
        internal static void Draw(Preset preset)
        {
            switch (preset)
            {
                #region ST

                case Preset.MCH_ST_Adv_Opener:
                    DrawHorizontalRadioButton(MCH_SelectedOpener,
                        Generics.StandardOpener, "Uses Standard Lvl 100 opener", 0);

                    DrawHorizontalRadioButton(MCH_SelectedOpener,
                        $"Early {Wildfire.ActionName()} opener", $"Uses Early {Wildfire.ActionName()} Lvl 100 opener", 1);

                    DrawBossOnlyChoice(MCH_Balance_Content);
                    break;

                case Preset.MCH_ST_Adv_WildFire:
                    DrawHorizontalRadioButton(MCH_ST_WildfireBossOption,
                        Generics.AllContent, $"Use {Wildfire.ActionName()} regardless of content.", 0);

                    DrawHorizontalRadioButton(MCH_ST_WildfireBossOption,
                        Generics.BossOnlyContent, $"Only use {Wildfire.ActionName()} when the targeted enemy is a boss.", 1);

                    if (MCH_ST_WildfireBossOption == 0)
                    {
                        DrawSliderInt(0, 50, MCH_ST_WildfireHPOption,
                            Generics.StopEnemyHpPercent);

                        ImGui.Indent();

                        ImGui.TextColored(ImGuiColors.DalamudYellow,
                            Generics.EnemyTypeCheck);

                        DrawHorizontalRadioButton(MCH_ST_WildfireBossHPOption,
                            Generics.NonBosses, Generics.HPCheckNonBosses, 0);

                        DrawHorizontalRadioButton(MCH_ST_WildfireBossHPOption,
                            Generics.AllEnemies, Generics.HPCheckAllEnemies, 1);

                        ImGui.Unindent();
                    }
                    break;

                case Preset.MCH_ST_Adv_Stabilizer:
                    DrawHorizontalRadioButton(MCH_ST_BarrelStabilizerBossOption,
                        Generics.AllContent, $"Use {BarrelStabilizer.ActionName()} regardless of content.", 0);

                    DrawHorizontalRadioButton(MCH_ST_BarrelStabilizerBossOption,
                        Generics.BossOnlyContent, $"Only use {BarrelStabilizer.ActionName()} when the targeted enemy is a boss.", 1);

                    if (MCH_ST_BarrelStabilizerBossOption == 0)
                    {
                        DrawSliderInt(0, 50, MCH_ST_BarrelStabilizerHPOption,
                            Generics.StopEnemyHpPercent);

                        ImGui.Indent();

                        ImGui.TextColored(ImGuiColors.DalamudYellow,
                            Generics.EnemyTypeCheck);

                        DrawHorizontalRadioButton(MCH_ST_BarrelStabilizerHPBossOption,
                            Generics.NonBosses, Generics.HPCheckNonBosses, 0);

                        DrawHorizontalRadioButton(MCH_ST_BarrelStabilizerHPBossOption,
                            Generics.AllEnemies, Generics.HPCheckAllEnemies, 1);

                        ImGui.Unindent();
                    }
                    break;

                case Preset.MCH_ST_Adv_Hypercharge:
                    DrawSliderInt(0, 50, MCH_ST_HyperchargeHPOption,
                        Generics.StopEnemyHpPercent);

                    ImGui.Indent();

                    ImGui.TextColored(ImGuiColors.DalamudYellow,
                        Generics.EnemyTypeCheck);

                    DrawHorizontalRadioButton(MCH_ST_HyperchargeBossOption,
                        Generics.NonBosses, Generics.HPCheckNonBosses, 0);

                    DrawHorizontalRadioButton(MCH_ST_HyperchargeBossOption,
                        Generics.AllEnemies, Generics.HPCheckAllEnemies, 1);

                    ImGui.Unindent();
                    break;

                case Preset.MCH_ST_Adv_TurretQueen:
                    DrawSliderInt(50, 100, MCH_ST_TurretUsage,
                        $"Use {AutomatonQueen.ActionName()} at this battery threshold outside of Boss encounter.");

                    DrawSliderInt(0, 50, MCH_ST_QueenHPOption,
                        Generics.StopEnemyHpPercent);

                    ImGui.Indent();

                    ImGui.TextColored(ImGuiColors.DalamudYellow,
                        Generics.EnemyTypeCheck);

                    DrawHorizontalRadioButton(MCH_ST_QueenBossOption,
                        Generics.NonBosses, Generics.HPCheckNonBosses, 0);

                    DrawHorizontalRadioButton(MCH_ST_QueenBossOption,
                        Generics.AllEnemies, Generics.HPCheckAllEnemies, 1);


                    ImGui.Unindent();
                    break;

                case Preset.MCH_ST_Adv_GaussRicochet:
                    DrawSliderInt(0, 2, MCH_ST_GaussRicoPool,
                        Generics.ChargePool);
                    break;

                case Preset.MCH_ST_Adv_Reassemble:

                    DrawHorizontalRadioButton(MCH_ST_Adv_ReassembleChoice,
                        "Save for 2 minute windows", "Saves Reassemble for 2 minute windows\nTHIS WILL OVERCAP UR REASSEMBLE.", 0);

                    DrawHorizontalRadioButton(MCH_ST_Adv_ReassembleChoice,
                        "Use every minute", "Uses Reassemble every minute/whenever ur highest lvl tool is off cooldown.", 1);

                    DrawSliderInt(0, 50, MCH_ST_ReassembleHPOption,
                        Generics.StopEnemyHpPercent);

                    ImGui.Indent();

                    ImGui.TextColored(ImGuiColors.DalamudYellow,
                        Generics.EnemyTypeCheck);

                    DrawHorizontalRadioButton(MCH_ST_ReassembleBossOption,
                        Generics.NonBosses, Generics.HPCheckNonBosses, 0);

                    DrawHorizontalRadioButton(MCH_ST_ReassembleBossOption,
                        Generics.AllEnemies, Generics.HPCheckAllEnemies, 1);

                    ImGui.Unindent();

                    DrawSliderInt(0, 1, MCH_ST_ReassemblePool,
                        Generics.ChargePool);

                    break;

                case Preset.MCH_ST_Adv_Tools:

                    DrawSliderInt(0, 50, MCH_ST_ToolsHPOption,
                        Generics.StopEnemyHpPercent);

                    ImGui.Indent();

                    ImGui.TextColored(ImGuiColors.DalamudYellow,
                        Generics.EnemyTypeCheck);

                    DrawHorizontalRadioButton(MCH_ST_ToolsBossOption,
                        Generics.NonBosses, Generics.HPCheckNonBosses, 0);

                    DrawHorizontalRadioButton(MCH_ST_ToolsBossOption,
                        Generics.AllEnemies, Generics.HPCheckAllEnemies, 1);

                    ImGui.Unindent();
                    break;

                case Preset.MCH_ST_Adv_QueenOverdrive:
                    DrawSliderInt(0, 100, MCH_ST_QueenOverDriveHPThreshold,
                        Generics.StopFriendlyHpPercent100);
                    break;

                case Preset.MCH_ST_Adv_SecondWind:
                    DrawSliderInt(0, 100, MCH_ST_SecondWindHPThreshold,
                        $"{Role.SecondWind.ActionName()} HP percentage threshold");
                    break;

                #endregion

                #region AoE

                case Preset.MCH_AoE_Adv_Reassemble:
                    DrawSliderInt(0, 100, MCH_AoE_ReassembleHPThreshold,
                        $"Stop Using {Reassemble.ActionName()} When Target HP% is at or Below (Set to 0 to Disable This Check)");

                    DrawSliderInt(0, 2, MCH_AoE_ReassemblePool,
                        Generics.ChargePool);
                    break;

                case Preset.MCH_AoE_Adv_QueenOverdrive:
                    DrawSliderInt(0, 100, MCH_AoE_QueenOverDriveHPThreshold,
                        Generics.StopFriendlyHpPercent100);
                    break;

                case Preset.MCH_AoE_Adv_SecondWind:
                    DrawSliderInt(0, 100, MCH_AoE_SecondWindHPThreshold,
                        $"{Role.SecondWind.ActionName()} HP percentage threshold");
                    break;

                case Preset.MCH_AoE_Adv_Queen:
                    DrawSliderInt(0, 100, MCH_AoE_QueenHpThreshold,
                        $"Stop Using {RookAutoturret.ActionName()} When Target HP% is at or Below (Set to 0 to Disable This Check)");

                    DrawSliderInt(50, 100, MCH_AoE_TurretBatteryUsage,
                        "Battery threshold", sliderIncrement: 5);
                    break;

                case Preset.MCH_AoE_Adv_FlameThrower:

                    DrawHorizontalRadioButton(MCH_AoE_FlamethrowerMovement,
                        Generics.StationaryOnly, $"Use {Flamethrower.ActionName()} only while stationary", 0);

                    DrawHorizontalRadioButton(MCH_AoE_FlamethrowerMovement,
                        Generics.AnyMovement, $"Use {Flamethrower.ActionName()} regardless of any movement conditions.", 1);

                    ImGui.Spacing();
                    if (MCH_AoE_FlamethrowerMovement == 0)
                    {
                        ImGui.SetCursorPosX(48);
                        DrawSliderFloat(0, 3, MCH_AoE_FlamethrowerTimeStill,
                            Generics.StationaryDelayCheck, decimals: 1);
                    }

                    DrawSliderInt(0, 50, MCH_AoE_FlamethrowerHPOption,
                        Generics.StopEnemyHpPercent);
                    ImGui.Indent();
                    break;

                case Preset.MCH_AoE_Adv_Hypercharge:
                    DrawSliderInt(0, 100, MCH_AoE_HyperchargeHPThreshold,
                        $"Stop Using {Hypercharge.ActionName()} When Target HP% is at or Below (Set to 0 to Disable This Check)");
                    break;

                case Preset.MCH_AoE_Adv_Tools:
                    DrawSliderInt(0, 100, MCH_AoE_ToolsHPThreshold,
                        "Stop Using Tools When Target HP% is at or Below (Set to 0 to Disable This Check)");

                    DrawAdditionalBoolChoice(MCH_AoE_AirAnchor, "Add Hotshot / Air Anchor to tools.", "Also use Hotshot / Air Anchor on cooldown.");
                    break;

                case Preset.MCH_AoE_Adv_Stabilizer:
                    DrawSliderInt(0, 100, MCH_AoE_BarrelStabilizerHPThreshold,
                        $"Stop Using {BarrelStabilizer.ActionName()} When Target HP% is at or Below (Set to 0 to Disable This Check)");
                    break;

                #endregion

                #region Misc

                case Preset.MCH_GaussRoundRicochet:
                    DrawHorizontalRadioButton(MCH_GaussRico,
                        $"Change {GaussRound.ActionName()} / {DoubleCheck.ActionName()}", $"Changes to {Ricochet.ActionName()} / {CheckMate.ActionName()} depending on charges and what was used last", 0);

                    DrawHorizontalRadioButton(MCH_GaussRico,
                        $"Change {Ricochet.ActionName()} / {CheckMate.ActionName()}", $"Changes to {GaussRound.ActionName()} / {DoubleCheck.ActionName()} depending on charges and what was used last", 1);
                    break;

                case Preset.MCH_ST_Dismantle:
                    DrawSliderInt(0, 5, MCH_DismantledDuration,
                        "Time Remaining on Dismantled to allow within\n(0 = Dismantled must not be on the target)");
                    break;

                #endregion
            }
        }

        #region Variables

        public static UserInt

            //ST
            MCH_Balance_Content = new("MCH_Balance_Content", 1),
            MCH_SelectedOpener = new("MCH_SelectedOpener"),
            MCH_ST_QueenOverDriveHPThreshold = new("MCH_ST_QueenOverDrive", 1),
            MCH_ST_BarrelStabilizerBossOption = new("MCH_ST_BarrelStabilizerBossOption", 1),
            MCH_ST_BarrelStabilizerHPOption = new("MCH_ST_BarrelStabilizerHPOption", 10),
            MCH_ST_BarrelStabilizerHPBossOption = new("MCH_ST_BarrelStabilizerHPBossOption"),
            MCH_ST_WildfireBossOption = new("MCH_ST_WildfireBossOption", 1),
            MCH_ST_WildfireHPOption = new("MCH_ST_WildfireHPOption", 25),
            MCH_ST_WildfireBossHPOption = new("MCH_ST_WildfireBossHPOption"),
            MCH_ST_HyperchargeBossOption = new("MCH_ST_HyperchargeBossOption"),
            MCH_ST_HyperchargeHPOption = new("MCH_ST_HyperchargeHPOption", 25),
            MCH_ST_ReassembleBossOption = new("MCH_ST_ReassembleBossOption"),
            MCH_ST_Adv_ReassembleChoice = new("MCH_ST_Adv_ReassembleChoice"),
            MCH_ST_ReassembleHPOption = new("MCH_ST_ReassembleHPOption", 25),
            MCH_ST_ToolsBossOption = new("MCH_ST_ToolsBossOption"),
            MCH_ST_ToolsHPOption = new("MCH_ST_ToolsHPOption", 25),
            MCH_ST_QueenHPOption = new("MCH_ST_QueenHPOption", 25),
            MCH_ST_QueenBossOption = new("MCH_ST_QueenBossOption"),
            MCH_ST_TurretUsage = new("MCH_ST_TurretUsage", 100),
            MCH_ST_ReassemblePool = new("MCH_ST_ReassemblePool"),
            MCH_ST_GaussRicoPool = new("MCH_ST_GaussRicoPool"),
            MCH_ST_SecondWindHPThreshold = new("MCH_ST_SecondWindThreshold", 40),

            //AoE
            MCH_AoE_ReassemblePool = new("MCH_AoE_ReassemblePool"),
            MCH_AoE_TurretBatteryUsage = new("MCH_AoE_TurretUsage", 100),
            MCH_AoE_FlamethrowerMovement = new("MCH_AoE_FlamethrowerMovement"),
            MCH_AoE_FlamethrowerHPOption = new("MCH_AoE_FlamethrowerHPOption", 25),
            MCH_AoE_HyperchargeHPThreshold = new("MCH_AoE_HyperchargeHPThreshold", 25),
            MCH_AoE_ReassembleHPThreshold = new("MCH_AoE_ReassembleHPThreshold", 25),
            MCH_AoE_ToolsHPThreshold = new("MCH_AoE_ToolsHPThreshold", 25),
            MCH_AoE_QueenHpThreshold = new("MCH_AoE_QueenHpThreshold", 25),
            MCH_AoE_BarrelStabilizerHPThreshold = new("MCH_AoE_BarrelStabilizerHPThreshold", 25),
            MCH_AoE_QueenOverDriveHPThreshold = new("MCH_AoE_QueenOverDrive", 25),
            MCH_AoE_SecondWindHPThreshold = new("MCH_AoE_SecondWindThreshold", 40),

            //Misc
            MCH_GaussRico = new("MCHGaussRico"),
            MCH_DismantledDuration = new("MCH_DismantledDuration");

        public static UserFloat
            MCH_AoE_FlamethrowerTimeStill = new("MCH_AoE_FlamethrowerTimeStill", 2.5f);

        public static UserBool
            MCH_AoE_AirAnchor = new("MCH_AoE_AirAnchor");

        #endregion
    }
}
