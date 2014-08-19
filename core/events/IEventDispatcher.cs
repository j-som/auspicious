using System;
namespace Core.Events
{
    public interface IEventDispatcher
    {
        void AddEventListener(string type, EventListener listener, int priority = 0);
        bool DispatchEvent(Event evt);
        bool HasEventListener(string type);
        void RemoveEventListener(string type, EventListener handler);
    }
}
