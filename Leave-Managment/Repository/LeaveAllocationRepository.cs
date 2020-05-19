using Leave_Managment.Contracts;
using Leave_Managment.Data;
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

        public ICollection<LeaveAllocation> FindAll()
        {
           return _db.LeaveAllocations.ToList();
            
        }

        public LeaveAllocation FindById(int id)
        {
          return  _db.LeaveAllocations.Find(id);
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
    }
}
