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

        public static string GeneratePaxKey(int length = 3) // 6 Digit PNR by Default
        {
            var random = new Random();
            StringBuilder result = new();
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            return result.ToString();
        }

        public static bool ValidatePassengerData(List<PassengerData> passengers, PassengerFares passengerFares)
        {
            // Validate Passenger Data
            var flag = true;
            int adt = 0, child = 0, inf = 0;

            foreach(var pax in passengers)
            {
                if (pax.PassengerType == PassengerType.ADT)
                    adt++;
                else if (pax.PassengerType == PassengerType.CHD)
                    child++;
                else if (pax.PassengerType == PassengerType.INF)
                    inf++;
            }

            if (passengerFares?.Adult?.Count != null && adt != passengerFares?.Adult?.Count)
                flag = false;
            if (passengerFares?.Child?.Count != null && adt != passengerFares?.Child?.Count)
                flag = false;
            if (passengerFares?.Infant?.Count != null && adt != passengerFares?.Infant?.Count)
                flag = false;
            return flag;
        }

        public static bool ValidatePNR(string pnr)
        {
            // Validate PNR
            return pnr.Length == 6;
        }
    }
}
