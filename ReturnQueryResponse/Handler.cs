using System;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Amazon.Runtime;
using Amazon.Util;
using Amazon.Runtime.Internal.Transform;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace AwsDotnetCsharp
{
    public class Handler
    {
        public async System.Threading.Tasks.Task<List<ResponseModel>> HelloAsync(Request request)
        {
            var DBClient = new AmazonDynamoDBClient();

            var result = new QueryRequest
            {
                TableName = "qa-device-data",
                KeyConditionExpression = "dserial = :v_DeviceSerial",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
             {":v_DeviceSerial",new AttributeValue{S=request.DeviceSerial.ToString()}}
             },
                ProjectionExpression = "dserial, dtype,con",
                ConsistentRead = true
            };
            var Response = await DBClient.QueryAsync(result);
            Console.WriteLine(Response);
            var getResult = new ResponseModel();
        var Deviceinfo = new List<ResponseModel>();
            foreach (Dictionary<string, AttributeValue> item in Response.Items)
            {
                foreach (KeyValuePair<string, AttributeValue> key in item)
                {
                   
                    string attributeName = key.Key;
                    AttributeValue value = key.Value;
                    getResult.dserial = value.S;
                    getResult.con = value.N;
                    getResult.dtype = value.N;
                    Deviceinfo.Add(getResult);
                    //Console.WriteLine(
                    //    attributeName + " " +
                    //    (value.S == null ? "" : "S=[" + value.S + "]") +
                    //    (value.N == null ? "" : "N=[" + value.N + "]") +
                    //    (value.SS == null ? "" : "SS=[" + string.Join(",", value.SS.ToArray()) + "]") +
                    //    (value.NS == null ? "" : "NS=[" + string.Join(",", value.NS.ToArray()) + "]")
                    //);
                }
            }
                Console.WriteLine("************************************************");
                /*  var ListofMsgs = new List<Response>();
                      Dictionary<string, AttributeValue> list = new Dictionary<string, AttributeValue>();
                  foreach (Dictionary<string,AttributeValue> item in Response.Items)
                  {
                      list.Add(item,value               
                  }

                   return new Response("Go Serverless v1.0! Your function executed successfully!", request.DeviceSerial);*/
                return Deviceinfo;

        }
    }

    public class ResponseModel
        {
        public string dserial { get; set; }
        public string dtype { get; set; }
        public string con { get; set; }

       /* public Response(string Dserial, string Dtype)
        {
            dserial = Dserial;
            dtype = Dtype;
            //con = Con;

        }*/
    }

    public class Request
    {
        public string DeviceSerial { get; set; }
        public string since { get; set; }
        public string Key3 { get; set; }
    }
}
