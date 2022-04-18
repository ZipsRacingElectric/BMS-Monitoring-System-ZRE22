using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMSMS.Constants;

namespace BMSMS.CAN
{
    public class CANMessage
    {
        public byte[] data = new byte[8];
        public int id;
        public int dlc;
        public int flags;
        public long timestamp;
        public long deltaTime;

        public CANMessage() { }
    }
}
