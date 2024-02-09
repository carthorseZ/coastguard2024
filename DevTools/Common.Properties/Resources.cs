namespace DevTools.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    internal class Resources
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resources()
        {
        }

        internal static string ConsolePrompterInvalidInput
        {
            get
            {
                return ResourceManager.GetString("ConsolePrompterInvalidInput", resourceCulture);
            }
        }

        internal static string ConsolePrompterNoConversion
        {
            get
            {
                return ResourceManager.GetString("ConsolePrompterNoConversion", resourceCulture);
            }
        }

        internal static string ConsolePrompterNoValidation
        {
            get
            {
                return ResourceManager.GetString("ConsolePrompterNoValidation", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        internal static string Int32ConsolePrompterConverterInvalidInput
        {
            get
            {
                return ResourceManager.GetString("Int32ConsolePrompterConverterInvalidInput", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("Readify.Useful.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static string StringConsolePrompterConverterInvalidInput
        {
            get
            {
                return ResourceManager.GetString("StringConsolePrompterConverterInvalidInput", resourceCulture);
            }
        }
    }
}

