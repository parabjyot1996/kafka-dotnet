namespace KafkaProducer
{
    using System;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    
    public class ProducerWrapper
    {
        private string _topicName;
        private ProducerConfig _config;
        private static readonly Random rand = new Random();

        public ProducerWrapper(ProducerConfig config, string topicName)
        {
            _config = config;
            _topicName = topicName;
        }

        public async Task WriteMessage(string message){
            var producer = new ProducerBuilder<Null,string>(this._config).Build();

            try
            {
                var dr = await producer.ProduceAsync(this._topicName, new Message<Null, string>
                            {
                                //Key = rand.Next(5).ToString(),
                                Value = message,
                            });

                Console.WriteLine($"KAFKA => Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
            
            return;
        }
    }
}