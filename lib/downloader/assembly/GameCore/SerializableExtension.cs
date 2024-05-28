// Decompiled with JetBrains decompiler
// Type: GameCore.SerializableExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace GameCore
{
  public static class SerializableExtension
  {
    public static void Serialize(this bool self, Stream writer)
    {
      new BinaryWriter(writer).Write(self);
    }

    public static bool Deserialize(this bool self, Stream reader)
    {
      return new BinaryReader(reader).ReadBoolean();
    }

    public static void Serialize(this bool? self, Stream writer)
    {
      BinaryWriter binaryWriter = new BinaryWriter(writer);
      binaryWriter.Write(self.HasValue);
      if (!self.HasValue)
        return;
      binaryWriter.Write(self.Value);
    }

    public static bool? Deserialize(this bool? self, Stream reader)
    {
      BinaryReader binaryReader = new BinaryReader(reader);
      return !binaryReader.ReadBoolean() ? new bool?() : new bool?(binaryReader.ReadBoolean());
    }

    public static void Serialize(this bool[] self, Stream writer)
    {
      BinaryWriter bw = new BinaryWriter(writer);
      bw.Write(self.Length);
      ((IEnumerable<bool>) self).ForEach<bool>((Action<bool>) (x => bw.Write(x)));
    }

    public static bool[] Deserialize(this bool[] self, Stream reader)
    {
      BinaryReader binaryReader = new BinaryReader(reader);
      bool[] flagArray = new bool[binaryReader.ReadInt32()];
      for (int index = 0; index < flagArray.Length; ++index)
        flagArray[index] = binaryReader.ReadBoolean();
      return flagArray;
    }

    public static void Serialize(this List<bool> self, Stream writer)
    {
      BinaryWriter bw = new BinaryWriter(writer);
      bw.Write(self.Count);
      self.ForEach((Action<bool>) (x => bw.Write(x)));
    }

    public static List<bool> Deserialize(this List<bool> self, Stream reader)
    {
      BinaryReader binaryReader = new BinaryReader(reader);
      int num = binaryReader.ReadInt32();
      List<bool> boolList = new List<bool>();
      for (int index = 0; index < num; ++index)
        boolList.Add(binaryReader.ReadBoolean());
      return boolList;
    }

    public static void Serialize(this int self, Stream writer)
    {
      new BinaryWriter(writer).Write(self);
    }

    public static int Deserialize(this int self, Stream reader)
    {
      return new BinaryReader(reader).ReadInt32();
    }

    public static void Serialize(this int? self, Stream writer)
    {
      BinaryWriter binaryWriter = new BinaryWriter(writer);
      binaryWriter.Write(self.HasValue);
      if (!self.HasValue)
        return;
      binaryWriter.Write(self.Value);
    }

    public static int? Deserialize(this int? self, Stream reader)
    {
      BinaryReader binaryReader = new BinaryReader(reader);
      return !binaryReader.ReadBoolean() ? new int?() : new int?(binaryReader.ReadInt32());
    }

    public static void Serialize(this int[] self, Stream writer)
    {
      BinaryWriter bw = new BinaryWriter(writer);
      bw.Write(self.Length);
      ((IEnumerable<int>) self).ForEach<int>((Action<int>) (x => bw.Write(x)));
    }

    public static int[] Deserialize(this int[] self, Stream reader)
    {
      BinaryReader binaryReader = new BinaryReader(reader);
      int[] numArray = new int[binaryReader.ReadInt32()];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = binaryReader.ReadInt32();
      return numArray;
    }

    public static void Serialize(this List<int> self, Stream writer)
    {
      BinaryWriter bw = new BinaryWriter(writer);
      bw.Write(self.Count);
      self.ForEach((Action<int>) (x => bw.Write(x)));
    }

    public static List<int> Deserialize(this List<int> self, Stream reader)
    {
      BinaryReader binaryReader = new BinaryReader(reader);
      int num = binaryReader.ReadInt32();
      List<int> intList = new List<int>();
      for (int index = 0; index < num; ++index)
        intList.Add(binaryReader.ReadInt32());
      return intList;
    }

    public static void Serialize(this float self, Stream writer)
    {
      new BinaryWriter(writer).Write(self);
    }

    public static float Deserialize(this float self, Stream reader)
    {
      return new BinaryReader(reader).ReadSingle();
    }

    public static void Serialize(this float? self, Stream writer)
    {
      BinaryWriter binaryWriter = new BinaryWriter(writer);
      binaryWriter.Write(self.HasValue);
      if (self.HasValue)
        return;
      binaryWriter.Write(self.Value);
    }

    public static float? Deserialize(this float? self, Stream reader)
    {
      BinaryReader binaryReader = new BinaryReader(reader);
      return !binaryReader.ReadBoolean() ? new float?() : new float?(binaryReader.ReadSingle());
    }

    public static void Serialize(this float[] self, Stream writer)
    {
      BinaryWriter bw = new BinaryWriter(writer);
      bw.Write(self.Length);
      ((IEnumerable<float>) self).ForEach<float>((Action<float>) (x => bw.Write(x)));
    }

    public static float[] Deserialize(this float[] self, Stream reader)
    {
      BinaryReader binaryReader = new BinaryReader(reader);
      float[] numArray = new float[binaryReader.ReadInt32()];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = binaryReader.ReadSingle();
      return numArray;
    }

    public static void Serialize(this List<float> self, Stream writer)
    {
      BinaryWriter bw = new BinaryWriter(writer);
      bw.Write(self.Count);
      self.ForEach((Action<float>) (x => bw.Write(x)));
    }

    public static List<float> Deserialize(this List<float> self, Stream reader)
    {
      BinaryReader binaryReader = new BinaryReader(reader);
      int num = binaryReader.ReadInt32();
      List<float> floatList = new List<float>();
      for (int index = 0; index < num; ++index)
        floatList.Add((float) binaryReader.ReadInt32());
      return floatList;
    }

    public static void Serialize(this string self, Stream writer)
    {
      new BinaryWriter(writer).Write(self);
    }

    public static string Deserialize(this string self, Stream reader)
    {
      return new BinaryReader(reader).ReadString();
    }

    public static void Serialize(this string[] self, Stream writer)
    {
      BinaryWriter bw = new BinaryWriter(writer);
      bw.Write(self.Length);
      ((IEnumerable<string>) self).ForEach<string>((Action<string>) (x => bw.Write(x)));
    }

    public static string[] Deserialize(this string[] self, Stream reader)
    {
      BinaryReader binaryReader = new BinaryReader(reader);
      string[] strArray = new string[binaryReader.ReadInt32()];
      for (int index = 0; index < strArray.Length; ++index)
        strArray[index] = binaryReader.ReadString();
      return strArray;
    }

    public static void Serialize(this List<string> self, Stream writer)
    {
      BinaryWriter bw = new BinaryWriter(writer);
      bw.Write(self.Count);
      self.ForEach((Action<string>) (x => bw.Write(x)));
    }

    public static List<string> Deserialize(this List<string> self, Stream reader)
    {
      BinaryReader binaryReader = new BinaryReader(reader);
      int num = binaryReader.ReadInt32();
      List<string> stringList = new List<string>();
      for (int index = 0; index < num; ++index)
        stringList.Add(binaryReader.ReadString());
      return stringList;
    }

    public static void Serialize<T>(this T[] self, Stream writer) where T : BLSerializable
    {
      new BinaryWriter(writer).Write(self.Length);
      ((IEnumerable<T>) self).ForEach<T>((Action<T>) (x => x.Serialize(writer)));
    }

    public static T[] Deserialize<T>(this T[] self, Stream reader) where T : BLSerializable, new()
    {
      int length = new BinaryReader(reader).ReadInt32();
      T[] objArray = new T[length];
      for (int index = 0; index < length; ++index)
      {
        objArray[index] = new T();
        objArray[index].Deserialize(reader);
      }
      return objArray;
    }

    public static void Serialize<T>(this List<T> self, Stream writer) where T : BLSerializable
    {
      new BinaryWriter(writer).Write(self.Count);
      self.ForEach((Action<T>) (x => x.Serialize(writer)));
    }

    public static List<T> Deserialize<T>(this List<T> self, Stream reader) where T : BLSerializable, new()
    {
      int capacity = new BinaryReader(reader).ReadInt32();
      List<T> objList = new List<T>(capacity);
      for (int index = 0; index < capacity; ++index)
      {
        T obj = new T();
        obj.Deserialize(reader);
        objList.Add(obj);
      }
      return objList;
    }

    public static void Serialize<T1, T2>(this Tuple<T1, T2> self, Stream writer)
      where T1 : BLSerializable
      where T2 : BLSerializable
    {
      self.Item1.Serialize(writer);
      self.Item2.Serialize(writer);
    }

    public static Tuple<T1, T2> Deserialize<T1, T2>(this Tuple<T1, T2> self, Stream reader)
      where T1 : BLSerializable, new()
      where T2 : BLSerializable, new()
    {
      T1 obj1 = new T1();
      T2 obj2 = new T2();
      obj1.Deserialize(reader);
      obj2.Deserialize(reader);
      return Tuple.Create<T1, T2>(obj1, obj2);
    }

    public static void Serialize(this Tuple<int, int> self, Stream writer)
    {
      self.Item1.Serialize(writer);
      self.Item2.Serialize(writer);
    }

    public static Tuple<int, int> Deserialize(this Tuple<int, int> self, Stream reader)
    {
      int self1 = 0;
      return Tuple.Create<int, int>(0.Deserialize(reader), self1.Deserialize(reader));
    }
  }
}
