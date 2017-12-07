using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class JsonUtils
    {

    }

    /*
     * 1) Выстрел
     * 2) Пауза / Конец игры
     * 3) Переход хода
     * 4)
     */

    [DataContract]
    public class Request
    {
        [DataMember]
        public string Header { get; set; }

        [DataMember]
        public string Req { get; set; }
    }
}
