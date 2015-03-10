using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Triggers;

namespace npsme.Models
{
    public class SurveyContext : DbContext
    {
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyResponse> Responses { get; set; }

        public override int SaveChanges()
        {
            return this.SaveChangesWithTriggers(base.SaveChanges);
        }

        //public override Task<int> SaveChangesAsync()
        //{
        //    return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync);
        //}

        public override Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            return this.SaveChangesWithTriggersAsync(base.SaveChangesAsync, cancellationToken);
        }
    }
}
