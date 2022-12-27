using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace Timereg.Pages
{
    public partial class InvoiceComponent : ComponentBase
    {
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected TimeregdataService Timeregdata { get; set; }

        protected RadzenGrid<Timereg.Models.Timeregdata.Invoice> grid0;
        protected RadzenGrid<Timereg.Models.Timeregdata.Invoiceline> grid1;

        Timereg.Models.Timeregdata.Invoice masterline = new Models.Timeregdata.Invoice();

        private int _companyid;
        public int companyid
        {
            get { return _companyid; }
            set
            {
                if (_companyid != value)
                {
                    _companyid = value;
                    if (_companyid != 0)
                    {
                        getInvoice = globalInvoice.Where(i => i.tocompanyid == _companyid);
                    }
                    GgetInvoiceline = null;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        IEnumerable<Timereg.Models.Timeregdata.Invoice> globalInvoice;

        IEnumerable<Timereg.Models.Timeregdata.Invoice> _getInvoice;
        protected IEnumerable<Timereg.Models.Timeregdata.Invoice> getInvoice
        {
            get
            {
                return _getInvoice;
            }
            set
            {
                if (!object.Equals(_getInvoice, value))
                {
                    _getInvoice = value;
                    Reload();
                }
            }
        }

        List<Timereg.Models.Timeregdata.Company> toCompany;

        IEnumerable<Timereg.Models.Timeregdata.Company> _getCompany;
        protected IEnumerable<Timereg.Models.Timeregdata.Company> getCompany
        {
            get
            {
                return _getCompany;
            }
            set
            {
                if (!object.Equals(_getCompany, value))
                {
                    _getCompany = value;
                    Reload();
                }
            }
        }

        protected IEnumerable<Timereg.Models.Timeregdata.Invoiceline> GgetInvoiceline;

        IEnumerable<Timereg.Models.Timeregdata.Invoiceline> _getInvoiceline;
        protected IEnumerable<Timereg.Models.Timeregdata.Invoiceline> getInvoiceline
        {
            get
            {
                return _getInvoiceline;
            }
            set
            {
                if (!object.Equals(_getInvoiceline, value))
                {
                    _getInvoiceline = value;
                    Reload();
                }
            }
        }

        private int SelectInvoice;

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            GgetInvoiceline = null;
            await Load();
        }

        protected async System.Threading.Tasks.Task Load()
        {
            toCompany = new List<Timereg.Models.Timeregdata.Company>();

            var gettetentinvoice = await Timeregdata.GetInvoice();
            globalInvoice = gettetentinvoice.OrderBy(i => i.invoiceno);

            var gettetentinvoiceline = await Timeregdata.GetInvoiceline();
            getInvoiceline = gettetentinvoiceline;

            var getcompany = await Timeregdata.GetCompany();
            getCompany = getcompany;

            /*List<int?> companyidlist = globalInvoice.Select(n => n.tocompanyid).ToList();
            foreach (var res in companyidlist)
            {
                try
                {
                    var company = getcompany.Where(m => m.companyid == (res ?? 0))?.First();
                    toCompany.Add(company);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }*/

            /*getCompany = toCompany;*/
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddInvoice>("Add Invoice", new Dictionary<string, object>() { { "companyid", companyid } });
            await grid0.Reload();
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task NewinvoiceClick(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddInvoice>("Add Invoice", new Dictionary<string, object>() { { "companyid", companyid } });
            DateTime invdate = DateTime.Today;
            DateTime duedate = invdate.AddDays(14);
            await grid0.Reload();
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Button1Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddInvoiceline>("Add Invoice line", new Dictionary<string, object>() { { "invoiceid", SelectInvoice } });
            await Grid0RowSelect(masterline);
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Timereg.Models.Timeregdata.Invoice args)
        {
            masterline = args;
            SelectInvoice = args.invoiceid;
            GgetInvoiceline = getInvoiceline.Where(i => i.invoiceid == args.invoiceid).ToList();
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid1RowSelect(Timereg.Models.Timeregdata.Invoiceline args)
        {
            var dialogResult = await DialogService.OpenAsync<EditInvoiceline>("Edit Invoice line", new Dictionary<string, object>() { { "invoicelineid", args.invoicelineid }, { "invoiceid", args.invoiceid } });
            await Grid0RowSelect(masterline);
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            var result = await JSRuntime.InvokeAsync<bool>("confirm", new[] { "Are you sure you want to delete this?" });
            try
            {
                if (result == true)
                {
                    var timeregdataDeleteInvoiceResult = await Timeregdata.DeleteInvoice(data.invoiceid);
                    if (timeregdataDeleteInvoiceResult != null)
                    {
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Success to delete Invoice");
                        //await grid0.Reload();
                        await InvokeAsync(() => { StateHasChanged(); });
                    }
                }
            }
            catch (System.Exception)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Invoice");
            }
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick0(MouseEventArgs args, dynamic data)
        {
            var result = await JSRuntime.InvokeAsync<bool>("confirm", new[] { "Are you sure you want to delete this?" });
            try
            {
                if (result == true)
                {
                    var timeregdataDeleteInvoicelineResult = await Timeregdata.DeleteInvoiceline(data.invoicelineid, data.invoiceid);
                    if (timeregdataDeleteInvoicelineResult != null)
                    {
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Success to delete Invoice line");
                        await Grid0RowSelect(masterline);
                        await InvokeAsync(() => { StateHasChanged(); });
                    }

                    int id = Convert.ToInt32(data.invoiceid);

                    var invoice = new Timereg.Models.Timeregdata.Invoice();
                    invoice = await Timeregdata.GetInvoiceByid(data.invoiceid);
                    var invoicelinelist = await Timeregdata.GetInvoiceline();
                    var listof = invoicelinelist.Where(x => x.invoiceid == id).ToList();

                    invoice.updatedby = 0;
                    invoice.updateddate = DateTime.Now;
                    invoice.amount = (double)(listof.Sum(b => b.amount));
                    var timeregdataUpdateInvoiceResult = await Timeregdata.UpdateInvoice(data.invoiceid, invoice);
                    StateHasChanged();
                }
            }
            catch (System.Exception)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Invoice line");
            }
        }

        public async Task goInvoicereport(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<InvoiceReport>("Invoice Report", new Dictionary<string, object>() { { "invoiceid", data.invoiceid } }, new DialogOptions() { Width = "1000px" });
        }

        public async Task EditInvoiceline(MouseEventArgs args, dynamic data)
        {
            var dialogResult = await DialogService.OpenAsync<EditInvoice>("Edit Invoice", new Dictionary<string, object>() { { "invoiceid", data.invoiceid } });
            await InvokeAsync(() => { StateHasChanged(); });
        }
    }
}
