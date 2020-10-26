using System.Collections.Generic;

namespace StockDB.Interfaces
{
    public interface IDatabaseOperation<T>
    {
        long SaveRecord(T record);
        bool UpdateRecord(T record);
        T GetRecordById(long id);
        List<T> GetAllRecords();
        bool DeleteRecord(T record);
        bool DeleteRecordById(long id);
    }
}
