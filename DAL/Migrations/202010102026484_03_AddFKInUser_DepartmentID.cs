namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03_AddFKInUser_DepartmentID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "Department_DepartmentID", "dbo.Department");
            DropIndex("dbo.User", new[] { "Department_DepartmentID" });
            RenameColumn(table: "dbo.User", name: "Department_DepartmentID", newName: "DepartmentID");
            AlterColumn("dbo.User", "DepartmentID", c => c.Int(nullable: false));
            CreateIndex("dbo.User", "DepartmentID");
            AddForeignKey("dbo.User", "DepartmentID", "dbo.Department", "DepartmentID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "DepartmentID", "dbo.Department");
            DropIndex("dbo.User", new[] { "DepartmentID" });
            AlterColumn("dbo.User", "DepartmentID", c => c.Int());
            RenameColumn(table: "dbo.User", name: "DepartmentID", newName: "Department_DepartmentID");
            CreateIndex("dbo.User", "Department_DepartmentID");
            AddForeignKey("dbo.User", "Department_DepartmentID", "dbo.Department", "DepartmentID");
        }
    }
}
