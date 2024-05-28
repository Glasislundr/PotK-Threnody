// Decompiled with JetBrains decompiler
// Type: Popup00230HuntingTargetDetail
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
public class Popup00230HuntingTargetDetail : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtTargetPointValue;
  [SerializeField]
  private UILabel txtTargetDescription;
  [SerializeField]
  private CreateIconObject dynThum;
  [SerializeField]
  private UI2DSprite[] gearKindIcons;
  [SerializeField]
  private GameObject dirTargetQuest;
  [SerializeField]
  private GameObject dirTargetQuestNon;
  [SerializeField]
  private NGxScroll scrollContainer;

  private Sprite GetGearKindSprite(GearKind kind)
  {
    string empty = string.Empty;
    return Resources.Load<Sprite>(kind.Enum == GearKindEnum.smith || kind.Enum == GearKindEnum.accessories || kind.Enum == GearKindEnum.dummy || kind.Enum == GearKindEnum.none ? "Icons/Materials/GuideWeaponBtn/slc_unique_wepon_idle" : string.Format("Icons/Materials/GuideWeaponBtn/slc_{0}_idle", (object) kind.Enum.ToString()));
  }

  public IEnumerator Init(
    WebAPI.Response.EventDetail detailData,
    EnemyTopInfo[] infos,
    GameObject questListPrefab)
  {
    int num1 = ((IEnumerable<EnemyTopInfo>) infos).Min<EnemyTopInfo>((Func<EnemyTopInfo, int>) (x => x.min_point));
    int num2 = ((IEnumerable<EnemyTopInfo>) infos).Max<EnemyTopInfo>((Func<EnemyTopInfo, int>) (x => x.min_point));
    if (num1 == num2)
      this.txtTargetPointValue.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_00231_POINT_MIN_MAX_SAME, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) num1
        }
      }));
    else
      this.txtTargetPointValue.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_00231_POINT_MIN_MAX, (IDictionary) new Hashtable()
      {
        {
          (object) "min",
          (object) num1
        },
        {
          (object) "max",
          (object) num2
        }
      }));
    IEnumerator e = this.dynThum.CreateThumbnail(MasterDataTable.CommonRewardType.unit, infos[0].unit_id, visibleBottom: false, isButton: false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((IEnumerable<UI2DSprite>) this.gearKindIcons).ForEach<UI2DSprite>((Action<UI2DSprite>) (x => ((Component) x).gameObject.SetActive(false)));
    List<GearKind> gearKindList = new List<GearKind>();
    ((IEnumerable<EnemyTopInfo>) infos).ForEach<EnemyTopInfo>((Action<EnemyTopInfo>) (x => gearKindList.Add(MasterData.UnitUnit[x.unit_id].initial_gear.kind)));
    gearKindList = gearKindList.Distinct<GearKind>().ToList<GearKind>();
    int num3 = gearKindList.Count<GearKind>() > this.gearKindIcons.Length ? this.gearKindIcons.Length : gearKindList.Count<GearKind>();
    for (int index = 0; index < num3; ++index)
    {
      GearKind kind = gearKindList[index];
      this.gearKindIcons[index].sprite2D = this.GetGearKindSprite(kind);
      ((Component) this.gearKindIcons[index]).gameObject.SetActive(true);
    }
    string text = string.Empty;
    this.scrollContainer.Clear();
    foreach (EnemyDetailInfoQuest_infos questInfo in detailData.enemy_detail_infos.quest_infos)
    {
      text = text + questInfo.enemy_text + "\n";
      GameObject gameObject = Object.Instantiate<GameObject>(questListPrefab);
      gameObject.GetComponent<Popup00230HuntingTargetDetailScroll>().Init(questInfo);
      this.scrollContainer.Add(gameObject);
    }
    this.txtTargetDescription.SetTextLocalize(text);
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().onDismiss();

  public void ResetScrollPosition() => this.scrollContainer.ResolvePosition();
}
