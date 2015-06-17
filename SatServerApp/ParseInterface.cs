using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invoiceServerApp
{
    interface ParseInterface
    {
        string messageParse(string msg);
        string messageInutilizacao(string msg);
        string ReImpressaoTEF(string msg);
        string ReImpressaoDanfe(string msg);
        string getCodigo(string msg);
        string getQrCode(string msg);
        string messageCancel(string msg);
        string DesbloqueioSat(string msg);
    }
}
