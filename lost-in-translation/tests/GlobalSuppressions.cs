// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("CodeQuality", "IDE0051:Remove unused private members",
    Justification = "Private test methods are used implicitly",
    Scope = "namespaceanddescendants",
    Target = "~N:tests")]

[assembly: SuppressMessage("Readability", "RCS1018:Add accessibility modifiers (or vice versa).",
    Justification = "Avoid noisy public modifier",
    Scope = "namespaceanddescendants",
    Target = "~N:tests")]
