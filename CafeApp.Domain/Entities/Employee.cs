using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CafeApp.Domain.Entities;

namespace CafeApp.Domain.Entities
{
    [Table("employee")]
    public class Employee
    {
        [Key]
        [Column("id")]
        [StringLength(10, MinimumLength = 9, ErrorMessage = "Id must follow UIXXXXXXX format")]
        public string Id { get; set; } = null!;  // UIXXXXXXX (9 chars, e.g., UI0000001)

        [Column("name")]
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Column("email_address")]
        [Required]
        [StringLength(255)]
        public string EmailAddress { get; set; } = null!;

        [Column("phone_number")]
        [Required]
        [StringLength(8, MinimumLength = 8)]
        public string PhoneNumber { get; set; } = null!;

        [Column("gender")]
        [Required]
        public string Gender { get; set; } = null!; // ENUM('Male','Female') in DB

        [Column("cafe_id")]
        public Guid? CafeId { get; set; }  // maps to CHAR(36) UUID

        public Cafe Cafe { get; set; } = null!;

        [Column("start_date")]
        public DateOnly? StartDate { get; set; }  // nullable DATE
    }
}
