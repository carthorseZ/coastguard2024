namespace DevTools.Windows.Forms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ProvideProperty("Cue", typeof(TextBox))]
    public class CueProvider : Component, IExtenderProvider
    {
        private IContainer components;
        private Dictionary<TextBox, string> m_CueLookup;

        public CueProvider()
        {
            this.components = null;
            this.m_CueLookup = new Dictionary<TextBox, string>();
            this.InitializeComponent();
        }

        public CueProvider(IContainer container)
        {
            this.components = null;
            this.m_CueLookup = new Dictionary<TextBox, string>();
            container.Add(this);
            this.InitializeComponent();
        }

        public bool CanExtend(object extendee)
        {
            return (extendee is TextBox);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        [Category("Appearance")]
        public string GetCue(TextBox extendee)
        {
            if (!this.m_CueLookup.ContainsKey(extendee))
            {
                return null;
            }
            return this.m_CueLookup[extendee];
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        public void SetCue(TextBox extendee, string cue)
        {
            if (!(base.DesignMode || string.IsNullOrEmpty(cue)))
            {
                HandleRef hWnd = new HandleRef(extendee, extendee.Handle);
                NativeMethods.SendMessage(hWnd, 0x1501, IntPtr.Zero, cue);
            }
            this.m_CueLookup[extendee] = cue;
        }
    }
}

