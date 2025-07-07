public interface IBatchJobScheduler
{
  /// <summary>
  /// Schedules a job in AWS Batch.
  /// </summary>
  /// <param name="jobName">The name of the job.</param>
  /// <param name="jobQueue">The job queue to submit the job to.</param>
  /// <param name="jobDefinition">The job definition to use for the job.</param>
  /// <param name="args">Arguments to pass to the job.</param>
  /// <returns>The ID of the scheduled job.</returns>
  Task<string> ScheduleAsync(string jobName, string jobQueue, string jobDefinition, IEnumerable<string> args);
}