using System.Text.RegularExpressions;

namespace GQ.Security
{
    /// <summary>
    /// 
    /// </summary>
    public enum PasswordScore
    {
        /// <summary>
        /// 
        /// </summary>
        Blank = 0,
        /// <summary>
        /// 
        /// </summary>
        VeryWeak = 1,
        /// <summary>
        /// 
        /// </summary>
        Weak = 2,
        /// <summary>
        /// 
        /// </summary>
        Medium = 3,
        /// <summary>
        /// 
        /// </summary>
        Strong = 4,
        /// <summary>
        /// 
        /// </summary>
        VeryStrong = 5
    }

    /// <summary>
    /// 
    /// </summary>
    public static class PasswordAdvisor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static PasswordScore CheckStrength(string password)
        {
            int score = 0;

            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;

            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.Match(password, @"/\d+/", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"/[a-z]/", RegexOptions.ECMAScript).Success &&
              Regex.Match(password, @"/[A-Z]/", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"/.[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]/", RegexOptions.ECMAScript).Success)
                score++;

            return (PasswordScore)score;
        }
    }
}
