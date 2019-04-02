﻿//
// IntentsUI bindings
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#if XAMCORE_2_0 // The IntentsUI framework relies on Intents which is only available in Unified

using System;
using CoreGraphics;
using Foundation;
using Intents;
using ObjCRuntime;
using UIKit;

namespace IntentsUI {

	[iOS (10, 0)]
	[Native]
	public enum INUIHostedViewContext : ulong {
		SiriSnippet,
		MapsCard
	}

	[iOS (11,0)]
	[Native]
	public enum INUIInteractiveBehavior : ulong {
		None,
		NextView,
		Launch,
		GenericAction,
	}

	[NoWatch, NoTV, NoMac, iOS (12,0)]
	[Native]
	public enum INUIAddVoiceShortcutButtonStyle : ulong {
		White = 0,
		WhiteOutline,
		Black,
		BlackOutline,
	}

	[iOS (11,0)]
	delegate void INUIHostedViewControllingConfigureViewHandler (bool success, NSSet<INParameter> configuredParameters, CGSize desiredSize);

	[iOS (10, 0)]
	[Protocol]
	interface INUIHostedViewControlling {

#if !XAMCORE_4_0 // Apple made this member optional in iOS 11
		[Abstract]
#endif
		[Export ("configureWithInteraction:context:completion:")]
		void Configure (INInteraction interaction, INUIHostedViewContext context, Action<CGSize> completion);

		[iOS (11,0)]
		[Export ("configureViewForParameters:ofInteraction:interactiveBehavior:context:completion:")]
		void ConfigureView (NSSet<INParameter> parameters, INInteraction interaction, INUIInteractiveBehavior interactiveBehavior, INUIHostedViewContext context, INUIHostedViewControllingConfigureViewHandler completionHandler);
	}

	[iOS (10, 0)]
	[Category]
	[BaseType (typeof (NSExtensionContext))]
	interface NSExtensionContext_INUIHostedViewControlling {

		[Export ("hostedViewMinimumAllowedSize")]
		CGSize GetHostedViewMinimumAllowedSize ();

		[Export ("hostedViewMaximumAllowedSize")]
		CGSize GetHostedViewMaximumAllowedSize ();

		[iOS (11,0)]
		[Export ("interfaceParametersDescription")]
		string GetInterfaceParametersDescription ();
	}

	[iOS (10, 0)]
	[Protocol]
	interface INUIHostedViewSiriProviding {

		[Export ("displaysMap")]
		bool DisplaysMap { get; }

		[Export ("displaysMessage")]
		bool DisplaysMessage { get; }

		[Export ("displaysPaymentTransaction")]
		bool DisplaysPaymentTransaction { get; }
	}

	[iOS (12,0)]
	[BaseType (typeof (UIViewController))]
	[DisableDefaultCtor]
	interface INUIAddVoiceShortcutViewController {

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IINUIAddVoiceShortcutViewControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("initWithShortcut:")]
		IntPtr Constructor (INShortcut shortcut);
	}

	interface IINUIAddVoiceShortcutViewControllerDelegate { }

	[iOS (12,0)]
	[Protocol, Model (AutoGeneratedName = true)]
	[BaseType (typeof (NSObject))]
	interface INUIAddVoiceShortcutViewControllerDelegate {

		[Abstract]
		[Export ("addVoiceShortcutViewController:didFinishWithVoiceShortcut:error:")]
		void DidFinish (INUIAddVoiceShortcutViewController controller, [NullAllowed] INVoiceShortcut voiceShortcut, [NullAllowed] NSError error);

		[Abstract]
		[Export ("addVoiceShortcutViewControllerDidCancel:")]
		void DidCancel (INUIAddVoiceShortcutViewController controller);
	}

	[iOS (12,0)]
	[BaseType (typeof (UIViewController))]
	[DisableDefaultCtor]
	interface INUIEditVoiceShortcutViewController {

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IINUIEditVoiceShortcutViewControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("initWithVoiceShortcut:")]
		IntPtr Constructor (INVoiceShortcut voiceShortcut);
	}

	interface IINUIEditVoiceShortcutViewControllerDelegate { }

	[iOS (12,0)]
	[Protocol, Model (AutoGeneratedName = true)]
	[BaseType (typeof (NSObject))]
	interface INUIEditVoiceShortcutViewControllerDelegate {

		[Abstract]
		[Export ("editVoiceShortcutViewController:didUpdateVoiceShortcut:error:")]
		void DidUpdate (INUIEditVoiceShortcutViewController controller, [NullAllowed] INVoiceShortcut voiceShortcut, [NullAllowed] NSError error);

		[Abstract]
		[Export ("editVoiceShortcutViewController:didDeleteVoiceShortcutWithIdentifier:")]
		void DidDelete (INUIEditVoiceShortcutViewController controller, NSUuid deletedVoiceShortcutIdentifier);

		[Abstract]
		[Export ("editVoiceShortcutViewControllerDidCancel:")]
		void DidCancel (INUIEditVoiceShortcutViewController controller);
	}

	[NoWatch, NoTV, NoMac, iOS (12,0)]
	[BaseType (typeof (UIButton))]
	[DisableDefaultCtor]
	interface INUIAddVoiceShortcutButton {

		[Export ("initWithStyle:")]
		IntPtr Constructor (INUIAddVoiceShortcutButtonStyle style);

		[Export ("style")]
		INUIAddVoiceShortcutButtonStyle Style { get; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IINUIAddVoiceShortcutButtonDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[NullAllowed, Export ("shortcut", ArgumentSemantic.Strong)]
		INShortcut Shortcut { get; set; }

		[iOS (12,2)]
		[Export ("cornerRadius", ArgumentSemantic.Assign)]
		nfloat CornerRadius { get; set; }
	}

	interface IINUIAddVoiceShortcutButtonDelegate {}

	[NoWatch, NoTV, NoMac, iOS (12,0)]
	[Protocol, Model (AutoGeneratedName = true)]
	[BaseType (typeof(NSObject))]
	interface INUIAddVoiceShortcutButtonDelegate {

		[Abstract]
		[Export ("presentAddVoiceShortcutViewController:forAddVoiceShortcutButton:")]
		void PresentAddVoiceShortcut (INUIAddVoiceShortcutViewController addVoiceShortcutViewController, INUIAddVoiceShortcutButton addVoiceShortcutButton);

		[Abstract]
		[Export ("presentEditVoiceShortcutViewController:forAddVoiceShortcutButton:")]
		void PresentEditVoiceShortcut (INUIEditVoiceShortcutViewController editVoiceShortcutViewController, INUIAddVoiceShortcutButton addVoiceShortcutButton);
	}
}

#endif
