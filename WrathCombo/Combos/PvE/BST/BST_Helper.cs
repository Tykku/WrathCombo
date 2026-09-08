using System.Collections.Generic;
using System.Linq;
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
        BorrowBeast = 47238,
        BorrowVile = 47239,
        BorrowCloud = 47240,
        BorrowSeed = 47241,
        BorrowWave = 47242,
        BorrowScale = 47243,
        BorrowSoul = 47244,
        BorrowAsh = 47245;

    public static class Tricks
    {
        public const uint
            Cusith_Rake = 44935,
            Squirrel_SomersaultSlash = 44937,
            Lamb_FleeceButt = 44939,
            Pugil_Screwdriver = 44941,
            Opoopo_StoneThrow = 44943,
            Dodo_FowlStench = 44945,
            Coblyn_BestialThunder = 44947,
            Diremite_DeadlyThrust = 44949,
            Megalocrab_DrenchingBlow = 44951,
            Wespe_SharpSting = 44953,
            Vulture_WingCutter = 44955,
            Mandragora_Budbutt = 44957,
            Geshunpest_DarkThunder = 44959,
            Puk_Fireball = 44961,
            Crab_BubbleShower = 44963,
            Mantis_StandingChine = 44965,
            Slime_Digest = 44967,
            Dullahan_IronJustice = 44969,
            Bat_BloodDrain = 44971,
            Flyingtrap_SourSough = 44973,
            Ziz_IceBreath = 44975,
            Sabotender_NaturalNeedles = 44977,
            Golem_BoulderClap = 44979,
            Apkallu_FlyingSardine = 44981,
            Adamantoise_BestialThunderII = 44983,
            Buffalo_Heave = 44985,
            Uragnite_FrostBreath = 44987,
            Worm_SandBreath = 44989,
            Spriggan_Romp = 44991,
            Goobbue_Beatdown = 44993,
            Gigantoad_BestialBlizzardII = 44995,
            Colibri_Loop = 44997,
            Coeurl_Blaster = 44999,
            Raptor_FrostBreath = 45001,
            Drake_BurningCyclone = 45003,
            Treant_AcornBomb = 45005,
            Antling_MandibleBite = 45007,
            Chimera_theLionsBreath = 45009,
            Morbol_VineProbe = 45011,
            Ghost_FellGale = 45013,
            Salamander_BrackishRain = 45015,
            Cobra_DrippingFang = 45017,
            Hydra_MainTrap = 45019,
            Damselfly_CursedSphere = 45021,
            Rottinggoobbue_DirtySneeze = 45023,
            Zu_FlyingFrenzy = 45025,
            Icegolem_IceGuillotine = 45027,
            Karlabos_Impale = 45029,
            Rafflesia_BloodyCaress = 45031,
            Behemoth_Thunderbolt = 45033;
    }

    public static class TemperedRelease
    {
        public const uint
            Cusith_RelentlessRake = 44936,
            Squirrel_Scamper = 44938,
            Lamb_Lullaby = 44940,
            Pugil_WaterWall = 44942,
            Opoopo_PinsandNails = 44944,
            Dodo_Strut = 44946,
            Coblyn_Vulcanize = 44948,
            Diremite_Silkscreen = 44950,
            Megalocrab_BubbleShower = 44952,
            Wespe_FinalSting = 44954,
            Vulture_BloodcurdlingCaw = 44956,
            Mandragora_HeirloomScream = 44958,
            Geshunpest_Odium = 44960,
            Puk_TailChase = 44962,
            Crab_HundredFists = 44964,
            Mantis_EerieSoundwave = 44966,
            Slime_Syrup = 44968,
            Dullahan_KingsWill = 44970,
            Bat_Ultrasonics = 44972,
            Flyingtrap_NecroticNectar = 44974,
            Ziz_Petribreath = 44976,
            Sabotender_HypodermicHustle = 44978,
            Golem_Rockslide = 44980,
            Apkallu_Regurgitate = 44982,
            Adamantoise_HardenShell = 44984,
            Buffalo_WarCry = 44986,
            Uragnite_GasShell = 44988,
            Worm_BottomlessDesert = 44990,
            Spriggan_FreneticFlurry = 44992,
            Goobbue_MoldySneeze = 44994,
            Gigantoad_StickyTongue = 44996,
            Colibri_PeckingFlurry = 44998,
            Coeurl_ChargedWhisker = 45000,
            Raptor_FoulBreath = 45002,
            Drake_SmolderingScales = 45004,
            Treant_ArborealStorm = 45006,
            Antling_FormicPheromones = 45008,
            Chimera_theRamsVoice = 45010,
            Morbol_BadBreath = 45012,
            Ghost_Curse = 45014,
            Salamander_PeculiarLight = 45016,
            Cobra_StoneGaze = 45018,
            Hydra_WhiteBreath = 45020,
            Damselfly_Venom = 45022,
            Rottinggoobbue_Inhale = 45024,
            Zu_BreathWing = 45026,
            Icegolem_FrozenHeart = 45028,
            Karlabos_TailScrew = 45030,
            Rafflesia_BlightedBouquet = 45032,
            Behemoth_Meteor = 45034;

    }

    private static List<uint> RampantTricks =
    [
         Tricks.Cusith_Rake,
         Tricks.Squirrel_SomersaultSlash,
         Tricks.Lamb_FleeceButt,
         Tricks.Opoopo_StoneThrow,
         Tricks.Diremite_DeadlyThrust,
         Tricks.Mandragora_Budbutt,
         Tricks.Puk_Fireball,
         Tricks.Sabotender_NaturalNeedles,
         Tricks.Buffalo_Heave,
         Tricks.Spriggan_Romp,
         Tricks.Goobbue_Beatdown,
         Tricks.Drake_BurningCyclone,
         Tricks.Antling_MandibleBite,
         Tricks.Chimera_theLionsBreath,
         Tricks.Morbol_VineProbe,
    ];

    private static List<uint> EldritchTricks =
    [
         Tricks.Dodo_FowlStench,
         Tricks.Coblyn_BestialThunder,
         Tricks.Geshunpest_DarkThunder,
         Tricks.Slime_Digest,
         Tricks.Golem_BoulderClap,
         Tricks.Adamantoise_BestialThunderII,
         Tricks.Worm_SandBreath,
         Tricks.Gigantoad_BestialBlizzardII,
         Tricks.Coeurl_Blaster,
         Tricks.Treant_AcornBomb,
         Tricks.Rottinggoobbue_DirtySneeze,
         Tricks.Rafflesia_BloodyCaress,
         Tricks.Behemoth_Thunderbolt,
    ];

    private static List<uint> DurantTricks =
    [
         Tricks.Pugil_Screwdriver,
         Tricks.Megalocrab_DrenchingBlow,
         Tricks.Crab_BubbleShower,
         Tricks.Mantis_StandingChine,
         Tricks.Dullahan_IronJustice,
         Tricks.Ziz_IceBreath,
         Tricks.Apkallu_FlyingSardine,
         Tricks.Uragnite_FrostBreath,
         Tricks.Raptor_FrostBreath,
         Tricks.Salamander_BrackishRain,
         Tricks.Cobra_DrippingFang,
         Tricks.Hydra_MainTrap,
         Tricks.Icegolem_IceGuillotine,
         Tricks.Karlabos_Impale,

    ];

    private static List<uint> VolantTricks =
    [
         Tricks.Wespe_SharpSting,
         Tricks.Vulture_WingCutter,
         Tricks.Bat_BloodDrain,
         Tricks.Flyingtrap_SourSough,
         Tricks.Colibri_Loop,
         Tricks.Ghost_FellGale,
         Tricks.Damselfly_CursedSphere,
         Tricks.Zu_FlyingFrenzy,
    ];

    public static bool TrickIsDurant()
    {
        return DurantTricks.Any(x => x == OriginalHook(Trick));
    }

    public static bool TrickIsEldritch()
    {
        return EldritchTricks.Any(x => x == OriginalHook(Trick));
    }

    public static bool TrickIsVolant()
    {
        return VolantTricks.Any(x => x == OriginalHook(Trick));
    }

    public static bool TrickIsRampant()
    {
        return RampantTricks.Any(x => x == OriginalHook(Trick));
    }

}