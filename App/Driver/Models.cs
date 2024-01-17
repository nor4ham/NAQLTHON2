using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Driver.Enum; // Make sure to include the necessary using statement for the Status enum.

namespace Driver.Models;
[Table("drivers")]
    public class Driver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("drivername")]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        public string DriverName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        [Column("licensenumber")]
        public string LicenseNumber { get; set; } = null!;

        [Column("dateofbirth",TypeName = "date")]
        
        public DateTime DateOfBirth { get; set; }

        [Column("vehicletype")]
        public string VehicleType { get; set; } = null!;

        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }= string.Empty; // Initialize with a default value!;

        [Phone]
        [Column("phonenumber")]
        public string PhoneNumber { get; set; } = null!;
        [Column("status")]

        public DriverStatus Status { get; set; } // Assuming DriverStatus is an enum type from the "Driver.Enum" namespace.

        // Additional properties...
    }