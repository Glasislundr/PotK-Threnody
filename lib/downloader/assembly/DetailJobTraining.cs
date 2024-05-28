// Decompiled with JetBrains decompiler
// Type: DetailJobTraining
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class DetailJobTraining : MonoBehaviour
{
  [SerializeField]
  private SpreadColorButton button;
  [SerializeField]
  private UILabel txtMenuName;
  [SerializeField]
  private GameObject txtUnopended;
  public UIDragScrollView dragScrollView;

  public IEnumerator Init(string menuName, bool isActive, Action callFunction)
  {
    this.txtMenuName.text = menuName;
    this.SetActive(isActive);
    if (isActive)
      EventDelegate.Add(this.button.onClick, (EventDelegate.Callback) (() => callFunction()));
    yield return (object) new WaitForEndOfFrame();
  }

  private void SetActive(bool active)
  {
    ((UIButtonColor) this.button).isEnabled = active;
    this.txtUnopended.SetActive(!active);
  }
}
