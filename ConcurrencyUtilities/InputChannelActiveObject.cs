using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
    /// <summary>
    ///     Channel-based active object that takes data from its
    ///     input channel and processes it.
    /// </summary>
    /// <typeparam name="IT">The type of data on the input channel.</typeparam>
    abstract class InputChannelActiveObject<IT>: ActiveObject
    {
        public readonly Channel<IT> inputChannel = new Channel<IT>();

        public InputChannelActiveObject() : base() { }

        /// <summary>
        ///     Loops forever taking data off the input channel calling the 'Process' method.
        /// </summary>
        protected override void Execute()
        {
            while (true)
            {
                Process(inputChannel.Take());
            }
        }

        /// <summary>
        ///     Process a given unit of data. Must be implemented by subclass.
        /// </summary>
        /// <param name="data">The data unit to process.</param>
        protected abstract void Process(IT data);
    }
}
