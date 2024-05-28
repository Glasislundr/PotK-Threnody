// Decompiled with JetBrains decompiler
// Type: MasterDataTable.AIScorePattern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class AIScorePattern
  {
    public int ID;
    public string name;

    public AIScorePattern.AICalculator Attack(PlayerUnit unit)
    {
      return this.getCalculator("a" + (object) unit.id, unit.ai_attack, "attack");
    }

    public AIScorePattern.AICalculator Move(PlayerUnit unit)
    {
      return this.getCalculator("m" + (object) unit.id, unit.ai_move, "move");
    }

    public AIScorePattern.AICalculator Heal(PlayerUnit unit)
    {
      return this.getCalculator("h" + (object) unit.id, unit.ai_heal, "heal");
    }

    public AIScorePattern.AICalculator Use(PlayerUnit unit)
    {
      return string.IsNullOrEmpty(unit.ai_use) ? (AIScorePattern.AICalculator) null : this.getCalculator("u" + (object) unit.id, unit.ai_use, "use");
    }

    private AIScorePattern.AICalculator getCalculator(string name, string exp_, string group)
    {
      string optional_exp = exp_.Trim();
      return new AIScorePattern.AICalculator(this.ID, group, optional_exp);
    }

    public static AIScorePattern Parse(MasterDataReader reader)
    {
      return new AIScorePattern()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true)
      };
    }

    public class AICalculator
    {
      private static GameGlobalVariable<Dictionary<string, GameCore.Calculator>> expCache = GameGlobalVariable<Dictionary<string, GameCore.Calculator>>.New();
      private readonly GameCore.Calculator calc;
      private readonly List<string> exps;
      private readonly int id;

      public AICalculator(int id_, string name, string optional_exp = null)
      {
        AIScorePattern.AICalculator aiCalculator = this;
        this.id = id_;
        AIScore[] aiScoreList = MasterData.AIScoreList;
        Dictionary<int, AIScoreCorrection> yx = ((IEnumerable<AIScoreCorrection>) MasterData.AIScoreCorrectionList).Where<AIScoreCorrection>((Func<AIScoreCorrection, bool>) (x => x.pattern_AIScorePattern == aiCalculator.id)).ToDictionary<AIScoreCorrection, int, AIScoreCorrection>((Func<AIScoreCorrection, int>) (x => x.score_AIScore), (Func<AIScoreCorrection, AIScoreCorrection>) (x => x));
        AIScoreCorrection y;
        this.exps = ((IEnumerable<AIScore>) aiScoreList).Where<AIScore>((Func<AIScore, bool>) (x => x.name == name)).Select<AIScore, string>((Func<AIScore, string>) (x => !yx.TryGetValue(x.ID, out y) ? x.exp.Replace("VAR1", x.var1.ToString()).Replace("VAR2", x.var2.ToString()).Replace("VAR3", x.var3.ToString()) : x.exp.Replace("VAR1", y.var1.ToString()).Replace("VAR2", y.var2.ToString()).Replace("VAR3", y.var3.ToString()))).ToList<string>();
        if (!string.IsNullOrEmpty(optional_exp))
          this.exps.Add(optional_exp);
        string str = "(" + this.exps.Join(") * (") + ")";
        Dictionary<string, GameCore.Calculator> dictionary = AIScorePattern.AICalculator.expCache.Get();
        if (dictionary.TryGetValue(str, out this.calc))
          return;
        this.calc = new GameCore.Calculator(str);
        dictionary[str] = this.calc;
      }

      public float Eval(Dictionary<string, float> vars) => this.calc.Eval(vars);

      public string Show(Dictionary<string, float> vars)
      {
        List<string> list = ((IEnumerable<AIScore>) MasterData.AIScoreList).Select<AIScore, string>((Func<AIScore, string>) (x => x.ID.ToString())).ToList<string>();
        list.Add("ext");
        return list.Select<string, string, string>((IEnumerable<string>) this.exps, (Func<string, string, string>) ((x, y) => string.Format("{0}({1:0.0000})", (object) x, (object) new GameCore.Calculator(y).Eval(vars)))).Join(" * ");
      }

      public string Show() => this.id.ToString() + "," + this.calc.Show();

      public bool isType<T>(Dictionary<string, float> vars) => this.calc.isType<T>(vars);

      public T getParamerter<T>(Dictionary<string, float> vars) => this.calc.getParam<T>(vars);
    }
  }
}
