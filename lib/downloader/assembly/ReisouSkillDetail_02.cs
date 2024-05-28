// Decompiled with JetBrains decompiler
// Type: ReisouSkillDetail_02
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ReisouSkillDetail_02 : MonoBehaviour
{
  [SerializeField]
  protected UIDragScrollView uiDragScrollView;
  [SerializeField]
  protected UILabel[] txtStatus;

  public IEnumerator Init(UIScrollView scrollView, List<string> paramTextList)
  {
    this.uiDragScrollView.scrollView = scrollView;
    int length = this.txtStatus.Length;
    for (int index = 0; index < length; ++index)
      this.txtStatus[index].SetTextLocalize("");
    for (int index = 0; index < paramTextList.Count; ++index)
      this.txtStatus[index].SetTextLocalize(paramTextList[index]);
    yield break;
  }
}
