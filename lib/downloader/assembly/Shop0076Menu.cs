// Decompiled with JetBrains decompiler
// Type: Shop0076Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Shop0076Menu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject linkParent;
  [SerializeField]
  private UILabel InpQuantity;
  [SerializeField]
  private UIInput InpUI;
  [SerializeField]
  private UILabel TxtDescription01;
  [SerializeField]
  private UILabel TxtDescription03;
  [SerializeField]
  private UILabel TxtDescription04;
  [SerializeField]
  private UILabel TxtDescription05;
  [SerializeField]
  private UILabel TxtDescription06;
  [SerializeField]
  private UILabel TxtPopuptitle;
  private GameObject linkTarget;
  private string itemName;
  private int itemId;
  private int price;
  private long holding;
  private int quantity;
  private const int INPUT_MIN = 1;
  private const int INPUT_MAX = 999;
  public SpreadColorLongPressButton btnMinus;
  public SpreadColorLongPressButton btnPlus;
  public UIButton btnYes;
  private bool initData;
  public int _playerQuantity;
  public List<Shop0074Scroll> _scrolls;
  private Action<long> _onPurchasedHolding;
  private Func<IEnumerator> _onPurchased;
  private int? _quantityLimit;
  public Sprite[] backSprite;

  public PlayerShopArticle _playerShopArticle { get; set; }

  public ShopArticle _shopArticle { get; set; }

  public Player _player { get; set; }

  public TowerPlayer _towerPlayer { get; set; }

  public PlayerAffiliation _playerAffiliation { get; set; }

  public PlayerShopArticle[] _articles { get; set; }

  public IEnumerator Init(
    PlayerShopArticle playerShopArticle,
    ShopArticle shopArticle,
    Player player,
    TowerPlayer towerPlayer,
    PlayerAffiliation playerAffiliation,
    int playerQuantity,
    List<Shop0074Scroll> scrolls,
    PlayerShopArticle[] articles,
    Func<IEnumerator> onPurchased,
    Action<long> onPurchasedHolding,
    int? quantityLimit)
  {
    this.initData = true;
    this._player = player;
    this._towerPlayer = towerPlayer;
    this._playerAffiliation = playerAffiliation;
    this._playerShopArticle = playerShopArticle;
    this._shopArticle = shopArticle;
    this._playerQuantity = playerQuantity;
    this._scrolls = scrolls;
    this._articles = articles;
    this._onPurchasedHolding = onPurchasedHolding;
    this._onPurchased = onPurchased;
    this._quantityLimit = quantityLimit;
    this.HoldingTypeCheck(this._shopArticle, player, towerPlayer, playerAffiliation);
    this.quantity = 1;
    this.itemId = this._shopArticle.ID;
    this.itemName = this._shopArticle.name;
    this.price = this._shopArticle.price;
    int num = this.capQuantity() ? 1 : 0;
    float duration1 = ((UIButtonColor) this.btnMinus).duration;
    ((UIButtonColor) this.btnMinus).duration = 0.0f;
    ((UIButtonColor) this.btnMinus).isEnabled = false;
    ((UIButtonColor) this.btnMinus).duration = duration1;
    float duration2 = ((UIButtonColor) this.btnPlus).duration;
    ((UIButtonColor) this.btnPlus).duration = 0.0f;
    ((UIButtonColor) this.btnPlus).isEnabled = false;
    ((UIButtonColor) this.btnPlus).duration = duration2;
    float duration3 = ((UIButtonColor) this.btnYes).duration;
    ((UIButtonColor) this.btnYes).duration = 0.0f;
    ((UIButtonColor) this.btnYes).isEnabled = false;
    ((UIButtonColor) this.btnYes).duration = duration3;
    if (num != 0 || this._shopArticle.daily_limit.HasValue && this._playerShopArticle.limit.Value == 1 || this._shopArticle.limit.HasValue && this._playerShopArticle.limit.Value == 1 || (long) (this.quantity * this.price) >= this.holding)
      ((UIButtonColor) this.btnPlus).isEnabled = false;
    else
      ((UIButtonColor) this.btnPlus).isEnabled = true;
    if ((long) (this.quantity * this.price) <= this.holding)
      ((UIButtonColor) this.btnYes).isEnabled = true;
    else
      ((UIButtonColor) this.btnYes).isEnabled = false;
    this.TxtDescription01.SetTextLocalize(this.itemName);
    this.InpQuantity.SetTextLocalize(this.quantity);
    this.SetTxtDescription03(this._shopArticle);
    this.SetTxtDescription05(this._shopArticle);
    this.SetTxtDescription06(this._shopArticle);
    this.SetLongPress();
    yield break;
  }

  private bool capQuantity()
  {
    if (!this._quantityLimit.HasValue)
      return false;
    int num = this._quantityLimit.Value - this._playerQuantity - this.quantity;
    if (num < 0)
      this.quantity = Mathf.Max(0, this._quantityLimit.Value - this._playerQuantity);
    return num <= 0;
  }

  private void HoldingTypeCheck(ShopArticle sa, Player p, TowerPlayer tp, PlayerAffiliation pa)
  {
    if (sa.pay_type == CommonPayType.money)
      this.holding = p.money;
    else if (sa.pay_type == CommonPayType.medal)
      this.holding = (long) p.medal;
    else if (sa.pay_type == CommonPayType.battle_medal)
      this.holding = (long) p.battle_medal;
    else if (sa.pay_type == CommonPayType.tower_medal)
      this.holding = tp != null ? (long) tp.tower_medal : 0L;
    else if (sa.pay_type == CommonPayType.guild_medal)
    {
      this.holding = pa != null ? (long) pa.guild_medal : 0L;
    }
    else
    {
      if (sa.pay_type != CommonPayType.raid_medal)
        return;
      this.holding = (long) p.raid_medal;
    }
  }

  public void SetTxtDescription03(ShopArticle sa)
  {
    if (sa.pay_type == CommonPayType.money)
      this.TxtDescription03.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION03_MONEY, (IDictionary) new Hashtable()
      {
        {
          (object) "money",
          (object) this.price
        }
      }));
    else if (sa.pay_type == CommonPayType.medal)
      this.TxtDescription03.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION03_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) this.price
        }
      }));
    else if (sa.pay_type == CommonPayType.battle_medal)
      this.TxtDescription03.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION03_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) this.price
        }
      }));
    else if (sa.pay_type == CommonPayType.tower_medal)
    {
      Hashtable args = new Hashtable()
      {
        {
          (object) "medal",
          (object) this.price
        },
        {
          (object) "unit",
          (object) Consts.GetInstance().SHOP_TOWERMEDAL_UNIT
        }
      };
      this.TxtDescription03.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION03_xMEDALxUNIT, (IDictionary) args));
    }
    else if (sa.pay_type == CommonPayType.guild_medal)
    {
      this.TxtDescription03.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION03_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) this.price
        }
      }));
    }
    else
    {
      if (sa.pay_type != CommonPayType.raid_medal)
        return;
      this.TxtDescription03.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION03_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) this.price
        }
      }));
    }
  }

  public void SetTxtDescription05(ShopArticle sa)
  {
    if (sa.pay_type == CommonPayType.money)
    {
      string text;
      if (this.holding < (long) (this.quantity * this.price))
        text = Consts.Format(Consts.GetInstance().SHOP_0076_TXT_DESCRIPTION05_MONEY_OVER, (IDictionary) new Hashtable()
        {
          {
            (object) "money",
            (object) (this.quantity * this.price).ToLocalizeNumberText()
          }
        });
      else
        text = Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION05_MONEY, (IDictionary) new Hashtable()
        {
          {
            (object) "money",
            (object) (this.quantity * this.price).ToLocalizeNumberText()
          }
        });
      this.TxtDescription05.SetTextLocalize(text);
    }
    else if (sa.pay_type == CommonPayType.medal)
    {
      string text;
      if (this.holding < (long) (this.quantity * this.price))
        text = Consts.Format(Consts.GetInstance().SHOP_0076_TXT_DESCRIPTION05_MEDAL_OVER, (IDictionary) new Hashtable()
        {
          {
            (object) "medal",
            (object) (this.quantity * this.price).ToLocalizeNumberText()
          }
        });
      else
        text = Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION05_MEDAL, (IDictionary) new Hashtable()
        {
          {
            (object) "medal",
            (object) (this.quantity * this.price).ToLocalizeNumberText()
          }
        });
      this.TxtDescription05.SetTextLocalize(text);
    }
    else if (sa.pay_type == CommonPayType.battle_medal)
    {
      string text;
      if (this.holding < (long) (this.quantity * this.price))
        text = Consts.Format(Consts.GetInstance().SHOP_0076_TXT_DESCRIPTION05_MEDAL_OVER, (IDictionary) new Hashtable()
        {
          {
            (object) "medal",
            (object) (this.quantity * this.price).ToLocalizeNumberText()
          }
        });
      else
        text = Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION05_MEDAL, (IDictionary) new Hashtable()
        {
          {
            (object) "medal",
            (object) (this.quantity * this.price).ToLocalizeNumberText()
          }
        });
      this.TxtDescription05.SetTextLocalize(text);
    }
    else if (sa.pay_type == CommonPayType.tower_medal)
    {
      Hashtable args = new Hashtable()
      {
        {
          (object) "medal",
          (object) (this.quantity * this.price).ToLocalizeNumberText()
        },
        {
          (object) "unit",
          (object) Consts.GetInstance().SHOP_TOWERMEDAL_UNIT
        }
      };
      this.TxtDescription05.SetTextLocalize(this.holding >= (long) (this.quantity * this.price) ? Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION05_xMEDALxUNIT, (IDictionary) args) : Consts.Format(Consts.GetInstance().SHOP_0076_TXT_DESCRIPTION05_xMEDALxUNIT_OVER, (IDictionary) args));
    }
    else if (sa.pay_type == CommonPayType.guild_medal)
    {
      string text;
      if (this.holding < (long) (this.quantity * this.price))
        text = Consts.Format(Consts.GetInstance().SHOP_0076_TXT_DESCRIPTION05_MEDAL_OVER, (IDictionary) new Hashtable()
        {
          {
            (object) "medal",
            (object) (this.quantity * this.price).ToLocalizeNumberText()
          }
        });
      else
        text = Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION05_MEDAL, (IDictionary) new Hashtable()
        {
          {
            (object) "medal",
            (object) (this.quantity * this.price).ToLocalizeNumberText()
          }
        });
      this.TxtDescription05.SetTextLocalize(text);
    }
    else
    {
      if (sa.pay_type != CommonPayType.raid_medal)
        return;
      string text;
      if (this.holding < (long) (this.quantity * this.price))
        text = Consts.Format(Consts.GetInstance().SHOP_0076_TXT_DESCRIPTION05_MEDAL_OVER, (IDictionary) new Hashtable()
        {
          {
            (object) "medal",
            (object) (this.quantity * this.price).ToLocalizeNumberText()
          }
        });
      else
        text = Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION05_MEDAL, (IDictionary) new Hashtable()
        {
          {
            (object) "medal",
            (object) (this.quantity * this.price).ToLocalizeNumberText()
          }
        });
      this.TxtDescription05.SetTextLocalize(text);
    }
  }

  public void SetTxtDescription06(ShopArticle sa)
  {
    if (sa.pay_type == CommonPayType.money)
      this.TxtDescription06.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_0076_TXT_DESCRIPTION06_MONEY, (IDictionary) new Hashtable()
      {
        {
          (object) "money",
          (object) this.holding
        }
      }));
    else if (sa.pay_type == CommonPayType.medal)
      this.TxtDescription06.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_0076_TXT_DESCRIPTION06_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) this.holding
        }
      }));
    else if (sa.pay_type == CommonPayType.battle_medal)
      this.TxtDescription06.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_0076_TXT_DESCRIPTION06_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) this.holding
        }
      }));
    else if (sa.pay_type == CommonPayType.tower_medal)
    {
      Hashtable args = new Hashtable()
      {
        {
          (object) "medal",
          (object) this.holding
        },
        {
          (object) "unit",
          (object) Consts.GetInstance().SHOP_TOWERMEDAL_UNIT
        }
      };
      this.TxtDescription06.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_0076_TXT_DESCRIPTION06_xMEDALxUNIT, (IDictionary) args));
    }
    else if (sa.pay_type == CommonPayType.guild_medal)
      this.TxtDescription06.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_0076_TXT_DESCRIPTION06_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) this.holding
        }
      }));
    else if (sa.pay_type == CommonPayType.raid_medal)
      this.TxtDescription06.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_0076_TXT_DESCRIPTION06_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) this.holding
        }
      }));
    else
      Debug.LogWarning((object) "selected pay_type is ohter than money, medal");
  }

  public void SetLongPress()
  {
    this.btnPlus.onLongPressLoop = (Func<IEnumerator>) (() => this.LongPressedCountPlus());
    this.btnMinus.onLongPressLoop = (Func<IEnumerator>) (() => this.LongPressedCountMinus());
  }

  public void InitObject(GameObject obj)
  {
    this.linkTarget = obj;
    GameObject gameObject = obj.Clone(this.linkParent.transform);
    UnitIcon component1 = gameObject.GetComponent<UnitIcon>();
    ItemIcon component2 = gameObject.GetComponent<ItemIcon>();
    if (Object.op_Implicit((Object) component1))
    {
      component1.SetIconBoxCollider(false);
    }
    else
    {
      if (!Object.op_Implicit((Object) component2))
        return;
      component2.isButtonActive = false;
    }
  }

  public void setKeyboardTypeNumber() => this.InpUI.keyboardType = (UIInput.KeyboardType) 4;

  private void setInput(UILabel label, string input) => label.SetTextLocalize(input);

  public void setTxt(string input) => this.setInput(this.InpQuantity, input);

  public void onChangeInputQuantity() => this.onChangeInputQuantity(true);

  public void onChangeInputQuantity(bool isInit = true)
  {
    if (isInit)
    {
      this.quantity = 1;
      int result;
      if (int.TryParse(this.InpUI.value, out result) && result != 0)
      {
        this.quantity = Mathf.Clamp(result, 1, 999);
        if ((this._shopArticle.daily_limit.HasValue || this._shopArticle.limit.HasValue) && this._playerShopArticle.limit.HasValue && Math.Min(this._playerShopArticle.limit.Value, 999) <= result)
          this.quantity = Math.Min(this._playerShopArticle.limit.Value, 999);
      }
      this.capQuantity();
      this.setTxt(this.quantity.ToString());
    }
    else
      this.capQuantity();
    if (this._shopArticle.daily_limit.HasValue || this._shopArticle.limit.HasValue || this._quantityLimit.HasValue)
    {
      int val2 = Math.Min(this._playerShopArticle.limit.Value, 999);
      if (this._quantityLimit.HasValue)
        val2 = Math.Min(Math.Max(0, this._quantityLimit.Value - this._playerQuantity), val2);
      if (this.quantity < val2)
        ((UIButtonColor) this.btnPlus).isEnabled = true;
      else
        ((UIButtonColor) this.btnPlus).isEnabled = false;
    }
    else if (this.quantity >= 999)
      ((UIButtonColor) this.btnPlus).isEnabled = false;
    else
      ((UIButtonColor) this.btnPlus).isEnabled = true;
    this.InpQuantity.SetTextLocalize(this.quantity.ToString());
    if (this.quantity <= 1)
      ((UIButtonColor) this.btnMinus).isEnabled = false;
    else
      ((UIButtonColor) this.btnMinus).isEnabled = true;
    if ((long) (this.quantity * this.price) > this.holding)
    {
      this.InpQuantity.SetTextLocalize(this.quantity.ToString());
      ((UIButtonColor) this.btnYes).isEnabled = false;
    }
    else
      ((UIButtonColor) this.btnYes).isEnabled = true;
    this.SetTxtDescription05(this._shopArticle);
    this.InpQuantity.SetTextLocalize(this.quantity);
    this.InpUI.value = this.quantity.ToString();
  }

  private IEnumerator OpenPopup00771()
  {
    Future<GameObject> prefab00771F = Res.Prefabs.popup.popup_007_7_1__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab00771F.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefab00771F.Result.Clone();
    Shop00771Menu component = prefab.GetComponent<Shop00771Menu>();
    component.InitObject(this.linkTarget);
    component.InitData(this._playerShopArticle, this.itemId, this.itemName, this.quantity, this._playerQuantity, this.price, this.holding, this._scrolls, this._articles, this._onPurchased, this._onPurchasedHolding);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  private IEnumerator LongPressedCountPlus()
  {
    while (true)
    {
      while (this._shopArticle.daily_limit.HasValue || this._shopArticle.limit.HasValue)
      {
        if ((this._playerShopArticle.limit.HasValue ? (Math.Min(this._playerShopArticle.limit.Value, 999) <= this.quantity ? 1 : 0) : 0) != 0)
        {
          yield break;
        }
        else
        {
          ++this.quantity;
          this.onChangeInputQuantity(false);
          yield return (object) new WaitForSeconds(0.1f);
        }
      }
      if (this.quantity < 999)
      {
        ++this.quantity;
        this.onChangeInputQuantity(false);
        yield return (object) new WaitForSeconds(0.1f);
      }
      else
        break;
    }
  }

  private IEnumerator LongPressedCountMinus()
  {
    while (this.quantity > 0)
    {
      --this.quantity;
      this.onChangeInputQuantity(false);
      yield return (object) new WaitForSeconds(0.1f);
    }
  }

  public void IbtnMinus()
  {
    if (this.quantity > 1)
      --this.quantity;
    this.onChangeInputQuantity(false);
  }

  public void IbtnPlus()
  {
    if (this.quantity < 999 & (!this._playerShopArticle.limit.HasValue || this.quantity < Math.Min(this._playerShopArticle.limit.Value, 999)))
      ++this.quantity;
    this.onChangeInputQuantity(false);
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void IbtnPopupYes()
  {
    if (this.quantity == 0 || this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss(true);
    if ((long) (this.quantity * this.price) > this.holding)
      this.StartCoroutine(PopupUtility._999_7_1(this._shopArticle));
    else
      this.StartCoroutine(this.OpenPopup00771());
  }

  protected override void Update()
  {
    if (!this.initData)
      this.StartCoroutine(this.Init(this._playerShopArticle, this._shopArticle, this._player, this._towerPlayer, this._playerAffiliation, this._playerQuantity, this._scrolls, this._articles, this._onPurchased, this._onPurchasedHolding, this._quantityLimit));
    base.Update();
  }
}
