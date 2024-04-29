using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Persistence.Model;
using Amazon.DynamoDBv2.DocumentModel;

namespace Persistence.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IAmazonDynamoDB _dynamoDB;
        private readonly string _tableName = "employees";//Table name we created in Dynamo DB

        public EmployeeRepository(IAmazonDynamoDB dynamoDB)
        {
            _dynamoDB = dynamoDB;
        }

        public async Task<bool> CreateAsync(Employee employee)
        {
            employee.UpdatedAt = DateTime.UtcNow;
            var employeeAsJson = JsonSerializer.Serialize(employee);
            var employeeAsAttributes = Document.FromJson(employeeAsJson).ToAttributeMap();

            var createItemRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = employeeAsAttributes
            };

            var response = await _dynamoDB.PutItemAsync(createItemRequest);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<Employee?> GetAsync(Guid id)
        {
            var getItemRequest = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue { S = id.ToString() } },
                { "SK", new AttributeValue { S = id.ToString() } }
            }
            };

            var response = await _dynamoDB.GetItemAsync(getItemRequest);

            if (response.Item.Count == 0)
            {
                return null;
            }

            var itemAsDocument = Document.FromAttributeMap(response.Item);

            return JsonSerializer.Deserialize<Employee>(itemAsDocument.ToJson());
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var scanRequest = new ScanRequest
            {
                TableName = _tableName,
            };

            var response = await _dynamoDB.ScanAsync(scanRequest);

            return response.Items.Select(x =>
            {
                var json = Document.FromAttributeMap(x).ToJson();
                return JsonSerializer.Deserialize<Employee>(json);
            });
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            employee.UpdatedAt = DateTime.UtcNow;
            var employeeAsJson = JsonSerializer.Serialize(employee);
            var employeeAsAttributes = Document.FromJson(employeeAsJson).ToAttributeMap();

            var updateItemRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = employeeAsAttributes
            };

            var response = await _dynamoDB.PutItemAsync(updateItemRequest);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var deleteItemRequest = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
            {
                { "PK", new AttributeValue { S = id.ToString() } },
                { "SK", new AttributeValue { S = id.ToString() } }
            }
            };

            var response = await _dynamoDB.DeleteItemAsync(deleteItemRequest);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}
