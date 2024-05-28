// Decompiled with JetBrains decompiler
// Type: JobChangeData.SceneParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace JobChangeData
{
  public class SceneParam
  {
    public int playerUnitId_ { get; private set; }

    public int? jobId_ { get; private set; }

    public Action onBackScene_ { get; private set; }

    public SceneParam(int playerUnitId, int? jobId = null, Action onBackScene = null)
    {
      this.playerUnitId_ = playerUnitId;
      this.jobId_ = jobId;
      this.onBackScene_ = onBackScene;
    }
  }
}
