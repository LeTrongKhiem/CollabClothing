using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Common
{
    public class ResultApiSuccessed<T> : ResultApi<T>
    {
        public ResultApiSuccessed(T resultObject)
        {
            IsSuccessed = true;
            ResultObject = resultObject;
        }
        public ResultApiSuccessed()
        {
            IsSuccessed = true;
        }
    }
}
