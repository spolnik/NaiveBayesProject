using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace NaiveBayesProject.Mushroom
{
    class DataPreparation
    {

        public bool loadData(string mushroomCategoryFile, string mushroomDataFile, string mushroomTeachingFile,string mushroomTestingFile)
        {
            if (!File.Exists(mushroomCategoryFile))
            {
                return false;
            }
            if (!File.Exists(mushroomTeachingFile) || !File.Exists(mushroomTestingFile))
            {
                prepareMushroomTeachingDataFile(mushroomDataFile, mushroomTeachingFile, mushroomTestingFile);
            }
            return true;
        }

        public void prepareMushroomTeachingDataFile(string inputStandardDataLocalization, string mushroomTeachingDataFile, string mushroomTestingDataFile)
        {
            var fileInfo = new FileInfo(inputStandardDataLocalization);
            Console.WriteLine("Preparing MushroomTeachingDataFile and MushroomTestingDataFile");
            TextReader reader = fileInfo.OpenText();
            TextWriter writer1 = new StreamWriter(mushroomTeachingDataFile);
            TextWriter writer2 = new StreamWriter(mushroomTestingDataFile);
            string text;

            int i = 0;
            while ((text = reader.ReadLine()) != null)
            {
                if(i%2 != 0)
                {
                    AddNewObjectToMushroomDataFile(writer1, text);
                }
                else
                {
                    AddNewObjectToMushroomDataFile(writer2, text);
                }
                i++;
            }
            writer1.Close();
            writer2.Close();
            reader.Close();
        }

        private void AddNewObjectToMushroomDataFile(TextWriter writer, string text)
        {
            writer.WriteLine("#NewObject");
            writer.WriteLine("#CategoryType");
            string[] tab = Regex.Split(text, @",");
            if (String.Equals(tab[0], "e"))
            {
                writer.WriteLine("Edible");
            }
            else
            {
                writer.WriteLine("Poisonous");
            }
            writer.WriteLine("#End");
            writer.WriteLine("#Attributes");
            switch (tab[1])
            {
                case "b":
                    writer.WriteLine("cap-shape-bell");
                    break;
                case "c":
                    writer.WriteLine("cap-shape-conical");
                    break;
                case "x":
                    writer.WriteLine("cap-shape-convex");
                    break;
                case "f":
                    writer.WriteLine("cap-shape-flat");
                    break;
                case "k":
                    writer.WriteLine("cap-shape-knobbed");
                    break;
                case "s":
                    writer.WriteLine("cap-shape-sunken");
                    break;

            }
            switch (tab[2])
            {
                case "f":
                    writer.WriteLine("cap-surface-figrous");
                    break;
                case "g":
                    writer.WriteLine("cap-surface-grooves");
                    break;
                case "y":
                    writer.WriteLine("cap-surface-scaly");
                    break;
                case "s":
                    writer.WriteLine("cap-surface-smooth");
                    break;
            }
            switch (tab[3])
            {
                case "n":
                    writer.WriteLine("cap-color-brown");
                    break;
                case "b":
                    writer.WriteLine("cap-color-buff");
                    break;
                case "c":
                    writer.WriteLine("cap-color-cinnamon");
                    break;
                case "g":
                    writer.WriteLine("cap-color-gray");
                    break;
                case "r":
                    writer.WriteLine("cap-color-green");
                    break;
                case "p":
                    writer.WriteLine("cap-color-pink");
                    break;
                case "u":
                    writer.WriteLine("cap-color-purple");
                    break;
                case "e":
                    writer.WriteLine("cap-color-red");
                    break;
                case "w":
                    writer.WriteLine("cap-color-white");
                    break;
                case "y":
                    writer.WriteLine("cap-color-yellow");
                    break;
            }
            switch (tab[5])
            {
                case "a":
                    writer.WriteLine("odor-almond");
                    break;
                case "l":
                    writer.WriteLine("odor-anise");
                    break;
                case "c":
                    writer.WriteLine("odor-creosote");
                    break;
                case "y":
                    writer.WriteLine("odor-fishy");
                    break;
                case "f":
                    writer.WriteLine("odor-foul");
                    break;
                case "m":
                    writer.WriteLine("odor-musty");
                    break;
                case "n":
                    writer.WriteLine("odor-none");
                    break;
                case "p":
                    writer.WriteLine("odor-pungent");
                    break;
                case "s":
                    writer.WriteLine("odor-spicy");
                    break;
            }
            switch (tab[10])
            {
                case "e":
                    writer.WriteLine("stalk-shape-enlarging");
                    break;
                case "t":
                    writer.WriteLine("stalk-shape-tapering");
                    break;
            }
            switch (tab[11])
            {
                case "b":
                    writer.WriteLine("stalk-root-bulbous");
                    break;
                case "c":
                    writer.WriteLine("stalk-root-club");
                    break;
                case "u":
                    writer.WriteLine("stalk-root-cup");
                    break;
                case "e":
                    writer.WriteLine("stalk-root-equal");
                    break;
                case "z":
                    writer.WriteLine("stalk-root-rhizomorphs");
                    break;
                case "r":
                    writer.WriteLine("stalk-root-rooted");
                    break;
                case "?":
                    writer.WriteLine("stalk-root-missing");
                    break;
            }
            switch (tab[21])
            {
                case "a":
                    writer.WriteLine("population-abundant");
                    break;
                case "c":
                    writer.WriteLine("population-clustered");
                    break;
                case "n":
                    writer.WriteLine("population-numerous");
                    break;
                case "s":
                    writer.WriteLine("population-scattered");
                    break;
                case "v":
                    writer.WriteLine("population-several");
                    break;
                case "y":
                    writer.WriteLine("population-solitary");
                    break;
            }
            switch (tab[22])
            {
                case "g":
                    writer.WriteLine("habitat-grasses");
                    break;
                case "l":
                    writer.WriteLine("habitat-leaves");
                    break;
                case "m":
                    writer.WriteLine("habitat-meadows");
                    break;
                case "p":
                    writer.WriteLine("habitat-paths");
                    break;
                case "u":
                    writer.WriteLine("habitat-urban");
                    break;
                case "w":
                    writer.WriteLine("habitat-waste");
                    break;
                case "d":
                    writer.WriteLine("habitat-woods");
                    break;
            }
            writer.WriteLine("#End");
            writer.WriteLine("#Count");
            writer.WriteLine("1");
            writer.WriteLine("#End");
            writer.WriteLine("#EndObject");
            writer.WriteLine();
        }
    }
}
