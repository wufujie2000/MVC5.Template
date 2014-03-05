using System;
using System.Web;
using System.Web.Mvc;
using Template.Components.Alerts;

namespace Template.Components.Services
{
    public interface IService
    {
        ModelStateDictionary ModelState { get; set; }
        MessagesContainer AlertMessages { get; set; }
        HttpContextBase HttpContext { get; set; }

        String CurrentAccountId { get; }

        String CurrentLanguage { get; }
        String CurrentArea { get; }
        String CurrentController { get; }
        String CurrentAction { get; }
    }
}