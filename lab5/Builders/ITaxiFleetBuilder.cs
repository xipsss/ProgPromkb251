using lab5.Collections;
using lab5.Models.Abstractions;

namespace lab5.Builders;

public interface ITaxiFleetBuilder
{

    ITaxiFleetBuilder SetCompanyName(string name);
    ITaxiFleetBuilder AddVehicle(PassengerCar car);
    TaxiFleet Build();
}
