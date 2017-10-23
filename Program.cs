using System;
using Newtonsoft.Json;

namespace Blockchain
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            
            Blockchain chain = new Blockchain();

            AddTransaction(chain);
            MineBlock(chain);
            AddTransaction(chain);
            AddTransaction(chain);
            AddTransaction(chain);
            MineBlock(chain);           
            PrintChain(chain);
        }

        static void AddTransaction(Blockchain chain)
        {
            int ti = chain.NewTransaction(new Transaction{ Sender = "s1", Recipient = "r1", Amount = 1.5f });
            
            Console.WriteLine($"Transaction added to block {ti}");
        }

        static void MineBlock(Blockchain chain)
        {
            Console.WriteLine("Mining new block...");
            long lastProof = chain.LastBlock.Proof;
            long proof = chain.ProofOfWork(lastProof);
            Block newBlock = chain.NewBlock(proof);
            Console.WriteLine($"New block forged: {newBlock}");
        }

        static void PrintChain(Blockchain chain)
        {
            var json = JsonConvert.SerializeObject(chain.Chain, Formatting.Indented);

            Console.WriteLine(json);
            Console.WriteLine("IsChainValid: " +chain.IsChainValid());
        }
    }
}
