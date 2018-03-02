using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSoft.Data.Interfaces.Base
{
    public interface IDataContext : IDisposable
    {
        void BeginTransaction(Guid guid);
        bool Commit(Guid guid);
        void Rollback(Guid guid);
    }
}
