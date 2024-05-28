// Decompiled with JetBrains decompiler
// Type: Quest002152Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Quest002152Menu : NGMenuBase
{
  [SerializeField]
  protected UILabel TxtDescription;
  [SerializeField]
  protected UILabel TxtLiberation;
  public GameObject popupPrefab002152;
  public int Pop;

  public virtual void IbtnPopupClose() => Debug.Log((object) "click default event IbtnPopupClose");

  public void popupIntimacy()
  {
    Singleton<PopupManager>.GetInstance().openAlert(this.popupPrefab002152);
  }

  public IEnumerator Init()
  {
    this.Pop = 0;
    yield break;
  }

  private void Update()
  {
    if (this.Pop != 4)
      return;
    this.popupPrefab002152.GetComponent<Quest002152popup>().PopupSetiing();
    this.popupIntimacy();
    this.Pop = 0;
  }

  public void PopPlus() => ++this.Pop;
}
