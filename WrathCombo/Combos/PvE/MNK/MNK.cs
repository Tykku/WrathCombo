using WrathCombo.Core;
using WrathCombo.CustomComboNS;
using static WrathCombo.Combos.PvE.MNK.Config;
namespace WrathCombo.Combos.PvE;

internal partial class MNK : Melee
{
    internal class MNK_ST_SimpleMode : CustomCombo
    {
        protected internal override Preset Preset => Preset.MNK_ST_SimpleMode;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not (Bootshine or LeapingOpo))
                return actionID;

            if (LevelChecked(SteeledMeditation) &&
                (!InCombat() || !InMeleeRange()) &&
                Chakra < 5 &&
                IsOriginal(MasterfulBlitz) &&
                !HasStatusEffect(Buffs.RiddleOfFire) &&
                !HasStatusEffect(Buffs.WindsRumination) &&
                !HasStatusEffect(Buffs.FiresRumination))
                return OriginalHook(SteeledMeditation);

            if (LevelChecked(FormShift) && !InCombat() &&
                !HasStatusEffect(Buffs.FormlessFist) &&
                !HasStatusEffect(Buffs.PerfectBalance) &&
                !HasStatusEffect(Buffs.OpoOpoForm) &&
                !HasStatusEffect(Buffs.RaptorForm) &&
                !HasStatusEffect(Buffs.CoeurlForm))
                return FormShift;

            //Variant Cure
            if (Variant.CanCure(Preset.MNK_Variant_Cure, MNK_VariantCure))
                return Variant.Cure;

            //Variant Rampart
            if (Variant.CanRampart(Preset.MNK_Variant_Rampart))
                return Variant.Rampart;

            if (ContentSpecificActions.TryGet(out uint contentAction))
                return contentAction;

            // OGCDs
            if (CanWeave())
            {
                if (UsePerfectBalanceST())
                    return PerfectBalance;

                if (UseBrotherhood())
                    return Brotherhood;

                if (UseRoF())
                    return RiddleOfFire;

                if (UseRoW())
                    return RiddleOfWind;

                if (Role.CanSecondWind(25))
                    return Role.SecondWind;

                if (Role.CanBloodBath(40))
                    return Role.Bloodbath;

                if (Chakra >= 5 && InCombat() && LevelChecked(SteeledMeditation) &&
                    !JustUsed(Brotherhood) && !JustUsed(RiddleOfFire) &&
                    InActionRange(OriginalHook(SteeledMeditation)))
                    return OriginalHook(SteeledMeditation);
            }

            // GCDs
            if (HasStatusEffect(Buffs.FormlessFist))
                return OpoOpo is 0
                    ? DragonKick
                    : OriginalHook(Bootshine);

            // Masterful Blitz
            if (LevelChecked(MasterfulBlitz) &&
                !HasStatusEffect(Buffs.PerfectBalance) &&
                InMasterfulRange() && !IsOriginal(MasterfulBlitz))
                return OriginalHook(MasterfulBlitz);

            if (HasStatusEffect(Buffs.WindsRumination) &&
                (GetCooldownRemainingTime(RiddleOfFire) > 5 ||
                 HasStatusEffect(Buffs.RiddleOfFire) ||
                 GetStatusEffectRemainingTime(Buffs.WindsRumination) < GCD * 2 ||
                 !InMeleeRange()))
                return WindsReply;

            if (HasStatusEffect(Buffs.FiresRumination) &&
                !HasStatusEffect(Buffs.FormlessFist) &&
                !HasStatusEffect(Buffs.PerfectBalance) &&
                IsOriginal(MasterfulBlitz) &&
                (JustUsed(OriginalHook(Bootshine)) ||
                 JustUsed(DragonKick) ||
                 GetStatusEffectRemainingTime(Buffs.FiresRumination) < GCD * 2 ||
                 !InMeleeRange()))
                return FiresReply;

