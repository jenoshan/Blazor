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
    public partial class ProjectComponent : ComponentBase
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

        protected RadzenGrid<Timereg.Models.Timeregdata.Project> grid0;

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
                    var args = new PropertyChangedEventArgs() { Name = "getProjectsResult", NewValue = value, OldValue = _getProjectsResult };
                    _getProjectsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            //if (!Security.IsAuthenticated())
            //{
            //    UriHelper.NavigateTo("Login", true);
            //}
            //else
            //{
            //    await Load();
            //}
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            var timeregdataGetProjectsResult = await Timeregdata.GetProjects();
            getProjectsResult = timeregdataGetProjectsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddProject>("Add Project", null);
            grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Timereg.Models.Timeregdata.Project args)
        {
            var dialogResult = await DialogService.OpenAsync<EditProject>("Edit Project", new Dictionary<string, object>() { { "id", args.id } });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            var result = await JSRuntime.InvokeAsync<bool>("confirm", new[] { "Are you sure you want to delete this?" });
            try
            {
                if (result == true)
                {
                    var timeregdataDeleteProjectResult = await Timeregdata.DeleteProject(data.id);
                    if (timeregdataDeleteProjectResult != null)
                    {
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Success to delete Project");
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception timeregdataDeleteProjectException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Project");
            }
        }
    }
}
