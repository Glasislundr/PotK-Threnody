// Decompiled with JetBrains decompiler
// Type: MypageIntroductionControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class MypageIntroductionControl
{
  private Queue<MypageIntroductionControl.Unit> queUnits_ = new Queue<MypageIntroductionControl.Unit>();
  private Action<int> onEnd_;

  public void addExecute(Func<int, bool> check, Func<int, IEnumerator> execute)
  {
    this.queUnits_.Enqueue(new MypageIntroductionControl.Unit(check, execute));
  }

  public void setEnd(Action<int> actEnd) => this.onEnd_ = actEnd;

  public IEnumerator doExecute()
  {
    int count = 0;
    while (this.queUnits_.Any<MypageIntroductionControl.Unit>())
    {
      MypageIntroductionControl.Unit unit = this.queUnits_.Dequeue();
      if (unit.checkExec_(count))
      {
        IEnumerator e = unit.execute_(count);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        ++count;
      }
    }
    if (this.onEnd_ != null)
      this.onEnd_(count);
  }

  public void clear()
  {
    this.queUnits_.Clear();
    this.onEnd_ = (Action<int>) null;
  }

  private class Unit
  {
    public Func<int, bool> checkExec_ { get; private set; }

    public Func<int, IEnumerator> execute_ { get; private set; }

    public Unit(Func<int, bool> check, Func<int, IEnumerator> execute)
    {
      this.checkExec_ = check;
      this.execute_ = execute;
    }
  }
}
