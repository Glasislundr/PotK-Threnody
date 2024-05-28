// Decompiled with JetBrains decompiler
// Type: Gsc.DOM.IValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace Gsc.DOM
{
  public interface IValue
  {
    bool IsNull();

    bool IsObject();

    bool IsArray();

    bool IsBool();

    bool IsString();

    bool IsInt();

    bool IsUInt();

    bool IsLong();

    bool IsULong();

    bool IsFloat();

    bool IsDouble();

    IObject GetObject();

    IArray GetArray();

    bool ToBool();

    string ToString();

    int ToInt();

    uint ToUInt();

    long ToLong();

    ulong ToULong();

    float ToFloat();

    double ToDouble();

    IValue this[int index] { get; }

    IValue this[string name] { get; }
  }
}
