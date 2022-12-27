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
    public partial class UserProjectComponent : ComponentBase
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

        protected RadzenGrid<Timereg.Models.Timeregdata.UserProject> grid0;


        IEnumerable<Timereg.Models.Timeregdata.UserProject> _getUserProjectsResult;
        protected IEnumerable<Timereg.Models.Timeregdata.UserProject> getUserProjectsResult
        {
            get
            {
                return _getUserProjectsResult;
            }
            set
            {
                if (!object.Equals(_getUserProjectsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getUserProjectsResult", NewValue = value, OldValue = _getUserProjectsResult };
                    _getUserProjectsResult = value;
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
            var Uid = await UserProfile.GetUserId(Security, Timeregdata);
            var timeregdataGetUserProjectsResult = await Timeregdata.GetUserProjects();
            getUserProjectsResult = timeregdataGetUserProjectsResult.ToList().FindAll(r => r.userid == Uid && r.Project != null && (r.Project.validto == null || r.Project.validto >= DateTime.Today));
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddUserProject>("Add User Project", null);
            await Load();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Timereg.Models.Timeregdata.UserProject args)
        {
            var dialogResult = await DialogService.OpenAsync<EditUserProject>("Edit User Project", new Dictionary<string, object>() { { "Upid", args.upid } });
            await Load();
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            var result = await JSRuntime.InvokeAsync<bool>("confirm", new[] { "Are you sure you want to delete this?" });
            try
            {
                if (result == true)
                {
                    var timeregdataDeleteUserProjectResult = await Timeregdata.DeleteUserProject(data.upid);
                    if (timeregdataDeleteUserProjectResult != null)
                    {
                        NotificationService.Notify(NotificationSeverity.Success, $"Success", $"Success to delete UserProject");
                        await grid0.Reload();
                        await Load();
                    }
                }
            }

            catch (System.Exception timeregdataDeleteUserProjectException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete UserProject");
            }
        }
    }
}
