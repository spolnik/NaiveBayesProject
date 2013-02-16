using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace NaiveBayesProject.Mushroom
{
    class Verification
    {
        public void verify(string inputFileName)
        {
            var fileInfo = new FileInfo(inputFileName);
            Console.WriteLine("Verification");
            TextReader reader = fileInfo.OpenText();

            string text;
            int i = 0;
            int j = 0;
            while ((text = reader.ReadLine()) != null)
            {
                string[] tab = Regex.Split(text, @" ");
                if (float.Parse(tab[0]) >= float.Parse(tab[1]) && String.Equals(tab[2], "Edible") || float.Parse(tab[0]) < float.Parse(tab[1]) && String.Equals(tab[2], "Poisonous"))
                {
                    i++;
                }
                else
                {
                    j++;
                }
            }
            Console.WriteLine("Correct: "  + i);
            Console.WriteLine("Incorrect: " + j);
            
        }

    }
}
