using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyUtils
{
    /// <summary>
    ///     Active (threaded) object class. Subclass and override 'Execute()' to use.
	/// 
	/// 	Author: Daniel Parker 971328X
    /// </summary>
    public abstract class ActiveObject
    {
		protected Boolean stop = false;
		private System.Threading.Thread activeThread;
		protected readonly Object lockObject = new object();

        /// <summary>
        ///     Public constructor that creates a Thread 
        ///     object with the 'Execute' method as parameter.
        /// </summary>
        public ActiveObject()
        {
            activeThread = new System.Threading.Thread(Execute);
        }

        /// <summary>
        ///     Use this method to start the active object's processing.
        /// </summary>
        public void Start()
        {
            activeThread.Start();
        }

        /// <summary>
        ///     Execute active object logic. Subclass and override this method.
        /// </summary>
        protected abstract void Execute();

        /// <summary>
        ///     Stop the active object by interrupting it.
        /// </summary>
        public void Stop()
        {
			lock(lockObject)
			{
				stop = true;
			}
			activeThread.Interrupt();
        }
    }
}
