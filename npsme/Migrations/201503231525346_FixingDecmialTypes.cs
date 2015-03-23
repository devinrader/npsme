namespace npsme.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingDecmialTypes : DbMigration
    {
        public override void Up()
        {
            AlterStoredProcedure("[dbo].[CalculateNps]",
               p => new
               {
                   SurveyId = p.Int()
               },
               "DECLARE @Responders decimal(10,2); DECLARE @Promotors decimal(10,2); DECLARE @Detractors decimal(10,2); DECLARE @Score decimal(10,2)\r\n" +
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
               "DECLARE @Responders int; DECLARE @Promotors int; DECLARE @Detractors int; DECLARE @Score float\r\n" +
               "SELECT @Responders = COUNT([dbo].[SurveyResponses].ResponseId), \r\n" +
                   "@Promotors = COUNT(CASE WHEN [dbo].[SurveyResponses].Score >= 9 THEN 1 ELSE null END), \r\n" +
                   "@Detractors = COUNT(CASE WHEN [dbo].[SurveyResponses].Score <= 6 THEN 1 ELSE null END) \r\n" +
               "FROM [dbo].[SurveyResponses] WHERE [dbo].[SurveyResponses].Survey_SurveyId = @SurveyId;\r\n" +
               "UPDATE [dbo].[Surveys] SET CalculatedNPS = (( @Promotors / @Responders )-( @Detractors / @Responders )) WHERE [dbo].[Surveys].SurveyId = @SurveyId");
        }
    }
}
