using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CollabClothing.Application.Common
{
    public class UtilitiesHelp : IUtilities
    {
        public UtilitiesHelp()
        {

        }
        public string SEOUrl(string url)
        {
            url = url.ToLower();
            url = Regex.Replace(url, @"[áàạảãâấầậẩẫăắằặẳẵ]", "a");
            url = Regex.Replace(url, @"[éèẹẻẽêếềệểễ]", "e");
            url = Regex.Replace(url, @"[óòọỏõôốồộổỗơớờợởỡ]", "o");
            url = Regex.Replace(url, @"[íìịỉĩ]", "i");
            url = Regex.Replace(url, @"[ýỳỵỉỹ]", "y");
            url = Regex.Replace(url, @"[úùụủũưứừựửữ]", "u");
            url = Regex.Replace(url, @"[đ]", "d");

            //2. Chỉ cho phép nhận:[0-9a-z-\s]
            url = Regex.Replace(url.Trim(), @"[^0-9a-z-\s]", "").Trim();
            //xử lý nhiều hơn 1 khoảng trắng --> 1 kt
            url = Regex.Replace(url.Trim(), @"\s+", "-");
            //thay khoảng trắng bằng -
            url = Regex.Replace(url, @"\s", "-");
            while (true)
            {
                if (url.IndexOf("--") != -1)
                {
                    url = url.Replace("--", "-");
                }
                else
                {
                    break;
                }
            }
            return url;
        }
    }
}
