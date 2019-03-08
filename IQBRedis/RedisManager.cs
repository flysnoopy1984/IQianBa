using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.Model;
using Newtonsoft.Json;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameRedis
{
    public class RedisManager
    {
        #region Base
        private static string _RedisHost = ConfigurationManager.AppSettings["RedisHost"];
        private StackExchangeRedisCacheClient _RedisClient;
        private ConnectionMultiplexer _TransConn;
        private ITransaction _Trans = null;
        private ConnectionMultiplexer _RedisConn;
        private readonly object ConnLocker = new object();


        public RedisManager()
        {
          //  _RedisConn = ConnectionMultiplexer.Connect(_RedisHost);
           
        }

       
        public ConnectionMultiplexer NewConnection()
        {
           var conn =  ConnectionMultiplexer.Connect(_RedisHost);
            return conn;
        }

        public ConnectionMultiplexer StartTrans()
        {
            _TransConn = NewConnection();
            _Trans = _TransConn.GetDatabase().CreateTransaction();
            return _TransConn;
        }

        public bool EndTrans()
        {
            bool result = true;
            if(_Trans !=null)
            {
                result = _Trans.Execute();
                
            }
            _Trans = null;
            _TransConn = null;

            return result;
        }

        private ConnectionMultiplexer GetUsingConn()
        {
            ConnectionMultiplexer conn = _TransConn;
            if (conn == null)
            {
                if (_RedisConn == null || !_RedisConn.IsConnected)
                {
                    lock(ConnLocker)
                    {
                        _RedisConn = ConnectionMultiplexer.Connect(_RedisHost);
                    }
                }
                conn = _RedisConn;
            }
              
            return conn;
        }

        #endregion

        #region Set

       
        public RedisValue[] SetGetAll(string key)
        {
            ConnectionMultiplexer conn = GetUsingConn();
          
            try
            {
                if (conn.GetDatabase().SetLength(key) == 0)
                    return null;
                return conn.GetDatabase().SetMembers(key);
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
          
        }
        public List<T> SetGetAll<T>(string key)
        {
            ConnectionMultiplexer conn = GetUsingConn();
            List<T> result = new List<T>();
            try
            {
                _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
                var values = _RedisClient.SetMembers<T>(key);
                result =  values.ToList();
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return result;

        }

        public bool SetAdd(string key, RedisValue data) 
        {
            ConnectionMultiplexer conn = GetUsingConn();
            bool result = false;

            try
            {
                if (_Trans != null)
                {
                 //   RedisValue v = JsonConvert.SerializeObject(data);
                    _Trans.SetAddAsync(key, data);
                }
                else
                {
                    conn.GetDatabase().SetAdd(key, data);
                }

            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return result;
        }

        public bool SetAddT<T>(string key, T data) where T : class
        {
            ConnectionMultiplexer conn = GetUsingConn();
            bool result = false;

            try
            {
                if(_Trans!=null)
                {
                    RedisValue v = JsonConvert.SerializeObject(data);
                    _Trans.SetAddAsync(key, v);
                }
                else
                {
                    _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
                    
                    result = _RedisClient.SetAdd<T>(key, data);
                }
               
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return result;
        }

        public bool SetUpdate<T>(string key, T data) where T:class
        {
            ConnectionMultiplexer conn = GetUsingConn();
            bool result = false;

            try
            {
                if (_Trans != null)
                {
                    RedisValue v = JsonConvert.SerializeObject(data);
                    _Trans.SetRemoveAsync(key, v);
                    _Trans.SetAddAsync(key, v);
                }
                else
                {
                    _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
                     result = _RedisClient.SetRemove<T>(key, data);
                     result = _RedisClient.SetAdd<T>(key, data);
                }

            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return result;
        }

        public bool ContainsSetValue(RedisKey key, RedisValue v)
        {
            bool r = false;
            ConnectionMultiplexer conn = GetUsingConn();
            try
            {
               r = conn.GetDatabase().SetContains(key, v);
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return r;
        }

        public bool DeleteSet<T>(string key,T data) where T:class
        {
            bool r = false;
            ConnectionMultiplexer conn = GetUsingConn();
            try
            {
                _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
                r = _RedisClient.SetRemove<T>(key, data);
              
            }
            
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return r;
        }

        #endregion

        #region SoredSet


        public bool WriteSortedSet<T>(string key, T data, double score)
        {
            ConnectionMultiplexer conn = GetUsingConn();
            bool result = false;

            try
            {
                if (_Trans != null)
                {
                    RedisValue v = JsonConvert.SerializeObject(data);
                    _Trans.SortedSetAddAsync(key, v, score);

                }
                else
                {
                    _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());

                    result = _RedisClient.SortedSetAdd<T>(key, data, score);

                }

            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return result;
        }
       
        public List<T> FindSortedSet<T>(string key, double start = 0, double end = 100, Order order = Order.Ascending)
        {
            List<T> result = new List<T>();
            ConnectionMultiplexer conn = GetUsingConn();
            try
            {
              
              
                 _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
             
                 result = _RedisClient.SortedSetRangeByScore<T>(key, start, end, Exclude.None, order).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return result;
        }

        public List<T> GetSet<T>(string key)
        {
            List<T> result = new List<T>();
            ConnectionMultiplexer conn = GetUsingConn();
            try
            {
              
                _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
                result =  _RedisClient.SetMembers<T>(key).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return result;
        }

        public double? GetSortedSetScore(string key,string v)
        {
            double? result = 0;
            ConnectionMultiplexer conn = GetUsingConn();
            try
            {
                RedisValue member = JsonConvert.SerializeObject(v);

                result = conn.GetDatabase().SortedSetScore(key, member);
            }
            catch (Exception ex)
            {
                NLogHelper.GameError("【Redis】GetSortedSetScore:" + ex.Message);
                return -1;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="v"></param>
        /// <param name="num">在原有Score上做加减</param>
        /// <returns></returns>
        public double? AdjustScore(string key, RedisValue v,double num)
        {
            double? result = null;
            if (num == 0) return 0;
            ConnectionMultiplexer conn = GetUsingConn();
            try
            {
                RedisValue member = JsonConvert.SerializeObject(v);
                if (_Trans != null)
                {
                    _Trans.SortedSetIncrementAsync(key, member, num);
                }
                else
                {
                    result = conn.GetDatabase().SortedSetIncrement(key, member, num);
                }
                   
            }
            catch (Exception ex)
            {
                NLogHelper.GameError("【Redis】AdjustScore:"+ex.Message);
                return null;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return result;
        }

        #endregion

        #region Json

        public bool WriteJson<T>(string key,T data)
        {
            ConnectionMultiplexer conn = GetUsingConn();
            bool result = false;

            try
            {
                if (_Trans != null)
                {
                    RedisValue v = JsonConvert.SerializeObject(data);
                    _Trans.StringSetAsync(key, v);
                }
                else
                {
                    _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
                    
                    result = _RedisClient.Add<T>(key, data);
                }
               
            }
            catch(Exception ex)
            {
                NLogHelper.GameError("【Redis】WriteJson:" + ex.Message);
                return false;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return result;     
        }

        public T ReadJson<T>(string key) where T:class
        {
            ConnectionMultiplexer conn = GetUsingConn();
            T result = null;
            try
            {
               
               
                _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
                result = _RedisClient.Get<T>(key);
            }
            catch(Exception ex)
            {
                NLogHelper.GameError("【Redis】ReadJson:" + ex.Message);
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
           
            return result;
        }

        public bool DeleteJson<T>(string key)
        {
            ConnectionMultiplexer conn = GetUsingConn();
            bool result = true;
            try
            {


                _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
                result = _RedisClient.Remove(key);
            }
            catch(Exception ex)
            {
                NLogHelper.GameError("【Redis】DeleteJson:" + ex.Message);
                return false;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return result;
        }

        public bool ExistJson<T>(string key)
        {
            ConnectionMultiplexer conn = GetUsingConn();
            bool result = false;

            try
            {
                _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());

                result = _RedisClient.Exists(key);
            }
            catch (Exception ex)
            {
                NLogHelper.GameError("【Redis】ExistJson:" + ex.Message);
                return false;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return result;
        }
        #endregion

        #region Hash
        public bool KeyDelete(RedisKey key)
        {
            ConnectionMultiplexer conn = GetUsingConn();
            try
            {
        
                return conn.GetDatabase().KeyDelete(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        public bool HashExist(RedisKey key,RedisValue hashField)
        {
            ConnectionMultiplexer conn = GetUsingConn();
            try
            {

                return conn.GetDatabase().HashExists(key, hashField);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            
        }

        //public OutAPIResult HashUpdate(RedisKey key, RedisValue hashField, RedisValue value)
        //{
        //    ConnectionMultiplexer conn = GetUsingConn();
        //    OutAPIResult r = new OutAPIResult();
        //    try
        //    {
        //        if (_Trans != null)
        //        {
        //            _Trans.HashDeleteAsync(key, hashField);
        //            _Trans.HashSetAsync(key, hashField, value);
        //        }
        //        else
        //        {
        //            var db = conn.GetDatabase();
        //            r.IsSuccess = db.HashDelete(key, hashField);
        //            r.IsSuccess = db.HashSet(key, hashField, value);
        //        }
                    
        //    }
        //    catch (Exception ex)
        //    {
        //        r.ErrorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        if (_TransConn == null)
        //        {
        //            conn.Close();
        //            conn.Dispose();
        //        }
        //    }
        //    return r;
        //}

        //public OutAPIResult HashUpdate<T>(RedisKey hashKey, RedisValue hashField, T value)
        //{
        //    ConnectionMultiplexer conn = GetUsingConn();
        //    OutAPIResult r = new OutAPIResult();
        //    try
        //    {
        //        if (_Trans != null)
        //        {

        //            RedisValue v = JsonConvert.SerializeObject(value);
        //            _Trans.HashDeleteAsync(hashKey, hashField);
        //            _Trans.HashSetAsync(hashKey, hashField, v);
        //        }
        //        else
        //        {
        //            _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
        //            _RedisClient.HashDelete(hashKey, hashField);
        //            _RedisClient.HashSet<T>(hashKey, hashField, value);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        r.ErrorMsg = ex.Message;
        //    }
        //    finally
        //    {
        //        if (_TransConn == null)
        //        {
        //            conn.Close();
        //            conn.Dispose();
        //        }
        //    }
        //    return r;
        //}

        

        public RedisValue[] HashValues(RedisKey key)
        {
            ConnectionMultiplexer conn = GetUsingConn();
            try
            {

                return conn.GetDatabase().HashValues(key);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public T HashGet<T>(RedisKey key, RedisValue hashField)
        {
            ConnectionMultiplexer conn = GetUsingConn();
            try
            {
                _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
                return _RedisClient.HashGet<T>(key, hashField);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        public RedisValue HashGet(RedisKey key, RedisValue hashField)
        {
            ConnectionMultiplexer conn = GetUsingConn();
            try
            {
              //  _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
              //return   _RedisClient.HashGet<string>(key, hashField);
                return conn.GetDatabase().HashGet(key, hashField);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public OutAPIResult HashAdd(RedisKey key, RedisValue hashField, RedisValue value)
        {
            ConnectionMultiplexer conn = GetUsingConn();
            OutAPIResult r = new OutAPIResult();
            try
            {
                if (_Trans != null)
                {

                    _Trans.HashSetAsync(key, hashField, value);
                }
                else
                {
                    var db = conn.GetDatabase();
                    var v = db.HashGet(key, hashField);
                    if (v == value)
                        r.IsSuccess = true;
                    else
                        r.IsSuccess = db.HashSet(key, hashField, value);
                }
                    

            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return r;
        }

        public OutAPIResult HashAddT<T>(RedisKey hashKey, RedisValue hashField, T value)
        {
           
            ConnectionMultiplexer conn = GetUsingConn();
            OutAPIResult r = new OutAPIResult();
            try
            {
                if (_Trans != null)
                {

                    RedisValue v = JsonConvert.SerializeObject(value);
                    _Trans.HashSetAsync(hashKey, hashField, v);
                }
                else
                {
                    _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
                    _RedisClient.HashSet<T>(hashKey, hashField, value);
                }

            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return r;
        }

        public OutAPIResult HashDelete(RedisKey hashKey, RedisValue hashField)
        {
            ConnectionMultiplexer conn = GetUsingConn();
            OutAPIResult r = new OutAPIResult();
            try
            {
                if (_Trans != null)
                {
                    _Trans.HashDeleteAsync(hashKey, hashField);
                    
                }
                else
                {
                    _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
                    _RedisClient.HashDelete(hashKey, hashField);
                }

            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return r;
        }

        public NResult<T> HashFindAllValue<T>(RedisKey hashKey)
        {
            ConnectionMultiplexer conn = GetUsingConn();
            NResult<T> r = new NResult<T>();
            try
            {
              
                 _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
                r.resultList = _RedisClient.HashGetAll<T>(hashKey).Values.ToList();
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return r;
        }

        public NResult<T> HashFindAll<T>(RedisKey hashKey)
        {
            ConnectionMultiplexer conn = GetUsingConn();
            NResult<T> r = new NResult<T>();
            try
            {
               
                _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
                r.resultDict = _RedisClient.HashGetAll<T>(hashKey);
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }
            finally
            {
                if (_TransConn == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return r;
        }
        #endregion
    }
}
