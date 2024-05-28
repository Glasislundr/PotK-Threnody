// Decompiled with JetBrains decompiler
// Type: CorpsQuestStageRewardPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class CorpsQuestStageRewardPopup : BackButtonMenuBase
{
  [SerializeField]
  private UIGrid Grid;
  [SerializeField]
  private UIScrollView ScrollView;

  public IEnumerator Initialize(int stageId, bool isCleared)
  {
    Future<GameObject> f = new ResourceObject("Prefabs/versus026_12/slc_Reward_Box").Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = f.Result.CloneAndGetComponent<Versus02612ScrollRewardBox>(((Component) this.Grid).gameObject).InitCorpsStage(stageId, isCleared);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ScrollView.ResetPosition();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
