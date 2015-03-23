namespace npsme.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixNpsCal : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure("[dbo].[CalculateNps]",
                p => new
                {
                    SurveyId = p.Int()
                },
                "DECLARE @Responders int; DECLARE @Promotors int; DECLARE @Detractors int; DECLARE @Score float\r\n" +
                "SELECT @Responders = COUNT([dbo].[SurveyResponses].ResponseId), \r\n" +
                    "@Promotors = COUNT(CASE WHEN [dbo].[SurveyResponses].Score >= 9 THEN 1 ELSE null END), \r\n" +
                    "@Detractors = COUNT(CASE WHEN [dbo].[SurveyResponses].Score <= 6 THEN 1 ELSE null END) \r\n" +
                "FROM [dbo].[SurveyResponses] WHERE [dbo].[SurveyResponses].Survey_SurveyId = @SurveyId;\r\n" +
                "UPDATE [dbo].[Surveys] SET CalculatedNPS = (( @Promotors / @Responders )-( @Detractors / @Responders )) WHERE [dbo].[Surveys].SurveyId = @SurveyId");

        }

        public override void Down()
        {
            AlterStoredProcedure("[dbo].[CalculateNps]",
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
        }
    }
}
