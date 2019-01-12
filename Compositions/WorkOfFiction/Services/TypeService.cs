
using System.Collections.Generic;
using System.Data.OracleClient;
using WorkOfFiction.Enums;
using WorkOfFiction.Helpers;
using WorkOfFiction.Models;
using static WorkOfFiction.Helpers.DbDictionaries;

namespace WorkOfFiction.Services
{
    public class TypeService
    {
        private readonly OracleHelper _oracleHelper;

        public TypeService(OracleHelper oracleHelper)
        {
            _oracleHelper = oracleHelper;
        }

        public bool CheckIfAlreadyExist(Type type)
        {
            return _oracleHelper.CheckIfAlreadyExist(TableName.Types, "name", type.Name);
        }

        public void Insert(Type type)
        {
            _oracleHelper.Insert(TableName.Types, type.ToStringExtension(false));
        }

        public void Update(Type type)
        {
            _oracleHelper.Update(TableName.Types, type.Id, type.ToStringExtension());
        }

        public void Delete(int typeId)
        {
            _oracleHelper.Delete(TableName.Types, typeId);
        }

        public IEnumerable<Type> GetAllTypes()
        {
            var queryString = $"select * from {Tables[TableName.Types]}";
            var types = new List<Type>();

            using (var connection = new OracleConnection(_oracleHelper.Connection))
            {
                var command = new OracleCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        types.Add(new Type
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }

            return types;
        }

        public Type GetType(int? id)
        {
            if (id.HasValue)
            {
                var queryString = $"select name from kudriavtseva_types where type_id = {id}";
                var type = new Type();

                using (var connection = new OracleConnection(_oracleHelper.Connection))
                {
                    var command = new OracleCommand(queryString, connection);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            type.Id = id.Value;
                            type.Name = reader.GetString(0);
                        }
                    }
                }

                return type;
            }

            return null;
        }

    }
}