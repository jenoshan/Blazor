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
    public partial class EditTimeusedComponent : ComponentBase
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
        public dynamic tid { get; set; }

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
            var timeregdataGetTimeusedBytidResult = await Timeregdata.GetTimeusedBytid(tid);
            timeused = timeregdataGetTimeusedBytidResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(Timereg.Models.Timeregdata.Timeused args)
        {
            try
            {
                var timeregdataUpdateTimeusedResult = await Timeregdata.UpdateTimeused(tid, args);
                DialogService.Close(timeused);
            }
            catch (System.Exception timeregdataUpdateTimeusedException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update Timeused");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            try
            {
                var timeregdataDeleteTimeusedResult = await Timeregdata.DeleteTimeused(timeused.tid);
                DialogService.Close(timeused);
            }
            catch (System.Exception timeregdataDeleteTimeusedException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Timeused");
            }
        }

        protected async System.Threading.Tasks.Task Button3Click(MouseEventArgs args)
        {
            DialogService.Close(timeused);
        }
    }
}
