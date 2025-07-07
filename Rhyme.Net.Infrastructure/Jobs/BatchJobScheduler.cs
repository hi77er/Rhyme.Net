using Amazon.Batch;
using Amazon.Batch.Model;

public class BatchJobScheduler : IBatchJobScheduler
{
  private readonly IAmazonBatch _batchClient;

  public BatchJobScheduler(IAmazonBatch batchClient)
  {
    _batchClient = batchClient;
  }

  public async Task<string> ScheduleAsync(
      string jobName,
      string jobQueue,
      string jobDefinition,
      IEnumerable<string> args)
  {
    var request = new SubmitJobRequest
    {
      JobName = jobName,
      JobQueue = jobQueue,
      JobDefinition = jobDefinition,
      ContainerOverrides = new ContainerOverrides
      {
        Command = args.ToList()
      }
    };

    var response = await _batchClient.SubmitJobAsync(request);
    return response.JobId;
  }
}