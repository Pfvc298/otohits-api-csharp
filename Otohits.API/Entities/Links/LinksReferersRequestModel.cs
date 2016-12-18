using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otohits.API.Entities
{
    public class LinksReferersRequestModel : LinksBaseRequestModel
    {
        public List<Referer> referers { get; set; } = new List<Referer>();
        public bool hide_referer { get; set; }
        public bool override_referer { get; set; }
    }
}
