// Decompiled with JetBrains decompiler
// Type: ClearMessagePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ClearMessagePopup : MonoBehaviour
{
  [SerializeField]
  private UILabel TxtTitle;
  [SerializeField]
  private UILabel TxtDescription;
  [SerializeField]
  private UIButton ibtnOK;

  public void Init(string title, string description, EventDelegate del)
  {
    ((UIRect) ((Component) this).gameObject.GetComponent<UIWidget>()).alpha = 0.0f;
    this.TxtTitle.SetText(title);
    this.TxtDescription.SetText(description);
    if (del != null)
      EventDelegate.Set(this.ibtnOK.onClick, del);
    ((UIRect) ((Component) this).gameObject.GetComponent<UIWidget>()).alpha = 1f;
  }

  public void btnOK() => Singleton<PopupManager>.GetInstance().onDismiss();
}
