using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invoiceServerApp
{
    interface CallbackStatusInterface
    {
        void NotificationChanged(int status);
        int GetStatusCupom();
    }
}
