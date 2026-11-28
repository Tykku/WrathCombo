using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using ECommons.GameHelpers;
using System;
using System.Linq;
using System.Numerics;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using WrathCombo.Combos.PvE;
using WrathCombo.Core;
using WrathCombo.CustomComboNS;
using WrathCombo.CustomComboNS.Functions;
using WrathCombo.Extensions;
using WrathCombo.Native;
using static WrathCombo.Combos.PvP.NINPvP.Config;
using static WrathCombo.Window.Functions.UserConfig;

namespace WrathCombo.Combos.PvP;

internal static class NINPvP
{
    #region IDS
    internal class Role : PvPMelee;
    internal const uint
        SpinningEdge = 29500,
        GustSlash = 29501,
        AeolianEdge = 29502,
        FumaShuriken = 29505,
        Dokumori = 41451,
        ThreeMudra = 29507,
        Bunshin = 29511,
        Shukuchi = 29513,
        SeitonTenchu = 29515,
        SeitonTenchuUnsealed = 29516, // Follow-up cast while under Seiton Unsealed
        ForkedRaiju = 29510,
        FleetingRaiju = 29707,
        HyoshoRanryu = 29506,
        GokaMekkyaku = 29504,
        Meisui = 29508,
        Huton = 29512,
        Doton = 29514,
        Assassinate = 29503,
        ZeshoMeppo = 41452;

    internal class Buffs
    {
        internal const ushort
            ThreeMudra = 1317,
            Hidden = 1316,
            Bunshin = 2010,
            ShadeShift = 2011,
            SeitonUnsealed = 3192,
            FleetingRaijuReady = 3211,
            ZeshoMeppoReady = 4305;
    }
    internal class Debuffs
    {
        internal const ushort
            SealedHyoshoRanryu = 3194,
            SealedGokaMekkyaku = 3193,
            SealedHuton = 3196,
            SealedDoton = 3197,
            SealedForkedRaiju = 3195,
            SealedMeisui = 3198,
            Dokumori = 4303;
    }
    #endregion

    #region Config
    public static class Config
    {
        public static UserInt
            NINPvP_Meisui_ST = new("NINPvP_Meisui_ST"),
            NINPvP_ST_FumaShuriken_RangedCharges = new("NINPvP_ST_FumaShuriken_RangedCharges"),
            NINPvP_Meisui_AoE = new("NINPvP_Meisui_AoE"),
            NINPvP_AoE_FumaShuriken_RangedCharges = new("NINPvP_AoE_FumaShuriken_RangedCharges"),
            NINPVP_SeitonTenchu = new("NINPVP_SeitonTenchu"),
            NINPVP_SeitonTenchuAoE = new("NINPVP_SeitonTenchuAoE"),
            NINPvP_SeitonTenchu_HPLockThreshold = new("NINPvP_SeitonTenchu_HPLockThreshold", 50),
            NINPvP_SeitonTenchu_CameraConeAngle = new("NINPvP_SeitonTenchu_CameraConeAngle", 90),
            NINPvP_SmiteThreshold = new("NINPvP_SmiteThreshold");

        public static UserBoolArray
            NINPvP_ST_MudraOption = new("NINPvP_ST_MudraOption", [true, true, true]),
            NINPvP_AoE_MudraOption = new("NINPvP_AoE_MudraOption", [true, true, true]);

        public static UserIntArray
            NINPvP_ST_MudraPriority = new("NINPvP_ST_MudraPriority", [1, 2, 3]),
            NINPvP_AoE_MudraPriority = new("NINPvP_AoE_MudraPriority", [1, 2, 3]);

