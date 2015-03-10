namespace npsme.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SurveyNPSField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Surveys", "ResponseText", c => c.String());
            AddColumn("dbo.Surveys", "CalcuatedNPS", c => c.Double(nullable: false));
            DropColumn("dbo.Surveys", "Response");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Surveys", "Response", c => c.String());
            DropColumn("dbo.Surveys", "CalcuatedNPS");
            DropColumn("dbo.Surveys", "ResponseText");
        }
    }
}
