// Decompiled with JetBrains decompiler
// Type: Celeste.SummitVignette
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Monocle;
using System;
using System.IO;
using System.Xml;

namespace Celeste
{

    public class SummitVignette : Scene
    {
      private CompleteRenderer complete;
      private bool slideFinished;
      private Session session;
      private bool ending;
      private bool ready;
      private bool addedRenderer;

      public SummitVignette(Session session)
      {
        this.session = session;
        session.Audio.Apply();
        RunThread.Start(new Action(this.LoadCompleteThread), "SUMMIT_VIGNETTE");
      }

      private void LoadCompleteThread()
      {
        Atlas atlas = (Atlas) null;
        XmlElement xml = GFX.CompleteScreensXml["Screens"]["SummitIntro"];
        if (xml != null)
          atlas = Atlas.FromAtlas(Path.Combine("Graphics", "Atlases", xml.Attr("atlas")), Atlas.AtlasDataFormat.PackerNoAtlas);
        this.complete = new CompleteRenderer(xml, atlas, 0.0f, (Action) (() => this.slideFinished = true));
        this.complete.SlideDuration = 7.5f;
        this.ready = true;
      }

      public override void Update()
      {
        if (this.ready && !this.addedRenderer)
        {
          this.Add((Monocle.Renderer) this.complete);
          this.addedRenderer = true;
        }
        base.Update();
        if (!Input.MenuConfirm.Pressed && !this.slideFinished || this.ending || !this.ready)
          return;
        this.ending = true;
        MountainWipe mountainWipe = new MountainWipe((Scene) this, false, (Action) (() => Engine.Scene = (Scene) new LevelLoader(this.session)));
      }

      public override void End()
      {
        base.End();
        if (this.complete == null)
          return;
        this.complete.Dispose();
      }
    }
}
