using EntityFramework.Triggers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace npsme.Models
{
    public class SurveyResponse : ITriggerable
    {
        public SurveyResponse()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;

        }

        [Key]
        public int ResponseId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
        public string MessageSid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Survey Survey { get; set; }
    }
}
