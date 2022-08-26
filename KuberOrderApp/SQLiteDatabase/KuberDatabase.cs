using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KuberOrderApp.Models.RequestModels;
using KuberOrderApp.Models.ResponseModels;
using SQLite;

namespace KuberOrderApp.SQLiteDatabase
{
    public class KuberDatabase
    {
        #region ReadOnly Section
        private readonly SQLiteAsyncConnection database;
        #endregion

        #region Constructor
        public KuberDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<ProductList>().Wait();
            database.CreateTableAsync<PartyList>().Wait();
            database.CreateTableAsync<AddPaymentAndReceiptRequest>().Wait();
            database.CreateTableAsync<SaveProductOrder>().Wait();
            database.CreateTableAsync<ProductCart>().Wait();
            database.CreateTableAsync<SettingResponse>().Wait();
        }

        #endregion

        #region Drop Tables and Create New Tables
        async public Task DropTableAndCreateNew()
        {
            if (database != null)
            {
                await database.DropTableAsync<ProductList>();
                await database.DropTableAsync<PartyList>();
                await database.DropTableAsync<AddPaymentAndReceiptRequest>();
                await database.DropTableAsync<SaveProductOrder>();
                await database.DropTableAsync<ProductCart>();
                await database.DropTableAsync<SettingResponse>();
                database.CreateTableAsync<SettingResponse>().Wait();
                database.CreateTableAsync<ProductList>().Wait();
                database.CreateTableAsync<PartyList>().Wait();
                database.CreateTableAsync<AddPaymentAndReceiptRequest>().Wait();
                database.CreateTableAsync<SaveProductOrder>().Wait();
                database.CreateTableAsync<ProductCart>().Wait();
            }
        }
        #endregion

        #region Insert, Delete and Get PartyList
        async public Task Insert<T>(List<T> dataList)
        {
            await database.RunInTransactionAsync(async obj =>
            {
                obj.InsertAll(dataList);
            });
        }
        async public Task<List<T>> GetDataList<T>() where T : new()
        {
            return await database.Table<T>().ToListAsync();
        }
        async public Task DropTableByName<T>() where T : new()
        {
            await database.DropTableAsync<T>();
        }

        async public Task<List<T>> GetOrderDataList<T>(int offset) where T : new()
        {
            return await database.QueryAsync<T>($"SELECT * FROM ProductList LIMIT 10 OFFSET {offset}");
        }

        async public Task DeleteData<T>(T deleteList) where T : new()
        {
            await database.RunInTransactionAsync(obj =>
            {
                obj.Delete(deleteList);
            });
        }

        async public Task UpdateData<T>(T deleteList) where T : new()
        {
            await database.RunInTransactionAsync(obj =>
            {
                obj.Update(deleteList);
            });
        }
        #endregion

        async public Task<int> GetCount<T>() where T : new()
        {
            return await database.Table<T>().CountAsync();
        }

        async public Task<ProductCart> GetCartProduct(string colPK)
        {
            return await database.Table<ProductCart>().Where(x=>x.ColPK == colPK).FirstOrDefaultAsync();
        }

        async public Task<List<ProductList>> GetGroupProduct(string groupName)
        {
            return await database.Table<ProductList>().Where(x => x.ColGrpName == groupName).ToListAsync();
        }

        async public Task<List<ProductList>> GetCategoryProduct(string categoryName)
        {
            return await database.Table<ProductList>().Where(x => x.ColCatName == categoryName).ToListAsync();
        }

        async public Task<List<ProductList>> GetTypeProduct(string typeName)
        {
            return await database.Table<ProductList>().Where(x => x.ColTypeName == typeName).ToListAsync();
        }

        async public Task<List<ProductList>> GetMasterProduct(string masterName)
        {
            return await database.Table<ProductList>().Where(x => x.ColMastersName == masterName).ToListAsync();
        }


        #region Insert and Get Payment
        //async public Task InsertPaymentList(List<AddPaymentAndReceiptRequest> paymentAndReceiptRequests)
        //{
        //    await database.RunInTransactionAsync(async obj =>
        //    {
        //        obj.InsertAll(paymentAndReceiptRequests);
        //    });
        //}

        //async public Task<List<AddPaymentAndReceiptRequest>> GetPaymentList()
        //{
        //    return await database.Table<AddPaymentAndReceiptRequest>().ToListAsync();
        //}
        #endregion
    }
}
