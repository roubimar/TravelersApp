using System;

namespace FreeTravelersApp.DataClass
{
    public class ConnectionInfo
    {
        public ConnectionInfo(int id)
        {
            UserId = id;
            ConnectionToken = Guid.NewGuid();
        }

        public Guid ConnectionToken { get; set; }
        public int UserId { get; set; }

    }
}