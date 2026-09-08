using Dalamud.Game.ClientState.JobGauge.Types;
using ECommons;
using ECommons.DalamudServices;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using WrathCombo.Combos.PvE.ALL;
using WrathCombo.CustomComboNS;
using WrathCombo.CustomComboNS.Functions;
using WrathCombo.Data;
using WrathCombo.Extensions;
using static WrathCombo.Combos.PvE.BST.Config;
using static WrathCombo.CustomComboNS.Functions.CustomComboFunctions;
namespace WrathCombo.Combos.PvE;

internal partial class BST
{

    public const uint
        SmashAxe = 44879,
        Capture = 44880,
        FirstBattlehorn = 44881,
        Gauge = 44882,
        AxebladeBite = 44883,
        AvalancheAxe = 44884,
        Shieldsplitter = 44885,
        BeastMode = 44886,
        MistralAxe = 44887,
        SpinningAxe = 44888,
        GaleAxe = 44889,
        TemperedRelease1 = 44890, //Not actually a 1, but same name as TemperedRelease2
        PartingBlow = 44891,
        SecondBattlehorn = 44892,
        ShieldCharge = 44893,
        ThirdBattlehorn = 44894,
        Borrow = 44895,
        Beastskin = 44896,
        Vileskin = 44897,
        CloudSkim = 44898,
        Seedsower = 44899,
        QuellingWave = 44900,
        Scaleskin = 44901,
        SoulCrush = 44902,
        ScouringAsh = 44903,
        RallyingCheer = 44904,
        Rally = 44905,
        BrutalRage = 44930,
        HawkishTalons = 44931,
        RisenFall = 44932,
        Calamity = 44933,
        TemperedRelease2 = 47092, //Not actually a 2, but same name as TemperedRelease1
        Trick = 47093,
        Borrow1 = 47238, //No idea what these borrows link up to at the moment, they're all just called Borrow in the sheets
        Borrow2 = 47239,
        Borrow3 = 47240,
        Borrow4 = 47241,
        Borrow5 = 47242,
        Borrow6 = 47243,
        Borrow7 = 47244,
        Borrow8 = 47245;


}