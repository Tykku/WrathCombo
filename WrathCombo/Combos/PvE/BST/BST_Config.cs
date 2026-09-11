using WrathCombo.CustomComboNS.Functions;
using static WrathCombo.Window.Functions.UserConfig;

namespace WrathCombo.Combos.PvE;

internal partial class BST
{
    public static UserInt
        BST_Instinctual_TpGauge = new("BST_Instinctual_TpGauge", 100),
        BST_Intentional_TpGauge = new("BST_Intentional_TpGauge", 100);
    public static UserBool
        BST_SimpleMode_CycleBeasts = new("BST_SimpleMode_CycleBeasts", false);

    internal static class Config
    {
        internal static void Draw(Preset preset)
        {
            switch (preset)
            {
                case Preset.BST_SimpleMode:
                    DrawAdditionalBoolChoice(BST_SimpleMode_CycleBeasts, "Cycle Beasts", "Will cycle beasts in combat. You may wish to do this manually.");
                    break;
                case Preset.BST_Instinctual_Combo:
                    DrawSliderInt(100, 250, BST_Instinctual_TpGauge, "Minimum TP for both player and beast", sliderIncrement: 10);
                    break;
                case Preset.BST_Intentional_Combo:
                    DrawSliderInt(100, 250, BST_Intentional_TpGauge, "Minimum TP for both player and beast", sliderIncrement: 10);
                    break;
            }
        }
    }
}