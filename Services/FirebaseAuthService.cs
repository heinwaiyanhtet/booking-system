using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace BookingSystem.Services
{
    public class FirebaseAuthService
    {
        private readonly FirebaseAuth _auth;
        public FirebaseAuthService()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                try
                {
                    FirebaseApp.Create();
                }
                catch
                {
                    // ignore initialization errors in sample
                }
            }
            _auth = FirebaseAuth.DefaultInstance;
        }

        public async Task<FirebaseToken?> VerifyIdTokenAsync(string idToken)
        {
            try
            {
                return await _auth.VerifyIdTokenAsync(idToken);
            }
            catch
            {
                return null;
            }
        }
    }
}