        internal static void Draw(Preset preset)
        {
            switch (preset)
            {
                case Preset.NINPvP_ST_MudraMode:
                    DrawHorizontalMultiChoice(NINPvP_ST_MudraOption, "Huton", "Use Huton in the Mudra sequence.", 3, 0);
                    DrawPriorityInput(NINPvP_ST_MudraPriority, 3, 0, "Huton Priority");
                    DrawHorizontalMultiChoice(NINPvP_ST_MudraOption, "Hyosho Ranryu", "Use Hyosho Ranryu in the Mudra sequence.", 3, 1);
                    DrawPriorityInput(NINPvP_ST_MudraPriority, 3, 1, "Hyosho Ranryu Priority");
                    DrawHorizontalMultiChoice(NINPvP_ST_MudraOption, "Forked Raiju", "Use Forked Raiju in the Mudra sequence.", 3, 2);
                    DrawPriorityInput(NINPvP_ST_MudraPriority, 3, 2, "Forked Raiju Priority");
                    break;

                case Preset.NINPvP_AoE_MudraMode:
                    DrawHorizontalMultiChoice(NINPvP_AoE_MudraOption, "Huton", "Use Huton in the Mudra sequence.", 3, 0);
                    DrawPriorityInput(NINPvP_AoE_MudraPriority, 3, 0, "Huton Priority");
                    DrawHorizontalMultiChoice(NINPvP_AoE_MudraOption, "Doton", "Use Doton in the Mudra sequence.", 3, 1);
                    DrawPriorityInput(NINPvP_AoE_MudraPriority, 3, 1, "Doton Priority");
                    DrawHorizontalMultiChoice(NINPvP_AoE_MudraOption, "Goka Mekkyaku", "Use Goka Mekkyaku in the Mudra sequence.", 3, 2);
                    DrawPriorityInput(NINPvP_AoE_MudraPriority, 3, 2, "Goka Mekkyaku Priority");
                    break;
                case Preset.NINPvP_ST_SeitonTenchu:
                    DrawSliderInt(1, 50, NINPVP_SeitonTenchu, "Target's HP% to be at or under", 200);
                    break;
                case Preset.NINPvP_AoE_SeitonTenchu:
                    DrawSliderInt(1, 50, NINPVP_SeitonTenchuAoE, "Target's HP% to be at or under", 200);
                    break;
                case Preset.NINPvP_SeitonTenchu_HPLock:
                    DrawSliderInt(1, 100, NINPvP_SeitonTenchu_HPLockThreshold, "Target's HP% to be at or under before the Limit Break is allowed", 200);
                    break;
                case Preset.NINPvP_SeitonTenchu_HPLock_Retarget_CameraCone:
                    DrawSliderInt(30, 180, NINPvP_SeitonTenchu_CameraConeAngle, "Total cone width in degrees (90 = 45 either side of where the camera faces)", 200);
                    break;
                case Preset.NINPvP_Smite:
                    DrawSliderInt(0, 100, NINPvP_SmiteThreshold,
                        "Target HP% to smite, Max damage below 25%");
                    break;
                case Preset.NINPvP_ST_FumaShuriken:
                    DrawSliderInt(0, 3, NINPvP_ST_FumaShuriken_RangedCharges,
                        "How many charges to retain for Ranged only. Set 0 to use all in melee. Set 3 use at range only.");
                    break;

                case Preset.NINPvP_AoE_FumaShuriken:
                    DrawSliderInt(0, 3, NINPvP_AoE_FumaShuriken_RangedCharges,
                        "How many charges to retain for Ranged only. Set 0 to use all in melee. Set 3 use at range only.");
                    break;

                case Preset.NINPvP_ST_Meisui:
                    string descriptionST = "Set the HP percentage to be at or under for the feature to kick in.\n100% is considered to start at 8,000 less than your max HP to prevent wastage.";

                    if (Player.Object != null)
                    {
                        uint maxHP = Player.Object.MaxHp <= 8000 ? 0 : Player.Object.MaxHp - 8000;
                        if (maxHP > 0)
                        {
                            float hpThreshold = (float)maxHP / 100 * NINPvP_Meisui_ST;

                            descriptionST += $"\nHP Value to be at or under: {hpThreshold}";
                        }
                    }
                    DrawSliderInt(1, 100, NINPvP_Meisui_ST, descriptionST);
                    break;

                case Preset.NINPvP_AoE_Meisui:
                    string descriptionAoE = "Set the HP percentage to be at or under for the feature to kick in.\n100% is considered to start at 8,000 less than your max HP to prevent wastage.";

                    if (Player.Object != null)
                    {
                        uint maxHP = Player.Object.MaxHp <= 8000 ? 0 : Player.Object.MaxHp - 8000;
                        if (maxHP > 0)
                        {
                            float hpThreshold = (float)maxHP / 100 * NINPvP_Meisui_AoE;

                            descriptionAoE += $"\nHP Value to be at or under: {hpThreshold}";
                        }
                    }
                    DrawSliderInt(1, 100, NINPvP_Meisui_AoE, descriptionAoE);
                    break;
            }
        }
    }
    #endregion

