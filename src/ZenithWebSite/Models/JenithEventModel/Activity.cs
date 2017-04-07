using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZenithWebSite.Models.JenithEventModel
{
    public class Activity
    {
        public int ActivityId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Description")]
        public string ActivityDescr { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        public List<Event> Events { get; set; }
    }
}
