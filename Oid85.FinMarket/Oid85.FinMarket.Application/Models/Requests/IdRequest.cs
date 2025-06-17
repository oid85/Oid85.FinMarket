using System.Text.Json.Serialization;

namespace Oid85.FinMarket.Application.Models.Requests;

public class IdRequest
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}