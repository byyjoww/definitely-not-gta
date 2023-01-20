using UnityEngine;

namespace DefinitelyNotGta.Vehicles
{
    public interface IDriver
    {
        void EnterVehicle(Transform seat);
        void ExitVehicle(Transform exit);
    }
}
