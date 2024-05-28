// Decompiled with JetBrains decompiler
// Type: Story00912Menu
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
public class Story00912Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  private NGxScroll ScrollContainer;
  [SerializeField]
  private GameObject DynCharacter;
  [SerializeField]
  private GameObject DynCharacter2;
  [SerializeField]
  private GameObject DynCharacter3;
  public Texture2D mask_right;
  public Texture2D mask_left;

  public override void onBackButton() => this.IbtnBack();

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("story009_10", false);
  }

  public void ScrollContainerResolvePosition() => this.ScrollContainer.ResolvePosition();

  private IEnumerator LoadCharacterSprite(
    int id,
    GameObject locationObject,
    Texture2D maskTexture,
    int position_x,
    int position_y,
    float sprite_scale)
  {
    locationObject.transform.Clear();
    UnitUnit unit = MasterData.UnitUnit[id];
    Future<GameObject> f = unit.LoadMypage();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject obj = f.Result.Clone(locationObject.transform);
    e = unit.SetLargeSpriteSnap(obj, 5);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    NGxMaskSpriteWithScale component = obj.GetComponent<NGxMaskSpriteWithScale>();
    component.maskTexture = maskTexture;
    component.xOffsetPixel = position_x;
    component.yOffsetPixel = position_y;
    if ((double) sprite_scale == 0.0)
      sprite_scale = 1f;
    component.scale = sprite_scale;
    component.FitMask();
  }

  private IEnumerator LoadMask()
  {
    Future<Texture2D> maskF = Res.GUI._009_12_sozai.mask_left.Load<Texture2D>();
    IEnumerator e = maskF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mask_left = maskF.Result;
    maskF = Res.GUI._009_12_sozai.mask_right.Load<Texture2D>();
    e = maskF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mask_right = maskF.Result;
  }

  private IEnumerator SetCharacterLargeImage(int id, int id2, int id3, QuestHarmonyS questS)
  {
    IEnumerator e = this.LoadMask();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.LoadCharacterSprite(id, this.DynCharacter, this.mask_right, questS.unit_x, questS.unit_y, questS.unit_scale);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.LoadCharacterSprite(id2, this.DynCharacter2, this.mask_left, questS.target_unit_x, questS.target_unit_y, questS.target_unit_scale);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.LoadCharacterSprite(id3, this.DynCharacter3, this.mask_right, questS.target_unit2_x, questS.target_unit2_y, questS.target_unit2_scale);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator CreateEpisodes(int id1, int id2, int id3)
  {
    Story00912Menu menu = this;
    Future<GameObject> prefabEpisodeF = Res.Prefabs.story009_6.story0096DirEpisode.Load<GameObject>();
    IEnumerator e = prefabEpisodeF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabEpisodeF.Result;
    IEnumerable<PlayerHarmonyQuestS> source = ((IEnumerable<PlayerHarmonyQuestS>) SMManager.Get<PlayerHarmonyQuestS[]>()).Where<PlayerHarmonyQuestS>((Func<PlayerHarmonyQuestS, bool>) (x => x.is_clear));
    List<PlayerHarmonyQuestS> questsList = new List<PlayerHarmonyQuestS>();
    List<StoryPlaybackHarmonyDetail> playbackList = new List<StoryPlaybackHarmonyDetail>();
    List<Story0093Scene.ContinuousData> continuousDataList = new List<Story0093Scene.ContinuousData>();
    source.OrderBy<PlayerHarmonyQuestS, int>((Func<PlayerHarmonyQuestS, int>) (x => x.quest_harmony_s.ID)).ForEach<PlayerHarmonyQuestS>((Action<PlayerHarmonyQuestS>) (qu =>
    {
      if (qu.quest_harmony_s.unit.ID != id1 || qu.quest_harmony_s.target_unit.ID != id2 || qu.quest_harmony_s.target_unit2.ID != id3)
        return;
      ((IEnumerable<StoryPlaybackHarmonyDetail>) MasterData.StoryPlaybackHarmonyDetailList).Where<StoryPlaybackHarmonyDetail>((Func<StoryPlaybackHarmonyDetail, bool>) (x => x.quest.ID == qu.quest_harmony_s.ID && x.timing_StoryPlaybackTiming != 2)).OrderBy<StoryPlaybackHarmonyDetail, int>((Func<StoryPlaybackHarmonyDetail, int>) (x => x.ID)).ForEach<StoryPlaybackHarmonyDetail>((Action<StoryPlaybackHarmonyDetail>) (story =>
      {
        questsList.Add(qu);
        playbackList.Add(story);
        continuousDataList.Add(new Story0093Scene.ContinuousData()
        {
          scriptId_ = story.script_id,
          continuousFlag_ = story.continuous_flag
        });
      }));
    }));
    for (int index = 0; index < questsList.Count; ++index)
    {
      GameObject gameObject = result.Clone();
      menu.ScrollContainer.Add(gameObject);
      gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, (float) -index * menu.ScrollContainer.grid.cellHeight, gameObject.transform.localPosition.z);
      gameObject.GetComponent<AnimationApplyBase>().Reset();
      gameObject.GetComponent<Story0096EpisodeParts>().setData(questsList[index], playbackList[index], (NGMenuBase) menu, continuousDataList);
    }
  }

  public IEnumerator Init(int id, int id2, int id3, int questSId)
  {
    this.TxtTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().TRIO_QUEST_TITLE, (IDictionary) new Hashtable()
    {
      {
        (object) "name",
        (object) MasterData.UnitUnit[id].name
      },
      {
        (object) "name2",
        (object) MasterData.UnitUnit[id2].name
      },
      {
        (object) "name3",
        (object) MasterData.UnitUnit[id3].name
      }
    }));
    QuestHarmonyS questS = ((IEnumerable<QuestHarmonyS>) MasterData.QuestHarmonySList).FirstOrDefault<QuestHarmonyS>((Func<QuestHarmonyS, bool>) (x => x.ID == questSId));
    IEnumerator e = this.SetCharacterLargeImage(id, id2, id3, questS);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ScrollContainer.Clear();
    e = this.CreateEpisodes(id, id2, id3);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ScrollContainer.ResolvePosition();
  }
}
