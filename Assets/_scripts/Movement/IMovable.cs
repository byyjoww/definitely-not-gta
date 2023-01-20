using UnityEngine;

namespace DefinitelyNotGta.Movement
{
    public interface IMovable
    {
        void Move(Vector3 position);
        void Stop();
    }
}