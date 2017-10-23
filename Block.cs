using System;
using System.Collections.Generic;
using System.Linq;

namespace Blockchain
{
    class Block
    {
        public int Index{get;set;}
        public DateTime Timestamp{get;set;}
        public IEnumerable<Transaction> Transactions{get;set;}
        public long Proof{get;set;}
        public string PrevHash{get;set;}

        public override string ToString()
        {
            return $"Index={Index}, Transactions={Transactions.Count()}, Proof={Proof}, PrevHash={PrevHash}";
        }
    }
}