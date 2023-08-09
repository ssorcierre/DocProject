using System;
using System.Text.RegularExpressions;

namespace Skloneniya
{
    public class Skl
    {
        public string LastnameSkl(string lastname)
        {
            string error = "�������������� �� �������";
            Regex patternOV = new Regex(@"\w+(��|��|��)$");
            Regex patternIY = new Regex(@"\w+(��)$");
            Regex patternA = new Regex(@"\w+(�)$");
            Regex patternOVA = new Regex(@"\w+(���)$");
            Regex patternINA = new Regex(@"\w+(���)$");
            Regex patternSogl = new Regex(@"\w+(�|�|�|�|�|�|�)");

            if (patternOV.IsMatch(lastname))
                return (lastname += '�');
            else if (patternIY.IsMatch(lastname))
            {
                string target = "���";
                return Regex.Replace(lastname, "��", target);
            }
            else if (patternOVA.IsMatch(lastname))
            {
                string target = "����";
                return Regex.Replace(lastname, "���", target);
            }
            else if (patternINA.IsMatch(lastname))
            {
                string target = "��";
                return Regex.Replace(lastname, "�", target);
            }
            else if (patternA.IsMatch(lastname))
            {
                string target = "�";
                return Regex.Replace(lastname, "�", target);
            }
            else if (patternSogl.IsMatch(lastname))
            {
                return (lastname += '�');
            }
            else
            {
                return (error);
            }
        }
    }
}
