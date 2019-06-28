using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnTheBeachTest.Models
{
    public class Jobs
    {
        [Display(Name ="Input the jobs")]
        public string InputValue { get; set; }
        [Display(Name = "Output")]
        public string OutputValue { get; set; }
        public string TempValue { get; set; }
    }
}