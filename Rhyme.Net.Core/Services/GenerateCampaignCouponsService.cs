using Ardalis.Result;
using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;
using Rhyme.Net.Core.Domain.MenuAggregate.Events;
using Rhyme.Net.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Ardalis.GuardClauses;
using Rhyme.Net.Core.Domain.CouponAggregate;
using System.Diagnostics;
using System.Text;

namespace Rhyme.Net.Core.Services;

/// <summary>
/// This is here mainly so there's an example of a domain service
/// and also to demonstrate how to fire domain events from a service.
/// </summary>
// /// <param name="_repository"></param>
// /// <param name="_mediator"></param>
/// <param name="_logger"></param>
public class GenerateCampaignCouponsService() : IGenerateCampaignCouponsService
// IDynamoRepository<Coupon, string> _repository,
// IMediator _mediator,
// ILogger<GenerateCampaignCouponsService> _logger
{
  // NOTE: 
  // This is a Domain Service.
  // Domain services are used to encapsulate domain logic that doesn't naturally fit within an entity or value object.
  // This often involves logic that spans multiple aggregates or requires external dependencies. 

  public async Task<Result> GenerateAsync(string campaignId, int totalCouponsCount)
  {
    Guard.Against.Default(campaignId, nameof(campaignId));
    Console.WriteLine("Starting Coupon generation for campaign: {campaignId}", campaignId);

    HashSet<string> results = new HashSet<string>(totalCouponsCount);
    Console.WriteLine($"Generating {totalCouponsCount:N0} unique 12-character Base32 strings...");

    while (results.Count < totalCouponsCount)
    {
      Guid guid = Guid.NewGuid();
      byte[] shortBytes = new byte[8]; // 64 bits
      Array.Copy(guid.ToByteArray(), shortBytes, 8);
      string encoded = Base32Encode(shortBytes).Substring(0, 12);

      results.Add(encoded);
    }

    Console.WriteLine($"✅ Done. Generated {results.Count:N0} unique coupon IDs.");

    // var domainEvent = new MenuDeletedEvent(aggregateToDelete);
    // await _mediator.Publish(domainEvent);

    await Task.Delay(1);
    return Result.Success();
  }


  private string Base32Encode(byte[] data)
  {
    const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
    StringBuilder result = new StringBuilder();
    int buffer = data[0];
    int next = 1;
    int bitsLeft = 8;

    while (result.Length < 13 && (bitsLeft > 0 || next < data.Length))
    {
      if (bitsLeft < 5)
      {
        if (next < data.Length)
        {
          buffer <<= 8;
          buffer |= data[next++] & 0xFF;
          bitsLeft += 8;
        }
        else
        {
          buffer <<= 5 - bitsLeft;
          bitsLeft = 5;
        }
      }

      int index = (buffer >> (bitsLeft - 5)) & 0x1F;
      bitsLeft -= 5;
      result.Append(alphabet[index]);
    }

    return result.ToString();
  }
}
