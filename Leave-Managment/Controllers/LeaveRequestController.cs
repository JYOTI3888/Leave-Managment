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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Leave_Managment.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        // GET: LeaveRequest
        private readonly ILeaveRequestRepository _leaveRequestrepo;
        private readonly ILeaveTypeRepository _leavTyperepo;
        private readonly ILeaveAllocationRepository _leavAllocationrepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;


        public LeaveRequestController(ILeaveRequestRepository leaverequestrepo, ILeaveAllocationRepository leaveallocationrepo, ILeaveTypeRepository leavetyperepo, IMapper mapper, UserManager<Employee> userManager)
        {
            _leaveRequestrepo = leaverequestrepo;
            _leavTyperepo = leavetyperepo;
            _leavAllocationrepo = leaveallocationrepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var leaverequets = _leaveRequestrepo.FindAll();
            var leaverequestmodel = _mapper.Map<List<LeaveRequestVM>>(leaverequets);
            var model = new AdminLeaveRequestVM
            {
                TotalRequets = leaverequestmodel.Count,
                ApproveRequets = leaverequestmodel.Count(q => q.Approved == true),
                PendingRequets = leaverequestmodel.Count(q => q.Approved == null),
                RejectedRequets = leaverequestmodel.Count(q => q.Approved == false),
                LeaveRequests = leaverequestmodel

            };
            return View(model);
        }

        public ActionResult Myleave()
        {
            var employee = _userManager.GetUserAsync(User).Result;
            var employeeid = employee.Id;
            var employeeallocations = _leavAllocationrepo.GetLeaveAllocationsByEmployeeId(employeeid);
            var employeeleaverequest = _leaveRequestrepo.GetLeaveRequestByEmployeeId(employeeid);

            var emoployeeallocationsModel = _mapper.Map<List<LeaveAllocationVM>>(employeeallocations);

            var emoployeerequestModel = _mapper.Map<List<LeaveRequestVM>>(employeeleaverequest);

            var model = new EmployeeRequestViewVM
            {
                LeaveAllocationVMs = emoployeeallocationsModel,
                LeaveRequests = emoployeerequestModel

            };

            return View(model);

        }


        // GET: LeaveRequest/Details/5
        public ActionResult Details(int id)
        {
            var leaveRequest = _leaveRequestrepo.FindById(id);
            var model = _mapper.Map<LeaveRequestVM>(leaveRequest);
            return View(model);
        }

        public ActionResult ApproveRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaveRequest = _leaveRequestrepo.FindById(id);
                var employeeid = leaveRequest.RequestingEmployeeId;
                var leavetypeid = leaveRequest.LeaveTypeId;
                var allocation = _leavAllocationrepo.GetLeaveAllocationsByEmployeeandType(employeeid, leavetypeid);
                leaveRequest.Approved = true;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateRequested = DateTime.Now;
                int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                allocation.NumberOfDays = allocation.NumberOfDays - daysRequested;

                leaveRequest.Approved = true;
                leaveRequest.ApprovedById = user.Id;
                // leaveRequest.DateActioned = DateTime.Now;

                _leaveRequestrepo.Update(leaveRequest);
                _leavAllocationrepo.Update(allocation);

                var isSucess = _leaveRequestrepo.Update(leaveRequest);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));

            }


        }

        public ActionResult RejectRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaveRequest = _leaveRequestrepo.FindById(id);
                leaveRequest.Approved = false;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateRequested = DateTime.Now;


                _leaveRequestrepo.Update(leaveRequest);
                return RedirectToAction(nameof(Index), "Home");

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index), "Home");

            }
        }
        // GET: LeaveRequest/Create
        public ActionResult Create()
        {
            var leavetypes = _leavTyperepo.FindAll();
            var leavetypeviewietms = leavetypes.Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()

            });
            var model = new CreateLeaveRequestVM
            {
                LeaveTypes = leavetypeviewietms
            };
            return View(model);
        }

        // POST: LeaveRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateLeaveRequestVM model)
        {
            var startDate = Convert.ToDateTime(model.StartDate);
            var EndDate = Convert.ToDateTime(model.EndDate);
            var leavetypes = _leavTyperepo.FindAll();
            var leavetypeviewietms = leavetypes.Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()

            });
            model.LeaveTypes = leavetypeviewietms;
            try
            {
                if (!ModelState.IsValid)

                {
                    return View(model);
                }

                if (DateTime.Compare(startDate, EndDate) > 1)
                {
                    ModelState.AddModelError("", "Start date can not be further in the furture than End Date");
                    return View(model);
                }
                var employee = _userManager.GetUserAsync(User).Result;
                var allocations = _leavAllocationrepo.GetLeaveAllocationsByEmployeeandType(employee.Id, model.LeaveTypeId);
                int daysRequested = (int)(EndDate - startDate).TotalDays;
                if (daysRequested > allocations.NumberOfDays)
                {
                    ModelState.AddModelError("", "You do not have sufficinet days for thsi Request");
                    return View(model);
                }
                var leaveRequetsmodel = new LeaveRequestVM
                {
                    RequestingEmployeeId = employee.Id,
                    StartDate = startDate,
                    EndDate = EndDate,
                    Approved = null,
                    DateRequested = DateTime.Now,
                    DateActioned = DateTime.Now,
                    LeaveTypeId = model.LeaveTypeId,
                    RequestComments=model.RequestComments

                };

                var leaverequest = _mapper.Map<LeaveRequest>(leaveRequetsmodel);
                var isuccess = _leaveRequestrepo.Create(leaverequest);

                if (!isuccess)
                {
                    ModelState.AddModelError("", "Somthing went wrong with somthing with submitting your record");
                    return View(model);
                }
                return RedirectToAction("MyLeave");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Somthing went wrong");
                return View(model);
            }
        }

        public ActionResult CancelRequest(int id)
        {
            var leaverequest = _leaveRequestrepo.FindById(id);
            //leaverequest.Cancelled = true;
            _leaveRequestrepo.Update(leaverequest);
            return RedirectToAction("MyLeave");
        }

        // GET: LeaveRequest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveRequest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveRequest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveRequest/Delete/5
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