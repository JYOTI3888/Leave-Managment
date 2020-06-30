using Leave_Managment.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Managment.Contracts
{
    public interface ILeaveAllocationRepository : IRepositoryBase<LeaveAllocation>
    {
        bool CheckAllocation(int leavetypeid, string empid);
        ICollection<LeaveAllocation> GetLeaveAllocations(string id);

        ICollection<LeaveAllocation> GetLeaveAllocationsByEmployeeId(string employeeid);
        LeaveAllocation GetLeaveAllocationsByEmployeeandType(string employeeid, int leavetypeid);
    }
}
