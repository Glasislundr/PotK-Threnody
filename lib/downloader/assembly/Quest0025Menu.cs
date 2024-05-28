// Decompiled with JetBrains decompiler
// Type: Quest0025Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest0025Menu : BackButtonMenuBase
{
  public BGChange bgchange;
  public Quest0025CircularMotionSet circul;
  private GameObject jog;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private Transform MainPanel;
  private PlayerStoryQuestS[] StoryData;
  private Quest00240723Menu.StoryMode storyMode = Quest00240723Menu.StoryMode.None;

  public virtual void IbtnStoryL() => Debug.Log((object) "click default event IbtnStoryL");

  public IEnumerator Init(
    PlayerStoryQuestS[] StoryData,
    int XLNum,
    int LNum,
    bool Hard,
    bool restart = false)
  {
    Quest0025Menu quest0025Menu = this;
    quest0025Menu.StoryData = StoryData;
    if ((Quest00240723Menu.StoryMode) XLNum != quest0025Menu.storyMode)
      restart = true;
    quest0025Menu.storyMode = (Quest00240723Menu.StoryMode) XLNum;
    IEnumerable<int> L_Ids = ((IEnumerable<PlayerStoryQuestS>) StoryData).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_xl_QuestStoryXL == XLNum)).Select<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l_QuestStoryL)).Distinct<int>();
    L_Ids.ForEach<int>((Action<int>) (x => Debug.LogWarning((object) ("解放章：" + (object) x))));
    if (restart)
    {
      quest0025Menu.circul = (Quest0025CircularMotionSet) null;
      if (Object.op_Inequality((Object) quest0025Menu.jog, (Object) null))
      {
        Object.Destroy((Object) quest0025Menu.jog);
        quest0025Menu.jog = (GameObject) null;
      }
    }
    if (Object.op_Equality((Object) quest0025Menu.circul, (Object) null))
    {
      Future<GameObject> Hscroll;
      IEnumerator e;
      if (quest0025Menu.storyMode == Quest00240723Menu.StoryMode.Heaven)
      {
        Hscroll = Res.Prefabs.quest002_5.Bottom.Load<GameObject>();
        e = Hscroll.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject result = Hscroll.Result;
        quest0025Menu.jog = result.Clone(quest0025Menu.MainPanel);
        Hscroll = (Future<GameObject>) null;
      }
      else if (quest0025Menu.storyMode == Quest00240723Menu.StoryMode.LostRagnarok)
      {
        Hscroll = new ResourceObject("Prefabs/quest002_5/Bottom_LostRagnarok").Load<GameObject>();
        e = Hscroll.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject result = Hscroll.Result;
        quest0025Menu.jog = result.Clone(quest0025Menu.MainPanel);
        Hscroll = (Future<GameObject>) null;
      }
      else if (quest0025Menu.storyMode == Quest00240723Menu.StoryMode.IntegralNoah)
      {
        Hscroll = new ResourceObject("Prefabs/quest002_5/Bottom_IntegralNoah").Load<GameObject>();
        e = Hscroll.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject result = Hscroll.Result;
        quest0025Menu.jog = result.Clone(quest0025Menu.MainPanel);
        Hscroll = (Future<GameObject>) null;
      }
      else if (quest0025Menu.storyMode == Quest00240723Menu.StoryMode.EverAfter)
      {
        Hscroll = new ResourceObject("Prefabs/quest002_5/Bottom_EverAfter").Load<GameObject>();
        e = Hscroll.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject result = Hscroll.Result;
        quest0025Menu.jog = result.Clone(quest0025Menu.MainPanel);
        Hscroll = (Future<GameObject>) null;
      }
      quest0025Menu.circul = quest0025Menu.jog.GetComponentInChildren<Quest0025CircularMotionSet>();
      quest0025Menu.circul.bgchange = quest0025Menu.bgchange;
    }
    if (Object.op_Inequality((Object) quest0025Menu.circul, (Object) null))
    {
      foreach (Quest0025SlidePanelDragged componentsInChild in ((Component) quest0025Menu.circul).GetComponentsInChildren<Quest0025SlidePanelDragged>())
      {
        componentsInChild.Initialize(((Component) quest0025Menu).GetComponent<BGChange>());
        if (!L_Ids.Contains<int>(componentsInChild.Lid))
        {
          componentsInChild.Release = false;
        }
        else
        {
          componentsInChild.Release = true;
          if (LNum == componentsInChild.Lid)
            quest0025Menu.circul.SetCenter(((Component) componentsInChild).transform);
        }
      }
      quest0025Menu.circul.SetValue();
      quest0025Menu.circul.Hard = Hard;
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    quest0025Menu.IsPush = false;
    PlayerStoryQuestS[] playerStoryQuestSArray = StoryData.L(XLNum);
    if (playerStoryQuestSArray != null && playerStoryQuestSArray.Length != 0)
      quest0025Menu.TxtTitle.SetText(playerStoryQuestSArray[0].quest_story_s.quest_xl.name);
  }

  private IEnumerator ChangeStoryMode(PlayerStoryQuestS[] stories, int XL, int L)
  {
    Quest0025Menu quest0025Menu = this;
    float startTime = Time.time;
    Singleton<NGSceneManager>.GetInstance().sceneBase.endTweens();
    while (!Singleton<NGSceneManager>.GetInstance().sceneBase.isTweenFinished && (double) Time.time - (double) startTime <= (double) Singleton<NGSceneManager>.GetInstance().sceneBase.tweenTimeoutTime)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) new WaitForSeconds(0.1f);
    IEnumerator e = ((Component) quest0025Menu).GetComponent<BGChange>().SetChapterBg((Quest00240723Menu.StoryMode) XL);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>().namePrefab = string.Format("{0}", (object) L);
    Quest0025Scene.changeScene0025(false, new Quest0025Scene.Quest0025Param(XL, L, false, true, false));
  }

  public IEnumerator BGchanger(int Lid)
  {
    IEnumerator e = this.bgchange.QuestBGprefabCreate(Lid);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void btnChangeStoryHeaven()
  {
    Singleton<CommonRoot>.GetInstance().GetHeavenCommonFooter().onButtonQuest();
  }

  public void btnChangeStoryLostRagnarok()
  {
    Singleton<CommonRoot>.GetInstance().GetHeavenCommonFooter().onButtonQuest();
  }

  public override void onBackButton() => this.showBackKeyToast();
}
