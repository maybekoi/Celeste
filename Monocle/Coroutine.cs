// Decompiled with JetBrains decompiler
// Type: Monocle.Coroutine
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using System.Collections;
using System.Collections.Generic;

namespace Monocle
{

    public class Coroutine : Component
    {
      public bool RemoveOnComplete = true;
      public bool UseRawDeltaTime;
      private Stack<IEnumerator> enumerators;
      private float waitTimer;
      private bool ended;

      public bool Finished { get; private set; }

      public Coroutine(IEnumerator functionCall, bool removeOnComplete = true)
        : base(true, false)
      {
        this.enumerators = new Stack<IEnumerator>();
        this.enumerators.Push(functionCall);
        this.RemoveOnComplete = removeOnComplete;
      }

      public Coroutine(bool removeOnComplete = true)
        : base(false, false)
      {
        this.RemoveOnComplete = removeOnComplete;
        this.enumerators = new Stack<IEnumerator>();
      }

      public override void Update()
      {
        this.ended = false;
        if ((double) this.waitTimer > 0.0)
        {
          this.waitTimer -= this.UseRawDeltaTime ? Engine.RawDeltaTime : Engine.DeltaTime;
        }
        else
        {
          if (this.enumerators.Count <= 0)
            return;
          IEnumerator enumerator = this.enumerators.Peek();
          if (enumerator.MoveNext() && !this.ended)
          {
            if (enumerator.Current is int)
              this.waitTimer = (float) (int) enumerator.Current;
            if (enumerator.Current is float)
            {
              this.waitTimer = (float) enumerator.Current;
            }
            else
            {
              if (!(enumerator.Current is IEnumerator))
                return;
              this.enumerators.Push(enumerator.Current as IEnumerator);
            }
          }
          else
          {
            if (this.ended)
              return;
            this.enumerators.Pop();
            if (this.enumerators.Count != 0)
              return;
            this.Finished = true;
            this.Active = false;
            if (!this.RemoveOnComplete)
              return;
            this.RemoveSelf();
          }
        }
      }

      public void Cancel()
      {
        this.Active = false;
        this.Finished = true;
        this.waitTimer = 0.0f;
        this.enumerators.Clear();
        this.ended = true;
      }

      public void Replace(IEnumerator functionCall)
      {
        this.Active = true;
        this.Finished = false;
        this.waitTimer = 0.0f;
        this.enumerators.Clear();
        this.enumerators.Push(functionCall);
        this.ended = true;
      }
    }
}
