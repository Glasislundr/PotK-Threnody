// Decompiled with JetBrains decompiler
// Type: PopupBuguSlotRelease
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class PopupBuguSlotRelease : BackButtonPopupBase
{
  [Header("主文")]
  [SerializeField]
  private UILabel txtDescription_;
  [Header("淘汰値情報")]
  [SerializeField]
  private UILabel toutaTotalDenominator_;
  [SerializeField]
  private UILabel toutaTotalNumerator_;
  [SerializeField]
  private UILabel toutaDefaultDenominator_;
  [SerializeField]
  private UILabel toutaDefaultNumerator_;
  [Header("メインボタン")]
  [SerializeField]
  private GameObject objRelease_;
  [SerializeField]
  private UIButton btnRelease_;
  [Header("素材情報")]
  [SerializeField]
  private PopupBuguSlotRelease.InfoMaterial[] infoMaterials_;
  [Header("素材簡易説明")]
  [SerializeField]
  private GameObject boxMaterialDialog_;
  [SerializeField]
  private UILabel txtMaterialName_;
  [SerializeField]
  private UILabel txtMaterialDescription_;
  [SerializeField]
  private BoxCollider colliderMaterialDialog_;
  private PlayerUnit playerUnit_;
  private List<PopupBuguSlotRelease.GearReleaseItem> releaseItemList_ = new List<PopupBuguSlotRelease.GearReleaseItem>();
  private Action onDone_;
  private bool releaseCheck_ = true;
  private DetailMenuScrollViewParam detailMenu_;
  private PlayerMaterialUnit[] playerMaterials_;
  private UITweener[] boxAnimations_;

  public static Future<GameObject> createLoader(bool isSea)
  {
    string str = isSea ? "_sea" : string.Empty;
    return new ResourceObject("Prefabs/unit004_2" + str + "/unit_buguRelease_Dialog" + str).Load<GameObject>();
  }

  public static void show(
    GameObject prefab,
    PlayerUnit playerUnit,
    DetailMenuScrollViewParam detailMenu,
    Action actDone)
  {
    Singleton<PopupManager>.GetInstance().open(prefab, isNonSe: true, isNonOpenAnime: true).GetComponent<PopupBuguSlotRelease>().initialize(playerUnit, detailMenu, actDone);
  }

  private void initialize(
    PlayerUnit playerUnit,
    DetailMenuScrollViewParam detailMenu,
    Action actDone)
  {
    this.setTopObject(((Component) this).gameObject);
    this.playerUnit_ = playerUnit;
    this.detailMenu_ = detailMenu;
    this.onDone_ = actDone;
    GearExtensionItem gearExtensionItem = Array.Find<GearExtensionItem>(MasterData.GearExtensionItemList, (Predicate<GearExtensionItem>) (x => x.same_character_id == this.playerUnit_.unit.same_character_id)) ?? Array.Find<GearExtensionItem>(MasterData.GearExtensionItemList, (Predicate<GearExtensionItem>) (x =>
    {
      int? kindGearKind1 = x.kind_GearKind;
      int kindGearKind2 = this.playerUnit_.unit.kind_GearKind;
      return kindGearKind1.GetValueOrDefault() == kindGearKind2 & kindGearKind1.HasValue;
    }));
    this.releaseItemList_.Add(new PopupBuguSlotRelease.GearReleaseItem()
    {
      materialId = gearExtensionItem.material1_id,
      quantity = gearExtensionItem.material1_num
    });
    this.releaseItemList_.Add(new PopupBuguSlotRelease.GearReleaseItem()
    {
      materialId = gearExtensionItem.material2_id,
      quantity = gearExtensionItem.material2_num
    });
    this.releaseItemList_.Add(new PopupBuguSlotRelease.GearReleaseItem()
    {
      materialId = gearExtensionItem.material3_id,
      quantity = gearExtensionItem.material3_num
    });
    this.releaseItemList_.Add(new PopupBuguSlotRelease.GearReleaseItem()
    {
      materialId = gearExtensionItem.material4_id,
      quantity = gearExtensionItem.material4_num
    });
    this.releaseItemList_.Add(new PopupBuguSlotRelease.GearReleaseItem()
    {
      materialId = gearExtensionItem.material5_id,
      quantity = gearExtensionItem.material5_num
    });
  }

  private IEnumerator Start()
  {
    PopupBuguSlotRelease popupBuguSlotRelease = this;
    UIPanel panel = ((Component) popupBuguSlotRelease).GetComponent<UIPanel>();
    ((UIRect) panel).alpha = 0.0f;
    ((UIRect) panel).UpdateAnchors();
    popupBuguSlotRelease.releaseCheck_ = true;
    GearExtensionUnity extensionUnity = MasterData.GearExtensionUnityList[0];
    float total = Mathf.Min((float) popupBuguSlotRelease.playerUnit_.unity_value + popupBuguSlotRelease.playerUnit_.buildup_unity_value_f, (float) PlayerUnit.GetUnityValueMax());
    popupBuguSlotRelease.toutaTotalNumerator_.SetTextLocalize((int) Math.Floor((double) total));
    popupBuguSlotRelease.toutaTotalDenominator_.SetTextLocalize(extensionUnity.total_unity_value.ToString());
    popupBuguSlotRelease.toutaDefaultNumerator_.SetTextLocalize(popupBuguSlotRelease.playerUnit_.unity_value.ToString());
    popupBuguSlotRelease.toutaDefaultDenominator_.SetTextLocalize(extensionUnity.true_unity_value.ToString());
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      ((UIWidget) popupBuguSlotRelease.toutaTotalNumerator_).color = ((UIWidget) popupBuguSlotRelease.toutaTotalDenominator_).color;
      ((UIWidget) popupBuguSlotRelease.toutaDefaultNumerator_).color = ((UIWidget) popupBuguSlotRelease.toutaDefaultDenominator_).color;
    }
    else
    {
      ((UIWidget) popupBuguSlotRelease.toutaTotalNumerator_).color = Color.white;
      ((UIWidget) popupBuguSlotRelease.toutaDefaultNumerator_).color = Color.white;
    }
    if ((double) total < (double) extensionUnity.total_unity_value)
    {
      ((UIWidget) popupBuguSlotRelease.toutaTotalNumerator_).color = Color.red;
      popupBuguSlotRelease.releaseCheck_ = false;
    }
    if (popupBuguSlotRelease.playerUnit_.unity_value < extensionUnity.true_unity_value)
    {
      ((UIWidget) popupBuguSlotRelease.toutaDefaultNumerator_).color = Color.red;
      popupBuguSlotRelease.releaseCheck_ = false;
    }
    Future<GameObject> ldIcon = (Future<GameObject>) null;
    ldIcon = Singleton<NGGameDataManager>.GetInstance().IsSea ? new ResourceObject("Prefabs/Sea/ItemIcon/prefab_sea").Load<GameObject>() : Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    yield return (object) ldIcon.Wait();
    GameObject prefabIcon = ldIcon.Result;
    ldIcon = (Future<GameObject>) null;
    int n = 0;
    bool material_check = true;
    for (; n < popupBuguSlotRelease.infoMaterials_.Length; ++n)
    {
      if (popupBuguSlotRelease.releaseItemList_[n].materialId != 0)
      {
        if (CommonRewardType.GetHaveCount(MasterDataTable.CommonRewardType.material_gear, popupBuguSlotRelease.releaseItemList_[n].materialId) < (long) popupBuguSlotRelease.releaseItemList_[n].quantity)
        {
          popupBuguSlotRelease.releaseCheck_ = false;
          material_check = false;
        }
        yield return (object) popupBuguSlotRelease.initMaterial(popupBuguSlotRelease.infoMaterials_[n], prefabIcon, popupBuguSlotRelease.releaseItemList_[n]);
      }
      else
        yield return (object) popupBuguSlotRelease.initMaterial(popupBuguSlotRelease.infoMaterials_[n], prefabIcon, (PopupBuguSlotRelease.GearReleaseItem) null);
    }
    string text = "";
    Consts instance = Consts.GetInstance();
    if ((double) total >= (double) extensionUnity.total_unity_value && popupBuguSlotRelease.playerUnit_.unity_value >= extensionUnity.true_unity_value && material_check)
      text = instance.POPUP_BUGU_SLOT_RELEACE_MESSAGE_CLEAR;
    else if ((double) total < (double) extensionUnity.total_unity_value || popupBuguSlotRelease.playerUnit_.unity_value < extensionUnity.true_unity_value)
      text = instance.POPUP_BUGU_SLOT_RELEACE_MESSAGE_UNITY_NON;
    else if (!material_check)
      text = instance.POPUP_BUGU_SLOT_RELEACE_MESSAGE_MATERIAL_NON;
    popupBuguSlotRelease.txtDescription_.SetTextLocalize(text);
    popupBuguSlotRelease.boxMaterialDialog_.SetActive(false);
    popupBuguSlotRelease.boxMaterialDialog_.GetComponent<UIPanel>().depth = panel.depth + 1;
    popupBuguSlotRelease.colliderMaterialDialog_.size = Vector2.op_Implicit(panel.GetViewSize());
    ((Component) popupBuguSlotRelease.colliderMaterialDialog_).transform.position = Vector3.zero;
    ((UIButtonColor) popupBuguSlotRelease.btnRelease_).isEnabled = popupBuguSlotRelease.releaseCheck_;
    Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) popupBuguSlotRelease).gameObject);
  }

  private IEnumerator initMaterial(
    PopupBuguSlotRelease.InfoMaterial info,
    GameObject prefab,
    PopupBuguSlotRelease.GearReleaseItem src)
  {
    PopupBuguSlotRelease popupBuguSlotRelease1 = this;
    if (src != null)
    {
      PopupBuguSlotRelease popupBuguSlotRelease = popupBuguSlotRelease1;
      GearGear gear = (GearGear) null;
      if (MasterData.GearGear.TryGetValue(src.materialId, out gear))
      {
        ItemIcon icon = prefab.Clone(info.link_).GetComponent<ItemIcon>();
        IEnumerator e = icon.InitByGear(gear, gear.GetElement());
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        long haveCount = CommonRewardType.GetHaveCount(MasterDataTable.CommonRewardType.material_gear, src.materialId);
        info.txtRequired_.SetTextLocalize(src.quantity);
        if (haveCount < (long) src.quantity)
          ((UIWidget) info.txtRequired_).color = Color.red;
        info.txtQuantity_.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_BUGU_SLOT_RELEACE_POSSESSION_TEXT, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) haveCount
          }
        }));
        icon.onClick = (Action<ItemIcon>) (x => popupBuguSlotRelease.showMaterialQuestInfo(gear));
        icon = (ItemIcon) null;
      }
    }
    else
    {
      ItemIcon component = prefab.Clone(info.link_).GetComponent<ItemIcon>();
      component.SetEmpty(true);
      component.gear.favorite.SetActive(false);
      component.gear.type.SetActive(false);
      ((Component) component.gear.button).gameObject.SetActive(true);
      info.dirAmount_.SetActive(false);
    }
  }

  private PlayerMaterialUnit[] getPlayerMaterials(
    List<PopupBuguSlotRelease.GearReleaseItem> releaseItemList)
  {
    PlayerMaterialUnit[] array = SMManager.Get<PlayerMaterialUnit[]>();
    PlayerMaterialUnit[] playerMaterials = new PlayerMaterialUnit[releaseItemList.Count];
    for (int index = 0; index < playerMaterials.Length; ++index)
    {
      int id = releaseItemList[index].materialId;
      playerMaterials[index] = Array.Find<PlayerMaterialUnit>(array, (Predicate<PlayerMaterialUnit>) (x => x._unit == id));
      if (playerMaterials[index] == null)
      {
        playerMaterials[index] = new PlayerMaterialUnit();
        playerMaterials[index].id = index;
        playerMaterials[index]._unit = id;
      }
    }
    return playerMaterials;
  }

  public void onClickedDone()
  {
    if (!this.releaseCheck_ || this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    this.onDone_();
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  private UITweener[] boxAnimations
  {
    get
    {
      return this.boxAnimations_ ?? (this.boxAnimations_ = NGTween.findTweeners(this.boxMaterialDialog_, true));
    }
  }

  private void showMaterialQuestInfo(GearGear material)
  {
    int num = !this.boxMaterialDialog_.activeInHierarchy ? 1 : 0;
    this.boxMaterialDialog_.SetActive(true);
    if (num != 0)
    {
      UITweener[] boxAnimations = this.boxAnimations;
      NGTween.playTweens(boxAnimations, NGTween.Kind.START_END);
      NGTween.playTweens(boxAnimations, NGTween.Kind.START);
      foreach (UITweener uiTweener in boxAnimations)
        uiTweener.onFinished.Clear();
    }
    this.txtMaterialName_.SetTextLocalize(material.name);
    GearMaterialQuestInfo materialQuestInfo = ((IEnumerable<GearMaterialQuestInfo>) MasterData.GearMaterialQuestInfoList).FirstOrDefault<GearMaterialQuestInfo>((Func<GearMaterialQuestInfo, bool>) (x => x.gear_id == material.group_id));
    if (materialQuestInfo == null)
    {
      this.txtMaterialDescription_.SetTextLocalize("");
    }
    else
    {
      string text = materialQuestInfo.detail_desc1;
      if (!materialQuestInfo.detail_desc2.isEmptyOrWhitespace())
        text = text + "\n" + materialQuestInfo.detail_desc2;
      if (!materialQuestInfo.detail_desc3.isEmptyOrWhitespace())
        text = text + "\n" + materialQuestInfo.detail_desc3;
      this.txtMaterialDescription_.SetTextLocalize(text);
    }
  }

  public void hideMaterialQuestInfo()
  {
    UITweener[] materialQuestInfo = this.getEndTweensMaterialQuestInfo();
    if (materialQuestInfo == null)
      return;
    NGTween.setOnTweenFinished(materialQuestInfo, (MonoBehaviour) this, "hideDialogBox");
  }

  public void onClickedDetail()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    this.detailMenu_.onClicedUnity();
  }

  private UITweener[] getEndTweensMaterialQuestInfo()
  {
    if (!this.boxMaterialDialog_.activeInHierarchy)
      return (UITweener[]) null;
    foreach (Behaviour boxAnimation in this.boxAnimations)
    {
      if (boxAnimation.enabled)
        return (UITweener[]) null;
    }
    NGTween.playTweens(this.boxAnimations, NGTween.Kind.START_END, true);
    NGTween.playTweens(this.boxAnimations, NGTween.Kind.END);
    return this.boxAnimations;
  }

  private void hideDialogBox() => this.boxMaterialDialog_.SetActive(false);

  [Serializable]
  private class InfoMaterial
  {
    public GameObject top_;
    public Transform link_;
    public GameObject dirAmount_;
    public UILabel txtRequired_;
    public UILabel txtQuantity_;
  }

  private class GearReleaseItem
  {
    public int materialId;
    public int quantity;
  }
}
