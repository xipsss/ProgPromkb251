using lab5.Models.Abstractions;

namespace lab5.Models;

public class BusinessSedan : PassengerCar
{

    public string InteriorMaterial { get; }

    public bool HasPassengerMultimedia { get; }

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

    public override string GetTariffDetails() =>
        $"Тариф «Бизнес» [Салон: {InteriorMaterial}, " +
        $"Мультимедиа сзади: {(HasPassengerMultimedia ? "да" : "нет")}]";
}
