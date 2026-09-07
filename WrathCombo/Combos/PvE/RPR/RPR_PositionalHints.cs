using WrathCombo.API.Enum;
using WrathCombo.Extensions;
using static WrathCombo.Combos.PvE.RPR.Config;
using static WrathCombo.CustomComboNS.Functions.CustomComboFunctions;

namespace WrathCombo.Combos.PvE;

internal partial class RPR
{
    private static void ReportRPRPositionalHints()
    {
        if (!CanReportPositionalHints())
            return;

        if (LocalPlayer.HasStatus(Buffs.Enshrouded))
        {
            ClearUpcomingPositional();
            return;
        }

        if (!LocalPlayer.HasStatus(Buffs.SoulReaver) && !LocalPlayer.HasStatus(Buffs.Executioner))
            return;

        if (LocalPlayer.HasStatus(Buffs.EnhancedGibbet))
        {
            ReportUpcomingPositional(PositionalDirection.Flank, OriginalHook(Gibbet), 1);
            return;
        }

        if (LocalPlayer.HasStatus(Buffs.EnhancedGallows))
        {
            ReportUpcomingPositional(PositionalDirection.Rear, OriginalHook(Gallows), 1);
            return;
        }

        if (!ActionLearned(Gibbet))
            return;

        // Simple / Advanced Rear First → Gallows; Advanced Flank First → Gibbet
        bool preferGibbet = IsEnabled(Preset.RPR_ST_AdvancedMode) && RPR_Positional == 1;
        if (preferGibbet)
            ReportUpcomingPositional(PositionalDirection.Flank, OriginalHook(Gibbet), 1);
        else
            ReportUpcomingPositional(PositionalDirection.Rear, OriginalHook(Gallows), 1);
    }
}
