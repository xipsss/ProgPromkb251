namespace lab5.Models.Abstractions;

public abstract class PassengerCar : IPassengerCar, IComparable<PassengerCar>
{

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

        BrandAndModel          = brandAndModel;
        Price                  = price;
        FuelConsumptionPer100Km = fuelConsumptionPer100Km;
        MaxSpeedKmh            = maxSpeedKmh;
    }
    public abstract string GetTariffDetails();

    public int CompareTo(PassengerCar? other)
    {
        if (other is null) return 1; // null считается "меньше" любого объекта
        return FuelConsumptionPer100Km.CompareTo(other.FuelConsumptionPer100Km);
    }

    public override string ToString() =>
        $"{BrandAndModel} | Цена: {Price:N0} ₽ | " +
        $"Расход: {FuelConsumptionPer100Km:F1} л/100км | " +
        $"Макс. скорость: {MaxSpeedKmh} км/ч | {GetTariffDetails()}";
}
