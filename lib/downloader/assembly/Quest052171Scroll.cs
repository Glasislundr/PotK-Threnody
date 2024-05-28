// Decompiled with JetBrains decompiler
// Type: Quest052171Scroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest052171Scroll : BannerBase
{
  [SerializeField]
  private UISprite Clear;
  [SerializeField]
  private UISprite New;
  [SerializeField]
  private SpriteDecimalControl possessionDigit;
  [SerializeField]
  private GameObject possessionObj;
  [SerializeField]
  private GameObject timeText;
  private bool canPlay;
  private Quest052171Menu menu;

  public bool CanPlay => this.canPlay;

  public IEnumerator InitScroll(
    EarthExtraQuest quest,
    MasterDataTable.EarthQuestKey questKey,
    bool isPlay,
    Quest052171Menu menu)
  {
    Quest052171Scroll quest052171Scroll = this;
    quest052171Scroll.menu = menu;
    int id = questKey.ID;
    quest052171Scroll.canPlay = isPlay;
    string spritePath = quest052171Scroll.GetSpritePath(id, quest052171Scroll.canPlay);
    IEnumerator e = quest052171Scroll.SetAndCreate_BannerSprite(spritePath, quest052171Scroll.IdleSprite);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    int keyNum = 0;
    Earth.EarthQuestKey earthQuestKey = ((IEnumerable<Earth.EarthQuestKey>) Singleton<EarthDataManager>.GetInstance().GetQuestKeys()).Where<Earth.EarthQuestKey>((Func<Earth.EarthQuestKey, bool>) (x => x.keyID == questKey.ID)).FirstOrDefault<Earth.EarthQuestKey>();
    if (earthQuestKey != null)
      keyNum = earthQuestKey.quantity;
    quest052171Scroll.SetPossession(keyNum);
    quest052171Scroll.SetScrollButtonCondition(quest, questKey, quest052171Scroll.canPlay);
    quest052171Scroll.timeText.SetActive(false);
  }

  private string GetSpritePath(int id, bool canplay)
  {
    return BannerBase.GetSpriteIdlePath(id, BannerBase.Type.quest_lock, QuestExtra.SeekType.None, canplay, true);
  }

  private IEnumerator SetAndCreate_BannerSprite(string path, UI2DSprite obj)
  {
    if (!Singleton<ResourceManager>.GetInstance().Contains(path))
    {
      Debug.LogWarning((object) path);
      path = string.Format("Prefabs/Banners/ExtraQuest/M/1/Specialquest_idle");
    }
    Future<Texture2D> future = Singleton<ResourceManager>.GetInstance().Load<Texture2D>(path);
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Texture2D result = future.Result;
    if (!Object.op_Equality((Object) result, (Object) null))
    {
      Sprite sprite = Sprite.Create(result, new Rect(0.0f, 0.0f, (float) ((Texture) result).width, (float) ((Texture) result).height), new Vector2(0.5f, 0.5f), 1f, 100U, (SpriteMeshType) 0);
      ((Object) sprite).name = ((Object) result).name;
      obj.sprite2D = sprite;
    }
  }

  private void SetScrollButtonCondition(
    EarthExtraQuest quest,
    MasterDataTable.EarthQuestKey questKey,
    bool canPlay)
  {
    if (canPlay)
      EventDelegate.Set(this.BtnFormation.onClick, (EventDelegate.Callback) (() => this.changeScene(quest)));
    else
      EventDelegate.Set(this.BtnFormation.onClick, (EventDelegate.Callback) (() => this.StartQuestReleasePopup(quest, questKey)));
  }

  private void changeScene(EarthExtraQuest quest) => Quest0528Scene.changeScene(true, quest);

  public void StartQuestReleasePopup(EarthExtraQuest quest, MasterDataTable.EarthQuestKey questKey)
  {
    this.StartCoroutine(this.OpenQuestReleasePopup(quest, questKey, this.menu, this));
  }

  private IEnumerator OpenQuestReleasePopup(
    EarthExtraQuest quest,
    MasterDataTable.EarthQuestKey questKey,
    Quest052171Menu menu,
    Quest052171Scroll scroll)
  {
    Future<GameObject> popupF = Res.Prefabs.quest052_17_1.quest_release_popup.Load<GameObject>();
    IEnumerator e = popupF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject popup = Singleton<PopupManager>.GetInstance().open(popupF.Result);
    popup.SetActive(false);
    e = popup.GetComponent<Quest052171QuestOpenPopup>().Init(quest, questKey, menu, scroll);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) null;
    popup.SetActive(true);
  }

  private void SetPossession(int keyNum)
  {
    this.possessionObj.SetActive(true);
    this.possessionDigit.setNumber(keyNum);
  }
}
