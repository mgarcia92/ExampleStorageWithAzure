using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tableConsole
{
    class Contact: TableEntity
    {
        public Contact(string firstName, string lastName)
        {
            RowKey = firstName;
            PartitionKey = lastName;
        }

        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
