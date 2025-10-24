namespace ContractClaimSystem.Web.Data
{
    public class UploadOptions
    {
        public long MaxFileSizeBytes { get; set; } = 5 * 1024 * 1024;

        public string[] AllowedExtensions { get; set; } = new[] { ".pdf", ".docx", ".xlsx" };

        public string UploadRoot { get; set; } = "wwwroot/uploads";
    }
}
