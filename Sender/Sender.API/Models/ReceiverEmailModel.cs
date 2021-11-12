using System;
using Db.Common.Models;

namespace Sender.API.Models
{
    public sealed class ReceiverEmailModel : IDbCollectionModel
    {
        public Guid Guid { get; set; }

        public Guid GroupGuid { get; set; }

        public string Email { get; set; }
    }
}
