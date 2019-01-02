using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelTest01.Model
{
    abstract class Person<TResult>
    {
        public abstract void Say(TResult result);
        public TResult Result { get; set; }
    }
}
