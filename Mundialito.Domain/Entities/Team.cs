using Mundialito.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Domain.Entities
{
    public class Team : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;

        private Team() { }

        public Team(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}
