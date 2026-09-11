using Dalamud.Plugin.Services;
using ECommons.DalamudServices;
using System;
using System.Collections.Generic;
using WrathCombo.CustomComboNS;
using WrathCombo.Data;
using WrathCombo.Extensions;
using WrathCombo.Native;

namespace WrathCombo.Combos.PvE;

internal partial class BST : Melee
{
    internal class BST_SimpleMode : CustomCombo
    {
        protected internal override Preset Preset => Preset.BST_SimpleMode;
        protected override uint Invoke(uint actionID)
        {
            if (!CustomActionHelper.OneButtonRotationChecker(actionID, CustomActionType.SingleTargetDPS, SmashAxe))
                return actionID;

            if (!CurrentPetIsBMPet && InCombat())
            {
                if (ActionReady(FirstBattlehorn))
                    return FirstBattlehorn;

                if (ActionReady(SecondBattlehorn))
                    return SecondBattlehorn;

                if (ActionReady(ThirdBattlehorn))
                    return ThirdBattlehorn;
            }

            if (TargetIsBstPet(CurrentTarget) && !PetUnlocked(GetPetIdFromModel(CurrentTarget)) && !CurrentTarget!.HasStatus(Debuffs.InterestCaptured))
            {
                if (ActionReady(Capture))
                    return Capture;
            }

            if (!FinisherLearnt)
            {
                if (TPRestoredByStacks(JobGauge.MasterInstinct) < 250 && ActionReady(Rally))
                    return Rally;

                if (TPRestoredByStacks(JobGauge.PetInstinct) < 250 && ActionReady(RallyingCheer))
                    return RallyingCheer;
            }

            //Intentional > Instinctual
            if (AbleToIntentional)
            {
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

                if (CanStartInstinctualCombo || InInstinctualCombo)
                {
                    bool cantOmniDirection = !ActionLearned(ClockwiseInstinctualAction) || !ActionLearned(CounterClockwiseInstinctualAction);
                    if (LowestRallyType is RallyingType.None or RallyingType.Rally || (cantOmniDirection && !ActionLearned(CounterClockwiseInstinctualAction))) //Prioritize our stacks
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
                }
            }
            else if (ActionLearned(InstinctualComboAxe))
            {
                //Fallback to instinctual
                if (LowestRallyType is RallyingType.None or RallyingType.Rally) //Prioritize our stacks
                {
                    if (ActionReady(Trick))
                        return Trick;

                    if (ActionReady(InstinctualComboAxe))
                        return InstinctualComboAxe;
                }
                else
                {
                    if (ActionReady(InstinctualComboAxe))
                        return InstinctualComboAxe;

                    if (ActionReady(Trick))
                        return Trick;
                }

            }
            else if (InstinctualComboAxe == 0) //If somehow you're at level 4-7 with a pet that isn't Rampant
            {
                if (ActionReady(AvalancheAxe))
                    return AvalancheAxe;
            }

            if (ActionReady(TemperedRelease) && CanWeave() && CurrentPetReleaseAction != TemperedReleaseActions.Wespe_FinalSting)
                return TemperedRelease;

            if (BST_SimpleMode_CycleBeasts && TraitLevelChecked(Traits.WildHeartII) && !ActionReady(TemperedRelease) && ActionReady(PartingBlow) && !OnLastHorn && CanWeave())
                return PartingBlow;

            if (CanWeave() && InMeleeRange() && ActionReady(ShieldCharge) && GetRemainingCharges(ShieldCharge) > 1) //Save one for manual use
                return ShieldCharge;

            if (BasicCombo(ref actionID))
                return actionID;


            return OriginalHook(SmashAxe);
        }
    }

    internal class BST_Basic_Combo : CustomCombo
    {
        protected internal override Preset Preset => Preset.BST_Basic_Combo;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not AxebladeBite)
                return actionID;

            BasicCombo(ref actionID);
            return actionID;
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

            if ((!playerTpMet || !beastTpMet) && !InInstinctualCombo)
                return All.Cease;

            if (LowestRallyType is RallyingType.None or RallyingType.Rally) //Prioritize our stacks
            {
                if (ActionReady(Trick))
                    return Trick;

                if (ActionReady(InstinctualComboAxe))
                    return InstinctualComboAxe;
            }
            else
            {
                if (ActionReady(InstinctualComboAxe))
                    return InstinctualComboAxe;

                if (ActionReady(Trick))
                    return Trick;
            }

            return actionID;
        }
    }

    internal class BST_Intentional_Combo : CustomCombo
    {
        bool FinisherReady = false;
        public BST_Intentional_Combo()
        {
            Svc.Framework.Update += CheckForFinisher;
        }

        List<uint> FinisherActions = [];
        DateTime? TimeFinisherStarted;

        private void CheckForFinisher(IFramework framework)
        {
            if (!FinisherLearnt)
            {
                TimeFinisherStarted = null;
                FinisherReady = false;
                FinisherActions.Clear();
                return;
            }


            if (!FinisherReady)
            {
                if (JobGauge.MasterInstinct == 3 && JobGauge.PetInstinct >= 1 && ActionReady(Rally) && ActionReady(RallyingCheer) && JobGauge.PlayerTP >= 100 && JobGauge.BeastTP >= 100)
                {
                    FinisherReady = true;
                    FinisherActions.AddRange([Trick, CounterClockwiseInstinctualAction, Rally, RallyingCheer, Trick]);
                }
            }
            else
            {
                if (FinisherActions.Count == 0 || (TimeFinisherStarted.HasValue && (DateTime.Now - TimeFinisherStarted.Value).TotalSeconds > 10))
                {
                    TimeFinisherStarted = null;
                    FinisherReady = false;
                    FinisherActions.Clear();
                }

                if (FinisherActions.Count > 0 && ActionWatching.LastAction == FinisherActions[0])
                {
                    if (FinisherActions.Count == 5)
                        TimeFinisherStarted = DateTime.Now;

                    Svc.Log.Debug($"Removing {FinisherActions[0].ActionName()} from FinisherActions");
                    FinisherActions.RemoveAt(0);
                }
            }
        }

        protected internal override Preset Preset => Preset.BST_Intentional_Combo;
        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Trick)
                return actionID;

            bool playerTpMet = JobGauge.PlayerTP >= BST_Intentional_TpGauge;
            bool beastTpMet = JobGauge.BeastTP >= BST_Intentional_TpGauge;

            if (FinisherReady && FinisherActions.Count > 0)
            {
                if (FinisherActions.Count == 4 && !InInstinctualCombo)
                    return All.Cease;

                return FinisherActions[0];
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
