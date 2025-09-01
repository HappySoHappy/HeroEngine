using HeroEngine.Framework;
using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Persistance
{
    public class Account
    {
        public bool Enabled { get; set; } = true;
        public ExecutionConfiguration? Configuration { get; set; }

        private FileLogger _logger;

        [JsonIgnore]
        public FileLogger Logger { get { _logger.Prefix = Server + " " + (!string.IsNullOrEmpty(Name) ? Name : Email); return _logger; } set { _logger = value; } }

        public string? Name { get; set; }
        public ExistingSession? Session { get; set; }

        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Server { get; set; }

        [JsonIgnore]
        public AccountStatus? Status { get; set; }

        [JsonIgnore]
        public InternalStateHolder? InternalState { get; set; }

        [JsonIgnore]
        public HeroZero? HeroZero { get; set; }

        public Account()
        {
            _logger = new FileLogger(!string.IsNullOrEmpty(Name) ? Name! : Email!, "execution");
            _logger.WriteConsole = true;
        }

        public Account(string email, string password, string server)
        {
            Email = email;
            Password = password;
            Server = server;

            _logger = new FileLogger(!string.IsNullOrEmpty(Name) ? Name! : Email!, "execution");
            _logger.WriteConsole = true;
        }

        public class ExistingSession
        {
            public string SessionId { get; set; } = "";
            public int UserId { get; set; }
            public string ClientId { get; set; } = "";
        }

        public enum AccountStatus
        {
            Undetermined,
            Unauthorized,  // Account was logged in, but there was a login from other device

            DoesNotExist,  // Game declared that this account doesn't exist
            Suspended,     // Game declared that this account is temporaily banned
            Deleted,       // Game declared that this account was permamently banned

            Ready,         // Account is logged in, and ready to run routines
            Active         // Account is running routine
        }
    }
}
