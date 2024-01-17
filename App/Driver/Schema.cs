using System.ComponentModel.DataAnnotations;
using Driver.Enum;

// Equivalent to your DisruptRequest in Python
public class DriverRequestDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string DriverName { get; set; }

    [Required]
    public string LicenseNumber { get; set; }

    // New fields
    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public string VehicleType { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    public string Address { get; set; }

    public DriverStatus Status { get; set; }


}

// Equivalent to your DisruptResponse in Python
public class DisruptResponseDto
{

}

// Continue defining other DTOs following the same pattern
