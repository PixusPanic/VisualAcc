using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.EquipType;

namespace VisualAcc {
    public class VisualAcc : Mod {
        private static bool Debug()
        {
            bool isDebug = ModContent.GetInstance<VisualAccConfig>().Debug;
            
            #if DEBUG
            isDebug = true;
            #endif

            return isDebug;
        }

        // Thanks to Antirhinnum for optimizing and fixing a lot of code here

        /// <summary>
        /// Safely checks to see if a texture in the path exists, and if it does, adds it to the game.
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="textureName"></param>
        /// <param name="typeSlot"></param>
        /// <param name="itemName"></param>
        private void EquipVanillaTexture(string folderName, string textureName, EquipType typeSlot, string itemName)
        {
            var texturePath = Name + "/" + folderName + "/" + textureName;
            // If a texture exists in the folder, load it
            if (ModContent.HasAsset(texturePath))
                EquipLoader.AddEquipTexture(this, texturePath, typeSlot, name: itemName);
            else // Else, it doesn't exist, so simply say that the texture couldn't be found
            if (Debug()) Logger.Error($"Couldn't find a texture in the following path: {folderName}");
        }

        /// <summary>
        /// 0 is for the Back and DrawInBackpackLayer -
        /// 1 is for the Face and DrawInFaceHeadLayer -
        /// 2 is for DrawInFaceFlowerLayer -
        /// 3 is for PreventHairDraw
        /// </summary>
        /// <param name="equipName"></param>
        /// <param name="slotType"></param>
        private void ArmorIDsSettings(string equipName, int slotType) {
            switch (slotType) {
                case 0:
                    ArmorIDs.Back.Sets.DrawInBackpackLayer[EquipLoader.GetEquipSlot(this, equipName, Back)] = true;
                    break;

                case 1:
                    ArmorIDs.Face.Sets.DrawInFaceHeadLayer[EquipLoader.GetEquipSlot(this, equipName, Face)] = true;
                    break;

                case 2:
                    ArmorIDs.Face.Sets.DrawInFaceFlowerLayer[EquipLoader.GetEquipSlot(this, equipName, Face)] = true;
                    break;

                case 3:
                    ArmorIDs.Face.Sets.PreventHairDraw[EquipLoader.GetEquipSlot(this, equipName, Face)] = true;
                    break;
            }
        }

        #region Modded equips
        private static readonly HashSet<ModItem> ModdedEquips = [];

        private static readonly Dictionary<int, string> ModEquips = [];
        private static readonly Dictionary<int, string> ModAddingEquips = [];
        private static readonly Dictionary<int, string> ModEquipFolders = [];
        private static readonly Dictionary<int, EquipType> EquipTypes = [];

        /// <summary>
        /// Safely checks to see if a texture in the path exists, and if it does, adds it to the game.
        /// </summary>
        /// <param name="mod">The name of the mod that adds the item.</param>
        /// <param name="folderName"></param>
        /// <param name="itemName">The internal name of the modItem.</param>
        /// <param name="typeSlot"></param>
        private void EquipModdedTexture(string mod, string folderName, string itemName, EquipType typeSlot)
        {
            var texturePath = folderName + "/" + itemName;
            // Try to find the item
            if (ModContent.TryFind(mod, itemName, out ModItem item))
            {
                if (ModContent.HasAsset(texturePath))
                    EquipLoader.AddEquipTexture(this, texturePath, typeSlot, item);
                // Else, it doesn't exist, so simply say that the texture couldn't be found
                else if (Debug()) Logger.Error($"Couldn't find a texture for {item.DisplayName} in the following path: {texturePath}");
            } // The request item doesn't exist at all in this case
            else if (Debug()) Logger.Error($"Couldn't find the following item: {mod}/{itemName}");
        }
        #endregion

