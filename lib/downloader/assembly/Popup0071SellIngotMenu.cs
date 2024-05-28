// Decompiled with JetBrains decompiler
// Type: Popup0071SellIngotMenu
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
public class Popup0071SellIngotMenu : BackButtonMenuBase
{
  [SerializeField]
  [Tooltip("売却アイテムアイコン表示用グリッド")]
  private UIGrid gridItemIcons_;
  [SerializeField]
  [Tooltip("売却アイテム数表示用グリッド")]
  private UIGrid gridItemAmounts_;
  [SerializeField]
  [Tooltip("売却アイテム数表示ノード")]
  private GameObject[] objItemAmounts_;
  [SerializeField]
  [Tooltip("売却アイテム数表示")]
  private UILabel[] txtItemAmounts_;
  [SerializeField]
  [Tooltip("売却総額表示")]
  private UILabel txtZenyAmount_;
  [SerializeField]
  [Tooltip("売却対象ID兼並べる順番")]
  private int[] targetIds_;
  private Action requestSell_;

  public int[] targetIds => this.targetIds_;

  public static IEnumerator show(
    Action<bool> actionEnd,
    Action<WebAPI.Response.UserError> actionWebError = null,
    Action actionReload = null)
  {
    Future<GameObject> ldPrefab = new ResourceObject("Prefabs/shop007_1/popup_007_1_sell_ingot__anim_popup01").Load<GameObject>();
    yield return (object) ldPrefab.Wait();
    GameObject result = ldPrefab.Result;
    ldPrefab = (Future<GameObject>) null;
    HashSet<int> targets = new HashSet<int>((IEnumerable<int>) result.GetComponent<Popup0071SellIngotMenu>().targetIds);
    bool bSell = false;
    PlayerMaterialGear[] source = SMManager.Get<PlayerMaterialGear[]>();
    PlayerMaterialGear[] hasItems = (source != null ? ((IEnumerable<PlayerMaterialGear>) source).Where<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => x.isExchangable() && x.quantity > 0 && targets.Contains(x.gear_id))).ToArray<PlayerMaterialGear>() : (PlayerMaterialGear[]) null) ?? new PlayerMaterialGear[0];
    if (hasItems.Length != 0)
    {
      KeyValuePair<PlayerMaterialGear, long>[] exchanges = Popup0071SellIngotMenu.calcExchangeAmount(targets.Select<int, PlayerMaterialGear>((Func<int, PlayerMaterialGear>) (i => ((IEnumerable<PlayerMaterialGear>) hasItems).FirstOrDefault<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => x.gear_id == i)))).Where<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (y => y != (PlayerMaterialGear) null)).ToArray<PlayerMaterialGear>());
      if (exchanges.Length != 0)
      {
        GameObject go = Singleton<PopupManager>.GetInstance().open(result, isNonSe: true, isNonOpenAnime: true);
        yield return (object) go.GetComponent<Popup0071SellIngotMenu>().initialize(exchanges, (Action) (() => bSell = true));
        Singleton<PopupManager>.GetInstance().startOpenAnime(go);
        while (Singleton<PopupManager>.GetInstance().isOpen)
          yield return (object) null;
        if (bSell)
        {
          Singleton<CommonRoot>.GetInstance().loadingMode = 1;
          Singleton<CommonRoot>.GetInstance().isLoading = true;
          long currentMoney = Player.Current.money;
          Future<WebAPI.Response.ItemSell> webApi = WebAPI.ItemSell(((IEnumerable<KeyValuePair<PlayerMaterialGear, long>>) exchanges).Select<KeyValuePair<PlayerMaterialGear, long>, int>((Func<KeyValuePair<PlayerMaterialGear, long>, int>) (p => p.Key.gear_id)).ToArray<int>(), ((IEnumerable<KeyValuePair<PlayerMaterialGear, long>>) exchanges).Select<KeyValuePair<PlayerMaterialGear, long>, long>((Func<KeyValuePair<PlayerMaterialGear, long>, long>) (p => p.Value)).ToArray<long>(), new int[0], new int[0], new int[0], actionWebError);
          yield return (object) webApi.Wait();
          Singleton<CommonRoot>.GetInstance().isLoading = false;
          actionReload();
          bool bWait = true;
          if (webApi.Result != null)
          {
            ModalWindow.Show(Consts.GetInstance().POPUP_007_1_TITLE_RESULT, Consts.Format(Consts.GetInstance().POPUP_007_1_DESCRIPTION_RESULT, (IDictionary) new Hashtable()
            {
              {
                (object) "money",
                (object) (Player.Current.money - currentMoney)
              }
            }), (Action) (() => bWait = false));
            while (bWait)
              yield return (object) null;
          }
          webApi = (Future<WebAPI.Response.ItemSell>) null;
        }
      }
    }
    actionEnd(bSell);
  }

  private IEnumerator initialize(KeyValuePair<PlayerMaterialGear, long>[] items, Action requestSell)
  {
    Popup0071SellIngotMenu popup0071SellIngotMenu = this;
    ((UIRect) ((Component) popup0071SellIngotMenu).GetComponent<UIWidget>()).alpha = 0.0f;
    Future<GameObject> ldPrefab = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    yield return (object) ldPrefab.Wait();
    GameObject iconPrefab = ldPrefab.Result;
    ldPrefab = (Future<GameObject>) null;
    popup0071SellIngotMenu.requestSell_ = requestSell;
    long total = 0;
    for (int n = 0; n < popup0071SellIngotMenu.objItemAmounts_.Length; ++n)
    {
      if (n < items.Length)
      {
        KeyValuePair<PlayerMaterialGear, long> keyValuePair = items[n];
        total += (long) keyValuePair.Key.gear.sell_price * keyValuePair.Value;
        yield return (object) popup0071SellIngotMenu.initItem(iconPrefab, keyValuePair.Key, keyValuePair.Value, n);
      }
      else
        popup0071SellIngotMenu.objItemAmounts_[n].SetActive(false);
    }
    popup0071SellIngotMenu.gridItemIcons_.repositionNow = true;
    popup0071SellIngotMenu.gridItemIcons_.Reposition();
    popup0071SellIngotMenu.gridItemAmounts_.repositionNow = true;
    popup0071SellIngotMenu.gridItemAmounts_.Reposition();
    popup0071SellIngotMenu.txtZenyAmount_.SetTextLocalize(total);
  }

  private IEnumerator initItem(GameObject prefab, PlayerMaterialGear item, long amount, int index)
  {
    GameObject go = prefab.Clone(((Component) this.gridItemIcons_).transform);
    ItemIcon icon = go.GetComponent<ItemIcon>();
    yield return (object) icon.InitByItemInfo(new GameCore.ItemInfo(item));
    foreach (Collider componentsInChild in go.GetComponentsInChildren<BoxCollider>())
      componentsInChild.enabled = false;
    icon.EnableQuantity(0);
    this.objItemAmounts_[index].SetActive(true);
    this.txtItemAmounts_[index].SetTextLocalize(amount);
  }

  private static KeyValuePair<PlayerMaterialGear, long>[] calcExchangeAmount(
    PlayerMaterialGear[] items)
  {
    Player current = Player.Current;
    long num1 = Consts.GetInstance().MONEY_MAX - current.money;
    IEnumerable<KeyValuePair<PlayerMaterialGear, int>> keyValuePairs = (IEnumerable<KeyValuePair<PlayerMaterialGear, int>>) ((IEnumerable<PlayerMaterialGear>) items).Select<PlayerMaterialGear, KeyValuePair<PlayerMaterialGear, int>>((Func<PlayerMaterialGear, KeyValuePair<PlayerMaterialGear, int>>) (y => new KeyValuePair<PlayerMaterialGear, int>(y, y.gear.sell_price))).OrderByDescending<KeyValuePair<PlayerMaterialGear, int>, int>((Func<KeyValuePair<PlayerMaterialGear, int>, int>) (x => x.Value));
    List<KeyValuePair<PlayerMaterialGear, long>> result = new List<KeyValuePair<PlayerMaterialGear, long>>(items.Length);
    if (num1 > 0L)
    {
      foreach (KeyValuePair<PlayerMaterialGear, int> keyValuePair in keyValuePairs)
      {
        if (keyValuePair.Value <= 0)
        {
          Debug.LogError((object) string.Format("{0}({1}) is price {2}!", (object) keyValuePair.Key.gear.name, (object) keyValuePair.Key.gear_id, (object) keyValuePair.Value));
        }
        else
        {
          long val1 = num1 / (long) keyValuePair.Value;
          if (val1 > 0L)
          {
            long num2 = Math.Min(val1, (long) keyValuePair.Key.quantity);
            result.Add(new KeyValuePair<PlayerMaterialGear, long>(keyValuePair.Key, num2));
            num1 -= (long) keyValuePair.Value * num2;
          }
        }
      }
    }
    return ((IEnumerable<PlayerMaterialGear>) items).Select<PlayerMaterialGear, KeyValuePair<PlayerMaterialGear, long>>((Func<PlayerMaterialGear, KeyValuePair<PlayerMaterialGear, long>>) (x => result.FirstOrDefault<KeyValuePair<PlayerMaterialGear, long>>((Func<KeyValuePair<PlayerMaterialGear, long>, bool>) (y => y.Key == x)))).Where<KeyValuePair<PlayerMaterialGear, long>>((Func<KeyValuePair<PlayerMaterialGear, long>, bool>) (z => !z.Equals((object) new KeyValuePair<PlayerMaterialGear, long>()) && z.Value > 0L)).ToArray<KeyValuePair<PlayerMaterialGear, long>>();
  }

  public override void onBackButton() => this.onClickedCancel();

  public void onClickedOk()
  {
    if (this.IsPushAndSet())
      return;
    this.requestSell_();
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void onClickedCancel()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
