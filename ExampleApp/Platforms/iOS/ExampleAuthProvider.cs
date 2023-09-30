using System;
using Foundation;
using ObjCRuntime;
using UIKit;
using UserNotifications;

namespace ExampleApp
{
    [Register("ExponeaAuthProvider")]
    public class ExampleAuthProvider : BloomreachSdkNativeiOS.MauiAuthorizationProvider
    {
        public ExampleAuthProvider()
        {
        }

        public override string AuthorizationToken => CustomerTokenStorage.INSTANCE.RetrieveJwtToken();
    }

}
