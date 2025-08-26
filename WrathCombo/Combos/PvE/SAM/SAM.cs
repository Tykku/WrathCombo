using WrathCombo.CustomComboNS;
using static WrathCombo.Combos.PvE.SAM.Config;
namespace WrathCombo.Combos.PvE;

internal partial class SAM : Melee
{
    internal class SAM_ST_SimpleMode : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_ST_SimpleMode;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not (Hakaze or Gyofu))
                return actionID;

            //Meikyo to start before combat
            if (!HasStatusEffect(Buffs.MeikyoShisui) && ActionReady(MeikyoShisui) &&
                !InCombat() && HasBattleTarget())
                return MeikyoShisui;

            if (Variant.CanCure(Preset.SAM_Variant_Cure, SAM_VariantCure))
                return Variant.Cure;

            if (Variant.CanRampart(Preset.SAM_Variant_Rampart))
                return Variant.Rampart;

            if (ContentSpecificActions.TryGet(out uint contentAction))
                return contentAction;

            //oGCDs
            if (CanWeave() && M6SReady)
            {
                //Meikyo Features
                if (UseMeikyo())
                    return MeikyoShisui;

                //Ikishoten Features
                if (ActionReady(Ikishoten) &&
                    !HasStatusEffect(Buffs.ZanshinReady))
                {
                    return Kenki switch
                    {
                        //Dumps Kenki in preparation for Ikishoten
                        >= 50 => Shinten,

                        < 50 => Ikishoten
                    };
                }

                switch (Kenki)
                {
                    //Senei Features
                    case >= 25 when ActionReady(Senei):
                        return Senei;

                    //Guren if no Senei
                    case >= 25 when !LevelChecked(Senei) &&
                                    ActionReady(Guren) &&
                                    InActionRange(Guren):
                        return Guren;
                }

                //Zanshin Usage
                //TODO Buffcheck
                if (ActionReady(Zanshin) && Kenki >= 50 &&
                    InActionRange(Zanshin) &&
                    HasStatusEffect(Buffs.ZanshinReady) &&
                    (JustUsed(Higanbana) ||
                     JustUsed(OriginalHook(OgiNamikiri)) ||
                     GetStatusEffectRemainingTime(Buffs.ZanshinReady) <= 8))
                    return Zanshin;

                if (ActionReady(Shoha) &&
                    MeditationStacks is 3 &&
                    InActionRange(Shoha))
                    return Shoha;

                if (ActionReady(Shinten) &&
                    !HasStatusEffect(Buffs.ZanshinReady) && !ActionReady(Senei) &&
                    (Kenki >= 65 || GetTargetHPPercent() <= 1 && Kenki >= 25))
                    return Shinten;

                // healing
                if (Role.CanSecondWind(25))
                    return Role.SecondWind;

                if (Role.CanBloodBath(40))
                    return Role.Bloodbath;
            }

            //Ranged
            if (ActionReady(Enpi) &&
                !InMeleeRange() &&
                HasBattleTarget())
                return Enpi;
            if (UseTsubame)
                return OriginalHook(TsubameGaeshi);

            //Ogi Namikiri Features
            if (ActionReady(OgiNamikiri) && M6SReady &&
                InActionRange(OriginalHook(OgiNamikiri)) &&
                HasStatusEffect(Buffs.OgiNamikiriReady) &&
                (JustUsed(Higanbana, 5f) ||
                 !TargetIsBoss() ||
                 GetStatusEffectRemainingTime(Buffs.OgiNamikiriReady) <= 8) || NamikiriReady)
                return OriginalHook(OgiNamikiri);

            // Iaijutsu Features
            if (UseIaijutsu() && !IsMoving())
                return OriginalHook(Iaijutsu);

            if (HasStatusEffect(Buffs.MeikyoShisui))
            {
                if (LevelChecked(Gekko) &&
                    (!HasStatusEffect(Buffs.Fugetsu) ||
                     !HasGetsu && HasStatusEffect(Buffs.Fuka)))
                    return Role.CanTrueNorth() && !OnTargetsRear()
                        ? Role.TrueNorth
                        : Gekko;

                if (LevelChecked(Kasha) &&
                    (!HasStatusEffect(Buffs.Fuka) ||
                     !HasKa && HasStatusEffect(Buffs.Fugetsu)))
                    return Role.CanTrueNorth() && !OnTargetsFlank()
                        ? Role.TrueNorth
                        : Kasha;

                if (LevelChecked(Yukikaze) && !HasSetsu)
                    return Yukikaze;
            }

            if (ComboTimer > 0)
            {
                if (ComboAction is Hakaze or Gyofu && LevelChecked(Jinpu))
                {
                    if (LevelChecked(Yukikaze) && !HasSetsu &&
                        HasStatusEffect(Buffs.Fugetsu) && HasStatusEffect(Buffs.Fuka))
                        return Yukikaze;

                    if (!LevelChecked(Kasha) &&
                        (RefreshFugetsu || !HasStatusEffect(Buffs.Fugetsu)) ||
                        LevelChecked(Kasha) &&
                        (!HasStatusEffect(Buffs.Fugetsu) ||
                         HasStatusEffect(Buffs.Fuka) && !HasGetsu ||
                         SenCount is 3 && RefreshFugetsu))
                        return Jinpu;

                    if (LevelChecked(Shifu) &&
                        (!LevelChecked(Kasha) &&
                         (RefreshFuka || !HasStatusEffect(Buffs.Fuka)) ||
                         LevelChecked(Kasha) &&
                         (!HasStatusEffect(Buffs.Fuka) ||
                          HasStatusEffect(Buffs.Fugetsu) && !HasKa ||
                          SenCount is 3 && RefreshFuka)))
                        return Shifu;
                }

                if (ComboAction is Jinpu && LevelChecked(Gekko))
                    return Gekko;

                if (ComboAction is Shifu && LevelChecked(Kasha))
                    return Kasha;
            }

            return actionID;
        }
    }

    internal class SAM_AoE_SimpleMode : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_AoE_SimpleMode;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not (Fuga or Fuko))
                return actionID;

            if (Variant.CanCure(Preset.SAM_Variant_Cure, SAM_VariantCure))
                return Variant.Cure;

            if (Variant.CanRampart(Preset.SAM_Variant_Rampart))
                return Variant.Rampart;

            if (ContentSpecificActions.TryGet(out uint contentAction))
                return contentAction;

            //oGCD Features
            if (CanWeave() && M6SReady)
            {
                if (OriginalHook(Iaijutsu) is MidareSetsugekka && LevelChecked(Hagakure))
                    return Hagakure;

                if (ActionReady(Ikishoten) && !HasStatusEffect(Buffs.ZanshinReady))
                {
                    return Kenki switch
                    {
                        //Dumps Kenki in preparation for Ikishoten
                        >= 50 => Kyuten,

                        < 50 => Ikishoten
                    };
                }

                if (ActionReady(MeikyoShisui) && !HasStatusEffect(Buffs.MeikyoShisui))
                    return MeikyoShisui;

                if (ActionReady(Zanshin) && HasStatusEffect(Buffs.ZanshinReady) && Kenki >= 50)
                    return Zanshin;

                if (ActionReady(Guren) && Kenki >= 25)
                    return Guren;

                if (ActionReady(Shoha) && MeditationStacks is 3)
                    return Shoha;

                if (ActionReady(Kyuten) && Kenki >= 50 &&
                    !ActionReady(Guren))
                    return Kyuten;

                // healing
                if (Role.CanSecondWind(25))
                    return Role.SecondWind;

                if (Role.CanBloodBath(40))
                    return Role.Bloodbath;
            }

            if (ActionReady(OgiNamikiri) && M6SReady &&
                !IsMoving() && (HasStatusEffect(Buffs.OgiNamikiriReady) || NamikiriReady))
                return OriginalHook(OgiNamikiri);

            if (LevelChecked(TenkaGoken))
            {
                if (LevelChecked(TsubameGaeshi) &&
                    (HasStatusEffect(Buffs.KaeshiGokenReady) ||
                     HasStatusEffect(Buffs.TendoKaeshiGokenReady)))
                    return OriginalHook(TsubameGaeshi);

                if (!IsMoving() &&
                    (OriginalHook(Iaijutsu) is TenkaGoken ||
                     OriginalHook(Iaijutsu) is TendoGoken))
                    return OriginalHook(Iaijutsu);
            }

            if (HasStatusEffect(Buffs.MeikyoShisui))
            {
                if (!HasGetsu && HasStatusEffect(Buffs.Fuka) ||
                    !HasStatusEffect(Buffs.Fugetsu))
                    return Mangetsu;

                if (!HasKa && HasStatusEffect(Buffs.Fugetsu) ||
                    !HasStatusEffect(Buffs.Fuka))
                    return Oka;
            }

            if (ComboTimer > 0 &&
                ComboAction is Fuko or Fuga && LevelChecked(Mangetsu))
            {
                if (!HasGetsu ||
                    RefreshFugetsu ||
                    !HasStatusEffect(Buffs.Fugetsu) ||
                    !LevelChecked(Oka))
                    return Mangetsu;

                if (LevelChecked(Oka) &&
                    (!HasKa ||
                     RefreshFuka ||
                     !HasStatusEffect(Buffs.Fuka)))
                    return Oka;
            }

            return actionID;
        }
    }

    internal class SAM_ST_AdvancedMode : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_ST_AdvancedMode;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not (Hakaze or Gyofu))
                return actionID;

            int kenkiOvercap = SAM_ST_KenkiOvercapAmount;
            int shintenTreshhold = SAM_ST_ExecuteThreshold;

            // Opener for SAM
            if (IsEnabled(Preset.SAM_ST_Opener) &&
                Opener().FullOpener(ref actionID))
                return actionID;

            //Meikyo to start before combat
            if (IsEnabled(Preset.SAM_ST_CDs) &&
                IsEnabled(Preset.SAM_ST_CDs_MeikyoShisui) &&
                ActionReady(MeikyoShisui) &&
                !HasStatusEffect(Buffs.MeikyoShisui) &&
                !InCombat() && HasBattleTarget())
                return MeikyoShisui;

            if (Variant.CanCure(Preset.SAM_Variant_Cure, SAM_VariantCure))
                return Variant.Cure;

            if (Variant.CanRampart(Preset.SAM_Variant_Rampart))
                return Variant.Rampart;

            if (ContentSpecificActions.TryGet(out uint contentAction))
                return contentAction;

            //oGCDs
            if (CanWeave() && M6SReady)
            {
                if (IsEnabled(Preset.SAM_ST_CDs))
                {
                    //Meikyo Features
                    if (IsEnabled(Preset.SAM_ST_CDs_MeikyoShisui) &&
                        UseMeikyo())
                        return MeikyoShisui;

                    //Ikishoten Features
                    if (IsEnabled(Preset.SAM_ST_CDs_Ikishoten) &&
                        ActionReady(Ikishoten) && !HasStatusEffect(Buffs.ZanshinReady))
                    {
                        return Kenki switch
                        {
                            //Dumps Kenki in preparation for Ikishoten
                            >= 50 => Shinten,

                            < 50 => Ikishoten
                        };
                    }
                }

                if (IsEnabled(Preset.SAM_ST_Damage))
                {
                    //Senei Features
                    if (IsEnabled(Preset.SAM_ST_CDs_Senei)
                        && Kenki >= 25)
                    {
                        if (ActionReady(Senei))
                            return Senei;

                        //Guren if no Senei
                        if (SAM_ST_CDs_Guren &&
                            !LevelChecked(Senei) &&
                            ActionReady(Guren) && InActionRange(Guren))
                            return Guren;
                    }

                    //Zanshin Usage
                    //TODO Buffcheck
                    if (IsEnabled(Preset.SAM_ST_CDs_Zanshin) &&
                        ActionReady(Zanshin) && Kenki >= 50 &&
                        InActionRange(Zanshin) &&
                        HasStatusEffect(Buffs.ZanshinReady) &&
                        (JustUsed(Higanbana) ||
                         JustUsed(OriginalHook(OgiNamikiri)) ||
                         SAM_ST_HiganbanaBossOption == 1 && !TargetIsBoss() ||
                         GetStatusEffectRemainingTime(Buffs.ZanshinReady) <= 8))
                        return Zanshin;

                    if (IsEnabled(Preset.SAM_ST_CDs_Shoha) &&
                        ActionReady(Shoha) && MeditationStacks is 3 &&
                        InActionRange(Shoha))
                        return Shoha;
                }
                if (IsEnabled(Preset.SAM_ST_Shinten) &&
                    ActionReady(Shinten) && !HasStatusEffect(Buffs.ZanshinReady) &&
                    (IsEnabled(Preset.SAM_ST_CDs_Senei) && !ActionReady(Senei) ||
                     IsNotEnabled(Preset.SAM_ST_CDs_Senei)) &&
                    (Kenki >= kenkiOvercap || GetTargetHPPercent() <= shintenTreshhold && Kenki >= 25))
                    return Shinten;

                if (IsEnabled(Preset.SAM_ST_Feint) &&
                    Role.CanFeint() &&
                    RaidWideCasting())
                    return Role.Feint;

                // healing
                if (IsEnabled(Preset.SAM_ST_ComboHeals))
                {
                    if (Role.CanSecondWind(SAM_STSecondWindHPThreshold))
                        return Role.SecondWind;

                    if (Role.CanBloodBath(SAM_STBloodbathHPThreshold))
                        return Role.Bloodbath;
                }

                if (IsEnabled(Preset.SAM_ST_StunInterupt) &&
                    RoleActions.Melee.CanLegSweep() &&
                    !TargetIsBoss() && TargetIsCasting())
                    return Role.LegSweep;
            }

            //Ranged
            if (IsEnabled(Preset.SAM_ST_RangedUptime) &&
                ActionReady(Enpi) && !InMeleeRange() && HasBattleTarget())
                return Enpi;

            if (IsEnabled(Preset.SAM_ST_Damage))
            {
                if (IsEnabled(Preset.SAM_ST_CDs_Iaijutsu) &&
                    SAM_ST_CDs_IaijutsuOption[3] && UseTsubame)
                    return OriginalHook(TsubameGaeshi);

                //Ogi Namikiri Features
                if (IsEnabled(Preset.SAM_ST_CDs_OgiNamikiri) &&
                    (!SAM_ST_CDs_OgiNamikiri_Movement || !IsMoving()) &&
                    ActionReady(OgiNamikiri) && InActionRange(OriginalHook(OgiNamikiri)) &&
                    HasStatusEffect(Buffs.OgiNamikiriReady) && M6SReady &&
                    (JustUsed(Higanbana, 5f) ||
                     SAM_ST_HiganbanaBossOption == 1 && !TargetIsBoss() ||
                     GetStatusEffectRemainingTime(Buffs.OgiNamikiriReady) <= 8) || NamikiriReady)
                    return OriginalHook(OgiNamikiri);

                // Iaijutsu Features
                if (IsEnabled(Preset.SAM_ST_CDs_Iaijutsu) &&
                    (!IsEnabled(Preset.SAM_ST_CDs_Iaijutsu_Movement) || !IsMoving()) &&
                    UseIaijutsu())
                    return OriginalHook(Iaijutsu);
            }

            if (HasStatusEffect(Buffs.MeikyoShisui))
            {
                if (IsEnabled(Preset.SAM_ST_Gekko) &&
                    LevelChecked(Gekko) &&
                    (!HasStatusEffect(Buffs.Fugetsu) ||
                     !HasGetsu && HasStatusEffect(Buffs.Fuka)))
                    return IsEnabled(Preset.SAM_ST_TrueNorth) &&
                           Role.CanTrueNorth() && !OnTargetsRear()
                        ? Role.TrueNorth
                        : Gekko;

                if (IsEnabled(Preset.SAM_ST_Kasha) &&
                    LevelChecked(Kasha) &&
                    (!HasStatusEffect(Buffs.Fuka) ||
                     !HasKa && HasStatusEffect(Buffs.Fugetsu)))
                    return IsEnabled(Preset.SAM_ST_TrueNorth) &&
                           Role.CanTrueNorth() && !OnTargetsFlank()
                        ? Role.TrueNorth
                        : Kasha;

                if (IsEnabled(Preset.SAM_ST_Yukikaze) &&
                    LevelChecked(Yukikaze) && !HasSetsu)
                    return Yukikaze;
            }

            if (ComboTimer > 0)
            {
                if (ComboAction is Hakaze or Gyofu && LevelChecked(Jinpu))
                {
                    if (IsEnabled(Preset.SAM_ST_Yukikaze) &&
                        !HasSetsu && LevelChecked(Yukikaze) &&
                        HasStatusEffect(Buffs.Fugetsu) && HasStatusEffect(Buffs.Fuka))
                        return Yukikaze;

                    if (IsEnabled(Preset.SAM_ST_Gekko) &&
                        !LevelChecked(Kasha) &&
                        (RefreshFugetsu || !HasStatusEffect(Buffs.Fugetsu)) ||
                        LevelChecked(Kasha) &&
                        (!HasStatusEffect(Buffs.Fugetsu) ||
                         HasStatusEffect(Buffs.Fuka) && !HasGetsu ||
                         SenCount is 3 && RefreshFugetsu))
                        return Jinpu;

                    if (IsEnabled(Preset.SAM_ST_Kasha) &&
                        LevelChecked(Shifu) &&
                        (!LevelChecked(Kasha) &&
                         (RefreshFuka || !HasStatusEffect(Buffs.Fuka)) ||
                         LevelChecked(Kasha) &&
                         (!HasStatusEffect(Buffs.Fuka) ||
                          HasStatusEffect(Buffs.Fugetsu) && !HasKa ||
                          SenCount is 3 && RefreshFuka)))
                        return Shifu;
                }

                if (ComboAction is Jinpu && LevelChecked(Gekko))
                    return Gekko;

                if (IsEnabled(Preset.SAM_ST_Kasha) &&
                    ComboAction is Shifu && LevelChecked(Kasha))
                    return Kasha;
            }

            return actionID;
        }
    }

    internal class SAM_AoE_AdvancedMode : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_AoE_AdvancedMode;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not (Fuga or Fuko))
                return actionID;

            float kenkiOvercapAoE = SAM_AoE_KenkiOvercapAmount;

            if (Variant.CanCure(Preset.SAM_Variant_Cure, SAM_VariantCure))
                return Variant.Cure;

            if (Variant.CanRampart(Preset.SAM_Variant_Rampart))
                return Variant.Rampart;

            if (ContentSpecificActions.TryGet(out uint contentAction))
                return contentAction;

            //oGCD Features
            if (CanWeave() && M6SReady)
            {
                if (IsEnabled(Preset.SAM_AoE_Hagakure) &&
                    OriginalHook(Iaijutsu) is MidareSetsugekka && LevelChecked(Hagakure))
                    return Hagakure;

                if (IsEnabled(Preset.SAM_AoE_CDs))
                {
                    if (IsEnabled(Preset.SAM_AoE_MeikyoShisui) &&
                        ActionReady(MeikyoShisui) && !HasStatusEffect(Buffs.MeikyoShisui))
                        return MeikyoShisui;

                    if (IsEnabled(Preset.SAM_AOE_CDs_Ikishoten) &&
                        ActionReady(Ikishoten) && !HasStatusEffect(Buffs.ZanshinReady))
                    {
                        return Kenki switch
                        {
                            //Dumps Kenki in preparation for Ikishoten
                            >= 50 => Kyuten,

                            < 50 => Ikishoten
                        };
                    }
                }

                if (IsEnabled(Preset.SAM_AoE_Damage))
                {
                    if (IsEnabled(Preset.SAM_AoE_Zanshin) &&
                        ActionReady(Zanshin) && HasStatusEffect(Buffs.ZanshinReady) && Kenki >= 50)
                        return Zanshin;

                    if (IsEnabled(Preset.SAM_AoE_Guren) &&
                        ActionReady(Guren) && Kenki >= 25)
                        return Guren;

                    if (IsEnabled(Preset.SAM_AoE_Shoha) &&
                        ActionReady(Shoha) && MeditationStacks is 3)
                        return Shoha;
                }

                if (IsEnabled(Preset.SAM_AoE_Kyuten) &&
                    ActionReady(Kyuten) && Kenki >= kenkiOvercapAoE &&
                    !ActionReady(Guren))
                    return Kyuten;

                if (IsEnabled(Preset.SAM_AoE_ComboHeals))
                {
                    if (Role.CanSecondWind(SAM_AoESecondWindHPThreshold))
                        return Role.SecondWind;

                    if (Role.CanBloodBath(SAM_AoEBloodbathHPThreshold))
                        return Role.Bloodbath;
                }

                if (IsEnabled(Preset.SAM_AoE_StunInterupt) &&
                    RoleActions.Melee.CanLegSweep() &&
                    !TargetIsBoss() && TargetIsCasting())
                    return Role.LegSweep;
            }

            if (IsEnabled(Preset.SAM_AoE_Damage))
            {
                if (IsEnabled(Preset.SAM_AoE_OgiNamikiri) &&
                    ActionReady(OgiNamikiri) && M6SReady &&
                    (!IsMoving() && HasStatusEffect(Buffs.OgiNamikiriReady) || NamikiriReady))
                    return OriginalHook(OgiNamikiri);

                if (IsEnabled(Preset.SAM_AoE_TenkaGoken) &&
                    LevelChecked(TenkaGoken))
                {
                    if (LevelChecked(TsubameGaeshi) &&
                        (HasStatusEffect(Buffs.KaeshiGokenReady) ||
                         HasStatusEffect(Buffs.TendoKaeshiGokenReady)))
                        return OriginalHook(TsubameGaeshi);

                    if (!IsMoving() &&
                        (OriginalHook(Iaijutsu) is TenkaGoken ||
                         OriginalHook(Iaijutsu) is TendoGoken))
                        return OriginalHook(Iaijutsu);
                }
            }

            if (HasStatusEffect(Buffs.MeikyoShisui))
            {
                if (!HasGetsu && HasStatusEffect(Buffs.Fuka) ||
                    !HasStatusEffect(Buffs.Fugetsu))
                    return Mangetsu;

                if (IsEnabled(Preset.SAM_AoE_Oka) &&
                    (!HasKa && HasStatusEffect(Buffs.Fugetsu) ||
                     !HasStatusEffect(Buffs.Fuka)))
                    return Oka;
            }

            if (ComboTimer > 0 &&
                ComboAction is Fuko or Fuga && LevelChecked(Mangetsu))
            {
                if (IsNotEnabled(Preset.SAM_AoE_Oka) ||
                    !HasGetsu || RefreshFugetsu ||
                    !HasStatusEffect(Buffs.Fugetsu) ||
                    !LevelChecked(Oka))
                    return Mangetsu;

                if (IsEnabled(Preset.SAM_AoE_Oka) &&
                    LevelChecked(Oka) &&
                    (!HasKa || RefreshFuka ||
                     !HasStatusEffect(Buffs.Fuka)))
                    return Oka;
            }
            return actionID;
        }
    }

    internal class SAM_ST_YukikazeCombo : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_ST_YukikazeCombo;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Yukikaze)
                return actionID;

            if (SAM_Yukaze_KenkiOvercap && CanWeave() &&
                Kenki >= SAM_Yukaze_KenkiOvercapAmount && LevelChecked(Shinten))
                return OriginalHook(Shinten);

            if (HasStatusEffect(Buffs.MeikyoShisui))
            {
                if (SAM_Yukaze_Gekko &&
                    LevelChecked(Gekko) &&
                    (!HasStatusEffect(Buffs.Fugetsu) ||
                     !HasGetsu && HasStatusEffect(Buffs.Fuka)))
                    return Gekko;

                if (SAM_Yukaze_Kasha &&
                    LevelChecked(Kasha) &&
                    (!HasStatusEffect(Buffs.Fuka) ||
                     !HasKa && HasStatusEffect(Buffs.Fugetsu)))
                    return Kasha;

                if (LevelChecked(Yukikaze) && !HasSetsu)
                    return Yukikaze;
            }

            if (ComboTimer > 0)
            {
                if (ComboAction is Hakaze or Gyofu && LevelChecked(Jinpu))
                {
                    if (SAM_Yukaze_Gekko &&
                        !LevelChecked(Kasha) &&
                        (RefreshFugetsu || !HasStatusEffect(Buffs.Fugetsu)) ||
                        LevelChecked(Kasha) &&
                        (!HasStatusEffect(Buffs.Fugetsu) ||
                         HasStatusEffect(Buffs.Fuka) && !HasGetsu ||
                         SenCount is 3 && RefreshFugetsu))
                        return Jinpu;

                    if (SAM_Yukaze_Kasha &&
                        LevelChecked(Shifu) &&
                        (!LevelChecked(Kasha) &&
                         (RefreshFuka || !HasStatusEffect(Buffs.Fuka)) ||
                         LevelChecked(Kasha) &&
                         (!HasStatusEffect(Buffs.Fuka) ||
                          HasStatusEffect(Buffs.Fugetsu) && !HasKa ||
                          SenCount is 3 && RefreshFuka)))
                        return Shifu;

                    if (LevelChecked(Yukikaze))
                        return OriginalHook(Yukikaze);
                }

                if (ComboAction is Jinpu && LevelChecked(Gekko))
                    return Gekko;

                if (IsEnabled(Preset.SAM_ST_Kasha) &&
                    ComboAction is Shifu && LevelChecked(Kasha))
                    return Kasha;
            }

            return OriginalHook(Hakaze);
        }
    }

    internal class SAM_ST_KashaCombo : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_ST_KashaCombo;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Kasha)
                return actionID;

            if (SAM_Kasha_KenkiOvercap && CanWeave() &&
                Kenki >= SAM_Kasha_KenkiOvercapAmount && LevelChecked(Shinten))
                return OriginalHook(Shinten);

            if (HasStatusEffect(Buffs.MeikyoShisui) && LevelChecked(Kasha))
                return OriginalHook(Kasha);

            if (ComboTimer > 0)
            {
                if (ComboAction == OriginalHook(Hakaze) && LevelChecked(Shifu))
                    return OriginalHook(Shifu);

                if (ComboAction is Shifu && LevelChecked(Kasha))
                    return OriginalHook(Kasha);
            }

            return OriginalHook(Hakaze);
        }
    }

    internal class SAM_ST_GeckoCombo : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_ST_GekkoCombo;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Gekko)
                return actionID;

            if (SAM_Gekko_KenkiOvercap && CanWeave() &&
                Kenki >= SAM_Gekko_KenkiOvercapAmount && LevelChecked(Shinten))
                return OriginalHook(Shinten);

            if (HasStatusEffect(Buffs.MeikyoShisui) && LevelChecked(Gekko))
                return OriginalHook(Gekko);

            if (ComboTimer > 0)
            {
                if (ComboAction == OriginalHook(Hakaze) && LevelChecked(Jinpu))
                    return OriginalHook(Jinpu);

                if (ComboAction is Jinpu && LevelChecked(Gekko))
                    return OriginalHook(Gekko);
            }

            return OriginalHook(Hakaze);
        }
    }

    internal class SAM_AoE_OkaCombo : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_AoE_OkaCombo;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Oka)
                return actionID;

            if (SAM_Oka_KenkiOvercap &&
                Kenki >= SAM_Oka_KenkiOvercapAmount &&
                LevelChecked(Kyuten) && CanWeave())
                return Kyuten;

            if (HasStatusEffect(Buffs.MeikyoShisui) ||
                ComboTimer > 0 && LevelChecked(Oka) &&
                ComboAction == OriginalHook(Fuko))
                return Oka;

            return OriginalHook(Fuko);
        }
    }

    internal class SAM_AoE_MangetsuCombo : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_AoE_MangetsuCombo;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Mangetsu)
                return actionID;

            if (SAM_Mangetsu_KenkiOvercap && Kenki >= SAM_Mangetsu_KenkiOvercapAmount &&
                LevelChecked(Kyuten) && CanWeave())
                return Kyuten;

            if (HasStatusEffect(Buffs.MeikyoShisui))
            {
                if (!HasGetsu && HasStatusEffect(Buffs.Fuka) ||
                    !HasStatusEffect(Buffs.Fugetsu))
                    return Mangetsu;

                if (SAM_Mangetsu_Oka &&
                    (!HasKa && HasStatusEffect(Buffs.Fugetsu) ||
                     !HasStatusEffect(Buffs.Fuka)))
                    return Oka;
            }

            if (ComboTimer > 0 &&
                ComboAction is Fuko or Fuga && LevelChecked(Mangetsu))
            {
                if (!SAM_Mangetsu_Oka ||
                    !HasGetsu || RefreshFugetsu ||
                    !HasStatusEffect(Buffs.Fugetsu) ||
                    !LevelChecked(Oka))
                    return Mangetsu;

                if (SAM_Mangetsu_Oka &&
                    LevelChecked(Oka) &&
                    (!HasKa || RefreshFuka ||
                     !HasStatusEffect(Buffs.Fuka)))
                    return Oka;
            }

            return OriginalHook(Fuko);
        }
    }

    internal class SAM_MeikyoSens : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_MeikyoSens;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not MeikyoShisui || !HasStatusEffect(Buffs.MeikyoShisui))
                return actionID;

            if (!HasStatusEffect(Buffs.Fugetsu) ||
                !HasGetsu)
                return Gekko;

            if (!HasStatusEffect(Buffs.Fuka) ||
                !HasKa)
                return Kasha;

            if (!HasSetsu)
                return Yukikaze;

            return actionID;
        }
    }

    internal class SAM_MeikyoShisuiProtection : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_MeikyoShisuiProtection;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not MeikyoShisui)
                return actionID;

            return HasStatusEffect(Buffs.MeikyoShisui) &&
                   ActionReady(MeikyoShisui)
                ? All.SavageBlade
                : actionID;
        }
    }

    internal class SAM_Iaijutsu : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_Iaijutsu;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Iaijutsu)
                return actionID;

            bool canAddShoha = IsEnabled(Preset.SAM_Iaijutsu_Shoha) &&
                               ActionReady(Shoha) &&
                               MeditationStacks is 3;

            if (canAddShoha && CanWeave())
                return Shoha;

            if (IsEnabled(Preset.SAM_Iaijutsu_OgiNamikiri) &&
                (ActionReady(OgiNamikiri) && HasStatusEffect(Buffs.OgiNamikiriReady) || NamikiriReady))
                return OriginalHook(OgiNamikiri);

            if (IsEnabled(Preset.SAM_Iaijutsu_TsubameGaeshi) &&
                SenCount is not 1 &&
                (LevelChecked(TsubameGaeshi) &&
                 (HasStatusEffect(Buffs.TsubameReady) ||
                  HasStatusEffect(Buffs.KaeshiGokenReady)) ||
                 LevelChecked(TendoKaeshiSetsugekka) &&
                 (HasStatusEffect(Buffs.TendoKaeshiSetsugekkaReady) ||
                  HasStatusEffect(Buffs.TendoKaeshiGokenReady))))
                return OriginalHook(TsubameGaeshi);

            if (canAddShoha)
                return Shoha;

            return actionID;
        }
    }

    internal class SAM_Shinten : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_Shinten;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Shinten)
                return actionID;

            if (IsEnabled(Preset.SAM_Shinten_Shoha) &&
                ActionReady(Shoha) && MeditationStacks is 3)
                return Shoha;

            if (IsEnabled(Preset.SAM_Shinten_Senei) &&
                ActionReady(Senei))
                return Senei;

            if (IsEnabled(Preset.SAM_Shinten_Zanshin) &&
                HasStatusEffect(Buffs.ZanshinReady))
                return Zanshin;

            return actionID;
        }
    }

    internal class SAM_Kyuten : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_Kyuten;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Kyuten)
                return actionID;

            if (IsEnabled(Preset.SAM_Kyuten_Guren) &&
                ActionReady(Guren))
                return Guren;

            if (IsEnabled(Preset.SAM_Kyuten_Zanshin) &&
                HasStatusEffect(Buffs.ZanshinReady))
                return Zanshin;

            if (IsEnabled(Preset.SAM_Kyuten_Shoha) &&
                ActionReady(Shoha) && MeditationStacks is 3)
                return Shoha;

            return actionID;
        }
    }

    internal class SAM_Ikishoten : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_Ikishoten;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Ikishoten)
                return actionID;

            if (IsEnabled(Preset.SAM_Ikishoten_Shoha) &&
                ActionReady(Shoha) &&
                HasStatusEffect(Buffs.OgiNamikiriReady) &&
                MeditationStacks is 3)
                return Shoha;

            if (IsEnabled(Preset.SAM_Ikishoten_Namikiri) &&
                (ActionReady(OgiNamikiri) && HasStatusEffect(Buffs.OgiNamikiriReady) || NamikiriReady))
                return OriginalHook(OgiNamikiri);

            return actionID;
        }
    }

    internal class SAM_GyotenYaten : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_GyotenYaten;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Gyoten)
                return actionID;

            if (Kenki >= 10)
            {
                if (InMeleeRange())
                    return Yaten;

                if (!InMeleeRange())
                    return Gyoten;
            }

            return actionID;
        }
    }

    internal class SAM_SeneiGuren : CustomCombo
    {
        protected internal override Preset Preset => Preset.SAM_SeneiGuren;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not Senei)
                return actionID;

            return !LevelChecked(Senei)
                ? Guren
                : actionID;
        }
    }
}
