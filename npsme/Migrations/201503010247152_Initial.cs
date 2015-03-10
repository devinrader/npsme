namespace npsme.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SurveyResponses",
                c => new
                    {
                        ResponseId = c.Int(nullable: false, identity: true),
                        From = c.String(),
                        To = c.String(),
                        Body = c.String(),
                        MessageSid = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Survey_SurveyId = c.Int(),
                    })
                .PrimaryKey(t => t.ResponseId)
                .ForeignKey("dbo.Surveys", t => t.Survey_SurveyId)
                .Index(t => t.Survey_SurveyId);
            
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        SurveyId = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(),
                        Response = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SurveyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SurveyResponses", "Survey_SurveyId", "dbo.Surveys");
            DropIndex("dbo.SurveyResponses", new[] { "Survey_SurveyId" });
            DropTable("dbo.Surveys");
            DropTable("dbo.SurveyResponses");
        }
    }
}
