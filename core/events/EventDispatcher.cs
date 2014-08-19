using System;
using System.Collections.Generic;
namespace Core.Events
{
    public delegate void EventListener(Event evt);

    public class EventDispatcher:IEventDispatcher
    {
        private Dictionary<string, List<Listener>> listener_dictionary;
        public EventDispatcher()
        {
        }
        /// <summary>
        /// 添加一个事件侦听器, 每一个类型的事件只能添加同一个侦听器1次，
        /// 重复添加会根据优先级重新设置执行的顺序。
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="listener">侦听函数代理</param>
        /// <param name="priority">优先级，越大的越先执行，优先级一样的，按代码顺序</param>
        public void AddEventListener(string type, EventListener listener, int priority = 0)
        {
            if (listener_dictionary == null) listener_dictionary = new Dictionary<string, List<Listener>>();

            List<Listener> listeners;
            if (listener_dictionary.ContainsKey(type))
            {
                listener_dictionary.TryGetValue(type, out listeners);
                Listener item = null;
                for (int i = listeners.Count - 1; i >= 0; --i)
                {
                    if (listeners[i].listener == listener)
                    {
                        item = listeners[i];
                        listeners.RemoveAt(i);
                    }
                }
                //查找该优先级在列表中的插入位置
                int index = 0;
                for (int i = listeners.Count - 1; i >= 0; --i)
                {
                    if (listeners[i].priority >= priority)
                    {
                        index = i + 1;
                        break;
                    }
                }
                if (item == null)
                {
                    item = new Listener(listener, priority);
                }
                else
                {
                    item.priority = priority;
                }
                listeners.Insert(index, item);
            }
            else
            {
                listeners = new List<Listener>();
                listeners.Add(new Listener(listener, priority));
                listener_dictionary.Add(type, listeners);
            }
            
        }
        /// <summary>
        /// 派发一个事件
        /// </summary>
        /// <param name="evt">该事件实例</param>
        /// <returns>返回派发是否成功，若返回false则表示没有该事件对应的侦听器</returns>
        public bool DispatchEvent(Event evt)
        {
            if (evt == null)
            {
                throw (new Exception("event is null"));
            }else
            {
                if (HasEventListener(evt.Type))
                {
                    if (evt.target == null) evt.target = this;
                    evt.currentTarget = this;
                    List<Listener> list;
                    listener_dictionary.TryGetValue(evt.Type, out list);
                    int count = list.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        list[i].listener.Invoke(evt);
                    }
                    return true;
                }else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 是否有某类型事件的侦听器
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <returns></returns>
        public bool HasEventListener(string type)
        {
            return listener_dictionary != null && listener_dictionary.ContainsKey(type);
        }

        public void RemoveEventListener(string type, EventListener listener)
        {
            if (HasEventListener(type))
            {
                List<Listener> list;
                listener_dictionary.TryGetValue(type, out list);
                for (int i = list.Count - 1; i >= 0; --i)
                {
                    if (list[i].listener == listener)
                    {
                        list.RemoveAt(i);
                    }
                }
                if (list.Count == 0)
                {
                    listener_dictionary.Remove(type);
                }
            }
        }
    }



    class Listener
    {
        public readonly EventListener listener;
        public  int priority;
        public Listener(EventListener listener, int priority)
        {
            this.listener = listener;
            this.priority = priority;
        }
    }
}
