using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ClientBookRQ
    {
        [Required]
        public required string TrnxId { get; set; }

        [Required]
        public required string ItemCodeRef { get; set; }

        [Required]
        public required List<PassengerData> Passengers { get; set; }

    }

    public class PassengerData
    {
        public PassengerName NameElement { get; set; }
        public required string PassengerType { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PassengerKey { get; set; }
        public string? PassportCopy { get; set; }
        public string? VisaCopy { get; set; }
    }

    public class PassengerName
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
