using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Template.Components.Alerts
{
    public class MessagesContainer : IEnumerable<AlertMessage>
    {
        public const Decimal DefaultFadeOut = 4;
        private ModelStateDictionary modelState;
        private List<AlertMessage> messages;

        public MessagesContainer(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
            messages = new List<AlertMessage>();
        }

        public void Add(AlertMessage message)
        {
            messages.Add(message);
        }
        public void Add(AlertMessageType type, String message)
        {
            Add(type, String.Empty, message, DefaultFadeOut);
        }
        public void Add(AlertMessageType type, String key, String message)
        {
            Add(type, key, message, DefaultFadeOut);
        }
        public void Add(AlertMessageType type, String message, Decimal fadeOutAfter)
        {
            Add(type, String.Empty, message, fadeOutAfter);
        }
        public void Add(AlertMessageType type, String key, String message, Decimal fadeOutAfter)
        {
            Add(new AlertMessage()
            {
                Key = key,
                Message = message,
                FadeOutAfter = fadeOutAfter,
                Type = type
            });
        }

        public void AddError(String message)
        {
            AddError(String.Empty, message, 0); 
        }
        public void AddError(String key, String message)
        {
            AddError(key, message, 0); 
        }
        public void AddError(String key, String message, Decimal fadeOutAfter)
        {
            Add(AlertMessageType.Danger, key, message, fadeOutAfter);
        }

        public IEnumerator<AlertMessage> GetEnumerator()
        {
            if (modelState == null)
                return messages.GetEnumerator();

            var keys = modelState.Keys.GetEnumerator();
            var values = modelState.Values.GetEnumerator();

            var modelStateMessages = new List<AlertMessage>();
            while (keys.MoveNext() && values.MoveNext())
                foreach (var error in values.Current.Errors)
                    modelStateMessages.Add(new AlertMessage()
                    {
                        Key = keys.Current,
                        Message = error.ErrorMessage,
                        Type = AlertMessageType.Danger
                    });

            return modelStateMessages.Union(messages).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
