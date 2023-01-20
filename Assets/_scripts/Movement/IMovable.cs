using UnityEngine;
using UnityEngine.Events;

namespace DefinitelyNotGta.Movement
{
    public interface IMovable
    {
        UnityEvent Move(Vector3 position);
        void Stop();
    }
}