using MvcTemplate.Components.Alerts;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Services
{
    public interface IService : IDisposable
    {
        ModelStateDictionary ModelState { get; set; }
        MessagesContainer AlertMessages { get; set; }
    }
}