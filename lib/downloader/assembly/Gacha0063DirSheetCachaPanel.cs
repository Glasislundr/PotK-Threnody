// Decompiled with JetBrains decompiler
// Type: Gacha0063DirSheetCachaPanel
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
public class Gacha0063DirSheetCachaPanel : MonoBehaviour
{
  [SerializeField]
  private GameObject iBtn_Panel;
  [SerializeField]
  private GameObject dyn_Detail;
  [SerializeField]
  private GameObject dyn_RewardThum;
  [SerializeField]
  private GameObject dir_Amount;
  [SerializeField]
  private GameObject dyn_Point;
  [SerializeField]
  private GameObject SheetPanelsBlacks;
  [SerializeField]
  private GameObject amountDigitX;
  [SerializeField]
  private GameObject[] amountDigit;
  [SerializeField]
  [Tooltip("桁が足りなければ自動で拡張")]
  private bool isExtensionPointDigit;
  [SerializeField]
  private GameObject[] pointDigit;
  private GameObject effectObject;
  private GameObject hitEffectObject;
  private Popup0063SheetMenu parentObject;
  private MasterDataTable.CommonRewardType rewardType;
  private int rewardID;
  public bool isOpen;
  public bool IsResultEffect;
  private const float ANIM_START_TIME = 0.25f;
  private const float ANIM_DURUTION = 0.5f;
  private const float ANIM_ADD_TIME1 = 0.5f;
  private const float ANIM_ADD_TIME2 = 0.25f;
  private Dictionary<GameObject, Gacha0063DirSheetCachaPanel.DisplayDigitEx> dicDisplayDigitEx;

  private void SentNum(GameObject parent, GameObject[] go, int num, bool bExtension = false)
  {
    int num1 = num;
    Gacha0063DirSheetCachaPanel.DisplayDigitEx displayDigitEx = (Gacha0063DirSheetCachaPanel.DisplayDigitEx) null;
    this.dicDisplayDigitEx?.TryGetValue(parent, out displayDigitEx);
    ((IEnumerable<GameObject>) go).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
    if (num1 == -1)
    {
      parent.SetActive(false);
    }
    else
    {
      parent.SetActive(true);
      int length = num.ToString().Length;
      if (bExtension && length > go.Length && go.Length > 1)
      {
        int num2 = length;
        if (displayDigitEx == null)
        {
          displayDigitEx = new Gacha0063DirSheetCachaPanel.DisplayDigitEx()
          {
            parentScale = parent.transform.localScale,
            digits = new List<GameObject>(length - go.Length)
          };
          if (this.dicDisplayDigitEx == null)
            this.dicDisplayDigitEx = new Dictionary<GameObject, Gacha0063DirSheetCachaPanel.DisplayDigitEx>()
            {
              {
                parent,
                displayDigitEx
              }
            };
          else
            this.dicDisplayDigitEx[parent] = displayDigitEx;
        }
        GameObject self = go[go.Length - 1];
        Vector3 localPosition = self.transform.localPosition;
        float num3 = localPosition.x - go[go.Length - 2].transform.localPosition.x;
        int count1 = num2 - go.Length;
        for (int count2 = displayDigitEx.digits.Count; count2 < count1; ++count2)
        {
          GameObject gameObject = self.Clone(parent.transform);
          ((Object) gameObject).name = string.Format("digit{0}(Ex)", (object) (go.Length + count2 + 1));
          gameObject.transform.localPosition = new Vector3(localPosition.x + num3 * (float) (count2 + 1), localPosition.y, localPosition.z);
          displayDigitEx.digits.Add(gameObject);
        }
        displayDigitEx.digits.SetActives(false);
        float num4 = (float) go.Length / (float) num2;
        parent.transform.localScale = new Vector3(displayDigitEx.parentScale.x * num4, displayDigitEx.parentScale.y * num4, displayDigitEx.parentScale.z);
        go = ((IEnumerable<GameObject>) go).Concat<GameObject>(displayDigitEx.digits.Take<GameObject>(count1)).ToArray<GameObject>();
        displayDigitEx = (Gacha0063DirSheetCachaPanel.DisplayDigitEx) null;
      }
      if (length <= go.Length)
      {
        for (int index = 0; index < length; ++index)
        {
          Sprite sprite = Resources.Load<Sprite>("Icons/slc_Number" + (num1 % 10).ToString());
          go[index].GetComponent<UI2DSprite>().sprite2D = sprite;
          UI2DSprite component = go[index].GetComponent<UI2DSprite>();
          Rect textureRect1 = sprite.textureRect;
          int width = (int) ((Rect) ref textureRect1).width;
          Rect textureRect2 = sprite.textureRect;
          int height = (int) ((Rect) ref textureRect2).height;
          ((UIWidget) component).SetDimensions(width, height);
          go[index].SetActive(true);
          num1 /= 10;
        }
      }
    }
    if (displayDigitEx == null)
      return;
    parent.transform.localScale = displayDigitEx.parentScale;
    foreach (GameObject digit in displayDigitEx.digits)
    {
      digit.SetActive(false);
      Object.Destroy((Object) digit);
    }
    if (this.dicDisplayDigitEx.Count == 1)
      this.dicDisplayDigitEx = (Dictionary<GameObject, Gacha0063DirSheetCachaPanel.DisplayDigitEx>) null;
    else
      this.dicDisplayDigitEx.Remove(parent);
  }

