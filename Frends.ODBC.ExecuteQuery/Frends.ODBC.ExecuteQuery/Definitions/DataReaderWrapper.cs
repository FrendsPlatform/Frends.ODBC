using System;
using System.Data.Odbc;

namespace Frends.ODBC.ExecuteQuery.Definitions
{
    /// <summary>
    /// Wrapper for OdbcDataReader to avoid assembly reference issues.
    /// </summary>
    public class DataReaderWrapper : IDisposable
    {
        private readonly OdbcDataReader _reader;

        internal DataReaderWrapper(OdbcDataReader reader)
        {
            _reader = reader;
        }

        /// <summary>
        /// Advances the reader to the next record.
        /// </summary>
        public bool Read()
        {
            return _reader.Read();
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        public object GetValue(int ordinal)
        {
            return _reader.GetValue(ordinal);
        }

        /// <summary>
        /// Populates an array of objects with the column values of the current row.
        /// </summary>
        public int GetValues(object[] values)
        {
            return _reader.GetValues(values);
        }

        /// <summary>
        /// Gets the name of the column at the specified ordinal.
        /// </summary>
        public string GetName(int ordinal)
        {
            return _reader.GetName(ordinal);
        }

        /// <summary>
        /// Gets a value indicating whether the specified column contains a null value.
        /// </summary>
        public bool IsDBNull(int ordinal)
        {
            return _reader.IsDBNull(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column by ordinal.
        /// </summary>
        public object this[int ordinal] => _reader[ordinal];

        /// <summary>
        /// Gets the value of the specified column by name.
        /// </summary>
        public object this[string name] => _reader[name];

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        public int FieldCount => _reader.FieldCount;

        /// <summary>
        /// Releases all resources used by the DataReader.
        /// </summary>
        public void Dispose()
        {
            _reader?.Dispose();
        }
    }

}
