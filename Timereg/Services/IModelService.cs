using Timereg.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timereg.Services
{
    public interface IModelService
    {
        event Action<ModelResult> OnClose;

        void Show<T>(string title, ModelParameters parameters) where T : ComponentBase;
        void Show<T>(string title, ModelParameters parameters=null,ModelOption option=null ) where T : ComponentBase;
        void Show(Type contentComponent ,string title, ModelParameters parameters,ModelOption option) ;
        void Close(ModelResult modelResult);
        void Cancel();
    }
}
