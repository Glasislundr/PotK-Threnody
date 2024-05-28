// Decompiled with JetBrains decompiler
// Type: Friend008203Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class Friend008203Menu : BackButtonMenuBase
{
  private IEnumerator BackSceneAsync()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<NGSceneManager>.GetInstance().changeScene("friend008_5", false);
    Singleton<NGSceneManager>.GetInstance().destroyScene("friend008_19");
    yield break;
  }

  public virtual void IbtnOk()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnOk();
}
