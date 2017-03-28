using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZenithWebSite.Models.JenithEventModel
{
    public class Event
    {
        public int EventId { get; set; }

        [Required]
        [Display(Name = "Event Date (YYYY-MM-DD)")]
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }

        [Required]
        [Display(Name = "EventTime From (YYYY-MM-DD HH:MM)")]
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EventDateTimeFrom { get; set; }

        [Required]
        [Display(Name = "EventTime To (YYYY-MM-DD HH:MM)")]
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EventDateTimeTo { get; set; }

        public Boolean IsActive { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Activity")]
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EventDateTimeFrom > EventDateTimeTo)
            {
                yield return
                  new ValidationResult(errorMessage: "End Date & Time must be greater than Start Date & Time",
                                       memberNames: new[] { "EventDateTimeFrom" });
            }

            if (EventDateTimeFrom.Date != EventDateTimeTo.Date)
            {
                yield return
                  new ValidationResult(errorMessage: "Event Start Date and End Date must occur on the same day",
                                       memberNames: new[] { "EventDateTimeTo" });
            }
        }
    }
    //Checks if EndDate is later than StartDate and if Event happens in same day
}
