using Leave_Managment.Contracts;
using Leave_Managment.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Managment.Repository
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveRequestRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Create(LeaveRequest entity)
        {
            _db.LeaveRequests.Add(entity);
            return save();
        }

        public bool Delete(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return save();
        }
        public bool IsExsits(int id)
        {
            var exsits = _db.LeaveRequests.Any(x => x.Id == id);
            return exsits;
        }
        public ICollection<LeaveRequest> FindAll()
        {
            var leavehistory = _db.LeaveRequests
                .Include(q => q.RequestingEmployee)
                .Include(q => q.ApprovedBy)
                .Include(q => q.LeaveType)               
                .ToList();
            return leavehistory;
        }

        public LeaveRequest FindById(int id)
        {
            var leavehistory = _db.LeaveRequests
                 .Include(q => q.RequestingEmployee)
                 .Include(q => q.ApprovedBy)
                 .Include(q => q.LeaveType)
                 .FirstOrDefault(q => q.Id == id);
            return leavehistory;

        }

        public ICollection<LeaveRequest> GetLeaveRequestByEmployeeId(string employeeid)
        {
            var leaverequest = FindAll()                 
                 .Where(q => q.RequestingEmployeeId == employeeid).ToList();
            return leaverequest;

        }

        public bool save()
        {
            var res = _db.SaveChanges();
            return res > 0;
        }

        public bool Update(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return save();
        }
    }
}
