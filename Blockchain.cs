using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain
{
    class Blockchain
    {
        private readonly SHA256 sha2;
        private readonly List<Block> chain;
        private List<Transaction> currentTransactions;

        public IEnumerable<Block> Chain { get { return chain.AsReadOnly(); } }
        public Block LastBlock { get { return chain[chain.Count-1]; } }

        public Blockchain()
        {
            sha2 = SHA256.Create();
            currentTransactions = new List<Transaction>();
            chain = new List<Block>();

            //genesis block
            NewBlock(1, "1");
        }

        public Block NewBlock(long proof, string prevHash=null)
        {
            var block = new Block
            {
                Index = chain.Count + 1,
                Timestamp = DateTime.Now,
                Transactions = currentTransactions.ToArray(),
                Proof = proof,
                PrevHash = prevHash ?? Hash(LastBlock),
            };

            currentTransactions.Clear();
            chain.Add(block);

            return block;
        }

        public int NewTransaction(Transaction transaction)
        {
            currentTransactions.Add(transaction);

            return LastBlock.Index + 1;
        }

        public long ProofOfWork(long lastProof)
        {
            long proof = 0;
            while(IsProofValid(lastProof, proof) == false)
                proof++;
            return proof;
        }

        private bool IsProofValid(long lastProof, long proof)
        {
            string guess = $"{lastProof}-{proof}";
            string hash = Hash(guess);
            return hash.EndsWith("0000");
        }

        public bool IsChainValid()
        {
            Block lastBlock = chain[0];
            int index = 1;

            while(index < chain.Count)
            {
                Block block = chain[index];

                if(block.PrevHash != Hash(lastBlock))
                    return false;
                if(IsProofValid(lastBlock.Proof, block.Proof) == false)
                    return false;
                
                lastBlock = block;
                index++;
            }

            return true;
        }

        private string Hash(Block block)
        {
            var json = JsonConvert.SerializeObject(block);

            return Hash(json);
        }

        private string Hash(string value)
        {
            return BitConverter
                .ToString(sha2.ComputeHash(Encoding.UTF8.GetBytes(value)))
                .Replace("-", "");
        }
    }
}