using DiscordBotBase.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace DiscordBotBase.Classes
{
    public sealed class Event
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; } 

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; private set; } 

        /// <summary>
        /// 
        /// </summary>
        public Action Action { get; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Interval { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsActived { get; private set; }

        private readonly Timer _timer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <param name="interval"></param>
        /// <param name="description"></param>
        public Event(string name, Action action, TimeSpan interval, string description = null)
        {
            this.Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("The Name of scheduled event can't be null!") : name;
            this.Action = action ?? throw new ArgumentNullException("The Action can't be null!");
            this.Interval = interval;
            this.Description = description;

            this._timer = new Timer()
            {
                Interval = this.Interval.TotalMilliseconds
            };
            this._timer.Elapsed += (s, e) => this.Action.Invoke();
            this._timer.Start();

            this.IsActived = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Activate()
        {
            this._timer.Start();
            
            if (!this.IsActived)
                this.IsActived = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Deactivate()
        {
            this._timer.Stop();
            
            if (this.IsActived)
                this.IsActived = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newName"></param>
        /// <param name="newInterval"></param>
        /// <param name="newDescription"></param>
        public void ChangeSettings(string newName = null, TimeSpan? newInterval = null, string newDescription = null)
        {
            if (!string.IsNullOrWhiteSpace(newName))
                this.Name = newName;

            if (newInterval.HasValue)
            {
                var newIntervalValue = newInterval.Value;
                this._timer.Interval = newIntervalValue.TotalMilliseconds;
                this.Interval = newIntervalValue;
            }

            if (!string.IsNullOrWhiteSpace(newDescription))
                this.Description = newDescription;
        }
    }
}