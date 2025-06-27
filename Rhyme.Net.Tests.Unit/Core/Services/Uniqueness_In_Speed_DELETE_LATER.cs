using Ardalis.SharedKernel;
using Rhyme.Net.Core.Domain.MenuAggregate;
using Rhyme.Net.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using System.Diagnostics;
using System.Text;

namespace Rhyme.Net.Tests.Unit.Core.Services;

public class Uniqueness_In_Speed_DELETE_LATER
{

  public Uniqueness_In_Speed_DELETE_LATER()
  {
  }

  [Fact]
  public void BlaBla()
  {
    const int total = 10_000_000;
    HashSet<string> results = new HashSet<string>(total);
    Console.WriteLine($"Generating {total:N0} unique 12-character Base32 strings...");

    Stopwatch sw = Stopwatch.StartNew();

    while (results.Count < total)
    {
      Guid guid = Guid.NewGuid();
      byte[] shortBytes = new byte[8]; // 64 bits
      Array.Copy(guid.ToByteArray(), shortBytes, 8);
      string encoded = Base32Encode(shortBytes).Substring(0, 12);

      results.Add(encoded);
    }

    sw.Stop();
    Console.WriteLine($"✅ Done. Generated {results.Count:N0} strings.");
    Console.WriteLine($"⏱ Time taken: {sw.Elapsed.TotalSeconds:N2} seconds");
  }

  static string Base32Encode(byte[] data)
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