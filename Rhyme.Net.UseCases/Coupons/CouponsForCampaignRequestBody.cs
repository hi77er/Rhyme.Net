namespace Rhyme.Net.UseCases.Coupons;

public record CouponsForCampaignRequestBody(bool HousekeepingOn, string CampaignId, int TotalCouponsCount);
