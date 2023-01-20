using UnityEngine.Events;

namespace DefinitelyNotGta.Movement
{
    public interface ITicker
    {
        public event UnityAction OnTick;
    }
}

