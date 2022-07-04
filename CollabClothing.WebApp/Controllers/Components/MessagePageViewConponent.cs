﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollabClothing.WebApp.Controllers.Components
{
    public class MessagePageViewConponent : ViewComponent
    {
        public const string COMPONENTNAME = "MessagePage";
        // Dữ liệu nội dung trang thông báo
        public class Message
        {
            public string title { set; get; } = "Thông báo";     // Tiêu đề của Box hiện thị
            public string htmlcontent { set; get; } = "";         // Nội dung HTML hiện thị
            public string urlredirect { set; get; } = "/";        // Url chuyển hướng đến
            public int secondwait { set; get; } = 3;              // Sau secondwait giây thì chuyển
        }
        public MessagePageViewConponent() { }
        public IViewComponentResult Invoke(Message message)
        {
            // Thiết lập Header của HTTP Respone - chuyển hướng về trang đích
            this.HttpContext.Response.Headers.Add("REFRESH", $"{message.secondwait};URL={message.urlredirect}");
            return View(message);
        }
    }
}
