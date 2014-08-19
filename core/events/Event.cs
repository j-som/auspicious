using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Events
{
    public class Event
    {
        public readonly string Type;

        public IEventDispatcher Target
        {
            get { return target; }
        }

        public IEventDispatcher CurrentTarget
        {
            get { return currentTarget; }
        }

        internal IEventDispatcher currentTarget;
        internal IEventDispatcher target;

        public bool IsBubbles
        {
            get { return _isBubbles; }
        }
        private bool _isBubbles;
        public Event(string type, bool bubbles = false)
        {
            this.Type = type;
            this._isBubbles = bubbles;
            target = null;
            currentTarget = null;
        }
    }
}
