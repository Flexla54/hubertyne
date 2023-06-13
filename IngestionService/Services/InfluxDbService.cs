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

        private readonly InfluxDBClient _client;
        private readonly WriteApi _writeApi;
        private readonly QueryApi _queryApi;

        public InfluxDbService(string token, string host)
        {
            _token = token;
            _host = host;

            _client = new InfluxDBClient(host, token);
            _writeApi = _client.GetWriteApi();
            _queryApi = _client.GetQueryApi();
        }

        public void Write(Action<WriteApi> action)
        {
            action(_writeApi);
        }

        public async Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
        {
            return await action(_queryApi);
        }
    }
}
