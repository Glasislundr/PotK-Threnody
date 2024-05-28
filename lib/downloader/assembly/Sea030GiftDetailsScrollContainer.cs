// Decompiled with JetBrains decompiler
// Type: Sea030GiftDetailsScrollContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Sea030GiftDetailsScrollContainer : MonoBehaviour
{
  private GameObject callItemList;
  private GameObject callItemChildList;
  private GameObject callItemTipsList;
  public NGxScrollMasonry Scroll;
  private GameObject itemIconPrefab;
  private Sea030GiftDetailsScrollList.ListType typeParent = Sea030GiftDetailsScrollList.ListType.Parent;
  private Sea030GiftDetailsScrollList.ListType typeChild = Sea030GiftDetailsScrollList.ListType.Child;
  private Sea030GiftDetailsScrollList.ListType typeTips = Sea030GiftDetailsScrollList.ListType.Tips;
  public Sea030GiftDetails giftDetailsMenu;
  public bool isLockRecipe;

  public IEnumerator Init(GearGear item, Sea030GiftDetails menu, bool isLockRecipe)
  {
    this.giftDetailsMenu = menu;
    this.isLockRecipe = isLockRecipe;
    IEnumerator e = this.LoadPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.InitScrollList(item);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator LoadPrefab()
  {
    Future<GameObject> callItemListprefab = new ResourceObject("Prefabs/sea030_giftList/dir_callItemList").Load<GameObject>();
    IEnumerator e = callItemListprefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.callItemList = callItemListprefab.Result;
    Future<GameObject> callItemChildListprefab = new ResourceObject("Prefabs/sea030_giftList/dir_callItemChildList").Load<GameObject>();
    e = callItemChildListprefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.callItemChildList = callItemChildListprefab.Result;
    Future<GameObject> callItemTipsListprefab = new ResourceObject("Prefabs/sea030_giftList/dir_callItemTipsList").Load<GameObject>();
    e = callItemTipsListprefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.callItemTipsList = callItemTipsListprefab.Result;
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.itemIconPrefab = prefabF.Result;
  }

  public IEnumerator InitScrollList(GearGear item)
  {
    Sea030GiftDetailsScrollContainer menu = this;
    menu.Scroll = ((Component) menu).GetComponent<NGxScrollMasonry>();
    ((Component) menu.Scroll.Scroll).transform.Clear();
    menu.Scroll.Reset();
    ((Component) menu.Scroll.Scroll).gameObject.SetActive(false);
    ((Component) menu).gameObject.SetActive(false);
    List<CallGiftRecipe> recipe = ((IEnumerable<CallGiftRecipe>) MasterData.CallGiftRecipeList).Where<CallGiftRecipe>((Func<CallGiftRecipe, bool>) (x => x.success_gear_id_GearGear == item.ID)).ToList<CallGiftRecipe>();
    GameObject scroll_list = menu.callItemList.Clone();
    IEnumerator e = scroll_list.GetComponent<Sea030GiftDetailsScrollList>().Init(recipe, item, item, menu.itemIconPrefab, menu.typeParent, menu, 0, 0);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    menu.Scroll.Add(scroll_list);
    GameObject scroll_tips_list = menu.callItemTipsList.Clone();
    e = scroll_tips_list.GetComponent<Sea030GiftDetailsScrollList>().Init(recipe, item, item, menu.itemIconPrefab, menu.typeTips, menu, 1, 0);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    menu.Scroll.Add(scroll_tips_list);
    ((Component) menu.Scroll.Scroll).gameObject.SetActive(true);
    ((Component) menu).gameObject.SetActive(true);
    menu.Scroll.ResolvePosition();
  }

  public IEnumerator AddChildScrollList(
    GearGear child_item,
    GearGear parent_item,
    int column_num,
    int line_num)
  {
    Sea030GiftDetailsScrollContainer menu = this;
    int child_line_num = line_num + 1;
    menu.Scroll = ((Component) menu).GetComponent<NGxScrollMasonry>();
    for (int index = menu.Scroll.Arr.Count<GameObject>() - 1; index > line_num; --index)
    {
      Transform child = ((Component) menu.Scroll.Scroll).transform.GetChild(child_line_num);
      child.parent = (Transform) null;
      Object.Destroy((Object) ((Component) child).gameObject);
      menu.Scroll.Arr.RemoveAt(child_line_num);
    }
    GameObject scroll_tips_list1;
    IEnumerator e;
    if (column_num == 0)
    {
      scroll_tips_list1 = menu.callItemTipsList.Clone();
      List<CallGiftRecipe> list = ((IEnumerable<CallGiftRecipe>) MasterData.CallGiftRecipeList).Where<CallGiftRecipe>((Func<CallGiftRecipe, bool>) (x => x.success_gear_id_GearGear == child_item.ID)).ToList<CallGiftRecipe>();
      e = scroll_tips_list1.GetComponent<Sea030GiftDetailsScrollList>().Init(list, child_item, parent_item, menu.itemIconPrefab, menu.typeTips, menu, child_line_num, 0);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      menu.Scroll.Add(scroll_tips_list1);
      ((Component) ((Component) menu.Scroll.Scroll).transform.GetChild(child_line_num)).gameObject.SetActive(false);
      ((Component) ((Component) menu.Scroll.Scroll).transform.GetChild(child_line_num)).gameObject.SetActive(true);
      scroll_tips_list1 = (GameObject) null;
    }
    else
    {
      List<CallGiftRecipe> child_recipe = ((IEnumerable<CallGiftRecipe>) MasterData.CallGiftRecipeList).Where<CallGiftRecipe>((Func<CallGiftRecipe, bool>) (x => x.success_gear_id_GearGear == child_item.ID)).ToList<CallGiftRecipe>();
      scroll_tips_list1 = menu.callItemChildList.Clone();
      e = scroll_tips_list1.GetComponent<Sea030GiftDetailsScrollList>().Init(child_recipe, child_item, parent_item, menu.itemIconPrefab, menu.typeChild, menu, child_line_num, column_num);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      menu.Scroll.Add(scroll_tips_list1);
      ((Component) ((Component) menu.Scroll.Scroll).transform.GetChild(child_line_num)).gameObject.SetActive(false);
      GameObject scroll_tips_list2 = menu.callItemTipsList.Clone();
      e = scroll_tips_list2.GetComponent<Sea030GiftDetailsScrollList>().Init(child_recipe, child_item, parent_item, menu.itemIconPrefab, menu.typeTips, menu, child_line_num + 1, 0);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      menu.Scroll.Add(scroll_tips_list2);
      ((Component) ((Component) menu.Scroll.Scroll).transform.GetChild(child_line_num + 1)).gameObject.SetActive(false);
      ((Component) ((Component) menu.Scroll.Scroll).transform.GetChild(child_line_num)).gameObject.SetActive(true);
      ((Component) ((Component) menu.Scroll.Scroll).transform.GetChild(child_line_num + 1)).gameObject.SetActive(true);
      child_recipe = (List<CallGiftRecipe>) null;
      scroll_tips_list1 = (GameObject) null;
      scroll_tips_list2 = (GameObject) null;
    }
    yield return (object) null;
  }
}
