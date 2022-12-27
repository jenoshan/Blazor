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

namespace Timereg.Pages
{
    public partial class TimeusedCalendarComponent : ComponentBase
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

        protected RadzenScheduler<Timereg.Models.Timeregdata.VTimeused> scheduler0;

        int Uid;

        IEnumerable<Timereg.Models.Timeregdata.VTimeused> _getTimeusedsResult;
        protected IEnumerable<Timereg.Models.Timeregdata.VTimeused> getTimeusedsResult
        {
            get
            {
                return _getTimeusedsResult;
            }
            set
            {
                if (!object.Equals(_getTimeusedsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getTimeusedsResult", NewValue = value, OldValue = _getTimeusedsResult };
                    _getTimeusedsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected string title = "Time used by";
        private string _dtimeused;
         protected string dtimeused
        {
            get
            {
                return _dtimeused;
            }
            set
            {
                if (!object.Equals(dtimeused, value))
                {
                    _dtimeused = value;
                   
                    int ind = _dtimeused.IndexOf(".");
                    int hour = 0, min = 0;
                    if (ind > 0)
                    {
                        int.TryParse(_dtimeused.Substring(0,ind), out hour);
                        int.TryParse(_dtimeused.Substring(ind+1), out min);
                    } else int.TryParse(_dtimeused, out hour);
                    timeused.usedtime = new TimeSpan(0, hour, min, 0);
                    timeused.timeto = timeused.timefrom + timeused.usedtime;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }
        private Timereg.Models.Timeregdata.Timeused _timeused;
        protected Timereg.Models.Timeregdata.Timeused timeused
        {
            get
            {
                return _timeused;
            }
            set
            {
                if (!object.Equals(_timeused, value))
                {
                    _timeused = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }
        protected  DateTime Lastdate { get; set; }
        IEnumerable<Timereg.Models.Timeregdata.VUserProject> _getProjectsResult;
        protected IEnumerable<Timereg.Models.Timeregdata.VUserProject> getProjectsResult
        {
            get
            {
                return _getProjectsResult;
            }
            set
            {
                if (!object.Equals(_getProjectsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getProjectsResult", NewValue = value, OldValue = _getProjectsResult };
                    _getProjectsResult = value;
                    OnPropertyChanged(args);
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
                title = "Time used by " + Security.User.Name;
                await Load();
            }
            //await Load();

        }
        protected async System.Threading.Tasks.Task Load()
        {
            Uid = await UserProfile.GetUserId(Security, Timeregdata);
            var timeregdataGetTimeusedsResult = await Timeregdata.GetVTimeuseds();
            getTimeusedsResult = timeregdataGetTimeusedsResult.ToList().FindAll(r => r.userid == Uid).OrderBy(d=>d.workdate);
            var last = getTimeusedsResult.LastOrDefault();
            if (last != null) Lastdate = last.workdate;
            else Lastdate = DateTime.Now;

            await AddTime(Lastdate, Lastdate);
            timeused = null;
          
        }

        protected async System.Threading.Tasks.Task Scheduler0AppointmentSelect(SchedulerAppointmentSelectEventArgs<Timereg.Models.Timeregdata.VTimeused> args)
        {
            await Updatetime(args.Data.tid);
         
        }
        protected void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<VTimeused> args)
        {
            // Never call StateHasChanged in AppointmentRender - would lead to infinite loop
            if (args.Data.color !=null)
            {
                args.Attributes["style"] = "background: "+ args.Data.color;
            }
           
        }
        protected async System.Threading.Tasks.Task Scheduler0SlotSelect(SchedulerSlotSelectEventArgs args)
        {
        
            var _dtimefrom = (args.Start.Minute==30 ? args.Start.AddMinutes(-30) : args.Start);
            var _dtimeto =args.Start.AddHours(2);
           await AddTime(_dtimefrom, _dtimeto);
                     
        }
        protected async System.Threading.Tasks.Task AddTime(DateTime Start,DateTime End)
        {
            var timeregdataGetProjectsResult = await Timeregdata.GetVUserProjects();
            getProjectsResult = timeregdataGetProjectsResult.ToList().FindAll(u=>u.userid== Uid && (u.validto == null || u.validto >= Start));
            var dt = Start;
            dt = dt.AddTicks(-(dt.Ticks % (60 * 10_000_000)));
         
            timeused = new Timereg.Models.Timeregdata.Timeused()
            {
                userid = Uid,
                workdate = dt,
                timefrom = Start,
                timeto = End
            };
            timeused.usedtime = End - Start;
            _dtimeused = timeused.usedtime.Hours.ToString() + "." + timeused.usedtime.Minutes.ToString(); //new DateTime(dt.Year,dt.Month,dt.Day, timeused.usedtime.Hours, timeused.usedtime.Minutes, 0);
         
        }
        protected async System.Threading.Tasks.Task AddSubmit(Timereg.Models.Timeregdata.Timeused args)
        {
            try
            {
                if(args.tid>0)
                   await Timeregdata.UpdateTimeused(args.tid, args);
                else 
                    await Timeregdata.CreateTimeused(timeused);
                timeused = null;
               await Load(); 
            }
            catch (System.Exception timeregdataCreateTimeusedException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Timeused!");
            }
        }
        protected async System.Threading.Tasks.Task Updatetime(int tid)
        {
            var timeregdataGetTimeusedBytidResult = await Timeregdata.GetTimeusedBytid(tid);
            timeused = timeregdataGetTimeusedBytidResult;
            var dt = timeused.workdate;
            _dtimeused = timeused.usedtime.Hours.ToString() + "." + timeused.usedtime.Minutes.ToString(); //new DateTime(dt.Year,dt.Month,dt.Day, timeused.usedtime.Hours, timeused.usedtime.Minutes, 0);
         }
        protected async System.Threading.Tasks.Task ButtonDeleteClick(MouseEventArgs args)
        {
            var result = await JSRuntime.InvokeAsync<bool>("confirm", new [] {"Are you sure you want to delete this?"});
            try
            {
                if(result==true){
                var timeregdataDeleteTimeusedResult = await Timeregdata.DeleteTimeused(timeused.tid);
               // DialogService.Close(timeused);
                if (timeregdataDeleteTimeusedResult != null) {
                    timeused = null;
                    await Load();
                }
                }
            }
            
            catch (System.Exception timeregdataDeleteTimeusedException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Timeused");
            }
        }
        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            timeused = null;
            DialogService.Close(null);
        }

    }
}
