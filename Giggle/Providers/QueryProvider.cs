using Giggle.Models.DTOs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Giggle.Providers
{
    public static class QueryProvider
    {
        public static string GenerateCreationQuery(Type type)
        {
            var tableAttr = type.GetCustomAttributes(typeof(TableAttribute), false)
                                .FirstOrDefault() as TableAttribute;
            var tableName = tableAttr?.Name ?? type.Name;

            var properties = type.GetProperties();
            var query = $"CREATE TABLE {tableName} (\n";
            List<string> foreignKeys = new List<string>();

            foreach (var prop in properties)
            {
                var column = prop.GetCustomAttributes(typeof(ColumnAttribute), false)
                                 .FirstOrDefault() as ColumnAttribute;
                var key = prop.GetCustomAttributes(typeof(KeyAttribute), false)
                              .FirstOrDefault() as KeyAttribute;
                var databaseGenerated = prop.GetCustomAttributes(typeof(DatabaseGeneratedAttribute), false)
                                            .FirstOrDefault() as DatabaseGeneratedAttribute;
                var foreignKey = prop.GetCustomAttributes(typeof(ForeignKeyAttribute), false)
                                      .FirstOrDefault() as ForeignKeyAttribute;

                var columnName = column?.Name ?? prop.Name;
                var columnType = column?.TypeName ?? "nvarchar(255)";

                if (key != null)
                {
                    if (databaseGenerated?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                        columnType += " IDENTITY(1,1)";
                }

                query += $"{columnName} {columnType} NOT NULL,\n";

                if (foreignKey != null)
                {
                    foreignKeys.Add($"FOREIGN KEY ({columnName}) REFERENCES {foreignKey.Name}({columnName})");
                }
            }

            query = query.TrimEnd('\n', ',') + "\n";
            if (foreignKeys.Any())
            {
                query += ",\n" + string.Join(",\n", foreignKeys);
            }

            query += ");";
            return query;
        }

    }
}
