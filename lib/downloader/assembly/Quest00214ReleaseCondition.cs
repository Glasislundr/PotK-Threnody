// Decompiled with JetBrains decompiler
// Type: Quest00214ReleaseCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Quest00214ReleaseCondition : MonoBehaviour
{
  public NGxScroll scroll;

  public IEnumerator InitRelease(
    List<QuestDisplayConditionConverter> list,
    GameObject iconPrefab,
    GameObject conditionPrefab)
  {
    this.scroll.Clear();
    foreach (QuestDisplayConditionConverter data in list)
    {
      GameObject gameObject = conditionPrefab.Clone();
      this.scroll.Add(gameObject);
      IEnumerator e = gameObject.GetComponent<Quest00214aScroll>().Init(iconPrefab, data);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.scroll.ResolvePosition();
  }

  public void StartTween(bool order)
  {
    foreach (UITweener component in ((Component) this).GetComponents<UITweener>())
    {
      if (component.tweenGroup == 1)
        component.Play(order);
    }
  }

  public void StartTweenClick(bool order)
  {
    foreach (UITweener component in ((Component) this).GetComponents<UITweener>())
    {
      if (component.tweenGroup == 2)
        component.Play(order);
    }
  }
}
