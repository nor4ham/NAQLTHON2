using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customer.Models ;
[Table("customers")]
public class Customer{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Column("FullName")]
    public string Name { get; set; } = null!;

    [EmailAddress]
    [Column("Email")]
    public string Email { get; set; } = null!;

    [Phone]
    [Column("PhoneNumber")]
    public string PhoneNumber { get; set; } = null!;

    [Column("Address")]
    public string Address { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime DateOfBirth { get; set; } 

        // Additional properties and annotations as needed
}