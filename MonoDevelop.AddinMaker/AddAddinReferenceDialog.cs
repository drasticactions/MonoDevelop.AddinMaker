using System;
using System.Collections.Generic;
using System.Linq;
using AppKit;
using CoreGraphics;
using Foundation;
using Mono.Addins;
using MonoDevelop.Core;
using ObjCRuntime;

namespace MonoDevelop.AddinMaker
{
    class AddinValueCell : StackView
    {
        public override CGSize IntrinsicContentSize {
            get {
                var textView = (NSTextField)Subviews [0];
                return new CGSize (textView.IntrinsicContentSize.Width, textView.IntrinsicContentSize.Height + 5);
            }
        }

        NSButton checkBox;
        NSTextField titleTextField, subTitleTextField;

        AddAddinReferenceViewDialog dialog => (AddAddinReferenceViewDialog) Window;

        public AddinValueCell () : base (NSUserInterfaceLayoutOrientation.Horizontal)
        {
            checkBox = new NSButton () { TranslatesAutoresizingMaskIntoConstraints = false, Title = "" };
            checkBox.SetButtonType (NSButtonType.Switch);
            AddArrangedSubview (checkBox);

            var verticalTextStack = new StackView (NSUserInterfaceLayoutOrientation.Vertical, 5);
            AddArrangedSubview (verticalTextStack);

            titleTextField = new NSTextField () {
                TranslatesAutoresizingMaskIntoConstraints = false,
                DrawsBackground = false,
                Editable = false,
                Bezeled = false,
                Selectable = false,
                Font = NSFont.BoldSystemFontOfSize (NSFont.SystemFontSize)
            };
            verticalTextStack.AddArrangedSubview (titleTextField);

            subTitleTextField = new NSTextField () {
                TranslatesAutoresizingMaskIntoConstraints = false,
                DrawsBackground = false,
                Editable = false,
                Bezeled = false,
                Selectable = false,
                Font = NSFont.SystemFontOfSize (NSFont.SystemFontSize),
                AlphaValue = 0.7f
            };
            verticalTextStack.AddArrangedSubview (subTitleTextField);

            checkBox.Target = this;
            checkBox.Action = new Selector (Selector);
        }

        [Export (Selector)]
        void DockButtonActivated (NSObject sender)
        {
            var button = (NSButton)sender;
            data.Value = button.State == NSCellStateValue.On;

            dialog.OnButtonActivated ();
        }

        const string Selector = "customSelectorName:";

        public void RefreshStates ()
        {
            NeedsDisplay = true;
        }

        AddinValue data;
        internal void SetData (AddinValue data)
        {
            this.data = data;
            titleTextField.StringValue = AddinHelpers.GetUnversionedId (data.Addin);
            subTitleTextField.StringValue = data.Addin.Name;
            checkBox.State = data.Value ? NSCellStateValue.On : NSCellStateValue.Off;
        }
    }

    class AddinReferenceTableView : NSTableView
    {
        const string ColumnName = "DataColumn";
        const string ColumnTitle = "Reference";

        public override bool IsFlipped => true;

        public AddinReferenceTableView ()
        {
            BackgroundColor = NSColor.Clear;
            var column = new NSTableColumn (ColumnName);
            column.Title = ColumnTitle;
            AddColumn (column);

            RowHeight = 50;

            HeaderView = null;

            DataSource = new AddinReferenceTableViewDataSource ();
            Delegate = new AddinReferenceTableViewDelegate ();
        }

        public void Clear ()
        {
            ((AddinReferenceTableViewDataSource)DataSource).Clear ();
            ReloadData ();
        }

        public List<AddinValue> Data {
            get => ((AddinReferenceTableViewDataSource)DataSource).Data;
            set {
                ((AddinReferenceTableViewDataSource)DataSource).Data = value;
                ReloadData ();
            }
        }

        public AddinValue DataForRow (int row)
        {
            var data = Data;
            if (row < 0 && row > data.Count - 1)
                return null;

            return data [row];
        }

