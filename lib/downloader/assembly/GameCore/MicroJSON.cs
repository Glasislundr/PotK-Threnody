// Decompiled with JetBrains decompiler
// Type: GameCore.MicroJSON
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MiniJSON;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameCore
{
  public class MicroJSON
  {
    private static object ParseMiniJSONValue(object obj, bool toIntern)
    {
      if (obj == null)
        return (object) null;
      if (obj is Dictionary<string, object> dictionary1)
      {
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        foreach (KeyValuePair<string, object> keyValuePair in dictionary1)
        {
          string key = toIntern ? string.Intern(keyValuePair.Key) : keyValuePair.Key;
          dictionary.Add(key, MicroJSON.ParseMiniJSONValue(keyValuePair.Value, toIntern));
        }
        return (object) new AssocList<string, object>((IDictionary<string, object>) dictionary);
      }
      if (obj is List<object> objectList)
      {
        object[] array = objectList.ToArray();
        for (int index = 0; index < array.Length; ++index)
          array[index] = MicroJSON.ParseMiniJSONValue(array[index], toIntern);
        return (object) array;
      }
      if (obj.GetType() == typeof (long))
        return (object) (int) (long) obj;
      if (obj.GetType() == typeof (double))
        return (object) (float) (double) obj;
      return obj is string str ? (toIntern ? (object) string.Intern(str) : (object) str) : obj;
    }

    public static MicroJSON.IValue FromMiniJSON(object obj, bool toIntern = false)
    {
      return (MicroJSON.IValue) new MicroJSON.Value(MicroJSON.ParseMiniJSONValue(obj, toIntern));
    }

    public interface IValue
    {
      bool IsBoolean { get; }

      bool AsBoolean { get; }

      bool IsInteger { get; }

      int AsInteger { get; }

      bool IsFloat { get; }

      float AsFloat { get; }

      bool IsString { get; }

      string AsString { get; }

      int Length { get; }

      bool IsObject { get; }

      bool ContainsKey(string key);

      MicroJSON.IValue this[string key] { get; }

      IEnumerable<KeyValuePair<string, MicroJSON.IValue>> EnumObject();

      bool IsArray { get; }

      MicroJSON.IValue this[int index] { get; }

      IEnumerable<MicroJSON.IValue> EnumArray();

      string Serialize();
    }

    public class Value : MicroJSON.IValue
    {
      private object value;

      public Value(object value) => this.value = value;

      public bool IsBoolean => this.value != null && this.value.GetType() == typeof (bool);

      public bool AsBoolean => (bool) this.value;

      public bool IsInteger => this.value != null && this.value.GetType() == typeof (int);

      public int AsInteger => (int) this.value;

      public bool IsFloat => this.value != null && this.value.GetType() == typeof (float);

      public float AsFloat => (float) this.value;

      public bool IsString => this.value is string;

      public string AsString => (string) this.value;

      public int Length
      {
        get
        {
          if (this.value is Array array)
            return array.Length;
          return this.value is IDictionary<string, object> dictionary ? dictionary.Count : throw new Exception("value must be object or array");
        }
      }

      public bool IsObject => this.value is IDictionary<string, object>;

      public bool ContainsKey(string key)
      {
        return this.value is IDictionary<string, object> dictionary ? dictionary.ContainsKey(key) : throw new Exception("value must be object");
      }

      public MicroJSON.IValue this[string key]
      {
        get
        {
          return this.value is IDictionary<string, object> dictionary ? (MicroJSON.IValue) new MicroJSON.Value(dictionary[key]) : throw new Exception("value must be object");
        }
      }

      public IEnumerable<KeyValuePair<string, MicroJSON.IValue>> EnumObject()
      {
        if (!(this.value is IDictionary<string, object> dictionary))
          throw new Exception("value must be object");
        foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) dictionary)
          yield return new KeyValuePair<string, MicroJSON.IValue>(keyValuePair.Key, (MicroJSON.IValue) new MicroJSON.Value(keyValuePair.Value));
      }

      public bool IsArray => this.value is Array;

      public MicroJSON.IValue this[int index]
      {
        get
        {
          return this.value is Array array ? (MicroJSON.IValue) new MicroJSON.Value(array.GetValue(index)) : throw new Exception("value must be array");
        }
      }

      public IEnumerable<MicroJSON.IValue> EnumArray()
      {
        if (!(this.value is Array array))
          throw new Exception("value must be object");
        foreach (object obj in array)
          yield return (MicroJSON.IValue) new MicroJSON.Value(obj);
      }

      public string Serialize() => Json.Serialize(this.value);
    }
  }
}
