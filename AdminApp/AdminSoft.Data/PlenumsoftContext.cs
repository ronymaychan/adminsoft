using AdminSoft.Data.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using AdminSoft.Domain.Employees;

namespace AdminSoft.Data
{
    public class AdminSoftContext : DbContext, IDataContext
    {
        
        public AdminSoftContext() : base("name=AdminSoftContext")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public virtual DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var employee = modelBuilder.Entity<Employee>();

            employee.ToTable("Employee");
            employee.HasKey(x => x.EmployeeId);
            employee.Property(x => x.EmployeeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            employee.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            employee.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            employee.Property(x => x.Address).IsOptional().HasMaxLength(100);
            employee.Property(x => x.City).IsOptional().HasMaxLength(100);
            employee.Property(x => x.Phone).IsOptional().HasMaxLength(100);
            employee.Property(x => x.State).IsOptional().HasMaxLength(100);
            employee.Property(x => x.RowVersion).IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }

        #region Métodos para la implementación de la transaccciones
        private Guid? sing;
        private ObjectContext _objectContext;
        private DbTransaction _transaction;
        private IsolationLevel _isolationLevel = IsolationLevel.Unspecified;
        public void BeginTransaction(Guid guid)
        {
            if (guid == null)
                throw new ArgumentNullException("guid transaction");

            if (sing != null)
                return;

            _objectContext = ((IObjectContextAdapter)this).ObjectContext;
            if (_objectContext.Connection.State != ConnectionState.Open)
            {
                _objectContext.Connection.Open();
            }

            _transaction = _objectContext.Connection.BeginTransaction(_isolationLevel);

            sing = guid;
        }
        public bool Commit(Guid guid)
        {
            if (sing == null || sing != guid)
                return true;

            sing = null;

            if (_transaction != null)
                _transaction.Commit();

            return true;
        }
        public void Rollback(Guid guid)
        {
            if (sing == null || sing != guid)
                return;

            sing = null;

            if (_transaction != null)
                _transaction.Rollback();
        }
        #endregion
    }
}
