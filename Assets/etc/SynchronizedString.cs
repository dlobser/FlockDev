using UnityEngine;
using Holojam.Network;

public class SynchronizedString : Holojam.Tools.Synchronizable{

  public string label;

   public override void ResetData() {
    data = new Flake(0, 0, 0, 0, 0, true);
   }

   public override string Label { get { return label; } }

   public override bool Host { get { return false; } }
   public override bool AutoHost { get { return true; } }

   protected override void Sync() { }

   public string GetText() { return data.text; }
   public void SetText(string t) { data.text = t; }
}
