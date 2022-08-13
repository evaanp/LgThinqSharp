using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Interfaces
{
    public interface ICommandBuilder<TSnapshot>
    {
        public ICommandBuilder<TSnapshot> Value<T>(Expression<Func<TSnapshot, T>> propertyLambda, T value, bool forceSend = false);
        public Task SendCommandsAsync();
    }
}
