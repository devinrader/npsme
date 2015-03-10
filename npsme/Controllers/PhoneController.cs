﻿using npsme.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EntityFramework.Triggers;
using Twilio.TwiML.Mvc;
using Twilio.TwiML;

namespace npsme.Controllers
{
    public class PhoneController : TwilioController
    {
        SurveyContext context = new SurveyContext();

        // GET: Phone
        public async Task<ActionResult> Store(string to, string from, string body)
        {

            var response = new TwilioResponse();

            //do we want to fail fast on non-numeric bodies?
            body = body.Trim();

            var survey = context.Surveys.Include(s=>s.Responses).Where(s => s.PhoneNumber == to).FirstOrDefault();
            if (survey != null)
            {
                if (!survey.IsEnabled) {
                    return TwiML(response);
                }

                //parse the NPS value and comment from the body
                int value; string comment;

                if (!int.TryParse(body, out value))
                {
                    if (body.Contains(' '))
                    {
                        if (!int.TryParse(body.Substring(0, body.IndexOf(' ')), out value))
                        {
                            response.Message("Hmm.  We could not find a score in your message.");
                            //no number could be found at the start of the message
                            return TwiML(response);
                        }
                    }
                } 
                
                var surveyresponse = new SurveyResponse()
                    {
                        From = from,
                        To = to,
                        Body = body,
                        Score = value
                    };

                survey.Responses.Add(surveyresponse);
                await context.SaveChangesAsync();

                survey.CalcuatedNPS = CalculateNps(survey);
                await context.SaveChangesAsync();
                    
                response.Message(survey.ResponseText);
                return TwiML(response);
            }

            return new HttpStatusCodeResult(500);
        }

        private double CalculateNps(Survey survey)
        {
            var ints = survey.Responses.Select(r => r.Score);

            double promotorcount = ints.Where(i => i >= 9).Count();
            double detractorcount = ints.Where(i => i <= 6).Count();

            double promoters = promotorcount / ints.Count();
            double detractors = detractorcount / ints.Count();

            var nps = promoters - detractors;

            return nps;
        }
    }
}