using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace Lion.Server.Hypermedia
{
    public abstract class CursorQuery
    {
        [FromQuery(Name = "cursor")]
        public string EncodedCursor { get; set; }

        public long DecodedCursor
        {
            get
            {
                if (string.IsNullOrEmpty(EncodedCursor))
                {
                    return 0;
                }

                var buffer = new Span<byte>(new byte[EncodedCursor.Length]);
                if (Convert.TryFromBase64String(EncodedCursor, buffer, out int _) == false)
                {
                    return 0;
                }

                var decoded = Encoding.ASCII.GetString(buffer);

                // entity|cursor
                return long.TryParse(decoded.Split('|')[1], out long cursor) ? cursor : 0;
            }
        }

        [FromQuery(Name = "limit")]
        public abstract int Limit { get; set; }
    }
}
