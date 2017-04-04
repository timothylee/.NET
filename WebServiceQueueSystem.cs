public static void WaitForQueue(Guid jobId)
{
	try
	{
		using (var queueClient = new WebSvcQueueSystem.QueueSystemSoapClient())
		{
			WebSvcQueueSystem.JobState jobState;
			const int QUEUE_WAIT_TIME = 2; // two seconds
			bool jobDone = false;
			string xmlError = string.Empty;
			int wait = 0;

			// Wait for the project to get through the queue.
			// Get the estimated wait time in seconds.
			wait = queueClient.GetJobWaitTime(jobId);

			// Wait for it.
			Thread.Sleep(wait * 1000);
			// Wait until it is finished.

			do
			{
				// Get the job state.
				jobState = queueClient.GetJobCompletionState(jobId, out xmlError);

				if (jobState == WebSvcQueueSystem.JobState.Success)
					jobDone = true;
				else
				{
					if (jobState == WebSvcQueueSystem.JobState.Unknown)
					{
						jobDone = true;
						Console.WriteLine("Project was already checked in, operation aborted");
					}
					else if (jobState == WebSvcQueueSystem.JobState.Failed
							 || jobState == WebSvcQueueSystem.JobState.FailedNotBlocking
							 || jobState == WebSvcQueueSystem.JobState.CorrelationBlocked
							 || jobState == WebSvcQueueSystem.JobState.Canceled)
					{
						// If the job failed, error out.
						throw (new ApplicationException("Queue request failed \"" + jobState + "\" Job ID: " + jobId +
														".\r\n" + xmlError));
					}
					else
					{
						Console.WriteLine("\tJob State: " + jobState + " / Job ID: " + jobId);
						Thread.Sleep(QUEUE_WAIT_TIME * 1000);
					}
				}
			} while (!jobDone);
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine("WaitForQueue:Error" + ex.Message);
		log.Error("WaitForQueue:Error" + ex.Message);
		throw ex;
	}
}