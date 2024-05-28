// Decompiled with JetBrains decompiler
// Type: Unit0042FloatingDialogBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/unit004_2/Unit0042FloatingDialogBase")]
public class Unit0042FloatingDialogBase : MonoBehaviour
{
  [SerializeField]
  protected GameObject DialogConteiner;
  [SerializeField]
  protected GameObject dir_SpecialPoint;
  [SerializeField]
  protected GameObject dir_FamilyType;
  protected bool isShow;

  public void Show()
  {
    if (this.DialogConteiner.activeInHierarchy && this.isShow)
      return;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
    this.isShow = true;
    this.DialogConteiner.SetActive(true);
    ((IEnumerable<UITweener>) ((Component) this).gameObject.GetComponentsInChildren<UITweener>()).ForEach<UITweener>((Action<UITweener>) (c =>
    {
      ((Behaviour) c).enabled = true;
      c.onFinished.Clear();
      c.PlayForward();
    }));
  }

  public void Hide()
  {
    if (!this.DialogConteiner.activeInHierarchy && !this.isShow)
      return;
    this.isShow = false;
    UITweener[] tweens = ((Component) this).gameObject.GetComponentsInChildren<UITweener>();
    if (tweens.Length == 0)
      return;
    int finishCount = 0;
    EventDelegate.Callback onFinish = (EventDelegate.Callback) (() =>
    {
      if (!Object.op_Inequality((Object) this.DialogConteiner, (Object) null) || ++finishCount < tweens.Length)
        return;
      this.DialogConteiner.SetActive(false);
    });
    ((IEnumerable<UITweener>) tweens).ForEach<UITweener>((Action<UITweener>) (c =>
    {
      c.onFinished.Clear();
      c.AddOnFinished(onFinish);
      c.PlayReverse();
    }));
  }

  private void Update()
  {
    if (!this.isShow || !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2) && (double) Input.GetAxis("Mouse ScrollWheel") == 0.0)
      return;
    this.Hide();
  }
}
