using System.Text.Json.Serialization;

namespace Oid85.FinMarket.External.ResourceStore.Models;

/// <summary>
/// Представление мультипликатора в ресурсах
/// </summary>
public class MultiplicatorUnitResource<T>
{
    /// <summary>
    /// 2020 год
    /// </summary>
    [JsonPropertyName("year2020")]
    public T? Year2020 { get; set; } = default;
    
    /// <summary>
    /// 2021 год
    /// </summary>
    [JsonPropertyName("year2021")]
    public T? Year2021 { get; set; } = default;
    
    /// <summary>
    /// 2022 год
    /// </summary>
    [JsonPropertyName("year2022")]
    public T? Year2022 { get; set; } = default;
    
    /// <summary>
    /// 2023 год
    /// </summary>
    [JsonPropertyName("year2023")]
    public T? Year2023 { get; set; } = default;
    
    /// <summary>
    /// 2024 год
    /// </summary>
    [JsonPropertyName("year2024")]
    public T? Year2024 { get; set; } = default;
    
    /// <summary>
    /// LTM
    /// </summary>
    [JsonPropertyName("ltm")]
    public T? Ltm { get; set; } = default;
    
    /// <summary>
    /// Единица измерения
    /// </summary>
    [JsonPropertyName("unit")]
    public string Unit { get; set; } = string.Empty;
}

