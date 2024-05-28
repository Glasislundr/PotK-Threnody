// Decompiled with JetBrains decompiler
// Type: EffectControllerUnitRegression
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Unit/EffectControllerRegression")]
public class EffectControllerUnitRegression : MonoBehaviour
{
  [SerializeField]
  private PopupUnitRegression root_;

  public void changeIcon() => this.root_.changeIconToAfter(true);
}
