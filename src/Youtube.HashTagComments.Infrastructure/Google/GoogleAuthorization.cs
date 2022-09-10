using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Youtube.HashTagComments.Core.Exceptions;

namespace Youtube.HashTagComments.Infrastructure.Google
{
    public class GoogleAuthorization
    {
        private readonly ILogger<GoogleAuthorization> _logger;

        private readonly string[] _scopes;
        private readonly string _user;
        private readonly FileDataStore _dataStore;

        public GoogleAuthorization(ILogger<GoogleAuthorization> logger,
                                   IConfiguration configuration)
        {
            _logger = logger;

            _scopes = new[] { YouTubeService.Scope.YoutubeReadonly };
            _user = configuration["Google:User"];
            _dataStore = new FileDataStore(this.GetType().ToString());
        }

        public async Task<UserCredential> Authorize()
        {
            try
            {
                return await TryAuthorize();
            }
            catch(UnauthorizedUserException ex)
            {
                _logger.LogWarning(ex.Message);

                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Unexpected exception", ex);

                throw;
            }
        }

        private async Task<UserCredential> TryAuthorize()
        {
            using var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read);

            var secrets = await GoogleClientSecrets.FromStreamAsync(stream);

            var credentials = await GoogleWebAuthorizationBroker.AuthorizeAsync(secrets.Secrets, _scopes, _user, CancellationToken.None, _dataStore);

            if (credentials is null)
                throw new UnauthorizedUserException();

            return credentials;
        }
    }
}
