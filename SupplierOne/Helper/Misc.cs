using System.Text;
using Core.Models;

namespace SupplierOne.Helper
{
    public static class Misc
    {
        private static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string GeneratePNR(int length = 6) // 6 Digit PNR by Default
        {
            var random = new Random();
            StringBuilder result = new();
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            return result.ToString();
        }

        public static bool ValidatePassengerData(List<PassengerData> passengers, Sup)
        {
            // Validate Passenger Data
            return true;
        }
    }
}
