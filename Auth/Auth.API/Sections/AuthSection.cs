namespace Auth.API.Sections
{
    public sealed class AuthSection
    {
        public string CallbackHost { get; set; }

        public string Domain { get; set; }

        public string ApiIdentifier { get; set; }

        public string ClientId { get; set; }

        public string PreparedDomain => $"https://{Domain}/";
    }
}
