using System;
using Mono.Addins.Description;
using MonoDevelop.Ide.Gui.Components;
using Gtk;
using MonoDevelop.Components;

namespace MonoDevelop.AddinMaker.AddinBrowser
{
	class ExtensionPointNodeBuilder : TypeNodeBuilder, ITreeDetailBuilder
	{
		public override Type NodeDataType {
			get { return typeof(ExtensionPoint); }
		}

		public override string GetNodeName (ITreeNavigator thisNode, object dataObject)
		{
			var extensionPoint = (ExtensionPoint)dataObject;
			return extensionPoint.Path;
		}

		public override void BuildNode (ITreeBuilder treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			var extensionPoint = (ExtensionPoint)dataObject;
			nodeInfo.Label = extensionPoint.Path;
		}

		public Control GetDetailWidget (object dataObject)
		{
			return new ExtensionPointDetailWidget ((ExtensionPoint)dataObject);
		}
	}

	class ExtensionPointDetailWidget : AppKit.NSStackView
	{
		public ExtensionPoint ExtensionPoint { get; private set; }

		public ExtensionPointDetailWidget (ExtensionPoint ep)
		{
			this.ExtensionPoint = ep;
			Distribution = AppKit.NSStackViewDistribution.Fill;
			Orientation = AppKit.NSUserInterfaceLayoutOrientation.Vertical;
			TranslatesAutoresizingMaskIntoConstraints = false;
			var label = new AppKit.NSTextField () {
				TranslatesAutoresizingMaskIntoConstraints = false,
				Bezeled = false, DrawsBackground = false, Editable = false, Selectable = false,
				StringValue = string.Format ("<big><tt>{0}</tt></big>\n{1}\n{2}", ep.Path, ep.Name, ep.Description)
			};
			AddArrangedSubview (label);
		}
	}
}