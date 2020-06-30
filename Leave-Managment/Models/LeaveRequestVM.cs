using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Managment.Models
{
    public class LeaveRequestVM
    {

        public int Id { get; set; }

        public EmployeeVM RequestingEmployee { get; set; }
        [Display(Name = "Employee Name")]
        public string RequestingEmployeeId { get; set; }
        [Display(Name = "State Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public LeaveTypeViewModel LeaveType { get; set; }
        public int LeaveTypeId { get; set; }

        [Display(Name = "Date Requested")]
        public DateTime DateRequested { get; set; }
        

        [Display(Name = "Date Actioned")]
        public DateTime? DateActioned { get; set; }

        [Display(Name = "Approved State")]
        public bool? Approved { get; set; }

        public bool Cancelled { get; set; }

        public EmployeeVM ApprovedBy { get; set; }
        [Display(Name = "Approver Name")]
        public string ApprovedById { get; set; }

        [Display(Name = "Employee Comments")]
        [MaxLength(300)]
        public string RequestComments { get; set; }

    }

    public class AdminLeaveRequestVM
    {
        [Display(Name = "Total Number of Requests")]
        public int TotalRequets { get; set; }
        [Display(Name = "Approved Requests")]
        public int ApproveRequets { get; set; }
        [Display(Name = "Pending Requests")]
        public int PendingRequets { get; set; }
        [Display(Name = "Rejected Requests")]
        public int RejectedRequets { get; set; }

        public List<LeaveRequestVM> LeaveRequests { get; set; }

    }

    public class CreateLeaveRequestVM
    {
        [Display(Name = "State Date")]
        [Required]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [Required]
        public DateTime EndDate { get; set; }
        public string RequestComments { get; set; }
        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }

    }

    public class EmployeeRequestViewVM
    {
        public List<LeaveAllocationVM> LeaveAllocationVMs { get; set; }

        public List<LeaveRequestVM> LeaveRequests { get; set; }
    }

}
