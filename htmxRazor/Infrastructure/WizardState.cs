namespace htmxRazor.Infrastructure;

/// <summary>
/// Tracks the state of a multi-step wizard, including the current step,
/// total steps, and which steps have been completed.
/// </summary>
public class WizardState
{
    /// <summary>
    /// The current step number (1-based).
    /// </summary>
    public int CurrentStep { get; set; } = 1;

    /// <summary>
    /// The total number of steps in the wizard.
    /// </summary>
    public int TotalSteps { get; set; }

    /// <summary>
    /// Tracks which steps have been completed. Key is the step number (1-based).
    /// </summary>
    public Dictionary<int, bool> CompletedSteps { get; set; } = new();

    /// <summary>
    /// Whether the current step is the first step.
    /// </summary>
    public bool IsFirstStep => CurrentStep == 1;

    /// <summary>
    /// Whether the current step is the last step.
    /// </summary>
    public bool IsLastStep => CurrentStep == TotalSteps;

    /// <summary>
    /// Marks a step as completed.
    /// </summary>
    /// <param name="step">The step number (1-based).</param>
    public void MarkComplete(int step) => CompletedSteps[step] = true;

    /// <summary>
    /// Returns whether a step has been completed.
    /// </summary>
    /// <param name="step">The step number (1-based).</param>
    public bool IsComplete(int step) => CompletedSteps.TryGetValue(step, out var complete) && complete;
}
