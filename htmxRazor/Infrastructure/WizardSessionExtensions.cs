using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace htmxRazor.Infrastructure;

/// <summary>
/// Extension methods for storing and retrieving <see cref="WizardState"/>
/// in ASP.NET Core TempData.
/// </summary>
public static class WizardSessionExtensions
{
    private const string KeyPrefix = "__WizardState_";

    /// <summary>
    /// Retrieves the wizard state for the given wizard ID from TempData.
    /// Returns a new default state if none exists.
    /// </summary>
    /// <param name="tempData">The TempData dictionary.</param>
    /// <param name="wizardId">A unique identifier for the wizard instance.</param>
    public static WizardState GetWizardState(this ITempDataDictionary tempData, string wizardId)
    {
        var key = KeyPrefix + wizardId;
        if (tempData.TryGetValue(key, out var value) && value is string json)
        {
            // Peek so the value persists across redirects
            tempData.Keep(key);
            return JsonSerializer.Deserialize<WizardState>(json) ?? new WizardState();
        }
        return new WizardState();
    }

    /// <summary>
    /// Stores the wizard state for the given wizard ID in TempData.
    /// </summary>
    /// <param name="tempData">The TempData dictionary.</param>
    /// <param name="wizardId">A unique identifier for the wizard instance.</param>
    /// <param name="state">The wizard state to persist.</param>
    public static void SetWizardState(this ITempDataDictionary tempData, string wizardId, WizardState state)
    {
        tempData[KeyPrefix + wizardId] = JsonSerializer.Serialize(state);
    }
}
