using lab5.Models.Abstractions;

namespace lab5.Comparers;

public class FuelConsumptionComparer : IComparer<PassengerCar>
{
    public int Compare(PassengerCar? x, PassengerCar? y)
    {
        // Если оба указателя ссылаются на один и тот же объект — равны
        if (ReferenceEquals(x, y)) return 0;

        // null считается "меньше" любого реального объекта
        if (x is null) return -1;
        if (y is null) return  1;

        return x.FuelConsumptionPer100Km.CompareTo(y.FuelConsumptionPer100Km);
    }
}
