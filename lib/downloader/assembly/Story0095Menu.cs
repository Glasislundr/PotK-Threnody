// Decompiled with JetBrains decompiler
// Type: Story0095Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Story0095Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private NGxScroll ScrollContainer;
  [SerializeField]
  private GameObject dirNoStory;

  public virtual void Foreground()
  {
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_0", false);
  }

  public virtual void VScrollBar()
  {
  }

  public IEnumerator InitScene(bool connect)
  {
    Story0095Menu story0095Menu = this;
    IEnumerator e;
    if (connect)
    {
      string KeyWebAPI = "QuestProgressCharacter";
      if (!WebAPI.IsResponsedAtContainsKey(KeyWebAPI))
      {
        // ISSUE: reference to a compiler-generated method
        Future<WebAPI.Response.QuestProgressCharacter> ft = WebAPI.QuestProgressCharacter(new Action<WebAPI.Response.UserError>(story0095Menu.\u003CInitScene\u003Eb__7_0));
        e = ft.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (ft.Result == null)
        {
          yield break;
        }
        else
        {
          WebAPI.SetLatestResponsedAt(KeyWebAPI);
          WebAPI.SetLatestResponsedAt("QuestProgressHarmony");
          ft = (Future<WebAPI.Response.QuestProgressCharacter>) null;
        }
      }
      KeyWebAPI = (string) null;
    }
    PlayerCharacterQuestS[] characterQuests = SMManager.Get<PlayerCharacterQuestS[]>().M();
    e = story0095Menu.InitCharacterQuestButton(characterQuests);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator InitCharacterQuestButton(PlayerUnit[] characterQuests)
  {
    Story0095Menu story0095Menu = this;
    Future<GameObject> unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = unitIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject unitIconPrefab = unitIconPrefabF.Result;
    story0095Menu.ScrollContainer.Clear();
    story0095Menu.dirNoStory.SetActive(characterQuests.Length == 0);
    characterQuests = ((IEnumerable<PlayerUnit>) characterQuests).OrderBy<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.unit.history_group_number)).ToArray<PlayerUnit>();
    PlayerUnit[] playerUnitArray = characterQuests;
    for (int index = 0; index < playerUnitArray.Length; ++index)
    {
      PlayerUnit playerUnit = playerUnitArray[index];
      GameObject gameObject = Object.Instantiate<GameObject>(unitIconPrefab);
      story0095Menu.ScrollContainer.Add(gameObject);
      UnitIcon unitIcon = gameObject.GetComponent<UnitIcon>();
      PlayerUnit[] playerUnits = new PlayerUnit[1]
      {
        playerUnit
      };
      e = unitIcon.SetPlayerUnit(playerUnit, playerUnits, (PlayerUnit) null, false, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      // ISSUE: reference to a compiler-generated method
      unitIcon.onClick = new Action<UnitIconBase>(story0095Menu.\u003CInitCharacterQuestButton\u003Eb__8_1);
      unitIcon = (UnitIcon) null;
    }
    playerUnitArray = (PlayerUnit[]) null;
    story0095Menu.ScrollContainer.ResolvePosition();
  }

  public IEnumerator InitCharacterQuestButton(PlayerCharacterQuestS[] characterQuests)
  {
    Story0095Menu story0095Menu = this;
    Future<GameObject> unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = unitIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject unitIconPrefab = unitIconPrefabF.Result;
    story0095Menu.ScrollContainer.Clear();
    story0095Menu.dirNoStory.SetActive(characterQuests.Length == 0);
    characterQuests = ((IEnumerable<PlayerCharacterQuestS>) characterQuests).OrderBy<PlayerCharacterQuestS, int>((Func<PlayerCharacterQuestS, int>) (x => x.quest_character_s.unit.history_group_number)).ToArray<PlayerCharacterQuestS>();
    PlayerCharacterQuestS[] playerCharacterQuestSArray = characterQuests;
    for (int index = 0; index < playerCharacterQuestSArray.Length; ++index)
    {
      UnitUnit unit = playerCharacterQuestSArray[index].quest_character_s.unit;
      GameObject gameObject = Object.Instantiate<GameObject>(unitIconPrefab);
      story0095Menu.ScrollContainer.Add(gameObject);
      UnitIcon unitIcon = gameObject.GetComponent<UnitIcon>();
      e = unitIcon.SetUnit(unit, unit.GetElement(), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      // ISSUE: reference to a compiler-generated method
      unitIcon.onClick = new Action<UnitIconBase>(story0095Menu.\u003CInitCharacterQuestButton\u003Eb__9_1);
      unitIcon.BottomModeValue = UnitIconBase.BottomMode.Nothing;
      unitIcon = (UnitIcon) null;
    }
    playerCharacterQuestSArray = (PlayerCharacterQuestS[]) null;
    story0095Menu.ScrollContainer.ResolvePosition();
  }

  private void Select(UnitIconBase unitIcon)
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_6", true, (object) unitIcon.Unit.ID);
  }
}
