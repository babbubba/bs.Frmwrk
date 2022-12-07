namespace bs.Frmwrk.Core.Dtos.Auth
{
    public interface IAuthRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}