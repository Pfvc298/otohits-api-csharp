using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otohits.API.Entities
{
    public class LinksThrottlingRequestModel : LinksBaseRequestModel
    {
        public int visits_per_hour { get; set; }
        public ThrottlingPlan visits_hours_mapping { get; set; } = new ThrottlingPlan();
        public string timezone { get; set; }
    }
}
