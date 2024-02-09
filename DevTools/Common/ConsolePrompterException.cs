namespace DevTools
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public sealed class ConsolePrompterException : Exception, ISerializable
    {
        private object m_ConvertedInput;
        private string m_Input;

        public ConsolePrompterException()
        {
        }

        public ConsolePrompterException(string message) : base(message)
        {
        }

        private ConsolePrompterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.m_Input = info.GetString("Input");
            this.m_ConvertedInput = info.GetValue("ConvertedInput", typeof(object));
        }

        public ConsolePrompterException(string message, Exception inner) : base(message, inner)
        {
        }

        public ConsolePrompterException(string message, string input, object convertedInput) : base(message)
        {
            this.m_Input = input;
            this.m_ConvertedInput = convertedInput;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Input", this.Input);
            info.AddValue("ConvertedInput", this.ConvertedInput);
            base.GetObjectData(info, context);
        }

        public object ConvertedInput
        {
            get
            {
                return this.m_ConvertedInput;
            }
        }

        public string Input
        {
            get
            {
                return this.m_Input;
            }
        }
    }
}

