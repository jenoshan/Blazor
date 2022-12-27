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
using System.Globalization;
using System.Collections.ObjectModel;
using Timereg.Data;
using System.Linq.Dynamic.Core;

namespace Timereg.Pages
{
    public partial class DashboardUserComponent : ComponentBase
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

        public List<CheckboxList<string>> CountrySelectList { get; set; }
        protected bool SelectAllCountry = true;


        protected RadzenGrid<Timereg.Models.Timeregdata.TimeusedPerProjMonth> grid0;
        protected RadzenGrid<VUserWithTimeVM> grid02;
        protected RadzenGrid<Timereg.Models.Timeregdata.VTimeusedPeruser> Timegrid0;
        protected RadzenGrid<Timereg.Models.Timeregdata.VTimeusedPeruser> Timegrid02;
        public VTimeusedPerProj[] Projecttime { get; set; }
        // public List<VTimeusedPerProj2> PiechartDAta { get; set; }
        public List<PPUSerMonth> PPUSerMonths { get; set; }
        public List<PPUSerMonth> Usertime { get; set; }
        public List<TimeusedPerProjMonth> Projmonth { get; set; }

        IEnumerable<VUserWithTime> GETUSerTime;
        IEnumerable<Timereg.Models.Timeregdata.VTimeusedPeruser> GgetUserTimeTable;
        IEnumerable<Timereg.Models.Timeregdata.VTimeusedPeruser> GgetUserTimeV;
        public List<Monthsweek> Weeks { get; set; }
        public List<Year> Years { get; set; }

        #region Properties

        private bool _weekactive = false;
        private bool inupdate = false;
        private int _weekno;
        private int manid;



