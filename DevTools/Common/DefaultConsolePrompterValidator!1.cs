namespace DevTools
{
    public class DefaultConsolePrompterValidator<T> : ConsolePrompterValidator<T>
    {
        public override ConsolePrompterValidationResult<T> Validate(T input)
        {
            ConsolePrompterValidationResult<T> result = new ConsolePrompterValidationResult<T>();
            result.IsValid = true;
            return result;
        }
    }
}

