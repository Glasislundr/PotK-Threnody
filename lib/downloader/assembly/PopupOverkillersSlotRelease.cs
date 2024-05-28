// Decompiled with JetBrains decompiler
// Type: PopupOverkillersSlotRelease
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupOverkillersSlotRelease : BackButtonPopupBase
{
  [Header("主文")]
  [SerializeField]
  private UILabel txtDescription_;
  [Header("姫石情報")]
  [SerializeField]
  private GameObject topNormalGem_;
  [SerializeField]
  private GameObject topErrorGem_;
  [SerializeField]
  private UILabel txtErrorGem_;
  [SerializeField]
  private UILabel txtBeforeGem_;
  [SerializeField]
  private UILabel txtAfterGem_;
  [Header("メインボタン")]
  [SerializeField]
  private GameObject objRelease_;
  [SerializeField]
  private UIButton btnRelease_;
  [SerializeField]
  private GameObject objBuyKiseki_;
  [Header("素材情報")]
  [SerializeField]
  private PopupOverkillersSlotRelease.InfoMaterial[] infoMaterials_;
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
  private int noSlot_;
  private OverkillersSlotRelease.Conditions condition_;
  private Action onDone_;
  private PopupOverkillersSlotRelease.NGFlags ngFlags_;
  private PlayerMaterialUnit[] playerMaterials_;
  private int lastReleased_;
  [SerializeField]
  private GameObject commercialObj;
  [SerializeField]
  private UISprite popupBaseSprite;
  private UITweener[] boxAnimations_;

  public static Future<GameObject> createLoader(bool isSea)
  {
    string str = isSea ? "_sea" : string.Empty;
    return new ResourceObject("Prefabs/unit004_2" + str + "/Unit_UnitEquipment_Release_Dialog" + str).Load<GameObject>();
  }

  public static void show(
    GameObject prefab,
    PlayerUnit playerUnit,
    int slot_no,
    bool[] slot_locks,
    OverkillersSlotRelease.Conditions condition,
    Action actDone)
  {
    Singleton<PopupManager>.GetInstance().open(prefab, isNonSe: true, isNonOpenAnime: true).GetComponent<PopupOverkillersSlotRelease>().initialize(playerUnit, slot_no, slot_locks, condition, actDone);
  }

  private void initialize(
    PlayerUnit playerUnit,
    int slot_no,
    bool[] slot_locks,
    OverkillersSlotRelease.Conditions condition,
    Action actDone)
  {
    this.setTopObject(((Component) this).gameObject);
    this.playerUnit_ = playerUnit;
    this.noSlot_ = slot_no;
    this.condition_ = condition;
    this.onDone_ = actDone;
    this.lastReleased_ = -1;
    for (int index = slot_locks.Length - 1; index >= 0; --index)
    {
      if (!slot_locks[index])
      {
        this.lastReleased_ = index;
        break;
      }
    }
    this.ngFlags_ = slot_no <= 0 || !slot_locks[slot_no - 1] ? PopupOverkillersSlotRelease.NGFlags.Clear : PopupOverkillersSlotRelease.NGFlags.Prev;
    if ((double) playerUnit.unityTotal < (double) condition.unity_value)
      this.ngFlags_ |= PopupOverkillersSlotRelease.NGFlags.Unity;
    if (Player.Current.coin < condition.gem)
      this.ngFlags_ |= PopupOverkillersSlotRelease.NGFlags.Gem;
    OverkillersMaterial[] materials = condition.materials;
    this.playerMaterials_ = condition.getPlayerMaterials();
    for (int index = 0; index < materials.Length; ++index)
    {
      if (this.playerMaterials_[index].quantity < materials[index].quantity)
      {
        this.ngFlags_ |= PopupOverkillersSlotRelease.NGFlags.Materials;
        break;
      }
    }
  }

  private IEnumerator Start()
  {
    PopupOverkillersSlotRelease overkillersSlotRelease = this;
    UIPanel panel = ((Component) overkillersSlotRelease).GetComponent<UIPanel>();
    ((UIRect) panel).alpha = 0.0f;
    ((UIRect) panel).UpdateAnchors();
    int coin = Player.Current.coin;
    if ((overkillersSlotRelease.ngFlags_ & PopupOverkillersSlotRelease.NGFlags.Gem) == PopupOverkillersSlotRelease.NGFlags.Clear)
    {
      overkillersSlotRelease.topNormalGem_.SetActive(true);
      overkillersSlotRelease.topErrorGem_.SetActive(false);
      overkillersSlotRelease.txtBeforeGem_.SetTextLocalize(coin);
      overkillersSlotRelease.txtAfterGem_.SetTextLocalize(coin - overkillersSlotRelease.condition_.gem);
    }
    else
    {
      overkillersSlotRelease.topNormalGem_.SetActive(false);
      overkillersSlotRelease.topErrorGem_.SetActive(true);
      overkillersSlotRelease.txtErrorGem_.SetTextLocalize(overkillersSlotRelease.condition_.gem);
    }
    string text;
    if ((overkillersSlotRelease.ngFlags_ & (PopupOverkillersSlotRelease.NGFlags.Prev | PopupOverkillersSlotRelease.NGFlags.Unity | PopupOverkillersSlotRelease.NGFlags.Gem)) == PopupOverkillersSlotRelease.NGFlags.Gem)
    {
      overkillersSlotRelease.objRelease_.SetActive(false);
      overkillersSlotRelease.objBuyKiseki_.SetActive(true);
      text = Consts.GetInstance().SHOP_99931_TXT_DESCRIPTION;
    }
    else
    {
      overkillersSlotRelease.objRelease_.SetActive(true);
      overkillersSlotRelease.objBuyKiseki_.SetActive(false);
      ((UIButtonColor) overkillersSlotRelease.btnRelease_).isEnabled = overkillersSlotRelease.ngFlags_ == PopupOverkillersSlotRelease.NGFlags.Clear;
      Consts instance = Consts.GetInstance();
      if (overkillersSlotRelease.ngFlags_ == PopupOverkillersSlotRelease.NGFlags.Clear)
      {
        string releaceMessageFirst = instance.POPUP_OVERKILLERS_SLOT_RELEACE_MESSAGE_FIRST;
        if (overkillersSlotRelease.condition_.gem > 0)
          releaceMessageFirst += Consts.Format(instance.POPUP_OVERKILLERS_SLOT_RELEACE_MESSAGE_KISEKI, (IDictionary) new Hashtable()
          {
            {
              (object) "gem",
              (object) overkillersSlotRelease.condition_.gem
            }
          });
        text = releaceMessageFirst + instance.POPUP_OVERKILLERS_SLOT_RELEACE_MESSAGE_END;
      }
      else if ((overkillersSlotRelease.ngFlags_ & PopupOverkillersSlotRelease.NGFlags.Prev) != PopupOverkillersSlotRelease.NGFlags.Clear)
        text = Consts.Format(instance.POPUP_OVERKILLERS_SLOT_RELEACE_NOTICE_PREV_SLOT, (IDictionary) new Hashtable()
        {
          {
            (object) "slot",
            (object) (overkillersSlotRelease.lastReleased_ + 2)
          }
        });
      else
        text = ((overkillersSlotRelease.ngFlags_ & PopupOverkillersSlotRelease.NGFlags.Unity) != PopupOverkillersSlotRelease.NGFlags.Clear ? instance.POPUP_OVERKILLERS_SLOT_RELEACE_SHORTAGE_UNITY : instance.POPUP_OVERKILLERS_SLOT_RELEACE_SHORTAGE_MATERIALS) + instance.POPUP_OVERKILLERS_SLOT_RELEACE_SHORTAGE_END;
    }
    overkillersSlotRelease.txtDescription_.SetTextLocalize(text);
    if (overkillersSlotRelease.condition_.gem <= 0)
    {
      overkillersSlotRelease.commercialObj.SetActive(false);
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
        ((UIWidget) overkillersSlotRelease.popupBaseSprite).SetDimensions(((UIWidget) overkillersSlotRelease.popupBaseSprite).width, 610);
      else
        ((UIWidget) overkillersSlotRelease.popupBaseSprite).SetDimensions(((UIWidget) overkillersSlotRelease.popupBaseSprite).width, 670);
    }
    Future<GameObject> ldIcon = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    yield return (object) ldIcon.Wait();
    GameObject prefabIcon = ldIcon.Result;
    ldIcon = (Future<GameObject>) null;
    for (int n = 0; n < overkillersSlotRelease.infoMaterials_.Length; ++n)
    {
      if (n < overkillersSlotRelease.condition_.materials.Length)
        yield return (object) overkillersSlotRelease.initMaterial(overkillersSlotRelease.infoMaterials_[n], prefabIcon, overkillersSlotRelease.condition_.materials[n], overkillersSlotRelease.playerMaterials_[n]);
      else
        yield return (object) overkillersSlotRelease.initMaterial(overkillersSlotRelease.infoMaterials_[n], prefabIcon, (OverkillersMaterial) null, (PlayerMaterialUnit) null);
    }
    overkillersSlotRelease.boxMaterialDialog_.SetActive(false);
    overkillersSlotRelease.boxMaterialDialog_.GetComponent<UIPanel>().depth = panel.depth + 1;
    overkillersSlotRelease.colliderMaterialDialog_.size = Vector2.op_Implicit(panel.GetViewSize());
    ((Component) overkillersSlotRelease.colliderMaterialDialog_).transform.position = Vector3.zero;
    Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) overkillersSlotRelease).gameObject);
  }

  private IEnumerator initMaterial(
    PopupOverkillersSlotRelease.InfoMaterial info,
    GameObject prefab,
    OverkillersMaterial src,
    PlayerMaterialUnit material)
  {
    PopupOverkillersSlotRelease overkillersSlotRelease = this;
    UnitIcon icon = prefab.Clone(info.link_).GetComponent<UnitIcon>();
    if (src != null)
    {
      UnitUnit unit = material.unit;
      yield return (object) icon.SetUnit(unit, unit.GetElement(), false);
      icon.BottomModeValue = UnitIconBase.BottomMode.Nothing;
      icon.Gray = src.quantity > material.quantity;
      // ISSUE: reference to a compiler-generated method
      icon.onClick = new Action<UnitIconBase>(overkillersSlotRelease.\u003CinitMaterial\u003Eb__30_0);
      info.txtRequired_.SetTextLocalize(src.quantity);
      if (icon.Gray)
        ((UIWidget) info.txtRequired_).color = Color.red;
      info.txtQuantity_.SetTextLocalize(Consts.Format(Consts.GetInstance().unit_004_9_9_possession_text, (IDictionary) new Hashtable()
      {
        {
          (object) "Count",
          (object) material.quantity
        }
      }));
    }
    else
    {
      icon.unit = (UnitUnit) null;
      icon.SetEmpty();
      ((UIButtonColor) icon.Button).isEnabled = false;
      info.dirAmount_.SetActive(false);
    }
    icon.SetCounter(0);
  }

  public void onClickedDone()
  {
    if (this.ngFlags_ != PopupOverkillersSlotRelease.NGFlags.Clear || this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    this.onDone_();
  }

  public void onClickedBuyKiseki()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupUtility.BuyKiseki());
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

  private void showMaterialQuestInfo(UnitUnit material)
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
    UnitMaterialQuestInfo materialQuestInfo = Array.Find<UnitMaterialQuestInfo>(MasterData.UnitMaterialQuestInfoList, (Predicate<UnitMaterialQuestInfo>) (x => x.unit_id == material.ID));
    if (materialQuestInfo == null)
      this.txtMaterialDescription_.text = string.Empty;
    else
      this.txtMaterialDescription_.SetTextLocalize(materialQuestInfo.long_desc);
  }

  public void hideMaterialQuestInfo()
  {
    UITweener[] materialQuestInfo = this.getEndTweensMaterialQuestInfo();
    if (materialQuestInfo == null)
      return;
    NGTween.setOnTweenFinished(materialQuestInfo, (MonoBehaviour) this, "hideDialogBox");
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

  private enum NGNumbers
  {
    None,
    Prev,
    Unity,
    Gem,
    Materials,
  }

  [Flags]
  private enum NGFlags
  {
    Clear = 0,
    None = 1,
    Prev = 2,
    Unity = 4,
    Gem = 8,
    Materials = 16, // 0x00000010
  }
}
