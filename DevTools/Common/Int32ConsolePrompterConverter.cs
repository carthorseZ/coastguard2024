using DevTools.Properties;

namespace DevTools
{
    public class Int32ConsolePrompterConverter : ConsolePrompterConverter<int>
    {
        public override ConsolePrompterConversionResult<int> Convert(string input)
        {
            int num = 0;
            bool flag = int.TryParse(input, out num);
            var result = new ConsolePrompterConversionResult<int>();
            if (flag)
            {
                result.ConvertedInput = num;
                result.IsConverted = true;
                return result;
            }
            result.ErrorMessage = Resources.Int32ConsolePrompterConverterInvalidInput;
            result.IsConverted = false;
            return result;
        }
    }
}