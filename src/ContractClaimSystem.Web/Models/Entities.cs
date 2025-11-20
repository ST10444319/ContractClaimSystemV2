using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractClaimSystem.Web.Models
{
    public class Claim
    {
        public int Id { get; set; }

        [Required]
        public string LecturerName { get; set; } = string.Empty;

        [Range(0, 9999)]
        public decimal HoursWorked { get; set; }

        [Range(0, 999999)]
        public decimal HourlyRate { get; set; }

        public string? Notes { get; set; }

        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public decimal TotalAmount => HoursWorked * HourlyRate;

        public bool IsAutoFlagged { get; set; } = false;
        public string? AutoValidationNotes { get; set; }

        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }

    public class Document
    {
        public int Id { get; set; }

        public int ClaimId { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }

    public class Approval
    {
        public int Id { get; set; }

        public int ClaimId { get; set; }

        public string ApproverName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public DateTime DateApproved { get; set; } = DateTime.UtcNow;

        public string Decision { get; set; } = "Pending";
    }

    public class AppUser
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Lecturer;
    }
}
