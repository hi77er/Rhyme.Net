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

  public async Task<Result<bool>> Handle(GenerateForCampaignCommand command, CancellationToken cancellationToken)
  {
    Stopwatch sw = Stopwatch.StartNew();

    try
    {
      // Console.WriteLine($"Generating coupons for campaign {command.CampaignId} ...");
      // var couponsResult = await _service.GenerateAsync(command.CampaignId, command.TotalCouponsCount);
      // Console.WriteLine($"Generation complete.");

      // var coupons = couponsResult
      //   .Value
      //   .Select(couponId => new Coupon
      //   {
      //     Id = couponId,
      //     CampaignId = command.CampaignId,
      //   })
      //   .AsEnumerable();
      // Console.WriteLine($"Projection complete.");

      // await _repository.WriteBatchAsync(coupons);
      // Console.WriteLine($"BatchWrite complete.");

      // sw.Stop();
      // Console.WriteLine($"⏱ Time taken: {sw.Elapsed.TotalSeconds:N2} seconds.");

      await _repository.FlushTableAsync();
      Console.WriteLine($"Table flushed.");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error generating coupons for campaign {command.CampaignId}: {ex.Message}, {ex.StackTrace}");
    }

    return Result.Success(true);
  }
}