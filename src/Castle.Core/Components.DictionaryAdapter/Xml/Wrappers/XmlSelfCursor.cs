﻿// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.f
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.Components.DictionaryAdapter.Xml
{
    using System;
	using System.Xml;

    public class XmlSelfCursor : IXmlCursor //, IXmlTypeMap
    {
        private readonly IXmlNode node;
		private readonly Type clrType;
        private int position;

        public XmlSelfCursor(IXmlNode node, Type clrType)
        {
            this.node    = node;
			this.clrType = clrType;
			Reset();
        }

		public CursorFlags Flags
		{
			get { return node.IsAttribute ? CursorFlags.Attributes : CursorFlags.Elements; }
		}

		public string LocalName
		{
			get { return node.LocalName; }
		}

		public string NamespaceUri
		{
			get { return node.NamespaceUri; }
		}

		public string XsiType
		{
			get { return node.XsiType; }
		}

		public Type ClrType
		{
			get { return clrType ?? node.ClrType; }
		}

		public bool IsElement
		{
			get { return node.IsElement; }
		}

		public bool IsAttribute
		{
			get { return node.IsAttribute; }
		}

		public bool IsRoot
		{
			get { return node.IsRoot; }
		}

		public bool IsNil
		{
			get { return node.IsNil; }
			set { node.IsNil = value; }
		}

		public string Value
		{
			get { return node.Value; }
			set { node.Value = value; }
		}

		public string Xml
		{
			get { return node.Xml; }
		}

        public bool MoveNext()
        {
            return 0 == ++position;
        }

		public void MoveToEnd()
		{
			position = 1;
		}

		public void Reset()
		{
			position = -1;
		}

		public void MoveTo(IXmlNode position)
		{
			if (position != node)
				throw Error.NotSupported();
		}

		public IXmlNode Save()
		{
			return node;
		}

		public IXmlCursor SelectSelf(Type clrType)
		{
			return new XmlSelfCursor(node, clrType);
		}

		public IXmlCursor SelectChildren(IXmlTypeMap knownTypes, CursorFlags flags)
		{
			return node.SelectChildren(knownTypes, flags);
		}

#if !SL3
		public IXmlCursor Select(ICompiledPath path, IXmlTypeMap knownTypes, CursorFlags flags)
		{
			return node.Select(path, knownTypes, flags);
		}

		public object Evaluate(ICompiledPath path)
		{
			return node.Evaluate(path);
		}
#endif

		public XmlReader ReadSubtree()
		{
			return node.ReadSubtree();
		}

		public XmlWriter WriteAttributes()
		{
			return node.WriteAttributes();
		}

		public XmlWriter WriteChildren()
		{
			return node.WriteChildren();
		}

		public void MakeNext(Type type)
		{
			throw Error.NotSupported();
		}

		public void Create(Type type)
		{
			throw Error.NotSupported();
		}

		public void Coerce(Type type)
		{
			throw Error.NotSupported();
		}

		public void Clear()
		{
			node.Clear();
		}

		public void Remove()
		{
			throw Error.NotSupported();
		}

		public void RemoveToEnd()
		{
			throw Error.NotSupported();
		}
	}
}
