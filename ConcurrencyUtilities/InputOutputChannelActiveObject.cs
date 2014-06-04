using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
    /// <summary>
    ///     Channel-based active object which takes data from it's input channel,
    ///     executes some processing (must be implemented by subclass) and places
    ///     the result on it's output channel.
    /// </summary>
    /// <typeparam name="IT">The type of data on the input channel.</typeparam>
    /// <typeparam name="OT">The type of data on the output channel.</typeparam>
    abstract class InputOutputChannelActiveObject<IT, OT>: ActiveObject
    {
        public Channel<IT> inputChannel = new Channel<IT>();
        public Channel<OT> outputChannel = new Channel<OT>();

        public InputOutputChannelActiveObject() : base() { }

        /// <summary>
        ///     Loops forever and processing the input channel data, placing
        ///     the results on the output channel.
        /// </summary>
        private override void Execute()
        {
            while(true)
            {
                outputChannel.Put(Process(inputChannel.Take()));
            }
        }

        /// <summary>
        ///     Process method must be implemented by subclass.
        /// </summary>
        /// <param name="data">The data unit to process.</param>
        /// <returns>The result of processing the input data unit.</returns>
        protected OT Process(IT data);
    }
}
