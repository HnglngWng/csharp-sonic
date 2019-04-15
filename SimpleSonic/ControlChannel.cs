using System;

namespace SimpleSonic
{
    public class ControlChannel : Channel
    {
        public ControlChannel(String address, int port, String password, int connectionTimeout, int readTimeout)
            : base(address, port, password, connectionTimeout, readTimeout)
        {
            this.Start(Mode.Control);
        }

        public void Consolidate()
        {
            this.Send("TRIGGER consolidate");
            this.AssertOK();
        }
    }
}