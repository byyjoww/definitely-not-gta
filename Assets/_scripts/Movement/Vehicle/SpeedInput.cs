namespace DefinitelyNotGta.Movement
{
    public class SpeedInput
    {
        private float max = 1.0f;
        private float min = 0f;
        private float increment = 0.1f;
        private float decrement = 0.5f;

        public float Current { get; private set; } = 0f;

        public SpeedInput()
        {
            
        }

        public void Decrement()
        {
            Current -= decrement;
            if (Current < min) { Current = min; }
        }

        public void Increment()
        {
            Current += increment;
            if (Current > max) { Current = max; }
        }
    }
}

