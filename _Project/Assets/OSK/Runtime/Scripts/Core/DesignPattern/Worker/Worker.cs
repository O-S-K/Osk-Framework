using System.Collections;
using System.Collections.Generic;

namespace OSK
{
	public abstract class Worker
	{
		#region Member Variables

		private readonly object stopLock		= new object();
		private readonly object	progressLock	= new object();

		private bool	stopping	= false;
		private bool	stopped		= false;
		private float	progress	= 0f;

		#endregion

		#region Properties

		public bool Stopping
		{
			get
			{
				lock (stopLock)
				{
					return stopping;
				}
			}
		}

		public bool Stopped
		{
			get
			{
				lock (stopLock)
				{
					return stopped;
				}
			}
		}

		public float Progress
		{
			get
			{
				lock (progressLock)
				{
					return progress;
				}
			}

			protected set
			{
				lock (progressLock)
				{
					progress = value;
				}
			}
		}

		public System.Action OnStopped;

		#endregion

		#region Abstract Methods

		protected abstract void DoWork();
		protected abstract void Begin();

		#endregion

		#region Public Methods

		public virtual void StartWorker()
		{
			new System.Threading.Thread(new System.Threading.ThreadStart(Run)).Start();
		}

		public void Stop()
		{
			lock (stopLock)
			{
				stopping = true;
			}
		}

		public void Run()
		{
			try
			{
				Begin();

				while (!Stopping)
				{
					DoWork();
				}
			}
			finally
			{
				SetStopped();
			}
		}

		#endregion

		#region Protected Methods

		protected virtual void SetStopped()
		{
			lock (stopLock)
			{
				stopped = true;
			}

			if (OnStopped != null)
			{
				OnStopped();
			}
		}

		#endregion
	}
}
