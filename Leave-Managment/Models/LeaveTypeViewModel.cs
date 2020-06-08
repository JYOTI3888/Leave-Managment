using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Managment.Models
{
    public class LeaveTypeViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Default no of Days")]
        [Range(1, 25, ErrorMessage = "Please enter valid number")]
        public int DefaultDays { get; set; }
        [Display(Name = "Date Created")]       
        public DateTime? DateCreated { get; set; }
    }


}
