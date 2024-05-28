// Decompiled with JetBrains decompiler
// Type: GuildLevelUpAnimPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GuildLevelUpAnimPopup : MonoBehaviour
{
  public const int BaseLayer = 0;
  [SerializeField]
  private Animator anim;
  [SerializeField]
  private Transform dir_guild_base_after;
  [SerializeField]
  private Transform dir_guild_base_before;
  [SerializeField]
  private Transform dir_guild_base_after_eff;
  [SerializeField]
  private Transform dir_guild_base_before_eff;
  [SerializeField]
  private UILabel txt_guild_level_before;
  [SerializeField]
  private UILabel txt_guild_level_after;

  public void Initialize(
    int level,
    GameObject guildBase,
    GameObject guildBaseEff,
    GuildImageCache imageCache)
  {
    GameObject gameObject1 = guildBase.Clone(this.dir_guild_base_after);
    gameObject1.GetComponent<Guild0282GuildBase>().GuildBankLevelUpSetSprite(level, imageCache);
    gameObject1.SetActive(true);
    GameObject gameObject2 = guildBaseEff.Clone(this.dir_guild_base_after_eff);
    gameObject2.GetComponent<Guild0282GuildBase>().GuildBankLevelUpSetSprite(level, imageCache);
    gameObject2.SetActive(true);
    int num = level - 1;
    GameObject gameObject3 = guildBase.Clone(this.dir_guild_base_before);
    gameObject3.GetComponent<Guild0282GuildBase>().GuildBankLevelUpSetSprite(num, imageCache);
    gameObject3.SetActive(true);
    GameObject gameObject4 = guildBaseEff.Clone(this.dir_guild_base_before_eff);
    gameObject4.GetComponent<Guild0282GuildBase>().GuildBankLevelUpSetSprite(num, imageCache);
    gameObject4.SetActive(true);
    this.SetLevel(num, level);
  }

  private void SetLevel(int before, int after)
  {
    this.txt_guild_level_before.SetTextLocalize(before);
    this.txt_guild_level_after.SetTextLocalize(after);
  }

  public void Skip()
  {
    this.anim.speed = 0.0f;
    this.Stop();
  }

  public void Stop() => Singleton<PopupManager>.GetInstance().dismiss();

  public void PlaySound(string clip) => Singleton<NGSoundManager>.GetInstance().PlaySe(clip);

  public bool animEnd
  {
    get
    {
      AnimatorStateInfo animatorStateInfo = this.anim.GetCurrentAnimatorStateInfo(0);
      return (double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime >= 1.0;
    }
  }

  private void Update()
  {
    if (!this.animEnd)
      return;
    this.Stop();
    ((Behaviour) this).enabled = false;
  }
}
