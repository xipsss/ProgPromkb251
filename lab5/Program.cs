using lab5.Builders;
using lab5.Collections;
using lab5.Comparers;
using lab5.Models;
using lab5.Models.Abstractions;

PassengerCar[] vehicles =
[
    // Эконом-класс: Volkswagen Polo
    new EconomySedan(
        brandAndModel:            "Volkswagen Polo",
        price:                    1_650_000m,
        fuelConsumptionPer100Km:  6.5,
        maxSpeedKmh:              190,
        hasChildSeat:             true,
        trunkVolumeLiters:        530),

    // Эконом-класс: Hyundai Solaris
    new EconomySedan(
        brandAndModel:            "Hyundai Solaris",
        price:                    1_720_000m,
        fuelConsumptionPer100Km:  6.2,
        maxSpeedKmh:              193,
        hasChildSeat:             false,
        trunkVolumeLiters:        480),

    // Бизнес-класс: Toyota Camry
    new BusinessSedan(
        brandAndModel:            "Toyota Camry",
        price:                    3_800_000m,
        fuelConsumptionPer100Km:  8.3,
        maxSpeedKmh:              210,
        interiorMaterial:         "Натуральная кожа",
        hasPassengerMultimedia:   true),

    // Бизнес-класс: BMW 5 Series
    new BusinessSedan(
        brandAndModel:            "BMW 5 Series",
        price:                    6_200_000m,
        fuelConsumptionPer100Km:  7.8,
        maxSpeedKmh:              250,
        interiorMaterial:         "Кожа Dakota",
        hasPassengerMultimedia:   true),

    // Минивэн: Hyundai Staria
    new Minivan(
        brandAndModel:            "Hyundai Staria",
        price:                    4_900_000m,
        fuelConsumptionPer100Km:  9.5,
        maxSpeedKmh:              185,
        passengerSeats:           7,
        hasSlidingDoors:          true),
];

ITaxiFleetBuilder builder = new TaxiFleetBuilder();

builder.SetCompanyName("Экспресс-Сити Такси");

foreach (PassengerCar vehicle in vehicles)
{
    builder.AddVehicle(vehicle);
}

TaxiFleet taxiFleet = builder.Build();


Console.WriteLine($"=== Автопарк компании «{taxiFleet.CompanyName}» ===");

foreach (PassengerCar car in taxiFleet)
{
    Console.WriteLine($"  - {car}");
}

decimal totalCost = taxiFleet.CalculateTotalFleetCost();
Console.WriteLine($"\nОбщая стоимость автопарка: {totalCost:N0} ₽");

Console.WriteLine("\n=== Сортировка по расходу топлива (л/100 км) ===");

taxiFleet.Sort(new FuelConsumptionComparer());

foreach (PassengerCar car in taxiFleet)
{
    Console.WriteLine($"  - {car.BrandAndModel}: {car.FuelConsumptionPer100Km:F1} л/100 км");
}


int minSpeed = 190;
int maxSpeed = 220;

Console.WriteLine($"\n=== Автомобили с максимальной скоростью от {minSpeed} до {maxSpeed} км/ч ===");

List<PassengerCar> carsInSpeedRange = taxiFleet.FindBySpeedRange(minSpeed, maxSpeed).ToList();

if (carsInSpeedRange.Count > 0)
{
    foreach (PassengerCar car in carsInSpeedRange)
    {
        Console.WriteLine($"  * {car}");
    }
}
else
{
    Console.WriteLine("  Автомобилей в заданном скоростном диапазоне не найдено.");
}
