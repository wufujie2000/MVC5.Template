using System;
using System.Collections;
using System.Collections.Generic;

namespace Template.Components.Alerts
{
    public class MessagesContainer : IEnumerable<AlertMessage>
    {
        public const Decimal DefaultFadeOut = 4;
        private List<AlertMessage> messages;

        public MessagesContainer()
        {
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

        public void Merge(MessagesContainer container)
        {
            if (container == this)
                throw new Exception("Messages container can not be merged to itself");

            foreach (AlertMessage message in container)
                Add(message);
        }

        public IEnumerator<AlertMessage> GetEnumerator()
        {
            return messages.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
