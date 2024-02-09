namespace DevTools
{
    using System;

    public class ConsolePrompterValidationResult<TInput>
    {
        private string m_ErrorMessage;
        private bool m_IsValid;

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

        public bool IsValid
        {
            get
            {
                return this.m_IsValid;
            }
            set
            {
                this.m_IsValid = value;
            }
        }
    }
}

