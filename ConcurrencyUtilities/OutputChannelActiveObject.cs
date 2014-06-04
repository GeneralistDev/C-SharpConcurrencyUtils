using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
    /// <summary>
    ///     Channel-based active object that runs some process and places the data on
    ///     its output channel.
    /// </summary>
    /// <typeparam name="OT">The type of data on the output channel.</typeparam>
    abstract class OutputChannelActiveObject<OT>: ActiveObject
    {
        public readonly Channel<OT> outputChannel = new Channel<OT>();

        public OutputChannelActiveObject() : base() { }

        /// <summary>
        ///     Loops forever putting the result of the 'Process' method on 
        ///     this active object's output channel.
        /// </summary>
        protected override void Execute()
        {
            while (true)
            {
                outputChannel.Put(Process());
            }
        }

        /// <summary>
        ///     Run a process and return the result of the process.
        ///     Must be implemented by subclass.
        /// </summary>
        /// <returns></returns>
        protected abstract OT Process();
    }
}
