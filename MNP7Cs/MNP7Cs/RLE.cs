using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNP7Cs
{
    // Identifikovanje podniza od bar 3 jednaka karaktera
    public static class RLE
    {
        public static string RunsIdentify(string input)
        {
			int i = 0, br;
			string stage1output = string.Empty;

			while (i < input.Length)
			{
				br = 0;
				if (i < input.Length - 2 && input[i] == input[i + 1] && input[i + 1] == input[i + 2])
				{
					// bar 3 su sigurno jednaka
					// provera da li ima jos jednakih u nastavku stringa
					i = i + 2;
					while (i < input.Length - 1 && input[i] == input[i + 1])
					{
						br++;
						i++;
					}
					//itoa(br, br_s, 10);

					for (int k = 0; k < 3; k++)
						stage1output += input[i];

					stage1output += br;
				}
				else // nema duplikata, samo ubacimo simbol u niz
				{
					stage1output += input[i];
				}
				i++;
			}
			return stage1output;

		}

		public static string Decode(string code)
        {
			string ret = code;
			for(int i=0; i<ret.Length-3; i++)
            {
				if (ret[i] == ret[i + 1] && ret[i + 1] == ret[i + 2])
                {
					int broj = int.Parse(ret[i + 3].ToString());
					ret = ret.Remove(i + 3, 1);
					for (int j = 0; j < broj; j++)
						ret = ret.Insert(i + 3, ret[i].ToString());
					i += broj;
                }
            }
			return ret;
        }
	}
}