        class BuildConfigurationsTableRowView : NSTableRowView
        {
            NSView cell;

            public override bool Selected {
                get {
                    return base.Selected;
                }
                set {
                    base.Selected = value;
                    NeedsDisplay = true;
                }
            }

            public override void AddSubview (NSView aView)
            {
                base.AddSubview (aView);
                this.cell = aView;
            }
        }

        internal class AddinReferenceTableViewDelegate : NSTableViewDelegate
        {
            public AddinReferenceTableViewDelegate ()
            {
            }

            const string identifer = "myCellIdentifier";
            public override NSView GetViewForItem (NSTableView tableView, NSTableColumn tableColumn, nint row)
            {
                var table = (AddinReferenceTableView)tableView;

                AddinValueCell view = tableView.MakeView (identifer, this) as AddinValueCell;
                if (view == null) {
                    view = new AddinValueCell() {
                        Identifier = identifer
                    };
                }

                var data = table.DataForRow ((int)row);
                if (data != null) {
                    view.SetData (data);
                }

                return view;
            }

            int GetColumnIndex (NSTableView tableView, NSTableColumn tableColumn)
            {
                return (int)tableView.FindColumn ((NSString)tableColumn.Identifier);
            }

            public override NSTableRowView CoreGetRowView (NSTableView tableView, nint row)
            {
                return tableView.GetRowView (row, false) ?? new BuildConfigurationsTableRowView ();
            }
        }

        internal class AddinReferenceTableViewDataSource : NSTableViewDataSource
        {
            readonly List<AddinValue> data = new List<AddinValue> ();
            internal List<AddinValue> Data {
                get => data;
                set {
                    if (data == value)
                        return;

                    this.data.Clear ();
                    this.data.AddRange (value);
                }
            }

            public override nint GetRowCount (NSTableView tableView)
                => data.Count;

            internal void Clear () => data.Clear ();
        }
    }

    class AddinValue
    {
        bool initValue;

        public Addin Addin { get; private set; }
        public bool Value { get; set; }

        public bool HasChanged => initValue != Value;

        public AddinValue(Addin addin, bool initValue)
        {
            this.Addin = addin;
            this.initValue = Value = initValue;
        }
    }

    class AddAddinReferenceViewDialog : OkCancelDialog
    {
        AddinReferenceTableView tableView;
        readonly List<AddinValue> store = new List<AddinValue> ();

        public AddAddinReferenceViewDialog (Addin [] allAddins)
        {
            if (allAddins == null || allAddins.Length == 0)
                throw new ArgumentException ();

            Title = GettextCatalog.GetString ("Add Extension Reference");

            var frame = Frame;
            frame.Size = new CoreGraphics.CGSize (481f, 380f);
            this.SetFrame (frame, true);
            this.ContentMinSize = this.ContentView.Frame.Size;

            tableView = new AddinReferenceTableView ();

            var scrollView = new NSScrollView () {
                TranslatesAutoresizingMaskIntoConstraints = false,
                HasVerticalScroller = true,
            };
            scrollView.DocumentView = tableView;

            contentStackView.AddArrangedSubview (scrollView);
            scrollView.LeadingAnchor.ConstraintEqualToAnchor(contentStackView.LeadingAnchor).Active = true;
            scrollView.TrailingAnchor.ConstraintEqualToAnchor (contentStackView.TrailingAnchor).Active = true;

            scrollView.BorderType = NSBorderType.LineBorder;

            foreach (var addin in allAddins.OrderBy (a => a.Id)) {
                store.Add (new AddinValue(addin, false));
            }

            tableView.Data = store;

            okButton.Enabled = false;
        }

        public Addin [] GetSelectedAddins ()
        {
            return store.Where (s => s.Value).Select(s => s.Addin).ToArray();
        }

        internal void OnButtonActivated ()
        {
            okButton.Enabled = store.Any (s => s.HasChanged);
        }
    }
}