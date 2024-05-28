// Decompiled with JetBrains decompiler
// Type: BasicAI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using AI.Logic;
using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class BasicAI : BattleMonoBehaviour
{
  private LispAILogic ai = new LispAILogic();

  public AILogicBase aiLogic => (AILogicBase) this.ai;

  protected override IEnumerator Start_Battle()
  {
    BasicAI basicAi = this;
    basicAi.ai.env = basicAi.env.core;
    basicAi.ai.cleanup();
    IEnumerator e = basicAi.loadScript();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    basicAi.StartCoroutine(basicAi.ai.doExecute());
  }

  private IEnumerator loadScript()
  {
    BasicAI basicAi = this;
    Future<TextAsset> scriptF = Singleton<ResourceManager>.GetInstance().Load<TextAsset>("AI/ai_script_bytes");
    IEnumerator e = scriptF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<TextAsset> numberF = Singleton<ResourceManager>.GetInstance().Load<TextAsset>("AI/ai_script_number_bytes");
    e = numberF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    object sexp = basicAi.ai.readLisp(scriptF.Result.bytes, numberF.Result.bytes);
    e = basicAi.ai.createEngine(sexp);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    foreach (BL.UnitPosition unitPosition in basicAi.env.core.unitPositions.value)
    {
      e = unitPosition.unit.InitAIExtention(basicAi.ai);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }
}
