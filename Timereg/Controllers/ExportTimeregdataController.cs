using Microsoft.AspNetCore.Mvc;
using Timereg.Data;

namespace Timereg
{
    public partial class ExportTimeregdataController : ExportController
    {
        private readonly TimeregdataContext context;

        public ExportTimeregdataController(TimeregdataContext context)
        {
            this.context = context;
        }

        [HttpGet("/export/Timeregdata/employees/csv")]
        public FileStreamResult ExportEmployeesToCSV()
        {
            return ToCSV(ApplyQuery(context.Employees, Request.Query));
        }

        [HttpGet("/export/Timeregdata/employees/excel")]
        public FileStreamResult ExportEmployeesToExcel()
        {
            return ToExcel(ApplyQuery(context.Employees, Request.Query));
        }

        [HttpGet("/export/Timeregdata/projects/csv")]
        public FileStreamResult ExportProjectsToCSV()
        {
            return ToCSV(ApplyQuery(context.Projects, Request.Query));
        }

        [HttpGet("/export/Timeregdata/projects/excel")]
        public FileStreamResult ExportProjectsToExcel()
        {
            return ToExcel(ApplyQuery(context.Projects, Request.Query));
        }

        [HttpGet("/export/Timeregdata/timeuseds/csv")]
        public FileStreamResult ExportTimeusedsToCSV()
        {
            return ToCSV(ApplyQuery(context.Timeuseds, Request.Query));
        }

        [HttpGet("/export/Timeregdata/timeuseds/excel")]
        public FileStreamResult ExportTimeusedsToExcel()
        {
            return ToExcel(ApplyQuery(context.Timeuseds, Request.Query));
        }

        [HttpGet("/export/Timeregdata/vtimeuseds/csv")]
        public FileStreamResult ExportVTimeusedsToCSV()
        {
            return ToCSV(ApplyQuery(context.VTimeuseds, Request.Query));
        }

        [HttpGet("/export/Timeregdata/vtimeuseds/excel")]
        public FileStreamResult ExportVTimeusedsToExcel()
        {
            return ToExcel(ApplyQuery(context.VTimeuseds, Request.Query));
        }
    }
}
