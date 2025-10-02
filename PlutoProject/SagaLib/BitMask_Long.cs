using System;

namespace SagaLib
{
    /// <summary>
    ///     原子掩码类的泛型封装
    /// </summary>
    /// <typeparam name="T">一个枚举类型</typeparam>
    public class BitMask_Long<T>
    {
        private readonly BitMask_Long _ori;

        public BitMask_Long()
        {
            _ori = new BitMask_Long();
        }

        public BitMask_Long(BitMask_Long ori)
        {
            _ori = ori;
        }

        /// <summary>
        ///     此子掩码64位整数值
        /// </summary>
        public ulong Value
        {
            get => _ori.Value;
            set => _ori.Value = value;
        }

        /// <summary>
        ///     检测某个标识
        /// </summary>
        /// <param name="mask">标识</param>
        /// <returns>值</returns>
        public bool Test(T mask)
        {
            return _ori.Test(mask);
        }

        /// <summary>
        ///     设定某标识的值
        /// </summary>
        /// <param name="bitmask">标识</param>
        /// <param name="val">真值</param>
        public void SetValue(T bitmask, bool val)
        {
            _ori.SetValue(bitmask, val);
        }

        public static implicit operator BitMask_Long<T>(BitMask_Long ori)
        {
            return new BitMask_Long<T>(ori);
        }
    }

    /// <summary>
    ///     子掩码标识类
    /// </summary>
    [Serializable]
    public class BitMask_Long
    {
        private ulong _value;

        public BitMask_Long()
        {
            _value = 0;
        }

        /// <summary>
        ///     由int32初始化子掩码
        /// </summary>
        /// <param name="value">值</param>
        public BitMask_Long(ulong value)
        {
            _value = value;
        }

        /// <summary>
        ///     由Boolean初始化子掩码 (64Boolean)
        /// </summary>
        /// <param name="values">真值</param>
        public BitMask_Long(bool[] values)
        {
            _value = 0;
            for (byte i = 0; i < values.Length; i++)
            {
                if (i >= 64)
                    break;
                SetValue(2 ^ i, values[i]);
            }
        }

        public ulong Value
        {
            get => _value;
            set => _value = value;
        }

        /// <summary>
        ///     检测某个标识
        /// </summary>
        /// <param name="mask">标识</param>
        /// <returns>值</returns>
        public bool Test(object mask)
        {
            return Test((ulong)mask);
        }

        /// <summary>
        ///     检测某个标识
        /// </summary>
        /// <param name="mask">标识</param>
        /// <returns>值</returns>
        public bool Test(ulong mask)
        {
            return (_value & mask) != 0;
        }

        /// <summary>
        ///     设定某标识的值
        /// </summary>
        /// <param name="bitmask">标识</param>
        /// <param name="val">真值</param>
        public void SetValue(object bitmask, bool val)
        {
            SetValue((ulong)bitmask, val);
        }

        /// <summary>
        ///     根据数值设置2^(n-1)位标识的值
        /// </summary>
        /// <param name="n">n</param>
        /// <param name="val">真值</param>
        public void SetValueForNum(double n, bool val)
        {
            var bitmask = (ulong)Math.Pow(2, n - 1);
            SetValue(bitmask, val);
        }

        /// <summary>
        ///     设定某标识的值
        /// </summary>
        /// <param name="bitmask">标识</param>
        /// <param name="val">真值</param>
        public void SetValue(ulong bitmask, bool val)
        {
            if (Test(bitmask) != val)
            {
                if (val)
                    _value = _value | bitmask;
                else
                    _value = _value ^ bitmask;
            }
        }
    }
}