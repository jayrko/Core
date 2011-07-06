// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
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

namespace Castle.DynamicProxy.Tests.BugsReported
{
	using System;

	using NUnit.Framework;
	using System.IO;
	using System.Collections.Generic;
	using System.Runtime.Serialization.Formatters.Binary;
	using System.Reflection;
	using System.Runtime.Serialization;

	[TestFixture]
	public class DynProxy159 : BasePEVerifyTestCase
	{
		// this test will only fail the first time it is run in a given VM
		[Test]
		public void ShouldSerializedMixofProxiedAndUnproxiedInstances()
		{			
			MySerialClass[] o = new MySerialClass[] {
				new MySerialClass(),
				(MySerialClass)generator.CreateClassProxy(typeof(MySerialClass)),
				new MySerialClass(),
			};

			o[2].yyy = 3.1415;
			o[2].zzz = 100;

			FakeSerialize(o);

			Assert.AreEqual(3.1415, o[2].yyy);
			Assert.AreEqual(100, o[2].zzz);
		}

		[Test]
		public void ShouldNotChangeOrderOfSerializeableMembers()
		{
			MemberInfo[] fromSystem = FormatterServices.GetSerializableMembers(typeof(MySerialClass));
			MemberInfo[] beforeProxySerialization = new MemberInfo[fromSystem.Length];
			Array.Copy(fromSystem, beforeProxySerialization, fromSystem.Length);
			fromSystem = null;

			FakeSerialize((MySerialClass)generator.CreateClassProxy(typeof(MySerialClass)));

			fromSystem = FormatterServices.GetSerializableMembers(typeof(MySerialClass));

			Assert.AreEqual(beforeProxySerialization, fromSystem);
		}

		private void FakeSerialize(object o) {
			using (MemoryStream ms = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(ms, o);
			}
		}

	}

	[Serializable]
	public class MySerialClass
	{
		public int? zzz { get; set; }
		public double? yyy { get; set; }
		public string xxx { get; set; }
	}

}