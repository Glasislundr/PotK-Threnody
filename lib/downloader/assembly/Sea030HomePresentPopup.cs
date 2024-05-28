// Decompiled with JetBrains decompiler
// Type: Sea030HomePresentPopup
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
public class Sea030HomePresentPopup : BackButtonMonoBehaiviour
{
  [SerializeField]
  private GameObject[] presentIconContainers;
  [SerializeField]
  private UIScrollView scroll;
  private PlayerUnit currentUnit;
  private GameObject popupPrefab;
  private Action<GameCore.ItemInfo, int> apiCall;

  public IEnumerator Init(PlayerUnit unit, Action<GameCore.ItemInfo, int> apiCallback)
  {
    Sea030HomePresentPopup homePresentPopup = this;
    homePresentPopup.apiCall = apiCallback;
    homePresentPopup.currentUnit = unit;
    GearGear[] gears = ((IEnumerable<GearGear>) MasterData.GearGearList).Where<GearGear>((Func<GearGear, bool>) (x => x.kind.Enum == GearKindEnum.sea_present)).OrderByDescending<GearGear, int>((Func<GearGear, int>) (x => x.rarity.index)).ThenBy<GearGear, DateTime>((Func<GearGear, DateTime>) (x => x.published_at)).ThenBy<GearGear, int>((Func<GearGear, int>) (x => x.ID)).ToArray<GearGear>();
    IEnumerable<PlayerMaterialGear> presents = ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).Where<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => x.gear.kind.Enum == GearKindEnum.sea_present));
    Future<GameObject> popupFuture = Res.Prefabs.popup.popup_030_sea_present_confirm__anim_fade.Load<GameObject>();
    IEnumerator e = popupFuture.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    homePresentPopup.popupPrefab = popupFuture.Result;
    Future<GameObject> iconFuture = Res.Prefabs.Sea.ItemIcon.prefab_sea.Load<GameObject>();
    e = iconFuture.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject iconPregab = iconFuture.Result;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    int gearsCount = gears.Length;
    for (int i = 0; i < homePresentPopup.presentIconContainers.Length; ++i)
    {
      if (gearsCount <= i)
      {
        homePresentPopup.presentIconContainers[i].SetActive(false);
      }
      else
      {
        homePresentPopup.presentIconContainers[i].SetActive(true);
        GearGear gear = gears[i];
        PlayerMaterialGear playerItem = presents.FirstOrDefault<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => x.gear_id == gear.ID));
        ItemIcon itemIcon = iconPregab.CloneAndGetComponent<ItemIcon>(homePresentPopup.presentIconContainers[i]);
        if (playerItem == (PlayerMaterialGear) null)
        {
          if (gear.published_at <= ServerTime.NowAppTimeAddDelta())
          {
            e = itemIcon.InitByGear(gear);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            itemIcon.Gray = true;
          }
          else
          {
            e = itemIcon.InitByGear((GearGear) null);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            itemIcon.BottomModeValue = ItemIcon.BottomMode.Nothing;
            itemIcon.Gray = true;
          }
        }
        else
        {
          e = itemIcon.InitByPlayerMaterialGear(playerItem);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          itemIcon.Gray = false;
          itemIcon.onClick = new Action<ItemIcon>(homePresentPopup.clickFunc);
        }
        itemIcon = (ItemIcon) null;
      }
    }
    homePresentPopup.scroll.ResetPosition();
  }

  private void clickFunc(ItemIcon icon)
  {
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    instance.loadingMode = 1;
    instance.isLoading = true;
    this.StartCoroutine(this.OpenPresentConfirmPopup(icon.ItemInfo));
  }

  private IEnumerator OpenPresentConfirmPopup(GameCore.ItemInfo info)
  {
    CommonRoot cr = Singleton<CommonRoot>.GetInstance();
    GameObject popup = this.popupPrefab.Clone(cr.LoadTmpObj.transform);
    IEnumerator e = popup.GetComponent<Sea030HomePresentConfirmPopup>().Init(this.currentUnit, info, this.apiCall);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    cr.isLoading = false;
    cr.loadingMode = 0;
    popup.SetActive(false);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    popup.SetActive(true);
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();

  public void IbtnQuest()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    Sea030_questScene.ChangeScene(true);
  }
}
