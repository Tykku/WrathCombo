using Dalamud.Game.ClientState.Conditions;
using ECommons.DalamudServices;
using ECommons.GameHelpers;

namespace WrathCombo.AutoRotation
{
    internal static class AutoRotationControllerHelpers
    {

        public static bool IsOccupied()
        {
            //Mirror of Ecommons version without some conditions that hang autorot in combat
            return Svc.Condition[ConditionFlag.Occupied]
                   || Svc.Condition[ConditionFlag.Occupied30]
                   || Svc.Condition[ConditionFlag.Occupied33]
                   || Svc.Condition[ConditionFlag.Occupied38]
                   || Svc.Condition[ConditionFlag.Occupied39]
                   || Svc.Condition[ConditionFlag.OccupiedInCutSceneEvent]
                   || Svc.Condition[ConditionFlag.OccupiedInEvent]
                   || Svc.Condition[ConditionFlag.OccupiedInQuestEvent]
                   || Svc.Condition[ConditionFlag.OccupiedSummoningBell]
                   || Svc.Condition[ConditionFlag.WatchingCutscene]
                   || Svc.Condition[ConditionFlag.WatchingCutscene78]
                   || Svc.Condition[ConditionFlag.BetweenAreas]
                   || Svc.Condition[ConditionFlag.BetweenAreas51]
                   || Svc.Condition[ConditionFlag.InThatPosition]
                   || Svc.Condition[ConditionFlag.Crafting]
                   || Svc.Condition[ConditionFlag.ExecutingCraftingAction]
                   || Svc.Condition[ConditionFlag.PreparingToCraft]
                   || Svc.Condition[ConditionFlag.InThatPosition]
                   || Svc.Condition[ConditionFlag.Unconscious]
                   || Svc.Condition[ConditionFlag.MeldingMateria]
                   || Svc.Condition[ConditionFlag.Gathering]
                   || Svc.Condition[ConditionFlag.OperatingSiegeMachine]
                   || Svc.Condition[ConditionFlag.CarryingItem]
                   || Svc.Condition[ConditionFlag.CarryingObject]
                   || Svc.Condition[ConditionFlag.BeingMoved]
                   || Svc.Condition[ConditionFlag.RidingPillion]
                   || Svc.Condition[ConditionFlag.Mounting]
                   || Svc.Condition[ConditionFlag.Mounting71]
                   || Svc.Condition[ConditionFlag.ParticipatingInCustomMatch]
                   || Svc.Condition[ConditionFlag.PlayingLordOfVerminion]
                   || Svc.Condition[ConditionFlag.ChocoboRacing]
                   || Svc.Condition[ConditionFlag.PlayingMiniGame]
                   || Svc.Condition[ConditionFlag.Performing]
                   || Svc.Condition[ConditionFlag.PreparingToCraft]
                   || Svc.Condition[ConditionFlag.Fishing]
                   || Svc.Condition[ConditionFlag.UsingHousingFunctions]
                   || !Player.Interactable;
        }
    }
}