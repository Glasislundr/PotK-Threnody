// Decompiled with JetBrains decompiler
// Type: JobChangeData.CompleteParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;

#nullable disable
namespace JobChangeData
{
  public class CompleteParam
  {
    public PlayerUnit before_ { get; private set; }

    public PlayerUnit after_ { get; private set; }

    public PlayerMaterialUnit[] materials_ { get; private set; }

    public CompleteParam(
      PlayerUnit beforeUnit,
      PlayerMaterialUnit[] materials,
      PlayerUnit afterUnit)
    {
      this.before_ = beforeUnit;
      this.after_ = afterUnit;
      this.materials_ = materials;
    }
  }
}
