using lab5.Collections;
using lab5.Models.Abstractions;

namespace lab5.Builders;
public class TaxiFleetBuilder : ITaxiFleetBuilder
{
    private TaxiFleet _fleet = new();

    public ITaxiFleetBuilder SetCompanyName(string name)
    {
        _fleet.CompanyName = name;
        return this; // возвращаем себя для цепочки вызовов
    }

    public ITaxiFleetBuilder AddVehicle(PassengerCar car)
    {
        _fleet.AddCar(car);
        return this; // возвращаем себя для цепочки вызовов
    }

    public TaxiFleet Build()
    {
        // Сохраняем собранный парк и сбрасываем строитель,
        // чтобы он был готов к созданию следующего объекта.
        TaxiFleet completedFleet = _fleet;
        _fleet = new TaxiFleet();
        return completedFleet;
    }
}
