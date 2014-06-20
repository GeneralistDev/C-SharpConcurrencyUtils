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
	/// 
	/// 	Author: Daniel Parker 971328X
    /// </summary>
    /// <typeparam name="IT">The type of data on the input channel.</typeparam>
    /// <typeparam name="OT">The type of data on the output channel.</typeparam>
    public abstract class InputOutputChannelActiveObject<IT, OT>: ActiveObject
    {
        /// <summary>
        ///     The input channel for this active object. Data will
        ///     be taken off this channel and processed by the active object.
        /// </summary>
        public Channel<IT> inputChannel = new Channel<IT>();

        /// <summary>
        ///     The output channel for this active object. Results of processing
        ///     will be placed on this channel.
        /// </summary>
        public Channel<OT> outputChannel = new Channel<OT>();

        /// <summary>
        ///     Public construtor.
        /// </summary>
        public InputOutputChannelActiveObject() : base() { }

        /// <summary>
        ///     Loops forever and processing the input channel data, placing
        ///     the results on the output channel.
        /// </summary>
        protected override void Execute()
        {
			Boolean stopRequested;
			lock(lockObject)
			{
				stopRequested = stop;
			}

			while (!stopRequested)
			{
				outputChannel.Put(Process(inputChannel.Take()));
				lock(lockObject)
				{
					stopRequested = stop;
				}
			}
        }

        /// <summary>
        ///     Process method must be implemented by subclass.
        /// </summary>
        /// <param name="data">The data unit to process.</param>
        /// <returns>The result of processing the input data unit.</returns>
        protected abstract OT Process(IT data);
    }
}
