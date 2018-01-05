using System;
using System.Configuration;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Protobuf;
using ProtoBuf;

namespace dotnet_redis
{
    [ProtoContract]
    struct Person
    {
        [ProtoMember(1)]
        string Name {get;set;}
        [ProtoMember(2)]
        public int Age {get;set;}
        [ProtoMember(3)]
        private bool _isMale {get;set;}

        public Person(string name, int age, bool isMale)
        {
            Name = name;
            Age = age;
            _isMale = isMale;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello John!");
            var redis = ConnectionMultiplexer.Connect("localhost");
            ReadProto(redis);
        }

        private static void ReadProto(ConnectionMultiplexer redis)    
        {
            var serializer = new ProtobufSerializer();
            var cacheClient = new StackExchangeRedisCacheClient(serializer, "localhost");
            
            var person = cacheClient.Get<Person>("proto");
            Console.Write($"Person: [{person.Age}]");
        }

        private static void WriteProto(ConnectionMultiplexer redis)    
        {
            var serializer = new ProtobufSerializer();
            var cacheClient = new StackExchangeRedisCacheClient(serializer, "localhost");
            var obj = new Person("john", 35, true);
            cacheClient.Add("proto", obj);
        }

        private static void TestPlain(ConnectionMultiplexer redis)
        {
            var db = redis.GetDatabase();
            var value = "john";
            db.StringSet("key", value);

            Console.WriteLine($"Value from redis: {db.StringGet("a")}");
        }
    }
}
