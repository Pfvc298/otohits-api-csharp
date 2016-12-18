using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otohits.API.Entities
{
    public class LinksUserAgentsRequestModel : LinksBaseRequestModel
    {
        public List<UserAgent> useragents { get; set; } = new List<UserAgent>();
        public bool override_useragents { get; set; }
    }
}
