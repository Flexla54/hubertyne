using InfluxDB.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngestionService.Services
{
    public class InfluxDbService
    {
        private readonly string _token;
        private readonly string _host;

        public InfluxDbService(string token, string host)
        {
            _token = token;
            _host = host;
        }

        public void Write(Action<WriteApiAsync> action)
        {
            using var client = new InfluxDBClient(_host, _token);
            var write = client.GetWriteApiAsync();
            action(write);
        }

        public async Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
        {
            using var client = new InfluxDBClient(_host, _token);
            var query = client.GetQueryApi();
            return await action(query);
        }
    }
}
