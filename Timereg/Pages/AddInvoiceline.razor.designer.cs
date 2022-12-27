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
    public partial class AddInvoicelineComponent : ComponentBase
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
        public dynamic invoiceid { get; set; }

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
            getInvoice = gettetentinvoice;

            var getemployee = await Timeregdata.GetEmployees();
            getEmployee = getemployee;

            invoiceline = new Timereg.Models.Timeregdata.Invoiceline();
            invoiceline.invoiceid = Convert.ToInt32(invoiceid);
        }

        protected async System.Threading.Tasks.Task Form0Submit(Timereg.Models.Timeregdata.Invoiceline args)
        {
            try
            {
                int MaxNo = 0;
                var invoicelinelist = await Timeregdata.GetInvoiceline();
                var selectlist = invoicelinelist.Where(x => x.invoiceid == args.invoiceid).ToList();
                if(selectlist.Count() > 0)
                {
                    MaxNo = selectlist.Max(m => m.invoicelineid) + 1;
                }
                else
                {
                    MaxNo = 1;
                }
                
                args.invoicelineid = MaxNo;

                var timeregdataCreateProjectResult = await Timeregdata.CreateTenentInvoicelineConfig(args);
                NotificationService.Notify(NotificationSeverity.Success, $"Success", $"success to create new  invoiceline!");
                DialogService.Close(invoiceline);

                var invoice = new Timereg.Models.Timeregdata.Invoice();
                invoice = await Timeregdata.GetInvoiceByid(args.invoiceid);           
                var listof = invoicelinelist.Where(x => x.invoiceid == args.invoiceid).ToList();

                invoice.updatedby = 0;
                invoice.updateddate = DateTime.Now;
                invoice.amount = (double)(listof.Sum(b => b.amount));
                var timeregdataUpdateInvoiceResult = await Timeregdata.UpdateInvoice(args.invoiceid, invoice);
            }
            catch (System.Exception)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new invoiceline!");
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
