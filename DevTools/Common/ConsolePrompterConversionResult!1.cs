namespace DevTools
{
    using System;

    public class ConsolePrompterConversionResult<T>
    {
        private T m_ConvertedInput;
        private string m_ErrorMessage;
        private bool m_IsConverted;

        public T ConvertedInput
        {
            get
            {
                return this.m_ConvertedInput;
            }
            set
            {
                this.m_ConvertedInput = value;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return this.m_ErrorMessage;
            }
            set
            {
                this.m_ErrorMessage = value;
            }
        }

        public bool IsConverted
        {
            get
            {
                return this.m_IsConverted;
            }
            set
            {
                this.m_IsConverted = value;
            }
        }
    }
}

