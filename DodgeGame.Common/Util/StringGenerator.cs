using System;

namespace DodgeGame.Common.Util
{
    public class StringGenerator
    {
        public static string GenerateAlpha6()
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            char[] buffer = new char[6];

            for (int i = 0; i < 6; i++)
                buffer[i] = letters[random.Next(letters.Length)];

            return new string(buffer);
        }

    }
}