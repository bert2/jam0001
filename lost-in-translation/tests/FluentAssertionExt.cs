namespace tests {
    using System;
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Collections;
    using FluentAssertions.Primitives;

    using foobar;

    using Microsoft.FSharp.Collections;

    public static class FluentAssertionExt {
        public static GenericCollectionAssertions<T> Should<T>(this FSharpList<T> xs) => ((IEnumerable<T>)xs).Should();

        public static AndConstraint<ObjectAssertions> Satisfy<T>(this ObjectAssertions parent, Action<T> inspector) {
            inspector((T)parent.Subject);
            return new AndConstraint<ObjectAssertions>(parent);
        }
    }
}
