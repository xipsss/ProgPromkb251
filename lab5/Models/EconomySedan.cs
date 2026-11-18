using lab5.Models.Abstractions;

namespace lab5.Models;

/// <summary>
/// Эконом-седан — самый доступный класс автомобиля в таксопарке.
/// Дополнительные характеристики: наличие детского кресла и объём багажника.
/// </summary>
public class EconomySedan : PassengerCar
{
    // ──────────────────────────────────────────────
    // Свойства, уникальные для эконом-класса
    // ──────────────────────────────────────────────

    /// <summary>Наличие встроенного детского кресла.</summary>
    public bool HasChildSeat { get; }

    /// <summary>Объём багажника в литрах.</summary>
    public int TrunkVolumeLiters { get; }

    // ──────────────────────────────────────────────
    // Конструктор
    // ──────────────────────────────────────────────

    public EconomySedan(
        string brandAndModel,
        decimal price,
        double fuelConsumptionPer100Km,
        int maxSpeedKmh,
        bool hasChildSeat,
        int trunkVolumeLiters)
        : base(brandAndModel, price, fuelConsumptionPer100Km, maxSpeedKmh)
    {
        HasChildSeat      = hasChildSeat;
        TrunkVolumeLiters = trunkVolumeLiters;
    }

    // ──────────────────────────────────────────────
    // Описание тарифа
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override string GetTariffDetails() =>
        $"Тариф «Эконом» [Детское кресло: {(HasChildSeat ? "да" : "нет")}, " +
        $"Багажник: {TrunkVolumeLiters} л]";
}
