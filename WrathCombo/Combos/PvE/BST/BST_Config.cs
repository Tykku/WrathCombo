using WrathCombo.CustomComboNS.Functions;
using WrathCombo.Resources.Localization.JobConfigs;
using static WrathCombo.Window.Functions.UserConfig;

namespace WrathCombo.Combos.PvE;

internal partial class BST
{
    public static UserInt
        BST_Instinctual_TpGauge = new("BST_Instinctual_TpGauge", 100),
        BST_Intentional_TpGauge = new("BST_Intentional_TpGauge", 100);
    public static UserBool
        BST_SimpleMode_CycleBeasts = new("BST_SimpleMode_CycleBeasts", false),
        BST_Borrow_OnlyCurrentHorn = new("BST_Borrow_OnlyCurrentHorn", false);

    internal static class Config
    {
        internal static void Draw(Preset preset)
        {
            switch (preset)
            {
                case Preset.BST_SimpleMode:
                    DrawAdditionalBoolChoice(BST_SimpleMode_CycleBeasts, BST_Config.CycleBeasts, BST_Config.CycleBeastsDesc);
                    break;
                case Preset.BST_Instinctual_Combo:
                    DrawSliderInt(100, 250, BST_Instinctual_TpGauge, BST_Config.MinTPPlayerBeast, sliderIncrement: 10);
                    break;
                case Preset.BST_Intentional_Combo:
                    DrawSliderInt(100, 250, BST_Intentional_TpGauge, BST_Config.MinTPPlayerBeast, sliderIncrement: 10);
                    break;
                case Preset.BST_Borrow_Feature:
                    DrawAdditionalBoolChoice(BST_Borrow_OnlyCurrentHorn, BST_Config.OnlyCurrentHorn, BST_Config.OnlyCurrentHornDesc);
                    break;
            }
        }
    }
}