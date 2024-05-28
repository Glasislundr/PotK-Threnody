// Decompiled with JetBrains decompiler
// Type: GameCore.Serialization.TestCrossSerializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace GameCore.Serialization
{
  public class TestCrossSerializer
  {
    private static Func<object, byte[]> SerializeToMemory;
    private static Func<byte[], object> DeserializeFromMemory;

    private static void Run(object obj, Func<object, object, bool> condition)
    {
      byte[] numArray = TestCrossSerializer.SerializeToMemory(obj);
      object obj1 = TestCrossSerializer.DeserializeFromMemory(numArray);
      if (!condition(obj, obj1))
        throw new Exception();
    }

    private static bool Eq(object a, object b) => a != null ? a.Equals(b) : b == null;

    private static bool ArrayEqual(object a, object b)
    {
      return TestCrossSerializer.ArrayEqual(a, b, new Func<object, object, bool>(TestCrossSerializer.Eq));
    }

    private static bool ArrayEqual(object a, object b, Func<object, object, bool> f)
    {
      IEnumerator enumerator1 = ((IEnumerable) a).GetEnumerator();
      IEnumerator enumerator2 = ((IEnumerable) b).GetEnumerator();
      bool flag1;
      bool flag2;
      do
      {
        flag1 = enumerator1.MoveNext();
        flag2 = enumerator2.MoveNext();
        if (flag1 != flag2)
          return false;
        if (!flag1 && !flag2)
          return true;
      }
      while (!(flag1 & flag2) || f(enumerator1.Current, enumerator2.Current));
      return false;
    }

    private static bool ArrayArrayEqual(object a, object b)
    {
      return TestCrossSerializer.ArrayEqual(a, b, (Func<object, object, bool>) ((x, y) => TestCrossSerializer.ArrayEqual(x, y)));
    }

    private static void RunTest()
    {
      TestCrossSerializer.Run((object) 10, new Func<object, object, bool>(TestCrossSerializer.Eq));
      TestCrossSerializer.Run((object) new int[2]{ 1, 2 }, new Func<object, object, bool>(TestCrossSerializer.ArrayEqual));
      TestCrossSerializer.Run((object) new List<int>()
      {
        1,
        2
      }, new Func<object, object, bool>(TestCrossSerializer.ArrayEqual));
      TestCrossSerializer.Run((object) new List<int[]>()
      {
        new int[2]{ 1, 2 },
        new int[2]{ 3, 4 }
      }, new Func<object, object, bool>(TestCrossSerializer.ArrayArrayEqual));
      TestCrossSerializer.Hoge hoge1 = new TestCrossSerializer.Hoge(1, "2");
      TestCrossSerializer.Hoge hoge2 = new TestCrossSerializer.Hoge(3, "45");
      TestCrossSerializer.Run((object) new TestCrossSerializer.Hoge[2]
      {
        hoge1,
        hoge2
      }, new Func<object, object, bool>(TestCrossSerializer.ArrayEqual));
      TestCrossSerializer.Run((object) new TestCrossSerializer.Hoge[2]
      {
        hoge1,
        hoge1
      }, (Func<object, object, bool>) ((a, b) => TestCrossSerializer.ArrayEqual(a, b) && ((TestCrossSerializer.Hoge[]) b)[0] == ((TestCrossSerializer.Hoge[]) b)[1]));
      TestCrossSerializer.Run((object) new List<TestCrossSerializer.Hoge>()
      {
        hoge1,
        hoge2
      }, new Func<object, object, bool>(TestCrossSerializer.ArrayEqual));
      TestCrossSerializer.Run((object) new List<List<TestCrossSerializer.Hoge>>()
      {
        new List<TestCrossSerializer.Hoge>() { hoge1, hoge2 },
        new List<TestCrossSerializer.Hoge>() { hoge1, hoge2 }
      }, (Func<object, object, bool>) ((a, b) => TestCrossSerializer.ArrayArrayEqual(a, b) && ((List<List<TestCrossSerializer.Hoge>>) b)[0][0] == ((List<List<TestCrossSerializer.Hoge>>) b)[1][0] && ((List<List<TestCrossSerializer.Hoge>>) b)[0][1] == ((List<List<TestCrossSerializer.Hoge>>) b)[1][1]));
      List<TestCrossSerializer.Hoge> hogeList = new List<TestCrossSerializer.Hoge>()
      {
        hoge1,
        hoge2
      };
      TestCrossSerializer.Run((object) new List<List<TestCrossSerializer.Hoge>>()
      {
        hogeList,
        hogeList
      }, (Func<object, object, bool>) ((a, b) => TestCrossSerializer.ArrayArrayEqual(a, b) && ((List<List<TestCrossSerializer.Hoge>>) b)[0] == ((List<List<TestCrossSerializer.Hoge>>) b)[1]));
      TestCrossSerializer.Run((object) new TestCrossSerializer.Hoge[2, 3]
      {
        {
          hoge1,
          hoge1,
          hoge2
        },
        {
          hoge2,
          hoge1,
          hoge1
        }
      }, (Func<object, object, bool>) ((a, b) => TestCrossSerializer.ArrayEqual(a, b) && ((TestCrossSerializer.Hoge[,]) b)[0, 0] == ((TestCrossSerializer.Hoge[,]) b)[0, 1] && ((TestCrossSerializer.Hoge[,]) b)[1, 1] == ((TestCrossSerializer.Hoge[,]) b)[1, 2] && ((TestCrossSerializer.Hoge[,]) b)[0, 2] == ((TestCrossSerializer.Hoge[,]) b)[1, 0]));
      TestCrossSerializer.Run((object) TestCrossSerializer.EHoge.Value, new Func<object, object, bool>(TestCrossSerializer.Eq));
      int? nullable1 = new int?();
      int? nullable2 = new int?(100);
      TestCrossSerializer.Run((object) nullable1, new Func<object, object, bool>(TestCrossSerializer.Eq));
      TestCrossSerializer.Run((object) new int?[2]
      {
        nullable1,
        nullable1
      }, (Func<object, object, bool>) ((a, b) =>
      {
        if (!TestCrossSerializer.ArrayEqual(a, b))
          return false;
        int? nullable3 = ((int?[]) b)[0];
        int? nullable4 = ((int?[]) b)[1];
        return nullable3.GetValueOrDefault() == nullable4.GetValueOrDefault() & nullable3.HasValue == nullable4.HasValue;
      }));
      TestCrossSerializer.Run((object) new int?[2]
      {
        nullable1,
        nullable2
      }, new Func<object, object, bool>(TestCrossSerializer.ArrayEqual));
      TestCrossSerializer.Run((object) nullable2, new Func<object, object, bool>(TestCrossSerializer.Eq));
      TestCrossSerializer.HogeEx hogeEx = new TestCrossSerializer.HogeEx(4, "derived");
      TestCrossSerializer.Run((object) hogeEx, new Func<object, object, bool>(TestCrossSerializer.Eq));
      TestCrossSerializer.Hoge2 hoge2_1 = new TestCrossSerializer.Hoge2()
      {
        h = hoge1,
        hex = hogeEx
      };
      TestCrossSerializer.Hoge2 hoge2_2 = new TestCrossSerializer.Hoge2();
      hoge2_2.h = hoge2;
      hoge2_2.hex = hogeEx;
      TestCrossSerializer.Run((object) hoge2_1, new Func<object, object, bool>(TestCrossSerializer.Eq));
      TestCrossSerializer.Run((object) hoge2_2, new Func<object, object, bool>(TestCrossSerializer.Eq));
      hoge2_1.h = hoge2;
      TestCrossSerializer.Run((object) hoge2_1, new Func<object, object, bool>(TestCrossSerializer.Eq));
    }

    public static void Run()
    {
      TestCrossSerializer.SerializeToMemory = (Func<object, byte[]>) (obj => JsonFormatter.MakeSerializer().SerializeToMemory(obj));
      TestCrossSerializer.DeserializeFromMemory = (Func<byte[], object>) (obj => JsonFormatter.MakeSerializer().DeserializeFromMemory(obj));
      TestCrossSerializer.RunTest();
      TestCrossSerializer.SerializeToMemory = (Func<object, byte[]>) (obj => BinaryFormatter.MakeSerializer().SerializeToMemory(obj));
      TestCrossSerializer.DeserializeFromMemory = (Func<byte[], object>) (obj => BinaryFormatter.MakeSerializer().DeserializeFromMemory(obj));
      TestCrossSerializer.RunTest();
      TestCrossSerializer.SerializeToMemory = (Func<object, byte[]>) (obj =>
      {
        CrossSerializer serializer1 = JsonFormatter.MakeSerializer();
        CrossSerializer serializer2 = BinaryFormatter.MakeSerializer();
        if (!TestCrossSerializer.ArrayEqual((object) serializer1.SerializeToMemory(serializer2.DeserializeFromMemory(serializer2.SerializeToMemory(obj))), (object) serializer1.SerializeToMemory(obj)))
          throw new Exception("hoge-");
        return serializer1.SerializeToMemory(obj);
      });
      TestCrossSerializer.DeserializeFromMemory = (Func<byte[], object>) (buf => JsonFormatter.MakeSerializer().DeserializeFromMemory(buf));
      TestCrossSerializer.RunTest();
      TestCrossSerializer.SerializeToMemory = (Func<object, byte[]>) (obj =>
      {
        CrossSerializer serializer3 = JsonFormatter.MakeSerializer();
        CrossSerializer serializer4 = BinaryFormatter.MakeSerializer(true);
        if (!TestCrossSerializer.ArrayEqual((object) serializer3.SerializeToMemory(serializer4.DeserializeFromMemory(serializer4.SerializeToMemory(obj))), (object) serializer3.SerializeToMemory(obj)))
          throw new Exception("hoge-");
        return serializer3.SerializeToMemory(obj);
      });
      TestCrossSerializer.DeserializeFromMemory = (Func<byte[], object>) (buf => JsonFormatter.MakeSerializer().DeserializeFromMemory(buf));
      TestCrossSerializer.RunTest();
      SerializeContext scon = new SerializeContext();
      DeserializeContext dcon = new DeserializeContext();
      TestCrossSerializer.SerializeToMemory = (Func<object, byte[]>) (obj => JsonFormatter.MakeSerializer().SerializeToMemory(obj, scon));
      TestCrossSerializer.DeserializeFromMemory = (Func<byte[], object>) (obj => JsonFormatter.MakeSerializer().DeserializeFromMemory(obj, dcon));
      TestCrossSerializer.RunTest();
    }

    private class Hoge
    {
      private int intValue;
      private string stringValue;
      private int? intnValue;

      public Hoge(int n, string s)
      {
        this.intValue = n;
        this.stringValue = s;
        this.intnValue = new int?(n);
      }

      public override bool Equals(object obj)
      {
        if (!(obj is TestCrossSerializer.Hoge hoge) || this.intValue != hoge.intValue || !(this.stringValue == hoge.stringValue))
          return false;
        int? intnValue1 = this.intnValue;
        int? intnValue2 = hoge.intnValue;
        return intnValue1.GetValueOrDefault() == intnValue2.GetValueOrDefault() & intnValue1.HasValue == intnValue2.HasValue;
      }

      public override int GetHashCode() => 0;
    }

    private class HogeEx : TestCrossSerializer.Hoge
    {
      private int intValue2;

      public HogeEx(int n, string s)
        : base(n, s)
      {
        this.intValue2 = n;
      }

      public override bool Equals(object obj)
      {
        return obj is TestCrossSerializer.HogeEx hogeEx && base.Equals(obj) && this.intValue2 == hogeEx.intValue2;
      }

      public override int GetHashCode() => 0;
    }

    private class Hoge2
    {
      public TestCrossSerializer.Hoge h;
      public TestCrossSerializer.HogeEx hex;

      public override bool Equals(object obj)
      {
        return obj is TestCrossSerializer.Hoge2 hoge2 && this.h.Equals((object) hoge2.h) && this.hex.Equals((object) hoge2.hex);
      }

      public override int GetHashCode() => 0;
    }

    private enum EHoge
    {
      Value = 20, // 0x00000014
    }
  }
}
