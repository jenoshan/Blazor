using Timereg.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timereg.Services
{
    public class ModelService : IModelService
    {
        public event Action<ModelResult> OnClose;
        internal event Action CloseModel;
        internal event Action<string, RenderFragment, ModelParameters, ModelOption> OnShow;
        private Type _modelType;

        public void Cancel()
        {
            CloseModel?.Invoke();
            OnClose?.Invoke(ModelResult.Cancel(_modelType));
        }

        public void Close(ModelResult modelResult)
        {
            modelResult.ModelType = _modelType;
            CloseModel?.Invoke();
            OnClose?.Invoke(modelResult);
        }

        public void Show<T>(string title, ModelParameters parameters) where T : ComponentBase
        {
            Show<T>(title, parameters, new ModelOption());
        }

        public void Show<T>(string title, ModelParameters parameters = null, ModelOption option = null) where T : ComponentBase
        {
            Show(typeof(T), title, parameters, option);
        }

        public void Show(Type contentComponent, string title, ModelParameters parameters, ModelOption option)
        {
            if (!typeof(ComponentBase).IsAssignableFrom(contentComponent))
            {
                throw new ArgumentException("Must be a Blazor componet ");
            }
            var content = new RenderFragment(x => { x.OpenComponent(1, contentComponent); x.CloseComponent(); });
            _modelType = contentComponent;
            OnShow?.Invoke(title, content, parameters, option);
        }
    }
}
