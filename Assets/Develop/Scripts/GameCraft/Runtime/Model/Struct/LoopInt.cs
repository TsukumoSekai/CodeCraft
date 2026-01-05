using System;

namespace OfflineFantasy.GameCraft.Models
{
    [Serializable]
    public struct LoopInt
    {
        private int m_Value;

        public int Value
        {
            set => m_Value = EnsureWithinRange(value, MinLimit, MaxLimit);
            get => m_Value;
        }

        public readonly int MinLimit { get; }

        public readonly int MaxLimit { get; }

        public bool IsMin => Value == MinLimit;

        public bool IsMax => Value == MaxLimit;

        private static int EnsureWithinRange(int _value, int _minLimit, int _maxLimit)
        {
            //int range = _maxLimit - _minLimit + 1;
            //return ((_value - _minLimit) % range + range) % range + _minLimit;

            if (_value > _maxLimit)
                return _minLimit;

            if (_value < _minLimit)
                return _maxLimit;

            return _value;
        }

        public LoopInt(int _currentValue, int _minLimitInclusive, int _maxLimitInclusive)
        {
            if (_minLimitInclusive < _maxLimitInclusive)
            {
                MinLimit = _minLimitInclusive;
                MaxLimit = _maxLimitInclusive;
            }
            else
            {
                MinLimit = _maxLimitInclusive;
                MaxLimit = _minLimitInclusive;
            }

            m_Value = EnsureWithinRange(_currentValue, MinLimit, MaxLimit);
        }

        public override bool Equals(object _obj)
        {
            return _obj is LoopInt loopInt &&
                   Value == loopInt.Value &&
                   MinLimit == loopInt.MinLimit &&
                   MaxLimit == loopInt.MaxLimit;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, MinLimit, MaxLimit);
        }

        public override string ToString()
        {
            return $"{Value.ToString()} : {MinLimit} ~ {MaxLimit}";
        }

        public static implicit operator int(LoopInt _loopInt)
        {
            return _loopInt.Value;
        }

        public static bool operator ==(LoopInt _left, int _right)
        {
            return _left.Value == _right;
        }

        public static bool operator !=(LoopInt _left, int _right)
        {
            return _left.Value != _right;
        }

        public static LoopInt operator +(LoopInt _left, int _right)
        {
            return new LoopInt(_left.Value + _right, _left.MinLimit, _left.MaxLimit);
        }

        public static LoopInt operator -(LoopInt _left, int _right)
        {
            return new LoopInt(_left.Value - _right, _left.MinLimit, _left.MaxLimit);
        }

        public static LoopInt operator ++(LoopInt _left)
        {
            return new LoopInt(_left.Value + 1, _left.MinLimit, _left.MaxLimit);
        }

        public static LoopInt operator --(LoopInt _left)
        {
            return new LoopInt(_left.Value - 1, _left.MinLimit, _left.MaxLimit);
        }


    }
}