        public override void Load()
        {
            if (Main.dedServ) return;
            var textures = "Textures/Vanilla";

            #region Vanilla accessories
            #region Movement
            var folderName = "/Movement";
            EquipVanillaTexture(textures + folderName, "Aglet", Shoes, "eAglet");
            EquipVanillaTexture(textures + folderName, "AnkletOfTheWind", Shoes, "eAnklet");
            EquipVanillaTexture(textures + folderName, "LavaCharm", Shield, "eLavaCharm");
            EquipVanillaTexture(textures + folderName, "LuckyHorseshoe", Shoes, "eLuckyHonse");
            EquipVanillaTexture(textures + folderName, "MoltenCharm", Shield, "eMoltCharm");
            EquipVanillaTexture(textures + folderName, "NeptunesShell", Shield, "eNepShell");
            EquipVanillaTexture(textures + folderName, "ObsidianHorseshoe", Shoes, "eObsHonse");
            #endregion

            #region Informational
            folderName = "/Informational";
            EquipVanillaTexture(textures + folderName, "DepthMeter", Waist, "eDepth");
            EquipVanillaTexture(textures + folderName, "Compass", Waist, "eCompass");
            EquipVanillaTexture(textures + folderName, "Radar", Face, "eRadar");
            EquipVanillaTexture(textures + folderName, "LifeformAnalyzer", Back, "eLifeAnalyzer");
            EquipVanillaTexture(textures + folderName, "TallyCounter", Waist, "eTally");
            EquipVanillaTexture(textures + folderName, "MetalDetector", Waist, "eMetal");
            EquipVanillaTexture(textures + folderName, "Stopwatch", Waist, "eStopwatch");
            EquipVanillaTexture(textures + folderName, "DPSMeter", Front, "eDPS");
            EquipVanillaTexture(textures + folderName, "FishermansPocketGuide", Waist, "eFishGuide");
            EquipVanillaTexture(textures + folderName, "WeatherRadio", Beard, "eRadio");
            EquipVanillaTexture(textures + folderName, "Sextant", Shield, "eSextant");
            EquipVanillaTexture(textures + folderName, "GPS", Face, "eGPS");
            EquipVanillaTexture(textures + folderName, "REK3000", Face, "eREK");
            EquipVanillaTexture(textures + folderName, "GoblinTech", Front, "eGoblin");
            EquipVanillaTexture(textures + folderName, "FishFinder", Face, "eFishFinder");
            EquipVanillaTexture(textures + folderName, "PDA", Waist, "ePDA");
            EquipVanillaTexture(textures + folderName, "MechanicalLens", Face, "eLens");
            EquipVanillaTexture(textures + folderName, "MechanicalRuler", Waist, "eMechRuler");
            #endregion

            #region Health and Mana
            folderName = "/HealthMana";
            EquipVanillaTexture(textures + folderName, "CelestialMagnet", Waist, "eCelMagnet");
            EquipVanillaTexture(textures + folderName, "CelestialEmblem", Waist, "eCelEmblem");
            EquipVanillaTexture(textures + folderName, "MagnetFlower", Waist, "eMagnetFlower");
            EquipVanillaTexture(textures + folderName, "PhilosophersStone", Neck, "ePhilStone");
            #endregion

            #region Debuff warding
            folderName = "/Warding";
            EquipVanillaTexture(textures + folderName, "AdhesiveBandage", Face, "eAdhBandage");
            EquipVanillaTexture(textures + folderName, "AnkhCharm", Waist, "eAnkh");
            EquipVanillaTexture(textures + folderName, "ArmorBracingChest", Neck, "eBracingChest");
            EquipVanillaTexture(textures + folderName, "ArmorBracingPads", HandsOn, "eBracingPads");
            EquipVanillaTexture(textures + folderName, "Bezoar", HandsOn, "eBezoar");
            EquipVanillaTexture(textures + folderName, "CountercurseMantra", Shield, "eMantra");
            EquipVanillaTexture(textures + folderName, "FastClock", Waist, "eFastClock");
            EquipVanillaTexture(textures + folderName, "MedicatedBandage", Face, "eMedBandage");
            EquipVanillaTexture(textures + folderName, "Megaphone", Face, "eMegaphone");
            EquipVanillaTexture(textures + folderName, "Nazar", Neck, "eNazar");
            EquipVanillaTexture(textures + folderName, "PocketMirror", Waist, "eMirror");
            EquipVanillaTexture(textures + folderName, "ThePlan", Waist, "ePlan");
            EquipVanillaTexture(textures + folderName, "TrifoldMap", Waist, "eMap");
            EquipVanillaTexture(textures + folderName, "Vitamins", Waist, "eVitamins");
            #endregion

            #region Combat
            folderName = "/Combat";
            EquipVanillaTexture(textures + folderName, "AvengerEmblem", Neck, "eAvenger");
            EquipVanillaTexture(textures + folderName, "MoonCharm", Neck, "eMoonCharm");
            EquipVanillaTexture(textures + folderName, "MoonShell", Neck, "eMoonShell");
            EquipVanillaTexture(textures + folderName, "CelestialStone", Neck, "eCelStone");
            EquipVanillaTexture(textures + folderName, "CelestialShell", Neck, "eCelShell");
            EquipVanillaTexture(textures + folderName, "DestroyerEmblem", Neck, "eDestroy");
            EquipVanillaTexture(textures + folderName, "EyeoftheGolem", Face, "eGolem");
            EquipVanillaTexture(textures + folderName, "FleshKnucklesHandsOn", HandsOn, "eFleshOn");
            EquipVanillaTexture(textures + folderName, "FleshKnucklesHandsOff", HandsOff, "eFleshOff");
            EquipVanillaTexture(textures + folderName, "HoneyComb", Neck, "eComb");
            EquipVanillaTexture(textures + folderName, "MagmaStone", Neck, "eMagmaStone");
            EquipVanillaTexture(textures + folderName, "PutridScent", Waist, "eScent");
            EquipVanillaTexture(textures + folderName, "RangerEmblem", Neck, "eRanger");
            EquipVanillaTexture(textures + folderName, "ReconScope", Back, "eRecon");
            EquipVanillaTexture(textures + folderName, "RifleScope", Back, "eRifle");
            EquipVanillaTexture(textures + folderName, "SniperScope", Back, "eSniper");
            EquipVanillaTexture(textures + folderName, "SorcererEmblem", Neck, "eSorcerer");
            EquipVanillaTexture(textures + folderName, "SummonerEmblem", Neck, "eSummoner");
            EquipVanillaTexture(textures + folderName, "WarriorEmblem", Neck, "eWarrior");
            EquipVanillaTexture(textures + folderName, "HerculesBeetle", Neck, "eBeetle");
            EquipVanillaTexture(textures + folderName, "NecromanticScroll", Waist, "eScroll");
            EquipVanillaTexture(textures + folderName, "PapyrusScarab", Waist, "eScarab");
            #endregion

            #region Construction
            folderName = "/Construction";
            EquipVanillaTexture(textures + folderName, "PortableCementMixer", Back, "eCement");
            EquipVanillaTexture(textures + folderName, "AncientChisel", Shield, "eChisel");
            EquipVanillaTexture(textures + folderName, "ToolBox", Waist, "eBox");
            EquipVanillaTexture(textures + folderName, "PaintSprayer", Waist, "eSpray");
            EquipVanillaTexture(textures + folderName, "ExtendoGrip", HandsOn, "eGrip");
            EquipVanillaTexture(textures + folderName, "BrickLayer", Waist, "eLayer");
            EquipVanillaTexture(textures + folderName, "Presserator", HandsOn, "ePress");
            #endregion

            #region Fishing
            folderName = "/Fishing";
            EquipVanillaTexture(textures + folderName, "AnglerEarring", Face, "eEarring");
            EquipVanillaTexture(textures + folderName, "TackleBox", Waist, "eTackle");
            EquipVanillaTexture(textures + folderName, "LavaProofFishingHook", Face, "eLavaHook");
            #endregion

            #region Miscellaneous
            folderName = "/Miscellaneous";
            EquipVanillaTexture(textures + folderName, "YoyoBag", Waist, "eYoyoBag");
            EquipVanillaTexture(textures + folderName, "ClothierVoodooDoll", Waist, "eClothier");
            EquipVanillaTexture(textures + folderName, "GuideVoodooDoll", Waist, "eGuide");
            EquipVanillaTexture(textures + folderName, "RadioThing", Face, "eThing");

            #endregion

            #region Resource gathering and collecting
            folderName = "/Gathering";
            EquipVanillaTexture(textures + folderName, "CoinRing", HandsOn, "eCoin");
            EquipVanillaTexture(textures + folderName, "GoldRing", HandsOn, "eGold");
            EquipVanillaTexture(textures + folderName, "GreedyRing", HandsOn, "eGreedy");
            EquipVanillaTexture(textures + folderName, "GregBook", Waist, "eCordage");
            EquipVanillaTexture(textures + folderName, "DiscountCard", Waist, "eCard");
            EquipVanillaTexture(textures + folderName, "LuckyCoin", Neck, "eLuckCoin");
            #endregion

            #region Golf balls
            folderName = "/Golf";
            EquipVanillaTexture(textures + folderName, "GolfBallWhite", Face, "eWhite");
            EquipVanillaTexture(textures + folderName, "GolfBallBlack", Face, "eBlack");
            EquipVanillaTexture(textures + folderName, "GolfBallBlue", Face, "eBlue");
            EquipVanillaTexture(textures + folderName, "GolfBallBrown", Face, "eBrown");
            EquipVanillaTexture(textures + folderName, "GolfBallCyan", Face, "eCyan");
            EquipVanillaTexture(textures + folderName, "GolfBallGreen", Face, "eGreen");
            EquipVanillaTexture(textures + folderName, "GolfBallLime", Face, "eLime");
            EquipVanillaTexture(textures + folderName, "GolfBallOrange", Face, "eOrange");
            EquipVanillaTexture(textures + folderName, "GolfBallPink", Face, "ePink");
            EquipVanillaTexture(textures + folderName, "GolfBallPurple", Face, "ePurple");
            EquipVanillaTexture(textures + folderName, "GolfBallRed", Face, "eRed");
            EquipVanillaTexture(textures + folderName, "GolfBallSkyBlue", Face, "eSkyBlue");
            EquipVanillaTexture(textures + folderName, "GolfBallTeal", Face, "eTeal");
            EquipVanillaTexture(textures + folderName, "GolfBallViolet", Face, "eViolet");
            EquipVanillaTexture(textures + folderName, "GolfBallYellow", Face, "eYellow");
            #endregion

            #region Expert
            folderName = "/Expert";
            EquipVanillaTexture(textures + folderName, "RoyalGel", Face, "eRoyal");
            EquipVanillaTexture(textures + folderName, "BrainOfConfusion", Face, "eBrain");
            EquipVanillaTexture(textures + folderName, "VolatileGelatin", Face, "eVolatile");
            EquipVanillaTexture(textures + folderName, "SporeSacAlt", Face, "eSac");
            EquipVanillaTexture(textures + folderName, "ShinyStone", Neck, "eShiny");
            EquipVanillaTexture(textures + folderName, "SoaringInsignia", Neck, "eSoar");
            EquipVanillaTexture(textures + folderName, "GravityGlobe", Face, "eGravity");
            #endregion
            #endregion

            // Example line of how one can add compatibility - replace Name with an instance of your mod
            // You may have to make your mod sort before VisualAcc in build.txt
            /*if (ModContent.TryFind("ExampleMod", "ExampleGolfBall", out ModItem golfBall))
            {
                Call("AddEquip", Name, golfBall, "Textures/Example/", Face);
            }*/

            #region Modded accessories
            if (ModdedEquips.Count > 0)
                foreach (var equip in ModdedEquips)
                    EquipModdedTexture(equip.Mod.Name, ModAddingEquips[equip.Type] + "/" +
                        ModEquipFolders[equip.Type], equip.Name, EquipTypes[equip.Type]);
            #endregion
        }
        public override void PostSetupContent() {
            if (Main.dedServ) return;

            #region Set ArmorIDs sets for the added equips
            #region Back
            ArmorIDsSettings("eRecon", 0);
            ArmorIDsSettings("eRifle", 0);
            ArmorIDsSettings("eSniper", 0);
            ArmorIDsSettings("eCement", 0);
            #endregion

            #region Face
            ArmorIDsSettings("eMegaphone", 3);
            ArmorIDsSettings("eRoyal", 3);
            ArmorIDsSettings("eVolatile", 3);

            ArmorIDsSettings("eWhite", 2);
            ArmorIDsSettings("eBlack", 2);
            ArmorIDsSettings("eBlue", 2);
            ArmorIDsSettings("eBrown", 2);
            ArmorIDsSettings("eCyan", 2);
            ArmorIDsSettings("eGreen", 2);
            ArmorIDsSettings("eLime", 2);
            ArmorIDsSettings("eOrange", 2);
            ArmorIDsSettings("ePink", 2);
            ArmorIDsSettings("ePurple", 2);
            ArmorIDsSettings("eRed", 2);
            ArmorIDsSettings("eSkyBlue", 2);
            ArmorIDsSettings("eTeal", 2);
            ArmorIDsSettings("eViolet", 2);
            ArmorIDsSettings("eYellow", 2);
            ArmorIDsSettings("eSac", 2);

            ArmorIDsSettings("eBrain", 3);
            ArmorIDsSettings("eGravity", 3);
            #endregion
            #endregion
        }

