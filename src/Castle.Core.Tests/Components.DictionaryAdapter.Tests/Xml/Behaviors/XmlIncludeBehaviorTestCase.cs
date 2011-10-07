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
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.Components.DictionaryAdapter.Xml.Tests
{
	using System;
	using System.Xml.Serialization;
	using NUnit.Framework;

	public class XmlIncludeBehaviorTestCase
	{
		public class IncludedByDeclaringType : XmlAdapterTestCase
		{
			[XmlInclude(typeof(IB))]
			public interface IFoo
			{
				IA X { get; set; }
			}

			public interface IA      { string A { get; set; } }
			public interface IB : IA { string B { get; set; } }

			[Test]
			public void Get_NoXsiType()
			{
				var xml = Xml("<Foo> <X> <A>a</A> </X> </Foo>");
				var obj = Create<IFoo>(xml);

				var value = obj.X;
				Assert.That(value,   Is.InstanceOf<IA>() & Is.Not.InstanceOf<IB>());
				Assert.That(value.A, Is.EqualTo("a"));
			}

			[Test]
			public void Get_XsiType_Default()
			{
				var xml = Xml("<Foo $xsi> <X xsi:type='A'> <A>a</A> </X> </Foo>");
				var obj = Create<IFoo>(xml);

				var value = obj.X;
				Assert.That(value,   Is.InstanceOf<IA>() & Is.Not.InstanceOf<IB>());
				Assert.That(value.A, Is.EqualTo("a"));
			}

			[Test]
			public void Get_XsiType_IncludedByDeclaringType()
			{
				var xml = Xml("<Foo $xsi> <X xsi:type='B'> <A>a</A> <B>b</B> </X> </Foo>");
				var obj = Create<IFoo>(xml);

				var value = obj.X;
				Assert.That(value,   Is.InstanceOf<IB>());
				Assert.That(value.A, Is.EqualTo("a"));

				var valueB = (IB) value;
				Assert.That(valueB.B, Is.EqualTo("b"));
			}
		}

		public class IncludedByDeclaredType : XmlAdapterTestCase
		{
			public interface IFoo
			{
				IA X { get; set; }
			}

			[XmlInclude(typeof(IB))]
			public interface IA      { string A { get; set; } }
			public interface IB : IA { string B { get; set; } }

			[Test]
			public void Get_XsiType_IncludedByDeclaredType()
			{
				var xml = Xml("<Foo $xsi> <X xsi:type='B'> <A>a</A> <B>b</B> </X> </Foo>");
				var obj = Create<IFoo>(xml);

				var value = obj.X;
				Assert.That(value,   Is.InstanceOf<IB>());
				Assert.That(value.A, Is.EqualTo("a"));

				var valueB = (IB) value;
				Assert.That(valueB.B, Is.EqualTo("b"));
			}
		}
	}
}