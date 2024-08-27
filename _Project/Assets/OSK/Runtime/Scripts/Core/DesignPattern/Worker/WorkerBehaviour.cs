using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
	public class WorkerBehaviour : MonoBehaviour
	{
		#region Inspector Variables

		#endregion

		#region Member Variables

		private Worker					worker;
		private System.Action<Worker>	finishedCallback;

		#endregion

		#region Unity Methods

		private void Update()
		{
			if (worker != null && worker.Stopped)
			{
				finishedCallback(worker);

				worker = null;

				Destroy(gameObject);
			}
		}

		#endregion

		#region Public Methods

		//WorkerBehaviour.StartWorker(boardCreatorWorker, (Worker worker) =>
		//	{
		//		finishedCallback((worker as BoardCreatorWorker).FinishedBoardDada);
		//	});
		public static void StartWorker(Worker worker, System.Action<Worker> finishedCallback)
		{
			new GameObject("worker").AddComponent<WorkerBehaviour>().Run(worker, finishedCallback);
		}

		public void Run(Worker worker, System.Action<Worker> finishedCallback)
		{
			this.worker				= worker;
			this.finishedCallback	= finishedCallback;
			worker.StartWorker();
		}
		#endregion
	}
}
