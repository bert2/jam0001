namespace foobar {
    using System;

    public class RuntimeError : Exception {
        private RuntimeError() { }

        public RuntimeError(string message) : base(message) { }

        public RuntimeError(string message, Exception innerException) : base(message, innerException) { }
    }
}
