using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterDetails
{
    public static class SessionHelper
    {
        public static TableBag<T> GetTableBag<T>(this HttpSessionStateBase session, Guid guid, IEnumerable<T> list)
        {

            string key = guid.ToString();
            if (!(session[key] is TableBag<T>))
                session[key] = new TableBag<T>(list);

            return session[key] as TableBag<T>;
        }
        public static TableBag<T> GetTableBag<T>(this HttpSessionStateBase session, Guid key)
        {
            return session[key.ToString()] as TableBag<T>;
        }

        public static TableBag<T, U> GetTableBag<T, U>(this HttpSessionStateBase session, Guid guid, IEnumerable<T> list, IEnumerable<U> detailsList, string parentName = "ParentID")
        {
            string key = guid.ToString();
            if (!(session[key] is TableBag<T, U>))
                session[key] = new TableBag<T, U>(list, detailsList, parentName);

            return session[key] as TableBag<T, U>;
        }
        public static TableBag<T, U> GetTableBag<T, U>(this HttpSessionStateBase session, Guid key)
        {
            return session[key.ToString()] as TableBag<T, U>;
        }
    }

    #region Bag
    public class TableBag<T>
    {
        public int LastID { get; set; }
        private List<TableBagRow<T>> Rows { get; set; }
        private Dictionary<int, TableBagRow<T>> Dictionary { get; set; }
        public Dictionary<int, Guid> GuidDictionary { get; set; }

        public List<T> Items
        {
            get { return this.Rows.Where(o => o.Status != TableBagRowStatus.Deleted).Select(o => o.Item).ToList(); }
        }
        public List<T> Inserted
        {
            get { return this.Rows.Where(o => o.Status == TableBagRowStatus.New).Select(o => o.Item).ToList(); }
        }
        public List<T> Modified
        {
            get { return this.Rows.Where(o => o.Status == TableBagRowStatus.Modified).Select(o => o.Item).ToList(); }
        }
        public List<T> Deleted
        {
            get { return this.Rows.Where(o => o.Status == TableBagRowStatus.Deleted).Select(o => o.Item).ToList(); }
        }

        public T this[int id]
        {
            get { return this.Dictionary[id].Item; }
        }

        public TableBag(IEnumerable<T> list)
        {
            this.Rows = new List<TableBagRow<T>>();
            this.Dictionary = new Dictionary<int, TableBagRow<T>>();
            this.GuidDictionary = new Dictionary<int, Guid>();
            this.LastID = -1;


            foreach (var item in list)
            {
                TableBagRow<T> row = new TableBagRow<T>(item, TableBagRowStatus.Original);
                this.Rows.Add(row);
                this.Dictionary.Add(GetID(item), row);
                this.GuidDictionary.Add(GetID(item), this.GetGuid());
            }
        }

        private int GetID(T rec)
        {
            return ((dynamic)rec).ID;
        }
        private int SetNewID(T rec)
        {
            int id = ((dynamic)rec).ID != null && ((dynamic)rec).ID > 0 ? ((dynamic)rec).ID : this.LastID--;
            ((dynamic)rec).ID = id;

            return id;
        }
        private Guid GetGuid()
        {
            return Guid.NewGuid();
        }

        public void Insert(T rec)
        {
            int id = SetNewID(rec);
            TableBagRow<T> row = new TableBagRow<T>(rec, TableBagRowStatus.New);
            this.Rows.Add(row);
            this.Dictionary.Add(id, row);
            this.GuidDictionary.Add(id, this.GetGuid());
        }
        public void Insert(T rec, int id, TableBagRowStatus rowStatus = TableBagRowStatus.New)
        {
            TableBagRow<T> row = new TableBagRow<T>(rec, rowStatus);
            this.Rows.Add(row);
            this.Dictionary.Add(id, row);
            this.GuidDictionary.Add(id, this.GetGuid());
        }
        public void Update(T rec)
        {
            int id = GetID(rec);
            if (!this.Dictionary.ContainsKey(id))
                throw new Exception("Invalid record.");

            TableBagRow<T> row = this.Dictionary[id];
            row.Item = rec;
            if (row.Status == TableBagRowStatus.Original)
                row.Status = TableBagRowStatus.Modified;
        }
        public void Delete(int id)
        {
            if (!this.Dictionary.ContainsKey(id))
                throw new Exception("Invalid record.");

            TableBagRow<T> row = this.Dictionary[id];
            if (row.Status == TableBagRowStatus.Original || row.Status == TableBagRowStatus.Modified)
                row.Status = TableBagRowStatus.Deleted;

            if (row.Status == TableBagRowStatus.New)
            {
                this.Rows.Remove(row);
                this.Dictionary.Remove(id);
                this.GuidDictionary.Remove(id);
            }
        }
        public void ClearData()
        {
            this.Rows.Clear();
            this.Dictionary.Clear();
            this.GuidDictionary.Clear();
        }
    }

    class TableBagRow<T>
    {
        public T Item { get; set; }
        public TableBagRowStatus Status { get; set; }
        public TableBagRow(T item, TableBagRowStatus status)
        {
            this.Item = item;
            this.Status = status;
        }
    }
    #endregion

    #region Bag Details

    public class TableBag<T, U>
    {
        public int LastID { get; set; }
        private List<TableBagRow<T, U>> Rows { get; set; }
        private Dictionary<int, TableBagRow<T, U>> Dictionary { get; set; }
        public Dictionary<int, Guid> GuidDictionary { get; set; }

        public List<T> Items
        {
            get { return this.Rows.Where(o => o.Status != TableBagRowStatus.Deleted).Select(o => o.Item).ToList(); }
        }
        public List<T> Inserted
        {
            get { return this.Rows.Where(o => o.Status == TableBagRowStatus.New).Select(o => o.Item).ToList(); }
        }
        public List<T> Modified
        {
            get { return this.Rows.Where(o => o.Status == TableBagRowStatus.Modified).Select(o => o.Item).ToList(); }
        }
        public List<T> Deleted
        {
            get { return this.Rows.Where(o => o.Status == TableBagRowStatus.Deleted).Select(o => o.Item).ToList(); }
        }
        public T this[int id]
        {
            get { return this.Dictionary[id].Item; }
        }
        public List<U> DetailsList(int id)
        {
            return this.Dictionary[id].Bag.Items;
        }
        public TableBag<U> DetailsBag(int id)
        {
            return this.Dictionary[id].Bag;
        }

        public TableBag(IEnumerable<T> list, IEnumerable<U> detailsList, string parentObjectName = "ParentID")
        {
            this.Rows = new List<TableBagRow<T, U>>();
            this.Dictionary = new Dictionary<int, TableBagRow<T, U>>();
            this.GuidDictionary = new Dictionary<int, Guid>();
            this.LastID = -1;

            foreach (var item in list)
            {
                TableBagRow<T, U> row = new TableBagRow<T, U>(item, TableBagRowStatus.Original, detailsList.Where(o => Convert.ToInt32(o.GetType().GetProperty(parentObjectName).GetValue(o)) == ((dynamic)item).ID));
                this.Rows.Add(row);
                this.Dictionary.Add(GetID(item), row);
                this.GuidDictionary.Add(GetID(item), this.GetGuid());
            }
        }

        private int GetID(T rec)
        {
            return ((dynamic)rec).ID;
        }
        private int SetNewID(T rec)
        {
            int id = this.LastID--;
            ((dynamic)rec).ID = id;

            return id;
        }
        private Guid GetGuid()
        {
            return Guid.NewGuid();
        }

        public void Insert(T rec)
        {
            int id = SetNewID(rec);
            TableBagRow<T, U> row = new TableBagRow<T, U>(rec, TableBagRowStatus.New);
            this.Rows.Add(row);
            this.Dictionary.Add(id, row);
            this.GuidDictionary.Add(id, this.GetGuid());
        }
        public void Insert(T rec, IEnumerable<U> list)
        {
            int id = SetNewID(rec);
            TableBagRow<T, U> row = new TableBagRow<T, U>(rec, TableBagRowStatus.New, list);
            this.Rows.Add(row);
            this.Dictionary.Add(id, row);
            this.GuidDictionary.Add(id, this.GetGuid());
        }
        public void Update(T rec)
        {
            int id = GetID(rec);
            if (!this.Dictionary.ContainsKey(id))
                throw new Exception("Invalid record.");

            TableBagRow<T, U> row = this.Dictionary[id];
            row.Item = rec;
            if (row.Status == TableBagRowStatus.Original)
                row.Status = TableBagRowStatus.Modified;
        }
        public void Delete(int id)
        {
            if (!this.Dictionary.ContainsKey(id))
                throw new Exception("Invalid record.");

            TableBagRow<T, U> row = this.Dictionary[id];
            if (row.Status == TableBagRowStatus.Original || row.Status == TableBagRowStatus.Modified)
                row.Status = TableBagRowStatus.Deleted;

            if (row.Status == TableBagRowStatus.New)
            {
                this.Rows.Remove(row);
                this.Dictionary.Remove(id);
                this.GuidDictionary.Remove(id);
            }
        }
    }

    class TableBagRow<T, U>
    {
        public T Item { get; set; }
        public TableBagRowStatus Status { get; set; }
        public TableBag<U> Bag { get; set; }

        public TableBagRow(T item, TableBagRowStatus status)
        {
            this.Item = item;
            this.Status = status;
        }

        public TableBagRow(T item, TableBagRowStatus status, IEnumerable<U> detailsList)
        {
            this.Item = item;
            this.Status = status;
            this.Bag = new TableBag<U>(detailsList);
        }
    }

    #endregion

    public enum TableBagRowStatus
    {
        Original,
        New,
        Modified,
        Deleted
    }
}