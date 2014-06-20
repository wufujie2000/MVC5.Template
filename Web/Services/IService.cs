using System;
using System.Web.Mvc;
using Template.Components.Alerts;

namespace Template.Services
{
    public interface IService : IDisposable
    {
        ModelStateDictionary ModelState { get; set; }
        MessagesContainer AlertMessages { get; set; }
    }
}