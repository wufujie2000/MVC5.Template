using System;
using System.Collections.Generic;

namespace Template.Components.Alerts
{
    public interface IMessagesContainer : IEnumerable<AlertMessage>
    {
        void Add(AlertMessage message);
        void Add(AlertMessageType type, String message);
        void Add(AlertMessageType type, String key, String message);
        void Add(AlertMessageType type, String message, Decimal fadeOutAfter);
        void Add(AlertMessageType type, String key, String message, Decimal fadeOutAfter);

        void AddError(String message);
        void AddError(String key, String message);
        void AddError(String key, String message, Decimal fadeOutAfter);
    }
}
