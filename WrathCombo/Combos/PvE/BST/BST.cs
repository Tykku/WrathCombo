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
            if (!CustomActionHelper.OneButtonRotationChecker(actionID, CustomActionType.SingleTargetDPS, SmashAxe))
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
            if (actionID is not (AvalancheAxe or MistralAxe or SpinningAxe or GaleAxe))
                return actionID;

            bool playerTpMet = JobGauge.PlayerTP >= BST_Instinctual_TpGauge;
            bool beastTpMet = JobGauge.BeastTP >= BST_Instinctual_TpGauge;

            uint instinctualAction = TrickType switch
            {
                TrickTypes.Durant => MistralAxe,
                TrickTypes.Rampant => AvalancheAxe,
                TrickTypes.Eldritch => SpinningAxe,
                TrickTypes.Volant => GaleAxe,
                _ => 0
            };

            if ((!playerTpMet || !beastTpMet) && !InInstinctualCombo)
                return All.Cease;

            if (LowestRallyType is RallyingType.None or RallyingType.Rally) //Prioritize our stacks
            {
                if (ActionReady(Trick))
                    return Trick;

                if (ActionReady(instinctualAction))
                    return instinctualAction;
            }
            else
            {
                if (ActionReady(instinctualAction))
                    return instinctualAction;

                if (ActionReady(Trick))
                    return Trick;
            }

            return actionID;
        }
    }

    internal class BST_Intentional_Combo : CustomCombo
    {
        protected internal override Preset Preset => Preset.BST_Intentional_Combo;
        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Trick)
                return actionID;

            bool playerTpMet = JobGauge.PlayerTP >= BST_Intentional_TpGauge;
            bool beastTpMet = JobGauge.BeastTP >= BST_Intentional_TpGauge;

            if (FinisherLearnt && FinisherReady)
            {
                if (ActionReady(Trick))
                    return Trick;

                if (ActionReady(CounterClockwiseInstinctualAction))
                    return CounterClockwiseInstinctualAction;

                if (ActionReady(Rally))
                    return Rally;

                if (ActionReady(RallyingCheer))
                    return RallyingCheer;
            }

            if ((!playerTpMet || !beastTpMet) && !InInstinctualCombo)
                return All.Cease;

            if (JobGauge.ActiveAffinity is Data.InstinctualAffinity.Moonstalker)
            {
                if (ActionReady(RisenFall))
                    return RisenFall;
            }

            if (JobGauge.ActiveAffinity is Data.InstinctualAffinity.Sunstrider)
            {
                if (ActionReady(Calamity))
                    return Calamity;
            }

            if (LowestRallyType is RallyingType.None or RallyingType.Rally) //Prioritize our stacks
            {
                if (ActionReady(Trick))
                    return Trick;

                if (ActionReady(ClockwiseInstinctualAction))
                    return ClockwiseInstinctualAction;
            }
            else
            {
                if (ActionReady(CounterClockwiseInstinctualAction))
                    return CounterClockwiseInstinctualAction;

                if (ActionReady(Trick))
                    return Trick;
            }

            return actionID;
        }
    }

    internal class BST_Capture_Helper : CustomCombo
    {
        protected internal override Preset Preset => Preset.BST_Capture_Helper;
        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Capture)
                return actionID;

            var petId = GetPetIdFromModel(CurrentTarget);
            if (petId != 0)
            {
                if (!PetUnlocked(petId))
                    return Capture;
            }

            return All.Cease;
        }
    }
}
