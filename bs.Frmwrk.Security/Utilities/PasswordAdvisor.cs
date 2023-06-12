using bs.Frmwrk.Core.Models.Security;
using bs.Frmwrk.Shared;
using System.Reflection.Metadata.Ecma335;
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

        public static string GetPasswordScoreTips(PasswordScore passwordScore)
        {
            switch (passwordScore)
            {
                case PasswordScore.Blank:
                    return "Password vuota";
                case PasswordScore.VeryWeak:
                    return "Password estremamente corta";
                case PasswordScore.Weak:
                case PasswordScore.Medium:
                case PasswordScore.Strong:
                case PasswordScore.VeryStrong:
                    return "Rendi la password piu complessa: maggiore di 8 caratteri ed includi maiuscole e segni di punteggiatura";
                default:
                    return "Password non valida";
            }
        }
    }


}