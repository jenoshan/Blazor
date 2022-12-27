using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Timereg.Models;
using System.Globalization;

namespace Timereg.Pages
{
    public partial class AddInvoiceComponent : ComponentBase
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
        public dynamic companyid { get; set; }

        int Uid;

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

        protected async System.Threading.Tasks.Task Load()
        {
            var getcompany = await Timeregdata.GetCompany();
            getCompany = getcompany;

            Uid = await UserProfile.GetUserId(Security, Timeregdata);
            invoice = new Timereg.Models.Timeregdata.Invoice();

            invoice.tocompanyid = Convert.ToInt32(companyid);
            invoice.remark = "Invoice for " + DateTime.Today.ToString("MMM", CultureInfo.InvariantCulture); ;
            invoice.companyid = 3;
            onchangeinvoicedate(DateTime.Today);
        }
        private void onchangeinvoicedate(DateTime d)
        {
            invoice.invoiceyear = d.Year;
            invoice.invoicedate = d;
            invoice.duedate = d.AddDays(14);
            invoice.billingperiod = d.ToString("MMMM", CultureInfo.InvariantCulture);
           
        }
        protected async System.Threading.Tasks.Task Form0Submit(Timereg.Models.Timeregdata.Invoice args)
        {
            try
            {
                args.createdby = Uid;
                args.updatedby = Uid;
                args.createddate = DateTime.Now;
                args.updateddate = DateTime.Now;

                var AllInvoice = await Timeregdata.GetInvoice();
                var GetToCompanyList = AllInvoice.Where (x => x.tocompanyid == args.tocompanyid).ToList();
                int maxNo = GetToCompanyList.Max(x => x.invoiceno ?? 0) + 1;
                args.invoiceno = maxNo;

                var timeregdataCreateProjectResult = await Timeregdata.CreateTenentInvoiceConfig(args);
                if (timeregdataCreateProjectResult != null && args.includetime==true)
                {
                    Query query = new Query() { Filter = "invoiceid==null" }; // "invoiceid is null"
                    var timeregdataGetTimeusedsResult = await Timeregdata.GetVTimeusedPeruser(query);
                    var emp = await Timeregdata.GetEmployees();
                    var emlist = emp.ToList();
                    //var invprojects = projects.ToList().FindAll(p => p.companyid == companyid);
                    int yearid = invoice.invoicedate.Year, monthid = invoice.invoicedate.Month;
                    var Timeperuser = timeregdataGetTimeusedsResult.ToList().FindAll(r => r.workyear == yearid && r.workmonth == monthid).GroupBy(x => new { x.userid, x.workmonth, x.workyear, x.username, x.companyid }).Select(c => new
                    {
                        userid = c.Key.userid,
                        username = c.Key.username,
                        companyid = c.Key.companyid,
                        UserTime = (Math.Floor(100 * c.Sum(r => r.totalminuts) / 60.00)) / 100
                    }).OrderBy(o => o.userid);

                    var curuid = 0;
                    double curcompantusage = 0, totalusage = 0, commission = 0,workspace= 11450, totworkspace =0;
                    string curuser = "";
                    double? sal = null;
                    int inline = 1, invid = timeregdataCreateProjectResult.invoiceid;
                    if (emp != null)
                        foreach (var ti in Timeperuser)
                        {
                            if (curuid != ti.userid)
                            {

                                if (curuid != 0 && sal != null && curcompantusage >0) //create invoice line
                                {
                                  var invoiceline = new Timereg.Models.Timeregdata.Invoiceline() { invoiceid = invid, invoicelineid = inline++, 
                                        employeeid = curuid, description = "Service by " + curuser,qty=(int)(100* curcompantusage / totalusage), amount = sal.Value * curcompantusage / totalusage };
                                    var invl = await Timeregdata.CreateTenentInvoicelineConfig(invoiceline);
                                    commission += sal.Value * 0.05 * curcompantusage / totalusage; // 15% commission 
                                    totworkspace += workspace * curcompantusage / totalusage; // 15% commission 
                                }
                                curuid = ti.userid;
                                curuser = ti.username;
                                sal = emlist.FirstOrDefault(e => e.userid == ti.userid)?.salary;

                                curcompantusage = 0;
                                totalusage = 0;
                                
                            }
                            if (sal == null) continue;
                            if (ti.companyid == companyid) curcompantusage += ti.UserTime;
                            if (ti.companyid != null && ti.companyid!=3) // not include vacation
                                totalusage += ti.UserTime;
                        }
                    if (curuid != 0 && sal != null) //create invoice line
                    {
                      var invoiceline = new Timereg.Models.Timeregdata.Invoiceline()
                        {
                            invoiceid = invid,
                            invoicelineid = inline++,
                            employeeid = curuid,
                          qty = (int)(100 * curcompantusage / totalusage),
                            description = "Service by " + curuser,
                            amount = sal.Value * curcompantusage / totalusage
                        };
                        var invl = await Timeregdata.CreateTenentInvoicelineConfig(invoiceline);
                        commission += sal.Value * 0.05 * curcompantusage / totalusage; // 05% commission 
                        totworkspace += workspace * curcompantusage / totalusage; // 05% commission 
                    }
                    if (totworkspace > 0)
                    {
                        var invoiceline = new Timereg.Models.Timeregdata.Invoiceline()
                        {
                            invoiceid = invid,
                            invoicelineid = inline++,
                            description = "Workspace charges",
                            amount = totworkspace
                        };
                        var invl = await Timeregdata.CreateTenentInvoicelineConfig(invoiceline);
                    }

                    if (commission > 0)
                    {
                        var invoiceline = new Timereg.Models.Timeregdata.Invoiceline()
                        {
                            invoiceid = invid,
                            invoicelineid = inline++,
                            description = "Our commission" ,
                            amount = commission 
                        };
                        var invl = await Timeregdata.CreateTenentInvoicelineConfig(invoiceline);
                    }
                }
                NotificationService.Notify(NotificationSeverity.Success, $"Success", $"success to create new  invoice!");
                DialogService.Close(invoice);
            }
            catch (System.Exception es)
            {
                NotificationService.Notify(NotificationSeverity.Error, es.Message, $"Unable to create new invoice!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        public async Task changeLock(bool value)
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
