namespace Service.ViewModels
{
    public class LoginResponse
    {
        public int UserId { get; }
        public string Username { get; }
        public string accessToken { get; }

        public LoginResponse(int userId, string username, string accessToken)
        {
            UserId = userId;
            Username = username;
            this.accessToken = accessToken;
        }
    }
}
