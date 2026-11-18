using lab5.Models.Abstractions;

namespace lab5.Models;

/// <summary>
/// Минивэн — вместительный автомобиль для перевозки больших групп пассажиров.
/// Дополнительные характеристики: количество пассажирских мест и наличие
/// сдвижных боковых дверей.
/// </summary>
public class Minivan : PassengerCar
{
    // ──────────────────────────────────────────────
    // Свойства, уникальные для минивэна
    // ──────────────────────────────────────────────

    /// <summary>Количество пассажирских мест (включая водителя).</summary>
    public int PassengerSeats { get; }

    /// <summary>Наличие сдвижных боковых дверей.</summary>
    public bool HasSlidingDoors { get; }

    // ──────────────────────────────────────────────
    // Конструктор
    // ──────────────────────────────────────────────

    public Minivan(
        string brandAndModel,
        decimal price,
        double fuelConsumptionPer100Km,
        int maxSpeedKmh,
        int passengerSeats,
        bool hasSlidingDoors)
        : base(brandAndModel, price, fuelConsumptionPer100Km, maxSpeedKmh)
    {
        PassengerSeats  = passengerSeats;
        HasSlidingDoors = hasSlidingDoors;
    }

    // ──────────────────────────────────────────────
    // Описание тарифа
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override string GetTariffDetails() =>
        $"Тариф «Минивэн» [Мест: {PassengerSeats}, " +
        $"Сдвижные двери: {(HasSlidingDoors ? "да" : "нет")}]";
}
