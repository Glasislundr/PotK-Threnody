// Decompiled with JetBrains decompiler
// Type: GuildBattleRecordScroll
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
public class GuildBattleRecordScroll : MonoBehaviour
{
  private const string NumSpriteName = "slc_text_number_{0}_60pt.png__GUI__guild_common__guild_common_prefab";
  private bool isEnemy;
  [SerializeField]
  private UILabel opponentGuildName;
  [SerializeField]
  private UI2DSprite opponentGuildTitle;
  [SerializeField]
  private GameObject dir_win;
  [SerializeField]
  private GameObject slc_lose;
  [SerializeField]
  private GameObject slc_draw;
  [SerializeField]
  private UIButton btnMemberScore;
  [SerializeField]
  private UILabel lblData;
  [SerializeField]
  private List<UISprite> spriteStarPlayer;
  [SerializeField]
  private List<UISprite> spriteStarOpponent;
  private GameObject memberScorePopup;
  private GvgHistory record;

  private IEnumerator ShowMemberScorePopup()
  {
    GameObject popup = this.memberScorePopup.Clone();
    GuildMemberScorePopup component = popup.GetComponent<GuildMemberScorePopup>();
    popup.SetActive(false);
    IEnumerator e = component.InitializeAsync(this.isEnemy, this.record.player_histories);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
  }

  public IEnumerator InitializeAsync(bool isEnemy, GvgHistory record, GameObject memberScorePopup)
  {
    this.isEnemy = isEnemy;
    this.record = record;
    this.memberScorePopup = memberScorePopup;
    this.opponentGuildName.SetTextLocalize(record.target_guild_name);
    ((Component) this.lblData).gameObject.SetActive(record.created_at.HasValue);
    if (record.created_at.HasValue)
    {
      UILabel lblData = this.lblData;
      string battleRecordDate = Consts.GetInstance().POPUP_GUILD_BATTLE_RECORD_DATE;
      Hashtable args = new Hashtable();
      DateTime dateTime = record.created_at.Value;
      args.Add((object) "month", (object) dateTime.Month);
      dateTime = record.created_at.Value;
      args.Add((object) "day", (object) dateTime.Day);
      string text = Consts.Format(battleRecordDate, (IDictionary) args);
      lblData.SetTextLocalize(text);
    }
    Future<Sprite> emblemF = EmblemUtility.LoadGuildEmblemSprite(record.target_guild_emblem_id.Value);
    IEnumerator e = emblemF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.opponentGuildTitle.sprite2D = emblemF.Result;
    this.dir_win.SetActive(false);
    this.slc_lose.SetActive(false);
    this.slc_draw.SetActive(false);
    if (record.status == GvgBattleStatus.win)
      this.dir_win.SetActive(true);
    else if (record.status == GvgBattleStatus.lose)
      this.slc_lose.SetActive(true);
    else
      this.slc_draw.SetActive(true);
    int num = record.attack_star.Value;
    string str1 = num.ToString();
    for (int index = 0; index < this.spriteStarPlayer.Count; ++index)
      ((Component) this.spriteStarPlayer[index]).gameObject.SetActive(false);
    for (int index = 0; index < str1.Length; ++index)
      this.spriteStarPlayer[index].SetSprite(string.Format("slc_text_number_{0}_60pt.png__GUI__guild_common__guild_common_prefab", (object) str1[str1.Length - index - 1]));
    num = record.opponent_attack_star.Value;
    string str2 = num.ToString();
    for (int index = 0; index < this.spriteStarOpponent.Count; ++index)
      ((Component) this.spriteStarOpponent[index]).gameObject.SetActive(false);
    for (int index = 0; index < str2.Length; ++index)
    {
      ((Component) this.spriteStarOpponent[index]).gameObject.SetActive(true);
      this.spriteStarOpponent[index].SetSprite(string.Format("slc_text_number_{0}_60pt.png__GUI__guild_common__guild_common_prefab", (object) str2[index]));
    }
    ((Component) this.btnMemberScore).GetComponent<BoxCollider>().size = Vector2.op_Implicit(((UIWidget) ((Component) this.btnMemberScore).GetComponent<UISprite>()).localSize);
  }

  public void onMemberScoreButton() => this.StartCoroutine(this.ShowMemberScorePopup());
}
