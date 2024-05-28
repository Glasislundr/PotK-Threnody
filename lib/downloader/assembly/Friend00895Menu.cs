// Decompiled with JetBrains decompiler
// Type: Friend00895Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class Friend00895Menu : NGMenuBase
{
  private bool isPush;

  private IEnumerator BackSceneAsync()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<NGSceneManager>.GetInstance().changeScene("friend008_5", false);
    Singleton<NGSceneManager>.GetInstance().destroyScene("friend008_19");
    yield break;
  }

  public virtual void IbtnOk()
  {
    if (this.isPush)
      return;
    this.isPush = true;
    this.StartCoroutine(this.BackSceneAsync());
  }
}
