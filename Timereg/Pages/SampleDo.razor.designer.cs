using Radzen;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timereg.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.JSInterop;
using Radzen.Blazor;

namespace Timereg.Pages
{
    public partial class SampleComponent : ComponentBase
    {

        private readonly TimeregdataContext context;
        private readonly NavigationManager navigationManager;
        private bool _Moris_init =false;

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

        public Dictionary<string, decimal> AssignmentCosts { get; set; }

        protected RadzenGrid<Timereg.Models.Timeregdata.VTimeusedPeruser> grid0;
        //protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        //{
        //    var dialogResult = await DialogService.OpenAsync<AddProject>("Add Project", null);
        //    grid0.Reload();

        //    await InvokeAsync(() => { StateHasChanged(); });
        //}
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                //title = "Time used by " + Security.User.Name;
                //AssignmentCosts = new Dictionary<string, decimal>();
                //var getPerWork = GetVAssignmentCosts().Result.ToList();
                //foreach (var item in getPerWork)
                //{
                //    AssignmentCosts.Add(item.username, item.Totalminuts);
                //}
                await Load();
            }
            //await Load();

        }

        //protected async Task Load()
        //{
        //    if (!_Moris_init)
        //    {
        //        _Moris_init = true;

        //        await JSRuntime.InvokeVoidAsync("DashboardVM.createMorisLineChart", AssignmentCosts);
        //    }
        //    else
        //        await UpdateDashboard();
        //}
        protected async System.Threading.Tasks.Task Load()
        {
            var timeregdataGetProjectsResult = await  Timeregdata.GetVTimeusedPeruser();
            getProjectsResult = timeregdataGetProjectsResult;
        }
        private async Task UpdateDashboard()
        {
            await JSRuntime.InvokeVoidAsync("DashboardVM.doughnutChartRefreshData", AssignmentCosts);
        }
      
        IEnumerable<Timereg.Models.Timeregdata.VTimeusedPeruser> _getProjectsResult;
        protected IEnumerable<Timereg.Models.Timeregdata.VTimeusedPeruser> getProjectsResult
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

       
    }
}
