using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Leave_Managment.Contracts;
using Leave_Managment.Data;
using Leave_Managment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Leave_Managment.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveAllocationController : Controller
    {
        // GET: LeaveAllocation
        private readonly ILeaveTypeRepository _leaverepo;
        private readonly ILeaveAllocationRepository _repo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public LeaveAllocationController(ILeaveTypeRepository leaverepo, ILeaveAllocationRepository repo, IMapper mapper, UserManager<Employee> userManager)
        {
            _leaverepo = leaverepo;
            _repo = repo;
            _mapper = mapper;
            _userManager = userManager;
        }
        public ActionResult Index()
        {
            var leavetypes = _leaverepo.FindAll().ToList();
            var mappedleavetype = _mapper.Map<List<LeaveType>, List<LeaveTypeViewModel>>(leavetypes);
            var model = new CreateLeaveallocationVM
            {
                LeaveTypes = mappedleavetype,
                NumberUpdated = 0
            };
            return View(model);
        }

        public ActionResult SetLeave(int id)
        {
            var leavetype = _leaverepo.FindById(id);
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            foreach (var emp in employees)
            {
                if (_repo.CheckAllocation(id, emp.Id))
                    break;
                var allocation = new LeaveAllocationVM
                {
                    DateCreated = DateTime.Now,
                    EmployeeId = emp.Id,
                    LeaveTypeId = id,
                    NumberOfDays = leavetype.DefaultDays,
                    Period = DateTime.Now.Year
                };
                var leaveallocation = _mapper.Map<LeaveAllocation>(allocation);
                _repo.Create(leaveallocation);

            }
            return RedirectToAction(nameof(Index));



        }

        public ActionResult ListEmployee()
        {
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            var model = _mapper.Map<List<EmployeeVM>>(employees);
            return View(model);

        }
        // GET: LeaveAllocation/Details/5
        public ActionResult Details(string id)
        {
            var employee = _mapper.Map<EmployeeVM>(_userManager.FindByIdAsync(id).Result);
            var period = DateTime.Now.Year;
            var allocations = _mapper.Map<List<LeaveAllocationVM>>(_repo.GetLeaveAllocations(id));
            var model = new ViewAllocationVM
            {
                Employee = employee,
                LeaveAllocations = allocations
            };
            return View(model);
        }

        // GET: LeaveAllocation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveAllocation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocation/Edit/5
        public ActionResult Edit(int id)
        {
            var leaveallocation = _repo.FindById(id);
            var model = _mapper.Map<EditLeaveallocationVM>(leaveallocation);
            return View(model);
        }

        // POST: LeaveAllocation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditLeaveallocationVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var record = _repo.FindById(model.Id);
                record.NumberOfDays = model.NumberOfDays;

                var isSucess = _repo.Update(record);
                if (!isSucess)
                {
                    ModelState.AddModelError("", "Error WhileSaving");
                    return View(model);
                }

                return RedirectToAction(nameof(Details), new { id = model.EmployeeId });
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveAllocation/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}