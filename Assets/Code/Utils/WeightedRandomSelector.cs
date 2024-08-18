using System;
using System.Linq;

namespace Code.Utils
{
    public class WeightedRandomSelector<T> where T : Enum
    {
        private readonly Random _random;
        private readonly T[] _values;
        private double[] _weights;
        private const double DecayFactor = 0.5;

        public WeightedRandomSelector()
        {
            _random = new Random();
            _values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            _weights = new double[_values.Length];

            for (int i = 0; i < _weights.Length; i++)
            {
                _weights[i] = 1.0;
            }
        }

        public T GetRandom()
        {
            double totalWeight = _weights.Sum();
            double randomValue = _random.NextDouble() * totalWeight;

            double cumulativeWeight = 0.0;
            int selectedIndex = 0;

            for (int i = 0; i < _weights.Length; i++)
            {
                cumulativeWeight += _weights[i];
                if (randomValue <= cumulativeWeight)
                {
                    selectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < _weights.Length; i++)
            {
                if (i == selectedIndex)
                {
                    _weights[i] *= DecayFactor;
                }
                else
                {
                    _weights[i] = Math.Min(_weights[i] + (1 - DecayFactor) / (_values.Length - 1), 1.0);
                }
            }

            return _values[selectedIndex];
        }
    }
}