﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Managment.Models
{
    public class LeaveAllocationVM
    {

        public int Id { get; set; }
        [Required]
        public int NumberOfDays { get; set; }
        public DateTime DateCreated { get; set; }

        public int Period { get; set; }
        public EmployeeVM Employee { get; set; }
        public string EmployeeId { get; set; }

        public LeaveTypeViewModel LeaveType { get; set; }
        public int LeaveTypeId { get; set; }

        //    public IEnumerable<SelectListItem> Employees { get; set; }
        //    public IEnumerable<SelectListItem> LeaveTypes { get; set; }

    }

    public class CreateLeaveallocationVM
    {
        public int NumberUpdated { get; set; }
        public List<LeaveTypeViewModel> LeaveTypes { get; set; }
    }
    public class EditLeaveallocationVM
    {
        public int Id { get; set; }
        public EmployeeVM Employee { get; set; }
        public LeaveTypeViewModel LeaveType { get; set; }
        public int NumberOfDays { get; set; }
        public string EmployeeId { get; set; }
    }

    public class ViewAllocationVM
    {
        public EmployeeVM Employee { get; set; }
        public string EmployeeId { get; set; }


        public List<LeaveAllocationVM> LeaveAllocations { get; set; }
    }
}
