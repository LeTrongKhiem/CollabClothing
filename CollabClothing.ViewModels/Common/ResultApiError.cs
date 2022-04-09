using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.ViewModels.Common
{
    public class ResultApiError<T> : ResultApi<T>
    {
        public string[] ValidationErrors { get; set; }
        public ResultApiError()
        {

        }

        public ResultApiError(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
        public ResultApiError(string[] validationErrors)
        {
            IsSuccessed = false;
            ValidationErrors = validationErrors;
        }

    }
}
