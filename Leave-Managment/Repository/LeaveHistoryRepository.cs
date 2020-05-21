using Leave_Managment.Contracts;
using Leave_Managment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Managment.Repository
{
    public class LeaveHistoryRepository : ILeaveHistoryRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveHistoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Create(LeaveHistory entity)
        {
            _db.LeaveHistories.Add(entity);
            return save();
        }

        public bool Delete(LeaveHistory entity)
        {
            _db.LeaveHistories.Remove(entity);
            return save();
        }
        public bool IsExsits(int id)
        {
            var exsits = _db.LeaveHistories.Any(x => x.Id == id);
            return exsits;
        }
        public ICollection<LeaveHistory> FindAll()
        {
           return _db.LeaveHistories.ToList();             
        }

        public LeaveHistory FindById(int id)
        {
           return _db.LeaveHistories.Find(id);
           
        }

        public bool save()
        {
            var res = _db.SaveChanges();
            return res > 0;
        }

        public bool Update(LeaveHistory entity)
        {
            _db.LeaveHistories.Update(entity);
            return save();
        }
    }
}
