using System;

namespace thibeastmo.Json
{
    [Serializable]
    public class JsonNotFoundException : Exception
    {
        private string message = "Json not found!";
        public JsonNotFoundException() { }

        public JsonNotFoundException(string message)
            : base(message) { }

        public JsonNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}
