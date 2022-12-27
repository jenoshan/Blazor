using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace Timereg.Pages
{
    public partial class EditInvoicelineComponent : ComponentBase
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

        [Parameter]
        public dynamic invoicelineid { get; set; }

        [Parameter]
        public dynamic invoiceid { get; set; }

        public bool disable;

        Timereg.Models.Timeregdata.Invoiceline _invoiceline;
        protected Timereg.Models.Timeregdata.Invoiceline invoiceline
        {
            get
            {
                return _invoiceline;
            }
            set
            {
                if (!object.Equals(_invoiceline, value))
                {
                    _invoiceline = value;
                    Reload();
                }
            }
        }

        IEnumerable<Timereg.Models.Timeregdata.Employee> _getEmployee;
        protected IEnumerable<Timereg.Models.Timeregdata.Employee> getEmployee
        {
            get
            {
                return _getEmployee;
            }
            set
            {
                if (!object.Equals(_getEmployee, value))
                {
                    _getEmployee = value;
                    Reload();
                }
            }
        }

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

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                await Load();
            }
        }

        protected async System.Threading.Tasks.Task Load()
        {
            var gettetentinvoice = await Timeregdata.GetInvoice();
            getInvoice = gettetentinvoice.Where(i => i.islock == false || i.islock == null).ToList();

            var getemployee = await Timeregdata.GetEmployees();
            getEmployee = getemployee;

            Query query = new Query() { Filter = $@"i => i.invoiceid == {Convert.ToInt32(invoiceid)} && i.invoicelineid == {Convert.ToInt32(invoicelineid)}" };
            var GetInvoiceline = await Timeregdata.GetInvoiceline(query);
            /*var timeregdataGetInvoicelineByidResult = await Timeregdata.GetInvoicelineByid(Convert.ToInt32(invoicelineid));*/
            invoiceline = GetInvoiceline.FirstOrDefault();

            /*if (invoiceline.islock == true)
            {
                disable = true;
            }
            else
            {
                disable = false;
            }*/
        }

        protected async System.Threading.Tasks.Task Form0Submit(Timereg.Models.Timeregdata.Invoiceline args)
        {
            try
            {
                var timeregdataUpdateInvoiceResult = await Timeregdata.UpdateInvoiceline(Convert.ToInt32(invoicelineid), Convert.ToInt32(invoiceid), invoiceline);
                NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Success to update Invoice line");

                var invoice = new Timereg.Models.Timeregdata.Invoice();
                invoice = await Timeregdata.GetInvoiceByid(args.invoiceid);
                var invoicelinelist = await Timeregdata.GetInvoiceline();
                var listof = invoicelinelist.Where(x => x.invoiceid == args.invoiceid).ToList();

                invoice.updatedby = 0;
                invoice.updateddate = DateTime.Now;
                invoice.amount = (double)(listof.Sum(b => b.amount));
                var Result = await Timeregdata.UpdateInvoice(args.invoiceid, invoice);

                DialogService.Close(invoiceline);
            }
            catch (System.Exception)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update Invoice line");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        public async Task changeLock(bool? value)
        {
            if (value == true)
            {
                var result = await JSRuntime.InvokeAsync<bool>("confirm", new[] { "Are you sure you want to lock this?" });

                if (result == true)
                {
                    invoiceline.islock = true;
                }
            }
            else
            {
                invoiceline.islock = false;
            }
        }
    }
}
