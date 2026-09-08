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
}
