using System;

namespace SagaLib
{
    /// <summary>
    ///     原子掩码类的泛型封装
    /// </summary>
    /// <typeparam name="T">一个枚举类型</typeparam>
    public class BitMask<T>
    {
        private readonly BitMask _ori;

        public BitMask()
        {
            _ori = new BitMask();
        }

        public BitMask(BitMask ori)
        {
            this._ori = ori;
        }

        /// <summary>
        ///     此子掩码32位整数值
        /// </summary>
        public int Value
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

        public static implicit operator BitMask<T>(BitMask ori)
        {
            return new BitMask<T>(ori);
        }
    }

    /// <summary>
    ///     子掩码标识类
    /// </summary>
    [Serializable]
    public class BitMask
    {
        private int _value;

        public BitMask()
        {
            _value = 0;
        }

        /// <summary>
        ///     由int32初始化子掩码
        /// </summary>
        /// <param name="value">值</param>
        public BitMask(int value)
        {
            this._value = value;
        }

        /// <summary>
        ///     由Boolean初始化子掩码 (32Boolean)
        /// </summary>
        /// <param name="values">真值</param>
        public BitMask(bool[] values)
        {
            _value = 0;
            for (byte i = 0; i < values.Length; i++)
            {
                if (i >= 32)
                    break;
                SetValue(2 ^ i, values[i]);
            }
        }

        public int Value
        {
            get => _value;
            set => this._value = value;
        }

        /// <summary>
        ///     检测某个标识
        /// </summary>
        /// <param name="mask">标识</param>
        /// <returns>值</returns>
        public bool Test(object mask)
        {
            return Test((int)mask);
        }

        /// <summary>
        ///     检测某个标识
        /// </summary>
        /// <param name="mask">标识</param>
        /// <returns>值</returns>
        public bool Test(int mask)
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
            SetValue((int)bitmask, val);
        }

        /// <summary>
        ///     设定某标识的值
        /// </summary>
        /// <param name="bitmask">标识</param>
        /// <param name="val">真值</param>
        public void SetValue(int bitmask, bool val)
        {
            if (Test(bitmask) == val)
            {
                return;
            }

            _value = (val) ? _value | bitmask : _value ^ bitmask;
        }
    }
}