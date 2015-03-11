using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace npsme.Models
{
    public class Survey
    {
        public Survey()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            this.Responses = new List<SurveyResponse>();
        }

        [Key]
        public int SurveyId { get; set; }
        public string PhoneNumber { get; set; }
        public string ResponseText { get; set; }
        public List<SurveyResponse> Responses { get; set; }
        public double CalculatedNPS { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
