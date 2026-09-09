using WrathCombo.CustomComboNS.Functions;
using static WrathCombo.Window.Functions.UserConfig;

namespace WrathCombo.Combos.PvE;

internal partial class BST
{
    public static UserInt
        BST_Instinctual_TpGauge = new("BST_Instinctual_TpGauge", 100);


    internal static class Config
    {
        internal static void Draw(Preset preset)
        {
            switch (preset)
            {
                case Preset.BST_Instinctual_Combo:
                    DrawSliderInt(100, 250, BST_Instinctual_TpGauge, "Minimum TP for both player and beast", sliderIncrement: 10);
                    break;
            }
        }
    }
}