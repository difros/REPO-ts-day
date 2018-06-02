using System;
using System.Collections.Generic;

namespace GQ.Core.utils
{
    public static class FunctionsUtils
    {
        /// <summary>
        /// 
        /// </summary>
        public enum PasswordCharaters : uint
        {
            Numeros = 0x1,
            Minusculas = 0x2,
            Mayusculas = 0x4,
            Simbolos = 0x8,
            NumerosMinusculasMayusculas = (0x1 + 0x2 + 0x4)
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PasswordLength"></param>
        /// <param name="allowedChars"></param>
        /// <returns></returns>
        public static string CreateRandomCode(int PasswordLength, PasswordCharaters allowedChars = PasswordCharaters.NumerosMinusculasMayusculas)
        {
            return CreateRandomCode(PasswordLength, (uint)allowedChars);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PasswordLength"></param>
        /// <param name="allowedChars"></param>
        /// <returns></returns>
        public static string CreateRandomCode(int PasswordLength, uint allowedChars)
        {
            string _allowedChars = "";
            if ((allowedChars & (uint)PasswordCharaters.Numeros) == (uint)PasswordCharaters.Numeros)
            {
                _allowedChars = _allowedChars + "0123456789";
            }
            if ((allowedChars & (uint)PasswordCharaters.Mayusculas) == (uint)PasswordCharaters.Mayusculas)
            {
                _allowedChars = _allowedChars + "ABCDEFGHJKLMNPQRSTUVWXYZ";
            }
            if ((allowedChars & (uint)PasswordCharaters.Minusculas) == (uint)PasswordCharaters.Minusculas)
            {
                _allowedChars = _allowedChars + "abcdefghjklmnpqrstuvwxyz";
            }
            if ((allowedChars & (uint)PasswordCharaters.Simbolos) == (uint)PasswordCharaters.Simbolos)
            {
                _allowedChars = _allowedChars + "/*-+!·$%&/()=?¿#@¡";
            }
            return CreateRandomCode(PasswordLength, _allowedChars);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PasswordLength"></param>
        /// <param name="allowedChars"></param>
        /// <returns></returns>
        public static string CreateRandomCode(int PasswordLength, string allowedChars)
        {
            string _allowedChars = allowedChars; // "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            Byte[] randomBytes = new Byte[PasswordLength];
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < PasswordLength; i++)
            {
                Random randomObj = new Random();
                randomObj.NextBytes(randomBytes);
                chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
            }

            return new string(chars);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="join"></param>
        /// <param name="data"></param>
        public static void AppendJoin(this System.Text.StringBuilder value, string join, IEnumerable<string> data)
        {
            foreach (var item in data)
                value.Append(item + join);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long JavaTimestampFromDateTime(DateTime date)
        {
            return (UnixTimestampFromDateTime(date) * 1000);
        }
    }
}
