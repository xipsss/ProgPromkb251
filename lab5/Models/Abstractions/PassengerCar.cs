namespace lab5.Models.Abstractions;

/// <summary>
/// Абстрактный базовый класс для всех легковых автомобилей таксопарка.
/// Реализует интерфейс IPassengerCar и поддерживает сортировку по расходу топлива
/// через интерфейс IComparable&lt;PassengerCar&gt;.
/// </summary>
public abstract class PassengerCar : IPassengerCar, IComparable<PassengerCar>
{
    // ──────────────────────────────────────────────
    // Конструктор
    // ──────────────────────────────────────────────

    /// <summary>
    /// Создаёт экземпляр автомобиля с проверкой корректности данных.
    /// </summary>
    protected PassengerCar(
        string brandAndModel,
        decimal price,
        double fuelConsumptionPer100Km,
        int maxSpeedKmh)
    {
        if (string.IsNullOrWhiteSpace(brandAndModel))
            throw new ArgumentException("Марка и модель не могут быть пустыми.", nameof(brandAndModel));

        if (price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Стоимость автомобиля должна быть положительной.");

        if (fuelConsumptionPer100Km < 0)
            throw new ArgumentOutOfRangeException(nameof(fuelConsumptionPer100Km), "Расход топлива не может быть отрицательным.");

        if (maxSpeedKmh <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxSpeedKmh), "Максимальная скорость должна быть положительной.");

        BrandAndModel           = brandAndModel;
        Price                   = price;
        FuelConsumptionPer100Km = fuelConsumptionPer100Km;
        MaxSpeedKmh             = maxSpeedKmh;
    }

    // ──────────────────────────────────────────────
    // Свойства (реализация IPassengerCar)
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public string  BrandAndModel           { get; }

    /// <inheritdoc/>
    public decimal Price                   { get; }

    /// <inheritdoc/>
    public double  FuelConsumptionPer100Km { get; }

    /// <inheritdoc/>
    public int     MaxSpeedKmh             { get; }

    // ──────────────────────────────────────────────
    // Абстрактный метод — каждый подкласс описывает свой тариф
    // ──────────────────────────────────────────────

    /// <summary>
    /// Возвращает строку с описанием тарифа и уникальных характеристик
    /// конкретного типа автомобиля.
    /// </summary>
    public abstract string GetTariffDetails();

    // ──────────────────────────────────────────────
    // Сравнение — используется List.Sort() без компаратора
    // ──────────────────────────────────────────────

    /// <summary>
    /// Сравнивает автомобили по расходу топлива (от меньшего к большему).
    /// </summary>
    public int CompareTo(PassengerCar? other)
    {
        if (other is null) return 1; // null считается "меньше" любого объекта
        return FuelConsumptionPer100Km.CompareTo(other.FuelConsumptionPer100Km);
    }

    // ──────────────────────────────────────────────
    // Строковое представление
    // ──────────────────────────────────────────────

    public override string ToString() =>
        $"{BrandAndModel} | Цена: {Price:N0} ₽ | " +
        $"Расход: {FuelConsumptionPer100Km:F1} л/100км | " +
        $"Макс. скорость: {MaxSpeedKmh} км/ч | {GetTariffDetails()}";
}
