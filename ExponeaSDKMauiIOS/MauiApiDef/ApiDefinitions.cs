using System;
using AVFoundation;
using Foundation;
using ObjCRuntime;
using UIKit;
using UserNotifications;

namespace ExponeaSdkNativeiOS
{
	// @protocol AuthorizationProviderType
	[Protocol]
	interface AuthorizationProviderType
	{
		// @required -(NSString * _Nullable)getAuthorizationToken __attribute__((warn_unused_result("")));
		[Abstract]
		[NullAllowed, Export ("getAuthorizationToken")]
		string AuthorizationToken { get; }
	}

	// @interface ExponeaMauiVersion : NSObject
	[BaseType (typeof(NSObject))]
	interface ExponeaMauiVersion
	{
	}

	// @interface ExponeaSDK : NSObject
    [BaseType (typeof(NSObject))]
	interface ExponeaSDK
	{
        // @property (readonly, nonatomic, strong, class) Exponea * _Nonnull instance;
        [Static]
        [Export ("instance", ArgumentSemantic.Strong)]
        ExponeaSDK Instance { get; }

		// -(MethodResult * _Nonnull)invokeMethodWithMethod:(NSString * _Nullable)method params:(NSString * _Nullable)params __attribute__((warn_unused_result("")));
		[Export ("invokeMethodWithMethod:params:")]
		MethodResult InvokeMethodWithMethod ([NullAllowed] string method, [NullAllowed] string @params);

		// -(void)invokeMethodAsyncWithMethod:(NSString * _Nullable)method params:(NSString * _Nullable)params done:(void (^ _Nonnull)(MethodResult * _Nonnull))done;
		[Export ("invokeMethodAsyncWithMethod:params:done:")]
		void InvokeMethodAsyncWithMethod ([NullAllowed] string method, [NullAllowed] string @params, Action<MethodResult> done);

		// -(MethodResultForUI * _Nonnull)invokeMethodForUIWithMethod:(NSString * _Nullable)method params:(NSString * _Nullable)params __attribute__((warn_unused_result("")));
		[Export ("invokeMethodForUIWithMethod:params:")]
		MethodResultForUI InvokeMethodForUIWithMethod ([NullAllowed] string method, [NullAllowed] string @params);
	}

	// @interface MauiAuthorizationProvider : NSObject
	[BaseType(typeof(NSObject))]
	interface MauiAuthorizationProvider
	{
		// -(NSString * _Nullable)getAuthorizationToken __attribute__((warn_unused_result("")));
		[NullAllowed, Export("getAuthorizationToken")]
		string AuthorizationToken { get; }
	}

	// @interface MethodResult : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface MethodResult
	{
		// @property (readonly, nonatomic) (BOOL) success;
		[Export ("success")]
		bool Success { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull data;
		[Export ("data")]
		string Data { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull error;
		[Export ("error")]
		string Error { get; }
	}
	
	// @interface MethodResultForUI : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface MethodResultForUI
	{
		// @property (readonly, nonatomic) (BOOL) success;
		[Export ("success")]
		bool Success { get; }

		// @property (readonly, nonatomic, strong) UIView * _Nullable data;
		[NullAllowed, Export ("data", ArgumentSemantic.Strong)]
		UIView Data { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull error;
		[Export ("error")]
		string Error { get; }
	}
}