            // Perfect Balance or Standard Beast Chakras
            return DoPerfectBalanceComboST(ref actionID)
                ? actionID
                : DetermineCoreAbility(actionID, true);
        }
    }

    internal class MNK_AOE_SimpleMode : CustomCombo
    {
        protected internal override Preset Preset => Preset.MNK_AOE_SimpleMode;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not (ArmOfTheDestroyer or ShadowOfTheDestroyer))
                return actionID;

            if (LevelChecked(InspiritedMeditation) &&
                (!InCombat() || !InMeleeRange()) &&
                Chakra < 5 &&
                IsOriginal(MasterfulBlitz) &&
                !HasStatusEffect(Buffs.RiddleOfFire) &&
                !HasStatusEffect(Buffs.WindsRumination) &&
                !HasStatusEffect(Buffs.FiresRumination))
                return OriginalHook(InspiritedMeditation);

            if (LevelChecked(FormShift) && !InCombat() &&
                !HasStatusEffect(Buffs.FormlessFist) &&
                !HasStatusEffect(Buffs.PerfectBalance) &&
                !HasStatusEffect(Buffs.OpoOpoForm) &&
                !HasStatusEffect(Buffs.RaptorForm) &&
                !HasStatusEffect(Buffs.CoeurlForm))
                return FormShift;

            //Variant Cure
            if (Variant.CanCure(Preset.MNK_Variant_Cure, MNK_VariantCure))
                return Variant.Cure;

            //Variant Rampart
            if (Variant.CanRampart(Preset.MNK_Variant_Rampart))
                return Variant.Rampart;

            if (ContentSpecificActions.TryGet(out uint contentAction))
                return contentAction;

            // OGCD's
            if (CanWeave())
            {
                if (UsePerfectBalanceAoE())
                    return PerfectBalance;

                if (UseBrotherhood())
                    return Brotherhood;

                if (UseRoF())
                    return RiddleOfFire;

                if (UseRoW())
                    return RiddleOfWind;

                if (Role.CanSecondWind(25))
                    return Role.SecondWind;

                if (Role.CanBloodBath(40))
                    return Role.Bloodbath;

                if (Chakra >= 5 &&
                    LevelChecked(InspiritedMeditation) &&
                    HasBattleTarget() && InCombat() &&
                    !JustUsed(Brotherhood) && !JustUsed(RiddleOfFire) &&
                    InActionRange(OriginalHook(InspiritedMeditation)))
                    return OriginalHook(InspiritedMeditation);
            }

            // Masterful Blitz
            if (LevelChecked(MasterfulBlitz) &&
                !HasStatusEffect(Buffs.PerfectBalance) &&
                InMasterfulRange() &&
                !IsOriginal(MasterfulBlitz))
                return OriginalHook(MasterfulBlitz);

            if (HasStatusEffect(Buffs.FiresRumination) &&
                !HasStatusEffect(Buffs.PerfectBalance) &&
                !HasStatusEffect(Buffs.FormlessFist) &&
                !JustUsed(RiddleOfFire, 4))
                return FiresReply;

            if (HasStatusEffect(Buffs.WindsRumination) &&
                !HasStatusEffect(Buffs.PerfectBalance) &&
                (GetCooldownRemainingTime(RiddleOfFire) > 10 ||
                 HasStatusEffect(Buffs.RiddleOfFire)))
                return WindsReply;

            // Perfect Balance
            if (DoPerfectBalanceComboAoE(ref actionID))
                return actionID;

            // Monk Rotation
            if (HasStatusEffect(Buffs.OpoOpoForm))
                return OriginalHook(ArmOfTheDestroyer);

            if (HasStatusEffect(Buffs.RaptorForm))
            {
                if (LevelChecked(FourPointFury))
                    return FourPointFury;

                if (LevelChecked(TwinSnakes))
                    return TwinSnakes;
            }

            if (HasStatusEffect(Buffs.CoeurlForm) && LevelChecked(Rockbreaker))
                return Rockbreaker;

            return actionID;
        }
    }

    internal class MNK_ST_AdvancedMode : CustomCombo
    {
        protected internal override Preset Preset => Preset.MNK_ST_AdvancedMode;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not (Bootshine or LeapingOpo))
                return actionID;

            if (IsEnabled(Preset.MNK_STUseOpener) &&
                Opener().FullOpener(ref actionID))
                return Opener().OpenerStep >= 9 &&
                       CanWeave() &&
                       Chakra >= 5
                    ? TheForbiddenChakra
                    : actionID;

            if (IsEnabled(Preset.MNK_STUseMeditation) &&
                LevelChecked(SteeledMeditation) &&
                (!InCombat() || !InMeleeRange()) &&
                Chakra < 5 &&
                IsOriginal(MasterfulBlitz) &&
                !HasStatusEffect(Buffs.RiddleOfFire) &&
                !HasStatusEffect(Buffs.WindsRumination) &&
                !HasStatusEffect(Buffs.FiresRumination))
                return OriginalHook(SteeledMeditation);

            if (IsEnabled(Preset.MNK_STUseFormShift) &&
                LevelChecked(FormShift) && !InCombat() &&
                !HasStatusEffect(Buffs.FormlessFist) &&
                !HasStatusEffect(Buffs.PerfectBalance) &&
                !HasStatusEffect(Buffs.OpoOpoForm) &&
                !HasStatusEffect(Buffs.RaptorForm) &&
                !HasStatusEffect(Buffs.CoeurlForm))
                return FormShift;

            //Variant Cure
            if (Variant.CanCure(Preset.MNK_Variant_Cure, MNK_VariantCure))
                return Variant.Cure;

            //Variant Rampart
            if (Variant.CanRampart(Preset.MNK_Variant_Rampart))
                return Variant.Rampart;

            if (ContentSpecificActions.TryGet(out uint contentAction))
                return contentAction;

            // OGCDs
            if (CanWeave() && M6SReady)
            {
                if (IsEnabled(Preset.MNK_STUsePerfectBalance) &&
                    UsePerfectBalanceST())
                    return PerfectBalance;

                if (IsEnabled(Preset.MNK_STUseBuffs))
                {
                    if (IsEnabled(Preset.MNK_STUseBrotherhood) &&
                        UseBrotherhood() &&
                        (MNK_ST_BrotherhoodBossOption == 0 || InBossEncounter()))
                        return Brotherhood;

                    if (IsEnabled(Preset.MNK_STUseROF) &&
                        UseRoF() &&
                        (MNK_ST_RiddleOfFireBossOption == 0 || InBossEncounter()))
                        return RiddleOfFire;

                    if (IsEnabled(Preset.MNK_STUseROW) &&
                        UseRoW() &&
                        (MNK_ST_RiddleOfWindBossOption == 0 || InBossEncounter()))
                        return RiddleOfWind;
                }

                if (IsEnabled(Preset.MNK_ST_Feint) &&
                    Role.CanFeint() &&
                    RaidWideCasting())
                    return Role.Feint;

                if (IsEnabled(Preset.MNK_ST_ComboHeals))
                {
                    if (Role.CanSecondWind(MNK_ST_SecondWindHPThreshold))
                        return Role.SecondWind;

                    if (Role.CanBloodBath(MNK_ST_BloodbathHPThreshold))
                        return Role.Bloodbath;
                }

                if (IsEnabled(Preset.MNK_ST_StunInterupt) &&
                    RoleActions.Melee.CanLegSweep() &&
                    !TargetIsBoss() && TargetIsCasting())
                    return Role.LegSweep;

                if (IsEnabled(Preset.MNK_STUseTheForbiddenChakra) &&
                    Chakra >= 5 && InCombat() &&
                    LevelChecked(SteeledMeditation) &&
                    !JustUsed(Brotherhood) && !JustUsed(RiddleOfFire) &&
                    InActionRange(OriginalHook(SteeledMeditation)))
                    return OriginalHook(SteeledMeditation);
            }

            // GCDs
            if (HasStatusEffect(Buffs.FormlessFist))
                return OpoOpo is 0
                    ? DragonKick
                    : OriginalHook(Bootshine);

            // Masterful Blitz
            if (IsEnabled(Preset.MNK_STUseMasterfulBlitz) &&
                LevelChecked(MasterfulBlitz) &&
                !HasStatusEffect(Buffs.PerfectBalance) && InMasterfulRange() &&
                !IsOriginal(MasterfulBlitz))
                return OriginalHook(MasterfulBlitz);

            if (IsEnabled(Preset.MNK_STUseBuffs))
            {
                if (IsEnabled(Preset.MNK_STUseWindsReply) &&
                    HasStatusEffect(Buffs.WindsRumination) &&
                    (GetCooldownRemainingTime(RiddleOfFire) > 5 ||
                     HasStatusEffect(Buffs.RiddleOfFire) ||
                     GetStatusEffectRemainingTime(Buffs.WindsRumination) < GCD * 2 ||
                     !InMeleeRange()))
                    return WindsReply;

                if (IsEnabled(Preset.MNK_STUseFiresReply) &&
                    HasStatusEffect(Buffs.FiresRumination) &&
                    !HasStatusEffect(Buffs.FormlessFist) &&
                    !HasStatusEffect(Buffs.PerfectBalance) &&
                    IsOriginal(MasterfulBlitz) &&
                    (JustUsed(OriginalHook(Bootshine)) ||
                     JustUsed(DragonKick) ||
                     GetStatusEffectRemainingTime(Buffs.FiresRumination) < GCD * 2 ||
                     !InMeleeRange()))
                    return FiresReply;
            }

            // Perfect Balance or Standard Beast Chakras
            return DoPerfectBalanceComboST(ref actionID)
                ? actionID
                : DetermineCoreAbility(actionID, IsEnabled(Preset.MNK_STUseTrueNorth));
        }
    }

    internal class MNK_AOE_AdvancedMode : CustomCombo
    {
        protected internal override Preset Preset => Preset.MNK_AOE_AdvancedMode;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not (ArmOfTheDestroyer or ShadowOfTheDestroyer))
                return actionID;

            if (IsEnabled(Preset.MNK_AoEUseMeditation) &&
                LevelChecked(InspiritedMeditation) &&
                (!InCombat() || !InMeleeRange()) &&
                Chakra < 5 &&
                IsOriginal(MasterfulBlitz) &&
                !HasStatusEffect(Buffs.RiddleOfFire) &&
                !HasStatusEffect(Buffs.WindsRumination) &&
                !HasStatusEffect(Buffs.FiresRumination))
                return OriginalHook(InspiritedMeditation);

            if (IsEnabled(Preset.MNK_AoEUseFormShift) &&
                LevelChecked(FormShift) && !InCombat() &&
                !HasStatusEffect(Buffs.FormlessFist) &&
                !HasStatusEffect(Buffs.PerfectBalance) &&
                !HasStatusEffect(Buffs.OpoOpoForm) &&
                !HasStatusEffect(Buffs.RaptorForm) &&
                !HasStatusEffect(Buffs.CoeurlForm))
                return FormShift;

            //Variant Cure
            if (Variant.CanCure(Preset.MNK_Variant_Cure, MNK_VariantCure))
                return Variant.Cure;

            //Variant Rampart
            if (Variant.CanRampart(Preset.MNK_Variant_Rampart))
                return Variant.Rampart;

            if (ContentSpecificActions.TryGet(out uint contentAction))
                return contentAction;

            // OGCD's 
            if (CanWeave() && M6SReady)
            {
                if (IsEnabled(Preset.MNK_AoEUsePerfectBalance) &&
                    UsePerfectBalanceAoE())
                    return PerfectBalance;

                if (IsEnabled(Preset.MNK_AoEUseBuffs))
                {
                    if (IsEnabled(Preset.MNK_AoEUseBrotherhood) &&
                        UseBrotherhood() &&
                        GetTargetHPPercent() >= MNK_AoE_BrotherhoodHPThreshold)
                        return Brotherhood;

                    if (IsEnabled(Preset.MNK_AoEUseROF) &&
                        UseRoF() &&
                        GetTargetHPPercent() >= MNK_AoE_RiddleOfFireHPTreshold)
                        return RiddleOfFire;

                    if (IsEnabled(Preset.MNK_AoEUseROW) &&
                        UseRoW() &&
                        GetTargetHPPercent() >= MNK_AoE_RiddleOfWindHPTreshold)
                        return RiddleOfWind;
                }

                if (IsEnabled(Preset.MNK_AoE_ComboHeals))
                {
                    if (Role.CanSecondWind(MNK_AoE_SecondWindHPThreshold))
                        return Role.SecondWind;

                    if (Role.CanBloodBath(MNK_AoE_BloodbathHPThreshold))
                        return Role.Bloodbath;
                }

                if (IsEnabled(Preset.MNK_AoE_StunInterupt) &&
                    RoleActions.Melee.CanLegSweep() &&
                    !TargetIsBoss() && TargetIsCasting())
                    return Role.LegSweep;

                if (IsEnabled(Preset.MNK_AoEUseHowlingFist) &&
                    Chakra >= 5 && HasBattleTarget() && InCombat() &&
                    LevelChecked(InspiritedMeditation) &&
                    !JustUsed(Brotherhood) && !JustUsed(RiddleOfFire) &&
                    InActionRange(OriginalHook(InspiritedMeditation)))
                    return OriginalHook(InspiritedMeditation);
            }

            // Masterful Blitz
            if (IsEnabled(Preset.MNK_AoEUseMasterfulBlitz) &&
                LevelChecked(MasterfulBlitz) &&
                !HasStatusEffect(Buffs.PerfectBalance) &&
                InMasterfulRange() &&
                !IsOriginal(MasterfulBlitz))
                return OriginalHook(MasterfulBlitz);

            if (IsEnabled(Preset.MNK_AoEUseBuffs))
            {
                if (IsEnabled(Preset.MNK_AoEUseFiresReply) &&
                    HasStatusEffect(Buffs.FiresRumination) &&
                    !HasStatusEffect(Buffs.FormlessFist) &&
                    !HasStatusEffect(Buffs.PerfectBalance) &&
                    !JustUsed(RiddleOfFire, 4))
                    return FiresReply;

                if (IsEnabled(Preset.MNK_AoEUseWindsReply) &&
                    HasStatusEffect(Buffs.WindsRumination) &&
                    !HasStatusEffect(Buffs.PerfectBalance) &&
                    (GetCooldownRemainingTime(RiddleOfFire) > 10 ||
                     HasStatusEffect(Buffs.RiddleOfFire)))
                    return WindsReply;
            }

            // Perfect Balance
            if (DoPerfectBalanceComboAoE(ref actionID))
                return actionID;

            // Monk Rotation
            if (HasStatusEffect(Buffs.OpoOpoForm))
                return OriginalHook(ArmOfTheDestroyer);

            if (HasStatusEffect(Buffs.RaptorForm))
            {
                if (LevelChecked(FourPointFury))
                    return FourPointFury;

                if (LevelChecked(TwinSnakes))
                    return TwinSnakes;
            }

            if (HasStatusEffect(Buffs.CoeurlForm) && LevelChecked(Rockbreaker))
                return Rockbreaker;

            return actionID;
        }
    }

    internal class MNK_BeastChakras : CustomCombo
    {
        protected internal override Preset Preset => Preset.MNK_ST_BeastChakras;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not (Bootshine or LeapingOpo or TrueStrike or RisingRaptor or SnapPunch or PouncingCoeurl))
                return actionID;

            if (MNK_BasicCombo[0] &&
                actionID is Bootshine or LeapingOpo)
                return OpoOpo is 0 && LevelChecked(DragonKick)
                    ? DragonKick
                    : OriginalHook(Bootshine);

            if (MNK_BasicCombo[1] &&
                actionID is TrueStrike or RisingRaptor)
                return Raptor is 0 && LevelChecked(TwinSnakes)
                    ? TwinSnakes
                    : OriginalHook(TrueStrike);

            if (MNK_BasicCombo[2] &&
                actionID is SnapPunch or PouncingCoeurl)
                return Coeurl is 0 && LevelChecked(Demolish)
                    ? Demolish
                    : OriginalHook(SnapPunch);

            return actionID;
        }
    }

    internal class MNK_Retarget_Thunderclap : CustomCombo
    {
        protected internal override Preset Preset => Preset.MNK_Retarget_Thunderclap;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Thunderclap)
                return actionID;

            return MNK_Thunderclap_FieldMouseover
                ? Thunderclap.Retarget(SimpleTarget.UIMouseOverTarget ?? SimpleTarget.ModelMouseOverTarget ?? SimpleTarget.HardTarget, true)
                : Thunderclap.Retarget(SimpleTarget.UIMouseOverTarget ?? SimpleTarget.HardTarget, true);
        }
    }

    internal class MNK_PerfectBalance : CustomCombo
    {
        protected internal override Preset Preset => Preset.MNK_PerfectBalance;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not PerfectBalance)
                return actionID;

            return OriginalHook(MasterfulBlitz) != MasterfulBlitz &&
                   LevelChecked(MasterfulBlitz)
                ? OriginalHook(MasterfulBlitz)
                : actionID;
        }
    }

    internal class MNK_Brotherhood_Riddle : CustomCombo
    {
        protected internal override Preset Preset => Preset.MNK_Brotherhood_Riddle;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not (Brotherhood or RiddleOfFire))
                return actionID;

            return actionID switch
            {
                Brotherhood when MNK_BH_RoF == 0 && ActionReady(RiddleOfFire) && IsOnCooldown(Brotherhood) => OriginalHook(RiddleOfFire),
                RiddleOfFire when MNK_BH_RoF == 1 && ActionReady(Brotherhood) && IsOnCooldown(RiddleOfFire) => Brotherhood,
                var _ => actionID
            };
        }
    }

    internal class MNK_PerfectBalanceProtection : CustomCombo
    {
        protected internal override Preset Preset => Preset.MNK_PerfectBalanceProtection;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not PerfectBalance)
                return actionID;

            return HasStatusEffect(Buffs.PerfectBalance) &&
                   LevelChecked(PerfectBalance)
                ? All.SavageBlade
                : actionID;
        }
    }
}
