#region Dependencies
using Dalamud.Game.ClientState.JobGauge.Types;
using System;
using System.Collections.Generic;
using WrathCombo.CustomComboNS;
using WrathCombo.CustomComboNS.Functions;
using WrathCombo.Extensions;
using static WrathCombo.Combos.PvE.PCT.Config;
using static WrathCombo.CustomComboNS.Functions.CustomComboFunctions;
#endregion

namespace WrathCombo.Combos.PvE;

internal partial class PCT
{
    #region Variables
    [Flags] private enum RotationMode 
    { simpleST = 1 << 0, advancedST = 1 << 1, simpleAoE = 1 << 2, advancedAoE = 1 << 3 }
    
    internal static PCTGauge gauge = GetJobGauge<PCTGauge>();
    internal static bool ScenicMuseReady => gauge.LandscapeMotifDrawn && ActionReady(ScenicMuse);
    internal static bool LivingMuseReady => ActionReady(LivingMuse) && gauge.CreatureMotifDrawn;
    internal static bool SteelMuseReady => ActionReady(SteelMuse) && !HasStatusEffect(Buffs.HammerTime) && gauge.WeaponMotifDrawn;
    internal static bool PortraitReady => ActionReady(MogoftheAges) && (gauge.MooglePortraitReady || gauge.MadeenPortraitReady);
    internal static bool CreatureMotifReady => !gauge.CreatureMotifDrawn && LevelChecked(CreatureMotif) && !HasStatusEffect(Buffs.StarryMuse);
    internal static bool WeaponMotifReady => !gauge.WeaponMotifDrawn && LevelChecked(WeaponMotif) && !HasStatusEffect(Buffs.StarryMuse) && !HasStatusEffect(Buffs.HammerTime);
    internal static bool LandscapeMotifReady => !gauge.LandscapeMotifDrawn && LevelChecked(LandscapeMotif) && !HasStatusEffect(Buffs.StarryMuse);
    internal static bool PaletteReady => SubtractivePalette.LevelChecked() && !HasStatusEffect(Buffs.SubtractivePalette) && !HasStatusEffect(Buffs.MonochromeTones) && 
                                            (HasStatusEffect(Buffs.SubtractiveSpectrum) || 
                                             gauge.PalleteGauge >= 50 && ScenicCD > 35 || 
                                             gauge.PalleteGauge == 100 && HasStatusEffect(Buffs.Aetherhues2)|| 
                                             gauge.PalleteGauge >= 50 && ScenicCD < 3 );
    internal static bool HasPaint => gauge.Paint > 0;
    internal static float ScenicCD => GetCooldownRemainingTime(StarryMuse);
    internal static float SteelCD => GetCooldownRemainingTime(StrikingMuse);
    #endregion
    
