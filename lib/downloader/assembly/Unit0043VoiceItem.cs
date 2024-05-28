// Decompiled with JetBrains decompiler
// Type: Unit0043VoiceItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using UnityEngine;

#nullable disable
public class Unit0043VoiceItem : MonoBehaviour
{
  public UILabel voiceName;
  public UIButton btn;
  private UnitVoiceView voiceView;
  private bool isTrustLock;
  private bool isRarityLock;
  private UnitUnit unit;
  private Unit0043Menu menu;
  private const string lockStateSprite = "ibtn_VoiceReproduction_locked.png__GUI__004-3_sozai__004-3_sozai_prefab";
  private const string idleStateSprite = "ibtn_VoiceReproduction_idle.png__GUI__004-3_sozai__004-3_sozai_prefab";
  private const string playingStateSprite = "ibtn_VoiceReproduction_playing.png__GUI__004-3_sozai__004-3_sozai_prefab";

  public void Init(
    Unit0043Menu menu,
    UnitVoiceView voice,
    PlayerUnit targetUnit,
    ref int lockedCount,
    Action<string> message)
  {
    this.voiceView = voice;
    this.menu = menu;
    this.unit = targetUnit.unit;
    this.btn = ((Component) this).gameObject.GetComponentInChildren<UIButton>();
    this.btn.onClick.Clear();
    this.btn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() =>
    {
      this.menu.messageObj.SetActive(false);
      if (!this.isTrustLock && !this.isRarityLock)
        this.PlayUnitVoice(voice.Cue_ID);
      else if (this.isTrustLock)
      {
        if (!targetUnit.unit.IsSea)
          message(string.Format(Consts.GetInstance().unit_004_3_voice_trust_text, (object) this.voiceView.ConditionValue));
        else
          message(string.Format(Consts.GetInstance().unit_004_3_voice_love_text, (object) this.voiceView.ConditionValue));
      }
      else
        message(string.Format(Consts.GetInstance().unit_004_3_voice_rarity_text, (object) this.voiceView.ConditionValue));
    })));
    this.voiceName.SetTextLocalize(voice.Cue_Info);
    if (this.voiceView.Condition == 1)
    {
      if (!targetUnit.is_trust)
        return;
      if ((double) targetUnit.trust_rate < (double) this.voiceView.ConditionValue)
      {
        this.isTrustLock = true;
        this.btn.normalSprite = "ibtn_VoiceReproduction_locked.png__GUI__004-3_sozai__004-3_sozai_prefab";
        ++lockedCount;
      }
      else
      {
        this.isTrustLock = false;
        this.btn.normalSprite = "ibtn_VoiceReproduction_idle.png__GUI__004-3_sozai__004-3_sozai_prefab";
      }
    }
    else if (this.voiceView.Condition == 2)
    {
      if (!targetUnit.is_trust)
        return;
      if (targetUnit.unit.rarity.index < this.voiceView.ConditionValue)
      {
        this.isRarityLock = true;
        this.btn.normalSprite = "ibtn_VoiceReproduction_locked.png__GUI__004-3_sozai__004-3_sozai_prefab";
        ++lockedCount;
      }
      else
      {
        this.isRarityLock = false;
        this.btn.normalSprite = "ibtn_VoiceReproduction_idle.png__GUI__004-3_sozai__004-3_sozai_prefab";
      }
    }
    else
    {
      this.isTrustLock = false;
      this.isRarityLock = false;
      this.btn.normalSprite = "ibtn_VoiceReproduction_idle.png__GUI__004-3_sozai__004-3_sozai_prefab";
    }
  }

  private void PlayUnitVoice(int cueID)
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (!instance.IsEffectiveCueID(this.unit.unitVoicePattern.file_name, cueID))
      return;
    instance.stopVoice(time: 0.0f);
    instance.playVoiceByID(this.unit.unitVoicePattern, cueID);
    this.menu.lastPlayCueID = cueID;
  }

  private void Update()
  {
    if (this.isTrustLock || this.isRarityLock)
      return;
    if (Singleton<NGSoundManager>.GetInstance().IsVoicePlaying(0) && this.voiceView.Cue_ID == this.menu.lastPlayCueID)
      this.btn.normalSprite = "ibtn_VoiceReproduction_playing.png__GUI__004-3_sozai__004-3_sozai_prefab";
    else
      this.btn.normalSprite = "ibtn_VoiceReproduction_idle.png__GUI__004-3_sozai__004-3_sozai_prefab";
  }
}
