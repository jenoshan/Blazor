using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Timereg.Models.Timeregdata;
using Microsoft.EntityFrameworkCore;
using Timereg.Models;
using Timereg.Pages;


namespace Timereg.Pages
{
    public partial class InvoiceReportComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
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

        public double totAmount;


        protected RadzenGrid<Timereg.Models.Timeregdata.Invoiceline> grid1;

        public Timereg.Models.Timeregdata.Company tocompany;

        private Timereg.Models.Timeregdata.Company _fromcompany;
        public Timereg.Models.Timeregdata.Company fromcompany
        {
            get
            {
                return _fromcompany;
            }
            set
            {
                if (!object.Equals(_fromcompany, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "fromcompany", NewValue = value, OldValue = _fromcompany };
                    _fromcompany = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        private Timereg.Models.Timeregdata.Invoice _invoice;
        public Timereg.Models.Timeregdata.Invoice invoice
        {
            get
            {
                return _invoice;
            }
            set
            {
                if (!object.Equals(_invoice, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "invoice", NewValue = value, OldValue = _invoice };
                    _invoice = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        private List<Timereg.Models.Timeregdata.Invoiceline> _invoiceline;
        public List<Timereg.Models.Timeregdata.Invoiceline> invoiceline
        {
            get
            {
                return _invoiceline;
            }
            set
            {
                if (!object.Equals(_invoiceline, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "invoiceline", NewValue = value, OldValue = _invoiceline };
                    _invoiceline = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }

        protected async System.Threading.Tasks.Task Load()
        {
            var getinvoice = await Timeregdata.GetInvoiceByid(invoiceid);
            invoice = getinvoice;

            var getfromcompany = await Timeregdata.GetCompanyByid(invoice.companyid);
            if (getfromcompany != null)
            {
                fromcompany = getfromcompany;
            }

            var gettocompany = await Timeregdata.GetCompanyByid(invoice.tocompanyid);
            if (gettocompany != null)
            {
                tocompany = gettocompany;
            }

            var getinvoiceline = await Timeregdata.GetInvoiceline();
            invoiceline = getinvoiceline.Where(i => i.invoiceid == invoice.invoiceid).ToList();

            foreach (var tot in invoiceline)
            {
                if (tot.amount.HasValue)
                    totAmount += tot.amount.Value;
            }

            img = invoice.imgname;
            if (string.IsNullOrEmpty(img))
            {
                img = "esys_solution.jpeg";
            }
        }

        protected async System.Threading.Tasks.Task downloadMapReport()
        {
            var totamount = Convert.ToInt32(totAmount);
            var Com_Nam = fromcompany.companyname;
            var Addr_One = fromcompany.address1;
            var Cit = fromcompany.city;
            var Contry_Nam = fromcompany.country;
            var Post_Cod = fromcompany.postalcode;
            var ToCom_Nam = tocompany.companyname;
            var Re_Mark = invoice.remark;
            var ToContry_Nam = tocompany.country;
            var Tot_Amount = string.Format("{0:N}", totamount);
            var Inv_No = invoice.invoiceno.ToString();
            var Inv_Date = invoice.invoicedate.ToShortDateString();
            var InvDue_Date = invoice.duedate.ToShortDateString();
            var Cur_Cod = invoice.currencycode;
            var Img_Logo = invoice.imgname;
            var Bank_Nam = fromcompany.bankname;
            var Acc_Nam = fromcompany.accountname;
            var Branch = fromcompany.branch;
            var Acc_Num = fromcompany.accountno;


            await JSRuntime.InvokeVoidAsync("PrintDiv", Com_Nam, Addr_One, Cit, Contry_Nam, Post_Cod, Inv_No, Inv_Date, InvDue_Date, ToCom_Nam, ToContry_Nam, Tot_Amount, Re_Mark, Cur_Cod, Img_Logo, Bank_Nam, Acc_Nam, Branch, Acc_Num, invoiceline);
        }

        public string img = "";

        public async Task SetImage(string image)
        {
            img = image;
            invoice.imgname = img;
            var timeregdataUpdateInvoiceResult = await Timeregdata.UpdateInvoice(invoice.invoiceid, invoice);
            StateHasChanged();
        }
    }
}
