using UnityEngine;
using UnityEngine.Events;

namespace DefinitelyNotGta.Movement
{
    public interface IMovable
    {
        UnityEvent MoveTo(Vector3 position);

        void Stop();
    }
}