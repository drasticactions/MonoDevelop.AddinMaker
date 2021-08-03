using System;
using Mono.Addins.Description;
using MonoDevelop.Ide.Gui.Components;
using Gtk;
using MonoDevelop.Components;

namespace MonoDevelop.AddinMaker.AddinBrowser
{
	class ExtensionNodeBuilder : TypeNodeBuilder, ITreeDetailBuilder
	{
		public override Type NodeDataType {
			get { return typeof(Extension); }
		}

		public override string GetNodeName (ITreeNavigator thisNode, object dataObject)
		{
			var extension = (Extension)dataObject;
			return extension.Path;
		}

		public override void BuildNode (ITreeBuilder treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			var extension = (Extension)dataObject;
			nodeInfo.Label = extension.Path;
		}

		public Control GetDetailWidget (object dataObject)
		{
			return new ExtensionDetailWidget ((Extension)dataObject);
		}
	}

	class ExtensionDetailWidget : AppKit.NSStackView
	{
		public Extension Extension { get; private set; }

		public ExtensionDetailWidget (Extension ext)
		{
			this.Extension = ext;
			Distribution = AppKit.NSStackViewDistribution.Fill;
			Orientation = AppKit.NSUserInterfaceLayoutOrientation.Vertical;
			TranslatesAutoresizingMaskIntoConstraints = false;
			var label = new AppKit.NSTextField () {
				TranslatesAutoresizingMaskIntoConstraints = false,
				Bezeled = false, DrawsBackground = false, Editable = false, Selectable = false,
				StringValue = ext.Path
			};
			AddArrangedSubview (label);
		}
	}
}
