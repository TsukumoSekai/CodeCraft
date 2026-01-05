namespace OfflineFantasy.GameCraft.Utility
{
    public static class ComparerUtility
    {
        public static bool Compare(int _left, int _right, ComparerType _comparer)
        {
            switch (_comparer)
            {
                case ComparerType.Equal:
                    return _left == _right;
                case ComparerType.NotEqual:
                    return _left != _right;
                case ComparerType.Greater:
                    return _left > _right;
                case ComparerType.GreaterOrEqual:
                    return _left >= _right;
                case ComparerType.Less:
                    return _left < _right;
                case ComparerType.LessOrEqual:
                    return _left <= _right;
                default:
                    return false;
            }
        }

        public static bool Compare(uint _left, uint _right, ComparerType _comparer)
        {
            switch (_comparer)
            {
                case ComparerType.Equal:
                    return _left == _right;
                case ComparerType.NotEqual:
                    return _left != _right;
                case ComparerType.Greater:
                    return _left > _right;
                case ComparerType.GreaterOrEqual:
                    return _left >= _right;
                case ComparerType.Less:
                    return _left < _right;
                case ComparerType.LessOrEqual:
                    return _left <= _right;
                default:
                    return false;
            }
        }

        public static bool Compare(float _left, float _right, ComparerType _comparer)
        {
            switch (_comparer)
            {
                case ComparerType.Equal:
                    return _left == _right;
                case ComparerType.NotEqual:
                    return _left != _right;
                case ComparerType.Greater:
                    return _left > _right;
                case ComparerType.GreaterOrEqual:
                    return _left >= _right;
                case ComparerType.Less:
                    return _left < _right;
                case ComparerType.LessOrEqual:
                    return _left <= _right;
                default:
                    return false;
            }
        }

        public static bool Compare(bool _left, bool _right, ComparerType _comparer)
        {
            switch (_comparer)
            {
                case ComparerType.Equal:
                    return _left == _right;
                case ComparerType.NotEqual:
                    return _left != _right;
                default:
                    return false;
            }
        }

        public static bool CheckEqual(bool _bool, ComparerType _comparer)
        {
            switch (_comparer)
            {
                case ComparerType.Equal:
                    return _bool;
                case ComparerType.NotEqual:
                    return !_bool;
                default:
                    return false;
            }
        }


        /// <summary>
        /// 比较器类型
        /// </summary>
        public enum ComparerType
        {
            /// <summary>
            /// 等于
            /// </summary>
            Equal,
            /// <summary>
            /// 不等于
            /// </summary>
            NotEqual,
            /// <summary>
            /// 大于
            /// </summary>
            Greater,
            /// <summary>
            /// 大于等于
            /// </summary>
            GreaterOrEqual,
            /// <summary>
            /// 小于
            /// </summary>
            Less,
            /// <summary>
            /// 小于等于
            /// </summary>
            LessOrEqual,
        }
    }
}