  private void SetAmountDigit(int num)
  {
    this.SentNum(this.dir_Amount, this.amountDigit, num);
    if (num < 10)
      this.amountDigitX.transform.position = new Vector3(this.amountDigit[1].transform.position.x, this.amountDigitX.transform.position.y, this.amountDigitX.transform.position.z);
    else if (num < 100)
    {
      this.amountDigitX.transform.position = new Vector3(this.amountDigit[2].transform.position.x, this.amountDigitX.transform.position.y, this.amountDigitX.transform.position.z);
    }
    else
    {
      if (num >= 1000)
        return;
      this.amountDigitX.transform.position = new Vector3(this.amountDigit[3].transform.position.x, this.amountDigitX.transform.position.y, this.amountDigitX.transform.position.z);
    }
  }

  private void SetPointDigit(int num)
  {
    this.SentNum(this.dyn_Point, this.pointDigit, num, this.isExtensionPointDigit);
  }

  public IEnumerator Init(
    Popup0063SheetMenu parent,
    GachaG007PlayerPanel panel,
    bool isResultEffect = false,
    GameObject effectPrefab = null,
    GameObject hitEffectPrefab = null)
  {
    Gacha0063DirSheetCachaPanel dirSheetCachaPanel = this;
    dirSheetCachaPanel.parentObject = parent;
    dirSheetCachaPanel.dir_Amount.SetActive(false);
    dirSheetCachaPanel.dyn_Point.SetActive(false);
    dirSheetCachaPanel.isOpen = panel.is_opened;
    dirSheetCachaPanel.IsResultEffect = isResultEffect;
    dirSheetCachaPanel.dyn_Detail.SetActive(false);
    ((Behaviour) dirSheetCachaPanel.iBtn_Panel.GetComponent<UIButton>()).enabled = false;
    CreateIconObject target = dirSheetCachaPanel.dyn_RewardThum.GetOrAddComponent<CreateIconObject>();
    if (panel.reward_type_id.HasValue)
    {
      dirSheetCachaPanel.rewardType = (MasterDataTable.CommonRewardType) panel.reward_type_id.Value;
      dirSheetCachaPanel.rewardID = panel.reward_id.HasValue ? panel.reward_id.Value : 0;
      IEnumerator e = target.CreateThumbnail((MasterDataTable.CommonRewardType) panel.reward_type_id.Value, !panel.reward_id.HasValue ? 0 : panel.reward_id.Value);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      switch (panel.reward_type_id.Value)
      {
        case 1:
        case 24:
          target.GetIcon().GetComponent<UnitIcon>().setLevelText("1");
          target.GetIcon().GetComponent<UnitIcon>().ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
          dirSheetCachaPanel.SetAmountDigit(panel.reward_quantity.Value);
          dirSheetCachaPanel.SetPointDigit(-1);
          break;
        case 2:
        case 3:
        case 5:
        case 10:
        case 14:
        case 17:
        case 19:
        case 20:
        case 23:
        case 26:
        case 29:
        case 34:
        case 35:
        case 39:
        case 40:
          dirSheetCachaPanel.SetAmountDigit(panel.reward_quantity.Value);
          dirSheetCachaPanel.SetPointDigit(-1);
          break;
        case 4:
        case 6:
        case 7:
        case 8:
        case 9:
        case 12:
        case 13:
        case 15:
          dirSheetCachaPanel.SetAmountDigit(-1);
          dirSheetCachaPanel.SetPointDigit(panel.reward_quantity.Value);
          break;
        default:
          dirSheetCachaPanel.SetAmountDigit(-1);
          dirSheetCachaPanel.SetPointDigit(-1);
          break;
      }
    }
    if (dirSheetCachaPanel.IsResultEffect && !dirSheetCachaPanel.isOpen)
    {
      dirSheetCachaPanel.effectObject = effectPrefab.Clone(((Component) dirSheetCachaPanel).transform);
      dirSheetCachaPanel.hitEffectObject = hitEffectPrefab.Clone(((Component) dirSheetCachaPanel).transform);
      dirSheetCachaPanel.effectObject.SetActive(false);
      dirSheetCachaPanel.hitEffectObject.SetActive(false);
    }
    dirSheetCachaPanel.SheetPanelsBlacks.SetActive(dirSheetCachaPanel.isOpen);
  }

  public void SelEffect(float correct)
  {
    this.effectObject.GetComponent<ParticleSystem>().playbackSpeed = 1f;
    this.effectObject.SetActive(false);
    this.effectObject.SetActive(true);
  }

  public void HitEffect()
  {
    this.effectObject.SetActive(false);
    this.hitEffectObject.SetActive(true);
  }

  public void IbtnDetail() => this.StartCoroutine(this.ShowDetailPopup());

  private IEnumerator ShowDetailPopup()
  {
    IEnumerator e = this.parentObject.ShowDetaiPopup(this.rewardType, this.rewardID);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private class DisplayDigitEx
  {
    public Vector3 parentScale;
    public List<GameObject> digits;
  }
}
