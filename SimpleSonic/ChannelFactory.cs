using System;

namespace SimpleSonic
{
    public class ChannelFactory
    {
        private String address;

        private int port;

        private String password;

        private int connectionTimeout;

        private int readTimeout;


        public ChannelFactory(String address, int port, String password,
                 int connectionTimeout, int readTimeout)
        {
            this.address = address;
            this.port = port;
            this.password = password;
            this.connectionTimeout = connectionTimeout;
            this.readTimeout = readTimeout;
        }

        public IngestChannel newIngestChannel()
        {
            return new IngestChannel(
                this.address,
                this.port,
                this.password,
                this.connectionTimeout,
                this.readTimeout
            );
        }

        public SearchChannel newSearchChannel()
        {
            return new SearchChannel(
                this.address,
                this.port,
                this.password,
                this.connectionTimeout,
                this.readTimeout
            );

        }



        public ControlChannel newControlChannel()
        {
            return new ControlChannel(
                    this.address,
                    this.port,
                    this.password,
                    this.connectionTimeout,
                    this.readTimeout
            );
        }

    }
}

