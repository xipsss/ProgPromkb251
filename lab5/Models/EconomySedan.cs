using lab5.Models.Abstractions;

namespace lab5.Models;

public class EconomySedan : PassengerCar
{
    public EconomySedan(
        string brandAndModel,
        decimal price,
        double fuelConsumptionPer100Km,
        int maxSpeedKmh,
        bool hasChildSeat,
        int trunkVolumeLiters)
        : base(brandAndModel, price, fuelConsumptionPer100Km, maxSpeedKmh)
    {
        HasChildSeat       = hasChildSeat;
        TrunkVolumeLiters  = trunkVolumeLiters;
    }
    public override string GetTariffDetails() =>
        $"Тариф «Эконом» [Детское кресло: {(HasChildSeat ? "да" : "нет")}, " +
        $"Багажник: {TrunkVolumeLiters} л]";
}
