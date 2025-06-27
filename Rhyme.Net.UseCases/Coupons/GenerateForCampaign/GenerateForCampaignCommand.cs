using Ardalis.Result;

namespace Rhyme.Net.UseCases.Coupons.GenerateForCampaign;

/// <summary>
/// Create a batch of Coupons.
/// </summary>
/// <param name="CampaignId"></param>
public record GenerateForCampaignCommand(string CampaignId) : Ardalis.SharedKernel.ICommand<Result<bool>>;