        private int _companyid;
        public int companyid
        {
            get { return _companyid; }
            set
            {
                if (_companyid != value)
                {
                    _companyid = value;
                    var timeregdataGetTimeusedsResult = Timeregdata.GetVUserWithTimes().Result;

                    GETUSerTime = timeregdataGetTimeusedsResult.ToList().FindAll(y => y.companyid == companyid);

                    Load4();
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }


        public int Weekno
        {
            get { return _weekno; }
            set
            {
                if (_weekno != value)
                {
                    _weekno = value;
                    _weekactive = true;
                    Loadweek();
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }
        private async Task<IEnumerable<VUserWithTime>> get_User_time()
        {
            if (GETUSerTime == null)
            {
                // var qry = new Query() { Filter = $"manageruserid=={manid}" };
                var timeregdataGetTimeusedsResult = await Timeregdata.GetVUserWithTimes();
                Years = timeregdataGetTimeusedsResult.GroupBy(p => p.workyear).Select(p => new Year() { Yearid = p.Key, Yearname = p.Key.ToString() }).ToList();

                GETUSerTime = timeregdataGetTimeusedsResult.ToList().FindAll(y => y.workyear == YearId);

                monthResults = GETUSerTime.OrderBy(o => o.workmonth).GroupBy(x => x.workmonth)
                    .Select(x => new MonthResult()
                    {
                        MonthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(x.Key),
                        MonthNum = x.Key
                    });
            }
            return GETUSerTime;
        }

        protected async System.Threading.Tasks.Task Loadweek()
        {
            getUserTime2 = null;
            // var qry = new Query() { Filter = $"manageruserid=={manid}" };
            var timeregdataGetProjectsResult = await Timeregdata.GetVTimeusedPeruser();
            var timeregdataGetTimeusedsResult = await get_User_time();
            if (_YearId == 0) _YearId = DateTime.Now.Year;
            if (_weekno != 0)
            {
                GgetUserTimeV = timeregdataGetProjectsResult.Where(x => x.workyear == YearId && x.weekno == _weekno);
                timeregdataGetTimeusedsResult = timeregdataGetTimeusedsResult.Where(x => x.weekno == _weekno);
                var week = Weeks.FirstOrDefault(w => w.weekid == _weekno);
                if (week != null)
                    MontStr = week.week;
                else MontStr = "";

            }
            else
            {
                GgetUserTimeV = timeregdataGetProjectsResult.Where(x => x.workyear == YearId);
                MontStr = "All Months";
            }

            getUserTimeV = null;
            getTimeusedsResult = GgetUserTimeV.GroupBy(x => new { x.userid, x.weekno, x.workyear, x.username })
                             .Select(c => new VUserWithTimeVM
                             {
                                 userid = c.Key.userid,
                                 username = c.Key.username,
                                 workyear = c.Key.workyear,
                                 workmonthnum = c.Key.weekno,
                                 UserTime = (Math.Floor(100 * c.Sum(r => r.totalminuts) / 60.00)) / 100,
                                 workmonth = (Weeks.FirstOrDefault(w => w.weekid == c.Key.weekno)?.week),
                                 Approved = c.Sum(r => r.approved),
                                 registered = c.Sum(r => r.registered),
                                 IsApproved = c.Sum(r => r.approved) == c.Sum(r => r.registered)
                             });

            Projecttime = GgetUserTimeV.ToList().GroupBy(l => l.projectname)
                            .Select(cl => new VTimeusedPerProj
                            {
                                projectname = cl.First().projectname,
                                totalminuts = cl.Sum(r => r.totalminuts)
                            }).ToArray();



            Projmonth = GgetUserTimeV.ToList().GroupBy(l => l.projectname)
                           .Select(cl => new TimeusedPerProjMonth
                           {
                               workmonth = cl.First().workmonth,
                               month = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(cl.First().workmonth),
                               projectname = cl.First().projectname,
                               totalhours = (Math.Floor(100 * cl.Sum(r => r.totalminuts) / 60.00)) / 100,

                           }).ToList();

            //Chart DAta
            PPUSerMonths = GgetUserTimeV.ToList().GroupBy(l => l.projectname)
                           .Select(cl => new PPUSerMonth
                           {
                               country = cl.First().projectname,
                               litres = cl.Sum(r => r.totalminuts),
                               subDatas = GgetUserTimeV.ToList().FindAll(f => f.projectname == cl.First().projectname).GroupBy(g => g.username).
                               Select(u => new subData
                               {
                                   name = u.First().username,
                                   value = u.Sum(s => s.totalminuts)
                               }).ToList()
                           }).ToList();
            //Chart DAta
            Usertime = GgetUserTimeV.ToList().GroupBy(l => l.username)
                           .Select(cl => new PPUSerMonth
                           {
                               country = cl.First().username,
                               litres = cl.Sum(r => r.totalminuts),
                               subDatas = GgetUserTimeV.ToList().FindAll(f => f.username == cl.First().username).GroupBy(g => g.projectname).
                               Select(u => new subData
                               {
                                   name = u.First().projectname,
                                   value = u.Sum(s => s.totalminuts)
                               }).ToList()
                           }).ToList();

            await PiechartLoad3();



        }
        private int myVar;
        public string yeartxt { get { return "Total hours for year " + YearId.ToString(); } }

        private int _YearId;

        public int YearId
        {
            get { return _YearId; }
            set
            {
                if (_YearId != value)
                {
                    _YearId = value;
                    var timeregdataGetTimeusedsResult = Timeregdata.GetVUserWithTimes().Result;

                    GETUSerTime = timeregdataGetTimeusedsResult.ToList().FindAll(y => y.workyear == YearId);

                    monthResults = GETUSerTime.OrderBy(o => o.workmonth).GroupBy(x => x.workmonth)
                        .Select(x => new MonthResult()
                        {
                            MonthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(x.Key),
                            MonthNum = x.Key
                        });
                    Load4();

                    _weekactive = false;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        private bool _Approved;
        public bool Approved
        {
            get { return _Approved; }
            set
            {
                if (_Approved != value)
                {
                    _Approved = value;
                    if (inupdate) UpdateAproved(_Approved);
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }
        public int MonthId
        {
            get { return myVar; }
            set
            {
                if (myVar != value)
                {
                    myVar = value;
                    // fnLoad();
                    Load4();
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        private string _MontStr;

        public string MontStr
        {
            get { return _MontStr; }
            set
            {
                if (_MontStr != value)
                {
                    _MontStr = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }
        public IEnumerable<MonthResult> monthResults;

        IEnumerable<Timereg.Models.Timeregdata.VTimeusedPeruser> _getUserTimeV;
        protected IEnumerable<Timereg.Models.Timeregdata.VTimeusedPeruser> getUserTimeV
        {
            get
            {
                return _getUserTimeV;
            }
            set
            {
                if (!object.Equals(_getUserTimeV, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getUserTimeV", NewValue = value, OldValue = _getUserTimeV };
                    _getUserTimeV = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<Timereg.Models.Timeregdata.VTimeusedPeruser> _getUserTime2;
        protected IEnumerable<Timereg.Models.Timeregdata.VTimeusedPeruser> getUserTime2
        {
            get
            {
                return _getUserTime2;
            }
            set
            {
                if (!object.Equals(_getUserTime2, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getUserTime2", NewValue = value, OldValue = _getUserTime2 };
                    _getUserTime2 = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<VUserWithTimeVM> _getTimeusedsResult;
        protected IEnumerable<VUserWithTimeVM> getTimeusedsResult
        {
            get
            {
                return _getTimeusedsResult;
            }
            set
            {
                if (!object.Equals(_getTimeusedsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getTimeusedsResult", NewValue = value, OldValue = _getTimeusedsResult };
                    _getTimeusedsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Company> _getCompany;
        protected IEnumerable<Company> getCompany
        {
            get
            {
                return _getCompany;
            }
            set
            {
                if (!object.Equals(_getTimeusedsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCompany", NewValue = value, OldValue = getCompany };
                    _getCompany = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        #endregion


        #region User Time base chart  

        protected async System.Threading.Tasks.Task Load4()
        {
            var timeregdataGetProjectsResult = await Timeregdata.GetVTimeusedPeruser();
            var timeregdataGetTimeusedsResult = await Timeregdata.GetVUserWithTimes();

            if (MonthId != 0)
            {
                GgetUserTimeV = timeregdataGetProjectsResult.Where(x => x.workmonth == MonthId);
                timeregdataGetTimeusedsResult = timeregdataGetTimeusedsResult.Where(x => x.workmonth == MonthId);
                MontStr = monthResults.Where(x => x.MonthNum == MonthId).Select(x => x.MonthName).FirstOrDefault();
            }

            else
            {
                GgetUserTimeV = timeregdataGetProjectsResult;
                MontStr = "All Months";
            }

            if (companyid != 0)
            {
                GgetUserTimeV = GgetUserTimeV.Where(x => x.companyid == companyid);
                timeregdataGetTimeusedsResult = timeregdataGetTimeusedsResult.Where(x => x.companyid == companyid);
            }

            getUserTimeV = null;
            getTimeusedsResult = GgetUserTimeV.GroupBy(x => new { x.userid, x.workmonth, x.workyear, x.username })
                             .Select(c => new VUserWithTimeVM
                             {
                                 userid = c.Key.userid,
                                 username = c.Key.username,
                                 workyear = c.Key.workyear,
                                 workmonthnum = c.Key.workmonth,
                                 UserTime = (Math.Floor(100 * c.Sum(r => r.totalminuts) / 60.00)) / 100,
                                 workmonth = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(c.Key.workmonth),
                             });

            Projecttime = GgetUserTimeV.ToList().GroupBy(l => l.projectname)
                            .Select(cl => new VTimeusedPerProj
                            {
                                projectname = cl.First().projectname,
                                totalminuts = cl.Sum(r => r.totalminuts)
                            }).ToArray();



            Projmonth = GgetUserTimeV.ToList().GroupBy(l => l.projectname)
                           .Select(cl => new TimeusedPerProjMonth
                           {
                               workmonth = cl.First().workmonth,
                               month = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(cl.First().workmonth),
                               projectname = cl.First().projectname,
                               totalhours = (Math.Floor(100 * cl.Sum(r => r.totalminuts) / 60.00)) / 100,

                           }).ToList();

            //Chart DAta
            PPUSerMonths = GgetUserTimeV.ToList().GroupBy(l => l.projectname)
                           .Select(cl => new PPUSerMonth
                           {
                               country = cl.First().projectname,
                               litres = cl.Sum(r => r.totalminuts),
                               subDatas = GgetUserTimeV.ToList().FindAll(f => f.projectname == cl.First().projectname).GroupBy(g => g.username).
                               Select(u => new subData
                               {
                                   name = u.First().username,
                                   value = u.Sum(s => s.totalminuts)
                               }).ToList()
                           }).ToList();
            //Chart DAta
            Usertime = GgetUserTimeV.ToList().GroupBy(l => l.username)
                           .Select(cl => new PPUSerMonth
                           {
                               country = cl.First().username,
                               litres = cl.Sum(r => r.totalminuts),
                               subDatas = GgetUserTimeV.ToList().FindAll(f => f.username == cl.First().username).GroupBy(g => g.projectname).
                               Select(u => new subData
                               {
                                   name = u.First().projectname,
                                   value = u.Sum(s => s.totalminuts)
                               }).ToList()
                           }).ToList();

            await PiechartLoad3();



        }

        protected async System.Threading.Tasks.Task PiechartLoad3()
        {

            // await JSRuntime.InvokeVoidAsync("NestedChart", MyPPUSerMonths);
            await JSRuntime.InvokeVoidAsync("NestedChart", PPUSerMonths, "chartdiv2");
            await JSRuntime.InvokeVoidAsync("NestedChart", Usertime, "chartdiv1");
        }

        protected async Task downloadReport()
        {
            //var ex = new DownloadController(Timeregdata);
            //var stream = await ex.ExportTimeusedPeruserToCSV();
            //return stream;
            UriHelper.NavigateTo($"/export/Timereg/TimeusedPeruser/excel", true);


            // await JSRuntime.InvokeVoidAsync("createChart", PiechartDAta);
        }
        #endregion


        #region CAlender
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {

                await Load();
                await Load4();
            }
        }



        #endregion



        protected async System.Threading.Tasks.Task Load()
        {
            var getcompanylist = await Timeregdata.GetCompany();
            getCompany = getcompanylist;

            try
            {
                var timeregdataGetTimeusedsResult = await Timeregdata.GetVUserWithTimes();
                GETUSerTime = timeregdataGetTimeusedsResult;
                Years = timeregdataGetTimeusedsResult.GroupBy(p => p.workyear).Select(p => new Year() { Yearid = p.Key, Yearname = p.Key.ToString() }).ToList();
                if (_YearId == 0) _YearId = DateTime.Now.Year;
                if (MonthId == 0) MonthId = DateTime.Now.Month;

                monthResults = GETUSerTime.GroupBy(x => x.workmonth)
                    .Select(x => new MonthResult()
                    {
                        MonthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(x.Key),
                        MonthNum = x.Key
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        protected async System.Threading.Tasks.Task Grid0RowSelect(Timereg.Models.Timeregdata.TimeusedPerProjMonth args)
        {

            getUserTimeV = GgetUserTimeV.ToList().FindAll(r => r.workmonth == args.workmonth && r.projectname == args.projectname);
        }



        protected async System.Threading.Tasks.Task Grid0RowSelect2(Timereg.Data.VUserWithTimeVM args)
        {
            var timeregdataGetProjectsResult = await Timeregdata.GetVTimeusedPeruser();
            GgetUserTimeTable = timeregdataGetProjectsResult;
            getUserTime2 = GgetUserTimeTable.ToList().FindAll(r => r.workmonth == args.workmonthnum && r.userid == args.userid);
        }

        private async System.Threading.Tasks.Task UpdateAproved(bool approved)
        {
            if (getUserTime2 == null) return;
            var ut = getUserTime2.FirstOrDefault();
            var fromd = getUserTime2.Min(r => r.fromdate);

            var tod = getUserTime2.Max(r => r.todate);
            await Timeregdata.ApproveTimeused(ut.userid, fromd, tod, (approved ? 1 : 0));
        }
    }
}
