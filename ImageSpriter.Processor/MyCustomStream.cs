using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageSpriter.Processor
{
    public class MyCustomStream : Stream
    {
        private readonly Stream _filter;

        public MyCustomStream(Stream filter)
        {
            this._filter = filter;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var allScripts = new StringBuilder();
            string wholeHtmlDocument = Encoding.UTF8.GetString(buffer, offset, count);
            var regex = new Regex(@"<script[^>]*>(?<script>([^<]|<[^/])*)</script>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //Remove all Script Tag
            wholeHtmlDocument = regex.Replace(wholeHtmlDocument, m => { allScripts.Append(m.Groups["script"].Value); return "<!-- Removed Script -->"; });

            //Put all Script at the end
            if (allScripts.Length > 0)
            {
                wholeHtmlDocument = wholeHtmlDocument.Replace("</html>", "<script type='text/javascript'>" + allScripts.ToString() + "</script></html>");
            }
            buffer = Encoding.UTF8.GetBytes(wholeHtmlDocument);
            this._filter.Write(buffer, 0, buffer.Length);
        }

        public override void Flush()
        {
            this._filter.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this._filter.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this._filter.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this._filter.Read(buffer, offset, count);
        }

        public override bool CanRead
        {
            get { return this._filter.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this._filter.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this._filter.CanWrite; }
        }

        public override long Length
        {
            get { return this._filter.Length; }
        }

        public override long Position
        {
            get { return this._filter.Position; }
            set { this._filter.Position = value; }
        }
    }
}
