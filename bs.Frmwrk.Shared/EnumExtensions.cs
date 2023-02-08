using bs.Frmwrk.Core.Exceptions;
using bs.Frmwrk.Core.ViewModels.Common;
using System.ComponentModel;

namespace bs.Frmwrk.Shared
{
    public static class EnumExtensions
    {
        public static IList<ISelectListItem> ToListItemViewModel<T>() where T : Enum
        {
            var result = new List<ISelectListItem>();

            foreach (var enumValue in Enum.GetValues(typeof(T)))
            {
                result.Add(new SelectListItem(enumValue?.ToString() ?? "", enumValue?.GetAttributeOfMember<DescriptionAttribute>()?.Description ?? enumValue?.ToString() ?? ""));
            }

            return result;
        }

        public static string ToDescription<T>(this T value) where T : Enum
        {
            return value.GetAttributeOfMember<DescriptionAttribute>()?.Description ?? value.ToString();
        }

        /// <summary>
        /// Converts to specified enum value an integer value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="ServiceException">2008251131 - Impossibile convertire il valore intero nell'enumeratore. Il valore non è valido!</exception>
        public static T ToEnum<T>(this int value) where T : Enum
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                return (T)Enum.ToObject(typeof(T), value);
            }
            throw new BsException(2212200857, $"Impossibile convertire il valore intero nell'enumeratore (valore non definito '{value}')");
        }

        public static T ToEnum<T>(this string value) where T : Enum
        {
            if (int.TryParse(value, out var result))
            {
                return result.ToEnum<T>();
            }
            throw new BsException(2212200858, $"Impossibile convertire il valore stringa in intero (valore '{value}')");
        }
    }
}