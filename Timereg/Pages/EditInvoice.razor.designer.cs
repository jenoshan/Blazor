using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Timereg.Models;
namespace Timereg.Pages
{
    public partial class EditInvoiceComponent : ComponentBase
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

        public bool disable;

        Timereg.Models.Timeregdata.Invoice _invoice;
        protected Timereg.Models.Timeregdata.Invoice invoice
        {
            get
            {
                return _invoice;
            }
            set
            {
                if (!object.Equals(_invoice, value))
                {
                    _invoice = value;
                    Reload();
                }
            }
        }

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

        int Uid;
        protected async System.Threading.Tasks.Task Load()
        {
            var getcompany = await Timeregdata.GetCompany();
            getCompany = getcompany;

            Uid = await UserProfile.GetUserId(Security, Timeregdata);

            var timeregdataGetInvooiceByidResult = await Timeregdata.GetInvoiceByid(invoiceid);
            invoice = timeregdataGetInvooiceByidResult;

            if (invoice.islock == true)
            {
                disable = true;
            }
            else
            {
                disable = false;
            }
        }

        protected async System.Threading.Tasks.Task Form0Submit(Timereg.Models.Timeregdata.Invoice args)
        {
            try
            {
                args.updatedby = Uid;
                args.updateddate = DateTime.Now;
                var timeregdataUpdateInvoiceResult = await Timeregdata.UpdateInvoice(invoiceid, invoice);
                NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Success to update Invoice");
                DialogService.Close(invoice);
            }
            catch (System.Exception)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update Invoice");
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
                    invoice.islock = true;
                }
            }
            else
            {
                invoice.islock = false;
            }
        }
    }
}
