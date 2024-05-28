// Decompiled with JetBrains decompiler
// Type: ConditionForVictory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UniLinq;
using UnityEngine;

#nullable disable
public class ConditionForVictory : MonoBehaviour
{
  [SerializeField]
  private TextMesh text;
  [SerializeField]
  private TextMesh textTwoLine;
  [SerializeField]
  private TextMesh textEff;
  [SerializeField]
  private TextMesh textEffTwoLine;
  [SerializeField]
  private GameObject defeatCondition;
  [SerializeField]
  private TextMesh textDefeatCondition;
  [SerializeField]
  private TextMesh textDefeatConditionTwoLine;
  [SerializeField]
  private GameObject waveNumObject;
  [SerializeField]
  private TextMesh textWaveNum;

  public void Initialize(BattleVictoryCondition condition, int wave, int maxWave)
  {
    ((Component) this.text).gameObject.SetActive(false);
    ((Component) this.textEff).gameObject.SetActive(false);
    ((Component) this.textTwoLine).gameObject.SetActive(false);
    ((Component) this.textEffTwoLine).gameObject.SetActive(false);
    if (condition.victory_text.Count<char>((Func<char, bool>) (x => x.Equals('\n'))) == 0)
    {
      ((Component) this.text).gameObject.SetActive(true);
      ((Component) this.textEff).gameObject.SetActive(true);
      this.text.text = condition.victory_text;
      this.textEff.text = condition.victory_text;
    }
    else
    {
      ((Component) this.textTwoLine).gameObject.SetActive(true);
      ((Component) this.textEffTwoLine).gameObject.SetActive(true);
      this.textTwoLine.text = condition.victory_text;
      this.textEffTwoLine.text = condition.victory_text;
    }
    this.defeatCondition.SetActive(false);
    if (!string.IsNullOrEmpty(condition.lose_text))
    {
      this.defeatCondition.SetActive(true);
      ((Component) this.textDefeatCondition).gameObject.SetActive(false);
      ((Component) this.textDefeatConditionTwoLine).gameObject.SetActive(false);
      if (condition.lose_text.Count<char>((Func<char, bool>) (x => x.Equals('\n'))) == 0)
      {
        ((Component) this.textDefeatCondition).gameObject.SetActive(true);
        this.textDefeatCondition.text = condition.lose_text;
      }
      else
      {
        ((Component) this.textDefeatConditionTwoLine).gameObject.SetActive(true);
        this.textDefeatConditionTwoLine.text = condition.lose_text;
      }
    }
    this.waveNumObject.SetActive(false);
    if (maxWave <= 0)
      return;
    this.waveNumObject.SetActive(true);
    this.textWaveNum.text = Consts.Format(Consts.GetInstance().CONDITION_FOR_VICTORY_WAVE, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) wave
      },
      {
        (object) "max",
        (object) maxWave
      }
    });
  }
}
