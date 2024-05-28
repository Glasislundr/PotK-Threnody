// Decompiled with JetBrains decompiler
// Type: AI.Logic.LispAILogic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using GameCore.LispCore;
using GameCore.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace AI.Logic
{
  public class LispAILogic : AILogicBase
  {
    private AILisp engine;
    private SExpNumber numberDic = new SExpNumber(new Dictionary<Decimal, Decimal?>());
    private bool loadInitialized;
    private bool isTerminate;
    private IELisp.ReturnObject retObj = new IELisp.ReturnObject();

    public void clearCache() => this.engine.clearCache();

    public object readLisp(string script)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str1 = script;
      char[] chArray = new char[1]{ '\n' };
      foreach (string str2 in str1.Split(chArray))
      {
        string str3 = str2.Trim();
        if (str3.Length != 0 && !str3.StartsWith(";"))
          stringBuilder.AppendLine(str3);
      }
      return new SExpReader(this.numberDic).parse(stringBuilder.ToString(), true);
    }

    public object readLisp(byte[] scriptBytes, byte[] numberBytes)
    {
      if (numberBytes != null)
        this.numberDic = EasySerializer.DeserializeObjectFromMemory(numberBytes) as SExpNumber;
      return EasySerializer.DeserializeObjectFromMemory(scriptBytes);
    }

    public void setReloadFlag() => this.loadInitialized = false;

    public IEnumerator createEngine(object sexp)
    {
      LispAILogic lispAiLogic = this;
      lispAiLogic.loadInitialized = false;
      lispAiLogic.isTerminate = true;
      lispAiLogic.engine = new AILisp(lispAiLogic.env, lispAiLogic.numberDic);
      object l;
      for (l = sexp; SExp.consp_(l); l = SExp.cdr(l))
      {
        IEnumerator ee = lispAiLogic.engine.evalTopLevelE(SExp.car(l), lispAiLogic.retObj);
        while (ee.MoveNext())
          yield return ee.Current;
        ee = (IEnumerator) null;
      }
      l = (object) null;
      lispAiLogic.loadInitialized = true;
    }

    private bool initLocal() => this.loadInitialized && this.isInitialized;

    public override IEnumerator doExecute()
    {
      LispAILogic lispAiLogic = this;
label_2:
      while (!lispAiLogic.initLocal())
        yield return (object) null;
      lispAiLogic.isTerminate = false;
      IEnumerator e = lispAiLogic.doAI();
      while (e.MoveNext())
      {
        yield return e.Current;
        if (lispAiLogic.isTerminate)
        {
          yield return (object) null;
          break;
        }
      }
      e = (IEnumerator) null;
      while (lispAiLogic.isCompleted)
        yield return (object) null;
      goto label_2;
    }

    public override bool terminateAI() => this.isTerminate = true;

    private IEnumerator doAI()
    {
      IEnumerator e = this.engine.topLevelCallE((object) "main", (object) null, this.retObj);
      while (e.MoveNext())
      {
        yield return e.Current;
        if (this.isTerminate)
          yield break;
        else if (this.engine.error != null)
        {
          yield return (object) null;
          Debug.LogError((object) ("AI script error!: " + this.engine.error));
          using (List<object>.Enumerator enumerator = this.engine.errorObjects.GetEnumerator())
          {
            while (enumerator.MoveNext())
              Debug.LogError((object) ("     :" + enumerator.Current));
            yield break;
          }
        }
      }
      e = (IEnumerator) null;
    }
  }
}
