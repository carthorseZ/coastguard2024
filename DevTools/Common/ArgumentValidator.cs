namespace DevTools
{
    using System;

    public static class ArgumentValidator
    {
        public static void ValidateArgumentIsNotNull(string parameterName, object parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentException(parameterName);
            }
        }
    }
}

