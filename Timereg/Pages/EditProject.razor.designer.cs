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
    public partial class EditProjectComponent : ComponentBase
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
        public dynamic id { get; set; }

        Timereg.Models.Timeregdata.Project _project;
        protected Timereg.Models.Timeregdata.Project project
        {
            get
            {
                return _project;
            }
            set
            {
                if (!object.Equals(_project, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "project", NewValue = value, OldValue = _project };
                    _project = value;
                    OnPropertyChanged(args);
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
                    var args = new PropertyChangedEventArgs() { Name = "getProjectsResult", NewValue = value, OldValue = _getCompany };
                    _getCompany = value;
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
            var getcompany = await Timeregdata.GetCompany();
            getCompany = getcompany;

            var timeregdataGetProjectByidResult = await Timeregdata.GetProjectByid(id);
            project = timeregdataGetProjectByidResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Timereg.Models.Timeregdata.Project args)
        {
            try
            {
                var timeregdataUpdateProjectResult = await Timeregdata.UpdateProject(id, project);
                NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Success to update Project");
                DialogService.Close(project);
            }
            catch (System.Exception timeregdataUpdateProjectException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update Project");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
