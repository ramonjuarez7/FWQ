using System;
using System.Threading;
using Confluent.Kafka;

namespace FWQ_Sensor
{
    class Program
    {
        static void Main(string[] args)
        {
            string ipBroker;
            string puertoBroker;
            string idAtraccion;

            if (args.Length == 4)
            {
                ipBroker = args[1];
                puertoBroker = args[2];
                idAtraccion = args[3];


                var config = new ProducerConfig
                {
                    BootstrapServers = ipBroker + ":" + puertoBroker,
                    SecurityProtocol = SecurityProtocol.SaslPlaintext,
                    SaslMechanism = SaslMechanism.ScramSha256,
                    SaslUsername = "root",
                    SaslPassword = "root"
                };


                var rand = new Random();
                int a = Int32.Parse(idAtraccion);
                int nVisitantes = rand.Next(a, (a * 10) / 3);
                int counter = 0;
                while (true)
                {
                    if(counter % 10 == 0 && nVisitantes > 0)
                    {
                        nVisitantes--;
                    }
                    String enviar = idAtraccion + ":" + nVisitantes + ":";
                    using (var producer = new ProducerBuilder<Null, string>(config).Build())
                    {
                        var dr = producer.ProduceAsync("sensores", new Message<Null, string> { Value = enviar }).Result;
                        Console.WriteLine($"Delivered '{dr.Value}' to: {dr.TopicPartitionOffset}");
                    }
                    rand = new Random();
                    Thread.Sleep(rand.Next(1,4) * 1000);
                    counter++;
                }
                

            } else
            {
                Console.WriteLine("Los parámetros introducidos deben ser 4.");
            }
        }
    }
}