    internal class NINPvP_ST_BurstMode : CustomCombo
    {
        protected internal override Preset Preset => Preset.NINPvP_ST_BurstMode;

        protected override uint Invoke(uint actionID)
        {
            if (!CustomActionHelper.OneButtonRotationChecker(actionID, CustomActionType.SingleTargetDPS, SpinningEdge, GustSlash, AeolianEdge))
                return actionID;

            // Cached variables for repeated conditions
            var bunshinStacks = LocalPlayer.HasStatus(Buffs.Bunshin) ? LocalPlayer.Status(Buffs.Bunshin).Stacks : 0;
            bool mudraMode = LocalPlayer.HasStatus(Buffs.ThreeMudra);
            var jobMaxHp = LocalPlayer.MaxHp;
            var maxHPThreshold = jobMaxHp - 8000;
            float remainingPercentage = (float)LocalPlayer.CurrentHp / maxHPThreshold;
            bool inMeisuiRange = NINPvP_Meisui_ST >= remainingPercentage * 100;

            // Hidden state actions
            if (LocalPlayer.HasStatus(Buffs.Hidden))
                return OriginalHook(Assassinate);

            if (!PvPCommon.TargetImmuneToDamage())
            {
                // Seiton Tenchu priority for targets below 50% HP
                if (IsEnabled(Preset.NINPvP_ST_SeitonTenchu) && GetTargetHPPercent() < NINPVP_SeitonTenchu &&
                    (IsLB1Ready || LocalPlayer.HasStatus(Buffs.SeitonUnsealed)))  // Limit Break or Unsealed buff
                    return OriginalHook(SeitonTenchu);

                //Smite
                if (IsEnabled(Preset.NINPvP_Smite) && PvPMelee.CanSmite() && InActionRange(PvPMelee.Smite) && HasTarget() &&
                    GetTargetHPPercent() <= NINPvP_SmiteThreshold)
                    return PvPMelee.Smite;

                // Zesho Meppo
                if (LocalPlayer.HasStatus(Buffs.ZeshoMeppoReady) && InMeleeRange())
                    return ZeshoMeppo;

                if (CanWeave())
                {
                    // Melee range actions
                    if (IsEnabled(Preset.NINPvP_ST_Dokumori) && InActionRange(Dokumori) && ActionReady(Dokumori))
                        return OriginalHook(Dokumori);

                    // Bunshin
                    if (IsEnabled(Preset.NINPvP_ST_Bunshin) && ActionReady(Bunshin))
                        return OriginalHook(Bunshin);

                    // Three Mudra
                    if (IsEnabled(Preset.NINPvP_ST_ThreeMudra) && HasCharges(ThreeMudra) && !mudraMode)
                    {
                        if (!IsEnabled(Preset.NINPvP_ST_ThreeMudraPool) || LocalPlayer.HasStatus(Buffs.Bunshin))
                            return OriginalHook(ThreeMudra);
                    }
                }
                // Mudra mode actions
                if (mudraMode)
                {
                    if (IsEnabled(Preset.NINPvP_ST_Meisui) && inMeisuiRange && !LocalPlayer.HasStatus(Debuffs.SealedMeisui))
                        return OriginalHook(Meisui);

                    if (IsEnabled(Preset.NINPvP_ST_MudraMode))
                    {
                        (uint Action, ushort SealedDebuff, Func<bool> Logic)[] PrioritizedMudras =
                        [
                            (Huton, Debuffs.SealedHuton, () => true),
                            (HyoshoRanryu, Debuffs.SealedHyoshoRanryu, () => true),
                            (ForkedRaiju, Debuffs.SealedForkedRaiju, () => bunshinStacks > 0)
                        ];

                        var sortedIndices = ((int[])NINPvP_ST_MudraPriority)
                                                    .Select((val, idx) => new { val, idx })
                                                    .OrderBy(x => x.val)
                                                    .Select(x => x.idx);
                        foreach (int index in sortedIndices)
                        {
                            if (index >= 0 && index < PrioritizedMudras.Length)
                            {
                                var mudra = PrioritizedMudras[index];
                                if (NINPvP_ST_MudraOption[index] && !LocalPlayer.HasStatus(mudra.SealedDebuff) && mudra.Logic())
                                    return OriginalHook(mudra.Action);
                            }
                        }
                    }
                }
                // Fuma Shuriken
                if (IsEnabled(Preset.NINPvP_ST_FumaShuriken) && !LocalPlayer.HasStatus(Buffs.ThreeMudra) && !LocalPlayer.HasStatus(Buffs.FleetingRaijuReady) && HasCharges(FumaShuriken) &&
                    (!InMeleeRange() && GetRemainingCharges(FumaShuriken) > 0 ||
                     InMeleeRange() && GetRemainingCharges(FumaShuriken) > NINPvP_ST_FumaShuriken_RangedCharges))
                    return OriginalHook(FumaShuriken);
            }

            if (LocalPlayer.HasStatus(Buffs.Hidden))
                return Assassinate;

            if (LocalPlayer.HasStatus(Buffs.ThreeMudra))
                return ForkedRaiju;

            if (ActionReady(FleetingRaiju))
                return FleetingRaiju;

            if (LocalPlayer.HasStatus(Buffs.ZeshoMeppoReady))
                return ZeshoMeppo;

            if (ActionReady(GustSlash))
                return GustSlash;

            if (ActionReady(AeolianEdge))
                return AeolianEdge;

            return SpinningEdge;
        }
    }

