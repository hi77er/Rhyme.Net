using System.Diagnostics;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.CouponAggregate;
using Rhyme.Net.Core.Interfaces;

namespace Rhyme.Net.UseCases.Coupons.GenerateForCampaign;

public class GenerateForCampaignHandler : ICommandHandler<GenerateForCampaignCommand, Result<bool>>
{
  private readonly IDynamoRepository<Coupon, string> _repository;
  private readonly IGenerateCampaignCouponsService _service;

  public GenerateForCampaignHandler(IDynamoRepository<Coupon, string> repository, IGenerateCampaignCouponsService service)
  {
    _repository = repository;
    _service = service;
  }

  // CURL command example to triger Job via the post command lambda:
  // curl -X POST https://gbtwi2old3.execute-api.eu-central-1.amazonaws.com/dev/api/coupons \
  //  -H "Content-Type: application/json" \
  //  -d '{
  //        "HousekeepingOn": true,
  //        "CampaignId": "TEST_CAMPAIGN_ID_1",
  //        "TotalCouponsCount":3000000
  //      }'
  public async Task<Result<bool>> Handle(GenerateForCampaignCommand command, CancellationToken cancellationToken)
  {
    Stopwatch sw = Stopwatch.StartNew();

    try
    {
      if (command.HousekeepingOn)
      {
        Console.WriteLine($"Housekeeping mode enabled. Flushing table {nameof(Coupon)} ...");
        await _repository.FlushTableAsync();
        Console.WriteLine($"Table flushed!");
      }
      else
      {
        Console.WriteLine($"Generating coupons for campaign {command.CampaignId} ...");
        var couponsResult = await _service.GenerateAsync(command.CampaignId, command.TotalCouponsCount);
        Console.WriteLine($"Generation complete for {couponsResult.Value.Count} randomly generated coupon IDs.");

        long memoryUsed = GC.GetTotalMemory(forceFullCollection: false);
        Console.WriteLine($"Memory used [Step 1]: {memoryUsed / 1048576:N0} MBs");

        var coupons = couponsResult
          .Value
          .Select(couponId => new Coupon
          {
            Id = couponId,
            CampaignId = command.CampaignId,
          })
          .AsEnumerable();
        Console.WriteLine($"Projection complete for {coupons.Count():N0} coupons to write to DynamoDB.");

        long memoryUsed2 = GC.GetTotalMemory(forceFullCollection: false);
        Console.WriteLine($"Memory used [Step 2]: {memoryUsed2 / 1048576:N0} MBs");

        await _repository.GentleWriteBatchAsync(coupons);
        Console.WriteLine($"BatchWrite complete.");

        sw.Stop();
        Console.WriteLine($"⏱ Time taken: {sw.Elapsed.TotalSeconds:N2} seconds.");
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error generating coupons for campaign {command.CampaignId}: {ex.Message}, {ex.StackTrace}");
    }

    return Result.Success(true);
  }
}