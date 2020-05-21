using Leave_Managment.Contracts;
using Leave_Managment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Managment.Repository
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Create(LeaveType entity)
        {
            _db.leaveTypes.Add(entity);
            return save();

        }

        public bool Delete(LeaveType entity)
        {
            _db.leaveTypes.Remove(entity);
            return save();
        }

        public ICollection<LeaveType> FindAll()
        {
            return _db.leaveTypes.ToList();
        }

        public LeaveType FindById(int id)
        {
            var leavtype = _db.leaveTypes.Find(id);
            return leavtype;
        }

        public ICollection<LeaveType> GetEmployeesByLeaveType(int leavetypeid)
        {
            throw new NotImplementedException();
        }
        public bool IsExsits(int id)
        {
            var exsits = _db.leaveTypes.Any(x=>x.Id == id);
            return exsits;
        }
        public bool save()
        {
            var res = _db.SaveChanges();
            return res > 0;
        }

        public bool Update(LeaveType entity)
        {
            _db.leaveTypes.Update(entity);
            return save();
        }
    }
}
