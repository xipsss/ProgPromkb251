using lab5.Models.Abstractions;

namespace lab5.Models;

/// <summary>
/// Бизнес-седан — премиальный класс автомобиля в таксопарке.
/// Дополнительные характеристики: материал отделки салона и наличие
/// мультимедиа-системы для пассажиров на заднем сиденье.
/// </summary>
public class BusinessSedan : PassengerCar
{
    // ──────────────────────────────────────────────
    // Свойства, уникальные для бизнес-класса
    // ──────────────────────────────────────────────

    /// <summary>Материал отделки салона (например, «Натуральная кожа»).</summary>
    public string InteriorMaterial { get; }

    /// <summary>Наличие мультимедиа-системы для пассажиров на заднем сиденье.</summary>
    public bool HasPassengerMultimedia { get; }

    // ──────────────────────────────────────────────
    // Конструктор
    // ──────────────────────────────────────────────

    public BusinessSedan(
        string brandAndModel,
        decimal price,
        double fuelConsumptionPer100Km,
        int maxSpeedKmh,
        string interiorMaterial,
        bool hasPassengerMultimedia)
        : base(brandAndModel, price, fuelConsumptionPer100Km, maxSpeedKmh)
    {
        InteriorMaterial       = interiorMaterial;
        HasPassengerMultimedia = hasPassengerMultimedia;
    }

    // ──────────────────────────────────────────────
    // Описание тарифа
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override string GetTariffDetails() =>
        $"Тариф «Бизнес» [Салон: {InteriorMaterial}, " +
        $"Мультимедиа сзади: {(HasPassengerMultimedia ? "да" : "нет")}]";
}
