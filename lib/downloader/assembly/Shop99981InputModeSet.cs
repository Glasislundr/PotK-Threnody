// Decompiled with JetBrains decompiler
// Type: Shop99981InputModeSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Shop99981InputModeSet : MonoBehaviour
{
  [SerializeField]
  protected UIInput nen;
  [SerializeField]
  protected UIInput getsu;
  [SerializeField]
  protected UIInput hi;

  private void Start() => this.setKeyboardTypeNumber();

  private void Update()
  {
  }

  public void setKeyboardTypeNumber()
  {
    this.nen.keyboardType = (UIInput.KeyboardType) 4;
    this.getsu.keyboardType = (UIInput.KeyboardType) 4;
    this.hi.keyboardType = (UIInput.KeyboardType) 4;
  }
}
