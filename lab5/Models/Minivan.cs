using lab5.Models.Abstractions;

namespace lab5.Models;

public class Minivan : PassengerCar
{

    public int PassengerSeats { get; }
    public bool HasSlidingDoors { get; }

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

    public override string GetTariffDetails() =>
        $"Тариф «Минивэн» [Мест: {PassengerSeats}, " +
        $"Сдвижные двери: {(HasSlidingDoors ? "да" : "нет")}]";
}
