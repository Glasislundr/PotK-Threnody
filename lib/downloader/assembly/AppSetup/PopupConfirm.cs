// Decompiled with JetBrains decompiler
// Type: AppSetup.PopupConfirm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace AppSetup
{
  public class PopupConfirm : MonoBehaviour
  {
    [SerializeField]
    private UIButton uiButton;
    [SerializeField]
    private UILabel label;

    public bool IsDecide { get; private set; }

    private void Start()
    {
      EventDelegate.Set(this.uiButton.onClick, (EventDelegate.Callback) (() => this.OnOK()));
      this.IsDecide = false;
    }

    public void SelectText(string text) => this.label.text = text;

    public void OnOK() => this.IsDecide = true;
  }
}
