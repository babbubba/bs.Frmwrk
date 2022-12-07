namespace bs.Frmwrk.Core.Dtos.Auth
{
    public interface ITokenJWTDto
    {
        string Token { get; set; }
        DateTime ExpireAt { get; set; }
    }
}