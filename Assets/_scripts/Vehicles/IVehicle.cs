using DefinitelyNotGta.Movement;
using DefinitelyNotGta.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefinitelyNotGta.Vehicles
{
    public interface IVehicle : IMovable
    {
        void StartDriving(IDriver driver);
        IDriver StopDriving();
    }
}
