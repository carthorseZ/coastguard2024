using System;
using DevTools.Properties;

namespace DevTools
{
    public static class ConsolePrompter
    {
        public static TInput Prompt<TInput, TConverter, TValidator>(string promptText)
            where TConverter : ConsolePrompterConverter<TInput>, new()
            where TValidator : ConsolePrompterValidator<TInput>, new()
        {
            return Prompt<TInput, TConverter, TValidator>(promptText, default(TInput), ConsolePrompterLineStyle.SameLine,
                                                          ConsolePrompterValidationStyle.Repeat);
        }

        public static TInput Prompt<TInput, TConverter, TValidator>(string promptText, TInput defaultInput)
            where TConverter : ConsolePrompterConverter<TInput>, new()
            where TValidator : ConsolePrompterValidator<TInput>, new()
        {
            return Prompt<TInput, TConverter, TValidator>(promptText, defaultInput, ConsolePrompterLineStyle.SameLine,
                                                          ConsolePrompterValidationStyle.ReturnDefault);
        }

        public static TInput Prompt<TInput, TConverter, TValidator>(string promptText,
                                                                    ConsolePrompterLineStyle lineStyle)
            where TConverter : ConsolePrompterConverter<TInput>, new()
            where TValidator : ConsolePrompterValidator<TInput>, new()
        {
            return Prompt<TInput, TConverter, TValidator>(promptText, default(TInput), lineStyle,
                                                          ConsolePrompterValidationStyle.ReturnDefault);
        }

        public static TInput Prompt<TInput, TConverter, TValidator>(string promptText,
                                                                    ConsolePrompterValidationStyle validationStyle)
            where TConverter : ConsolePrompterConverter<TInput>, new()
            where TValidator : ConsolePrompterValidator<TInput>, new()
        {
            return Prompt<TInput, TConverter, TValidator>(promptText, default(TInput), ConsolePrompterLineStyle.SameLine,
                                                          validationStyle);
        }

        public static TInput Prompt<TInput, TConverter, TValidator>(string promptText, TInput defaultInput,
                                                                    ConsolePrompterLineStyle lineStyle)
            where TConverter : ConsolePrompterConverter<TInput>, new()
            where TValidator : ConsolePrompterValidator<TInput>, new()
        {
            return Prompt<TInput, TConverter, TValidator>(promptText, defaultInput, lineStyle,
                                                          ConsolePrompterValidationStyle.ReturnDefault);
        }

        public static TInput Prompt<TInput, TConverter, TValidator>(string promptText,
                                                                    ConsolePrompterLineStyle lineStyle,
                                                                    ConsolePrompterValidationStyle validationStyle)
            where TConverter : ConsolePrompterConverter<TInput>, new()
            where TValidator : ConsolePrompterValidator<TInput>, new()
        {
            return Prompt<TInput, TConverter, TValidator>(promptText, default(TInput), lineStyle, validationStyle);
        }

        public static TInput Prompt<TInput, TConverter, TValidator>(string promptText, TInput defaultInput,
                                                                    ConsolePrompterLineStyle lineStyle,
                                                                    ConsolePrompterValidationStyle validationStyle)
            where TConverter : ConsolePrompterConverter<TInput>, new()
            where TValidator : ConsolePrompterValidator<TInput>, new()
        {
            var local = Activator.CreateInstance<TConverter>();
            var local2 = Activator.CreateInstance<TValidator>();
            string str = null;
            var result = new ConsolePrompterConversionResult<TInput>();
            result.ErrorMessage = Resources.ConsolePrompterNoConversion;
            var result2 = new ConsolePrompterValidationResult<TInput>();
            result2.ErrorMessage = Resources.ConsolePrompterNoValidation;
            do
            {
                str = WritePromptAndReadInput(promptText, lineStyle);
                if (!string.IsNullOrEmpty(str))
                {
                    result = local.Convert(str);
                }
                if (result.IsConverted)
                {
                    result2 = local2.Validate(result.ConvertedInput);
                    if (!result2.IsValid)
                    {
                        Console.WriteLine("Error: {0}", result2.ErrorMessage);
                    }
                }
                else if (validationStyle != ConsolePrompterValidationStyle.ReturnDefault)
                {
                    Console.WriteLine("Error: {0}", result.ErrorMessage);
                }
            } while ((validationStyle == ConsolePrompterValidationStyle.Repeat) && !result2.IsValid);
            if (!(result2.IsValid || (validationStyle != ConsolePrompterValidationStyle.OnceOnly)))
            {
                throw new ConsolePrompterException(Resources.ConsolePrompterInvalidInput, str, result.ConvertedInput);
            }
            if (!(result2.IsValid || (validationStyle != ConsolePrompterValidationStyle.ReturnDefault)))
            {
                return defaultInput;
            }
            return result.ConvertedInput;
        }

