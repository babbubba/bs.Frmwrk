using bs.Frmwrk.Core.Models.Security;
using bs.Frmwrk.Shared;
using System.Text.RegularExpressions;

namespace bs.Frmwrk.Security.Utilities
{
    public class PasswordAdvisor
    {
        public static PasswordScore CheckStrength(string password)
        {
            int score = 1; //Very Weak

            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;

            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.Match(password, @"\d+", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"[a-z]", RegexOptions.ECMAScript).Success &&
              Regex.Match(password, @"[A-Z]", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @".[!,@,#,$,%,^,&,*,?,_,~,£,(,)]", RegexOptions.ECMAScript).Success)
                score++;

            return score <= 5 ? score.ToEnum<PasswordScore>() : PasswordScore.VeryStrong;
        }
    }
}