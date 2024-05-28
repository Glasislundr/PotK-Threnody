// Decompiled with JetBrains decompiler
// Type: GameCore.Serialization.ICrossFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.IO;

#nullable disable
namespace GameCore.Serialization
{
  public interface ICrossFormatter
  {
    void Save(int rootId, TypeObject[] objects, TreeObject[] trees, Stream stream);

    void Load(Stream stream, out int rootId, out TypeObject[] objects, out TreeObject[] trees);
  }
}
