using Leave_Managment.Contracts;
using Leave_Managment.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Managment.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Create(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Add(entity);
            return save();
        }

        public bool Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return save();
        }
        public bool IsExsits(int id)
        {
            var exsits = _db.LeaveAllocations.Any(x => x.Id == id);
            return exsits;
        }
        public ICollection<LeaveAllocation> FindAll()
        {
            var leaveallocations = _db.LeaveAllocations.Include(q => q.LeaveType).ToList();
            return leaveallocations;

        }

        public LeaveAllocation FindById(int id)
        {
            var leaveallocations = _db.LeaveAllocations.Include(q => q.LeaveType).Include(q => q.Employee).FirstOrDefault(q => q.Id == id);
            return leaveallocations;
        }

        public bool save()
        {
            var res = _db.SaveChanges();
            return res > 0;
        }

        public bool Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return save();
        }

        public bool CheckAllocation(int leavetypeid, string empid)
        {
            var period = DateTime.Now.Year;
            return FindAll().Where(q => q.EmployeeId == empid && q.LeaveTypeId == leavetypeid && q.Period == period).Any();
        }

        public ICollection<LeaveAllocation> GetLeaveAllocations(string id)
        {
            var period = DateTime.Now.Year;
            return FindAll()
                 .Where(q => q.EmployeeId == id && q.Period == period)
                 .ToList();
        }

        public ICollection<LeaveAllocation> GetLeaveAllocationsByEmployeeId(string id)
        {
            var period = DateTime.Now.Year;
            return FindAll()
                 .Where(q => q.EmployeeId == id && q.Period == period)
                 .ToList();
        }

        public LeaveAllocation GetLeaveAllocationsByEmployeeandType(string id, int leavetypeid)
        {
            var period = DateTime.Now.Year;
            return FindAll()
                 .FirstOrDefault(q => q.EmployeeId == id && q.Period == period && q.LeaveTypeId == leavetypeid);
        }
    }
}
