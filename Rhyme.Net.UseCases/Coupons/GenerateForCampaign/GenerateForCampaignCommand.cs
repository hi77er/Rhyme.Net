﻿using Ardalis.Result;

namespace Rhyme.Net.UseCases.Coupons.GenerateForCampaign;

/// <summary>
/// Create a batch of Coupons.
/// </summary>
/// <param name="CampaignId"></param>
/// <param name="TotalCouponCount"></param>
public record GenerateForCampaignCommand(bool HousekeepingOn, string CampaignId, int TotalCouponsCount) : Ardalis.SharedKernel.ICommand<Result<bool>>;