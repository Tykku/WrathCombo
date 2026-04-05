using Dalamud.Interface.Colors;
using ECommons.ImGuiMethods;
using WrathCombo.CustomComboNS.Functions;
using WrathCombo.Extensions;
using WrathCombo.Window;
using WrathCombo.Window.Functions;
using static WrathCombo.Resources.Localization.JobConfigs.Generics;
namespace WrathCombo.Combos.PvE;

internal partial class All
{
    internal static class Config
    {
        internal static void Draw(Preset preset)
        {
            switch (preset)
            {
                case Preset.ALL_Tank_Reprisal:
                    UserConfig.DrawSliderInt(0, 9, AllTankReprisalThreshold,
                        Text.FormatAndCache(TimeRemainingOnOthers, Tank.Role.Reprisal.ActionName()));
                    break;

                case Preset.ALL_Caster_Addle:
                    UserConfig.DrawSliderInt(0, 5, AllCasterAddleThreshold,
                        Text.FormatAndCache(TimeRemainingOnOthers, Caster.Role.Addle.ActionName()));
                    break;

                case Preset.ALL_Melee_Feint:
                    UserConfig.DrawSliderInt(0, 5, AllMeleeFeintThreshold,
                        Text.FormatAndCache(TimeRemainingOnOthers, Melee.Role.Feint.ActionName()));
                    break;

                case Preset.ALL_Ranged_Mitigation:
                    UserConfig.DrawSliderInt(0, 5, AllRangedMitigationThreshold,
                        Text.FormatAndCache(TimeRemainingOnOthers3,
                            BRD.Troubadour.ActionName(), MCH.Tactician.ActionName(), DNC.ShieldSamba.ActionName()
                        ));
                    break;

                case Preset.ALL_Healer_RescueRetargeting:
                    ImGui.Indent();
                    ImGuiEx.TextWrapped(ImGuiColors.DalamudYellow, AllHealerRetargetting);
                    ImGui.Unindent();
                    UserConfig.DrawHorizontalMultiChoice(AllHealerRescueRetargetingOptions, FieldMouseover, string.Format(WillAdd_0_ToThePriorityStack, FieldMouseover), 3, 0);
                    UserConfig.DrawHorizontalMultiChoice(AllHealerRescueRetargetingOptions, FocusTarget, string.Format(WillAdd_0_ToThePriorityStack, FocusTarget), 3, 1);
                    UserConfig.DrawHorizontalMultiChoice(AllHealerRescueRetargetingOptions, SoftTarget, string.Format(WillAdd_0_ToThePriorityStack, SoftTarget), 3, 2);
                    break;
            }
        }

        #region Variables

        public static readonly UserInt
            AllTankReprisalThreshold = new("AllTankReprisalThreshold"),
            AllCasterAddleThreshold = new("AllCasterAddleThreshold"),
            AllMeleeFeintThreshold = new("AllMeleeFeintThreshold"),
            AllRangedMitigationThreshold = new("AllRangedMitigationThreshold");

        public static readonly UserBoolArray AllHealerRescueRetargetingOptions = new("ALL_Healer_RescueRetargetingOptions");

        #endregion
    }
}
