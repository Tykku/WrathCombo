using FFXIVClientStructs.FFXIV.Client.Game;
using System.Linq;
using WrathCombo.CustomComboNS;
using WrathCombo.Extensions;
using WrathCombo.Native;
using static WrathCombo.Combos.PvE.BST.Config;

namespace WrathCombo.Combos.PvE;

internal partial class BST : Melee
{
    internal class BST_Basic_Combo : CustomCombo
    {
        protected internal override Preset Preset => Preset.BST_Basic_Combo;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not SmashAxe)
                return actionID;

            if (ComboAction is SmashAxe && ActionReady(AxebladeBite))
                return AxebladeBite;

            if (ComboAction is AxebladeBite && ActionReady(Shieldsplitter))
                return Shieldsplitter;

            return SmashAxe;
        }
    }

    internal class BST_Instinctual_Combo : CustomCombo
    {
        protected internal override Preset Preset => Preset.BST_Instinctual_Combo;
        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Trick)
                return actionID;

            bool playerTpMet = JobGauge.PlayerTP >= BST_Instinctual_TpGauge;
            bool beastTpMet = JobGauge.BeastTP >= BST_Instinctual_TpGauge;

            if ((!playerTpMet || !beastTpMet) && !InInstinctualCombo)
                return All.Cease;

            if (ActionReady(TrickFollowUp))
                return TrickFollowUp;

            return actionID;
        }
    }
}
