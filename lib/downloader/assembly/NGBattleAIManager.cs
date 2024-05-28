// Decompiled with JetBrains decompiler
// Type: NGBattleAIManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using AI.Logic;
using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class NGBattleAIManager : BattleManagerBase
{
  public AILogicBase ai;

  public override IEnumerator initialize(BattleInfo battleInfo, BE env_ = null)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    NGBattleAIManager ngBattleAiManager = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    env_ = env_ ?? Singleton<NGBattleManager>.GetInstance().environment;
    BattleFuncs.createAsterNodeCache(env_.core);
    ngBattleAiManager.ai = ((Component) ngBattleAiManager).gameObject.AddComponent<BasicAI>().aiLogic;
    return false;
  }

  public override IEnumerator cleanup()
  {
    this.ai = (AILogicBase) null;
    yield break;
  }

  public void clearCache()
  {
    if (!(this.ai is LispAILogic ai))
      return;
    ai.clearCache();
  }
}
