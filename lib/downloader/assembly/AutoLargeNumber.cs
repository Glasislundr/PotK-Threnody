// Decompiled with JetBrains decompiler
// Type: AutoLargeNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AutoLargeNumber : MonoBehaviour
{
  private UILabel textbox;

  private void Awake() => this.textbox = ((Component) this).GetComponent<UILabel>();

  private void Start()
  {
  }

  private void Update() => this.textbox.SetTextLocalize(this.textbox.text);
}
