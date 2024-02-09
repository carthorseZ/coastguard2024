namespace DevTools
{
    using System;

    public abstract class ConsolePrompterValidator<TInput> : IDisposable
    {
        protected ConsolePrompterValidator()
        {
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        ~ConsolePrompterValidator()
        {
            this.Dispose(false);
        }

        public abstract ConsolePrompterValidationResult<TInput> Validate(TInput input);
    }
}

