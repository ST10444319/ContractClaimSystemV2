namespace ContractClaimSystem.Web.Models
{
    public enum ClaimStatus
    {
        Pending,
        Verified,
        Approved,
        Rejected,
        Settled
    }

    public enum UserRole
    {
        Lecturer,
        Coordinator,
        Manager,
        HR
    }
}
