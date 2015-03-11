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
                        Score = c.Int(nullable: false),
                        Comment = c.String(),
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
                        ResponseText = c.String(),
                        CalculatedNPS = c.Double(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SurveyId);

            CreateStoredProcedure("[dbo].[CalculateNps]",
                p => new
                {
                    SurveyId = p.Int()
                },
                "DECLARE @Responders int; DECLARE @Promotors int; DECLARE @Detractors int;\r\n" +
                "SELECT COUNT([dbo].[SurveyResponses].ResponseId) as Responders, \r\n" +
                    "COUNT(CASE WHEN [dbo].[SurveyResponses].Score >= 9 THEN 1 ELSE null END) AS Promotors, \r\n" +
                    "COUNT(CASE WHEN [dbo].[SurveyResponses].Score >= 9 THEN 1 ELSE null END) AS Detractors \r\n" +
                "FROM [dbo].[SurveyResponses] WHERE [dbo].[SurveyResponses].Survey_SurveyId = @SurveyId;\r\n" +
                "UPDATE [dbo].[Surveys] SET CalculatedNPS = (( @Promotors / @Responders )-( @Detractors / @Responders )) WHERE [dbo].[Surveys].SurveyId = @SurveyId");

            Sql("CREATE TRIGGER [dbo].[onInsertCalculateNps] ON [dbo].[SurveyResponses] AFTER INSERT, UPDATE AS EXEC [dbo].[CalculateNps]");
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
