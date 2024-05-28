// Decompiled with JetBrains decompiler
// Type: SoundQualityConfirmPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.IO;
using UnityEngine;

#nullable disable
public class SoundQualityConfirmPopup : BackButtonMenuBase
{
  public bool IsNormal;

  public override void onBackButton() => this.OnNo();

  public void OnYes()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    ResourceDownloader.ClearDLC();
    CachedFile.Clear();
    Caching.ClearCache();
    File.Delete(ResourceManager.dlcVersionPath);
    File.Delete(ResourceManager.pathsJsonPath);
    Persist.normalDLC.Data.IsSound = this.IsNormal;
    Persist.normalDLC.Data.IsSoundSetup = true;
    Persist.normalDLC.Flush();
    ResourceManager.alreadyDirPath.Clear();
    StartScript.Restart();
  }

  public void OnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
