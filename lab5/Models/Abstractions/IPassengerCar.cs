namespace lab5.Models.Abstractions;

public interface IPassengerCar
{
    string BrandAndModel { get; }
    decimal Price { get; }
    double FuelConsumptionPer100Km { get; }
    int MaxSpeedKmh { get; }
}