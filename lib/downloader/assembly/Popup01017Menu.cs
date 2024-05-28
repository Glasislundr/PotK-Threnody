// Decompiled with JetBrains decompiler
// Type: Popup01017Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class Popup01017Menu : NGMenuBase
{
  public IEnumerator Init()
  {
    yield break;
  }

  public virtual void IbtnOk() => Singleton<PopupManager>.GetInstance().onDismiss();
}