    #region Rotation
    #region OGCD Spells
    private static bool TryOGCDSpells(RotationMode rotationFlags, ref uint actionID)
    {
        #region Enables
        bool subtractivePaletteEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_StarPrism) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_StarPrism) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        bool scenicMuseEnabled = 
            IsEnabled(Preset.PCT_ST_AdvancedMode_ScenicMuse) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_ScenicMuse) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        bool livingMuseEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_RainbowDrip) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_RainbowDrip) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        bool steelMuseEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_CometinBlack) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_CometinBlack) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        bool portraitEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_HammerStampCombo) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_HammerStampCombo) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        bool LucidDreamingEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_HammerStampCombo) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_HammerStampCombo) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        #endregion
        
        #region Configs
        
        int scenicThresholdST = PCT_ST_AdvancedMode_ScenicMuse_SubOption == 1 || !InBossEncounter() ? PCT_ST_AdvancedMode_ScenicMuse_Threshold : 0;
        int scenicThresholdAoE = PCT_AoE_AdvancedMode_ScenicMuse_SubOption == 1 || !InBossEncounter() ? PCT_AoE_AdvancedMode_ScenicMuse_Threshold : 0;
        int scenicStop = 
            rotationFlags.HasFlag(RotationMode.advancedST) ? scenicThresholdST : 
            rotationFlags.HasFlag(RotationMode.advancedAoE) ? scenicThresholdAoE : 
            0;
        
        bool scenicMovementPrevention =
            rotationFlags.HasFlag(RotationMode.advancedST) ? PCT_ST_AdvancedMode_ScenicMuse_MovementOption : 
            rotationFlags.HasFlag(RotationMode.advancedAoE) ? PCT_AoE_AdvancedMode_ScenicMuse_MovementOption : 
            false;
        
        int lucidThreshold = 
            rotationFlags.HasFlag(RotationMode.advancedST) ? PCT_ST_AdvancedMode_LucidOption : 
            rotationFlags.HasFlag(RotationMode.advancedAoE) ? PCT_AoE_AdvancedMode_LucidOption : 
            6500;
        
        #endregion
        
        if (InCombat() && HasBattleTarget())
        {
            // SubtractivePalette
            if (subtractivePaletteEnabled && CanWeave() && PaletteReady)
            {
                actionID = SubtractivePalette;
                return true;
            }

            // ScenicMuse
            if (scenicMuseEnabled && ScenicMuseReady && CanDelayedWeave() && GetTargetHPPercent() > scenicStop &&
                (!IsMoving() || !scenicMovementPrevention))
            {
                actionID = OriginalHook(ScenicMuse);
                return true;
            }

            // LivingMuse
            if (livingMuseEnabled && LivingMuseReady && CanWeave() && !JustUsed(StarryMuse) &&
                (!PortraitReady || GetRemainingCharges(LivingMuse) == GetMaxCharges(LivingMuse)) &&
                (!LevelChecked(ScenicMuse) || ScenicCD > GetCooldownChargeRemainingTime(LivingMuse) ||
                 !scenicMuseEnabled))
            {
                actionID = OriginalHook(LivingMuse);
                return true;
            }

            // SteelMuse
            if (steelMuseEnabled && SteelMuseReady && CanWeave() &&
                (SteelCD < ScenicCD || GetRemainingCharges(SteelMuse) == GetMaxCharges(SteelMuse) ||
                 !LevelChecked(ScenicMuse)))
            {
                actionID = OriginalHook(SteelMuse);
                return true;
            }

            // Portrait Mog or Madeen
            if (portraitEnabled && PortraitReady && CanWeave() && IsOffCooldown(OriginalHook(MogoftheAges)) &&
                !JustUsed(StarryMuse) &&
                (ScenicCD >= 60 || !LevelChecked(ScenicMuse) || !scenicMuseEnabled))
            {
                actionID = OriginalHook(MogoftheAges);
                return true;
            }
        }
        //LucidDreaming
        if (LucidDreamingEnabled && Role.CanLucidDream(lucidThreshold))
        {
            actionID = Role.LucidDreaming;
            return true;
        }
        return false;
    }
    #endregion
    
    #region Mitigation
    private static bool TryMitigation(RotationMode rotationFlags, ref uint actionID)
    {
        #region Enables
        bool addleEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_Addle) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            rotationFlags.HasFlag(RotationMode.simpleST);
        
        bool tempuraEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_Tempura) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            rotationFlags.HasFlag(RotationMode.simpleST);
        
        #endregion

        if (addleEnabled && Role.CanAddle() && CanWeave() && GroupDamageIncoming() && HasBattleTarget())
        {
            actionID = Role.Addle;
            return true;
        }

        if (tempuraEnabled && CanWeave() && GroupDamageIncoming() && !JustUsed(Role.Addle, 6))
        {
            if (LevelChecked(TempuraCoat) && IsOffCooldown(TempuraCoat))
            {
                actionID = TempuraCoat;
                return true;
            }
                    
            if (LevelChecked(TempuraGrassa) && IsInParty() &&
                NumberOfAlliesInRange(TempuraGrassa) >= GetPartyMembers().Count * .75 &&
                HasStatusEffect(Buffs.TempuraCoat))
            {
                actionID = TempuraGrassa;
                return true;
            }
        }
        return false;
    }
    #endregion
    
    #region Movement

    private static bool TryMovementOption(RotationMode rotationFlags, ref uint actionID)
    {
        #region Enables

        bool movementEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_MovementFeature) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_MovementFeature) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);

        bool rainbowDripEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_RainbowDrip) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_RainbowDrip) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);

        bool hammerStampEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_MovementOption_HammerStampCombo) &&
            rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_MovementOption_HammerStampCombo) &&
            rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);

        bool cometinBlackEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_MovementOption_CometinBlack) &&
            rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_MovementOption_CometinBlack) &&
            rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);

        bool holyInWhiteEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_MovementOption_HolyInWhite) &&
            rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_MovementOption_HolyInWhite) &&
            rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);

        bool swiftcastEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_SwiftcastOption) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_SwiftcastOption) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);

        #endregion

        if (!movementEnabled || !IsMoving() || !InCombat()) return false;
        
        if (rainbowDripEnabled && HasStatusEffect(Buffs.RainbowBright))
        {
            actionID = OriginalHook(RainbowDrip);
            return true;
        }

        if (hammerStampEnabled && LevelChecked(HammerStamp) && HasStatusEffect(Buffs.HammerTime) &&
            !HasStatusEffect(Buffs.Hyperphantasia))
        {
            actionID = OriginalHook(HammerStamp);
            return true;
        }

        if (cometinBlackEnabled && HasStatusEffect(Buffs.MonochromeTones) && HasPaint)
        {
            actionID = OriginalHook(CometinBlack);
            return true;
        }

        if (holyInWhiteEnabled && HasPaint)
        {
            actionID = OriginalHook(HolyInWhite);
            return true;
        }

        if (swiftcastEnabled && ActionReady(Role.Swiftcast) &&
            (CreatureMotifReady || WeaponMotifReady || LandscapeMotifReady))
        {
            actionID = Role.Swiftcast;
            return true;
        }
        
        return false;
        
    }
    #endregion
    
    #region GCD Spells
    private static bool TryGCDSpells(RotationMode rotationFlags, ref uint actionID)
    {
        #region Enables
        bool starPrismEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_StarPrism) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_StarPrism) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        bool rainbowDripEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_RainbowDrip) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_RainbowDrip) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        bool cometInBlackEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_CometinBlack) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_CometinBlack) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        bool hammerStampComboEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_HammerStampCombo) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_HammerStampCombo) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        bool scenicMuseEnabled = 
            IsEnabled(Preset.PCT_ST_AdvancedMode_ScenicMuse) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_ScenicMuse) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        #endregion
        
        //Star Prism
        if (starPrismEnabled && HasStatusEffect(Buffs.Starstruck) && !JustUsed(StarryMuse))
        {
            actionID = StarPrism;
            return true;
        }

        //Rainbow Drip
        if (rainbowDripEnabled && HasStatusEffect(Buffs.RainbowBright))
        {
            actionID = RainbowDrip;
            return true;
        }
       
        //Comet in Black
        if (cometInBlackEnabled && HasStatusEffect(Buffs.MonochromeTones) && HasPaint && !JustUsed(StarryMuse) &&
            (!HasStatusEffect(Buffs.StarryMuse) || HasStatusEffect(Buffs.Hyperphantasia)) &&
            (ScenicCD > 10 || !LevelChecked(ScenicMuse) || !scenicMuseEnabled))
        {
            actionID = OriginalHook(CometinBlack);
            return true;
        }
        
        //Hammer Stamp Combo
        if (hammerStampComboEnabled && ActionReady(OriginalHook(HammerStamp)) &&
            !HasStatusEffect(Buffs.Hyperphantasia) &&
            (ScenicCD > 10 || !LevelChecked(ScenicMuse) || IsNotEnabled(Preset.PCT_ST_AdvancedMode_ScenicMuse)))
        {
            actionID = OriginalHook(HammerStamp);
            return true;
        }
        
        return false;
    }
    #endregion
    
    #region Motifs
    private static bool TryDrawMotif(RotationMode rotationFlags, ref uint actionID)
    {
        #region Enables
        bool motifsEnabled = 
            IsEnabled(Preset.PCT_ST_AdvancedMode_MotifFeature) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_MotifFeature) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
            
        bool prepullEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_PrePullMotifs) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_PrePullMotifs) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        bool noTargetEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_NoTargetMotifs) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_NoTargetMotifs) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        bool swiftcastEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_SwiftMotifs) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_SwiftMotifs) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        bool creatureEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_CreatureMotif) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_CreatureMotif) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        bool weaponEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_WeaponMotif) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_WeaponMotif) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        bool landscapeEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_LandscapeMotif) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_LandscapeMotif) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        
        
        #endregion
        
        #region Configs
        
        int creatureStop = 
                rotationFlags.HasFlag(RotationMode.advancedST) ? PCT_ST_CreatureStop : 
                rotationFlags.HasFlag(RotationMode.advancedAoE) ? PCT_AoE_CreatureStop : 
                0;
        bool creatureHealthCheck = GetTargetHPPercent() > creatureStop;
        bool hasLivingMuseCharges = HasCharges(LivingMuse) || GetCooldownChargeRemainingTime(LivingMuse) <= 8;
        
        int weaponStop = 
            rotationFlags.HasFlag(RotationMode.advancedST) ? PCT_ST_WeaponStop : 
            rotationFlags.HasFlag(RotationMode.advancedAoE) ? PCT_AoE_WeaponStop : 
            0;
        bool weaponHealthCheck = GetTargetHPPercent() > weaponStop;
        bool hasSteelMuseCharges = HasCharges(SteelMuse) || GetCooldownChargeRemainingTime(SteelMuse) <= 8;
        
        int landscapeStop = 
            rotationFlags.HasFlag(RotationMode.advancedST) ? PCT_ST_LandscapeStop : 
            rotationFlags.HasFlag(RotationMode.advancedAoE) ? PCT_AoE_LandscapeStop : 
            0;
        bool landscapeHealthCheck = GetTargetHPPercent() > landscapeStop;
        
        #endregion
        
        if (motifsEnabled)
        {
            if (creatureEnabled && CreatureMotifReady &&
                (prepullEnabled && !InCombat() ||
                 noTargetEnabled && InCombat() && CurrentTarget == null ||
                 swiftcastEnabled && HasStatusEffect(Role.Buffs.Swiftcast) && creatureHealthCheck ||
                 LevelChecked(ScenicMuse) && GetCooldownRemainingTime(ScenicMuse) <= 20 &&
                 creatureHealthCheck || //Burst Prep
                 hasLivingMuseCharges && creatureHealthCheck)) //Standard Use
            {
                actionID = OriginalHook(CreatureMotif);
                return true;
            }

            if (weaponEnabled && WeaponMotifReady &&
                (prepullEnabled && !InCombat() ||
                 noTargetEnabled && InCombat() && CurrentTarget == null ||
                 swiftcastEnabled && HasStatusEffect(Role.Buffs.Swiftcast) && weaponHealthCheck ||
                 LevelChecked(ScenicMuse) && GetCooldownRemainingTime(ScenicMuse) <= 20 && weaponHealthCheck ||
                 hasSteelMuseCharges && weaponHealthCheck))
            {
                actionID = OriginalHook(WeaponMotif);
                return true;
            }

            if (landscapeEnabled && LandscapeMotifReady &&
                (prepullEnabled && !InCombat() ||
                 noTargetEnabled && InCombat() && CurrentTarget == null ||
                 swiftcastEnabled && HasStatusEffect(Role.Buffs.Swiftcast) && landscapeHealthCheck ||
                 LevelChecked(ScenicMuse) && GetCooldownRemainingTime(ScenicMuse) <= 20 && landscapeHealthCheck))
            {
                actionID = OriginalHook(LandscapeMotif);
                return true;
            }
        }
        return false;
    }
    #endregion
    
    #region SubCombos and Holy in White
    private static bool TryCombos(RotationMode rotationFlags, ref uint actionID)
    {
        #region Enables
        bool subComboEnabled = 
            IsEnabled(Preset.PCT_ST_AdvancedMode_BlizzardInCyan) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_BlizzardInCyan) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
            
        bool holyInWhiteEnabled =
            IsEnabled(Preset.PCT_ST_AdvancedMode_HolyinWhite) && rotationFlags.HasFlag(RotationMode.advancedST) ||
            IsEnabled(Preset.PCT_AoE_AdvancedMode_HolyinWhite) && rotationFlags.HasFlag(RotationMode.advancedAoE) ||
            rotationFlags.HasFlag(RotationMode.simpleST) || rotationFlags.HasFlag(RotationMode.simpleAoE);
        #endregion
        
        #region Configs
        int holdPaintCharges =
                rotationFlags.HasFlag(RotationMode.advancedST) 
                    ? PCT_ST_AdvancedMode_HolyinWhiteOption
                    : rotationFlags.HasFlag(RotationMode.advancedAoE)
                        ? PCT_AoE_AdvancedMode_HolyinWhiteOption
                        : 2;
        #endregion

        if (rotationFlags.HasFlag(RotationMode.advancedST) || rotationFlags.HasFlag(RotationMode.simpleST))
        {
            if (subComboEnabled && HasStatusEffect(Buffs.SubtractivePalette))
            {
                actionID = OriginalHook(BlizzardinCyan);
                return true;
            }
            if (holyInWhiteEnabled && !HasStatusEffect(Buffs.MonochromeTones) && gauge.Paint > holdPaintCharges && NumberOfEnemiesInRange(HolyInWhite) > 1)
            {
                actionID = OriginalHook(HolyInWhite);
                return true;
            }
            return false;
        }
        
        if (rotationFlags.HasFlag(RotationMode.advancedAoE) || rotationFlags.HasFlag(RotationMode.simpleAoE))
        {
            if (subComboEnabled && HasStatusEffect(Buffs.SubtractivePalette))
            {
                actionID = OriginalHook(BlizzardIIinCyan);
                return true;
            }

            if (holyInWhiteEnabled && !HasStatusEffect(Buffs.MonochromeTones) && gauge.Paint > holdPaintCharges)
            {
                actionID = OriginalHook(HolyInWhite);
                return true;
            }
        }
        return false;
    }
    #endregion
    #endregion

    #region ID's
    public const uint
        BlizzardinCyan = 34653,
        StoneinYellow = 34654,
        BlizzardIIinCyan = 34659,
        ClawMotif = 34666,
        ClawedMuse = 34672,
        CometinBlack = 34663,
        CreatureMotif = 34689,
        FireInRed = 34650,
        AeroInGreen = 34651,
        WaterInBlue = 34652,
        FireIIinRed = 34656,
        AeroIIinGreen = 34657,
        HammerMotif = 34668,
        WingedMuse = 34671,
        StrikingMuse = 34674,
        StarryMuse = 34675,
        HammerStamp = 34678,
        HammerBrush = 34679,
        PolishingHammer = 34680,
        HolyInWhite = 34662,
        StarrySkyMotif = 34669,
        LandscapeMotif = 34691,
        LivingMuse = 35347,
        MawMotif = 34667,
        MogoftheAges = 34676,
        PomMotif = 34664,
        PomMuse = 34670,
        RainbowDrip = 34688,
        RetributionoftheMadeen = 34677,
        ScenicMuse = 35349,
        Smudge = 34684,
        StarPrism = 34681,
        SteelMuse = 35348,
        SubtractivePalette = 34683,
        StoneIIinYellow = 34660,
        TempuraCoat = 34685,
        TempuraGrassa = 34686,
        ThunderIIinMagenta = 34661,
        ThunderinMagenta = 34655,
        WaterinBlue = 34652,
        WeaponMotif = 34690,
        WingMotif = 34665;

    public static class Buffs
    {
        public const ushort
            SubtractivePalette = 3674,
            Aetherhues2 = 3676,
            RainbowBright = 3679,
            HammerTime = 3680,
            MonochromeTones = 3691,
            StarryMuse = 3685,
            TempuraCoat = 3686,
            Hyperphantasia = 3688,
            Inspiration = 3689,
            SubtractiveSpectrum = 3690,
            Starstruck = 3681;
    }

    public static class Debuffs
    {

    }
    #endregion

    #region Openers
    internal static PCT2ndStarryMaxLvl SecondStarryMaxLvl = new();
    internal static PCT3rdStarryMaxLvl ThirdStarryMaxLvl = new();
    internal static PCT2ndStarryLvl90 SecondStarryLvl90 = new();
    internal static PCT3rdStarryLvl90 ThirdStarryLvl90 = new();
    
    internal static WrathOpener Opener()
    {
        if (SecondStarryLvl90.LevelChecked && PCT_Opener_Choice == 0)
            return SecondStarryLvl90;

        if (ThirdStarryLvl90.LevelChecked && PCT_Opener_Choice == 1)
            return ThirdStarryLvl90;
        
        if (SecondStarryMaxLvl.LevelChecked && PCT_Opener_Choice == 0)
            return SecondStarryMaxLvl;
        
        if (ThirdStarryMaxLvl.LevelChecked && PCT_Opener_Choice == 1)
            return ThirdStarryMaxLvl;
        
        return WrathOpener.Dummy;
    }
    
    public static bool HasMotifs()
    {

        if (!gauge.CanvasFlags.HasFlag(Dalamud.Game.ClientState.JobGauge.Enums.CanvasFlags.Pom))
            return false;

        if (!gauge.CanvasFlags.HasFlag(Dalamud.Game.ClientState.JobGauge.Enums.CanvasFlags.Weapon))
            return false;

        if (!gauge.CanvasFlags.HasFlag(Dalamud.Game.ClientState.JobGauge.Enums.CanvasFlags.Landscape))
            return false;

        return true;
    }
    
    internal class PCT2ndStarryMaxLvl : WrathOpener
    {
        //2nd GCD Starry Opener
        public override int MinOpenerLevel => 100;
        public override int MaxOpenerLevel => 109;
        public override List<uint> OpenerActions { get; set; } =
        [
            RainbowDrip,
            PomMuse,
            StrikingMuse,
            WingMotif,
            StarryMuse, //5
            HammerStamp,
            SubtractivePalette,
            BlizzardinCyan,
            StoneinYellow,
            ThunderinMagenta,//10
            CometinBlack,
            WingedMuse,
            MogoftheAges,
            StarPrism,
            HammerBrush,//15
            PolishingHammer,
            RainbowDrip,
            Role.Swiftcast,
            ClawMotif,
            ClawedMuse,//20
        ];
        internal override UserData? ContentCheckConfig => PCT_Balance_Content;
        public override Preset Preset => Preset.PCT_ST_Advanced_Openers;
        public override List<(int[] Steps, uint NewAction, Func<bool> Condition)> SubstitutionSteps { get; set; } =
[
            ([8, 9, 10], BlizzardinCyan, () => OriginalHook(BlizzardinCyan) == BlizzardinCyan),
            ([8, 9, 10], StoneinYellow, () => OriginalHook(BlizzardinCyan) == StoneinYellow),
            ([8, 9, 10], ThunderinMagenta, () => OriginalHook(BlizzardinCyan) == ThunderinMagenta),
            ([11], HolyInWhite, () => !HasStatusEffect(Buffs.MonochromeTones)),
        ];
        public override List<(int[] Steps, Func<bool> Condition)> SkipSteps { get; set; } = [([17], () => !HasStatusEffect(Buffs.RainbowBright))];

        public override bool HasCooldowns()
        {
            if (!IsOffCooldown(StarryMuse))
                return false;

            if (GetRemainingCharges(LivingMuse) < 3)
                return false;

            if (GetRemainingCharges(SteelMuse) < 2)
                return false;

            if (!HasMotifs())
                return false;

            if (HasStatusEffect(Buffs.SubtractivePalette))
                return false;

            if (IsOnCooldown(Role.Swiftcast))
                return false;

            return true;
        }
    }
    
    internal class PCT3rdStarryMaxLvl : WrathOpener
    {
        //3rd GCD Starry Opener
        public override int MinOpenerLevel => 100;
        public override int MaxOpenerLevel => 109;
        public override List<uint> OpenerActions { get; set; } =
        [
            RainbowDrip,
            StrikingMuse,
            HolyInWhite,
            PomMuse,
            WingMotif, //5 
            StarryMuse,
            HammerStamp,
            SubtractivePalette,
            BlizzardinCyan,
            BlizzardinCyan, //10
            BlizzardinCyan,
            CometinBlack,
            WingedMuse,
            MogoftheAges,
            StarPrism, //15
            HammerBrush,
            PolishingHammer,
            RainbowDrip,
            FireInRed,
            Role.Swiftcast, //20
            ClawMotif,
            ClawedMuse
        ];
        internal override UserData? ContentCheckConfig => PCT_Balance_Content;

        public override List<(int[] Steps, Func<bool> Condition)> SkipSteps { get; set; } = [([18], () => !HasStatusEffect(Buffs.RainbowBright))];

        public override List<(int[] Steps, uint NewAction, Func<bool> Condition)> SubstitutionSteps { get; set; } =
        [
            ([3], CometinBlack, () => HasStatusEffect(Buffs.MonochromeTones)),
            ([9, 10, 11], BlizzardinCyan, () => OriginalHook(BlizzardinCyan) == BlizzardinCyan),
             ([9, 10, 11], StoneinYellow, () => OriginalHook(BlizzardinCyan) == StoneinYellow),
            ([9, 10, 11], ThunderinMagenta, () => OriginalHook(BlizzardinCyan) == ThunderinMagenta),
            ([12], HolyInWhite, () => !HasStatusEffect(Buffs.MonochromeTones)),
        ];
        public override Preset Preset => Preset.PCT_ST_Advanced_Openers;
        public override bool HasCooldowns()
        {
            if (!IsOffCooldown(StarryMuse))
                return false;

            if (GetRemainingCharges(LivingMuse) < 3)
                return false;

            if (GetRemainingCharges(SteelMuse) < 2)
                return false;

            if (!HasMotifs())
                return false;

            if (HasStatusEffect(Buffs.SubtractivePalette))
                return false;

            if (IsOnCooldown(Role.Swiftcast))
                return false;

            return true;
        }
    }
    
     internal class PCT2ndStarryLvl90 : WrathOpener
    {
        //2nd GCD Starry Opener
        public override int MinOpenerLevel => 90;
        public override int MaxOpenerLevel => 90;
        
        public override List<uint> OpenerActions { get; set; } =
        [
            FireInRed,
            StrikingMuse,
            AeroInGreen,
            StarryMuse,
            HammerStamp,
            PomMuse,
            SubtractivePalette,
            WingMotif,
            WingedMuse,
            HammerBrush,
            MogoftheAges,
            PolishingHammer,
            ThunderinMagenta,
            BlizzardinCyan,
            StoneinYellow,//15
            CometinBlack,
            WaterInBlue,
            FireInRed//20
        ];
        internal override UserData? ContentCheckConfig => PCT_Balance_Content;

        public override List<(int[] Steps, uint NewAction, Func<bool> Condition)> SubstitutionSteps { get; set; } =
[
            ([13, 14, 15], BlizzardinCyan, () => OriginalHook(BlizzardinCyan) == BlizzardinCyan),
            ([13, 14, 15], StoneinYellow, () => OriginalHook(BlizzardinCyan) == StoneinYellow),
            ([13, 14, 15], ThunderinMagenta, () => OriginalHook(BlizzardinCyan) == ThunderinMagenta),
            ([16], HolyInWhite, () => !HasStatusEffect(Buffs.MonochromeTones)),
        ];
        public override Preset Preset => Preset.PCT_ST_Advanced_Openers;
        public override bool HasCooldowns()
        {
            if (!IsOffCooldown(StarryMuse))
                return false;

            if (GetRemainingCharges(LivingMuse) < 2)
                return false;

            if (GetRemainingCharges(SteelMuse) < 2)
                return false;

            if (!HasMotifs())
                return false;

            if (HasStatusEffect(Buffs.SubtractivePalette))
                return false;

            return true;
        }
    }
     
     internal class PCT3rdStarryLvl90 : WrathOpener
    {
        //3rd GCD Starry Opener
        public override int MinOpenerLevel => 90;
        public override int MaxOpenerLevel => 90;
        public override List<uint> OpenerActions { get; set; } =
        [
            FireInRed,
            StrikingMuse,
            AeroInGreen,
            WaterInBlue,
            StarryMuse,
            HammerStamp,
            PomMuse,
            SubtractivePalette,
            WingMotif,
            WingedMuse,
            HammerBrush,
            MogoftheAges,
            PolishingHammer,
            BlizzardinCyan,
            StoneinYellow,
            ThunderinMagenta,
            CometinBlack,
            FireInRed,
            AeroInGreen,
            Role.Swiftcast, //20
            WaterInBlue
        ];
        internal override UserData? ContentCheckConfig => PCT_Balance_Content;
        
        public override List<(int[] Steps, uint NewAction, Func<bool> Condition)> SubstitutionSteps { get; set; } =
        [
            ([14, 15, 16], BlizzardinCyan, () => OriginalHook(BlizzardinCyan) == BlizzardinCyan),
             ([14, 15, 16], StoneinYellow, () => OriginalHook(BlizzardinCyan) == StoneinYellow),
            ([14,15,16], ThunderinMagenta, () => OriginalHook(BlizzardinCyan) == ThunderinMagenta),
            ([17], HolyInWhite, () => !HasStatusEffect(Buffs.MonochromeTones)),
        ];
        public override Preset Preset => Preset.PCT_ST_Advanced_Openers;
        public override bool HasCooldowns()
        {
            if (!IsOffCooldown(StarryMuse))
                return false;

            if (GetRemainingCharges(LivingMuse) < 2)
                return false;

            if (GetRemainingCharges(SteelMuse) < 2)
                return false;

            if (!HasMotifs())
                return false;

            if (HasStatusEffect(Buffs.SubtractivePalette))
                return false;

            if (IsOnCooldown(Role.Swiftcast))
                return false;

            return true;
        }
    }
     
#endregion
}