// Decompiled with JetBrains decompiler
// Type: Celeste.OuiOptions
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Microsoft.Xna.Framework;
using Monocle;
using System.Collections;

namespace Celeste
{

    public class OuiOptions : Oui
    {
      private TextMenu menu;
      private const float onScreenX = 960f;
      private const float offScreenX = 2880f;
      private string startLanguage;
      private string currentLanguage;
      private float alpha;

      public override void Added(Scene scene) => base.Added(scene);

      private void ReloadMenu()
      {
        Vector2 vector2 = Vector2.Zero;
        int num = -1;
        if (this.menu != null)
        {
          vector2 = this.menu.Position;
          num = this.menu.Selection;
          this.Scene.Remove((Entity) this.menu);
        }
        this.menu = MenuOptions.Create();
        if (num >= 0)
        {
          this.menu.Selection = num;
          this.menu.Position = vector2;
        }
        this.Scene.Add((Entity) this.menu);
      }

      public override IEnumerator Enter(Oui from)
      {
        OuiOptions ouiOptions = this;
        ouiOptions.ReloadMenu();
        ouiOptions.menu.Visible = ouiOptions.Visible = true;
        ouiOptions.menu.Focused = false;
        ouiOptions.currentLanguage = ouiOptions.startLanguage = Settings.Instance.Language;
        for (float p = 0.0f; (double) p < 1.0; p += Engine.DeltaTime * 4f)
        {
          ouiOptions.menu.X = (float) (2880.0 + -1920.0 * (double) Ease.CubeOut(p));
          ouiOptions.alpha = Ease.CubeOut(p);
          yield return (object) null;
        }
        ouiOptions.menu.Focused = true;
      }

      public override IEnumerator Leave(Oui next)
      {
        OuiOptions ouiOptions = this;
        Audio.Play("event:/ui/main/whoosh_large_out");
        ouiOptions.menu.Focused = false;
        UserIO.SaveHandler(false, true);
        while (UserIO.Saving)
          yield return (object) null;
        for (float p = 0.0f; (double) p < 1.0; p += Engine.DeltaTime * 4f)
        {
          ouiOptions.menu.X = (float) (960.0 + 1920.0 * (double) Ease.CubeIn(p));
          ouiOptions.alpha = 1f - Ease.CubeIn(p);
          yield return (object) null;
        }
        if (ouiOptions.startLanguage != Settings.Instance.Language)
        {
          ouiOptions.Overworld.ReloadMenus(Overworld.StartMode.ReturnFromOptions);
          yield return (object) null;
        }
        ouiOptions.menu.Visible = ouiOptions.Visible = false;
        ouiOptions.menu.RemoveSelf();
        ouiOptions.menu = (TextMenu) null;
      }

      public override void Update()
      {
        if (this.menu != null && this.menu.Focused && this.Selected && Input.MenuCancel.Pressed)
        {
          Audio.Play("event:/ui/main/button_back");
          this.Overworld.Goto<OuiMainMenu>();
        }
        if (this.Selected && this.currentLanguage != Settings.Instance.Language)
        {
          this.currentLanguage = Settings.Instance.Language;
          this.ReloadMenu();
        }
        base.Update();
      }

      public override void Render()
      {
        if ((double) this.alpha > 0.0)
          Draw.Rect(-10f, -10f, 1940f, 1100f, Color.Black * this.alpha * 0.4f);
        base.Render();
      }
    }
}
