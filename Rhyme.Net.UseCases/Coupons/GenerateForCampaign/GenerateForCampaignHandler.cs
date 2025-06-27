using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.CouponAggregate;

namespace Rhyme.Net.UseCases.Coupons.GenerateForCampaign;

public class GenerateForCampaignHandler : ICommandHandler<GenerateForCampaignCommand, Result<bool>>
{
  private readonly IRepository<Coupon> _repository;

  public GenerateForCampaignHandler(IRepository<Coupon> repository)
  {
    _repository = repository;
  }

  public async Task<Result<bool>> Handle(GenerateForCampaignCommand request, CancellationToken cancellationToken)
  {
    try
    {
      Console.WriteLine($"Generating coupons for campaign {request.CampaignId}...");
      await Task.Delay(10); // Simulate some delay for coupon generation

      // var newOrder = new Coupon(string.Empty, request.CampaignId);
      // var createdItem = await _repository.AddAsync(newOrder, cancellationToken);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error generating coupons for campaign {request.CampaignId}: {ex.Message}");
    }

    return Result.Success(true);
  }
}