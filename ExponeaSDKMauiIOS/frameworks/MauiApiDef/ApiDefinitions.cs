using ExponeaSDKMauiIOS;
using Foundation;

namespace ExponeaSdk
{
	// @protocol AuthorizationProviderType
	/*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/[Protocol]
	interface AuthorizationProviderType
	{
		// @required -(NSString * _Nullable)getAuthorizationToken __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export ("getAuthorizationToken")]
		[Verify (MethodToProperty)]
		string AuthorizationToken { get; }
	}

	// @interface ExponeaMauiVersion
	interface ExponeaMauiVersion
	{
	}

	// @interface ExponeaSDK
	interface ExponeaSDK
	{
		// -(MethodResult * _Nonnull)invokeMethodWithMethod:(NSString * _Nullable)method params:(NSString * _Nullable)params __attribute__((warn_unused_result("")));
		[Export ("invokeMethodWithMethod:params:")]
		MethodResult InvokeMethodWithMethod ([NullAllowed] string method, [NullAllowed] string @params);
	}

	// @interface MauiAuthorizationProvider <AuthorizationProviderType>
	interface MauiAuthorizationProvider : IAuthorizationProviderType
	{
		// -(NSString * _Nullable)getAuthorizationToken __attribute__((warn_unused_result("")));
		[NullAllowed, Export ("getAuthorizationToken")]
		[Verify (MethodToProperty)]
		string AuthorizationToken { get; }
	}

	// @interface MethodResult
	[DisableDefaultCtor]
	interface MethodResult
	{
		// @property (readonly, nonatomic) int success;
		[Export ("success")]
		int Success { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull data;
		[Export ("data")]
		string Data { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull error;
		[Export ("error")]
		string Error { get; }
	}
}
