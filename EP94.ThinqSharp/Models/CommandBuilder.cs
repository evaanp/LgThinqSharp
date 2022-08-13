using EP94.ThinqSharp.Clients;
using EP94.ThinqSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    internal class CommandBuilder<TSnapshot> : ICommandBuilder<TSnapshot>
    {
        private DeviceClient _deviceClient;
        private List<Func<Task>> _tasks;
        private TimeSpan _timeBetweenCalls;
        public CommandBuilder(DeviceClient deviceClient, TimeSpan timeBetweenCalls)
        {
            _deviceClient = deviceClient;
            _timeBetweenCalls = timeBetweenCalls;
            _tasks = new List<Func<Task>>();
        }

        public async Task SendCommandsAsync()
        {
            foreach (var task in _tasks)
            {
                await task();
                if (!task.Equals(_tasks.Last()))
                {
                    await Task.Delay(_timeBetweenCalls);
                }
            }
        }

        public ICommandBuilder<TSnapshot> Value<T>(Expression<Func<TSnapshot, T>> propertyLambda, T value, bool forceSend)
        {
            _tasks.Add(() => _deviceClient.SetSnapshotValue(propertyLambda, value, forceSend));
            return this;
        }
    }
}
