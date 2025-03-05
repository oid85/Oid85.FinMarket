using Oid85.FinMarket.Application.Interfaces.Services;

namespace Oid85.FinMarket.Application.Services;

public class NormalizeService : INormalizeService
{
    public string NormalizeInstrumentName(string instrumentName)
    {
        var replaces = new List<Tuple<string, string>>
        {
            new(" - привилегированные акции", " а.п."),
            new(" - акции привилегированные", " а.п."),
            new(" - ао", ""),
            new(" - Акции привилегированные", " а.п."),
            new(" - акции обыкновенные", ""),
            new(" - Привилегированные акции", " а.п."),
            new(" (привилегированные)", " а.п."),
            new("Вторая генерирующая компания оптового рынка электроэнергии", "ОГК-2"),
            new("Объединенная авиастроительная корпорация", "ОАК"),
            new("Магнитогорский металлургический комбинат", "ММК"),
            new("Трубная Металлургическая Компания", "ТМК")
        };
        
        string normalizedInstrumentName = instrumentName;

        foreach (var replace in replaces) 
            normalizedInstrumentName = normalizedInstrumentName.Replace(replace.Item1, replace.Item2);
        
        return normalizedInstrumentName;
    }
}