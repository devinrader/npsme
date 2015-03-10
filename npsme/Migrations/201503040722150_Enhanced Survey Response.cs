namespace npsme.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnhancedSurveyResponse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SurveyResponses", "Score", c => c.Int(nullable: false));
            AddColumn("dbo.SurveyResponses", "Comment", c => c.String());
            AddColumn("dbo.Surveys", "IsEnabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Surveys", "IsEnabled");
            DropColumn("dbo.SurveyResponses", "Comment");
            DropColumn("dbo.SurveyResponses", "Score");
        }
    }
}
