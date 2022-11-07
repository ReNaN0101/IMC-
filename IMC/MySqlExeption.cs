using System;
using System.Runtime.Serialization;

namespace IMC
{
    [Serializable]
    internal class MySqlExeption : Exception
    {
        public MySqlExeption()
        {
        }

        public MySqlExeption(string message) : base(message)
        {
        }

        public MySqlExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MySqlExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string Number { get; internal set; }
    }
}