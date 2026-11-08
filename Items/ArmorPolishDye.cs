using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace VisualAcc.Items;

public class ArmorPolishDye : ModItem
{
    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 3;

        if (Main.dedServ || Main.netMode == NetmodeID.Server) return;
        GameShaders.Armor.BindShader(Type, new ArmorShaderData(
            ModContent.GetInstance<VisualAcc>().Assets.Request<Effect>
            ("Effects/ArmorPolishShader"), "ArmorPolishShaderPass")).UseColor(0.65f, 0.65f, 0.65f);
    }
    
    public override void SetDefaults() {
        // This code here remembers Item.dye so that information isn't lost during CloneDefaults.
        int dye = Item.dye;

        Item.CloneDefaults(ItemID.GelDye); // Makes the item copy the attributes of the item "Gel Dye"

        Item.dye = dye;
    }

    public override void AddRecipes()
    {
        CreateRecipe(6)
            .AddIngredient(ItemID.ArmorPolish)
            .AddTile(TileID.DyeVat)
            .Register();

        Recipe.Create(ItemID.ArmorPolish)
            .AddIngredient(this, 6)
            .AddTile(TileID.DyeVat)
            .Register();
    }
}