﻿using MonoDevelop.AddinMaker.Editor.ManifestSchema;

namespace MonoDevelop.AddinMaker.Editor
{
	class AddinManifestEditorExtension : SchemaBasedEditorExtension
	{
		public override bool IsValidInContext (MonoDevelop.Ide.Editor.DocumentContext context)
		{
			return base.IsValidInContext (context) && context.HasProject && context.Project.HasFlavor<AddinProjectFlavor> ();
		}

		protected override SchemaElement CreateSchema ()
		{
			var project = DocumentContext.Project.GetFlavor<AddinProjectFlavor> ();

			var addinContents = new SchemaElement[] {
				new RuntimeSchemaElement (),
				new DependenciesSchemaElement (),
				new LocalizerSchemaElement (),
				new ExtensionSchemaElement (project),
				new ExtensionPointSchemaElement (project),
				new SchemaElement ("ExtensionNodeSet", "Declares an extension node set"),
				new SchemaElement ("ConditionType", "Declares a global condition type"),
			};

			return new SchemaElement (null, null, new[] {
				new SchemaElement (
					"Addin",
					"Root element for add-in and add-in root descriptions",
					addinContents,
					new[] {
						new SchemaAttribute ("id", "The identifier of the add-in. It is mandatory for add-in roots and for add-ins that can be extended, optional for other add-ins."),
						new SchemaAttribute ("namespace", "Namespace of the add-in. The full ID of an add-in is composed by 'namespace.name'."),
						new SchemaAttribute ("version", "The version of the add-in. It is mandatory for add-in roots and for add-ins that can be extended."),
						new SchemaAttribute ("compatVersion", "Version of the add-in with which this add-in is backwards compatible (optional)."),
						new SchemaAttribute ("name", "Display name of the add-in."),
						new SchemaAttribute ("description", "Description of the add-in."),
						new SchemaAttribute ("author", "Author of the add-in."),
						new SchemaAttribute ("url", "Url of a web page with more information about the add-in."),
						new SchemaAttribute ("defaultEnabled", "When set to 'false', the add-in won't be enabled until it is explicitly enabled by the user. The default is 'true'."),
						//TODO: enable this if we ever support arbitrary addins
						//new SchemaAttribute ("isroot", "Must be true if this manifest belongs to an add-in root.")
					}
				),
				new SchemaElement ("ExtensionModel", "Root element for add-in and add-in root descriptions", addinContents)
			});
		}
	}
}
