namespace bs.Frmwrk.Core.Dtos.Security
{
    public interface IRecaptchaResponseDto
    {
        string Action { get; set; }
        DateTime CheckDate { get; set; }
        string[] ErrorCodes { get; set; }
        string Hostname { get; set; }
        decimal Score { get; set; }
        bool Success { get; set; }
    }
}