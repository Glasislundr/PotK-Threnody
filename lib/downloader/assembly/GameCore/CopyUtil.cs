// Decompiled with JetBrains decompiler
// Type: GameCore.CopyUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore.Serialization;

#nullable disable
namespace GameCore
{
  public static class CopyUtil
  {
    public static T DeepCopy<T>(T target)
    {
      return (T) EasySerializer.DeserializeObjectFromMemory(EasySerializer.SerializeObjectToMemory((object) target));
    }
  }
}
