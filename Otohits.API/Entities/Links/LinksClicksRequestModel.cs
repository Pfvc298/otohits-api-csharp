using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otohits.API.Entities
{
    public class LinksClicksRequestModel : LinksBaseRequestModel
    {
        public bool? enable_clicks { get; set; }
        public int? clicks_chance { get; set; }
        public int? max_clicks { get; set; }
        public int? min_clicks { get; set; }
        public int? waiting_time { get; set; }
    }
}
