using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDataEntities.Models
{
    public class Supplier
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Postal Address")]
        public string PostalAddress { get; set; }
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        public string City { get; set; }
        [DisplayName("Contact One")]
        [Required]
        public string Contact1 { get; set; }

        [DisplayName("Contact Two")]
        public string Contact2 { get; set; }
        
        public string Email { get; set; }
        [DisplayName("Pin Number")]
        public string PinNumber { get; set; }
        [DisplayName("Contact person")]
        public string ContactPersonName { get; set; }
        [DisplayName("Credit Limit")]
        public double CreditLimit { get; set; }
    }
}
