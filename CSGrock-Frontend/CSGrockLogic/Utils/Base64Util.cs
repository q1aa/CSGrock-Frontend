using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGrock_Frontend.CSGrockLogic.Utils
{
    internal class Base64Util
    {
        public static Dictionary<int, string> GetImageReturn(string base64Image, int splitLenght = 100)
        {
            Dictionary<int, string> imageParts = new Dictionary<int, string>();

            int parts = base64Image.Length / splitLenght;

            for (int i = 0; i < parts; i++)
            {
                imageParts.Add(i, base64Image.Substring(i * splitLenght, splitLenght));
            }

            return imageParts;
        }
    }
}
