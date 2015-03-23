using npsme.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Twilio;

namespace npsme.Controllers
{
    public class HomeController : Controller
    {
        //replace using block with DI eventually
        SurveyContext context = new SurveyContext();

        // GET: Home
        public ActionResult Index()
        {
            //var surveys = context;
            return View(context.Surveys.Include(s=>s.Responses));
        }

        [HttpGet]
        public ActionResult Survey(int? surveyid)
        {
            var survey = context.Surveys.Include(s=>s.Responses).Where(s => s.SurveyId == surveyid).FirstOrDefault();

            var vm = new SurveyViewModel();
            if (survey != null)
            {
                vm.SurveyId = survey.SurveyId;
                vm.PhoneNumber = survey.PhoneNumber;
                vm.ResponseText = survey.ResponseText;
                vm.IsEnabled = survey.IsEnabled;
                vm.Responses = survey.Responses;
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Survey(SurveyViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var survey = context.Surveys.Where(s => s.SurveyId == vm.SurveyId).FirstOrDefault();

                if (survey == null)
                {
                    var client = new TwilioRestClient(ConfigurationManager.AppSettings["AccountSid"], ConfigurationManager.AppSettings["AuthToken"]);
                    
#if DEBUG
                    var result = client.AddIncomingLocalPhoneNumber(new PhoneNumberOptions() { PhoneNumber = vm.PhoneNumber, SmsUrl ="http://example.com/Phone/Store", VoiceUrl = "" });
#else
                    var result = client.AddIncomingLocalPhoneNumber(new PhoneNumberOptions() { PhoneNumber = vm.PhoneNumber, SmsUrl = Url.Action("Store", "Phone", null, "http"), VoiceUrl = "" });
#endif

                    if (result.RestException != null)
                    {
                        throw new Exception(string.Format("Unable to add phone number: \"{0}\"", result.RestException.Message));
                    }

                    survey = new Survey();
                    survey = context.Surveys.Add(survey);
                }

                survey.PhoneNumber = vm.PhoneNumber;
                survey.ResponseText = vm.ResponseText;
                survey.IsEnabled = vm.IsEnabled;
                survey.UpdatedAt = DateTime.Now;

                await context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteSurvey(int surveyid)
        {
            context.Surveys.Remove(context.Surveys.Where(s => s.SurveyId == surveyid).First());
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult PhoneNumber(string areacode)
        {
            var client = new TwilioRestClient(ConfigurationManager.AppSettings["AccountSid"], ConfigurationManager.AppSettings["AuthToken"]);
            var result = client.ListAvailableLocalPhoneNumbers("US", new AvailablePhoneNumberListRequest() { AreaCode = areacode });

            if (result.RestException != null)
            {

            }
            
            return Content(result.AvailablePhoneNumbers.First().PhoneNumber);
        }

        public async Task<ActionResult> SurveyResponse(int responseid, string body, int score)
        {
           var response = context.Responses.Where(r => r.ResponseId == responseid).FirstOrDefault();
            if (response != null)
            {                                
                response.Body = body;
                response.Score = score;
                await context.SaveChangesAsync();

                var surveyIdParam = new SqlParameter("@SurveyId", response.Survey.SurveyId);
                await context.Database.ExecuteSqlCommandAsync("[dbo].[CalculateNps] @SurveyId", surveyIdParam);

                return new System.Web.Mvc.HttpStatusCodeResult(200);
            }

            return new System.Web.Mvc.HttpStatusCodeResult(500);
        }
    }
}