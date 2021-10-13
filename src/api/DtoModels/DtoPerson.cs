using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aspen.Api.DtoeModels
{
    public class DtoPerson
    {
        public string ID { get; set; }

        public string AuthID { get; set; }

        public string Name { get; set; }

        public string Bio { get; set; }
    }
}
