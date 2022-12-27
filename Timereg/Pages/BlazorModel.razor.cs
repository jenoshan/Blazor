using BlazorApp3.Data;
using BlazorApp3.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp3.Pages
{
    public partial class BlazorModel :IDisposable
    {
        const string _defauldStyle = "blazored-modal";
        const string _defauldPosition = "blazored-modal-center";

        [Inject] private IModelService ModelService { get; set; }

        [Parameter] public bool HideHeader { get; set; }
        [Parameter] public bool HideCloseButton { get; set; }
        [Parameter] public bool DisableBackgrounCancel { get; set; }
        [Parameter] public string Position { get; set; }
        [Parameter] public string Style { get; set; }

        private bool ComponentDisableBackgrountCancel { get; set; }
        private bool ComponentHideHeader { get; set; }
        private bool ComponentHideCloseButton { get; set; }
        private string ComponentPosition { get; set; }
        private string ComponentStyle { get; set; }
        private bool IsVisible { get; set; }
        private string Title { get; set; }
        private RenderFragment Content { get; set; }
        private ModelParameters Parameters { get; set; }


        protected override void OnInitialized()
        {
            ((ModelService)ModelService).OnShow -= ShowModel;
            ((ModelService)ModelService).CloseModel -= CloseModel;
        }
       
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async void ShowModel(string title, RenderFragment content,ModelParameters parameters , ModelOption options)
        {
            Title = title;
            Content = content;
            Parameters = parameters;
            IsVisible = true;
            await InvokeAsync(StateHasChanged);
        }
        private async void CloseModel()
        {
            Title = "";
            Content = null;
            ComponentStyle = "";
            IsVisible = false;
            await InvokeAsync(StateHasChanged);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ((ModelService)ModelService).OnShow -= ShowModel;
                ((ModelService)ModelService).CloseModel -= CloseModel;
            }
        }
        private void HandleBackgroundClick()
        {
            if (ComponentDisableBackgrountCancel) return;
            ModelService.Cancel();
        }
        private void SetModelOptions(ModelOption option)
        {
            ComponentHideHeader = HideHeader;
            if (option.HidenHeader.HasValue)
                ComponentHideHeader = option.HidenHeader.Value; 
            
            ComponentHideCloseButton = HideCloseButton;
            if (option.HideClouseButton.HasValue)
                ComponentHideCloseButton = option.HideClouseButton.Value; 
            
            ComponentDisableBackgrountCancel = DisableBackgrounCancel;
            if (option.DisableBackgrountCancel.HasValue)
                ComponentDisableBackgrountCancel = option.DisableBackgrountCancel.Value;
            
            ComponentPosition = string.IsNullOrWhiteSpace(option.Position)?Position: option.Position;
            if (string.IsNullOrWhiteSpace(ComponentPosition))
                ComponentPosition = _defauldPosition;

            ComponentStyle = string.IsNullOrWhiteSpace(option.Style) ? Style : option.Style;
            if (string.IsNullOrWhiteSpace(ComponentStyle))
                ComponentStyle = _defauldStyle;
        }

        public  void SetTitle (string title)
        {
            Title = title;
            StateHasChanged();
        }
    }
}
