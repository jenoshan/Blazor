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
    public partial class TimeusedComponent : ComponentBase
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

        protected RadzenGrid<Timereg.Models.Timeregdata.Timeused> grid0;

        IEnumerable<Timereg.Models.Timeregdata.Project> _getProjectsResult;
        protected IEnumerable<Timereg.Models.Timeregdata.Project> getProjectsResult
        {
            get
            {
                return _getProjectsResult;
            }
            set
            {
                if (!object.Equals(_getProjectsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getProjectsResult", NewValue = value, OldValue = _getProjectsResult };
                    _getProjectsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Timereg.Models.Timeregdata.Employee> _getEmployeesResult;
        protected IEnumerable<Timereg.Models.Timeregdata.Employee> getEmployeesResult
        {
            get
            {
                return _getEmployeesResult;
            }
            set
            {
                if (!object.Equals(_getEmployeesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getEmployeesResult", NewValue = value, OldValue = _getEmployeesResult };
                    _getEmployeesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Timereg.Models.Timeregdata.Timeused> _getTimeusedsResult;
        protected IEnumerable<Timereg.Models.Timeregdata.Timeused> getTimeusedsResult
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

        Timereg.Models.Timeregdata.Timeused _timeused;
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
                    var args = new PropertyChangedEventArgs(){ Name = "timeused", NewValue = value, OldValue = _timeused };
                    _timeused = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        int Uid { get; set; }
        bool _isEdit;
        protected bool isEdit
        {
            get
            {
                return _isEdit;
            }
            set
            {
                if (!object.Equals(_isEdit, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "isEdit", NewValue = value, OldValue = _isEdit };
                    _isEdit = value;
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
                await Load();
            }
            //await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            Uid = await UserProfile.GetUserId(Security, Timeregdata);
            var timeregdataGetProjectsResult = await Timeregdata.GetProjects();
            getProjectsResult = timeregdataGetProjectsResult;

            var timeregdataGetEmployeesResult = await Timeregdata.GetEmployees();
            getEmployeesResult = timeregdataGetEmployeesResult;

            var timeregdataGetTimeusedsResult = await Timeregdata.GetTimeuseds();
            getTimeusedsResult = timeregdataGetTimeusedsResult;

            timeused = timeregdataGetTimeusedsResult.FirstOrDefault();

            isEdit = true;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
          
            if (Uid < 1) return;
            timeused = new Timereg.Models.Timeregdata.Timeused() { userid = Uid };

            isEdit = false;
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Timereg.Models.Timeregdata.Timeused args)
        {
            isEdit = true;

            timeused = args;
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            var result = await JSRuntime.InvokeAsync<bool>("confirm", new [] {"Are you sure you want to delete this?"});
            try
            {
                if(result==true){
                var timeregdataDeleteTimeusedResult = await Timeregdata.DeleteTimeused(data.tid);
                if (timeregdataDeleteTimeusedResult != null) {
                    await grid0.Reload();
                }
                }
            }
            catch (System.Exception timeregdataDeleteTimeusedException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Timeused");
            }
        }

        protected async System.Threading.Tasks.Task Form0Submit(Timereg.Models.Timeregdata.Timeused args)
        {
            try
            {
                if (isEdit)
                {
                    var timeregdataUpdateTimeusedResult = await Timeregdata.UpdateTimeused(timeused.tid, timeused);
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Timeused updated!");


                }
            }
            catch (System.Exception timeregdataUpdateTimeusedException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update Timeused");
            }

            try
            {
                if (!this.isEdit)
                {
                    var timeregdataCreateTimeusedResult = await Timeregdata.CreateTimeused(args);
                    timeused = new Timereg.Models.Timeregdata.Timeused();

                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Timeused created!");
                }
            }
            catch (System.Exception timeregdataCreateTimeusedException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Timeused!");
            }
        }
    }
}
