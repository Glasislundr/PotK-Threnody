// Decompiled with JetBrains decompiler
// Type: SetTextPullDown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SetTextPullDown : MonoBehaviour
{
  [SerializeField]
  public UILabel label;
  [SerializeField]
  public UIPopupList popup;
  [SerializeField]
  public UIInput input;

  public void SetText() => this.label.text = this.popup.value;

  public void SetTextInput() => this.label.text = this.input.value;
}
