using WrathCombo.CustomComboNS;
using WrathCombo.Data;
using WrathCombo.Extensions;
namespace WrathCombo.Combos.PvE;

internal partial class BLM : CasterJob
{
    internal class BLM_ST_SimpleMode : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_ST_SimpleMode;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Fire)
                return actionID;

            if (Variant.CanCure(CustomComboPreset.BLM_Variant_Cure, Config.BLM_VariantCure))
                return Variant.Cure;

            if (Variant.CanRampart(CustomComboPreset.BLM_Variant_Rampart))
                return Variant.Rampart;

            if (CanSpellWeave())
            {
                if (Gauge.InAstralFire && JustUsed(Despair) &&
                    IsOnCooldown(Manafont) && !JustUsed(Manafont))
                    return Transpose;

                if (Gauge.InUmbralIce)
                {
                    if (JustUsed(Transpose) && IsOffCooldown(Role.Swiftcast))
                        return Role.Swiftcast;

                    if (JustUsed(Paradox))
                        return Transpose;
                }

                if (ActionReady(Amplifier) && Gauge.EnochianTimer >= 20000 &&
                    !HasMaxPolyglotStacks)
                    return Amplifier;

                if (ActionReady(LeyLines) && !HasEffect(Buffs.LeyLines))
                    return LeyLines;

                if (ActionReady(Manafont) && CurMp is 0 && JustUsed(Despair))
                    return Manafont;
            }

            if (HasMaxPolyglotStacks && Gauge.EnochianTimer < 3000)
                return LevelChecked(Xenoglossy)
                    ? Xenoglossy
                    : Foul;

            if (HasEffect(Buffs.Thunderhead) && LevelChecked(Thunder) &&
                InBossEncounter() &&
                (ThunderDebuffST is null || ThunderDebuffST.RemainingTime < 3))
                return OriginalHook(Thunder);

            if (IsMoving() && InCombat())
            {
                if (ActionReady(Triplecast) && !HasEffect(Buffs.Triplecast))
                    return Triplecast;

                if (Gauge.InAstralFire && Gauge.IsParadoxActive)
                    return Paradox;

                if (ActionReady(Role.Swiftcast) && !HasEffect(Buffs.Triplecast))
                    return Role.Swiftcast;

                if (HasPolyglotStacks())
                    return LevelChecked(Xenoglossy)
                        ? Xenoglossy
                        : Foul;
            }

            if (Gauge.InAstralFire)
            {
                if (LevelChecked(Amplifier) && HasPolyglotStacks() &&
                    (GetCooldownRemainingTime(Amplifier) < 3 || GetCooldownRemainingTime(Amplifier) > 100))
                    return Xenoglossy;

                if (Gauge.IsParadoxActive && JustUsed(Transpose, 5) &&
                    !HasEffect(Buffs.Firestarter) && (Gauge.AstralFireStacks < 3 || JustUsed(FlareStar)))
                    return Paradox;

                if (HasEffect(Buffs.Firestarter) && Gauge.AstralFireStacks < 3)
                    return Fire3;

                if (LevelChecked(FlareStar) && FlarestarReady)
                    return FlareStar;

                if (CurMp < MP.FireI && LevelChecked(Despair) && CurMp >= MP.Despair)
                    return Despair;

                if (CurMp >= MP.FireI)
                {
                    return LevelChecked(Fire4)
                        ? Fire4
                        : Fire;
                }
            }

            if (Gauge.InUmbralIce)
            {
                if (Gauge.UmbralHearts is 3 && Gauge.IsParadoxActive && CurMp is MP.MaxMP)
                    return Paradox;

                if (JustUsed(Blizzard3, 5) && LevelChecked(Blizzard4))
                    return Blizzard4;

                if (ActionReady(Blizzard3) && Gauge.UmbralIceStacks < 3)
                    return Blizzard3;
            }

            //TODO fix recover from death
            if (LevelChecked(Fire3))
            {
                return CurMp > 7500
                    ? Fire3
                    : Blizzard3;
            }

            return actionID;
        }
    }

    internal class BLM_ST_AdvancedMode : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_ST_AdvancedMode;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Fire)
                return actionID;

            if (Variant.CanCure(CustomComboPreset.BLM_Variant_Cure, Config.BLM_VariantCure))
                return Variant.Cure;

            if (Variant.CanRampart(CustomComboPreset.BLM_Variant_Rampart))
                return Variant.Rampart;

            if (CanSpellWeave())
            {
                if (Gauge.InAstralFire && JustUsed(Despair) &&
                    IsOnCooldown(Manafont) && !JustUsed(Manafont))
                    return Transpose;

                if (Gauge.InUmbralIce)
                {
                    if (IsEnabled(CustomComboPreset.BLM_ST_Swiftcast) &&
                        JustUsed(Transpose) && IsOffCooldown(Role.Swiftcast))
                        return Role.Swiftcast;

                    if (JustUsed(Paradox))
                        return Transpose;
                }

                if (IsEnabled(CustomComboPreset.BLM_ST_Amplifier) &&
                    ActionReady(Amplifier) && Gauge.EnochianTimer >= 20000 &&
                    !HasMaxPolyglotStacks)
                    return Amplifier;

                if (IsEnabled(CustomComboPreset.BLM_ST_LeyLines) &&
                    ActionReady(LeyLines) && !HasEffect(Buffs.LeyLines))
                    return LeyLines;

                if (IsEnabled(CustomComboPreset.BLM_ST_Manafont) &&
                    ActionReady(Manafont) && CurMp is 0 && JustUsed(Despair))
                    return Manafont;
            }

            if (IsEnabled(CustomComboPreset.BLM_ST_UsePolyglot) &&
                HasMaxPolyglotStacks && Gauge.EnochianTimer < 3000)
                return LevelChecked(Xenoglossy)
                    ? Xenoglossy
                    : Foul;

            if (IsEnabled(CustomComboPreset.BLM_ST_Thunder) &&
                HasEffect(Buffs.Thunderhead) && LevelChecked(Thunder) &&
                (Config.BLM_ST_Thunder_SubOption == 0 ||
                 Config.BLM_ST_Thunder_SubOption == 1 && InBossEncounter()) &&
                (ThunderDebuffST is null || ThunderDebuffST.RemainingTime < 3))
                return OriginalHook(Thunder);

            if (IsMoving() && InCombat())
            {
                if (Config.BLM_ST_MovementOption[1] &&
                    ActionReady(Triplecast) && !HasEffect(Buffs.Triplecast))
                    return Triplecast;

                if (Config.BLM_ST_MovementOption[2] &&
                    Gauge.InAstralFire && Gauge.IsParadoxActive)
                    return Paradox;
                
                if (Config.BLM_ST_MovementOption[0] &&
                    ActionReady(Role.Swiftcast) && !HasEffect(Buffs.Triplecast))
                    return Role.Swiftcast;

                if (Config.BLM_ST_MovementOption[3] &&
                    HasPolyglotStacks())
                    return LevelChecked(Xenoglossy)
                        ? Xenoglossy
                        : Foul;
            }

            if (Gauge.InAstralFire)
            {
                if (IsEnabled(CustomComboPreset.BLM_ST_UsePolyglot) &&
                    LevelChecked(Amplifier) && HasPolyglotStacks() &&
                    (GetCooldownRemainingTime(Amplifier) < 3 || GetCooldownRemainingTime(Amplifier) > 100))
                    return Xenoglossy;

                if (Gauge.IsParadoxActive && JustUsed(Transpose, 5) &&
                    !HasEffect(Buffs.Firestarter) && (Gauge.AstralFireStacks < 3 || JustUsed(FlareStar)))
                    return Paradox;

                if (HasEffect(Buffs.Firestarter) && Gauge.AstralFireStacks < 3)
                    return Fire3;

                if (IsEnabled(CustomComboPreset.BLM_ST_FlareStar) &&
                    LevelChecked(FlareStar) && FlarestarReady)
                    return FlareStar;

                if (IsEnabled(CustomComboPreset.BLM_ST_Despair) &&
                    CurMp < MP.FireI && LevelChecked(Despair) && CurMp >= MP.Despair)
                    return Despair;

                if (CurMp >= MP.FireI)
                {
                    return LevelChecked(Fire4)
                        ? Fire4
                        : Fire;
                }
            }

            if (Gauge.InUmbralIce)
            {
                if (Gauge.UmbralHearts is 3 && Gauge.IsParadoxActive && CurMp is MP.MaxMP)
                    return Paradox;

                if (JustUsed(Blizzard3, 5) && LevelChecked(Blizzard4))
                    return Blizzard4;

                if (ActionReady(Blizzard3) && Gauge.UmbralIceStacks < 3)
                    return Blizzard3;
            }

            if (LevelChecked(Fire3))
            {
                return CurMp >= 7500
                    ? Fire3
                    : Blizzard3;
            }

            return actionID;
        }
    }

    //TODO Fix AoE
    internal class BLM_AoE_SimpleMode : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_AoE_SimpleMode;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not (Blizzard2 or HighBlizzard2))
                return actionID;

            if (Variant.CanCure(CustomComboPreset.BLM_Variant_Cure, Config.BLM_VariantCure))
                return Variant.Cure;

            if (Variant.CanRampart(CustomComboPreset.BLM_Variant_Rampart))
                return Variant.Rampart;

            if (WasLastSpell(UmbralSoul))
                return OriginalHook(Fire2);

            if (HasEffect(Buffs.Thunderhead) && Thunder2.LevelChecked() &&
                (ThunderDebuffAoE is null || ThunderDebuffAoE.RemainingTime < 3))
                return OriginalHook(Thunder2);

            if (ActionReady(Amplifier) && RemainingPolyglotCD >= 20000 && CanSpellWeave())
                return Amplifier;

            if (IsMoving())
            {
                if (ActionReady(Amplifier) && Gauge.PolyglotStacks < MaxPolyglot)
                    return Amplifier;

                if (HasPolyglotStacks())
                    return Foul;
            }

            if (CanSpellWeave() &&
                ActionReady(LeyLines) && !HasEffect(Buffs.LeyLines))
                return LeyLines;

            if (Gauge.InAstralFire)
            {
                if (CurMp == 0 && FlareStar.LevelChecked() && Gauge.AstralSoulStacks == 6)
                    return FlareStar;

                if (!FlareStar.LevelChecked() && Fire2.LevelChecked() && CurMp >= MP.FireAoE &&
                    (Gauge.UmbralHearts > 1 || !TraitLevelChecked(Traits.UmbralHeart)))
                    return OriginalHook(Fire2);

                if (Flare.LevelChecked() && CurMp >= MP.AllMPSpells)
                {
                    if (ActionReady(Triplecast) && GetBuffStacks(Buffs.Triplecast) == 0 &&
                        CanSpellWeave())
                        return Triplecast;
                    if (Flare.LevelChecked() && CurMp >= MP.FlareAoE)
                    {
                        if (ActionReady(Triplecast) && GetBuffStacks(Buffs.Triplecast) == 0 &&
                            CanSpellWeave())
                            return Triplecast;

                        return Flare;
                    }

                    if (Fire2.LevelChecked())
                        if (CurMp >= MP.FireAoE)
                            return OriginalHook(Fire2);

                    if (ActionReady(Manafont))
                        return Manafont;

                    if (ActionReady(Transpose) && !TraitLevelChecked(Traits.AspectMasteryIII))
                        return Transpose;

                    if (ActionReady(Blizzard2) && TraitLevelChecked(Traits.AspectMasteryIII))
                        return OriginalHook(Blizzard2);
                }
            }

            if (Gauge.InUmbralIce)
            {
                if (HasPolyglotStacks())
                    return Foul;

                if (ActionWatching.WhichOfTheseActionsWasLast(OriginalHook(Fire2), OriginalHook(Freeze),
                        OriginalHook(Flare), OriginalHook(FlareStar)) == OriginalHook(Freeze) &&
                    FlareStar.LevelChecked())
                {
                    if (ActionReady(Transpose) && CanSpellWeave())
                        return Transpose;

                    return OriginalHook(Fire2);
                }

                if (ActionReady(OriginalHook(Blizzard2)) && Gauge.UmbralIceStacks < 3 &&
                    TraitLevelChecked(Traits.AspectMasteryIII))
                {
                    if (ActionReady(Triplecast) && GetBuffStacks(Buffs.Triplecast) == 0 &&
                        CanSpellWeave())
                        return Triplecast;

                    if (GetBuffStacks(Buffs.Triplecast) == 0 && IsOffCooldown(Role.Swiftcast) &&
                        CanSpellWeave())
                        return Role.Swiftcast;

                    if (HasEffect(MagicRole.Buffs.Swiftcast) || GetBuffStacks(Buffs.Triplecast) > 0)
                        return OriginalHook(Blizzard2);
                }

                if (Gauge.UmbralIceStacks < 3 && ActionReady(OriginalHook(Blizzard2)))
                    return OriginalHook(Blizzard2);

                if (Freeze.LevelChecked() && Gauge.UmbralHearts < 3 && TraitLevelChecked(Traits.UmbralHeart))
                    return Freeze;

                if (DoubleBlizz() && Fire2.LevelChecked())
                    return OriginalHook(Fire2);

                if (CurMp < LocalPlayer?.MaxMp)
                    return Freeze.LevelChecked()
                        ? OriginalHook(Freeze)
                        : OriginalHook(Blizzard2);

                if (ActionReady(Transpose) && CanSpellWeave() &&
                    (Flare.LevelChecked() || !TraitLevelChecked(Traits.AspectMasteryIII)))
                    return Transpose;

                if (Fire2.LevelChecked() && TraitLevelChecked(Traits.AspectMasteryIII))
                    return OriginalHook(Fire2);
            }

            if (Blizzard2.LevelChecked())
                return OriginalHook(Blizzard2);

            return actionID;
        }
    }

    internal class BLM_AoE_AdvancedMode : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_AoE_AdvancedMode;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not (Blizzard2 or HighBlizzard2))
                return actionID;

            int polyglotStacks = Gauge.PolyglotStacks;
            float triplecastChargetime = GetCooldownChargeRemainingTime(Triplecast);

            if (Variant.CanCure(CustomComboPreset.BLM_Variant_Cure, Config.BLM_VariantCure))
                return Variant.Cure;

            if (Variant.CanRampart(CustomComboPreset.BLM_Variant_Rampart))
                return Variant.Rampart;

            if (WasLastSpell(UmbralSoul))
                return OriginalHook(Fire2);

            if (IsEnabled(CustomComboPreset.BLM_AoE_Thunder) &&
                HasEffect(Buffs.Thunderhead) && LevelChecked(Thunder2) &&
                GetTargetHPPercent() >= Config.BLM_AoE_ThunderHP &&
                (ThunderDebuffAoE is null || ThunderDebuffAoE.RemainingTime < 3))
                return OriginalHook(Thunder2);

            if (IsEnabled(CustomComboPreset.BLM_AoE_Amplifier) &&
                ActionReady(Amplifier) && RemainingPolyglotCD >= 20000 && CanSpellWeave())
                return Amplifier;

            if (IsMoving())
            {
                if (IsEnabled(CustomComboPreset.BLM_AoE_Amplifier) &&
                    ActionReady(Amplifier) && Gauge.PolyglotStacks < MaxPolyglot)
                    return Amplifier;

                if (IsEnabled(CustomComboPreset.BLM_AoE_UsePolyglotMoving) &&
                    polyglotStacks > Config.BLM_AoE_UsePolyglotMoving_HoldCharges)
                    return Foul;
            }

            if (IsEnabled(CustomComboPreset.BLM_AoE_LeyLines) &&
                CanSpellWeave() &&
                ActionReady(LeyLines) && !HasEffect(Buffs.LeyLines) &&
                GetRemainingCharges(LeyLines) > Config.BLM_AoE_LeyLinesCharges)
                return LeyLines;

            if (Gauge.InAstralFire)
            {
                if (IsEnabled(CustomComboPreset.BLM_AoE_FlareStar) &&
                    CurMp == 0 && FlareStar.LevelChecked() && Gauge.AstralSoulStacks == 6)
                    return FlareStar;

                if (!FlareStar.LevelChecked() && Fire2.LevelChecked() && CurMp >= MP.FireAoE &&
                    (Gauge.UmbralHearts > 1 || !TraitLevelChecked(Traits.UmbralHeart)))
                    return OriginalHook(Fire2);

                if (IsEnabled(CustomComboPreset.BLM_AoE_Flare) &&
                    Flare.LevelChecked() && CurMp >= MP.FlareAoE)
                {
                    if (LevelChecked(Triplecast) && CanSpellWeave() &&
                        GetBuffStacks(Buffs.Triplecast) == 0 &&
                        (GetRemainingCharges(Triplecast) > Config.BLM_AoE_Triplecast_HoldCharges ||
                         triplecastChargetime <= Config.BLM_AoE_Triplecast_ChargeTime))
                        return Triplecast;

                    return Flare;
                }

                if (Fire2.LevelChecked())
                    if (CurMp >= MP.FireAoE)
                        return OriginalHook(Fire2);

                if (IsEnabled(CustomComboPreset.BLM_AoE_Manafont) &&
                    ActionReady(Manafont))
                    return Manafont;

                if (IsEnabled(CustomComboPreset.BLM_AoE_Transpose) &&
                    ActionReady(Transpose) && !TraitLevelChecked(Traits.AspectMasteryIII))
                    return Transpose;

                if (ActionReady(Blizzard2) && TraitLevelChecked(Traits.AspectMasteryIII))
                    return OriginalHook(Blizzard2);
            }

            if (Gauge.InUmbralIce)
            {
                if (IsEnabled(CustomComboPreset.BLM_AoE_UsePolyglot) &&
                    polyglotStacks > Config.BLM_AoE_UsePolyglot_HoldCharges)
                    return Foul;

                if (ActionWatching.WhichOfTheseActionsWasLast(OriginalHook(Fire2), OriginalHook(Freeze),
                        OriginalHook(Flare), OriginalHook(FlareStar)) == OriginalHook(Freeze) &&
                    FlareStar.LevelChecked())
                {
                    if (IsEnabled(CustomComboPreset.BLM_AoE_Transpose) &&
                        ActionReady(Transpose) && CanSpellWeave())
                        return Transpose;

                    return OriginalHook(Fire2);
                }

                if (ActionReady(OriginalHook(Blizzard2)) && Gauge.UmbralIceStacks < 3 &&
                    TraitLevelChecked(Traits.AspectMasteryIII))
                {
                    if (IsEnabled(CustomComboPreset.BLM_AoE_Triplecast) &&
                        LevelChecked(Triplecast) && CanSpellWeave() &&
                        GetBuffStacks(Buffs.Triplecast) == 0 &&
                        (GetRemainingCharges(Triplecast) > Config.BLM_AoE_Triplecast_HoldCharges ||
                         triplecastChargetime <= Config.BLM_AoE_Triplecast_ChargeTime))
                        return Triplecast;

                    if (IsEnabled(CustomComboPreset.BLM_AoE_Swiftcast) &&
                        GetBuffStacks(Buffs.Triplecast) == 0 && IsOffCooldown(Role.Swiftcast) &&
                        CanSpellWeave())
                        return Role.Swiftcast;

                    if (HasEffect(MagicRole.Buffs.Swiftcast) || GetBuffStacks(Buffs.Triplecast) > 0)
                        return OriginalHook(Blizzard2);
                }

                if (Gauge.UmbralIceStacks < 3 && ActionReady(OriginalHook(Blizzard2)))
                    return OriginalHook(Blizzard2);

                if (Freeze.LevelChecked() && Gauge.UmbralHearts < 3 && TraitLevelChecked(Traits.UmbralHeart))
                    return Freeze;

                if (DoubleBlizz() && Fire2.LevelChecked())
                    return OriginalHook(Fire2);

                if (CurMp < LocalPlayer?.MaxMp)
                    return Freeze.LevelChecked()
                        ? OriginalHook(Freeze)
                        : OriginalHook(Blizzard2);

                if (IsEnabled(CustomComboPreset.BLM_AoE_Transpose) &&
                    ActionReady(Transpose) && CanSpellWeave() &&
                    (Flare.LevelChecked() || !TraitLevelChecked(Traits.AspectMasteryIII)))
                    return Transpose;

                if (Fire2.LevelChecked() && TraitLevelChecked(Traits.AspectMasteryIII))
                    return OriginalHook(Fire2);
            }

            if (Blizzard2.LevelChecked())
                return OriginalHook(Blizzard2);
            return actionID;
        }
    }

    internal class BLM_Variant_Raise : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_Variant_Raise;

        protected override uint Invoke(uint actionID) =>
            actionID is Role.Swiftcast && Variant.CanRaise(CustomComboPreset.BLM_Variant_Raise)
                ? Variant.Raise
                : actionID;
    }

    internal class BLM_Scathe_Xeno : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_Scathe_Xeno;

        protected override uint Invoke(uint actionID) =>
            actionID is Scathe && LevelChecked(Xenoglossy) && HasPolyglotStacks()
                ? Xenoglossy
                : actionID;
    }

    internal class BLM_Blizzard_1to3 : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_Blizzard_1to3;

        protected override uint Invoke(uint actionID)
        {
            switch (actionID)
            {
                case Blizzard when LevelChecked(Freeze) && !Gauge.InUmbralIce:
                    return Blizzard3;

                case Freeze when !LevelChecked(Freeze):
                    return Blizzard2;

                default:
                    return actionID;
            }
        }
    }

    internal class BLM_Fire_1to3 : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_Fire_1to3;

        protected override uint Invoke(uint actionID) =>
            actionID is Fire &&
            (LevelChecked(Fire3) && !Gauge.InAstralFire ||
             HasEffect(Buffs.Firestarter))
                ? Fire3
                : actionID;
    }

    internal class BLM_Between_The_LeyLines : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_Between_The_LeyLines;

        protected override uint Invoke(uint actionID) =>
            actionID is LeyLines && HasEffect(Buffs.LeyLines) && LevelChecked(BetweenTheLines)
                ? BetweenTheLines
                : actionID;
    }

    internal class BLM_Aetherial_Manipulation : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_Aetherial_Manipulation;

        protected override uint Invoke(uint actionID) =>
            actionID is AetherialManipulation && ActionReady(BetweenTheLines) &&
            HasEffect(Buffs.LeyLines) && !HasEffect(Buffs.CircleOfPower) && !IsMoving()
                ? BetweenTheLines
                : actionID;
    }

    internal class BLM_UmbralSoul : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_UmbralSoul;

        protected override uint Invoke(uint actionID) =>
            actionID is Transpose && Gauge.InUmbralIce && LevelChecked(UmbralSoul)
                ? UmbralSoul
                : actionID;
    }

    internal class BLM_TriplecastProtection : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_TriplecastProtection;

        protected override uint Invoke(uint actionID) =>
            actionID is Triplecast && HasEffect(Buffs.Triplecast) && LevelChecked(Triplecast)
                ? All.SavageBlade
                : actionID;
    }

    internal class BLM_FireandIce : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_FireandIce;

        protected override uint Invoke(uint actionID)
        {
            switch (actionID)
            {
                case Fire4 when Gauge.InAstralFire && LevelChecked(Fire4):
                    return Fire4;

                case Fire4 when Gauge.InUmbralIce && LevelChecked(Blizzard4):
                    return Blizzard4;

                case Flare when Gauge.InAstralFire && LevelChecked(Flare):
                    return Flare;

                case Flare when Gauge.InUmbralIce && LevelChecked(Freeze):
                    return Freeze;

                default:
                    return actionID;
            }
        }
    }

    internal class BLM_FireFlarestar : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_FireFlarestar;

        protected override uint Invoke(uint actionID)
        {
            return actionID switch
            {
                Fire4 when Gauge.InAstralFire && FlarestarReady && LevelChecked(FlareStar) => FlareStar,
                Flare when Gauge.InAstralFire && FlarestarReady && LevelChecked(FlareStar) => FlareStar,
                var _ => actionID
            };
        }
    }
}