    internal class NINPvP_AoE_BurstMode : CustomCombo
    {
        protected internal override Preset Preset => Preset.NINPvP_AoE_BurstMode;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not FumaShuriken)
                return actionID;

            bool mudraMode = LocalPlayer.HasStatus(Buffs.ThreeMudra);
            var jobMaxHp = LocalPlayer.MaxHp;
            var maxHPThreshold = jobMaxHp - 8000;
            var remainingPercentage = (float)LocalPlayer.CurrentHp / (float)maxHPThreshold;
            bool inMeisuiRange = NINPvP_Meisui_AoE >= remainingPercentage * 100;
            bool hasBunshin = LocalPlayer.HasStatus(Buffs.Bunshin);

            if (LocalPlayer.HasStatus(Buffs.Hidden))
                return OriginalHook(Assassinate);

            if (!PvPCommon.TargetImmuneToDamage())
            {
                // Seiton Tenchu priority for targets below 50% HP
                if (IsEnabled(Preset.NINPvP_AoE_SeitonTenchu) && GetTargetHPPercent() < (NINPVP_SeitonTenchu) && IsLB1Ready)
                    return OriginalHook(SeitonTenchu);

                if (CanWeave())
                {
                    // Overarching Priority: Bunshin first
                    if (IsEnabled(Preset.NINPvP_AoE_Bunshin) && !GetCooldown(Bunshin).IsCooldown)
                        return OriginalHook(Bunshin);

                    // Dokumori requires Bunshin and range check (8y)
                    if (IsEnabled(Preset.NINPvP_AoE_Dokumori) && hasBunshin && IsInRange(null, 8) && !GetCooldown(Dokumori).IsCooldown)
                        return OriginalHook(Dokumori);

                    // Three Mudra waits for Bunshin
                    if (IsEnabled(Preset.NINPvP_AoE_ThreeMudra) && HasCharges(ThreeMudra) && !mudraMode)
                    {
                        if (!IsEnabled(Preset.NINPvP_AoE_ThreeMudraPool) || hasBunshin)
                            return OriginalHook(ThreeMudra);
                    }
                }

                if (mudraMode)
                {
                    if (IsEnabled(Preset.NINPvP_AoE_MudraMode))
                    {
                        if (IsEnabled(Preset.NINPvP_AoE_Meisui) && inMeisuiRange && !LocalPlayer.HasStatus(Debuffs.SealedMeisui))
                            return OriginalHook(Meisui);

                        (uint Action, ushort SealedDebuff, Func<bool> Logic)[] PrioritizedMudras =
                        [
                            (Huton, Debuffs.SealedHuton, () => true),
                            (Doton, Debuffs.SealedDoton, () => IsInRange(null, 8)),
                            (GokaMekkyaku, Debuffs.SealedGokaMekkyaku, () => GetTargetDistance() <= 20)
                        ];

                        var sortedIndices = ((int[])NINPvP_AoE_MudraPriority)
                                                    .Select((val, idx) => new { val, idx })
                                                    .OrderBy(x => x.val)
                                                    .Select(x => x.idx);

                        foreach (int index in sortedIndices)
                        {
                            if (index >= 0 && index < PrioritizedMudras.Length)
                            {
                                var mudra = PrioritizedMudras[index];
                                if (NINPvP_AoE_MudraOption[index] && !LocalPlayer.HasStatus(mudra.SealedDebuff) && mudra.Logic())
                                    return OriginalHook(mudra.Action);
                            }
                        }
                    }
                    else return actionID;  // if automatic is not enabled and in mudra mode, ensures fuma shuriken is the option so mudras can be properly chosen
                }

                if (IsEnabled(Preset.NINPvP_AoE_FumaShuriken) && !LocalPlayer.HasStatus(Buffs.FleetingRaijuReady) && HasCharges(FumaShuriken) &&
                    (!InMeleeRange() && GetRemainingCharges(FumaShuriken) > 0 ||
                     InMeleeRange() && GetRemainingCharges(FumaShuriken) > NINPvP_AoE_FumaShuriken_RangedCharges))
                    return OriginalHook(FumaShuriken);

                if (InMeleeRange()) // Melee Combo
                {
                    switch (ComboAction)
                    {
                        case GustSlash:
                            return OriginalHook(AeolianEdge);
                        case SpinningEdge:
                            return OriginalHook(GustSlash);
                        default:
                            return OriginalHook(SpinningEdge);
                    }
                }
            }
            return actionID;
        }
    }

    internal class NINPvP_SeitonTenchu_HPLock : CustomCombo
    {
        protected internal override Preset Preset => Preset.NINPvP_SeitonTenchu_HPLock;

        protected override uint Invoke(uint actionID)
        {
            if (actionID is not SeitonTenchu)
                return actionID;

            // 29515 normally, 29516 while under Seiton Unsealed
            uint lb = OriginalHook(SeitonTenchu);

            // Retarget onto any enemy in range at or under the threshold
            if (IsEnabled(Preset.NINPvP_SeitonTenchu_HPLock_Retarget))
            {
                if (SeitonRetargetTarget is { } lbTarget)
                    return lb.Retarget(SeitonTenchu, lbTarget);

                return All.Cease;
            }

            // Block the LB while the target is above the HP threshold or can't be hurt
            if (HasTarget() && !SeitonTargetAllowed(CurrentTarget))
                return All.Cease;

            return lb;
        }

        /// <summary> Current target first if it qualifies, otherwise the lowest HP% enemy in range under the threshold. </summary>
        private static IBattleChara? SeitonRetargetTarget =>
            Svc.Objects.OfType<IBattleChara>()
                .Where(x => x.IsHostile() &&
                            x.IsTargetable &&
                            !x.IsDead &&
                            InActionRange(OriginalHook(SeitonTenchu), x) &&
                            SeitonTargetAllowed(x) &&
                            (!IsEnabled(Preset.NINPvP_SeitonTenchu_HPLock_Retarget_CameraCone) ||
                             InCameraCone(x, NINPvP_SeitonTenchu_CameraConeAngle)))
                .OrderByDescending(x => x.GameObjectId == CurrentTarget?.GameObjectId)
                .ThenBy(x => GetTargetHPPercent(x))
                .FirstOrDefault();

        /// <summary>
        /// False if the target has an immunity (Hallowed Ground, Undead Redemption) or Hardened Scales.
        /// Also false for a DRK at 1 HP, since Eventide drops them to 1 HP and Undead Redemption can show up a moment later.
        /// Guard is ignored on purpose: Seiton Tenchu ignores Guard.
        /// </summary>
        internal static bool SeitonCanHurt(IGameObject? target) =>
            target is not null &&
            !PvPCommon.TargetImmuneToDamage(false, target) &&
            !target.HasStatus(VPRPvP.Buffs.HardenedScales, true) &&
            !(target is IBattleChara { CurrentHp: <= 1 } chara && (ECommons.ExcelServices.Job)chara.ClassJob.RowId == ECommons.ExcelServices.Job.DRK);

        /// <summary> At or under the HP Lock threshold and hurtable. Also used as the final check when the action is sent. </summary>
        internal static bool SeitonTargetAllowed(IGameObject? target) =>
            target is not null &&
            GetTargetHPPercent(target) <= NINPvP_SeitonTenchu_HPLockThreshold &&
            SeitonCanHurt(target);

        /// <summary> True if the target is within a horizontal cone (total width in degrees) centred on the camera's facing, measured from the player. </summary>
        private static unsafe bool InCameraCone(IGameObject target, float coneDegrees)
        {
            var manager = CameraManager.Instance();
            if (manager == null || manager->Camera == null || LocalPlayer is not { } player)
                return true;

            var scene = manager->Camera->SceneCamera;
            var forward = new Vector2(scene.LookAtVector.X - scene.Position.X, scene.LookAtVector.Z - scene.Position.Z);
            var toTarget = new Vector2(target.Position.X - player.Position.X, target.Position.Z - player.Position.Z);

            if (forward.LengthSquared() < 0.0001f || toTarget.LengthSquared() < 0.0001f)
                return true;

            float halfAngle = coneDegrees * 0.5f * MathF.PI / 180f;
            return Vector2.Dot(Vector2.Normalize(forward), Vector2.Normalize(toTarget)) >= MathF.Cos(halfAngle);
        }
    }

    internal class NINPvP_MeisuiOnRecuperate : CustomCombo
    {
        protected internal override Preset Preset => Preset.NINPvP_MeisuiOnRecuperate;

        protected override uint Invoke(uint actionID)
        {
            // During Three Mudra the game would turn this into Meisui; show Cease instead so it can't be double cast
            if (actionID is ThreeMudra)
                return LocalPlayer.HasStatus(Buffs.ThreeMudra) ? All.Cease : ThreeMudra;

            // Recuperate becomes Meisui during Three Mudra
            if (actionID is PvPCommon.Recuperate &&
                LocalPlayer.HasStatus(Buffs.ThreeMudra) &&
                !LocalPlayer.HasStatus(Debuffs.SealedMeisui))
                return Meisui;

            return actionID;
        }
    }
}