        public static int PromptForInt32(string promptText)
        {
            return Prompt<int, Int32ConsolePrompterConverter, DefaultConsolePrompterValidator<int>>(promptText, 0,
                                                                                                    ConsolePrompterLineStyle
                                                                                                        .SameLine,
                                                                                                    ConsolePrompterValidationStyle
                                                                                                        .Repeat);
        }

        public static int PromptForInt32<TValidator>(string promptText)
            where TValidator : ConsolePrompterValidator<int>, new()
        {
            return Prompt<int, Int32ConsolePrompterConverter, TValidator>(promptText, 0,
                                                                          ConsolePrompterLineStyle.SameLine,
                                                                          ConsolePrompterValidationStyle.Repeat);
        }

        public static int PromptForInt32(string promptText, int defaultInput)
        {
            return Prompt<int, Int32ConsolePrompterConverter, DefaultConsolePrompterValidator<int>>(promptText,
                                                                                                    defaultInput,
                                                                                                    ConsolePrompterLineStyle
                                                                                                        .SameLine,
                                                                                                    ConsolePrompterValidationStyle
                                                                                                        .ReturnDefault);
        }

        public static int PromptForInt32<TValidator>(string promptText, int defaultInput)
            where TValidator : ConsolePrompterValidator<int>, new()
        {
            return Prompt<int, Int32ConsolePrompterConverter, TValidator>(promptText, defaultInput,
                                                                          ConsolePrompterLineStyle.SameLine,
                                                                          ConsolePrompterValidationStyle.ReturnDefault);
        }

        public static string PromptForString(string promptText)
        {
            return Prompt<string, StringConsolePrompterConverter, DefaultConsolePrompterValidator<string>>(promptText,
                                                                                                           null,
                                                                                                           ConsolePrompterLineStyle
                                                                                                               .SameLine,
                                                                                                           ConsolePrompterValidationStyle
                                                                                                               .Repeat);
        }

        public static string PromptForString<TValidator>(string promptText)
            where TValidator : ConsolePrompterValidator<string>, new()
        {
            return Prompt<string, StringConsolePrompterConverter, TValidator>(promptText, null,
                                                                              ConsolePrompterLineStyle.SameLine,
                                                                              ConsolePrompterValidationStyle.Repeat);
        }

        public static string PromptForString(string promptText, string defaultInput)
        {
            return Prompt<string, StringConsolePrompterConverter, DefaultConsolePrompterValidator<string>>(promptText,
                                                                                                           defaultInput,
                                                                                                           ConsolePrompterLineStyle
                                                                                                               .SameLine,
                                                                                                           ConsolePrompterValidationStyle
                                                                                                               .
                                                                                                               ReturnDefault);
        }

        public static string PromptForString<TValidator>(string promptText, string defaultInput)
            where TValidator : ConsolePrompterValidator<string>, new()
        {
            return Prompt<string, StringConsolePrompterConverter, TValidator>(promptText, defaultInput,
                                                                              ConsolePrompterLineStyle.SameLine,
                                                                              ConsolePrompterValidationStyle.
                                                                                  ReturnDefault);
        }

        private static string WritePromptAndReadInput(string promptText, ConsolePrompterLineStyle lineStyle)
        {
            if (lineStyle == ConsolePrompterLineStyle.LineBelow)
            {
                Console.WriteLine(promptText);
            }
            else
            {
                Console.Write(promptText);
            }
            return Console.ReadLine();
        }
    }
}