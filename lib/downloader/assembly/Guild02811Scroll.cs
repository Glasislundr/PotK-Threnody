// Decompiled with JetBrains decompiler
// Type: Guild02811Scroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild02811Scroll : MonoBehaviour
{
  private GuildDirectory guild;
  private GuildInfoPopup guildPopup;
  [SerializeField]
  private Transform guildBasePos;
  private GameObject guildBasePrefab;
  private Guild0282GuildBase guildBase;
  [SerializeField]
  private UI2DSprite guildTitleImage;
  [SerializeField]
  private UILabel guildName;
  [SerializeField]
  private UISprite guildRank1;
  [SerializeField]
  private UISprite guildRank10;
  [SerializeField]
  private UILabel nextExp;
  [SerializeField]
  private UILabel currentMember;
  [SerializeField]
  private UILabel maxMember;
  [SerializeField]
  private NGTweenGaugeScale expGauge;
  [SerializeField]
  private GameObject applyObj;
  [SerializeField]
  private GameObject battleObj;
  [SerializeField]
  private UILabel playerExpBonus;
  [SerializeField]
  private UILabel dropBonus;
  [SerializeField]
  private UILabel unitExpBonus;
  [SerializeField]
  private UILabel zenyBonus;
  private bool fromChangeGuild;
  private GuildImageCache guildImageCache;

  public IEnumerator Initialize(
    GuildDirectory guildData,
    GuildInfoPopup popup,
    bool IsApply,
    bool changeGuild = false)
  {
    this.guildImageCache = new GuildImageCache();
    this.applyObj.SetActive(IsApply);
    this.battleObj.SetActive(guildData.in_gvg);
    this.fromChangeGuild = changeGuild;
    this.guild = guildData;
    this.guildPopup = popup;
    IEnumerator e;
    if (Object.op_Equality((Object) this.guildBasePrefab, (Object) null))
    {
      Future<GameObject> fgObj = Res.Prefabs.guild028_2.GuildBase.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildBasePrefab = fgObj.Result;
      e = this.guildImageCache.ResourceLoad(guildData.appearance);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      fgObj = (Future<GameObject>) null;
    }
    e = this.SetGuildData(guildData);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void UpdateApply(string guildID)
  {
    this.applyObj.SetActive(this.guild.guild_id == guildID);
  }

  private IEnumerator SetGuildData(GuildDirectory data)
  {
    this.guild = data;
    this.guildName.SetTextLocalize(this.guild.guild_name);
    this.SetGuildData(this.guild.appearance, this.guild.guild_id);
    Future<Sprite> futureGuildTitleImage = EmblemUtility.LoadGuildEmblemSprite(this.guild.appearance._current_emblem);
    IEnumerator e = futureGuildTitleImage.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.guildTitleImage.sprite2D = futureGuildTitleImage.Result;
  }

  private void SetGuildData(GuildAppearance data, string guild_id)
  {
    if (Object.op_Inequality((Object) this.guildBase, (Object) null))
      Object.Destroy((Object) this.guildBase);
    this.guildBase = this.guildBasePrefab.Clone(((Component) this.guildBasePos).transform).GetComponent<Guild0282GuildBase>();
    ((Collider) ((Component) this.guildBase).GetComponent<BoxCollider>()).enabled = false;
    this.guildBase.SetSprite(data, this.guildImageCache);
    if (data.level / 10 == 0)
    {
      ((Component) this.guildRank1).gameObject.SetActive(false);
      ((Component) this.guildRank10).gameObject.SetActive(true);
      this.guildRank10.SetSprite(string.Format("slc_text_glv_number{0}.png__GUI__guild_common_other__guild_common_other_prefab", (object) (data.level % 10)));
    }
    else
    {
      ((Component) this.guildRank10).gameObject.SetActive(true);
      this.guildRank10.SetSprite(string.Format("slc_text_glv_number{0}.png__GUI__guild_common_other__guild_common_other_prefab", (object) (data.level / 10)));
      this.guildRank1.SetSprite(string.Format("slc_text_glv_number{0}.png__GUI__guild_common_other__guild_common_other_prefab", (object) (data.level % 10)));
    }
    this.nextExp.SetTextLocalize(Consts.Format(Consts.GetInstance().Guild0281MENU_NEXTEXP, (IDictionary) new Hashtable()
    {
      {
        (object) "nextExp",
        (object) data.experience_next
      }
    }));
    if (data.experience_next == 0)
      this.expGauge.setValue(0, 1);
    else
      this.expGauge.setValue(data.experience, data.experience + data.experience_next);
    this.currentMember.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_4_CURRENT_MEMBER, (IDictionary) new Hashtable()
    {
      {
        (object) "currentMember",
        (object) data.membership_num
      }
    }));
    this.maxMember.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_028_1_1_4_MAX_MEMBER, (IDictionary) new Hashtable()
    {
      {
        (object) "maxMember",
        (object) data.membership_capacity
      }
    }));
    this.playerExpBonus.SetTextLocalize(this.guild.level_bonus.player_exp);
    this.dropBonus.SetTextLocalize(this.guild.level_bonus.item);
    this.unitExpBonus.SetTextLocalize(this.guild.level_bonus.unit_exp);
    this.zenyBonus.SetTextLocalize(this.guild.level_bonus.money);
  }

  public void onButtonGuildInfo() => this.StartCoroutine(this.ShowGuildInfo());

  private IEnumerator ShowGuildInfo()
  {
    GameObject prefab = this.guildPopup.guildInfoPopup.Clone();
    Guild028114Popup component = prefab.GetComponent<Guild028114Popup>();
    prefab.SetActive(false);
    component.Initialize(this.guild, this.guildPopup, this.fromChangeGuild);
    prefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    IEnumerator e = component.ResetScrollPosition();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
