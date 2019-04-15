using System;

namespace SimpleSonic
{
    public class IngestChannel : Channel
    {

        public IngestChannel(String address, int port, String password, int connectionTimeout, int readTimeout)
            : base(address, port, password, connectionTimeout, readTimeout)
        {
            this.Start(Mode.Ingest);
        }



        public void Push(String collection, String bucket, String obj, String text)
        {
            this.Send(String.Format(
                    "PUSH %s %s %s \"%s\"",
                    collection,
                    bucket,
                    obj,
                    text
            ));
            this.AssertOK();
        }



        public void Pop(String collection, String bucket, String obj, String text)
        {
            this.Send(String.Format(
                    "POP %s %s %s \"%s\"",
                    collection,
                    bucket,
                    obj,
                    text
            ));
            this.AssertOK();
        }



        public int Count(String collection, String bucket, String obj)
        {
            if (bucket == null && obj != null)
            {
                throw new ArgumentException("bucket is required for counting an object");
            }

            this.Send(String.Format(
                    "COUNT %s%s%s",
                    collection,
                    bucket == null ? "" : " " + bucket,
                    obj == null ? "" : " " + obj
            ));
            return this.AssertResult();
        }



        public int Count(String collection, String bucket)
        {
            return this.Count(collection, bucket, null);
        }


        public int Count(String collection)
        {
            return this.Count(collection, null);
        }


        public int Flushc(String collection)
        {
            this.Send(String.Format("FLUSHC %s", collection));
            return this.AssertResult();
        }


        public int Flushb(String collection, String bucket)
        {
            this.Send(String.Format("FLUSHB %s %s", collection, bucket));
            return this.AssertResult();
        }

        public int Flusho(String collection, String bucket, String obj)
        {
            this.Send(String.Format("FLUSHO %s %s %s", collection, bucket, obj));
            return this.AssertResult();
        }

    }
}