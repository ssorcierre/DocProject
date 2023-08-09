using System;
using System.Text.RegularExpressions;

namespace Skloneniya
{
    public class Skl
    {
        public string LastnameSkl(string lastname)
        {
            string error = "Преобразование не удалось";
            Regex patternOV = new Regex(@"\w+(ов|ев|ин)$");
            Regex patternIY = new Regex(@"\w+(ий)$");
            Regex patternA = new Regex(@"\w+(а)$");
            Regex patternOVA = new Regex(@"\w+(ова)$");
            Regex patternINA = new Regex(@"\w+(ина)$");
            Regex patternSogl = new Regex(@"\w+(к|т|з|ч|ц|г|б)");

            if (patternOV.IsMatch(lastname))
                return (lastname += 'у');
            else if (patternIY.IsMatch(lastname))
            {
                string target = "ому";
                return Regex.Replace(lastname, "ий", target);
            }
            else if (patternOVA.IsMatch(lastname))
            {
                string target = "овой";
                return Regex.Replace(lastname, "ова", target);
            }
            else if (patternINA.IsMatch(lastname))
            {
                string target = "ой";
                return Regex.Replace(lastname, "а", target);
            }
            else if (patternA.IsMatch(lastname))
            {
                string target = "е";
                return Regex.Replace(lastname, "а", target);
            }
            else if (patternSogl.IsMatch(lastname))
            {
                return (lastname += 'у');
            }
            else
            {
                return (error);
            }
        }
    }
}
