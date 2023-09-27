using System;
using AVFoundation;
using Foundation;
using ObjCRuntime;
using UIKit;
using UserNotifications;

namespace BloomreachSdkNativeiOS
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

	// @interface BloomreachMauiVersion : NSObject
	[BaseType (typeof(NSObject))]
	interface BloomreachMauiVersion
	{
	}

	// @interface BloomreachSdkIOS : NSObject
    [BaseType (typeof(NSObject))]
	interface BloomreachSdkIOS
	{
        // @property (readonly, nonatomic, strong, class) BloomreachSdkIOS * _Nonnull instance;
        [Static]
        [Export ("instance", ArgumentSemantic.Strong)]
        BloomreachSdkIOS Instance { get; }

		// -(MethodResult * _Nonnull)invokeMethodWithMethod:(NSString * _Nullable)method params:(NSString * _Nullable)params __attribute__((warn_unused_result("")));
		[Export ("invokeMethodWithMethod:params:")]
		MethodResult InvokeMethodWithMethod ([NullAllowed] string method, [NullAllowed] string @params);

		// -(void)invokeMethodAsyncWithMethod:(NSString * _Nullable)method params:(NSString * _Nullable)params done:(void (^ _Nonnull)(MethodResult * _Nonnull))done;
		[Export ("invokeMethodAsyncWithMethod:params:done:")]
		void InvokeMethodAsyncWithMethod ([NullAllowed] string method, [NullAllowed] string @params, Action<MethodResult> done);

		// -(MethodResultForUI * _Nonnull)invokeMethodForUIWithMethod:(NSString * _Nullable)method params:(NSString * _Nullable)params __attribute__((warn_unused_result("")));
		[Export ("invokeMethodForUIWithMethod:params:")]
		MethodResultForUI InvokeMethodForUIWithMethod ([NullAllowed] string method, [NullAllowed] string @params);

		// -(MethodResult * _Nonnull)handleRemoteMessageWithNotificationRequest:(UNNotificationRequest * _Nonnull)notificationRequest handler:(void (^ _Nonnull)(UNNotificationContent * _Nonnull))handler __attribute__((warn_unused_result("")));
		[Export ("handleRemoteMessageWithNotificationRequest:handler:")]
		MethodResult HandleRemoteMessageWithNotificationRequest (UNNotificationRequest notificationRequest, Action<UNNotificationContent> handler);
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
