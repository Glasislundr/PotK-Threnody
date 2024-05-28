// Decompiled with JetBrains decompiler
// Type: BattleSceneBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class BattleSceneBase : NGSceneBase
{
  public override IEnumerator onInitSceneAsync()
  {
    BattleSceneBase battleSceneBase = this;
    BattleMonoBehaviour[] battleMonoBehaviourArray = ((Component) battleSceneBase).GetComponentsInChildren<BattleMonoBehaviour>(true);
    int index;
    IEnumerator e;
    for (index = 0; index < battleMonoBehaviourArray.Length; ++index)
    {
      e = battleMonoBehaviourArray[index].onBattleInitSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    battleMonoBehaviourArray = (BattleMonoBehaviour[]) null;
    NGBattleMenuBase[] ngBattleMenuBaseArray = ((Component) battleSceneBase).GetComponentsInChildren<NGBattleMenuBase>(true);
    for (index = 0; index < ngBattleMenuBaseArray.Length; ++index)
    {
      e = ngBattleMenuBaseArray[index].onBattleInitSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    ngBattleMenuBaseArray = (NGBattleMenuBase[]) null;
  }

  public override void onSceneInitialized() => Singleton<NGBattleManager>.GetInstance();

  public override IEnumerator onEndSceneAsync()
  {
    BattleSceneBase battleSceneBase = this;
    float timeOutTime = 0.5f;
    BattleMonoBehaviour[] battleMonoBehaviourArray = ((Component) battleSceneBase).GetComponentsInChildren<BattleMonoBehaviour>();
    int index;
    float startTime;
    for (index = 0; index < battleMonoBehaviourArray.Length; ++index)
    {
      BattleMonoBehaviour m = battleMonoBehaviourArray[index];
      startTime = Time.time;
      while (!m.isStartInitialized())
      {
        yield return (object) null;
        if ((double) Time.time - (double) startTime > (double) timeOutTime)
          break;
      }
      m = (BattleMonoBehaviour) null;
    }
    battleMonoBehaviourArray = (BattleMonoBehaviour[]) null;
    NGBattleMenuBase[] ngBattleMenuBaseArray = ((Component) battleSceneBase).GetComponentsInChildren<NGBattleMenuBase>();
    for (index = 0; index < ngBattleMenuBaseArray.Length; ++index)
    {
      NGBattleMenuBase mb = ngBattleMenuBaseArray[index];
      startTime = Time.time;
      while (!mb.isStartInitialized())
      {
        yield return (object) null;
        if ((double) Time.time - (double) startTime > (double) timeOutTime)
          break;
      }
      mb = (NGBattleMenuBase) null;
    }
    ngBattleMenuBaseArray = (NGBattleMenuBase[]) null;
  }
}
