using System;
using System.Collections.Generic;

namespace StockDB.Interfaces
{
    public interface IGuidDatabaseOperation<T>
    {
        Guid SaveRecord(T record);
        bool UpdateRecord(T record);
        T GetRecordById(Guid id);
        List<T> GetAllRecords();
        bool DeleteRecord(T record);
        bool DeleteRecordById(Guid id);

    }
}
