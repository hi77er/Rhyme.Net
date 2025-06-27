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

  public async Task<Result<bool>> Handle(GenerateForCampaignCommand request, CancellationToken cancellationToken)
  {
    try
    {
      Stopwatch sw = Stopwatch.StartNew();

      Console.WriteLine($"Generating coupons for campaign {request.CampaignId}...");
      string campaignId = $"CAMPAIGN-{Guid.NewGuid()}";
      const int total = 10_000_000;

      await _service.GenerateAsync(campaignId, total);

      sw.Stop();
      Console.WriteLine($"⏱ Time taken: {sw.Elapsed.TotalSeconds:N2} seconds.");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error generating coupons for campaign {request.CampaignId}: {ex.Message}");
    }

    return Result.Success(true);
  }
}