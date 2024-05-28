// Decompiled with JetBrains decompiler
// Type: Gacha0063Point
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Gacha0063Point : Gacha0063hindicator
{
  private const int GachaButtonMax = 2;
  [SerializeField]
  private UI2DSprite bannerSprite;
  [SerializeField]
  private GameObject dirBtnSingle;
  [SerializeField]
  private GameObject dirBtnDouble;
  [SerializeField]
  private GameObject dirBtnSingleLimit;
  [SerializeField]
  private GameObject slcStepUPBase;
  [SerializeField]
  private UISprite slcDenominator;
  [SerializeField]
  private UISprite slcNumerator;
  [SerializeField]
  private UILabel dirSinglePointLabel;
  [SerializeField]
  private UILabel dirMultiPointLabel;
  [SerializeField]
  private UILabel dirMultiGachaLabel;
  [SerializeField]
  private SpreadColorButton dirSingleButton;
  [SerializeField]
  private SpreadColorButton dirMultiButton;
  [SerializeField]
  private List<UIWidget> dirSingleLabelColor;
  [SerializeField]
  private List<UIWidget> dirMultiLabelColor;
  private bool isSingleLimit;

  public override void InitGachaModuleGacha(
    Gacha0063Menu gacha0063Menu,
    GachaModule gachaModule,
    DateTime serverTime,
    UIScrollView scrollView,
    int prefabCount)
  {
    this.GachaModule = gachaModule;
    this.Menu = gacha0063Menu;
    this.isSingleLimit = false;
    this.PrefabCount = prefabCount;
    this.Uipanel.baseClipRegion = Vector4.op_Addition(((Component) scrollView).gameObject.GetComponent<UIPanel>().baseClipRegion, new Vector4(0.0f, 0.0f, 0.0f, 500f));
    this.Uipanel.baseClipRegion = new Vector4(0.0f, this.Uipanel.baseClipRegion.y, this.Uipanel.baseClipRegion.z, this.Uipanel.baseClipRegion.w);
    ((UIRect) this.Uipanel).ResetAnchors();
    if (gachaModule.gacha.Length > 1)
    {
      this.dirBtnDouble.SetActive(true);
      this.dirBtnSingle.SetActive(false);
      this.dirBtnSingleLimit.SetActive(false);
      ((IEnumerable<GachaModuleGacha>) gachaModule.gacha).ForEachIndex<GachaModuleGacha>((Action<GachaModuleGacha, int>) ((x, n) => this.gachaButton[n].Init(gachaModule.name, x, this.Menu, gachaModule.type, gachaModule.number, gachaModule)));
    }
    else
    {
      if (gachaModule.gacha.Length != 1)
        return;
      this.dirBtnDouble.SetActive(false);
      this.dirBtnSingle.SetActive(false);
      this.dirBtnSingleLimit.SetActive(false);
      this.slcStepUPBase.SetActive(false);
      int num1 = 0;
      int num2 = 0;
      if (gachaModule.gacha[0].limit.HasValue)
      {
        num2 = gachaModule.gacha[0].count;
        num1 = gachaModule.gacha[0].limit.Value;
      }
      else if (gachaModule.gacha[0].daily_limit.HasValue)
      {
        num2 = gachaModule.gacha[0].daily_count;
        num1 = gachaModule.gacha[0].daily_limit.Value;
      }
      if (num1 > 0)
      {
        this.dirBtnSingleLimit.SetActive(true);
        this.slcStepUPBase.SetActive(true);
        this.isSingleLimit = true;
        string str1 = string.Format("slc_StepUpSmall_num{0}.png__GUI__006-3_sozai__006-3_sozai_prefab", (object) num1);
        UISpriteData sprite1 = this.slcDenominator.atlas.GetSprite(str1);
        if (sprite1 != null)
        {
          ((UIWidget) this.slcDenominator).SetDimensions(sprite1.width, sprite1.height);
          this.slcDenominator.spriteName = str1;
        }
        string str2 = string.Format("slc_StepUp_num{0}.png__GUI__006-3_sozai__006-3_sozai_prefab", (object) num2);
        UISpriteData sprite2 = this.slcNumerator.atlas.GetSprite(str2);
        if (sprite2 != null)
        {
          ((UIWidget) this.slcNumerator).SetDimensions(sprite2.width, sprite2.height);
          this.slcNumerator.spriteName = str2;
        }
        this.singleGachaButtonEx.Init(gachaModule.name, gachaModule.gacha[0], this.Menu, gachaModule.type, gachaModule.number, gachaModule);
      }
      else
      {
        this.dirBtnSingle.SetActive(true);
        this.singleGachaButton.Init(gachaModule.name, gachaModule.gacha[0], this.Menu, gachaModule.type, gachaModule.number, gachaModule);
      }
    }
  }

  private void SetGachBtnInfo(Player playerData, GachaButton btn, GachaModuleGacha gachaData)
  {
    ((Component) btn).GetComponent<UISprite>();
    ((Component) btn).GetComponentsInChildren<UISprite>();
    UIButton component = ((Component) btn).GetComponent<UIButton>();
    int max_play_nam1 = gachaData.max_roll_count.HasValue ? gachaData.max_roll_count.Value : 0;
    if (max_play_nam1 == 0)
      return;
    int max_play_nam2 = Mathf.Clamp(playerData.friend_point / gachaData.payment_amount, 0, max_play_nam1);
    if (max_play_nam1 == 1)
    {
      this.dirSinglePointLabel.text = gachaData.payment_amount.ToString();
      if (max_play_nam2 >= 1)
      {
        this.dirSingleButton.SetColor(Color.gray);
        foreach (UIWidget uiWidget in this.dirSingleLabelColor)
          uiWidget.color = Color.white;
        ((Behaviour) component).enabled = true;
      }
      else
      {
        this.dirSingleButton.SetColor(((UIButtonColor) this.dirSingleButton).disabledColor);
        foreach (UIWidget uiWidget in this.dirSingleLabelColor)
          uiWidget.color = Color.gray;
        ((Behaviour) component).enabled = false;
        ((UIButtonColor) ((Component) this.gachaButton[0]).GetComponent<UIButton>()).isEnabled = false;
      }
    }
    else
    {
      btn.SetMaxPlayNum(max_play_nam2);
      if (max_play_nam2 >= 2)
      {
        this.dirMultiButton.SetColor(Color.gray);
        foreach (UIWidget uiWidget in this.dirMultiLabelColor)
          uiWidget.color = Color.white;
        this.dirMultiPointLabel.text = (gachaData.payment_amount * max_play_nam2).ToString();
        this.dirMultiGachaLabel.text = Consts.Format(Consts.GetInstance().GACHA_0063MULTI_GACHA_PLAY, (IDictionary) new Hashtable()
        {
          {
            (object) "num",
            (object) max_play_nam2.ToString()
          }
        });
        ((Behaviour) component).enabled = true;
      }
      else
      {
        this.dirMultiButton.SetColor(((UIButtonColor) this.dirMultiButton).disabledColor);
        foreach (UIWidget uiWidget in this.dirMultiLabelColor)
          uiWidget.color = Color.gray;
        this.dirMultiPointLabel.text = (gachaData.payment_amount * 10).ToString();
        this.dirMultiGachaLabel.text = Consts.Format(Consts.GetInstance().GACHA_0063MULTI_GACHA_PLAY, (IDictionary) new Hashtable()
        {
          {
            (object) "num",
            (object) 10.ToString()
          }
        });
        btn.SetMaxPlayNum(max_play_nam1);
        btn.SetMaxPlayNum(max_play_nam1);
        ((Behaviour) component).enabled = false;
        ((UIButtonColor) ((Component) this.gachaButton[1]).GetComponent<UIButton>()).isEnabled = false;
      }
    }
  }

  public override IEnumerator Set(GameObject detailPopup)
  {
    Gacha0063Point gacha0063Point = this;
    if (!PerformanceConfig.GetInstance().IsGachaLowMemory)
    {
      IEnumerator e = Singleton<NGGameDataManager>.GetInstance().LoadWebImage(gacha0063Point.GachaModule.front_image_url);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Player playerData = SMManager.Get<Player>();
    string text = "[ffff00]" + playerData.friend_point.ToString().ToConverter() + "[-]";
    gacha0063Point.TxtGachaPt.SetTextLocalize(text);
    if (gacha0063Point.GachaModule.gacha.Length > 1)
    {
      for (int index = 0; index < 2; ++index)
        gacha0063Point.SetGachBtnInfo(playerData, gacha0063Point.gachaButton[index], gacha0063Point.GachaModule.gacha[index]);
    }
    else if (gacha0063Point.isSingleLimit)
      gacha0063Point.SetGachBtnInfo(playerData, gacha0063Point.singleGachaButtonEx, gacha0063Point.GachaModule.gacha[0]);
    else
      gacha0063Point.SetGachBtnInfo(playerData, gacha0063Point.singleGachaButton, gacha0063Point.GachaModule.gacha[0]);
  }

  public override IEnumerator TextureSet()
  {
    Gacha0063Point gacha0063Point = this;
    IEnumerator e = Singleton<NGGameDataManager>.GetInstance().GetWebImage(gacha0063Point.GachaModule.front_image_url, gacha0063Point.bannerSprite);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void TextureClear()
  {
    if (this.GachaModule.front_image_url != "")
      this.bannerSprite.sprite2D = (Sprite) null;
    if (!PerformanceConfig.GetInstance().IsGachaLowMemory)
      return;
    Singleton<NGGameDataManager>.GetInstance().webImageCache.Remove(this.GachaModule.front_image_url);
  }

  private void ChangeColor(Transform trans, Color color)
  {
    UISprite component = ((Component) trans).GetComponent<UISprite>();
    if (Object.op_Inequality((Object) component, (Object) null))
      ((UIWidget) component).color = color;
    trans.GetChildren().ForEach<Transform>((Action<Transform>) (x => this.ChangeColor(x, color)));
  }
}
