namespace Service.ViewModels
{
    public class LoginResponse
    {
        public string Username { get; }
        public string accessToken { get; }

        public LoginResponse(string username, string accessToken)
        {
            Username = username;
            this.accessToken = accessToken;
        }
    }
}
