namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _02_NewTable_Department : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
            AddColumn("dbo.User", "Department_DepartmentID", c => c.Int());
            CreateIndex("dbo.User", "Department_DepartmentID");
            AddForeignKey("dbo.User", "Department_DepartmentID", "dbo.Department", "DepartmentID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "Department_DepartmentID", "dbo.Department");
            DropIndex("dbo.User", new[] { "Department_DepartmentID" });
            DropColumn("dbo.User", "Department_DepartmentID");
            DropTable("dbo.Department");
        }
    }
}
