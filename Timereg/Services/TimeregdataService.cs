using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Timereg.Data;

namespace Timereg
{
    public partial class TimeregdataService
    {
        private readonly TimeregdataContext context;
        private readonly NavigationManager navigationManager;

        public TimeregdataService(TimeregdataContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public async Task ExportEmployeesToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/timeregdata/employees/excel") : "export/timeregdata/employees/excel", true);
        }

        public async Task ExportEmployeesToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/timeregdata/employees/csv") : "export/timeregdata/employees/csv", true);
        }

        partial void OnEmployeesRead(ref IQueryable<Models.Timeregdata.Employee> items);

        public async Task<IQueryable<Models.Timeregdata.Employee>> GetEmployees(Query query = null)
        {
            var items = context.Employees.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnEmployeesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEmployeeCreated(Models.Timeregdata.Employee item);

        public async Task<Models.Timeregdata.Employee> CreateEmployee(Models.Timeregdata.Employee employee)
        {
            OnEmployeeCreated(employee);

            context.Employees.Add(employee);
            context.SaveChanges();

            return employee;
        }
        public async Task<bool> ApproveTimeused(int userid, DateTime fromdate, DateTime todate, int approved)
        {

            // await context.Database.ExecuteSqlCommand("UPDATE Timeused SET Approved = {0} WHERE userid = {1} and workdate between {2} and {3}", approved,userid, Datetostr(fromdate), Datetostr(todate.AddDays(1));

            var p = context.Timeuseds.Where(u => u.userid == userid && u.workdate >= fromdate && u.workdate < todate.AddDays(1));
            if (p != null && p.Any())
            {
                foreach (var t in p)
                    t.approved = approved;
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task ExportProjectsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/timeregdata/projects/excel") : "export/timeregdata/projects/excel", true);
        }

        public async Task ExportProjectsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/timeregdata/projects/csv") : "export/timeregdata/projects/csv", true);
        }

        partial void OnProjectsRead(ref IQueryable<Models.Timeregdata.Project> items);

        public async Task<IQueryable<Models.Timeregdata.Project>> GetProjects(Query query = null)
        {
            var items = context.Projects.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnProjectsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProjectCreated(Models.Timeregdata.Project item);

        public async Task<Models.Timeregdata.Project> CreateProject(Models.Timeregdata.Project project)
        {
            OnProjectCreated(project);

            context.Projects.Add(project);
            context.SaveChanges();

            return project;
        }
        public async Task ExportTimeusedsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/timeregdata/timeuseds/excel") : "export/timeregdata/timeuseds/excel", true);
        }

        public async Task ExportTimeusedsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/timeregdata/timeuseds/csv") : "export/timeregdata/timeuseds/csv", true);
        }

        partial void OnTimeusedsRead(ref IQueryable<Models.Timeregdata.Timeused> items);

        public async Task<IQueryable<Models.Timeregdata.Timeused>> GetTimeuseds(Query query = null)
        {
            var items = context.Timeuseds.AsQueryable();

            items = items.Include(i => i.Project);

            items = items.Include(i => i.Employee);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnTimeusedsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTimeusedCreated(Models.Timeregdata.Timeused item);

        public async Task<Models.Timeregdata.Timeused> CreateTimeused(Models.Timeregdata.Timeused timeused)
        {
            OnTimeusedCreated(timeused);

            context.Timeuseds.Add(timeused);
            context.SaveChanges();

            return timeused;
        }
        public async Task ExportUserProjectsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/timeregdata/userprojects/excel") : "export/timeregdata/userprojects/excel", true);
        }

