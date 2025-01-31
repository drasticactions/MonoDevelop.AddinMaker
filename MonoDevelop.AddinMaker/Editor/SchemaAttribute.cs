//
// SchemaItem.cs
//
// Author:
//       Mikayla Hutchinson <m.j.hutchinson@gmail.com>
//
// Copyright (c) 2015 Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//using MonoDevelop.Ide.CodeCompletion;
//using MonoDevelop.Xml.Dom;

//namespace MonoDevelop.AddinMaker.Editor
//{
//	class SchemaAttribute
//	{
//		public string Name { get; private set; }
//		public string Description { get; private set; }

//		/// <summary>
//		/// Name of attributes that cannot be specified in conjection with this one.
//		/// </summary>
//		public string[] Exclude { get; set; }

//		public SchemaAttribute (string name, string description, string[] exclude = null)
//		{
//			this.Name = name;
//			this.Description = description;
//			this.Exclude = exclude;
//		}

//		//public virtual void GetAttributeValueCompletions (CompletionDataList list, IAttributedXObject attributedOb)
//		//{
//		//}
//	}

//	class BoolSchemaAttribute : SchemaAttribute
//	{
//		public BoolSchemaAttribute (string name, string description, string[] exclude = null) : base (name, description, exclude)
//		{
//		}

//		//public override void GetAttributeValueCompletions (CompletionDataList list, IAttributedXObject attributedOb)
//		//{
//		//	list.Add ("true");
//		//	list.Add ("false");
//		//}
//	}
//}
