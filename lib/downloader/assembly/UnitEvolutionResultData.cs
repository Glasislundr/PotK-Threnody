// Decompiled with JetBrains decompiler
// Type: UnitEvolutionResultData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class UnitEvolutionResultData : MonoBehaviour
{
  private static UnitEvolutionResultData Instance;
  private UnitEvolutionResultData.ResultData resultData;

  public static UnitEvolutionResultData GetInstance()
  {
    if (Object.op_Equality((Object) UnitEvolutionResultData.Instance, (Object) null))
    {
      GameObject gameObject = GameObject.Find("Evolution Manager");
      if (Object.op_Equality((Object) gameObject, (Object) null))
      {
        gameObject = new GameObject("Evolution Manager");
        Object.DontDestroyOnLoad((Object) gameObject);
      }
      UnitEvolutionResultData.Instance = gameObject.GetComponent<UnitEvolutionResultData>();
      if (Object.op_Equality((Object) UnitEvolutionResultData.Instance, (Object) null))
        UnitEvolutionResultData.Instance = gameObject.AddComponent<UnitEvolutionResultData>();
    }
    return UnitEvolutionResultData.Instance;
  }

  public UnitEvolutionResultData.ResultData GetData() => this.resultData;

  public void SetData(WebAPI.Response.UnitEvolution data)
  {
    this.resultData = (UnitEvolutionResultData.ResultData) null;
    this.resultData = new UnitEvolutionResultData.ResultData();
    this.resultData.unlockQuests = data.unlock_quests;
  }

  public IEnumerator CharacterStoryPopup()
  {
    if (this.resultData != null)
    {
      Future<GameObject> prefab = Res.Prefabs.battle.popup_020_11_2__anim_popup01.Load<GameObject>();
      IEnumerator e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (this.resultData.unlockQuests != null && this.resultData.unlockQuests.Length != 0)
      {
        UnlockQuest[] unlockQuestArray = this.resultData.unlockQuests;
        for (int index = 0; index < unlockQuestArray.Length; ++index)
        {
          QuestCharacterS quest = MasterData.QuestCharacterS[unlockQuestArray[index].quest_s_id];
          Singleton<NGSoundManager>.GetInstance().playSE("SE_1028", delay: 0.8f);
          Battle020112Menu o = this.OpenPopup(prefab.Result).GetComponent<Battle020112Menu>();
          e = o.Init(quest);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          bool isFinished = false;
          o.SetCallback((Action) (() => isFinished = true));
          while (!isFinished)
            yield return (object) null;
          yield return (object) new WaitForSeconds(0.6f);
          o = (Battle020112Menu) null;
        }
        unlockQuestArray = (UnlockQuest[]) null;
        this.resultData.unlockQuests = (UnlockQuest[]) null;
      }
    }
  }

  public GameObject OpenPopup(GameObject original)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(original);
    ((Component) gameObject.transform.parent.Find("Popup Mask")).gameObject.GetComponent<TweenAlpha>().to = 0.75f;
    return gameObject;
  }

  public class ResultData
  {
    public UnlockQuest[] unlockQuests { get; set; }

    public UnlockQuest[] GetUnlockQuestData() => this.unlockQuests;
  }
}
