using System.Collections;
using lab5.Models.Abstractions;

namespace lab5.Collections;

public class TaxiFleet : IEnumerable<PassengerCar>
{

    private readonly List<PassengerCar> _cars = new();
    public string CompanyName { get; set; } = "Городской Таксопарк";
    public IReadOnlyCollection<PassengerCar> Cars => _cars.AsReadOnly();

    public void AddCar(PassengerCar car)
    {
        ArgumentNullException.ThrowIfNull(car);
        _cars.Add(car);
    }

    public decimal CalculateTotalFleetCost() => _cars.Sum(car => car.Price);
    public void SortByFuelConsumption() => _cars.Sort();
    public void Sort(IComparer<PassengerCar> comparer) => _cars.Sort(comparer);

    public IEnumerable<PassengerCar> FindBySpeedRange(int minSpeedKmh, int maxSpeedKmh)
    {
        return _cars.Where(car => car.MaxSpeedKmh >= minSpeedKmh
                               && car.MaxSpeedKmh <= maxSpeedKmh);
    }

    public IEnumerator<PassengerCar> GetEnumerator() => _cars.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator()          => GetEnumerator();
}
