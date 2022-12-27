using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timereg.Services
{
    public class ModelResult
    {
        public object Data { get;  }
        public Type DataType { get;  }
        public bool Canceled { get; }
        public Type ModelType { get; set; }

        protected ModelResult(object data,Type resultType, bool canceled, Type modelType)
        {
            Data = data;
            DataType = resultType;
            Canceled = canceled;
            ModelType = modelType;
        }
        public static ModelResult Ok<T>(T result) => Ok(result, default);  
        public static ModelResult Ok<T>(T result,Type modelType) => new ModelResult(result,typeof(T),false,modelType);
        public static ModelResult Cancel() => Cancel( default);
        public static ModelResult Cancel(Type modelType) => new ModelResult(default, typeof(object), true , modelType);

    }
}
