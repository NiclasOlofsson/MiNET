using System.Threading.Tasks;

namespace MiNET.Events
{
	/// <summary>
	/// 	The base class all OpenApi Events implement
	/// </summary>
	public abstract class Event : ICancellable
	{
		public bool IsCancelled { get; set; }

		public Event()
		{
			IsCancelled = false;
		}

		public void SetCancelled(bool isCanceled)
		{
			IsCancelled = isCanceled;
		}

        public Task Executed { get; } = Task.CompletedTask;

	    /*private Task _firstCompleteTask = null;
	    private Task _lastCompleteTask = null;
        public void ContinueWith(Task action)
	    {
	        if (_firstCompleteTask == null)
	        {
	            _firstCompleteTask = action;
	        }

	        if (_lastCompleteTask != null)
	        {
	            _lastCompleteTask.ContinueWith(task => action);
	        }

	        _lastCompleteTask = action;
	    }*/

	    internal void OnComplete()
	    {
            if (!Executed.IsCompleted)
	            Executed.Start();
        }
    }
}
