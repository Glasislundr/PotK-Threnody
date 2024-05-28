// Decompiled with JetBrains decompiler
// Type: GameCore.Serialization.VersionDeserializationBinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace GameCore.Serialization
{
  public sealed class VersionDeserializationBinder : SerializationBinder
  {
    public override System.Type BindToType(string assemblyName, string typeName)
    {
      if (string.IsNullOrEmpty(assemblyName) || string.IsNullOrEmpty(typeName))
        return (System.Type) null;
      assemblyName = Assembly.GetExecutingAssembly().FullName;
      return System.Type.GetType(string.Format("{0}, {1}", (object) typeName, (object) assemblyName));
    }
  }
}
