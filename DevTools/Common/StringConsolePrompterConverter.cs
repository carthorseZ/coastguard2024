using DevTools.Properties;

namespace DevTools
{
    public class StringConsolePrompterConverter : ConsolePrompterConverter<string>
    {
        public override ConsolePrompterConversionResult<string> Convert(string input)
        {
            var result = new ConsolePrompterConversionResult<string>();
            if (string.IsNullOrEmpty(input))
            {
                result.IsConverted = false;
                result.ErrorMessage = Resources.StringConsolePrompterConverterInvalidInput;
                return result;
            }
            result.IsConverted = true;
            result.ConvertedInput = input;
            return result;
        }
    }
}