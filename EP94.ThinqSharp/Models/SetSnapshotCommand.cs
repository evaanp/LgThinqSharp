using EP94.ThinqSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Models
{
    internal record SetSnapshotCommand<TSnapshot, T>(Expression<Func<TSnapshot, T>> PropertyLambda, T Value, bool ForceSend)
    {
    }
}
