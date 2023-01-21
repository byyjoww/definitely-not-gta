using UnityEngine.Events;

namespace DefinitelyNotGta.Utils
{
    public interface ITicker
    {
        public event UnityAction OnTick;
    }
}

