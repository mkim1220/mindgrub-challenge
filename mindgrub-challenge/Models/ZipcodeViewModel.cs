using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mindgrub_challenge.Models
{
    public class ZipcodeViewModel
    {
        [Required, RegularExpression(@"^[0-9]{5}(?:-[0-9]{4})?$")]
        public string Zipcode { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public double Distance { get; set; }
    }
}
