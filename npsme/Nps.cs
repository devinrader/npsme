using npsme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace npsme
{
    public class Nps
    {
        public double Calculate(Survey survey)
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
