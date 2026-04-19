using Mundialito.Domain.Common;
using Mundialito.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Domain.Entities
{
    public class Player : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public PositionPlayer Position { get; private set; }
        public Guid TeamId { get; private set; }
        public Team? Team { get; private set; }

        private Player() { }

        public Player(string name, PositionPlayer position, Guid teamId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Position = position;
            TeamId = teamId;
        }
    }
}
