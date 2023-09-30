using Android.Runtime;
using ExponeaSdk.Services;

namespace ExampleApp
{
    [Register("ExampleApp.ExampleAuthProvider")]
    public class ExampleAuthProvider : Java.Lang.Object, IAuthorizationProvider
    {
        public ExampleAuthProvider()
        {
        }

        public string AuthorizationToken => CustomerTokenStorage.INSTANCE.RetrieveJwtToken();

    }

}
