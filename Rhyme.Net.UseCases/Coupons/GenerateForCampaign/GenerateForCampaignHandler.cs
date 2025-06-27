using System.Diagnostics;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.CouponAggregate;
using Rhyme.Net.Core.Interfaces;

namespace Rhyme.Net.UseCases.Coupons.GenerateForCampaign;

public class GenerateForCampaignHandler : ICommandHandler<GenerateForCampaignCommand, Result<bool>>
{
  private readonly IGenerateCampaignCouponsService _service;

  public GenerateForCampaignHandler(IGenerateCampaignCouponsService service)
  {
    _service = service;
  }

  public async Task<Result<bool>> Handle(GenerateForCampaignCommand command, CancellationToken cancellationToken)
  {
    try
    {
      Stopwatch sw = Stopwatch.StartNew();

      Console.WriteLine($"Generating coupons for campaign {command.CampaignId}...");
      await _service.GenerateAsync(command.CampaignId, command.TotalCouponsCount);

      sw.Stop();
      Console.WriteLine($"⏱ Time taken: {sw.Elapsed.TotalSeconds:N2} seconds.");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error generating coupons for campaign {command.CampaignId}: {ex.Message}");
    }

    return Result.Success(true);
  }
}