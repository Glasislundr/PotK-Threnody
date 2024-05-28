// Decompiled with JetBrains decompiler
// Type: Startup00014Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Startup00014Menu : BackButtonMenuBase
{
  [SerializeField]
  protected List<IconPosition> positionList = new List<IconPosition>();
  protected List<Transform> listIcons = new List<Transform>();

  public Startup00014Scene Parent { set; get; }

  public virtual IEnumerator InitSceneAsync(PlayerLoginBonus lB)
  {
    yield return (object) null;
  }

  public void Play(Transform trans)
  {
    foreach (Transform tran in trans)
      this.Play(tran);
    List<UITweener> uiTweenerList = new List<UITweener>();
    foreach (UITweener uiTweener in ((IEnumerable<UITweener>) ((Component) trans).gameObject.GetComponentsInChildren<UITweener>()).ToList<UITweener>())
      uiTweenerList.Add(uiTweener);
    uiTweenerList.ForEach((Action<UITweener>) (x => ((Behaviour) x).enabled = true));
    uiTweenerList.ForEach((Action<UITweener>) (x => x.ResetToBeginning()));
    uiTweenerList.ForEach((Action<UITweener>) (x => x.PlayForward()));
  }

  public override void onBackButton() => this.showBackKeyToast();
}