        public override void Unload()
        {
            ModdedEquips.Clear();
            ModEquips.Clear();
            ModAddingEquips.Clear();
            ModEquipFolders.Clear();
            EquipTypes.Clear();
            EquipTypes.Clear();
        }

        private readonly string callError = "Visual Accessories call error: ";

        public override object Call(params object[] args)
        {
            try
            {
                if (args is null) Logger.Error(callError + "Arguments are null.");
                if (args.Length == 0) Logger.Error(callError + "Arguments are empty.");
                if (args[0] is not string context) return null;

                switch (context)
                {
                    case "AddEquip":
                        {
                            if (args[1] is not string modName)
                                throw new ArgumentException(callError + "ModdedEquipTexture paramater 1 (mod) should be a string");
                            if (args[2] is not ModItem item)
                                throw new ArgumentException(callError + "ModdedEquipTexture paramater 2 (item) should be a ModItem");
                            if (args[3] is not string folderName)
                                throw new ArgumentException(callError + "ModdedEquipTexture paramater 3 (folderName) should be a string");
                            if (args[4] is not EquipType typeSlot)
                                throw new ArgumentException(callError + "ModdedEquipTexture paramater 5 (typeSlot) should be an EquipType");
                            var mod = "";
                            mod = modName;

                            var modAdding = "";
                            if (args.Length > 5 && args[5] is not null)
                            {
                                if (args[5] is not string modAddingName)
                                    throw new ArgumentException(callError + "ModdedEquipTexture paramater 6 (modAddingEquips) should be a string or empty");
                                else modAdding = modAddingName;
                            }
                            else modAdding = mod;

                            ModdedEquips.Add(item);

                            ModEquips[item.Type] = mod;
                            ModEquipFolders[item.Type] = folderName;
                            EquipTypes[item.Type] = typeSlot;
                            ModAddingEquips[item.Type] = modAdding;

                            break;
                        }
                    default:
                        {
                            Logger.Error(callError + $"Context '{context}' is invalid.");
                            return null;
                        }
                }
            }
            catch (Exception e)
            {
                Logger.Error(callError + e.StackTrace + e.Message);
            }

            return base.Call(args);
        }
    }
}