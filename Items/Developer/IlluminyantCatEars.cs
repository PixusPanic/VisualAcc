using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace VisualAcc.Items.Developer
{
    [LegacyName("CatEars")]
    [AutoloadEquip(EquipType.Head)]
    public class IlluminyantCatEars : ModItem {
        public override void SetStaticDefaults() =>
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;

        public override void SetDefaults() {
            Item.width = 30;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 1, 6, 69);
            Item.rare = ItemRarityID.Orange;
            Item.vanity = true;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player) {
            player.GetModPlayer<VisualAccPlayer>().catEars = true;
        }

        public override void UpdateVanity(Player player) {
            player.GetModPlayer<VisualAccPlayer>().catEars = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            foreach (TooltipLine tooltipLine in tooltips)
                if (tooltipLine.Mod == "Terraria" && tooltipLine.Name == "ItemName")
                    tooltipLine.OverrideColor = new Color(235, 4, 80);
        }
        
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<VisualAccConfig>().DeveloperSets;
        }
    }

    public class CatEarsPlayerLayer : PlayerDrawLayer
    {
        public static int frame;
        public static int frameCounter;
        public static bool catEars = false;
        public override bool IsHeadLayer => true;
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            bool ears = false;
            for (int n = 13; n < 18 + drawInfo.drawPlayer.extraAccessorySlots; n++)
            {
                Item item = drawInfo.drawPlayer.armor[n];
                if (item.type == ModContent.ItemType<IlluminyantCatEars>())
                    ears = true;
            }

            catEars = ears || drawInfo.drawPlayer.GetModPlayer<VisualAccPlayer>().catEars;
            return ears || drawInfo.drawPlayer.GetModPlayer<VisualAccPlayer>().catEars;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;

            Mortis(ref drawInfo, "CatEars_Anim_Front");
        }

        public static void Mortis(ref PlayerDrawSet drawInfo, string texName)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            int secondyoffset;
            var bodFrame = drawPlayer.bodyFrame;
            int dyeShader = drawPlayer.dye?[1].dye ?? 0;
            for (int n = 0; n < 18 + drawInfo.drawPlayer.extraAccessorySlots; n++)
            {
                Item item = drawInfo.drawPlayer.armor[n];
                if (item.type == ModContent.ItemType<IlluminyantCatEars>())
                {
                    if (n > 9)
                        dyeShader = drawPlayer.dye?[n - 10].dye ?? 0;
                    else
                        dyeShader = drawPlayer.dye?[n].dye ?? 0;
                }
            }

            if (bodFrame.Y == bodFrame.Height * 7 || bodFrame.Y == bodFrame.Height * 8 || bodFrame.Y == bodFrame.Height * 9
                || bodFrame.Y == bodFrame.Height * 14 || bodFrame.Y == bodFrame.Height * 15 || bodFrame.Y == bodFrame.Height * 16)
                secondyoffset = 2;
            else
                secondyoffset = 0;

            frameCounter++;
            if (frameCounter > 8)
            {
                frameCounter = 0;
                frame++;
                if (frame > 8)
                    frame = 0;
            }

            Vector2 headPosition = new Vector2(
                (int)(drawInfo.Position.X - drawInfo.drawPlayer.bodyFrame.Width / 2 + drawInfo.drawPlayer.width / 2),
                   (int)(drawInfo.Position.Y + drawInfo.drawPlayer.height) - (float)drawInfo.drawPlayer.bodyFrame.Height - secondyoffset)
               + drawInfo.drawPlayer.headPosition + drawInfo.headVect;

            headPosition -= Main.screenPosition;

            Texture2D texture = ModContent.Request<Texture2D>("VisAcc/Items/" + texName).Value;
            Rectangle yFrame = texture.Frame(1, 9, 0, frame);
            DrawData dat = new DrawData(texture, new Vector2(headPosition.X - 1 * drawPlayer.direction, headPosition.Y - 5), yFrame, drawInfo.colorArmorHead,
                0f, new Vector2(texture.Width / 2f, texture.Height / 2f / 9f), 1f, drawPlayer.direction != -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            dat.shader = dyeShader;
            drawInfo.DrawDataCache.Add(dat);
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<VisualAccConfig>().DeveloperSets;
        }
    }
    public class CatEarsPlayerLayerBack : PlayerDrawLayer
    {
        public override bool IsHeadLayer => true;
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Head);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return CatEarsPlayerLayer.catEars;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;

            CatEarsPlayerLayer.Mortis(ref drawInfo, "CatEars_Anim_Back");
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<VisualAccConfig>().DeveloperSets;
        }
    }
}