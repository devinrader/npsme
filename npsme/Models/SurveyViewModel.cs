using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace npsme.Models
{
    public class SurveyViewModel
    {
        public SurveyViewModel()
        {
            this.Responses = new List<SurveyResponse>();
        }

        public int SurveyId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string ResponseText { get; set; }

        public IList<SurveyResponse> Responses { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}