        public async Task ExportUserProjectsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/timeregdata/userprojects/csv") : "export/timeregdata/userprojects/csv", true);
        }

        partial void OnUserProjectsRead(ref IQueryable<Models.Timeregdata.UserProject> items);

        public async Task<IQueryable<Models.Timeregdata.UserProject>> GetUserProjects(Query query = null)
        {
            var items = context.UserProjects.AsQueryable();

            items = items.Include(i => i.Project);

            items = items.Include(i => i.Employee);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnUserProjectsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnUserProjectCreated(Models.Timeregdata.UserProject item);

        public async Task<Models.Timeregdata.UserProject> CreateUserProject(Models.Timeregdata.UserProject userProject)
        {
            OnUserProjectCreated(userProject);

            context.UserProjects.Add(userProject);
            context.SaveChanges();

            return userProject;
        }
        public async Task ExportVTimeusedsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/timeregdata/vtimeuseds/excel") : "export/timeregdata/vtimeuseds/excel", true);
        }

        public async Task ExportVTimeusedsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/timeregdata/vtimeuseds/csv") : "export/timeregdata/vtimeuseds/csv", true);
        }
        partial void OnVUserWithTimesRead(ref IQueryable<Models.Timeregdata.VUserWithTime> items);

        public async Task<IQueryable<Models.Timeregdata.VUserWithTime>> GetVUserWithTimes(Query query = null)
        {
            var items = context.VUserWithTimes.AsQueryable();
            items = items.AsNoTracking();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnVUserWithTimesRead(ref items);

            return await Task.FromResult(items);
        }
        partial void OnVTimeusedsRead(ref IQueryable<Models.Timeregdata.VTimeused> items);

        public async Task<IQueryable<Models.Timeregdata.VTimeused>> GetVTimeuseds(Query query = null)
        {
            var items = context.VTimeuseds.AsQueryable();
            items = items.AsNoTracking();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnVTimeusedsRead(ref items);

            return await Task.FromResult(items);
        }
        public async Task ExportVUserProjectsToExcel(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/timeregdata/vuserprojects/excel") : "export/timeregdata/vuserprojects/excel", true);
        }

        public async Task ExportVUserProjectsToCSV(Query query = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl("export/timeregdata/vuserprojects/csv") : "export/timeregdata/vuserprojects/csv", true);
        }

        partial void OnTimeusedPeruserRead(ref IQueryable<Models.Timeregdata.VTimeusedPeruser> items);

        public async Task<IQueryable<Models.Timeregdata.VTimeusedPeruser>> GetVTimeusedPeruser(Query query = null)
        {

            var items = context.VTimeusedPerusers.AsQueryable();
            items = items.AsNoTracking();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnTimeusedPeruserRead(ref items);

            return await Task.FromResult(items);


        }
        partial void OnVUserProjectsRead(ref IQueryable<Models.Timeregdata.VUserProject> items);

        public async Task<IQueryable<Models.Timeregdata.VUserProject>> GetVUserProjects(Query query = null)
        {
            var items = context.VUserProjects.AsQueryable();
            items = items.AsNoTracking();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnVUserProjectsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEmployeeDeleted(Models.Timeregdata.Employee item);

        public async Task<Models.Timeregdata.Employee> DeleteEmployee(int? userid)
        {
            var item = context.Employees
                              .Where(i => i.userid == userid)
                              .Include(i => i.Timeuseds)
                              .Include(i => i.UserProjects)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnEmployeeDeleted(item);

            context.Employees.Remove(item);

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                context.Entry(item).State = EntityState.Unchanged;
                throw ex;
            }

            return item;
        }

        partial void OnEmployeeGet(Models.Timeregdata.Employee item);

        public async Task<Models.Timeregdata.Employee> GetEmployeeByuserid(int? userid)
        {
            var items = context.Employees
                              .AsNoTracking()
                              .Where(i => i.userid == userid);

            var item = items.FirstOrDefault();

            OnEmployeeGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.Timeregdata.Employee> CancelEmployeeChanges(Models.Timeregdata.Employee item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnEmployeeUpdated(Models.Timeregdata.Employee item);

        public async Task<Models.Timeregdata.Employee> UpdateEmployee(int? userid, Models.Timeregdata.Employee employee)
        {
            OnEmployeeUpdated(employee);

            var item = context.Employees
                              .Where(i => i.userid == userid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(employee);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return employee;
        }

        partial void OnProjectDeleted(Models.Timeregdata.Project item);

        public async Task<Models.Timeregdata.Project> DeleteProject(int? id)
        {
            var item = context.Projects
                              .Where(i => i.id == id)
                              .Include(i => i.Timeuseds)
                              .Include(i => i.UserProjects)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnProjectDeleted(item);

            context.Projects.Remove(item);

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                context.Entry(item).State = EntityState.Unchanged;
                throw ex;
            }

            return item;
        }

        partial void OnProjectGet(Models.Timeregdata.Project item);

        public async Task<Models.Timeregdata.Project> GetProjectByid(int? id)
        {
            var items = context.Projects
                              .AsNoTracking()
                              .Where(i => i.id == id);

            var item = items.FirstOrDefault();

            OnProjectGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.Timeregdata.Project> CancelProjectChanges(Models.Timeregdata.Project item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnProjectUpdated(Models.Timeregdata.Project item);

        public async Task<Models.Timeregdata.Project> UpdateProject(int? id, Models.Timeregdata.Project project)
        {
            OnProjectUpdated(project);

            var item = context.Projects
                              .Where(i => i.id == id)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(project);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return project;
        }

        partial void OnTimeusedDeleted(Models.Timeregdata.Timeused item);

        public async Task<Models.Timeregdata.Timeused> DeleteTimeused(int? tid)
        {
            var item = context.Timeuseds
                              .Where(i => i.tid == tid)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnTimeusedDeleted(item);

            context.Timeuseds.Remove(item);

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                context.Entry(item).State = EntityState.Unchanged;
                throw ex;
            }

            return item;
        }

        partial void OnTimeusedGet(Models.Timeregdata.Timeused item);

        public async Task<Models.Timeregdata.Timeused> GetTimeusedBytid(int? tid)
        {
            var items = context.Timeuseds
                              .AsNoTracking()
                              .Where(i => i.tid == tid);

            items = items.Include(i => i.Project);

            items = items.Include(i => i.Employee);

            var item = items.FirstOrDefault();

            OnTimeusedGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.Timeregdata.Timeused> CancelTimeusedChanges(Models.Timeregdata.Timeused item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnTimeusedUpdated(Models.Timeregdata.Timeused item);

        public async Task<Models.Timeregdata.Timeused> UpdateTimeused(int? tid, Models.Timeregdata.Timeused timeused)
        {
            OnTimeusedUpdated(timeused);

            var item = context.Timeuseds
                              .Where(i => i.tid == tid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(timeused);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return timeused;
        }

        partial void OnUserProjectDeleted(Models.Timeregdata.UserProject item);

        public async Task<Models.Timeregdata.UserProject> DeleteUserProject(int? upid)
        {
            var item = context.UserProjects
                              .Where(i => i.upid == upid)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnUserProjectDeleted(item);

            context.UserProjects.Remove(item);

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                context.Entry(item).State = EntityState.Unchanged;
                throw ex;
            }

            return item;
        }

        partial void OnUserProjectGet(Models.Timeregdata.UserProject item);

        public async Task<Models.Timeregdata.UserProject> GetUserProjectByUpid(int? upid)
        {
            var items = context.UserProjects
                              .AsNoTracking()
                              .Where(i => i.upid == upid);

            items = items.Include(i => i.Project);

            items = items.Include(i => i.Employee);

            var item = items.FirstOrDefault();

            OnUserProjectGet(item);

            return await Task.FromResult(item);
        }

        public async Task<Models.Timeregdata.UserProject> CancelUserProjectChanges(Models.Timeregdata.UserProject item)
        {
            var entity = context.Entry(item);
            entity.CurrentValues.SetValues(entity.OriginalValues);
            entity.State = EntityState.Unchanged;

            return item;
        }

        partial void OnUserProjectUpdated(Models.Timeregdata.UserProject item);

        public async Task<Models.Timeregdata.UserProject> UpdateUserProject(int? upid, Models.Timeregdata.UserProject userProject)
        {
            OnUserProjectUpdated(userProject);

            var item = context.UserProjects
                              .Where(i => i.upid == upid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(userProject);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return userProject;
        }

        //Invoice Config created 

        partial void OnInvoice(ref IQueryable<Models.Timeregdata.Invoice> items);

        public async Task<IQueryable<Models.Timeregdata.Invoice>> GetInvoice(Query query = null)
        {
            var items = context.Invoice.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnInvoice(ref items);

            return await Task.FromResult(items);
        }


        partial void OnInvoiceCreated(Models.Timeregdata.Invoice item);

        public async Task<Models.Timeregdata.Invoice> CreateTenentInvoiceConfig(Models.Timeregdata.Invoice invoice)
        {
            OnInvoiceCreated(invoice);

            context.Invoice.Add(invoice);
            context.SaveChanges();

            return invoice;
        }


        partial void OnInvoiceDeleted(Models.Timeregdata.Invoice item);

        public async Task<Models.Timeregdata.Invoice> DeleteInvoice(int? invoiceid)
        {
            var item = context.Invoice
                              .Where(i => i.invoiceid == invoiceid)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnInvoiceDeleted(item);

            context.Invoice.Remove(item);

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                context.Entry(item).State = EntityState.Unchanged;
                throw ex;
            }

            return item;
        }

        partial void OnInvoiceUpdated(Models.Timeregdata.Invoice item);

        public async Task<Models.Timeregdata.Invoice> UpdateInvoice(int? invoiceid, Models.Timeregdata.Invoice invoice)
        {
            OnInvoiceUpdated(invoice);

            var item = context.Invoice
                              .Where(i => i.invoiceid == invoiceid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(invoice);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return invoice;
        }


        partial void OnInvoiceGet(Models.Timeregdata.Invoice item);

        public async Task<Models.Timeregdata.Invoice> GetInvoiceByid(int? invoiceid)
        {
            var items = context.Invoice
                              .AsNoTracking()
                              .Where(i => i.invoiceid == invoiceid);

            var item = items.FirstOrDefault();

            OnInvoiceGet(item);

            return await Task.FromResult(item);
        }


        //Company 

        partial void OnCompany(ref IQueryable<Models.Timeregdata.Company> items);

        public async Task<IQueryable<Models.Timeregdata.Company>> GetCompany(Query query = null)
        {
            var items = context.Company.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
            OnCompany(ref items);

            return await Task.FromResult(items);

        }

        partial void OnCompanyGet(Models.Timeregdata.Company item);

        public async Task<Models.Timeregdata.Company> GetCompanyByid(int? companyid)
        {
            var items = context.Company
                              .AsNoTracking()
                              .Where(i => i.companyid == companyid);

            var item = items.FirstOrDefault();

            OnCompanyGet(item);

            return await Task.FromResult(item);
        }


        //invoice line

        partial void OnInvoiceline(ref IQueryable<Models.Timeregdata.Invoiceline> items);

        public async Task<IQueryable<Models.Timeregdata.Invoiceline>> GetInvoiceline(Query query = null)
        {
            var items = context.Invoiceline.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    items = items.Where(query.Filter);
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnInvoiceline(ref items);

            return await Task.FromResult(items);
        }

        partial void OnInvoicelineCreated(Models.Timeregdata.Invoiceline item);

        public async Task<Models.Timeregdata.Invoiceline> CreateTenentInvoicelineConfig(Models.Timeregdata.Invoiceline invoiceline)
        {
            OnInvoicelineCreated(invoiceline);

            context.Invoiceline.Add(invoiceline);
            context.SaveChanges();

            return invoiceline;
        }

        partial void OnInvoicelineDeleted(Models.Timeregdata.Invoiceline item);

        public async Task<Models.Timeregdata.Invoiceline> DeleteInvoiceline(int? invoicelineid, int invoiceid)
        {
            var item = context.Invoiceline
                              .Where(i => i.invoicelineid == invoicelineid && i.invoiceid == invoiceid)
                              .FirstOrDefault();

            if (item == null)
            {
                throw new Exception("Item no longer available");
            }

            OnInvoicelineDeleted(item);

            context.Invoiceline.Remove(item);

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                context.Entry(item).State = EntityState.Unchanged;
                throw ex;
            }

            return item;
        }

        partial void OnInvoicelineGet(Models.Timeregdata.Invoiceline item);

        public async Task<Models.Timeregdata.Invoiceline> GetInvoicelineByid(int? invoicelineid)
        {
            var items = context.Invoiceline
                              .AsNoTracking()
                              .Where(i => i.invoicelineid == invoicelineid);

            var item = items.FirstOrDefault();

            OnInvoicelineGet(item);

            return await Task.FromResult(item);
        }

        partial void OnInvoicelineUpdated(Models.Timeregdata.Invoiceline item);

        public async Task<Models.Timeregdata.Invoiceline> UpdateInvoiceline(int? invoicelineid, int invoiceid, Models.Timeregdata.Invoiceline invoiceline)
        {
            OnInvoicelineUpdated(invoiceline);

            var item = context.Invoiceline
                              .Where(i => i.invoicelineid == invoicelineid && i.invoiceid == invoiceid)
                              .FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Item no longer available");
            }
            var entry = context.Entry(item);
            entry.CurrentValues.SetValues(invoiceline);
            entry.State = EntityState.Modified;
            context.SaveChanges();

            return invoiceline;
        }

        public static Dictionary<int, string> ProjectType()
        {
            Dictionary<int, string> ProjectType = new Dictionary<int, string>();

            ProjectType.Add(0, "Features");
            ProjectType.Add(1, "Bugs");

            return ProjectType;
        }

    }
}
