// Decompiled with JetBrains decompiler
// Type: Quest052171QuestOpenPopup
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
public class Quest052171QuestOpenPopup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel description;
  [SerializeField]
  private UILabel Title;
  [SerializeField]
  private UIButton ibtnYes;
  [SerializeField]
  private UIButton ibtnNo;
  [SerializeField]
  private List<GameObject> Banners;
  [SerializeField]
  private UI2DSprite keySprite;
  [SerializeField]
  private UILabel txtPossession;
  private UIGrid grid;
  private NGxScroll ngxScroll;
  private Quest052171Menu menu;
  private Quest052171Scroll scrollcomp;
  private EarthExtraQuest quest;
  private MasterDataTable.EarthQuestKey questKey;

  public IEnumerator Init(
    EarthExtraQuest quest,
    MasterDataTable.EarthQuestKey questKey,
    Quest052171Menu menu,
    Quest052171Scroll scrollcomp)
  {
    this.menu = menu;
    this.scrollcomp = scrollcomp;
    this.quest = quest;
    this.questKey = questKey;
    int num = 0;
    Earth.EarthQuestKey earthQuestKey = ((IEnumerable<Earth.EarthQuestKey>) Singleton<EarthDataManager>.GetInstance().GetQuestKeys()).Where<Earth.EarthQuestKey>((Func<Earth.EarthQuestKey, bool>) (x => x.keyID == questKey.ID)).FirstOrDefault<Earth.EarthQuestKey>();
    if (earthQuestKey != null)
      num = earthQuestKey.quantity;
    this.txtPossession.SetTextLocalize(num);
    this.Title.SetText(Consts.GetInstance().QUEST_002171_RELEASEPOPUP_TITLE);
    IEnumerator e = this.CreateKeySprite(questKey.ID);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.description.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_052171_RELEASEPOPUP_DESCRIPTION, (IDictionary) new Hashtable()
    {
      {
        (object) "name",
        (object) questKey.name
      },
      {
        (object) "quantity",
        (object) quest.use_key_num.ToLocalizeNumberText()
      },
      {
        (object) "count",
        (object) quest.clear_limit
      }
    }));
  }

  public void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    ((UIButtonColor) this.ibtnYes).isEnabled = false;
    ((UIButtonColor) this.ibtnNo).isEnabled = false;
    Singleton<PopupManager>.GetInstance().dismiss();
    this.menu.StartAPI_QuestRelease(this.quest, this.questKey, this.scrollcomp.CanPlay);
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  private IEnumerator CreateKeySprite(int keyID)
  {
    Future<Sprite> spriteF = MasterData.EarthQuestKey[keyID].LoadSpriteThumbnail();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.keySprite.sprite2D = spriteF.Result;
  }
}
