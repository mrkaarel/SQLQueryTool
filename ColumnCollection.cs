using System;
using System.Collections.Generic;
using System.Text;

namespace SqlQueryTool
{
    public class ColumnCollection : IEnumerable<Column>
    {
        private List<Column> columns;

        public ColumnCollection()
        {
            columns = new List<Column>();
        }

        public void Add(Column column)
        {
            columns.Add(column);
        }

        public Column this[string name]
        { 
            get {
                foreach (Column column in columns) {
                    if (column.Name == name) {
                        return column;
                    }
                }

                return null;
            }
            set {
                bool columnExists = false;
                for (int i = 0; i < columns.Count; i++) {
                    if (columns[i].Name == name) {
                        columns[i] = value;
                        columnExists = true;
                    }                
                }
                if (!columnExists) {
                    Add(value);
                }
               
            }
        }

        public Column this[int index]
        {
            get {
                return columns[index];
            }
            set {
                columns[index] = value;
            }
        }

        public Column IdentityColumn
        {
            get {
                foreach (Column col in this.columns) {
                    if (col.IsIdentity) {
                        return col;
                    }
                }

                return null;
            }
        }

        #region IEnumerable<Column> Members

        public IEnumerator<Column> GetEnumerator()
        {
            return columns.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return columns.GetEnumerator();
        }

        #endregion
    }
}
