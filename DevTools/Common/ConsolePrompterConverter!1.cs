namespace DevTools
{
    using System;

    public abstract class ConsolePrompterConverter<TInput> : IDisposable
    {
        protected ConsolePrompterConverter()
        {
        }

        public abstract ConsolePrompterConversionResult<TInput> Convert(string input);
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        ~ConsolePrompterConverter()
        {
            this.Dispose(false);
        }
    }
}

