// Decompiled with JetBrains decompiler
// Type: GameCore.Serialization.TypeObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UniLinq;

#nullable disable
namespace GameCore.Serialization
{
  public class TypeObject
  {
    public int objectId;
    public TypeInfo typeInfo;
    public byte[] buf;
    public int? length;
    public int value;

    private void SetT<T>(T value) where T : struct
    {
      T[] src = TypeObject.OneArray<T>.Value;
      src[0] = value;
      int count = Buffer.ByteLength((Array) src);
      this.buf = new byte[count];
      Buffer.BlockCopy((Array) src, 0, (Array) this.buf, 0, count);
    }

    private T GetT<T>() where T : struct
    {
      T[] dst = TypeObject.OneArray<T>.Value;
      Buffer.BlockCopy((Array) this.buf, 0, (Array) dst, 0, this.buf.Length);
      return dst[0];
    }

    public void Set(bool v) => this.SetT<bool>(v);

    public void Set(sbyte v) => this.SetT<sbyte>(v);

    public void Set(short v) => this.SetT<short>(v);

    public void Set(int v) => this.value = v;

    public void Set(long v) => this.SetT<long>(v);

    public void Set(byte v) => this.SetT<byte>(v);

    public void Set(ushort v) => this.SetT<ushort>(v);

    public void Set(uint v) => this.SetT<uint>(v);

    public void Set(ulong v) => this.SetT<ulong>(v);

    public void Set(char v) => this.SetT<char>(v);

    public void Set(float v) => this.SetT<float>(v);

    public void Set(double v) => this.SetT<double>(v);

    public bool GetBool() => this.GetT<bool>();

    public sbyte GetSByte() => this.GetT<sbyte>();

    public short GetShort() => this.GetT<short>();

    public int GetInt() => this.value;

    public long GetLong() => this.GetT<long>();

    public byte GetByte() => this.GetT<byte>();

    public ushort GetUShort() => this.GetT<ushort>();

    public uint GetUInt() => this.GetT<uint>();

    public ulong GetULong() => this.GetT<ulong>();

    public char GetChar() => this.GetT<char>();

    public float GetFloat() => this.GetT<float>();

    public double GetDouble() => this.GetT<double>();

    public void Set(string v) => this.buf = Encoding.UTF8.GetBytes(v);

    public string GetString() => Encoding.UTF8.GetString(this.buf);

    private void SetTArray<T>(T[] v) where T : struct
    {
      int count = Buffer.ByteLength((Array) v);
      this.buf = new byte[count];
      this.length = new int?(v.Length);
      Buffer.BlockCopy((Array) v, 0, (Array) this.buf, 0, count);
    }

    private T[] GetTArray<T>() where T : struct
    {
      T[] dst = new T[this.length.Value];
      Buffer.BlockCopy((Array) this.buf, 0, (Array) dst, 0, this.buf.Length);
      return dst;
    }

    public void SetArray(bool[] v) => this.SetTArray<bool>(v);

    public void SetArray(sbyte[] v) => this.SetTArray<sbyte>(v);

    public void SetArray(short[] v) => this.SetTArray<short>(v);

    public void SetArray(int[] v) => this.SetTArray<int>(v);

    public void SetArray(long[] v) => this.SetTArray<long>(v);

    public void SetArray(byte[] v) => this.SetTArray<byte>(v);

    public void SetArray(ushort[] v) => this.SetTArray<ushort>(v);

    public void SetArray(uint[] v) => this.SetTArray<uint>(v);

    public void SetArray(ulong[] v) => this.SetTArray<ulong>(v);

    public void SetArray(char[] v) => this.SetTArray<char>(v);

    public void SetArray(float[] v) => this.SetTArray<float>(v);

    public void SetArray(double[] v) => this.SetTArray<double>(v);

    public bool[] GetBoolArray() => this.GetTArray<bool>();

    public sbyte[] GetSByteArray() => this.GetTArray<sbyte>();

    public short[] GetShortArray() => this.GetTArray<short>();

    public int[] GetIntArray() => this.GetTArray<int>();

    public long[] GetLongArray() => this.GetTArray<long>();

    public byte[] GetByteArray() => this.GetTArray<byte>();

    public ushort[] GetUShortArray() => this.GetTArray<ushort>();

    public uint[] GetUIntArray() => this.GetTArray<uint>();

    public ulong[] GetULongArray() => this.GetTArray<ulong>();

    public char[] GetCharArray() => this.GetTArray<char>();

    public float[] GetFloatArray() => this.GetTArray<float>();

    public double[] GetDoubleArray() => this.GetTArray<double>();

    public Dictionary<string, object> ToJson()
    {
      return new Dictionary<string, object>()
      {
        {
          "objectId",
          (object) this.objectId
        },
        {
          "typeInfo",
          (object) this.typeInfo.AssemblyQualifiedName
        },
        {
          "buf",
          this.buf == null ? (object) (string) null : (object) BitConverter.ToString(this.buf).Replace("-", "")
        },
        {
          "length",
          this.length.HasValue ? (object) this.length.Value : (object) null
        },
        {
          "value",
          (object) this.value
        }
      };
    }

    public static TypeObject FromJson(IDictionary<string, object> json)
    {
      TypeObject typeObject = new TypeObject();
      typeObject.objectId = (int) (long) json["objectId"];
      typeObject.typeInfo = TypeInfo.Parse((string) json["typeInfo"]);
      typeObject.buf = json["buf"] == null ? (byte[]) null : TypeObject.ToBytes((string) json["buf"]);
      long? nullable = (long?) json["length"];
      typeObject.length = nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?();
      typeObject.value = (int) (long) json["value"];
      return typeObject;
    }

    private static byte[] ToBytes(string xs)
    {
      return Enumerable.Range(0, xs.Length / 2).Select<int, byte>((Func<int, byte>) (x => Convert.ToByte(xs.Substring(x * 2, 2), 16))).ToArray<byte>();
    }

    private static class OneArray<T>
    {
      public static T[] Value => new T[1];
    }
  }
}
