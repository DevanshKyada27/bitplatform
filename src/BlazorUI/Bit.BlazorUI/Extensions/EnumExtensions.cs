﻿using System.Reflection;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

[assembly: InternalsVisibleTo("Bit.BlazorUI.Demo.Client.Core")]
namespace Bit.BlazorUI;

internal static class EnumExtensions
{
    internal static string? GetDisplayName(this Enum enumValue, bool showNameIfHasNoDisplayName = true, bool toLowerDisplayName = false)
    {
        if (enumValue is null) return null;

        var name = enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .FirstOrDefault()?
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetName();

        string? displayName = null;

        if (name.HasValue())
        {
            displayName = name!;
        }
        else if (showNameIfHasNoDisplayName)
        {
            displayName = enumValue.ToString();
        }

        if (displayName.HasValue() && toLowerDisplayName)
        {
            displayName = displayName!.ToLower(CultureInfo.CurrentUICulture);
        }

        return displayName;
    }
}
