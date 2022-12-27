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
    public partial class EditUserProjectComponent : ComponentBase
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
        public dynamic Upid { get; set; }

        Timereg.Models.Timeregdata.UserProject _userproject;
        protected Timereg.Models.Timeregdata.UserProject userproject
        {
            get
            {
                return _userproject;
            }
            set
            {
                if (!object.Equals(_userproject, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "userproject", NewValue = value, OldValue = _userproject };
                    _userproject = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

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
        protected int Uid { get; set; }
        protected async System.Threading.Tasks.Task Load()
        {
            var timeregdataGetUserProjectByUpidResult = await Timeregdata.GetUserProjectByUpid(Upid);
            userproject = timeregdataGetUserProjectByUpidResult;

            var timeregdataGetProjectsResult = await Timeregdata.GetProjects();
            getProjectsResult = timeregdataGetProjectsResult;
            Uid = await UserProfile.GetUserId(Security, Timeregdata);

            var timeregdataGetEmployeesResult = await Timeregdata.GetEmployees();
            getEmployeesResult = timeregdataGetEmployeesResult.ToList().FindAll(f=>f.userid==Uid);
        }

        protected async System.Threading.Tasks.Task Form0Submit(Timereg.Models.Timeregdata.UserProject args)
        {
            try
            {
                var timeregdataUpdateUserProjectResult = await Timeregdata.UpdateUserProject(Upid, userproject);
                DialogService.Close(userproject);
            }
            catch (System.Exception timeregdataUpdateUserProjectException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update UserProject");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
