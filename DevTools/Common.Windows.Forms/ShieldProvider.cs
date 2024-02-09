namespace DevTools.Windows.Forms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ProvideProperty("RequiresUserAccessControl", typeof(Button))]
    public class ShieldProvider : Component, IExtenderProvider
    {
        private IContainer components;
        private Dictionary<Button, bool> m_RequiresUserAccessControlLookup;

        public ShieldProvider()
        {
            this.components = null;
            this.m_RequiresUserAccessControlLookup = new Dictionary<Button, bool>();
            this.InitializeComponent();
        }

        public ShieldProvider(IContainer container)
        {
            this.components = null;
            this.m_RequiresUserAccessControlLookup = new Dictionary<Button, bool>();
            container.Add(this);
            this.InitializeComponent();
        }

        public bool CanExtend(object extendee)
        {
            return (extendee is Button);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        [Category("Apperance")]
        public bool GetRequiresUserAccessControl(Button extendee)
        {
            if (this.m_RequiresUserAccessControlLookup.ContainsKey(extendee))
            {
                return this.m_RequiresUserAccessControlLookup[extendee];
            }
            return false;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        public void SetRequiresUserAccessControl(Button extendee, bool requiresUserAccessControl)
        {
            this.m_RequiresUserAccessControlLookup[extendee] = requiresUserAccessControl;
            if (requiresUserAccessControl)
            {
                extendee.FlatStyle = FlatStyle.System;
            }
            HandleRef hWnd = new HandleRef(extendee, extendee.Handle);
            NativeMethods.SendMessage(hWnd, 0x160c, IntPtr.Zero, requiresUserAccessControl);
        }
    }
}

