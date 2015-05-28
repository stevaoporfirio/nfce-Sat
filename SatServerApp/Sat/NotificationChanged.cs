using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invoiceServerApp
{
    public interface NotificationChanged
    {
        string NotificationChangedClient(string _id, string _receive);
    }
}
