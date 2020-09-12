using System;
using System.IO;

namespace LicentaPuzzlePirates.Helpers
{
    public class FileHandler
    {
        private static char[] lineSplitChars = new char[] { '\n', '\r', '\t' };


        public string[] GetBoats()
        {
            DirectoryInfo dir = new DirectoryInfo(@"../../Boats");
            FileInfo[] fileInfos = dir.GetFiles();

            string[] boats = new string[fileInfos.Length];

            for (int i = 0; i < fileInfos.Length; i++)
            {
                boats[i] = fileInfos[i].ToString();
            }

            return boats;
        }

        public string GetBoatStas(string boatName)
        {
            string wholeText = File.ReadAllText(@"../../Boats/" + boatName);

            string boatStats = wholeText.Split(lineSplitChars, StringSplitOptions.RemoveEmptyEntries)[1];

            return boatStats;
        }


        public string[] GetHelpImages()
        {
            DirectoryInfo dir = new DirectoryInfo(@"../../HelpImages");
            FileInfo[] fileInfos = dir.GetFiles();

            string[] images = new string[fileInfos.Length];

            for (int i = 0; i < fileInfos.Length; i++)
            {
                images[i] = dir.FullName + "\\" + fileInfos[i].ToString();
            }

            return images;
        }
    }
}