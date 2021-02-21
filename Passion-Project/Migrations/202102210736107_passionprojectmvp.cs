namespace Passion_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class passionprojectmvp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dishes",
                c => new
                    {
                        DishID = c.Int(nullable: false, identity: true),
                        DishName = c.String(),
                        DishPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.DishID);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        RecipeID = c.Int(nullable: false, identity: true),
                        RecipeType = c.String(),
                        NoOfServings = c.Int(nullable: false),
                        DishID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RecipeID)
                .ForeignKey("dbo.Dishes", t => t.DishID, cascadeDelete: true)
                .Index(t => t.DishID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recipes", "DishID", "dbo.Dishes");
            DropIndex("dbo.Recipes", new[] { "DishID" });
            DropTable("dbo.Recipes");
            DropTable("dbo.Dishes");
        }
    }
}
