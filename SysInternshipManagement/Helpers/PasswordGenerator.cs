using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationPlanCadre.Helpers
{
    public class PasswordGenerator
    {
        List<int> charIndexs;
        Random rand;

        public PasswordGenerator()
        {
            rand = new Random();
        }

        public string GeneratePassword(int length)
        {
            const int NBNO = 2;
            const int NBCAP = 4;
            const int NBSPECIAL = 2;

            Reset(length);
            char[] password = BaseChars(length);
            AddNumbers(password, NBNO);
            AddCaps(password, NBCAP);
            AddSpecial(password, NBSPECIAL);

            return new string(password);
        }

        private int PickIndex()
        {
            int index = rand.Next(0, charIndexs.Count());
            int indexValue = charIndexs.ElementAt(index);
            charIndexs.RemoveAt(index);
            return indexValue;
        }

        private void Reset(int length)
        {
            charIndexs = new List<int>();
            for (int i = 0; i < length; i++)
                charIndexs.Add(i);
        }

        private char[] BaseChars(int length)
        {
            char[] password = new char[length];
            for (int i = 0; i < length; i++)
                password[i] = (char)rand.Next(97, 122);
            return password;
        }

        private void AddNumbers(char[] password, int nbNo)
        {
            for (int i = 0; i < nbNo; i++)
            {
                password[PickIndex()] = (char)rand.Next(48, 57);
            }
        }

        private void AddCaps(char[] password, int nbCap)
        {
            for (int i = 0; i < nbCap; i++)
            {
                int index = PickIndex();
                password[index] = char.ToUpper(password[index]);
            }
        }

        private void AddSpecial(char[] password, int nbSpecial)
        {
            for (int i = 0; i < nbSpecial; i++)
            {
                password[PickIndex()] = (char)rand.Next(33, 47);
            }
        }
    }
}