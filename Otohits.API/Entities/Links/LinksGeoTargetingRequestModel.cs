using System.Collections.Generic;

namespace Otohits.API.Entities
{
    public class LinksGeoTargetingRequestModel : LinksBaseRequestModel
    {
        public List<string> countries { get; set; } = new List<string>();
    }
}
