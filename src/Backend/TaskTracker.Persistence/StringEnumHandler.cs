using Dapper;
using System;
using System.Data;

namespace TaskTracker.Persistence;

public class StringEnumHandler<T> : SqlMapper.TypeHandler<T>
{
    public override void SetValue(IDbDataParameter parameter, T value)
    {
        parameter.Value = value.ToString();
        parameter.DbType = DbType.String;
    }

    public override T Parse(object value)
    {
        return (T)Enum.Parse(typeof(T), value.ToString(), true);
    }
}
