using AppKit;
using Foundation;
using MonoDevelop.Core;
using ObjCRuntime;

namespace MonoDevelop.AddinMaker
{
	class OkCancelDialog : NSWindow
	{
		bool handlesClose = false;

		class StackViewDialogDelegate : NSWindowDelegate
		{
			public override void WillClose (NSNotification notification)
			{
				if (notification.Object is OkCancelDialog dialog) {
					dialog.OnDialogWillClose ();
				}
			}
		}

		protected virtual void OnDialogWillClose ()
		{
			if (!handlesClose) {
				NSApplication.SharedApplication.AbortModal ();
			}
		}

		protected NSStackView contentStackView;

		const string OkSelectorName = "stackViewDialogOkButtonActivated:";
		const string BackSelectorName = "stackViewDialogBackButtonActivated:";


		public override void KeyDown (NSEvent theEvent)
		{
			if ((theEvent.KeyCode == (ushort)NSKey.Escape) && ChildWindows.Length == 0) {
				PerformClose (this);
				return;
			}
			base.KeyDown (theEvent);
		}

		public OkCancelDialog (float padding = 15)
		{
			this.StyleMask |= NSWindowStyle.Closable;
			this.StandardWindowButton (NSWindowButton.ZoomButton).Enabled = false;

			Delegate = new StackViewDialogDelegate ();

			var stackView = new StackView (spacing: 0);
			ContentView = stackView;

			contentStackView = new StackView ();
			stackView.AddArrangedSubview (contentStackView);
			contentStackView.EdgeInsets = new NSEdgeInsets (padding, 0, 0, 0);

			contentStackView.LeadingAnchor.ConstraintEqualToAnchor (stackView.LeadingAnchor, padding).Active = true;
			contentStackView.TrailingAnchor.ConstraintEqualToAnchor (stackView.TrailingAnchor, -padding).Active = true;

			var buttonsRow = new StackView (NSUserInterfaceLayoutOrientation.Horizontal);
			stackView.AddArrangedSubview (buttonsRow);
			buttonsRow.LeadingAnchor.ConstraintEqualToAnchor (stackView.LeadingAnchor, padding).Active = true;
			buttonsRow.TrailingAnchor.ConstraintEqualToAnchor (stackView.TrailingAnchor, -padding).Active = true;

			var buttonsTrailingHeightConstraint = buttonsRow.HeightAnchor.ConstraintEqualToConstant (43);
			buttonsTrailingHeightConstraint.Active = true;

			buttonsRow.AddArrangedSubview (new NSView () { TranslatesAutoresizingMaskIntoConstraints = false });

			backButton = new AppKit.NSButton ();
			backButton.BezelStyle = NSBezelStyle.Rounded;
			backButton.Title = GettextCatalog.GetString ("Cancel");
			backButton.TranslatesAutoresizingMaskIntoConstraints = false;
			backButton.Target = this;
			backButton.Action = new Selector (BackSelectorName);

			buttonsRow.AddArrangedSubview (backButton);
			var backButtonWidthConstraint = backButton.WidthAnchor.ConstraintEqualToConstant (82f);
			backButtonWidthConstraint.Priority = (System.Int32)AppKit.NSLayoutPriority.DefaultLow;
			backButtonWidthConstraint.Active = true;

			okButton = new AppKit.NSButton ();
			okButton.BezelStyle = NSBezelStyle.Rounded;
			okButton.Font = AppKit.NSFont.SystemFontOfSize (AppKit.NSFont.SystemFontSize);
			okButton.Title = GettextCatalog.GetString ("OK");
			okButton.KeyEquivalent = "\r";
			okButton.TranslatesAutoresizingMaskIntoConstraints = false;

			buttonsRow.AddArrangedSubview (okButton);
			var actionButtonWidthConstraint = okButton.WidthAnchor.ConstraintEqualToConstant (82f);
			actionButtonWidthConstraint.Priority = (System.Int32)AppKit.NSLayoutPriority.DefaultLow;
			actionButtonWidthConstraint.Active = true;

			okButton.Target = this;
			okButton.Action = new Selector (OkSelectorName);

			buttonsRow.EdgeInsets = new NSEdgeInsets (padding, 0, padding, 0);
		}

		public string OkButtonTitle {
			get => okButton.Title;
			set => okButton.Title = value;
		}

		protected AppKit.NSButton okButton, backButton;

		protected virtual bool OnValidated ()
		{
			return true;
		}

		[Export (OkSelectorName)]
		protected virtual void OnOkButtonActivated (NSObject target)
		{
			if (OnValidated ()) {
				// Validate changes before saving
				handlesClose = true;
				NSApplication.SharedApplication.StopModalWithCode ((int)NSModalResponse.OK);
				Close ();
			}
		}

		[Export (BackSelectorName)]
		protected virtual void OnBackButtonActivated (NSObject target)
		{
			handlesClose = true;
			NSApplication.SharedApplication.StopModalWithCode ((int)NSModalResponse.Cancel);
			Close ();
		}
	}
}